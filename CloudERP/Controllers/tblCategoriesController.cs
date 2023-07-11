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
    public class tblCategoriesController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblCategories
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

           var tblCategories = db.tblCategories.Include(t => t.tblBranch).Include(t => t.tblUser).Include(t => t.tblCompany).Where(c=>c.CompanyID==companyid&& c.BranchID==branchid);
            return View(tblCategories.ToList());
        }

        // GET: tblCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // GET: tblCategories/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name");
           
            return View();
        }

        // POST: tblCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( tblCategory tblCategory)
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
            tblCategory.BranchID = branchid;
            tblCategory.UserID = userid;
            tblCategory.CompanyID = companyid;
            if (ModelState.IsValid)
            {
                var findcategory = db.tblCategories.Where(c => c.CompanyID == companyid && c.BranchID == branchid && c.categoryName == tblCategory.categoryName).FirstOrDefault();
             if (findcategory==null)
                {
                    db.tblCategories.Add(tblCategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already Exist !";
                }
                
            }

            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblCategory.BranchID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblCategory.UserID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblCategory.CompanyID);
            return View(tblCategory);
        }

        // GET: tblCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblCategory.BranchID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblCategory.UserID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblCategory.CompanyID);
            return View(tblCategory);
        }

        // POST: tblCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(  tblCategory tblCategory)
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
            //tblCategory.BranchID = branchid;
            //tblCategory.BranchID = branchid;

            tblCategory.UserID = userid;
            if (ModelState.IsValid)
            {
                var findcategory = db.tblCategories.Where(c => c.CompanyID == tblCategory.CompanyID && c.BranchID == tblCategory.BranchID && c.categoryName == tblCategory.categoryName&& c.CategoryID !=tblCategory.CategoryID).FirstOrDefault();
                if (findcategory == null)
                {
                    db.Entry(tblCategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
                else
                {
                    ViewBag.Message = "Already Exist !";
                }
   
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblCategory.BranchID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblCategory.UserID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblCategory.CompanyID);
            return View(tblCategory);
        }

        // GET: tblCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // POST: tblCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            db.tblCategories.Remove(tblCategory);
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
