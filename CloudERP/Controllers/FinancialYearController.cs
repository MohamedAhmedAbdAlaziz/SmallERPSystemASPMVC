using DatabaseAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class FinancialYearController : Controller
    {
        private CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: tblAccountSubControls
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
           
            int userid = 0;
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var tblFinancialYears = db.tblFinancialYears;

            return View(tblFinancialYears.ToList());
        }
    }
}