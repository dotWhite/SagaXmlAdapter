using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SagaXmlAdapter.Web.Data;
using SagaXmlAdapter.Web.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Net.Http.Headers;

namespace SagaXmlAdapter.Web.Controllers
{
    public class InvoiceHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public InvoiceHeadersController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: InvoiceHeaders
        public async Task<IActionResult> Index()
        {
            return View(await _context.InvoiceHeader.ToListAsync());
        }

        // GET: InvoiceHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeader
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InvoiceHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod,CsvFile")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.InvoiceHeader.SingleOrDefaultAsync(m => m.Id == id);
            
            model.Details = await _context.InvoiceDetail.ToListAsync();

            if (model != null && model.Details != null)
            {
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

                foreach (var item in model.Details)
                {
                    item.ProviderList = new SelectList(getProviders, "Id", "Value");
                    item.ClientList = new SelectList(getClients, "Id", "Value");
                }
            } else if(model == null)
            {
                return NotFound();
            }
        
            return View(model);
        }

        // POST: InvoiceHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod")] InvoiceHeader invoiceHeader)
        {
            if (id != invoiceHeader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceHeaderExists(invoiceHeader.Id))
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
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeader
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoiceHeader = await _context.InvoiceHeader.SingleOrDefaultAsync(m => m.Id == id);
            _context.InvoiceHeader.Remove(invoiceHeader);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InvoiceHeaderExists(int id)
        {
            return _context.InvoiceHeader.Any(e => e.Id == id);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, [Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod")] InvoiceHeader invoiceHeader)
        {
            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads" + "\\");

            var fileDetail = new FileDetail();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

                    using (var inputStream = new StreamReader(formFile.OpenReadStream()))
                    {
                        var items = await inputStream.ReadToEndAsync();

                        fileDetail.FileName = fileName;
                        fileDetail.FileType = formFile.ContentType;
                        fileDetail.Length = Convert.ToInt32(formFile.Length);
                        fileDetail.Content = ConvertCSVtoList(items);
                    }
                }

                _context.FileDetail.Add(fileDetail);
                await _context.SaveChangesAsync();
            }
            return View(invoiceHeader.Details);
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
