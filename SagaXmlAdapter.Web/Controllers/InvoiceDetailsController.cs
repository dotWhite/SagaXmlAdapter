using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SagaXmlAdapter.Web.Data;
using SagaXmlAdapter.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace SagaXmlAdapter.Web.Controllers
{
    public class InvoiceDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public InvoiceDetailsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: InvoiceDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.InvoiceDetail.ToListAsync());
        } 

        // GET: InvoiceDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InvoiceDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Name,CodeProvider,CodeClient,BarCode,AdditionalInfo,MeasurementUnit,Quantity,Price,Value,VatPercentage,VAT,Observations")] InvoiceDetail invoiceDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = new InvoiceDetail();

            model = await _context.InvoiceDetail.SingleOrDefaultAsync(m => m.Id == id);

            var getProviders = await _context.Provider
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Id = x.Id,
                    Value = x.Name
                }).ToListAsync();

            var getClients = await _context.Client
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Id = x.Id,
                    Value = x.Name
                }).ToListAsync();

            model.ProviderList = new SelectList(getProviders, "Id", "Value");
            model.ClientList = new SelectList(getClients, "Id", "Value");

            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: InvoiceDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Name,CodeProvider,CodeClient,BarCode,AdditionalInfo,MeasurementUnit,Quantity,Price,Value,VatPercentage,VAT,Observations")] InvoiceDetail invoiceDetail)
        {
            if (id != invoiceDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceDetail = await _context.InvoiceDetail.SingleOrDefaultAsync(m => m.Id == id);
            _context.InvoiceDetail.Remove(invoiceDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InvoiceDetailExists(int id)
        {
            return _context.InvoiceDetail.Any(e => e.Id == id);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, [Bind("Id,Number,Name,CodeProvider,CodeClient,BarCode,AdditionalInfo,MeasurementUnit,Quantity,Price,Value,VatPercentage,VAT,Observations")] InvoiceDetail invoiceDetail)
        {
            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads" + "\\");

            //var invoiceDetail = new InvoiceDetail();
            //var invoiceDetail = _context.InvoiceDetail.SingleOrDefaultAsync(x => x.Id == id);

            var fileDetail = new FileDetail();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

                    using (var inputStream = new StreamReader(formFile.OpenReadStream()))
                    {
                        var items = await inputStream.ReadToEndAsync();
                       // foreach(var fileDetail in fileDetails)
                       // {
                            fileDetail.FileName = fileName;
                            fileDetail.FileType = formFile.ContentType;
                            fileDetail.Length = Convert.ToInt32(formFile.Length);
                            fileDetail.Content = ConvertCSVtoList(items);
                       // }
                    }
                }

                invoiceDetail.FileDetail = fileDetail;
            }   

            return View(invoiceDetail);
        }

        private List<InvoiceDetail> ConvertCSVtoList(string stream)
        {
            var invoices = new List<InvoiceDetail>();

            var line = stream.Split('\n');
            var valuesWithoutHeader = line.Skip(1);
            foreach (var item in valuesWithoutHeader)
            {
                var splittedString = item.Split(';');

                var invoice = new InvoiceDetail()
                {
                    CodeProvider = splittedString[0],
                    Name = splittedString[1],
                    MeasurementUnit = splittedString[2],
                    VAT = Decimal.Parse(splittedString[3]),
                    AdditionalInfo = splittedString[4],
                    Quantity = Decimal.Parse(splittedString[5]),
                    Price = Decimal.Parse(splittedString[6]),
                    VatPercentage = Decimal.Parse(splittedString[7]),
                    BarCode = splittedString[8]
                };

                invoices.Add(invoice);

            }
            return invoices;
        }
    }
}
