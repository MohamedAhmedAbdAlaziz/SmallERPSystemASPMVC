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
    public class tblAccountSettingsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblAccountSettings
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
            //tblAccountControl.BranchID = branchid;
            //tblAccountControl.UserID = userid;
            //tblAccountControl.CompanyID = companyid;
            var tblAccountSettings = db.tblAccountSettings.Include(t => t.tblAccountActivity).Include(t => t.tblAccountControl).Include(t => t.tblAccountHead).
                Include(t => t.tblAccountSubControl).Include(t => t.tblBranch).
                Include(t => t.tblCompany).Where(t=>t.CompanyID==companyid && t.BranchID==branchid);
            return View(tblAccountSettings.ToList());
        }

        // GET: tblAccountSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            if (tblAccountSetting == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSetting);
        }

        // GET: tblAccountSettings/Create
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

            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name","0");
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", "0");
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", "0");
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountSubControlID", "AccountSubControlName", "0");
    
            return View();
        }

        // POST: tblAccountSettings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblAccountSetting tblAccountSetting)
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
            tblAccountSetting.BranchID = branchid; 
            tblAccountSetting.CompanyID = companyid; 
      
            if (ModelState.IsValid)
            {
                var find = db.tblAccountSettings.Where(c => c.CompanyID == tblAccountSetting.CompanyID && c.BranchID == tblAccountSetting.BranchID && c.AccountActivityID==tblAccountSetting.AccountActivityID).FirstOrDefault();
                if (find == null)
                {
                    db.tblAccountSettings.Add(tblAccountSetting);

                    db.SaveChanges();
                    ViewBag.Message = "Save successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }

              
            }

            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", tblAccountSetting.AccountActivityID);
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", tblAccountSetting.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSetting.AccountHeadID);
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountSubControlID", "AccountSubControlName", tblAccountSetting.AccountSubControlID);
 
            return View(tblAccountSetting);
        }

        // GET: tblAccountSettings/Edit/5
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
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            if (tblAccountSetting == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", tblAccountSetting.AccountActivityID);
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", tblAccountSetting.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSetting.AccountHeadID);
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountSubControlID", "AccountSubControlName", tblAccountSetting.AccountSubControlID);
         
            return View(tblAccountSetting);
        }

        // POST: tblAccountSettings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(  tblAccountSetting tblAccountSetting)
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
            //tblAccountSetting.BranchID = branchid;
            //tblAccountSetting.CompanyID = companyid;
            if (ModelState.IsValid)
            {
                var find = db.tblAccountSettings.Where(c => c.CompanyID == tblAccountSetting.CompanyID && c.BranchID == tblAccountSetting.BranchID && c.AccountActivityID == tblAccountSetting.AccountActivityID && c.AccountSettingID != tblAccountSetting.AccountSettingID).FirstOrDefault();
                if (find == null)
                {

                    db.Entry(tblAccountSetting).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }
            }
           
            ViewBag.AccountActivityID = new SelectList(db.tblAccountActivities, "AccountActivityID", "Name", tblAccountSetting.AccountActivityID);
            ViewBag.AccountControlID = new SelectList(db.tblAccountControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountControlID", "AccountControlName", tblAccountSetting.AccountControlID);
            ViewBag.AccountHeadID = new SelectList(db.tblAccountHeads, "AccountHeadID", "AccountHeadName", tblAccountSetting.AccountHeadID);
            ViewBag.AccountSubControlID = new SelectList(db.tblAccountSubControls.Where(c => c.CompanyID == companyid && c.BranchID == branchid), "AccountSubControlID", "AccountSubControlName", tblAccountSetting.AccountSubControlID);
     
            return View(tblAccountSetting);
        }

        // GET: tblAccountSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            if (tblAccountSetting == null)
            {
                return HttpNotFound();
            }
            return View(tblAccountSetting);
        }

        // POST: tblAccountSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblAccountSetting tblAccountSetting = db.tblAccountSettings.Find(id);
            db.tblAccountSettings.Remove(tblAccountSetting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult GetAccountControls(int? id)
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
            List<AccountControlMV> Controls = new List<AccountControlMV>();
            var list = db.tblAccountControls.Where(i => i.BranchID == branchid && i.CompanyID == companyid&&i.AccountHeadID==id).ToList();
            foreach (var item in list)
            {
                Controls.Add(new AccountControlMV { AccountControlID = item.AccountControlID, AccountControlName = item.AccountControlName  });
            }
            return Json(new { data = Controls }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        public ActionResult GetSubControls(int? id)
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
            List<AccountSubControlMV> Controls = new List<AccountSubControlMV>();
            var list = db.tblAccountSubControls.Where(i => i.BranchID == branchid && i.CompanyID == companyid&& i.AccountControlID==id).ToList();
            foreach (var item in list)
            {
                Controls.Add(new AccountSubControlMV { AccountSubControlID = item.AccountSubControlID, AccountSubControlName = item.AccountSubControlName });
            }
            return Json(new { data = Controls }, JsonRequestBehavior.AllowGet);
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
