using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public class Provider: Company
    {
        public decimal Capital { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
