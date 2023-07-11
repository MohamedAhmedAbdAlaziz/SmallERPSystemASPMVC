using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudERP.Models
{
    public class AccountControlsMV
    {
        public int AccountControlID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public int AccountHeadID { get; set; }
      
        public string AccountControlName { get; set; }
        public int UserID { get; set; }

    }
}