using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public abstract class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CIF { get; set; }
        public string RegNumber { get; set; }        
        public string Address { get; set; }
        public string Bank { get; set; }
        public string IBAN { get; set; }
        public string Description { get; set; }
    }
}
