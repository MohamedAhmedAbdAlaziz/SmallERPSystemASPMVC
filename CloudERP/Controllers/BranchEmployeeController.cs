using CloudERP.HelperCls;
using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class BranchEmployeeController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();
        // GET: BranchEmployee
        public ActionResult Employee()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int companyid = 0;
            int branchid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            var tblEmployee = db.tblEmployees.Where(c => c.CompanyID == companyid && c.BranchID == branchid);

            return View(tblEmployee);
        }
        public ActionResult EmployeeRegistration()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeRegistration(tblEmployee employee)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int companyid = 0;
            int branchid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
           // userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));// Session["UserID"]
            employee.BranchID = branchid;
            employee.CompanyID = companyid;
           employee.UserID = null;
            var t = db.tblEmployees.Where(e=>e.Email==employee.Email).FirstOrDefault();
            if(t!=null)
            {
                ViewData["E"] = "sorry this Email is already exist";
                return View("EmployeeRegistration", employee);
            }

            if (ModelState.IsValid)
            {
                db.tblEmployees.Add(employee);
                db.SaveChanges();
                if (employee.LogoFile != null)
                {
                    var folder = "/Content/EmployeeLogos";
                    var file = string.Format("{0}.jpg", employee.EmployeeID);
                    var response = FileHelpers.UploadPhoto(employee.LogoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        employee.Photo = pic;
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                //db.tblEmployees.Add(employee);
                //db.SaveChanges();
                return RedirectToAction("Employee");
            }
            return View(employee);
        }
        public ActionResult EmployeeUpdation(int? id)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = db.tblEmployees.Find(id);
           
            Session["ourpic"] = employee.Photo;
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeUpdation(tblEmployee employee)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }

            int companyid = 0;
            int branchid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            employee.BranchID = branchid;
            employee.CompanyID = companyid;
           employee.UserID = employee.UserID;
            if (ModelState.IsValid)
            {

                if (employee.LogoFile != null)
                {
                    var folder = "/Content/EmployeeLogos";
                    var file = string.Format("{0}.jpg", employee.EmployeeID);
                    var response = FileHelpers.UploadPhoto(employee.LogoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        employee.Photo = pic;
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                   
                }
                else
                {
                    //var employee2 = db.tblEmployees.Find(employee.EmployeeID);
                    
                    employee.Photo =(string) Session["ourpic"];
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //db.tblEmployees.Add(employee);
                //db.SaveChanges();
                return RedirectToAction("Employee");
            }
            return View(employee);
        }

        public ActionResult ViewProfile(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
                {
                    return RedirectToAction("Login", "Home");
                }
                if (id == null)
                {
                    return RedirectToAction("EP500", "EP");
                    // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //var employee = db.tblEmployees.Find(id);
                int companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
                var employee = db.tblEmployees.Where(e => e.CompanyID == companyid && e.EmployeeID == id).FirstOrDefault();
                if (employee == null)
                {
                    return RedirectToAction("EP404", "EP");
                    //return HttpNotFound();
                }
                return View(employee);
            }
            catch 
            {

                return RedirectToAction("EP404", "EP");
            }

        }

        public ActionResult CreateUser(int? employeeid)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {

                return RedirectToAction("Login", "Home");
            }
            Session["CEmployeeID"] = employeeid;
            var employee = db.tblEmployees.Find(employeeid);
            var user = new tblUser();
            user.Email = employee.Email;
            user.ContactNo = employee.ContactNo;
            user.FullName = employee.Name;
            
            user.UserName = employee.Email;
            user.IsActive = true;
            ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(tblUser user)
        {
            user.UserTypeID = 2;
            try
            {

                if (ModelState.IsValid)
                {
                    var guser = db.tblUsers.Where(u => u.Email == user.Email);
                    if (guser.Count() > 0)
                    {
                        ViewBag.Message = "Email is Already registered";
                        return View("CreateUser", user);
                    }
                    else
                    {
                        db.tblUsers.Add(user);
                        db.SaveChanges();
                        int? employeeid = Convert.ToInt32(Convert.ToString(Session["CEmployeeID"]));
                        var employee = db.tblEmployees.Find(employeeid);
                        employee.UserID = user.UserID;
                        db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        Session["CEmployeeID"] = null;
                        //ViewBag.Message = "User registered Successfully !";
                        //user = null;
                        return RedirectToAction("Employee", "BranchEmployee");
                    }

                    //     return RedirectToAction("Index");
                }
                if (user == null)
                {
                    ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType");
                }
                else
                {
                    ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType", user.UserTypeID);

                }
                //  ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType", user.UserTypeID);
                return View(user);
            }
            catch
            {
                return RedirectToAction("EP", "EP500");
            }
        }
        public ActionResult UpdateUser(int? userid)
        {
            var user = db.tblUsers.Find(userid);
            ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType", user.UserTypeID);
            ViewData["pass"]= user.Password;
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(tblUser user)
        {
            user.UserTypeID = 2;
            //if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            if (ModelState.IsValid)
            {
                var guser = db.tblUsers.Where(u => u.Email == user.Email && u.UserID != user.UserID);
                if (guser.Count() > 0)
                {
                    ViewBag.Message = "Email is Already registered";
                }
                else
                {
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //ViewBag.Message = "User Updated Successfully !";
                    //user = null;
                    return RedirectToAction("Employee", "BranchEmployee");
                }

                //     return RedirectToAction("Index");
            }
            if (user == null)
            {
                ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType");
            }
            else
            {
                ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType", user.UserTypeID);

            }

            //   ViewBag.UserTypeID = new SelectList(db.tblUserTypes.ToList(), "UserTypeID", "UserType",user.UserTypeID);
            return View(user);
        }

    }
}