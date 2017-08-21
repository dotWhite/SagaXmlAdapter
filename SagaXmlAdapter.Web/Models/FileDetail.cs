using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagaXmlAdapter.Web.Models
{
    public class FileDetail
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public int Length { get; set; }
        public string FileType { get; set; }
        public List<InvoiceDetail> Content { get; set; }        
    }
}
