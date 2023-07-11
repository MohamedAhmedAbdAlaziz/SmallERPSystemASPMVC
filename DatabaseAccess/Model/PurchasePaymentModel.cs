using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Model
{
   public class PurchasePaymentModel
    {
        public int SupplierPaymentID { get; set; }
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }

        public string SupplierConatctNo { get; set; }
        public string SupplierAddress { get; set; }
        public int SupplierInvoiceID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public double TotalAmount { get; set; }
        public double PaymentAmount { get; set; }
        public double RemainingBalance { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }

        //SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"])),
        //            BranchID = Convert.ToInt32(Convert.ToString(row["BranchID"])),
        //            CompanyID = Convert.ToInt32(Convert.ToString(row["CompanyID"])),
        //            InvoiceDate = Convert.ToDateTime(Convert.ToString(row["InvoiceDate"])),
        //            InvoiceNo = Convert.ToString(row["InvoiceNo"]),
        //            PaymentAmount = double.Parse(Convert.ToString(row["Payment"])),
        //            RemainingBalance = double.Parse(Convert.ToString(row["ReamingBalance"])),
        //            SupplierConatctNo = supplier.SupplierConatctNo,
        //            SupplierAddress = supplier.SupplierAddress,
        //            SupplierID = supplier.SupplierID,
        //            SupplierName = supplier.SupplierName,
        //            // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
        //            TotalAmount = double.Parse(Convert.ToString(row["TotalAmount"])),
        //           UserID =user.UserID,
        //           UserName=user.UserName

    }
}
