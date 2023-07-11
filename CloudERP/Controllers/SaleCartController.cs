using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess.Code;
using CloudERP.Models;

namespace CloudERP.Controllers
{

    public class SaleCartController : Controller
    {

     //   public string Title=null;

        private CloudErpV1Entities db = new CloudErpV1Entities();
        private SaleEntry saleentry = new SaleEntry();
        // GET: SaleCart
        public ActionResult NewSale(string ss= null )
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            double totalamount = 0;
            int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var find = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);
            ViewBag.Message = ss ;
            foreach (var item in find)
            {
                totalamount += (item.SaleQuantity * item.SaleUnitPrice);
            }
            ViewBag.TotalAmount = totalamount;
            return View(find.ToList());

        }

        public ActionResult AddItem(int PID, int Qty, float Price)
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
            var checkqty = db.tblStocks.Find(PID);
            if (Qty > checkqty.Quantity)
            {
                ViewBag.Message = "Sale Quantity must be less then are equal to avl Qty.";
                //ViewBag.Message = "Sale Quantity must be less then are equal to avl Qty.";
                return RedirectToAction("NewSale", new {ss= ViewBag.Message });

            }
            var find = db.tblSaleCartDetails.Where(i => i.ProductID == PID && i.BranchID == branchid && i.CompanyID == companyid).FirstOrDefault();
            if (find == null)
            {
                if (PID > 0 && Qty > 0 && Price > 0)
                {
                    var newitem = new tblSaleCartDetail()
                    {
                        ProductID = PID,
                        SaleQuantity = Qty,
                        SaleUnitPrice = Price,
                        BranchID = branchid,
                        CompanyID = companyid,
                        UserID = userid
                    };
                    db.tblSaleCartDetails.Add(newitem);
                    db.SaveChanges();
                    ViewBag.Message = "Item Add Successfully !";
                }
            }
            else
            {
                ViewBag.Message = "Already Exist !";
            }




            return RedirectToAction("NewSale", new { ss = ViewBag.Message });
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

            var product = db.tblSaleCartDetails.Find(id);

            if (product != null)

            {

                db.Entry(product).State = System.Data.Entity.EntityState.Deleted;

                db.SaveChanges();

                ViewBag.Message = "Deleted Successfully.";

                return RedirectToAction("NewSale", new { ss = ViewBag.Message });

            }

            ViewBag.Message = "Some Unexptected issue is occure, please contact to concern person!";

            var find = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);

            return View(find.ToList());

        }
        [HttpGet]
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
            var productlist = db.tblStocks.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();
            foreach (var item in productlist)
            {
                list.Add(new ProductMV { Name = item.ProductName + "(Avl Qty : " + item.Quantity + " )", ProductID = item.ProductID });
            }
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetProductDetails(int? id)
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
            var product = db.tblStocks.Find(id);

            return Json(new { data = product.SaleUnitPrice }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelSale()

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

            var list = db.tblSaleCartDetails.Where(p => p.BranchID == branchid && p.CompanyID == companyid && p.UserID == userid).ToList();

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

                ViewBag.Message = "Sale is Canceled.";

                return RedirectToAction("NewSale",new { ss = ViewBag.Message });

            }

            ViewBag.Message = "Some Unexptected issue is occure, please contact to concern person!";

            ///   var find = db.tblPurchaseCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid && i.UserID == userid);
            return RedirectToAction("NewSale", new { ss = ViewBag.Message });
            //    return View(find.ToList());

        }
        public ActionResult SelectCustomer(tblSupplier purchase)
        {
            Session["ErrorMessageSale"] = string.Empty;
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
            var checksaledetails = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid).FirstOrDefault();
            if (checksaledetails == null)
            {
                Session["ErrorMessageSale"] = "Please Sale Emplty!";
                return RedirectToAction("NewSale", new { ss = (string)Session["ErrorMessageSale"] });
            }

            //var saledetails = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();
            //if (saledetails.Count == 0)
            //{
            //    ViewBag.Message = "Please Sale Emplty!";
            //    return View("NewSale");
            //}


            var Customers = db.tblCustomers.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();

            return View(Customers);

        }
        [HttpPost]
        public ActionResult SaleConfirm(FormCollection collection)
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
            int customerid = 0;
            bool IsPayment = false;
            string[] keys = collection.AllKeys;
            foreach (var name in keys)
            {
                if (name.Contains("name"))
                {
                    string idname = name;
                    string[] valueids = idname.Split(' ');
                    customerid = Convert.ToInt32(valueids[1]);
                }
            }
            //   supplierid = Convert.ToInt32(collection.AllKeys[5]);
            try
            {
                customerid = Convert.ToInt32(collection.AllKeys[4]);
            }
            catch
            {
                customerid = Convert.ToInt32(collection.AllKeys[3]);
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
            var customer = db.tblCustomers.Find(customerid);
            var saledetails = db.tblSaleCartDetails.Where(i => i.BranchID == branchid && i.CompanyID == companyid).ToList();
            double totalamount = 0;
            foreach (var item in saledetails)
            {
                totalamount = totalamount + (item.SaleQuantity * item.SaleUnitPrice);

            }
            if (totalamount == 0)
            {
                ViewBag.Message = "Sale Cart Empty!";
                return View("NewSale", new { ss = ViewBag.Message });

            }

            string Invoiceno = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
            var invoiceheader = new tblCustomerInvoice()
            {
                BranchID = branchid,
                Title = "Sale Invoice" + customer.Customername,
                CompanyID = companyid,
                Description = Description,
                InvoiceDate = DateTime.Now,
                InvoiceNo = Invoiceno,
                CustomerID = customerid,
                UserID = userid
               ,
                TotalAmount = totalamount

            };
            db.tblCustomerInvoices.Add(invoiceheader);
            db.SaveChanges();
            foreach (var item in saledetails)
            {
                var saledetail = new tblCustomerInvoiceDetail()
                {
                    ProductID = item.ProductID,
                    SaleQuantity = item.SaleQuantity,
                    SaleUnitPrice = item.SaleUnitPrice,
                    CustomerInvoiceID = invoiceheader.CustomerInvoiceID
                };
                db.tblCustomerInvoiceDetails.Add(saledetail);
                db.SaveChanges();
            }
            string Message = saleentry.ConfrimSale(companyid, branchid, userid, Invoiceno, invoiceheader.CustomerInvoiceID.ToString(), (float)totalamount, customerid.ToString(), customer.Customername, IsPayment);
            if (Message.Contains("Success"))
            {
                foreach (var item in saledetails)
                {
                    var stockitem = db.tblStocks.Find(item.ProductID);
                    //    stockitem.CurrentPurchaseUnitPrice = item.purchaseUnitPrice;
                    stockitem.Quantity = stockitem.Quantity - item.SaleQuantity;
                    db.Entry(stockitem).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

            }
            Session["Message"] = Message;
            return RedirectToAction("NewSale", new { ss = (string)Session["Message"] });
             


        }

    }
}