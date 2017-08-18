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
            UploadFile();
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

            var invoiceHeader = await _context.InvoiceHeader.SingleOrDefaultAsync(m => m.Id == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }
            return View(invoiceHeader);
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

        [HttpPost, ActionName("Upload")]
        // [ValidateAntiForgeryToken]
        public IActionResult UploadFile()
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath + "\\" + "PTANDI.csv");
            ConvertCSVtoList(path);
            return View();
        }

        private List<InvoiceDetail> ConvertCSVtoList(string path)
        {
            var invoices = new List<InvoiceDetail>();

            String[] values = System.IO.File.ReadAllLines(path);
            var valuesWithoutHeader = values.Skip(1);

            if(valuesWithoutHeader != null)
            {
                foreach (var value in valuesWithoutHeader)
                {
                    var line = value.Split(';');

                    if (line.Length > 0)
                    {
                        var invoice = new InvoiceDetail()
                        {
                            CodeProvider = line[0],
                            Name = line[1],
                            MeasurementUnit = line[2],
                            VAT = Decimal.Parse(line[3]),
                            AdditionalInfo = line[4],
                            Quantity = Decimal.Parse(line[5]),
                            Price = Decimal.Parse(line[6]),
                            VatPercentage = Decimal.Parse(line[7]),
                            BarCode = line[8]
                        };

                        invoices.Add(invoice);
                    }
                }
            }
         
            return invoices;
        }
    }
}
