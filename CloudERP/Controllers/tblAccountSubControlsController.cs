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
    public class tblAccountSubControlsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblAccountSubControls
        public ActionResult Index()
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


            var tblAccountSubControls = db.tblAccountSubControls.Include(t => t.tblAccountControl).
                Include(t => t.tblAccountHead).Include(t => t.tblBranch).Include(t => t.tblUser).Where(c => c.CompanyID == companyid && c.BranchID == branchid);
            return View(tblAccountSubControls.ToList());
        }

        // GET: tblAccountSubControls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            if (tblAccountSubControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSubControl);
        }

        // GET: tblAccountSubControls/Create
        public ActionResult Create()
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

            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
             
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName","0");
           // ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName");
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            return View();
        }

        // POST: tblAccountSubControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( tblAccountSubControl tblAccountSubControl)
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
            tblAccountSubControl.BranchID = branchid;
            tblAccountSubControl.UserID = userid;
            tblAccountSubControl.CompanyID = companyid;
            tblAccountSubControl.AccountHeadID = db.tblAccountControls.Find(tblAccountSubControl.AccountControlID).AccountHeadID;
            if (ModelState.IsValid)
            {
                var findControl = db.tblAccountSubControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid && c.AccountSubControlName==tblAccountSubControl.AccountSubControlName).FirstOrDefault();
                if (findControl == null)
                {
                    db.tblAccountSubControls.Add(tblAccountSubControl);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }


            }

            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", tblAccountSubControl.AccountControlID);
            //ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSubControl.AccountHeadID);
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblAccountSubControl.BranchID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblAccountSubControl.UserID);
            return View(tblAccountSubControl);
        }

        // GET: tblAccountSubControls/Edit/5
        public ActionResult Edit(int? id)
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            if (tblAccountSubControl == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", tblAccountSubControl.AccountControlID);
           // ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSubControl.AccountHeadID);
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblAccountSubControl.BranchID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblAccountSubControl.UserID);
            return View(tblAccountSubControl);
        }

        // POST: tblAccountSubControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAccountSubControl tblAccountSubControl)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            int branchid = 0;
         //   int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
           // userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            
           
            int userid = 0;
             userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
             tblAccountSubControl.UserID = userid;
             tblAccountSubControl.AccountHeadID = db.tblAccountControls.Find(tblAccountSubControl.AccountControlID).AccountHeadID;
            if (ModelState.IsValid)
            {
                var findControl = db.tblAccountSubControls.Where(c => c.CompanyID == tblAccountSubControl.CompanyID && c.BranchID == tblAccountSubControl.BranchID && c.AccountSubControlName == tblAccountSubControl.AccountSubControlName&& c.AccountSubControlID != tblAccountSubControl.AccountSubControlID).FirstOrDefault();
                if (findControl == null)
                {
                    db.Entry(tblAccountSubControl).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }


            }

            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", tblAccountSubControl.AccountControlID);
            //ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSubControl.AccountHeadID);
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblAccountSubControl.BranchID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblAccountSubControl.UserID);
            return View(tblAccountSubControl);
            
        }

        // GET: tblAccountSubControls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            if (tblAccountSubControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSubControl);
        }

        // POST: tblAccountSubControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAccountSubControl tblAccountSubControl = db.tblAccountSubControls.Find(id);
            db.tblAccountSubControls.Remove(tblAccountSubControl);
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
