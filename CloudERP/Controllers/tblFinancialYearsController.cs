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
    public class tblFinancialYearsController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblFinancialYears
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));

            var tblFinancialYears = db.tblFinancialYears.Include(t => t.tblUser);
            return View(tblFinancialYears.ToList());
        }

        // GET: tblFinancialYears/Details/5
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
            tblFinancialYear tblFinancialYear = db.tblFinancialYears.Find(id);
            if (tblFinancialYear == null)
            {
                return HttpNotFound();
            }
            return View(tblFinancialYear);
        }

        // GET: tblFinancialYears/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //    ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName");
            return View();
        }

        // POST: tblFinancialYears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblFinancialYear tblFinancialYear)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblFinancialYear.UserID = userid;
            if (ModelState.IsValid)
            {
                var find = db.tblFinancialYears.Where(c => c.FinancialYear == tblFinancialYear.FinancialYear).FirstOrDefault();
                if (find == null)
                {
                    db.tblFinancialYears.Add(tblFinancialYear);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }
            }
 

            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblFinancialYear.UserID);
            return View(tblFinancialYear);
        }

        // GET: tblFinancialYears/Edit/5
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
            tblFinancialYear tblFinancialYear = db.tblFinancialYears.Find(id);
            if (tblFinancialYear == null)
            {
                return HttpNotFound();
            }
            ///ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblFinancialYear.UserID);
            return View(tblFinancialYear);
        }

        // POST: tblFinancialYears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tblFinancialYear tblFinancialYear)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int userid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            tblFinancialYear.UserID = userid;
            if (ModelState.IsValid)
            {
                var find = db.tblFinancialYears.Where(c => c.FinancialYear == tblFinancialYear.FinancialYear && c.FinancialYearID != tblFinancialYear.FinancialYearID).FirstOrDefault();
                if (find == null)
                {
                    db.Entry(tblFinancialYear).State = EntityState.Modified;
                    db.SaveChanges(); 
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already is Exist !";
                }
            }


            //ViewBag.UserID = new SelectList(db.tblUsers, "UserID", "FullName", tblFinancialYear.UserID);
            return View(tblFinancialYear);
              }

        // GET: tblFinancialYears/Delete/5
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
            tblFinancialYear tblFinancialYear = db.tblFinancialYears.Find(id);
            if (tblFinancialYear == null)
            {
                return HttpNotFound();
            }
            return View(tblFinancialYear);
        }

        // POST: tblFinancialYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblFinancialYear tblFinancialYear = db.tblFinancialYears.Find(id);
            db.tblFinancialYears.Remove(tblFinancialYear);
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
