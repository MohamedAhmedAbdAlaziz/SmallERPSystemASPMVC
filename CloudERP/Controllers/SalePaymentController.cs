using DatabaseAccess;
using DatabaseAccess.Code;
using DatabaseAccess.Code.SP_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class SalePaymentController : Controller
    {
        // GET: SalePayment
        private CloudErpV1Entities db = new CloudErpV1Entities();
        SP_Sale sale = new SP_Sale();
        private SaleEntry saleentry = new SaleEntry();
        // GET: salePayment
        public ActionResult RemainingPaymentList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var list = sale.ReamaningPaymentList(companyid, branchid);

            return View(list.ToList());
        }

        public ActionResult PaidHistory(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])) || string.IsNullOrEmpty(Convert.ToString(id)))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            //string payinvoicenno = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
            //var supplier = db.tblSuppliers.Find(db.tblSupplierInvoices.Find(id).SupplierID);
            //var saleinvoice = db.tblSupplierInvoices.Find(id);

            //var salepaymentdetails = db.tblSupplierPayments.Where(p => p.SupplierInvoiceID == id);

            var salepaymentdetails = sale.SalePaymentHistory((int)id);
            return View(salepaymentdetails.ToList());
        }
        public ActionResult PaidAmount(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])) || string.IsNullOrEmpty(Convert.ToString(id)))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            //string payinvoicenno = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
            //var supplier = db.tblSuppliers.Find(db.tblSupplierInvoices.Find(id).SupplierID);
            //var saleinvoice = db.tblSupplierInvoices.Find(id);

            //var salepaymentdetails = db.tblSupplierPayments.Where(p => p.SupplierInvoiceID == id);

            var salepaymentdetails = sale.SalePaymentHistory((int)id);
            double reaminingamount = 0;
            foreach (var item in salepaymentdetails)
            {
                reaminingamount = item.RemainingBalance;
            }
            if (reaminingamount == 0)
            {
                reaminingamount = db.tblCustomerInvoices.Find(id).TotalAmount;
            }
            ViewBag.PreviousRemaining = reaminingamount;
            ViewBag.InvoiceID = id;
            return View(salepaymentdetails.ToList());
        }
        [HttpPost]
        public ActionResult PaidAmount(int? id, float previousremainingamount, float paidamount)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])) || string.IsNullOrEmpty(Convert.ToString(id)))
                {
                    return RedirectToAction("Login", "Home");
                }


                int companyid = 0;
                int branchid = 0;
                int userid = 0;
                companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
                branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
                userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
                if (paidamount > previousremainingamount)
                {
                    ViewBag.Message = "Payment muat be less or Eqaul to Previous Rwmaining Amount";
                    var list = sale.SalePaymentHistory((int)id);
                    double reaminingamount = 0;
                    foreach (var item in list)
                    {
                        reaminingamount = item.RemainingBalance;
                    }
                    if (reaminingamount == 0)
                    {
                        reaminingamount = db.tblCustomerInvoices.Find(id).TotalAmount;
                    }
                    ViewBag.PreviousRemaining = reaminingamount;
                    ViewBag.InvoiceID = id;

                    return View(list);
                }

                string payinvoicenno = "INP" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
                var customer = db.tblCustomers.Find(db.tblCustomerInvoices.Find(id).CustomerID);
                var saleinvoice = db.tblCustomerInvoices.Find(id);

                var salepaymentdetails = db.tblCustomerPayments.Where(p => p.CustomerInvoiceID == id);
                string message = saleentry.SalePayment(companyid, branchid, userid, payinvoicenno,
                    Convert.ToString(id), (float)saleinvoice.TotalAmount, paidamount, Convert.ToString(customer.CustomerID), customer.Customername, (previousremainingamount - paidamount));
                   Session["Message"] = message;
                return RedirectToAction("RemainingPaymentList");
            }
            catch (Exception)
            {

                ViewBag.Message = "Please try again";
                var list = sale.SalePaymentHistory((int)id);
                double reaminingamount = 0;
                foreach (var item in list)
                {
                    reaminingamount = item.RemainingBalance;
                }
                if (reaminingamount == 0)
                {
                    reaminingamount = db.tblCustomerInvoices.Find(id).TotalAmount;
                }
                ViewBag.PreviousRemaining = reaminingamount;
                ViewBag.InvoiceID = id;

                return View(list);
            }
        }
        public ActionResult CustomSalesHistory(DateTime FromDate, DateTime ToDate)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var list = sale.CustomSaleList(companyid, branchid, FromDate, ToDate);

            return View(list.ToList());
        }

        public ActionResult SaleItemDetail(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var list = db.tblCustomerInvoiceDetails.Where(d => d.CustomerInvoiceID == id);
            
            return View(list.ToList());
        }

    }
}