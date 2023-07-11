using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess;

namespace CloudERP.Controllers
{
    public class tblStocksController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblStocks
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            var tblStocks = db.tblStocks.Include(t => t.tblBranch).Include(t => t.tblCategory).Include(t => t.tblUser).Include(t => t.tblCompany).Where(c => c.CompanyID == companyid && c.BranchID == branchid);

            return View(tblStocks.ToList());
        }

        // GET: tblStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            return View(tblStock);
        }

        // GET: tblStocks/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
              companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
           

            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c=>c.BranchID==branchid&&c.CompanyID==companyid), "CategoryID", "categoryName","0");
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name");
            return View();
        }

        // POST: tblStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(  tblStock tblStock)
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
            tblStock.BranchID = branchid;
            tblStock.UserID = userid;
            tblStock.CompanyID = companyid;


            if (ModelState.IsValid)
            {
                var findProduct = db.tblStocks.Where(c => c.CompanyID == companyid && c.BranchID == branchid && c.ProductName == tblStock.ProductName).FirstOrDefault();
                if (findProduct == null)
                {
                    db.tblStocks.Add(tblStock);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }

                    }

           // ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblStock.BranchID);
            ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == branchid && c.CompanyID == companyid), "CategoryID", "categoryName", tblStock.CategoryID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblStock.UserID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblStock.CompanyID);
            return View(tblStock);
        }

        // GET: tblStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == branchid && c.CompanyID == companyid), "CategoryID", "categoryName", "0");
         //   ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == tblStock.BranchID && c.CompanyID == tblStock.BranchID), "CategoryID", "categoryName","0");
          ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == tblStock.BranchID && c.CompanyID == tblStock.CompanyID), "CategoryID", "categoryName", tblStock.CategoryID);
              return View(tblStock);
        }

        // POST: tblStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tblStock tblStock)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //int companyid = 0;
            //int branchid = 0;
            int userid = 0;
            //companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            //branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            //tblStock.BranchID = branchid;      
            //tblStock.CompanyID = companyid;

            tblStock.UserID = userid;
            if (ModelState.IsValid)
            {
                //var findProduct = db.tblStocks.Where(c => c.CompanyID == tblStock.CompanyID && c.BranchID == tblStock.BranchID && c.ProductName == tblStock.ProductName&& c.ProductID==tblStock.ProductID).FirstOrDefault();
                //if (findProduct == null)
                //{
                 db.Entry(tblStock).State = EntityState.Modified;
                 //   db.Entry(tblStock).State = EntityState.Detached;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                //}
             
                //else
                //{
                //    ViewBag.Message = "Already is Exist !";
                //}

            }
            
             ViewBag.CategoryID = new SelectList(db.tblCategories.Where(c => c.BranchID == tblStock.BranchID && c.CompanyID == tblStock.CompanyID), "CategoryID", "categoryName", tblStock.CategoryID);
             return View(tblStock);
        }

        // GET: tblStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStock tblStock = db.tblStocks.Find(id);
            if (tblStock == null)
            {
                return HttpNotFound();
            }
            return View(tblStock);
        }

        // POST: tblStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblStock tblStock = db.tblStocks.Find(id);
            db.tblStocks.Remove(tblStock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
