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
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

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
            }
            else if (model == null)
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

        public async Task<IActionResult> ExportToXML()
        {
            var processXML = ProcessListToXML();

            if(processXML != null)
            {

            }

            return View();

        }

        private string ProcessListToXML()
        {
            var xml = new XDocument();

            var invoiceHeaderList = _context.InvoiceHeader.ToList();
            foreach(var invoiceHeader in invoiceHeaderList)
            {
                invoiceHeader.Details = _context.InvoiceDetail.ToList();
                var clientList = _context.Client.ToList();
                foreach(var client in clientList)
                {
                    invoiceHeader.Client = client;
                }
                var providerList = _context.Provider.ToList();
                foreach(var provider in providerList)
                {
                    invoiceHeader.Provider = provider;

                    xml = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
                        new XElement("InviceHeaders",
                           new XElement("InvoiceHeader",
                               new XElement("Header",
                               new XElement("CompanyName", invoiceHeader.Provider.Name),
                               new XElement("CompanyCIF", invoiceHeader.Provider.CIF),
                               new XElement("CompanyRegNumber", invoiceHeader.Provider.RegNumber),
                               new XElement("CompanyCapital", invoiceHeader.Provider.Capital),
                               new XElement("CompanyAddress", invoiceHeader.Provider.Address),
                               new XElement("CompanyBank", invoiceHeader.Provider.Bank),
                               new XElement("CompanyIBAN", invoiceHeader.Provider.IBAN),
                               new XElement("CompanyAdditionalInfo", invoiceHeader.Provider.Description),

                               new XElement("ClientBank", invoiceHeader.Client.Name),
                               new XElement("ClientDescription", invoiceHeader.Client.Description),
                               new XElement("ClientCIF", invoiceHeader.Client.CIF),
                               new XElement("ClientRegNumber", invoiceHeader.Client.RegNumber),
                               new XElement("ClientAddress", invoiceHeader.Client.Address),
                               new XElement("ClientBank", invoiceHeader.Client.Bank),
                               new XElement("ClientIBAN", invoiceHeader.Client.IBAN),

                               new XElement("InvoiceNumber", invoiceHeader.Number),
                               new XElement("InvoiceIssueDate", invoiceHeader.IssueDate),
                               new XElement("InvoiceDueDate", invoiceHeader.DueDate),
                               new XElement("InvoiceInversTaxing", invoiceHeader.InversTaxing),
                               new XElement("InvoiceVatCollecting", invoiceHeader.VatCollecting),
                               new XElement("InvoiceDescription", invoiceHeader.Description),
                               new XElement("InvoiceCurrency", invoiceHeader.Currecy),
                               new XElement("InvoiceVAT", invoiceHeader.VAT),
                               new XElement("InvoiceWeight", invoiceHeader.Weight)),

                           new XElement("InvoiceDetails",
                               from detail in invoiceHeader.Details
                               select new XElement("InvoiceContent",
                                      new XElement("No", detail.Number),
                                      new XElement("ProviderCode", detail.CodeProvider),
                                      new XElement("ClientCode", detail.CodeClient),
                                      new XElement("BarCode", detail.BarCode),
                                      new XElement("AdditionalInformation", detail.AdditionalInfo),
                                      new XElement("MeasurementUnit", detail.MeasurementUnit),
                                      new XElement("Quantity", detail.Quantity),
                                      new XElement("Price", detail.Price),
                                      new XElement("Value", detail.Value),
                                      new XElement("VatPercentage", detail.VatPercentage),
                                      new XElement("VAT", detail.VAT))),

                           new XElement("Summary",
                               new XElement("TotalValue", invoiceHeader.TotalValue),
                               new XElement("TotalVAT", invoiceHeader.VAT),
                               new XElement("TotalAmount", invoiceHeader.TotalAmount))),

                           new XElement("Observations",
                               new XElement("Observations", invoiceHeader.Observations),
                               new XElement("ClientSold", invoiceHeader.ClientSoldInfo),
                               new XElement("PaymentMethod", invoiceHeader.PaymentMethod))));
                }

            }

            return xml.ToString();
        }
    }
}
