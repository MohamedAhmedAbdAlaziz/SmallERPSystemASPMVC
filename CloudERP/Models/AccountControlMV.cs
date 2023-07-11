using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudERP.Models
{
    public class AccountControlMV
    {
        public int AccountControlID { get; set; }
        public string Name { get; set; }
        public int CompanyID { get; set; }
        public string BeanchName  {get; set;}
        public int BranchID { get; set; }
       
        public string AccountHeadName { get; set; }
        public int AccountHeadID { get; set; }
        public string AccountControlName { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
    }
}