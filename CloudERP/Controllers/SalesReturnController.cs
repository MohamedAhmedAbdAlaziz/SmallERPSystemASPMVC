using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class SalesReturnController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
      //  private PurchaseEntry purchaseEntry = new PurchaseEntry();
        // GET: SalesReturn
        public ActionResult Index()
        {
            return View();
        }


      

        //SP_Purchase purchase = new SP_Purchase();
        //private PurchaseEntry paymententry = new PurchaseEntry();
        // GET: PurchasesReturn
        public ActionResult FindSale()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            // var purchaseinvoice = db.tblSupplierInvoices.Find(0);
            tblCustomerInvoice invoice;
            if (Session["InvoiceNo"] != null)
            {
                var inviceno = Convert.ToString(Session["InvoiceNo"]);
                if (!string.IsNullOrEmpty(inviceno))
                {
                    invoice = db.tblCustomerInvoices.Where(p => p.InvoiceNo == inviceno.Trim()).FirstOrDefault();
                }
                else
                {
                    invoice = db.tblCustomerInvoices.Find(0);
                }
            }
            else
            {
                invoice = db.tblCustomerInvoices.Find(0);

            }
            return View(invoice);
        }
        [HttpPost]
        public ActionResult FindSale(string inviceid)
        {
            Session["InvoiceNo"] = string.Empty;
            Session["ReturnMessage"] = string.Empty;
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            
            var purchaseinvoice = db.tblCustomerInvoices.Where(p => p.InvoiceNo == inviceid).FirstOrDefault<tblCustomerInvoice>();

            return View(purchaseinvoice);
        }
    }
}