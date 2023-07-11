using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
  public  class SalePaymentModel
    {
        public int CustomerPaymentID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        public string CustomerConatctNo { get; set; }
        public string CustomerAddress { get; set; }
        public int CustomerInvoiceID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public double TotalAmount { get; set; }
        public double PaymentAmount { get; set; }
        public double RemainingBalance { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
