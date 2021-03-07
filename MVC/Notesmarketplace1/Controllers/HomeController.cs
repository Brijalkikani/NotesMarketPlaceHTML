using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Notesmarketplace1.Models;

namespace Notesmarketplace1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
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
        [HttpGet]
        public ActionResult contactus()
        {
            

            return View();
        }
        [HttpPost]
        public ActionResult contactus(Contactmodel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.EmailAddress);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("kikanibrijal23@gmail.com");//Where mail will be sent 
                    msz.Subject = vm.FullName +"   "+ vm.Subject;
                    msz.Body = vm.Comments;
                  
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("kikanibrijal23@gmail.com", "mtnapkudprykaqpe");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }


            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }
       
    }
}