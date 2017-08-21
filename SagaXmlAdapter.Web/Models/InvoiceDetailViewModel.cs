using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public class InvoiceDetailViewModel
    {
        public InvoiceDetail InvoiceDetail { get; set; }
        public IEnumerable<InvoiceDetail> AllInvoiceDetails { get; set; }
    }
}
