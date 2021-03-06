﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public class InvoiceHeader
    {
        public int Id { get; set; }
        public virtual Tenant Tenant { get; set; }

        public virtual Provider Provider { get; set; }
        public virtual Client Client { get; set; }

        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool InversTaxing { get; set; }
        public bool VatCollecting { get; set; }
        public string Description { get; set; }
        public string Currecy { get; set; }
        public decimal VAT { get; set; }
        public decimal Weight { get; set; }

        public ICollection<InvoiceDetail> Details { get; private set; }

        public decimal TotalValue { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalAmount { get; set; }

        public string Observations { get; set; }
        public string ClientSoldInfo { get; set; }
        public string PaymentMethod { get; set; }
    }
}
