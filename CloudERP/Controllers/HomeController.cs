using DatabaseAccess;
using DatabaseAccess.Code.SP_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class HomeController : Controller
    {
        SP_Sale sale = new SP_Sale();
        CloudErpV1Entities ds = new CloudErpV1Entities();
        public ActionResult Index()
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
             
                return RedirectToAction("Login");
            }
            var companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            var branchid = Convert.ToInt32(Convert.ToString(Session["BranchID"]));
            
            ViewBag.employess = ds.tblEmployees.Where(x => x.CompanyID == companyid).ToList().Count;
            ViewBag.employess2 = ds.tblEmployees.Where(x => x.CompanyID == companyid && x.BranchID==branchid).ToList().Count;

            ViewBag.branches = ds.tblBranches.Where(x => x.CompanyID == companyid).ToList().Count;
            ViewBag.stocks = ds.tblStocks.Where(x => x.CompanyID == companyid).ToList().Count;
            ViewBag.supliers = ds.tblSuppliers.Where(x => x.CompanyID == companyid).ToList().Count;
            ViewBag.customers = ds.tblCustomers.Where(x => x.CompanyID == companyid).ToList().Count;

            /////////////////////////////////////////////////////////////////////////
            ///

           // int companyid = 0;
           // int branchid = 0;
            int userid = 0;
            companyid = Convert.ToInt32(Convert.ToString(Session["CompanyID"]));
            branchid = Convert.ToInt32(Convert.ToString(Session["BranchId"]));
            userid = Convert.ToInt32(Convert.ToString(Session["UserID"]));
            var list = sale.ReamaningPaymentList(companyid, branchid);

            var companyprocess = ds.tblCustomerInvoices.Where(x => x.CompanyID == companyid).ToList().Count();
            var branchprocess = ds.tblBranches.Where(x => x.CompanyID == companyid ).ToList().Count();

            var prosses = ds.tblCustomerInvoices.Where(x => x.CompanyID == companyid).ToList();
     
         double[] elements = new double[branchprocess];
            string[] elementsName = new string[branchprocess];
         
            List<int> branchI = ds.tblBranches.Where(x => x.CompanyID == companyid).Select(x => x.BranchID).ToList();
            for (int i = 0; i < branchprocess; i++)
            {
                 
                int d = branchI[i];
                int cou= ds.tblCustomerInvoices.Where(x => x.CompanyID == companyid && x.BranchID ==d).ToList().Count();
                // co   list2;
               double  hg   =( ((double)cou * 100.000) / companyprocess) ;

                elements[i]= Math.Round(hg, 2,MidpointRounding.ToEven);

                elementsName[i] = (string)ds.tblBranches.Where(x => x.CompanyID == companyid && x.BranchID == d).Select(x=>x.BranchName).FirstOrDefault();
               
            }
            ViewData["elementsqantity"] =elements.ToList();
            ViewData["elementsNames"] = elementsName.ToList();
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            var companyprocess2 = ds.tblSupplierInvoices.Where(x => x.CompanyID == companyid).ToList().Count();
            var branchprocess2 = ds.tblBranches.Where(x => x.CompanyID == companyid).ToList().Count();

            var prosses2 = ds.tblSupplierInvoices.Where(x => x.CompanyID == companyid).ToList();

            double[] elements2 = new double[branchprocess2];
            string[] elementsName2 = new string[branchprocess2];

            List<int> branchI2 = ds.tblBranches.Where(x => x.CompanyID == companyid).Select(x => x.BranchID).ToList();
            for (int i = 0; i < branchprocess; i++)
            {

                int d2 = branchI2[i];
                int cou2 = ds.tblSupplierInvoices.Where(x => x.CompanyID == companyid && x.BranchID == d2).ToList().Count();
                // co   list2;
                double hg2 = (((double)cou2 * 100.000) / companyprocess2);

                elements2[i] = Math.Round(hg2, 2, MidpointRounding.ToEven);

                elementsName2[i] = (string)ds.tblBranches.Where(x => x.CompanyID == companyid && x.BranchID == d2).Select(x => x.BranchName).FirstOrDefault();

            }
            ViewData["elementsqantity2"] = elements2.ToList();
            ViewData["elementsNames2"] = elementsName2.ToList();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            var companyprocess3= ds.tblStocks.Where(x => x.CompanyID == companyid && x.BranchID== branchid).ToList().Count();
            //var branchprocess3 = ds.tblCustomerInvoiceDetails.Where(x => x.CompanyID == companyid).ToList().Count();
            // var branchprocess3 = ds.tblCustomerInvoiceDetails.Where(x => x.CompanyID == companyid).ToList().Count();
            List<int> ProductsList = ds.tblStocks.Where(x => x.CompanyID == companyid && x.BranchID == branchid).Select(x=>x.ProductID).ToList();

            int count = 0;
            for (int i = 0; i < companyprocess3; i++)
            {
                int g = ProductsList[i];
                int cy= ds.tblCustomerInvoiceDetails.Where(x => x.ProductID ==g).ToList().Count();
                count += cy;
            }

            double[] elements3 = new double[companyprocess3];
            string[] elementsName3 = new string[companyprocess3];
            for (int i = 0; i < companyprocess3; i++)
            {
                int g = ProductsList[i];
                int cy = ds.tblCustomerInvoiceDetails.Where(x => x.ProductID == g).ToList().Count();

                double hg2 = (((double)cy * 100.000) / count);

                elements3[i] = Math.Round(hg2, 2, MidpointRounding.ToEven);

                elementsName3[i] = (string)ds.tblStocks.Where(x =>x.ProductID==g).Select(x => x.ProductName).FirstOrDefault();

            }

            ViewData["elementsqantity3"] = elements3.ToList();
            ViewData["elementsNames3"] = elementsName3.ToList();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///  
            

             var companyprocess4= ds.tblStocks.Where(x => x.CompanyID == companyid && x.BranchID== branchid).ToList().Count();
            //var branchprocess3 = ds.tblCustomerInvoiceDetails.Where(x => x.CompanyID == companyid).ToList().Count();
            // var branchprocess3 = ds.tblCustomerInvoiceDetails.Where(x => x.CompanyID == companyid).ToList().Count();
            List<int> ProductsList4 = ds.tblStocks.Where(x => x.CompanyID == companyid && x.BranchID == branchid).Select(x => x.ProductID).ToList();

            int count4 = 0;
            for (int i = 0; i < companyprocess4; i++)
            {
                int g = ProductsList4[i];
                int cy = ds.tblSupplierInvoiceDetails.Where(x => x.ProductID == g).ToList().Count();
                count4 += cy;
            }

            double[] elements4 = new double[companyprocess4];
            string[] elementsName4 = new string[companyprocess4];
            for (int i = 0; i < companyprocess3; i++)
            {
                int g = ProductsList4[i];
                int cy = ds.tblSupplierInvoiceDetails.Where(x => x.ProductID == g).ToList().Count();

                double hg4 = (((double)cy * 100.000) / count);

                elements4[i] = Math.Round(hg4, 2, MidpointRounding.ToEven);

                elementsName4[i] = (string)ds.tblStocks.Where(x => x.ProductID == g).Select(x => x.ProductName).FirstOrDefault();

            }

            ViewData["elementsqantity4"] = elements4.ToList();
            ViewData["elementsNames4"] = elementsName4.ToList();

            //var wc = 0;
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email,string password)
        {
            var user = ds.tblUsers.Where(x => x.Email == email && x.Password == password&& x.IsActive==true).FirstOrDefault();
      
            if(user != null)
            {
                Session["UserTypeID"] = user.UserTypeID;
                Session["FullName"] = user.FullName;
                Session["Email"] = user.Email;
                Session["ContactNo"] = user.ContactNo;
                Session["UserName"] = user.UserName;
                Session["Password"] = user.Password;
                Session["IsActive"] = user.IsActive;
                Session["UserID"] = user.UserID;
                var EmployeeDetails = ds.tblEmployees.Where(e => e.UserID == user.UserID).FirstOrDefault();
                if (EmployeeDetails == null)
                {
                    ViewBag.Messsage = "Please Contact to Adminstrator!";
                    Session["UserTypeID"] = string.Empty;
                    Session["FullName"] = string.Empty;
                    Session["Email"] = string.Empty;
                    Session["ContactNo"] = string.Empty;
                    Session["UserName"] = string.Empty;
                    Session["Password"] = string.Empty;
                    Session["IsActive"] = string.Empty;
                    Session["EmployeeID"] = string.Empty;
                    Session["EName"] = string.Empty;
                    Session["EPhoto"] = string.Empty;
                    Session["Designation"] = string.Empty;
                    Session["BranchID"] = string.Empty;
                    Session["CompanyID"] = string.Empty;
                    return View("Login");

                }

                Session["EmployeeID"] = EmployeeDetails.EmployeeID;
                Session["EName"] = EmployeeDetails.Name;
                Session["EPhoto"] = EmployeeDetails.Photo;
                Session["Designation"] = EmployeeDetails.Designation;
                Session["BranchID"] = EmployeeDetails.BranchID;
                Session["CompanyID"] = EmployeeDetails.CompanyID;

                var company = ds.tblCompanies.Where(c => c.CompanyID == EmployeeDetails.CompanyID).FirstOrDefault();
                if (company == null)
                {
                    ViewBag.Messsage = "Please Contact to Adminstrator!";
                    Session["UserTypeID"] = string.Empty;
                    Session["FullName"] = string.Empty;
                    Session["Email"] = string.Empty;
                    Session["ContactNo"] = string.Empty;
                    Session["UserName"] = string.Empty;
                    Session["Password"] = string.Empty;
                    Session["IsActive"] = string.Empty;
                    Session["EmployeeID"] = string.Empty;
                    Session["EName"] = string.Empty;
                    Session["EPhoto"] = string.Empty;
                    Session["Designation"] = string.Empty;
                    Session["BranchID"] = string.Empty;
                    Session["CompanyID"] = string.Empty;
                    return View("Login");
                }

                 Session["CName"] = company.Name;
                Session["Logo"] = company.Logo;

                var BranchType = ds.tblBranches.Where(b => b.BranchID == EmployeeDetails.BranchID).FirstOrDefault();
                if (BranchType == null)
                {
                    ViewBag.Messsage = "Please Contact Adminstrator";
                    return View("Login");
                }
                Session["BranchTypeID"] = BranchType.BranchTypeID;
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Messsage = "Incorrect creditionals";
                Session["UserTypeID"] = string.Empty;
                Session["FullName"] = string.Empty;
                Session["Email"] = string.Empty;
                Session["ContactNo"] = string.Empty;
                Session["UserName"] = string.Empty;
                Session["Password"] = string.Empty;
                Session["IsActive"] = string.Empty;
                Session["EmployeeID"] = string.Empty;
                Session["EName"] = string.Empty;
                Session["EPhoto"] = string.Empty;
                Session["Designation"] = string.Empty;
                Session["BranchID"] = string.Empty;
                Session["CompanyID"] = string.Empty;

            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session["UserTypeID"] = string.Empty;
            Session["FullName"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["ContactNo"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["IsActive"] = string.Empty;
            Session["EmployeeID"] = string.Empty;
            Session["EName"] = string.Empty;
            Session["EPhoto"] = string.Empty;
            Session["Designation"] = string.Empty;
            Session["BranchID"] = string.Empty;
            Session["CompanyID"] = string.Empty;

            return View("Login");
        }
        public ActionResult ForgetPassword()
        {
            
            
            
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}