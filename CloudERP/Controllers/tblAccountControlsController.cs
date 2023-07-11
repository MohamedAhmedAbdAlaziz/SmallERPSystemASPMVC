using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CloudERP.Models;
using DatabaseAccess;

namespace CloudERP.Controllers
{
    public class tblAccountControlsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        private List<AccountControlMV> accountControlMVs = new List<AccountControlMV>();
        // GET: tblAccountControls
        public ActionResult Index()
        {
            accountControlMVs.Clear();
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
            var tblAccountControls = db.tblAccountControls.Include(t => t.tblBranch).Include(t => t.tblUser).Include(t => t.tblCompany).Where(c => c.CompanyID == companyid && c.BranchID == branchid);
            foreach(var item in tblAccountControls)
            {
                accountControlMVs.Add(new AccountControlMV
                {
                    AccountControlID = item.AccountControlID,
                    AccountControlName = item.AccountControlName,
                    AccountHeadID = item.AccountHeadID,
                    AccountHeadName = db.tblAccountHeads.Find(item.AccountHeadID).AccountHeadName,
                    BranchID=item.BranchID,
                    BeanchName=item.tblBranch.BranchName,
                    CompanyID=item.CompanyID,
                    Name= item.tblCompany.Name,
                    UserID=item.UserID,
                    UserName=item.tblUser.FullName
                });
            }


            //return View(tblAccountControls.ToList());
            return View(accountControlMVs.ToList());
        }

        // GET: tblAccountControls/Details/5
        public ActionResult Details(int? id)
        {
            accountControlMVs.Clear();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            if (tblAccountControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountControl);
        }

        // GET: tblAccountControls/Create
        public ActionResult Create()
        {
          //  accountControlMVs.Clear();
            //ViewBag.AccountHeadID = new SelectList(db.tblAccountControls, "AccountControlID", "AccountControlName");

            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName" );
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name");
            return View();
        }

        // POST: tblAccountControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( tblAccountControl tblAccountControl)
        {
            accountControlMVs.Clear();

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
            tblAccountControl.BranchID = branchid;
            tblAccountControl.UserID = userid;
            tblAccountControl.CompanyID = companyid;

            if (ModelState.IsValid)
            {
                var findControl= db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid && c.AccountControlName == tblAccountControl.AccountControlName).FirstOrDefault();
                if (findControl == null)
                {
                    db.tblAccountControls.Add(tblAccountControl);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }
                
            }
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName"); 

            return View(tblAccountControl);
        }

        // GET: tblAccountControls/Edit/5
        public ActionResult Edit(int? id)
        {
            //accountControlMVs.Clear();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            if (tblAccountControl == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountControl.AccountHeadID);

            return View(tblAccountControl);
        }

        // POST: tblAccountControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tblAccountControl tblAccountControl)
        {
           // accountControlMVs.Clear();
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //int companyid = 0;
            //int branchid = 0;
        
            //companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            //branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"])); 
            //tblAccountControl.CompanyID = companyid;
            //tblAccountControl.BranchID = branchid;
            int userid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblAccountControl.UserID = userid;
            

        
          

            if (ModelState.IsValid)
            {
                var findControl = db.tblAccountControls.Where(c => c.CompanyID == tblAccountControl.CompanyID && c.BranchID == tblAccountControl.BranchID && c.AccountControlName == tblAccountControl.AccountControlName&&c.AccountControlID==tblAccountControl.AccountControlID).FirstOrDefault();
                if (findControl == null)
                {
                    db.Entry(tblAccountControl).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }

            }
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountControl.AccountHeadID);

            //ViewBag.AccountControlID = new SelectList(db.tblAccountControls, "AccountControlID", "AccountControlName", tblAccountControl.AccountHeadID);

            return View(tblAccountControl);
        }

        // GET: tblAccountControls/Delete/5
        public ActionResult Delete(int? id)
        {
            accountControlMVs.Clear();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            if (tblAccountControl == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountControl);
        }

        // POST: tblAccountControls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAccountControl tblAccountControl = db.tblAccountControls.Find(id);
            db.tblAccountControls.Remove(tblAccountControl);
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
