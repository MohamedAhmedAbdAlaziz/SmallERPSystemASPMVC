using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseAccess;
using CloudERP.HelperCls;
namespace CloudERP.Controllers
{
    public class tblBranchesController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblBranches
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int companyid = 0;
            companyid=Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            var tblBranches = db.tblBranches.Include(t => t.tblBranchType).Where(c=>c.CompanyID==companyid);
          //  ViewBag.BrchID = db.tblBranches.Where(c => c.CompanyID == companyid);
 
            return View(tblBranches.ToList());
     
        
        }

        // GET: tblBranches/Details/5
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
            tblBranch tblBranch = db.tblBranches.Find(id);
            if (tblBranch == null)
            {
                return HttpNotFound();
            }
            return View(tblBranch);
        }

        // GET: tblBranches/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            ViewBag.BranchTypeID = new SelectList(db.tblBranchTypes, "BranchTypeID", "BranchType",0);
            ViewBag.BrchID = new SelectList(db.tblBranches.Where(c=>c.CompanyID ==  companyid ).ToList(), "BranchID", "BranchName",0);

          
        
            //var tblBranches = db.tblBranches.Include(t => t.tblBranchType);
            //ViewBag.BrchID = db.tblBranches.Where(c => c.CompanyID == companyid);


            return View();
        }

        // POST: tblBranches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( tblBranch tblBranch)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            tblBranch.CompanyID = companyid;

            if (ModelState.IsValid)
            {
                db.tblBranches.Add(tblBranch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrchID = new SelectList(db.tblBranches.Where(c => c.CompanyID == companyid).ToList(), "BranchID", "BranchName");

            ViewBag.BranchTypeID = new SelectList(db.tblBranchTypes, "BranchTypeID", "BranchType", tblBranch.BranchTypeID);
         
            return View(tblBranch);
        }

        // GET: tblBranches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblBranch tblBranch = db.tblBranches.Find(id);
            if (tblBranch == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrchID = new SelectList(db.tblBranches.Where(c => c.CompanyID == companyid).ToList(), "BranchID", "BranchName",tblBranch.BranchID);

            ViewBag.BranchTypeID = new SelectList(db.tblBranchTypes, "BranchTypeID", "BranchType", tblBranch.BranchTypeID);
            return View(tblBranch);
        }

        // POST: tblBranches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( tblBranch tblBranch)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            int companyid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            tblBranch.CompanyID = companyid;

            if (ModelState.IsValid)
            {
                db.Entry(tblBranch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrchID = new SelectList(db.tblBranches.Where(c => c.CompanyID == companyid).ToList(), "BranchID", "BranchName", tblBranch.BranchID);

            ViewBag.BranchTypeID = new SelectList(db.tblBranchTypes, "BranchTypeID", "BranchType", tblBranch.BranchTypeID);
            return View(tblBranch);
        }

        // GET: tblBranches/Delete/5
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
            tblBranch tblBranch = db.tblBranches.Find(id);
            if (tblBranch == null)
            {
                return HttpNotFound();
            }
            return View(tblBranch);
        }

        // POST: tblBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            tblBranch tblBranch = db.tblBranches.Find(id);
            db.tblBranches.Remove(tblBranch);
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

        public ActionResult CompanyUpdate()
        {
            int id = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
       
            tblCompany tblCompany = db.tblCompanies.Find(id);
            Session["ourpic22"] = tblCompany.Logo;
            if (tblCompany == null)
            {
                return HttpNotFound();
            }
            return View(tblCompany);
        }

        // POST: tblCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyUpdate(tblCompany tblCompany)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                if (tblCompany.LogoFile != null)
                {
                    var folder = "/Content/CompanyLogos";
                    var file = string.Format("{0}.jpg", tblCompany.CompanyID);
                    var response = FileHelpers.UploadPhoto(tblCompany.LogoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        tblCompany.Logo = pic;
                    }
                }
                else
                {
                    
                    tblCompany.Logo= (string)Session["ourpic22"];
                   
                }
                db.Entry(tblCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Logout","Home");
            }
            return View(tblCompany);
        }
    }
}
