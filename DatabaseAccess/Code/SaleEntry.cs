using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Code
{
   public class SaleEntry
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        public string selectcustomerid = string.Empty;

        DataTable dtEntries = null;
        public string ConfrimSale(int CompanyID, int BranchID, int UserID, string InvoiceNo, string CustomerInvoiceID, float Amount, string CustomerID, string CustomerName, bool ispayment)
        {

            try
            {
                //ep.Clear();
                dtEntries = null;
                // int supplierid = 0;

                string pruchasetitle = "Sale to " + CustomerName.Trim();
                //   string pruchasetitle = "Purchase From " + supplier.SupplierName.Trim();
                //    string successmessage = "Purchase Success";
                var Financialyearcheck = DatabaseQuery.Retrive("select top 1 FinancialYearID from tblFinancialYear where IsActive = 1");
                string FinancialYearID = (Financialyearcheck != null ? Convert.ToString(Financialyearcheck.Rows[0][0]) : string.Empty);
                if (string.IsNullOrEmpty(FinancialYearID))
                {
                    return "Your Company Financial is not Set, Please Contact To Adminstrator!";
                }



                // Get Invoice Purchase ID
                string successmessage = "Sale Success";


                string AccountHeadID = string.Empty;
                string AccountControlID = string.Empty;
                string AccountSubControlID = string.Empty;
                // Assests 1      increae(Debit)   decrese(Credit)
                // Liabilities 2     increae(Credit)   decrese(Debit)
                // Expenses 3     increae(Debit)   decrese(Credit)
                // Capital 4     increae(Credit)   decrese(Debit)
                // Revenue 5     increae(Credit)   decrese(Debit)
                var Saleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID ==1 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                //Credit Entry Sale
                AccountHeadID = Convert.ToString(Saleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(Saleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(Saleaccount.AccountSubControlID);
                string transectiontitle = string.Empty;
                transectiontitle = "Sale to " + CustomerName.Trim();
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);


                //Debit Entry Sale
                Saleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 10 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(Saleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(Saleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(Saleaccount.AccountSubControlID);
                transectiontitle = CustomerName + " , Sale Payment is Pending!";
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(),  "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);

                if (ispayment == true)
                {
                    string payinvoicenno = "INP" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                    Saleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 10 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                    AccountHeadID = Convert.ToString(Saleaccount.AccountHeadID);
                    AccountControlID = Convert.ToString(Saleaccount.AccountControlID);
                    AccountSubControlID = Convert.ToString(Saleaccount.AccountSubControlID);

                    //transectiontitle = "Payment Paid to " +supplierName.Trim();
                    transectiontitle = "Payment Paid By " + CustomerName;
                    SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, payinvoicenno, UserID.ToString(),Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);

                    Saleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID ==11 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                    AccountHeadID = Convert.ToString(Saleaccount.AccountHeadID);
                    AccountControlID = Convert.ToString(Saleaccount.AccountControlID);
                    AccountSubControlID = Convert.ToString(Saleaccount.AccountSubControlID);

                    //transectiontitle = "Payment Paid to " +supplierName.Trim();
                    transectiontitle = CustomerName + " , Sale Payment is Succeed!"; ;
                    SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, payinvoicenno, UserID.ToString(), "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);


                    string paymentquery = string.Format("insert into tblCustomerPayment(CustomerID,CustomerInvoiceID,UserID,invoiceNo,TotalAmount,PaidAmount,RemainingBalance, CompanyID,BranchID,InvoiceDate)" +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    CustomerID, CustomerInvoiceID, UserID, payinvoicenno, Amount, Amount, "0", CompanyID, BranchID, DateTime.Now.ToString("yyyy/MM/dd"));
                    DatabaseQuery.Insert(paymentquery);

                    successmessage = successmessage + " with Payment.";

                }
                foreach (DataRow entryrow in dtEntries.Rows)
                {
                    string entryquery = string.Format("insert into tblTransaction (FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID,InvoiceNo, UserID,Credit,Debit,TransectionDate,TransectionTitle,CompanyID,BranchID) values " +
                 " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                 Convert.ToString(entryrow[0]), Convert.ToString(entryrow[1]), Convert.ToString(entryrow[2]), Convert.ToString(entryrow[3]), Convert.ToString(entryrow[4]), Convert.ToString(entryrow[5]), Convert.ToString(entryrow[6]), Convert.ToString(entryrow[7]), Convert.ToDateTime(Convert.ToString(entryrow[8])).ToString("yyyy/MM/dd HH:mm"), Convert.ToString(entryrow[9]), CompanyID, BranchID);
                    DatabaseQuery.Insert(entryquery);
                }
                return successmessage;




            }
            catch (Exception)
            {

                return "Unexpected Error id Occur Please Try Again !";
            }


        }





        public string SalePayment(int CompanyID, int BranchID, int UserID, string InvoiceNo, 
            string CustomerInvoiceID, float TotalAmount, float Amount, string CustomerID, string CustomerName, float RemainingBalance)
        {
              try
            {
                //ep.Clear();
                dtEntries = null;
                // int supplierid = 0;

                string pruchasetitle = "Sale tp " + CustomerName.Trim();
                //   string pruchasetitle = "Purchase From " + supplier.SupplierName.Trim();
                //    string successmessage = "Purchase Success";
                var Financialyearcheck = DatabaseQuery.Retrive("select top 1 FinancialYearID from tblFinancialYear where IsActive = 1");
                string FinancialYearID = (Financialyearcheck != null ? Convert.ToString(Financialyearcheck.Rows[0][0]) : string.Empty);
                if (string.IsNullOrEmpty(FinancialYearID))
                {
                    return "Your Company Financial is not Set, Please Contact To Adminstrator!";
                }



                // Get Invoice Purchase ID

                string AccountHeadID = string.Empty;
                string AccountControlID = string.Empty;
                string AccountSubControlID = string.Empty;
                // Assests 1      increae(Debit)   decrese(Credit)
                // Liabilities 2     increae(Credit)   decrese(Debit)
                // Expenses 3     increae(Debit)   decrese(Credit)
                // Capital 4     increae(Credit)   decrese(Debit)
                // Revenue 5     increae(Credit)   decrese(Debit)
                //  var purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 3 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();


                string transectiontitle = string.Empty;
               // string payinvoicenno = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

                string payinvoicenno = "INP" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
              var  Saleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 10 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(Saleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(Saleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(Saleaccount.AccountSubControlID);

                //transectiontitle = "Payment Paid to " +supplierName.Trim();
                transectiontitle = "Payment Paid By " + CustomerName;
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);

                 Saleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 11 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(Saleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(Saleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(Saleaccount.AccountSubControlID);

                //transectiontitle = "Payment Paid to " +supplierName.Trim();
                transectiontitle = CustomerName + " , Sale Payment is Succeed!"; ;
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);


                string paymentquery = string.Format("insert into tblCustomerPayment(CustomerID,CustomerInvoiceID,UserID,invoiceNo,TotalAmount,PaidAmount,RemainingBalance, CompanyID,BranchID,InvoiceDate)" +
                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                CustomerID, CustomerInvoiceID, UserID, InvoiceNo, TotalAmount, Amount, Convert.ToString(RemainingBalance), CompanyID, BranchID, DateTime.Now.ToString("yyyy/MM/dd"));
                DatabaseQuery.Insert(paymentquery);

              //  successmessage = successmessage + " with Payment.";


                //string paymentquery = string.Format("insert into tblSupplierPayment(SupplierID,SupplierInvoiceID,UserID,InvoiceNo,TotalAmount,PaymentAmount,RemainingBalance, CompanyID,BranchID) " +
                //"values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                //SupplierID, SupplierInvoiceID, UserID, InvoiceNo, TotalAmount, Amount, Convert.ToString(RemainingBalance), CompanyID, BranchID);
                //DatabaseQuery.Insert(paymentquery);




                foreach (DataRow entryrow in dtEntries.Rows)
                {
                    string entryquery = string.Format("insert into tblTransaction (FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID,InvoiceNo, UserID,Credit,Debit,TransectionDate,TransectionTitle,CompanyID,BranchID) values " +
                 " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                 Convert.ToString(entryrow[0]), Convert.ToString(entryrow[1]), Convert.ToString(entryrow[2]), Convert.ToString(entryrow[3]), Convert.ToString(entryrow[4]), Convert.ToString(entryrow[5]), Convert.ToString(entryrow[6]), Convert.ToString(entryrow[7]), Convert.ToDateTime(Convert.ToString(entryrow[8])).ToString("yyyy/MM/dd HH:mm"), Convert.ToString(entryrow[9]), CompanyID, BranchID);
                    DatabaseQuery.Insert(entryquery);
                }
                return  "Paid Successfully.";




            }
            catch (Exception)
            {

                return "Unexpected Error id Occur Please Try Again !";
            }


        }







        //

        public string ReturnSale(int CompanyID, int BranchID, int UserID, string InvoiceNo, string CustomerInvoiceID,int CustomerReturnInvoiceID, float Amount, string CustomerID, string CustomerName, bool ispayment)
        {

            try
            {
                //ep.Clear();
                dtEntries = null;
                // int supplierid = 0;

                string returnpruchasetitle = "Return Sale from " + CustomerName.Trim();
                //   string pruchasetitle = "Purchase From " + supplier.SupplierName.Trim();
                //    string successmessage = "Purchase Success";
                var Financialyearcheck = DatabaseQuery.Retrive("select top 1 FinancialYearID from tblFinancialYear where IsActive = 1");
                string FinancialYearID = (Financialyearcheck != null ? Convert.ToString(Financialyearcheck.Rows[0][0]) : string.Empty);
                if (string.IsNullOrEmpty(FinancialYearID))
                {
                    return "Your Company Financial is not Set, Please Contact To Adminstrator!";
                }



                // Get Invoice Purchase ID
                string successmessage = "Return Sale Success";


                string AccountHeadID = string.Empty;
                string AccountControlID = string.Empty;
                string AccountSubControlID = string.Empty;
                // Assests 1      increae(Debit)   decrese(Credit)
                // Liabilities 2     increae(Credit)   decrese(Debit)
                // Expenses 3     increae(Debit)   decrese(Credit)
                // Capital 4     increae(Credit)   decrese(Debit)
                // Revenue 5     increae(Credit)   decrese(Debit)

                //Debit entry my sale
                var ReturnSaleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 2 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                //Credit Entry return Sale
                AccountHeadID = Convert.ToString(ReturnSaleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(ReturnSaleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(ReturnSaleaccount.AccountSubControlID);
                string transectiontitle = string.Empty;
                transectiontitle = "Return Sale from " + CustomerName.Trim();
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(),"0", Convert.ToString(Amount),DateTime.Now, transectiontitle);


                //Credit Entry return Sale
                ReturnSaleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 14 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(ReturnSaleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(ReturnSaleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(ReturnSaleaccount.AccountSubControlID);
                transectiontitle = CustomerName + " , Return Sale Payment is Pending!";
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);

                if (ispayment == true)
                {
                    string payinvoicenno = "RIP" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                    ReturnSaleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 14 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                    AccountHeadID = Convert.ToString(ReturnSaleaccount.AccountHeadID);
                    AccountControlID = Convert.ToString(ReturnSaleaccount.AccountControlID);
                    AccountSubControlID = Convert.ToString(ReturnSaleaccount.AccountSubControlID);

                    //transectiontitle = "Payment Paid to " +supplierName.Trim();
                    transectiontitle = "Return Payment Paid to " + CustomerName;
                    SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, payinvoicenno, UserID.ToString(),"0", Convert.ToString(Amount), DateTime.Now, transectiontitle);

                    ReturnSaleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 15 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                    AccountHeadID = Convert.ToString(ReturnSaleaccount.AccountHeadID);
                    AccountControlID = Convert.ToString(ReturnSaleaccount.AccountControlID);
                    AccountSubControlID = Convert.ToString(ReturnSaleaccount.AccountSubControlID);

                    //transectiontitle = "Payment Paid to " +supplierName.Trim();
                    transectiontitle = CustomerName + " , Return Sale Payment is Succeed!"; ;
                    SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, payinvoicenno, UserID.ToString(), Convert.ToString(Amount), "0",  DateTime.Now, transectiontitle);


                    string paymentquery = string.Format("insert into tblCustomerReturnPayment(CustomerID,CustomerInvoiceID,UserID,InvoiceNo,TotalAmount,PaidAmount,RemainingBalance, CompanyID,BranchID,CustomerReturnInvoiceID,InvoiceDate)" +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    CustomerID, CustomerInvoiceID, UserID, payinvoicenno, Amount, Amount, "0", CompanyID, BranchID, CustomerReturnInvoiceID, DateTime.Now.ToString("yyyy/MM/dd"));
                    DatabaseQuery.Insert(paymentquery);

                    successmessage = successmessage + " with Payment.";

                }
                foreach (DataRow entryrow in dtEntries.Rows)
                {
                    string entryquery = string.Format("insert into tblTransaction (FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID,InvoiceNo, UserID,Credit,Debit,TransectionDate,TransectionTitle,CompanyID,BranchID) values " +
                 " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                 Convert.ToString(entryrow[0]), Convert.ToString(entryrow[1]), Convert.ToString(entryrow[2]), Convert.ToString(entryrow[3]), Convert.ToString(entryrow[4]), Convert.ToString(entryrow[5]), Convert.ToString(entryrow[6]), Convert.ToString(entryrow[7]), Convert.ToDateTime(Convert.ToString(entryrow[8])).ToString("yyyy/MM/dd HH:mm"), Convert.ToString(entryrow[9]), CompanyID, BranchID);
                    DatabaseQuery.Insert(entryquery);
                }
                return successmessage;




            }
            catch (Exception)
            {

                return "Unexpected Error id Occur Please Try Again !";
            }


        }





        public string ReturnSalePayment(int CompanyID, int BranchID, int UserID, string InvoiceNo,
            string CustomerInvoiceID, int CustomerReturnInvoiceID, float TotalAmount, float Amount, string CustomerID, string CustomerName, float RemainingBalance)
        {
            try
            {
                //ep.Clear();
                dtEntries = null;
                // int supplierid = 0;

                string pruchasetitle = "Return Sale from " + CustomerName.Trim();
                //   string pruchasetitle = "Purchase From " + supplier.SupplierName.Trim();
                //    string successmessage = "Purchase Success";
                var Financialyearcheck = DatabaseQuery.Retrive("select top 1 FinancialYearID from tblFinancialYear where IsActive = 1");
                string FinancialYearID = (Financialyearcheck != null ? Convert.ToString(Financialyearcheck.Rows[0][0]) : string.Empty);
                if (string.IsNullOrEmpty(FinancialYearID))
                {
                    return "Your Company Financial is not Set, Please Contact To Adminstrator!";
                }



                // Get Invoice Purchase ID

                string AccountHeadID = string.Empty;
                string AccountControlID = string.Empty;
                string AccountSubControlID = string.Empty;
                // Assests 1      increae(Debit)   decrese(Credit)
                // Liabilities 2     increae(Credit)   decrese(Debit)
                // Expenses 3     increae(Debit)   decrese(Credit)
                // Capital 4     increae(Credit)   decrese(Debit)
                // Revenue 5     increae(Credit)   decrese(Debit)
                //  var purchaseaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 3 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();


                string transectiontitle = string.Empty;
                // string payinvoicenno = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

               // string payinvoicenno = "INP" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                var ReturnSaleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 14 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(ReturnSaleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(ReturnSaleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(ReturnSaleaccount.AccountSubControlID);

                //transectiontitle = "Payment Paid to " +supplierName.Trim();
                transectiontitle = "Return Payment Paid to " + CustomerName;
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), "0", Convert.ToString(Amount), DateTime.Now, transectiontitle);

                ReturnSaleaccount = db.tblAccountSettings.Where(a => a.AccountActivityID == 15 && a.CompanyID == CompanyID && a.BranchID == BranchID).FirstOrDefault();
                AccountHeadID = Convert.ToString(ReturnSaleaccount.AccountHeadID);
                AccountControlID = Convert.ToString(ReturnSaleaccount.AccountControlID);
                AccountSubControlID = Convert.ToString(ReturnSaleaccount.AccountSubControlID);

                //transectiontitle = "Payment Paid to " +supplierName.Trim();
                transectiontitle = CustomerName + " , Return Sale Payment is Succeed!"; ;
                SetEntries(FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID, InvoiceNo, UserID.ToString(), Convert.ToString(Amount), "0", DateTime.Now, transectiontitle);


                string paymentquery = string.Format("insert into tblCustomerReturnPayment(CustomerID,CustomerInvoiceID,UserID,InvoiceNo,TotalAmount,PaidAmount,RemainingBalance, CompanyID,BranchID,CustomerReturnInvoiceID,InvoiceDate)" +
                "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                CustomerID, CustomerInvoiceID, UserID, InvoiceNo, TotalAmount, Amount,Convert.ToString(RemainingBalance), CompanyID, BranchID, CustomerReturnInvoiceID, DateTime.Now.ToString("yyyy/MM/dd"));
                DatabaseQuery.Insert(paymentquery);


                //  successmessage = successmessage + " with Payment.";


                //string paymentquery = string.Format("insert into tblSupplierPayment(SupplierID,SupplierInvoiceID,UserID,InvoiceNo,TotalAmount,PaymentAmount,RemainingBalance, CompanyID,BranchID) " +
                //"values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                //SupplierID, SupplierInvoiceID, UserID, InvoiceNo, TotalAmount, Amount, Convert.ToString(RemainingBalance), CompanyID, BranchID);
                //DatabaseQuery.Insert(paymentquery);




                foreach (DataRow entryrow in dtEntries.Rows)
                {
                    string entryquery = string.Format("insert into tblTransaction (FinancialYearID, AccountHeadID, AccountControlID, AccountSubControlID,InvoiceNo, UserID,Credit,Debit,TransectionDate,TransectionTitle,CompanyID,BranchID) values " +
                 " ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                 Convert.ToString(entryrow[0]), Convert.ToString(entryrow[1]), Convert.ToString(entryrow[2]), Convert.ToString(entryrow[3]), Convert.ToString(entryrow[4]), Convert.ToString(entryrow[5]), Convert.ToString(entryrow[6]), Convert.ToString(entryrow[7]), Convert.ToDateTime(Convert.ToString(entryrow[8])).ToString("yyyy/MM/dd HH:mm"), Convert.ToString(entryrow[9]), CompanyID, BranchID);
                    DatabaseQuery.Insert(entryquery);
                }
                return "Paid Successfully.";




            }
            catch (Exception)
            {

                return "Unexpected Error id Occur Please Try Again !";
            }


        }








        private void SetEntries(
       string FinancialYearID,
       string AccountHeadID,
       string AccountControlID,
       string AccountSubControlID,
       string InvoiceNo,
       string UserID,
       string Credit,
       string Debit,
       DateTime TransactionDate,
       string TransectionTitle)
        {
            if (dtEntries == null)
            {
                dtEntries = new DataTable();
                dtEntries.Columns.Add("FinancialYearID");
                dtEntries.Columns.Add("AccountHeadID");
                dtEntries.Columns.Add("AccountControlID");
                dtEntries.Columns.Add("AccountSubControlID");
                dtEntries.Columns.Add("InvoiceNo");
                dtEntries.Columns.Add("UserID");
                dtEntries.Columns.Add("Credit");
                dtEntries.Columns.Add("Debit");
                dtEntries.Columns.Add("TransactionDate");
                dtEntries.Columns.Add("TransectionTitle");
            }

            if (dtEntries != null)
            {
                //if (dtEntries.Rows.Count == 0)
                //{
                dtEntries.Rows.Add(
                FinancialYearID,
                AccountHeadID,
                AccountControlID,
                AccountSubControlID,
                InvoiceNo,
                UserID,
                Credit,
                Debit,
                TransactionDate,
                TransectionTitle);
                //}
                //else
                //{
                //    bool isupdated = false;
                //    foreach (DataRow item in dtEntries.Rows)
                //    {
                //        decimal creditvalue = 0;
                //        decimal debetvalue = 0;
                //        decimal.TryParse(Convert.ToString(item[6]).Trim(), out creditvalue);
                //        decimal.TryParse(Convert.ToString(item[7]).Trim(), out debetvalue);

                //        if (Convert.ToString(item[1]).Trim() == AccountHeadID.Trim() &&
                //           Convert.ToString(item[2]).Trim() == AccountControlID.Trim() &&
                //           Convert.ToString(item[3]).Trim() == AccountSubControlID.Trim() &&
                //           creditvalue > 0)
                //        {
                //            item[6] = (creditvalue + Convert.ToDecimal(Credit));
                //            isupdated = true;

                //        }
                //        else if (Convert.ToString(item[1]).Trim() == AccountHeadID.Trim() &&
                //          Convert.ToString(item[2]).Trim() == AccountControlID.Trim() &&
                //          Convert.ToString(item[3]).Trim() == AccountSubControlID.Trim() &&
                //          debetvalue > 0)
                //        {
                //            item[7] = (debetvalue + Convert.ToDecimal(Debit));
                //            isupdated = true;
                //        }
                //    }

                //    if (isupdated == false)
                //    {
                //        dtEntries.Rows.Add(
                //        FinancialYearID,
                //        AccountHeadID,
                //        AccountControlID,
                //        AccountSubControlID,
                //        InvoiceNo,
                //        UserID,
                //        Credit,
                //        Debit,
                //        TransactionDate,
                //        TransectionTitle);

                //    }
                //  }
                //     }


            }
        }
    }
}
