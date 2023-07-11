using DatabaseAccess;
using DatabaseAccess.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class CompanyRegistrationController : Controller
    {
          CloudErpV1Entities db = new CloudErpV1Entities();

        // GET: CompanyRegistration
        public ActionResult RegistrationForm()
        { 
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult RegistrationForm(
            string UserName,
            string Password,
         //   string CPassword,
            string EName,
            string EContactNo,
            string EEmail,
            string ECNIC,
            string EDesignation,
            string EDescription,
            float EMonthlySalary,
            string EAddress ,
            string CName,
            string BranchName,
            string BranchContact,
            string BranchAddress

            )
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["CompanyID"])))
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                if (!string.IsNullOrEmpty(UserName)
                    && !string.IsNullOrEmpty(Password)
                    && !string.IsNullOrEmpty(EName)
                    && !string.IsNullOrEmpty(EContactNo)
                    && !string.IsNullOrEmpty(EEmail)
                    && !string.IsNullOrEmpty(ECNIC)
                    && !string.IsNullOrEmpty(EDescription)
                    && !string.IsNullOrEmpty(EDesignation)
                    && !string.IsNullOrEmpty(EAddress)
                    && EMonthlySalary > 0
                    && !string.IsNullOrEmpty(CName)
                    && !string.IsNullOrEmpty(BranchName)
                    && !string.IsNullOrEmpty(BranchAddress)
                    && !string.IsNullOrEmpty(BranchContact)
                     )
                {
                    var company = new tblCompany()
                    {
                        Name = CName,
                        Logo = string.Empty
                    };
                    db.tblCompanies.Add(company);
                    db.SaveChanges();
                    // int t = (int)(db.tblBranches.Where(x => x.BranchName == branch.BranchName).Select(s => s.BranchID).FirstOrDefault());

                    //  int tt = db.tblBranches.Take(1).LastOrDefault().BranchID;
                    var branch = new tblBranch();

                    ///   BranchID = tt++,
                    branch.BranchAddress = BranchAddress;
                    branch.BranchContact = BranchContact;
                    branch.BranchName = BranchName;
                    branch.BranchTypeID = 1;
                    branch.CompanyID = company.CompanyID;//(int)(db.tblCompanies.Where(x => x.Name == company.Name).Select(s => s.CompanyID).FirstOrDefault()),    //company.CompanyID
                    branch.BrchID = null;
                    //db.tblBranches.Add(branch);
                    //db.SaveChanges(); using (SqlConnection connection = new SqlConnection("data source=.;initial catalog=MyTest;integrated security=True;"))
                    using (SqlConnection connection = new SqlConnection("data source=.;initial catalog=CloudErpV1;integrated security=True;"))

                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;            // <== lacking
                            command.CommandType = CommandType.Text;
                            command.CommandText = "insert into [tblBranch] (BranchAddress,BranchContact,BranchName,BranchTypeID,CompanyID) values" +
                        " (@BranchAddress,@BranchContact,@BranchName,@BranchTypeID,@CompanyID)";
                            command.Parameters.AddWithValue("@BranchAddress", BranchAddress);
                            command.Parameters.AddWithValue("@BranchContact", BranchContact);
                            command.Parameters.AddWithValue("@BranchName", BranchName);
                            command.Parameters.AddWithValue("@BranchTypeID", 1);
                            command.Parameters.AddWithValue("@CompanyID", company.CompanyID);
                            //   command.Parameters.AddWithValue("@BrchID", 1);
                            try
                            {
                                connection.Open();
                                int recordsAffected = command.ExecuteNonQuery();
                               /// Console.WriteLine("Done");
                            }
                            catch (SqlException)
                            {
                               // Console.WriteLine(s.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }

                   
                  


                    //                    SqlCommand command = new SqlCommand("insert into [tblBranch] (BranchAddress,BranchContact,BranchName,BranchTypeID,CompanyID,BrchID) values" +
                    //    " (@BranchAddress,@BranchContact,@BranchName,@BranchTypeID,@CompanyID,@BrchID)", DatabaseQuery.ConnOpen());


                    //command.Parameters.AddWithValue("@BranchAddress", BranchAddress);
                    //command.Parameters.AddWithValue("@BranchContact", BranchContact);
                    //command.Parameters.AddWithValue("@BranchName", BranchName);
                    //command.Parameters.AddWithValue("@BranchTypeID", 1);
                    //command.Parameters.AddWithValue("@CompanyID", company.CompanyID);
                    //command.Parameters.AddWithValue("@BrchID", null);


               ///     command.ExecuteNonQuery();








                    var user = new tblUser()
                    {
                        ContactNo = EContactNo,
                        Email = EEmail,
                        FullName = EName,
                        IsActive = true,
                        Password = Password,
                        UserName = UserName,
                        UserTypeID = 2
                    };
                    db.tblUsers.Add(user);
                    db.SaveChanges();

                    var employee = new tblEmployee()
                    {
                        Address = EAddress,
                      //  BranchID = (int)(db.tblBranches.Where(x => x.BranchName == branch.BranchName).Select(s => s.BranchID).FirstOrDefault()),//branch.BranchID
                        BranchID = (int)(db.tblBranches.OrderByDescending(t => t.BranchID).Select(s => s.BranchID).First()),//branch.BranchID
                        CNIC = ECNIC,
                        CompanyID = company.CompanyID,//(int)(db.tblCompanies.Where(x => x.Name == company.Name).Select(s => s.CompanyID).FirstOrDefault())  ,
                        ContactNo = EContactNo,
                        Designation = EDesignation,
                          Email=EEmail,
                        Description = EDescription,
                       // UserID = (int)(db.tblUsers.Where(x => x.UserName == user.UserName).Select(s => s.UserID).FirstOrDefault()),// user.UserID,
                        UserID =   user.UserID,
                        Name = EName,
                        MonthlySalary=EMonthlySalary

                    };
                    db.tblEmployees.Add(employee);
                    db.SaveChanges();
                    ViewBag.Message = "Registration Successfully !";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.Message = "Please Provide Correct Details";
                    return View("RegistrationForm");
                }
            }
            catch  
            {
                ViewBag.Message = "Please Contact To Adminstrator !";
                return View();
            }
        }
    }
}