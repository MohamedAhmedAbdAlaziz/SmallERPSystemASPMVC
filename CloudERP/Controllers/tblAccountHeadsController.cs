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
    public class tblAccountHeadsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblAccountHeads
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


            //var tblAccountHeads = db.tblAccountHeads.Include(t => t.tblUser).Where(c => c.CompanyID == companyid && c.BranchID == branchid);
            var tblAccountHeads = db.tblAccountHeads.Include(t => t.tblUser); 
            return View(tblAccountHeads.ToList());
        }

        // GET: tblAccountHeads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountHead tblAccountHead = db.tblAccountHeads.Find(id);
            if (tblAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountHead);
        }

        // GET: tblAccountHeads/Create
        public ActionResult Create()
        {
           // ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            return View();
        }

        // POST: tblAccountHeads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAccountHead tblAccountHead)
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
           // tblAccountHead.BranchID = branchid;
            tblAccountHead.UserID = userid;
          //  tblAccountHead.CompanyID = companyid;


            if (ModelState.IsValid)
            {
                //var findhead = db.tblAccountHeads.Where(c => c.CompanyID == companyid && c.BranchID == branchid && c.AccountHeadName== tblAccountHead.AccountHeadName).FirstOrDefault();
                var findhead = db.tblAccountHeads.Where(c =>  c.AccountHeadName== tblAccountHead.AccountHeadName).FirstOrDefault();
                if (findhead == null)
                {
                    db.tblAccountHeads.Add(tblAccountHead);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }
                }

        //    ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblAccountHead.UserID);
            return View(tblAccountHead);
        }

        // GET: tblAccountHeads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountHead tblAccountHead = db.tblAccountHeads.Find(id);
            if (tblAccountHead == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblAccountHead.UserID);
            return View(tblAccountHead);
        }

        // POST: tblAccountHeads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblAccountHead tblAccountHead)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

         //   int companyid = 0;
         //   int branchid = 0;
            int userid = 0;
            //companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            //branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
           // tblAccountHead.BranchID = branchid;
            tblAccountHead.UserID = userid;
          //  tblAccountHead.CompanyID = companyid;


            if (ModelState.IsValid)
            {
                //var findhead = db.tblAccountHeads.Where(c => c.CompanyID == tblAccountHead.CompanyID && c.BranchID == tblAccountHead.BranchID && c.AccountHeadName == tblAccountHead.AccountHeadName&&c.AccountHeadID!= tblAccountHead.AccountHeadID ).FirstOrDefault();
                var findhead = db.tblAccountHeads.Where(c => c.AccountHeadName == tblAccountHead.AccountHeadName&&c.AccountHeadID!= tblAccountHead.AccountHeadID ).FirstOrDefault();
                if (findhead == null)
                {
                    db.Entry(tblAccountHead).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }
            }
        

          //  ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblAccountHead.UserID);
            return View(tblAccountHead);
        }

        // GET: tblAccountHeads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountHead tblAccountHead = db.tblAccountHeads.Find(id);
            if (tblAccountHead == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountHead);
        }

        // POST: tblAccountHeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAccountHead tblAccountHead = db.tblAccountHeads.Find(id);
            db.tblAccountHeads.Remove(tblAccountHead);
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
