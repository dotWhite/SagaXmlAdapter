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
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var upload = Path.Combine(_hostingEnvironment.WebRootPath + file.FileName);
            if(file != null && file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(upload, file.FileName), FileMode.Open, FileAccess.Read))
                {
                    if (file.Name.EndsWith(".csv"))
                    {
                        var code = new List<String>();
                        var name = new List<String>();
                        var measurement = new List<String>();
                        var tva = new List<String>();
                        var den_tip = new List<String>();
                        var stoc = new List<String>();
                        var pretAchizitie = new List<String>();
                        var pretVanzare = new List<String>();
                        var codBare = new List<String>();

                        //using (var fileStream = new FileStream(Path.Combine(upload, file.FileName), FileMode.Open, FileAccess.Read))
                        //{
                        using (var reader = new StreamReader(fileStream))
                        {
                            //read from file
                            StreamReader streamReader = new StreamReader(fileStream);
                            string line = streamReader.ReadLine();

                            var fields = line.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                            code.Add(fields[0]);

                            if (fields.Length > 1)
                                name.Add(fields[1]);
                            if (fields.Length > 2)
                                measurement.Add(fields[2]);
                            if (fields.Length > 3)
                                tva.Add(fields[3]);
                            if (fields.Length > 4)
                                den_tip.Add(fields[4]);
                            if (fields.Length > 5)
                                stoc.Add(fields[5]);
                            if (fields.Length > 6)
                                den_tip.Add(fields[6]);
                            if (fields.Length > 7)
                                pretAchizitie.Add(fields[7]);
                            if (fields.Length > 8)
                                pretVanzare.Add(fields[8]);
                            if (fields.Length > 9)
                                codBare.Add(fields[9]);
                        }
                    }
                    // Saves the uploaded file to the specified location. Replace for "SaveAsAsync"
                    await file.CopyToAsync(fileStream);
                }
            }  
            return View();
        }
    }
}
