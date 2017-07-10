using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public class Client: Company
    {
        public virtual Tenant Tenant { get; set; }
    }
}
