using CloudERP.Models;
using DatabaseAccess;
using DatabaseAccess.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CloudERP.Controllers
{
    public class PurchaseCartController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        private PurchaseEntry purchaseEntry = new PurchaseEntry();
        // GET: PurchaseCart
        public ActionResult NewPurchase( string ss =null)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Message = ss;
            int companyid = 0;
            double totalamount = 0;
            int branchid = 0;
           int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);
            foreach(var item in find)
            {
                totalamount += (item.PurchaseQuantity * item.purchaseUnitPrice);
            }
            ViewBag.TotalAmount = totalamount;
            return View(find.ToList());
        }
        //public ActionResult AddItem()
        //{

        //    if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    return View();
        //}
        [HttpPost]
        public ActionResult AddItem(int PID, int Qty, float Price  )
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
            //tblCategory.BranchID = branchid;
            //tblCategory.UserID = userid;
            //tblCategory.CompanyID = companyid;
            var find = db.tblPurchaseCartDetails.Where(i => i.ProductID == PID && i.BranchID == branchid && i.CompanyID == companyid).FirstOrDefault();
            if (find == null)
            {
                if (PID > 0 && Qty > 0 && Price > 0)
                {
                    var newitem = new tblPurchaseCartDetail()
                    {
                        ProductID = PID,
                        PurchaseQuantity = Qty,
                        purchaseUnitPrice = Price,
                        BranchID = branchid,
                        CompanyID = companyid,
                        UserID = userid
                    };
                    db.tblPurchaseCartDetails.Add(newitem);
                    db.SaveChanges();
                    ViewBag.Message = "Item Add Successfully !";
                }
            }
            else
            {
                ViewBag.Message = "Already Exist !";
            }

          


            return RedirectToAction("NewPurchase", new { ss = ViewBag.Message });
        }
        public ActionResult GetProduct()
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
            List<ProductMV> list = new List<ProductMV>();
            var productlist = db.tblStocks.Where(i => i.BranchID == branchid && i.CompanyID == companyid ).ToList();
            foreach(var item in productlist)
            {
                list.Add(new ProductMV { Name = item.ProductName, ProductID = item.ProductID });
            }
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }


        
      

public ActionResult DeleteConfirm(int? id)

