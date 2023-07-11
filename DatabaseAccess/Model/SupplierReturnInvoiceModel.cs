using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
 public class SupplierReturnInvoiceModel
    {
        public int SupplierPaymentID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierConatctNo { get; set; }
        public string SupplierAddress { get; set; }
        public int SupplierReturnInvoiceID { get; set; }
        public int SupplierInvoiceID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public double ReturnTotalAmount { get; set; }
        public double ReturnPaymentAmount { get; set; }
        public double RemainingBalance { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
