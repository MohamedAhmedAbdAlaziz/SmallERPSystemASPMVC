using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CloudERP.HelperCls;
using CloudERP.Models;
using DatabaseAccess;

namespace CloudERP.Controllers
{
    public class tblSuppliersController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        public ActionResult AllSuppliers()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var tblSuppliers = db.tblSuppliers.Include(t => t.tblBranch).Include(t => t.tblCompany).Include(t => t.tblUser);
            return View(tblSuppliers.ToList());
        }
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

            var tblSuppliers = db.tblSuppliers.Include(t => t.tblBranch).Include(t => t.tblCompany).
                Include(t => t.tblUser).Where(c => c.BranchID == branchid && c.CompanyID == companyid);
            return View(tblSuppliers.ToList());
        }
        public ActionResult SubBranchsupplier()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
           
            List<BranchSuppliersMV> list = new List<BranchSuppliersMV>();
            //// int companyid = 0;
            int branchid = 0;


            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            List<int> branchids = Branch.GetBranchids(branchid, db);
          
            foreach (var item in branchids)
            {
                foreach (var supplier in db.tblSuppliers.Where(c => c.BranchID == item))
                {
                    var sus= new BranchSuppliersMV();
                    
                    sus.BranchName = supplier.tblBranch.BranchName;
                    sus.CompanyName = supplier.tblCompany.Name;
                    sus.SupplierConatctNo = supplier.SupplierConatctNo;
                    sus.SupplierName = supplier.SupplierName;
                    sus.SupplierAddress = supplier.SupplierName;
                    sus.SupplierEmail = supplier.SupplierEmail;
                    sus.Discription = supplier.Discription;
                    sus.User = supplier.tblUser.FullName;
                    list.Add(sus);
 
                      }
            }

         

            return View(list);
        }
        // GET: tblSuppliers/Details/5
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
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        public ActionResult SuplierDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // GET: tblSuppliers/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
        
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName");
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name");
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            return View();
        }

        // POST: tblSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSupplier tblSupplier)
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
            tblSupplier.BranchID = branchid;
            tblSupplier.CompanyID = companyid;
            tblSupplier.UserID = userid;

            if (ModelState.IsValid)
            {
                var find = db.tblSuppliers.Where(s => s.SupplierName == tblSupplier.SupplierName && s.SupplierConatctNo == tblSupplier.SupplierConatctNo && s.BranchID == tblSupplier.BranchID).FirstOrDefault();
                if (find == null)
                {
                    db.tblSuppliers.Add(tblSupplier);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }

            }

            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblSupplier.BranchID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblSupplier.CompanyID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblSupplier.UserID);
            return View(tblSupplier);
        }

        // GET: tblSuppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblSupplier.BranchID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblSupplier.CompanyID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblSupplier.UserID);
            return View(tblSupplier);
        }

        // POST: tblSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tblSupplier tblSupplier)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
             
            int userid = 0;
              userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
              tblSupplier.UserID = userid;

            if (ModelState.IsValid)
            {
                var find = db.tblSuppliers.Where(s => s.SupplierName == tblSupplier.SupplierName && s.SupplierConatctNo == tblSupplier.SupplierConatctNo && s.BranchID == tblSupplier.BranchID&& s.SupplierID!=tblSupplier.SupplierID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(tblSupplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }

            }
           
            //ViewBag.BranchID = new SelectList(db.tblBranches, "BranchID", "BranchName", tblSupplier.BranchID);
            //ViewBag.CompanyID = new SelectList(db.tblCompanies, "CompanyID", "Name", tblSupplier.CompanyID);
            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblSupplier.UserID);
            return View(tblSupplier);
        }

        // GET: tblSuppliers/Delete/5
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
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            if (tblSupplier == null)
            {
                return HttpNotFound();
            }
            return View(tblSupplier);
        }

        // POST: tblSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblSupplier tblSupplier = db.tblSuppliers.Find(id);
            db.tblSuppliers.Remove(tblSupplier);
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
