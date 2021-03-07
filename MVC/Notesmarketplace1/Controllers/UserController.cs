using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notesmarketplace1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        
        
        public ActionResult userprofile()
        {
            return View();
        }
        public ActionResult BuyerRequest()
        {
            return View();
        }
    }
}