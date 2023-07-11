using DatabaseAccess;
using DatabaseAccess.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class PurchasesReturnController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        private PurchaseEntry purchaseEntry = new PurchaseEntry();

        //SP_Purchase purchase = new SP_Purchase();
        //private PurchaseEntry paymententry = new PurchaseEntry();
        // GET: PurchasesReturn
        public ActionResult FindPurchase()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            // var purchaseinvoice = db.tblSupplierInvoices.Find(0);
            tblSupplierInvoice invoice;
            if(Session["InvoiceNo"] != null)
            {
                var inviceno = Convert.ToString(Session["InvoiceNo"]);
                if (!string.IsNullOrEmpty(inviceno))
                {
                    invoice = db.tblSupplierInvoices.Where(p => p.InvoiceNo == inviceno.Trim()).FirstOrDefault();
                }
                else
                {
                    invoice = db.tblSupplierInvoices.Find(0);
                }
            }
            else
            {
                invoice = db.tblSupplierInvoices.Find(0);

            }
            return View(invoice);
        }
        [HttpPost]
        public ActionResult FindPurchase(string inviceid)
        {
            Session["InvoiceNo"] = string.Empty;
            Session["ReturnMessage"] = string.Empty;
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var purchaseinvoice = db.tblSupplierInvoices.Where(p=>p.InvoiceNo == inviceid).FirstOrDefault<tblSupplierInvoice>();

            return View(purchaseinvoice);
        } 
        [HttpPost]
        public ActionResult ReturnConfirm(FormCollection collection)
        {
            Session["ReturnMessage"] = string.Empty;
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
            //string[] SupplierID = collection["item.SupplierID"].Split(',');
            //string[] Description = collection["item.Description"].Split(',');
            //string[] Stutus = collection["item.Stutus"].Split(',');
            //
            int supplierid = 0;
            int SupplierInvoiceID = 0;
            
            bool IsPayment = false;
            string[] keys = collection.AllKeys;
            List<int> ProductIDs = new List<int>();
            List<int> ReturnQuantity = new List<int>();
            foreach (var name in keys)
            {
                if (name.Contains("ProductID"))
                {
                    string idname = name;
                    string[] valueids = idname.Split(' ');
                   // supplierid = Convert.ToInt32(valueids[1]);
                    ProductIDs.Add(Convert.ToInt32(valueids[1]));
                    ReturnQuantity.Add(Convert.ToInt32(collection[idname].Split(',')[0]));
                }
            }
            //   supplierid = Convert.ToInt32(collection.AllKeys[5]);
            //try
            //{
            //    supplierid = Convert.ToInt32(collection.AllKeys[4]);
            //}
            //catch
            //{
            //    supplierid = Convert.ToInt32(collection.AllKeys[3]);
            //}


            // supplierid = Convert.ToInt32(collection[4]);

            string Description = "Purchase Return";
            string[] SupplierInvoiceIDs = collection["supplierinvoiceid"].Split(',');
            if (SupplierInvoiceIDs != null)
            {
                if (SupplierInvoiceIDs[0] != null)
                {
                    SupplierInvoiceID = Convert.ToInt32( SupplierInvoiceIDs[0]);
                }
            }
            if (collection["IsPayment"] != null)
            {
                string[] ispaymentdirct = collection["IsPayment"].Split(',');
                if (ispaymentdirct[0] == "on")
                {
                    IsPayment = true;
                }
                else
                {
                    IsPayment = false;
                }
                //IsPayment = Convert.ToBoolean(collection["IsPayment"]);
                // IsPayment =  ((CheckBox)collection["IsPayment"]).Checked;
            }
            else
            {
                IsPayment = false;
            }
            double TotalAmount = 0;

            var purchasedetails = db.tblSupplierInvoiceDetails.Where(p => p.SupplierInvoiceID == SupplierInvoiceID).ToList();

            for (int i=0; i< purchasedetails.Count; i++ )
            {
                foreach ( int proid in ProductIDs)
                {
                    if(proid == purchasedetails[i].ProductID)
                    {
                        TotalAmount = TotalAmount + (ReturnQuantity[i] * purchasedetails[i].purchaseUnitPrice);

                    }
                }
                    }

            var supplierinvoice = db.tblSupplierInvoices.Find(SupplierInvoiceID);
            supplierid = supplierinvoice.SupplierID;
            if (TotalAmount == 0)
            {
                Session["InvoiceNo"] = supplierinvoice.InvoiceNo;
                Session["ReturnMessage"] = "Must be at least one product return qty! ";
                return RedirectToAction("FindPurchase");
            }


            string Invoiceno = "RPU" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;

            var returninvoiceheader = new tblSupplierReturnInvoice()
            {
                BranchID = branchid,
                CompanyID = companyid,
                Description = Description,
                InvoiceDate = DateTime.Now,
                InvoiceNo = Invoiceno,
                SupplierID = supplierid,
                UserID = userid   ,
                TotalAmount = TotalAmount,
                SupplierInvoiceID=SupplierInvoiceID

            };
            db.tblSupplierReturnInvoices.Add(returninvoiceheader);
            db.SaveChanges();


            //      var purchasedetails = db.tblSupplierInvoiceDetails.Where(p => p.SupplierInvoiceID == SupplierInvoiceID).ToList();
            var supplier = db.tblSuppliers.Find(supplierid);
            string Message = purchaseEntry.ReturnPurchase(companyid, branchid, userid, Invoiceno, returninvoiceheader.SupplierInvoiceID.ToString(), returninvoiceheader.SupplierReturnInvoiceID, (float)TotalAmount, supplierid.ToString(), supplier.SupplierName, IsPayment);
            if (Message.Contains("Success"))
            {

                for (int i = 0; i < purchasedetails.Count; i++)
                {
                    foreach (int proid in ProductIDs)
                    {
                        if (proid == purchasedetails[i].ProductID)
                        {

                            if (ReturnQuantity[i] > 0)
                            {
                                var rdp = new tblSupplierReturnInvoiceDetail();
                                rdp.SupplierInvoiceID = SupplierInvoiceID;
                                rdp.PurchaseReturnQuantity = ReturnQuantity[i];
                                rdp.ProductID = proid;
                                rdp.purchaseReturnUnitPrice = purchasedetails[i].purchaseUnitPrice;
                                rdp.SupplierReturnInvoiceID = returninvoiceheader.SupplierReturnInvoiceID;
                                rdp.SupplierInvoiceDetailID = purchasedetails[i].SupplierInvoiceDetailID;
                                db.tblSupplierReturnInvoiceDetails.Add(rdp);
                                db.SaveChanges();

                                var stock = db.tblStocks.Find(proid);
                                stock.Quantity = (stock.Quantity - ReturnQuantity[i]);
                                db.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();

                            }

                            //     TotalAmount = TotalAmount + (ReturnQuantity[i] * purchasedetails[i].purchaseUnitPrice);

                        }
                    }
                }
                Session["InvoiceNo"] = supplierinvoice.InvoiceNo;
                Session["ReturnMessage"] = "Return Successfully ";
            return RedirectToAction("FindPurchase");
            }


            Session["InvoiceNo"] = supplierinvoice.InvoiceNo;
            Session["ReturnMessage"] = "Some unexpected issue is occure plz contact to Adminstrator!"  ;
            return RedirectToAction("FindPurchase");
                
        }
    }
}