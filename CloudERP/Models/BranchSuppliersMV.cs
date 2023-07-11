using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudERP.Models
{
    public class BranchSuppliersMV
    {
        [Key]
        public int ID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierConatctNo { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierEmail { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string User { get; set; }
        public string Discription { get; set; }

    }
}