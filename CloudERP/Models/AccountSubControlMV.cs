using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudERP.Models
{
    public class AccountSubControlMV
    {
        public int AccountSubControlID { get; set; }
 
        public int AccountHeadID { get; set; }
 
        public int AccountControlID { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
 
        public string AccountSubControlName { get; set; }
        public int UserID { get; set; }
    }
}