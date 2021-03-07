using Notesmarketplace1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Notesmarketplace1.Controllers
{
    public class AccountController : Controller
    {
        private notemarketplaceEntities dbObj = new notemarketplaceEntities();

        public bool Status { get; private set; }
        public double timeout { get; private set; }

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Userlogin login, string ReturnUrl = "")
        {
            string message = "";

            {
                var v = dbObj.Users.Where(a => a.EmailId == login.EmailId).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailId, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Notes");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        //Registration action
        public ActionResult Register()
        {
            return View();
        }
        //Registration Post action
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "IsEmailVerified")] UserRegistration user)
        {
            bool status = false;
            string message = "";
            //model validation
            if (ModelState.IsValid)
            {
                //email is already exist
                var isExist = IsEmailExist(user.EmailId);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                //Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //

                user.IsEmailVerified = false;


                UserRole objrole = dbObj.UserRoles.Where(x => x.Name.ToLower() == "member").FirstOrDefault();

                User objUser = new User
                {
                    RoleID = objrole.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailId = user.EmailId,
                    Password = user.Password,
                    CreatedDate = DateTime.Now,
                    IsActive = true,


                };

                var userName = user.FirstName.ToString();


                dbObj.Users.Add(objUser);
                dbObj.SaveChanges();
                //Send Email to User
                SendVerificationLinkEmail(user.EmailId.ToString());
                message = "Registration successfully done. Account activation link " +
                    " has been sent to your email id:" + user.EmailId;

                //TempData["userName"] = userName;
                //return new RedirectResult(@"~\Account\VerifyAccount\");


            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);


        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID)
        {
            var verifyUrl = "/Account/VerifyAccount/?emailID=" + emailID;

            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("kikanibrijal23@gmail.com", "Notemarketplace");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "mtnapkudprykaqpe"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "Hello " + "<br/>";
            body += "<br/>Thank You for signing up with us.  Please click on the below link to verify your emailaddress and to do login.<br/>";
            body += " <a href='" + link + "'>" + link + "Click to VerifyEmail</a> ";
            body += "<br/><br/>Regards,<br/>";
            body += "Notemarketplace";


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        public ActionResult VerifyAccount()
        {
            return View();
        }
        //Verify Account  

        [HttpGet]
        public ActionResult VerifyAccount(string emailID)
        {
            //bool Status = false;
            {

                dbObj.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                   // Confirm password does not match issue on save changes
                var v = dbObj.Users.Where(a => a.EmailId == emailID).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    ViewBag.firstname = v.FirstName;
                    dbObj.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.message("Email is not verified");
                }
            }
            return View();

        }




        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (notemarketplaceEntities dbobj = new notemarketplaceEntities())
            {
                var v = dbobj.Users.Where(x => x.EmailId == emailID).FirstOrDefault();
                return v != null;
            }
        }
        public ActionResult Forgotpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forgotpassword(Forgotpwd f)
        {
            if (ModelState.IsValid)
            {
                bool isvalid = dbObj.Users.Any(x => x.EmailId == f.EmailId);
                if (isvalid)
                {
                    User userdetails = dbObj.Users.Where(x => x.EmailId == f.EmailId).FirstOrDefault();
                    //var v = dbObj.Users.Where(a => a.EmailId == f.EmailId).FirstOrDefault();
                    //if (v != null)

                    //Random Password Generator
                    Random generator = new Random();
                    int otp = generator.Next(100000, 999999);
                    string strotp = otp.ToString("");
                    //encrypt password
                    userdetails.Password = Crypto.Hash(strotp);
                    dbObj.SaveChanges();

                    //Send Password to Email
                    SendVerificationCode(f.EmailId.ToString(), strotp);
                    dbObj.SaveChanges();
                    return RedirectToAction("Login", "Account");

                }


                string msg = "Enter valid EmailID";
                ViewBag.ErrorMsg = msg;
                return View();

            }
            return View();
        }


        [NonAction]
        public void SendVerificationCode(string emailId, string otp)
        {
           
            var verifyUrl = "Account/Login/?emailID=" + emailId + otp;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("kikanibrijal23@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailId);
            var fromEmailPassword = "mtnapkudprykaqpe"; // Replace with actual password
            string subject = "New Temporary password has been created for you";
            string body = "Hello,";
            
            body += "<br/>we have generated a new password for you";
            body += "Your Password is  :" + otp;
            body += "Regards,";
            body += "Notes Marketplace";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
    }

   









