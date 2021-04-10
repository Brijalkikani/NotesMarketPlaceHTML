using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Notesmarketplace1.Models;
using System.Data.Entity;

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
            SystemConfiguration system = dbobj.SystemConfigurations.Where(x => x.Key == "SupportEmail").FirstOrDefault();
            SystemConfiguration system1 = dbobj.SystemConfigurations.Where(x => x.Key == "Password").FirstOrDefault();

            var fromEmail = new MailAddress(vm.EmailAddress);
            var toEmail = new MailAddress(system.Value, "Notemarketplace");
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
                Credentials = new NetworkCredential(system.Value, system1.Value)
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
