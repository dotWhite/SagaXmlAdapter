using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SagaXmlAdapter.Web.Data;
using SagaXmlAdapter.Web.Models;

namespace SagaXmlAdapter.Web.Controllers
{
    public class InvoiceHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceHeadersController(ApplicationDbContext context)
        {
            _context = context;    
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
        public async Task<IActionResult> Create([Bind("Id,Number,IssueDate,DueDate,InversTaxing,VatCollecting,Description,Currecy,VAT,Weight,TotalValue,TotalVat,TotalAmount,Observations,ClientSoldInfo,PaymentMethod")] InvoiceHeader invoiceHeader)
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
    }
}
