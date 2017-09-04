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

        public Provider GetProviderById(int id)
        {
            var provider = _context.Provider.SingleOrDefault(m => m.Id == id);
            return provider;
        }

        public Client GetClientById(int id)
        {
            var client = _context.Client.SingleOrDefault(m => m.Id == id);
            return client;
        }

        public List<SelectListItem> GetProviderDetails()
        {
            var selectList = new List<SelectListItem>();
            var getProviders = _context.Provider.ToList();

            foreach (var provider in getProviders)
            {
                selectList.Add(new SelectListItem
                {
                    Text = provider.Name,
                    Value = provider.Id.ToString(),
                });
            }
            return selectList;
        }

        public List<SelectListItem> GetClientDetails()
        {
            var selectList = new List<SelectListItem>();
            var getClients = _context.Client.ToList();

            foreach (var client in getClients)
            {
                selectList.Add(new SelectListItem
                {
                    Text = client.Name,
                    Value = client.Id.ToString(),
                });
            }

            return selectList;
        }


        // GET: InvoiceHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeader.SingleOrDefaultAsync(m => m.Id == id);
            var file = await _context.FileDetail.SingleOrDefaultAsync(m => m.Id == invoiceHeader.FileDetailId);
            var details = await _context.InvoiceDetail.Where(x => x.FileDetailId == file.Id).ToListAsync();
            var client = await _context.Client.SingleOrDefaultAsync(c => c.Id == invoiceHeader.ClientId);
            var provider = await _context.Provider.SingleOrDefaultAsync(p => p.Id == invoiceHeader.ProviderId);

            invoiceHeader.FileDetail = file;
            invoiceHeader.Details = details;
            invoiceHeader.FileDetail.FileName = file.FileName;
            invoiceHeader.Client = client;
            invoiceHeader.Provider = provider;

            if (invoiceHeader == null && details == null && client == null && provider == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }


        // POST: InvoiceHeaders/Details
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod")] InvoiceHeader invoiceHeader)
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

        // GET: InvoiceHeaders/Create
        public IActionResult Create()
        {
            ViewBag.Provider = GetProviderDetails();
            ViewBag.Client = GetClientDetails();

            var invoiceHeader = new InvoiceHeader();
            var invoiceDetails = _context.InvoiceDetail.ToList();
            invoiceHeader.Details = invoiceDetails;
            invoiceHeader.showInvoiceDetails = false;

            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod")] InvoiceHeader invoiceHeader, List<IFormFile> files, string ddlProvider, string ddlClient)
        {
            if (ModelState.IsValid)
            {
                // Populate dropdown contents
                ViewBag.Provider = GetProviderDetails();
                ViewBag.Client = GetClientDetails();

                int selectedProviderId = 0;
                int selectedClientId = 0;
                var selectedProvider = new Provider();
                var selectedClient = new Client();

                // Get selected items
                if (int.TryParse(ddlProvider, out selectedProviderId))
                {
                    selectedProvider = GetProviderById(selectedProviderId);
                }
                if (int.TryParse(ddlClient, out selectedClientId))
                {
                    selectedClient = GetClientById(selectedClientId);
                }

                invoiceHeader.Client = selectedClient;
                invoiceHeader.Provider = selectedProvider;
                invoiceHeader.showInvoiceDetails = true;

                var fileUploaded = UploadFiles(files);
                invoiceHeader.FileDetail = fileUploaded;
                invoiceHeader.Details = fileUploaded.Content;

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

            // Populate dropdown contents
            ViewBag.Provider = GetProviderDetails();
            ViewBag.Client = GetClientDetails();

            var model = await _context.InvoiceHeader.SingleOrDefaultAsync(m => m.Id == id);

            var file = _context.FileDetail.SingleOrDefault(m => m.Id == model.FileDetailId);
            var details = _context.InvoiceDetail.Where(x => x.FileDetailId == file.Id).ToList();
            var client = _context.Client.SingleOrDefault(c => c.Id == model.ClientId);
            var provider = _context.Provider.SingleOrDefault(p => p.Id == model.ProviderId);

            model.FileDetail = file;
            model.Details = details;
            model.FileDetail.FileName = file.FileName;
            model.Client = client;
            model.Provider = provider;

            if (model == null)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod")] InvoiceHeader invoiceHeader, List<IFormFile> files, string ddlProvider, string ddlClient)
        {
            if (id != invoiceHeader.Id && files == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int selectedProviderId = 0;
                    int selectedClientId = 0;
                    var selectedProvider = new Provider();
                    var selectedClient = new Client();

                    // Get selected items
                    if (int.TryParse(ddlProvider, out selectedProviderId))
                    {
                        selectedProvider = GetProviderById(selectedProviderId);
                    }
                    if (int.TryParse(ddlClient, out selectedClientId))
                    {
                        selectedClient = GetClientById(selectedClientId);
                    }

                    invoiceHeader.Client = selectedClient;
                    invoiceHeader.Provider = selectedProvider;

                    var uploadedFile = UploadFiles(files);
                    invoiceHeader.FileDetail = uploadedFile;
                    invoiceHeader.Details = uploadedFile.Content;

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
                return View("Details", invoiceHeader);
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

        private FileDetail UploadFiles(List<IFormFile> files)
        {
            // Start processing uploaded file
            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads" + "\\");

            var fileDetail = new FileDetail();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

                    using (var inputStream = new StreamReader(formFile.OpenReadStream()))
                    {
                        var items = inputStream.ReadToEnd();

                        fileDetail.FileName = fileName;
                        fileDetail.FileType = formFile.ContentType;
                        fileDetail.Length = Convert.ToInt32(formFile.Length);
                        fileDetail.Content = ConvertCSVtoList(items);
                    }
                }
            }
            // Save uploaded file
            _context.FileDetail.Add(fileDetail);
            return fileDetail;
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

        public FileContentResult ExportToXML(string ProviderId, string ClientId, int invoiceId)
        {
            var xml = ProcessListToXML(ProviderId, ClientId, invoiceId);

            XDocument doc = XDocument.Parse(xml);
            var contentType = "application/xml";
            var content = xml.ToString();
            var bytes = Encoding.UTF8.GetBytes(content);
            var result = new FileContentResult(bytes, contentType);
            var currentDate = DateTime.Now.ToString("MMddyyyy");
            result.FileDownloadName = "Invoice_" + currentDate + "_" + Guid.NewGuid() + ".xml";

            return result;
        }

        private string ProcessListToXML(string sprovider, string sclient, int invoiceId)
        {
            var xml = new XDocument();

            var invoiceHeader = _context.InvoiceHeader.SingleOrDefault(m => m.Id == invoiceId);
            var file = _context.FileDetail.SingleOrDefault(m => m.Id == invoiceHeader.FileDetailId);
            var details = _context.InvoiceDetail.Where(x => x.FileDetailId == file.Id).ToList();
            var client = _context.Client.SingleOrDefault(c => c.Id == invoiceHeader.ClientId);
            var provider = _context.Provider.SingleOrDefault(p => p.Id == invoiceHeader.ProviderId);

            invoiceHeader.FileDetail = file;
            invoiceHeader.Details = details;
            invoiceHeader.FileDetail.FileName = file.FileName;
            invoiceHeader.Client = client;
            invoiceHeader.Provider = provider;

            xml = new XDocument(new XDeclaration("1.0", "UTF - 8", "yes"),
                new XElement("Facturi",
                    new XElement("Factura",
                        new XElement("Antet",
                        new XElement("FurnizorNume", invoiceHeader.Provider.Name),
                        new XElement("FurnizorCIF", invoiceHeader.Provider.CIF),
                        new XElement("FurnizorNrRegCom", invoiceHeader.Provider.RegNumber),
                        new XElement("FurnizorCapital", invoiceHeader.Provider.Capital),
                        new XElement("FurnizorAdresa", invoiceHeader.Provider.Address),
                        new XElement("FurnizorBanca", invoiceHeader.Provider.Bank),
                        new XElement("FurnizorIBAN", invoiceHeader.Provider.IBAN),
                        new XElement("FurnizorInformatiiSuplimentare", invoiceHeader.Provider.Description),

                        new XElement("ClientNume", invoiceHeader.Client.Name),
                        new XElement("ClientInformatiiSuplimentare", invoiceHeader.Client.Description),
                        new XElement("ClientCIF", invoiceHeader.Client.CIF),
                        new XElement("ClientNrRegCom", invoiceHeader.Client.RegNumber),
                        new XElement("ClientAdresa", invoiceHeader.Client.Address),
                        new XElement("ClientBanca", invoiceHeader.Client.Bank),
                        new XElement("ClientIBAN", invoiceHeader.Client.IBAN),

                        new XElement("FacturaNumar", invoiceHeader.Number),
                        new XElement("FacturaData", invoiceHeader.IssueDate),
                        new XElement("FacturaScadenta", invoiceHeader.DueDate),
                        new XElement("FacturaTaxareInversa", invoiceHeader.InversTaxing),
                        new XElement("FacturaTVAIncasare", invoiceHeader.VatCollecting),
                        new XElement("FacturaInformatiiSuplimentare", invoiceHeader.Description),
                        new XElement("FacturaMoneda", invoiceHeader.Currecy),
                        new XElement("FacturaCotaTVA", invoiceHeader.VAT),
                        new XElement("FacturaGreutate", invoiceHeader.Weight)),

                    new XElement("Detalii",
                        from detail in invoiceHeader.Details
                        select new XElement("Continut",
                                new XElement("Linie",
                                new XElement("LinieNrCrt", detail.Number),
                                // new XElement("Descriere", detail.)
                                new XElement("CodArticolFurnizor", detail.CodeProvider),
                                new XElement("CodArticolClient", detail.CodeClient),
                                new XElement("CodBare", detail.BarCode),
                                new XElement("InformatiiSuplimentare", detail.AdditionalInfo),
                                new XElement("UM", detail.MeasurementUnit),
                                new XElement("Cantitate", detail.Quantity),
                                new XElement("Pret", detail.Price),
                                new XElement("Valoare", detail.Value),
                                new XElement("ProcTVA", detail.VatPercentage),
                                new XElement("TVA", detail.VAT)))),

                    new XElement("Sumar",
                        new XElement("TotalValoare", invoiceHeader.TotalValue),
                        new XElement("TotalTVA", invoiceHeader.VAT),
                        new XElement("Total", invoiceHeader.TotalAmount))),

                    new XElement("Observatii",
                        new XElement("txtObservatii", invoiceHeader.Observations),
                        new XElement("SoldClient", invoiceHeader.ClientSoldInfo),
                        new XElement("ModalitatePlata", invoiceHeader.PaymentMethod))));

            return xml.ToString();
        }
    }
}
