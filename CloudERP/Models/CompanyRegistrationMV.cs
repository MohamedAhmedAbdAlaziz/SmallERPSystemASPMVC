using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CloudERP.Models
{
    public class CompanyRegistrationMV
    {
        //user Details
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UserStatus { get; set; }


 //Company Details
        [Required(ErrorMessage = "*Required!")]
        public string CName { get; set; }
        //[DataType(DataType.ImageUrl)]
        //public string Logo { get; set; }
        //[NotMapped]
        //public HttpPostedFileBase LogoFile { get; set; }


        //Branch Details
 
         
        public string BranchName { get; set; }
        public string BranchContact { get; set; }
        public string BranchAddress { get; set; }
        //Employee Details
        public int EmployeeID { get; set; }
        public string EName { get; set; }
        public string EContactNo { get; set; }
        public string EEmail { get; set; }
        public string EPhoto { get; set; }
        public string EAddress { get; set; }
        public string ECNIC { get; set; }
        public string EDesignation { get; set; }
        public string EDescription { get; set; }
        public double EMonthlySalary { get; set; }
    }
}