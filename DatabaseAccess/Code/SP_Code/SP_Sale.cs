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
   public class SP_Sale
    {

        private CloudErpV1Entities db = new CloudErpV1Entities();
        public List<SalePaymentModel> ReamaningPaymentList(int CompanyID, int BranchID)
        {
            var remaingpaymentlist = new List<SalePaymentModel>();

            SqlCommand command = new SqlCommand("GetCustomersRemainPaymentRecord", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BranchID", BranchID);
            command.Parameters.AddWithValue("@CompanyID", CompanyID);
            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                int customerID = Convert.ToInt32(Convert.ToString(row[4]));
                var customer = db.tblCustomers.Find(customerID);
                //int userid = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]));
                //var user = db.tblUsers.Find(userid);
                var Payement = new SalePaymentModel();

                Payement.CustomerInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                Payement.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                Payement.InvoiceNo = Convert.ToString(row[5]);
                //double payamount = 0;
                //double.TryParse(Convert.ToString(row[7]), out payamount);
                
                try
                {


                    Payement.PaymentAmount = double.Parse(Convert.ToString(row[7]));
                }
                catch
                {
                    Payement.PaymentAmount = 0;

                }

                try
                {


                    Payement.RemainingBalance = double.Parse(Convert.ToString(row[8]));
                }
                catch
                {
                    Payement.RemainingBalance = 0;

                }

                 try
                {


                    Payement.TotalAmount = double.Parse(Convert.ToString(row[6]));
                }
                catch
                {
                    Payement.TotalAmount = 0;

                }



                Payement.CustomerConatctNo = customer.CustomerContact;
                Payement.CustomerAddress = customer.CustomerAddress;
                Payement.CustomerID = customer.CustomerID;
                Payement.CustomerName = customer.Customername;
                // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
              //  Payement.TotalAmount = double.Parse(Convert.ToString(row["TotalAmount"]));
                // UserID = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]))

                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }
            public List<SalePaymentModel> CustomSaleList(int CompanyID, int BranchID, DateTime FromDate, DateTime ToDate)
        {
            var remaingpaymentlist = new List<SalePaymentModel>();

            SqlCommand command = new SqlCommand("GetSalesHistory", DatabaseQuery.ConnOpen());
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
                int customerID = Convert.ToInt32(Convert.ToString(row[4]));
                var customer = db.tblCustomers.Find(customerID);
                //int userid = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]));
                //var user = db.tblUsers.Find(userid);
                var Payement = new SalePaymentModel();

                Payement.CustomerInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                Payement.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                Payement.InvoiceNo = Convert.ToString(row[5]);
                //double payamount = 0;
                //double.TryParse(Convert.ToString(row[7]), out payamount);
                
                try
                {


                    Payement.PaymentAmount = double.Parse(Convert.ToString(row[7]));
                }
                catch
                {
                    Payement.PaymentAmount = 0;

                }

                try
                {


                    Payement.RemainingBalance = double.Parse(Convert.ToString(row[8]));
                }
                catch
                {
                    Payement.RemainingBalance = 0;

                }

                 try
                {


                    Payement.TotalAmount = double.Parse(Convert.ToString(row[6]));
                }
                catch
                {
                    Payement.TotalAmount = 0;

                }



                Payement.CustomerConatctNo = customer.CustomerContact;
                Payement.CustomerAddress = customer.CustomerAddress;
                Payement.CustomerID = customer.CustomerID;
                Payement.CustomerName = customer.Customername;
                // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
              //  Payement.TotalAmount = double.Parse(Convert.ToString(row["TotalAmount"]));
                // UserID = Convert.ToInt32(Convert.ToString(row["SupplierInvoiceID"]))

                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }


        public List<SalePaymentModel> SalePaymentHistory(int CustomerInvoiceID)
        {
            var remaingpaymentlist = new List<SalePaymentModel>();

            SqlCommand command = new SqlCommand("GetCustomerPaymentHistory", DatabaseQuery.ConnOpen());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustomerInvoiceID", CustomerInvoiceID);

            var dt = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                int customerID = Convert.ToInt32(Convert.ToString(row[4]));
                var customer = db.tblCustomers.Find(customerID);
                int userid = Convert.ToInt32(Convert.ToString(row[9]));
                var user = db.tblUsers.Find(userid);



                var Payement = new SalePaymentModel();

     
                Payement.CustomerInvoiceID = Convert.ToInt32(Convert.ToString(row[0]));
                Payement.BranchID = Convert.ToInt32(Convert.ToString(row[1]));
                Payement.CompanyID = Convert.ToInt32(Convert.ToString(row[2]));
                Payement.InvoiceDate = Convert.ToDateTime(Convert.ToString(row[3]));
                Payement.InvoiceNo = Convert.ToString(row[5]);
                double payamount = 0;
                double.TryParse(Convert.ToString(row[7]), out payamount); 
                double remainingBalance = 0;
                double.TryParse(Convert.ToString(row[8]), out remainingBalance); 
                 double totalAmount = 0;
                double.TryParse(Convert.ToString(row[7]), out totalAmount);

                Payement.PaymentAmount = payamount;
                Payement.RemainingBalance = remainingBalance;

                Payement.CustomerConatctNo = customer.CustomerContact;
                Payement.CustomerAddress = customer.CustomerAddress;
                Payement.CustomerID = customer.CustomerID;
                Payement.CustomerName = customer.Customername;
                // SupplierPaymentID = Convert.ToInt32(Convert.ToString(row["SupplierPaymentID"])),
                Payement.TotalAmount = totalAmount;
                    Payement.UserID = user.UserID;
                Payement.UserName = user.UserName;

                
                remaingpaymentlist.Add(Payement);
            }

            return remaingpaymentlist;
        }
    }
}
