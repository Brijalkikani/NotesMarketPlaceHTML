using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Notesmarketplace1.Models;

namespace Notesmarketplace1.Controllers
{
    public class HomeController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
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

            if (User.Identity.IsAuthenticated)
            {
                var Emaild = User.Identity.Name.ToString();
                User user = dbobj.Users.Where(x => x.EmailId == Emaild).FirstOrDefault();
                Contactmodel model = new Contactmodel
                {
                    FullName = user.FirstName + " " + user.LastName,
                    EmailAddress = user.EmailId

                };
                return View(model);
            }
            else
            {
                return View();
            }
          
        }
        [HttpPost]
        public ActionResult contactus(Contactmodel vm)
        {
            var fromEmail = new MailAddress(vm.EmailAddress);
            var toEmail = new MailAddress("kikanibrijal23@gmail.com", "Notemarketplace");
            string subject = vm.FullName + " - " + vm.Subject;

            string body = "Hello ," + "<br/>";
            body += vm.Comments;
            body += "<br/><br/>Regards,<br/>";
            body += "Notemarketplace";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("kikanibrijal23@gmail.com", "mtnapkudprykaqpe")
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

    }
}
