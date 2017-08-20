using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string CodeProvider { get; set; }
        public string CodeClient { get; set; }
        public string BarCode { get; set; }
        public string AdditionalInfo { get; set; }
        public string MeasurementUnit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal VAT { get; set; }
        public string Observations { get; set; }

        public virtual FileDetails FileDetail { get; set; }
        public SelectList ProviderList { get; set; }
        public SelectList ClientList { get; set; }
    }
}
