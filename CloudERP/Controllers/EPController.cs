using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudERP.Controllers
{
    public class EPController : Controller
    {
        // GET: EP
        public ActionResult EP404()
        {
            return View();
        } 
        public ActionResult EP500()
        {
            return View();
        }   
        public ActionResult EP600()
        {
            return View();
        }
    }
}