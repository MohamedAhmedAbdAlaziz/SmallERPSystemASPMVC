using DatabaseAccess.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Code.SP_Code
{
    public class SP_Purchase
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        public List<PurchasePaymentModel> ReamaningPaymentList(int CompanyID,int BranchID)
        {
            var remaingpaymentlist= new  List<PurchasePaymentModel>();

            SqlCommand command = new SqlCommand("GetSupppliersRemainPaymentRecord", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BranchID", BranchID);
            command.Parameters.AddWithValue("@CompanyID", CompanyID);
            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                int supplierid = Convert.ToInt32(Convert.ToString(row["SupplierID"]));
                var supplier = db.tblSuppliers.Find(supplierid);
                //int userid = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]));
                //var user = db.tblUsers.Find(userid);
                var Payement = new PurchasePaymentModel();

                Payement.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                Payement.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                Payement.InvoiceNo = Convert.ToString(row[4]);
                double totalamount = 0;
                double.TryParse(Convert.ToString(row[6]), out totalamount);

                //double returntotalamount = 0;
                //double.TryParse(Convert.ToString(row[7]), out returntotalamount);

                //double afterreturntotalamount = 0;
                //double.TryParse(Convert.ToString(row[8]), out afterreturntotalamount);

              double payamount = 0;
                 double.TryParse(Convert.ToString(row[7]), out payamount);
                //double returnpayamount = 0;
                //double.TryParse(Convert.ToString(row[7]), out returnpayamount);


                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingbalance);

                Payement.PaymentAmount = payamount;
                Payement.RemainingBalance = remainingbalance;
                //try
                //{


                //    Payement.PaymentAmount = double.Parse(Convert.ToString(row["Payment"]));
                //}
                //catch
                //{
                //    Payement.PaymentAmount = 0;

                //}

                //try
                //{


                //    Payement.RemainingBalance = double.Parse(Convert.ToString(row["ReamingBalance"]));
                //}
                //catch
                //{
                //    Payement.RemainingBalance = 0;

                //}



                Payement.SupplierConatctNo = supplier.SupplierConatctNo;
                Payement.SupplierAddress = supplier.SupplierAddress;
                Payement.SupplierID = supplier.SupplierID;
                Payement.SupplierName = supplier.SupplierName;
                // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
                // Payement.TotalAmount = double.Parse(Convert.ToString(row["TotalAmount"]));
                Payement.TotalAmount = totalamount;
                //Payement.ReturnProductAmount = returntotalamount;
                //Payement.ReturnPaymentAmount = returnpayamount;
                //Payement.AfterReturnTotalAmount = afterreturntotalamount;

                // UserID = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]))

                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }


        public List<PurchasePaymentModel> CustomPurchaseList(int CompanyID, int BranchID,DateTime FromDate , DateTime ToDate)
        {
            var remaingpaymentlist = new List<PurchasePaymentModel>();

            SqlCommand command = new SqlCommand("GetPurchasesHistory", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BranchID", BranchID);
            command.Parameters.AddWithValue("@CompanyID", CompanyID);
            command.Parameters.AddWithValue("@FromDate", FromDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@ToDate", ToDate.ToString("yyyy-MM-dd"));
            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                int supplierid = Convert.ToInt32(Convert.ToString(row[4]));
                var supplier = db.tblSuppliers.Find(supplierid);
                //int userid = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]));
                //var user = db.tblUsers.Find(userid);
                var Payement = new PurchasePaymentModel();

                Payement.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                Payement.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                Payement.InvoiceNo = Convert.ToString(row[5]);
                double totalamount=0;
                double.TryParse(Convert.ToString(row[6]), out totalamount);

                //double returntotalamount=0;
                //double.TryParse(Convert.ToString(row[7]), out returntotalamount);

                //double afterreturntotalamount=0;
                //double.TryParse(Convert.ToString(row[8]), out afterreturntotalamount);
              
                double payamount = 0;
                double.TryParse(Convert.ToString(row[7]), out payamount);
               
                //double returnpayamount = 0;
                //double.TryParse(Convert.ToString(row[10]), out returnpayamount);


                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingbalance);

                Payement.PaymentAmount = payamount;
                Payement.RemainingBalance = remainingbalance;
                //try
                //{


                //    Payement.PaymentAmount = double.Parse(Convert.ToString(row["Payment"]));
                //}
                //catch
                //{
                //    Payement.PaymentAmount = 0;

                //}

                //try
                //{


                //    Payement.RemainingBalance = double.Parse(Convert.ToString(row["ReamingBalance"]));
                //}
                //catch
                //{
                //    Payement.RemainingBalance = 0;

                //}



                Payement.SupplierConatctNo = supplier.SupplierConatctNo;
                Payement.SupplierAddress = supplier.SupplierAddress;
                Payement.SupplierID = supplier.SupplierID;
                Payement.SupplierName = supplier.SupplierName;
                // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
               // Payement.TotalAmount = double.Parse(Convert.ToString(row["TotalAmount"]));
                Payement.TotalAmount = totalamount;
                //Payement.ReturnProductAmount = returntotalamount;
                //Payement.ReturnPaymentAmount = returnpayamount;
                //Payement.AfterReturnTotalAmount = afterreturntotalamount;

                // UserID = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]))

                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }
         



        public List<PurchasePaymentModel> PurchasePaymentHistory(int SupplierInvoiceID)
        {
            var remaingpaymentlist = new List<PurchasePaymentModel>();

            SqlCommand command = new SqlCommand("GetSupplierPaymentHistory", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SupplierInvoiceID", SupplierInvoiceID);
         
            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {

                int supplierid = Convert.ToInt32(Convert.ToString(row[4]));
                var supplier = db.tblSuppliers.Find(supplierid);
            int userid = Convert.ToInt32(Convert.ToString(row[9]));
               var user = db.tblUsers.Find(userid);
                double payamount = 0;
                double.TryParse(Convert.ToString(row[7]), out payamount);
                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingbalance);
                double totalAmount = 0;
                double.TryParse(Convert.ToString(row[6]), out totalAmount);

                var Payement = new PurchasePaymentModel();

                Payement.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                    Payement.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                    Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                    Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                    Payement.InvoiceNo = Convert.ToString(row[5]);

                    Payement.PaymentAmount = payamount;//double.Parse(Convert.ToString(row["PaymentAmount"])),
                    Payement.RemainingBalance = remainingbalance;// double.Parse(Convert.ToString(row["RemainingBalance"])),
                   Payement.SupplierConatctNo = supplier.SupplierConatctNo;
                    Payement.SupplierAddress = supplier.SupplierAddress;
                   Payement.SupplierID = supplier.SupplierID;
                    Payement.SupplierName = supplier.SupplierName;
                    // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
                    Payement.TotalAmount = totalAmount;// double.Parse(Convert.ToString(row["TotalAmount"])),
                   Payement.UserID = user.UserID;
                   Payement.UserName = user.UserName;
                   
              
                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }


        public List<SupplierReturnInvoiceModel> PurchaseReturnPaymentPending(int SupplierInvoiceID)
     {
            var remaingpaymentlist = new List<SupplierReturnInvoiceModel>();

            SqlCommand command = new SqlCommand("GetSupplierReturnPurchasePaymentPending", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SupplierInvoiceID", (int)SupplierInvoiceID);

            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {

                int supplierid = Convert.ToInt32(Convert.ToString(row[5]));
                var supplier = db.tblSuppliers.Find(supplierid);
                int userid = Convert.ToInt32(Convert.ToString(row[10]));
                var user = db.tblUsers.Find(userid);
                double payamount = 0;
                double.TryParse(Convert.ToString(row[8]), out payamount);
                double remainingbalance = 0;
                double.TryParse(Convert.ToString(row[9]), out remainingbalance);
                double totalAmount = 0;
                double.TryParse(Convert.ToString(row[7]), out totalAmount);

                var Payement = new SupplierReturnInvoiceModel();

                Payement.SupplierReturnInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                Payement.SupplierInvoiceID = Convert.ToInt32(Convert.ToString(row[1]));
                Payement.BranchID = Convert.ToInt32(Convert.ToString(row[2]));
                Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[3]));
                Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[4]));
                Payement.InvoiceNo = Convert.ToString(row[6]);

                Payement.ReturnPaymentAmount = payamount;//double.Parse(Convert.ToString(row["PaymentAmount"])),
                Payement.RemainingBalance = remainingbalance;// double.Parse(Convert.ToString(row["RemainingBalance"])),
                Payement.SupplierConatctNo = supplier.SupplierConatctNo;
                Payement.SupplierAddress = supplier.SupplierAddress;
                Payement.SupplierID = supplier.SupplierID;
                Payement.SupplierName = supplier.SupplierName;
                // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
                Payement.ReturnTotalAmount = totalAmount;// double.Parse(Convert.ToString(row["TotalAmount"])),
                Payement.UserID = user.UserID;
                Payement.UserName = user.UserName;


                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }
    }
}