{

    if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))

    {

        return RedirectToAction("Login", "Home");

    }

    int companyid = 0;

    int branchid = 0;

    int userid = 0;

    branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));

    companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));

    userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

    var product = db.tblPurchaseCartDetails.Find(id);

    if (product != null)

    {

        db.Entry(product).State = System.Data.Entity.EntityState.Deleted;

        db.SaveChanges();

        ViewBag.Message = "Deleted Successfully.";

        return RedirectToAction("NewPurchase", new { ss = ViewBag.Message });

    }

    ViewBag.Message = "Some Unexptected issue is occure, please contact to concern person!";

    var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);

    return View(find.ToList());

       }
        public ActionResult CancelPurchase()

        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))

            {

                return RedirectToAction("Login", "Home");

            }

            int companyid = 0;

            int branchid = 0;

            int userid = 0;

            branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));

            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));

            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var list = db.tblPurchaseCartDetails.Where(p => p.BranchID == branchid && p.CompanyID == companyid && p.UserID == userid).ToList();

            bool cancelstatus = false;

            foreach (var item in list)

            {

                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;

                int noofrecords = db.SaveChanges();

                if (cancelstatus == false)

                {

                    if (noofrecords > 0)

                    {

                        cancelstatus = true;

                    }

                }

            }

            if (cancelstatus == true)

            {

                ViewBag.Message = "Purchase is Canceled.";

                return RedirectToAction("NewPurchase", new { ss = ViewBag.Message });

            }

            ViewBag.Message = "Some Unexptected issue is occure, please contact to concern person!";

            ///   var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);
            return RedirectToAction("NewPurchase");
        //    return View(find.ToList());

        }

        //public ActionResult Finilize()
        //{
        //    return View();
        //}
        //[HttpPost]
   
       public ActionResult SelectSupplier(tblSupplier purchase)
        {
            Session["ErrorMessagePurchase"] = string.Empty;
          
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
            var checkpurchasecart = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid).FirstOrDefault();
            if (checkpurchasecart == null)
            {
             Session["ErrorMessagePurchase"] = "Purchase Cart Emplty!";
                return RedirectToAction("NewPurchase", new { ss = (string)Session["ErrorMessagePurchase"] });
            }
            

            //var purchasedetails = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();
            //if (purchasedetails.Count == 0)
            //{
            //    ViewBag.Message = "Please Cart Cart Emplty!";
            //    return View("NewPurchase");
            //}


            var suppliers = db.tblSuppliers.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();
            return View(suppliers);
            
        }

        [HttpPost]
        public ActionResult PurchaseConfirm(FormCollection collection)
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
            //string[] SupplierID = collection["item.SupplierID"].Split(',');
            //string[] Description = collection["item.Description"].Split(',');
            //string[] Stutus = collection["item.Stutus"].Split(',');
            //
            int supplierid = 0;
            bool IsPayment = false;
            string[] keys = collection.AllKeys;
            foreach (var name in keys)
            {
                if (name.Contains("name"))
                {
                    string idname = name;
                    string[] valueids = idname.Split(' ');
                    supplierid = Convert.ToInt32(valueids[1]);
                }
            }
            //   supplierid = Convert.ToInt32(collection.AllKeys[5]);
            try
            {
                supplierid = Convert.ToInt32(collection.AllKeys[4]);
            }
            catch
            {
                supplierid = Convert.ToInt32(collection.AllKeys[3]);
            }
            
           
           // supplierid = Convert.ToInt32(collection[4]);
            
            string Description = string.Empty;
            string[] Descriptionlist = collection["item.Description"].Split(',');
            if (Descriptionlist != null)
            {
                if (Descriptionlist[0] != null)
                {
                    Description = Descriptionlist[0];
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
            var supplier = db.tblSuppliers.Find(supplierid);
            var purchasedetails= db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();
            double totalamount = 0;
            foreach (var item in purchasedetails)
            {
                totalamount = totalamount + (item.PurchaseQuantity * item.purchaseUnitPrice);

            }
            if (totalamount == 0)
            {
                ViewBag.Message = "Purchase Cart Empty!";
                return View("NewPurchase", new { ss = ViewBag.Message });

            }

            string Invoiceno = "PUR" + DateTime.Now.ToString("yyyyMMddHHmmss")+DateTime.Now.Millisecond;
            var invoiceheader = new tblSupplierInvoice()
            {
                BranchID = branchid,
                CompanyID = companyid,
                Description = Description,
                InvoiceDate = DateTime.Now,
                InvoiceNo = Invoiceno,
                SupplierID = supplierid,
                UserID = userid
               ,TotalAmount=totalamount

            };
            db.tblSupplierInvoices.Add(invoiceheader);
            db.SaveChanges();
            foreach(var item in purchasedetails)
            {
                var purdetails = new tblSupplierInvoiceDetail()
                {
                    ProductID = item.ProductID,
                    PurchaseQuantity = item.PurchaseQuantity,
                    purchaseUnitPrice = item.purchaseUnitPrice,
                    SupplierInvoiceID = invoiceheader.SupplierInvoiceID
                };
                db.tblSupplierInvoiceDetails.Add(purdetails);
                db.SaveChanges();
            }
             string Message=    purchaseEntry.ConfrimPurchase(companyid, branchid, userid, Invoiceno, invoiceheader.SupplierInvoiceID.ToString(),(float) totalamount, supplierid.ToString(), supplier.SupplierName, IsPayment);
            if (Message.Contains("Success"))
            {
                foreach (var item in purchasedetails)
                {
                    var stockitem = db.tblStocks.Find(item.ProductID);
                    stockitem.CurrentPurchaseUnitPrice = item.purchaseUnitPrice;
                    stockitem.Quantity = stockitem.Quantity + item.PurchaseQuantity;    
                    db.Entry(stockitem).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

            }
           Session["Message"] = Message;
            return RedirectToAction("NewPurchase", new { ss = (string)Session["Message"] });

        }


    }
}