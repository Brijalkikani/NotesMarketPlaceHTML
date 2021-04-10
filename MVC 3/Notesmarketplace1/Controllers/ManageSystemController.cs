using Notesmarketplace1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Notesmarketplace1.Controllers
{
    public class ManageSystemController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        // GET: ManageSystem
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageSystemConfiguration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManageSystemConfiguration(Managesystem managesystem)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {


                SystemConfiguration systemConfiguration1 = new SystemConfiguration()
                {
                    Key = "SupportEmail",
                    Value = managesystem.SupportEmailid,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration1);
                dbobj.SaveChanges();

                SystemConfiguration systemConfiguration = new SystemConfiguration()
                {
                    Key = "Password",
                    Value = managesystem.Password,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration);
                dbobj.SaveChanges();
                SystemConfiguration systemConfiguration2 = new SystemConfiguration()
                {
                    Key = "SupportPhoneNumber",
                    Value = managesystem.PhoneNumber,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration2);
                dbobj.SaveChanges();
                SystemConfiguration systemConfiguration3 = new SystemConfiguration()
                {
                    Key = "EmailAddress",
                    Value = managesystem.EmailAddress,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration3);
                dbobj.SaveChanges();
                SystemConfiguration systemConfiguration4 = new SystemConfiguration()
                {
                    Key = "Facebook URL",
                    Value = managesystem.Facebook,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration4);
                dbobj.SaveChanges();
                SystemConfiguration systemConfiguration5 = new SystemConfiguration()
                {
                    Key = "Twitter URL",
                    Value = managesystem.Twitter,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration5);
                dbobj.SaveChanges();
                SystemConfiguration systemConfiguration6 = new SystemConfiguration()
                {
                    Key = "Linkedin URL",
                    Value = managesystem.Linkedin,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                dbobj.SystemConfigurations.Add(systemConfiguration6);
                dbobj.SaveChanges();
                SystemConfiguration systemConfiguration7 = new SystemConfiguration()
                {
                    Key = "DefaultImageforNote",
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                string storepath = Server.MapPath("/System Configuration/DefaultImage/");
                if (managesystem.DisplayPicture != null && managesystem.DisplayPicture.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(managesystem.DisplayPicture.FileName);
                    string extension = Path.GetExtension(managesystem.DisplayPicture.FileName);
                    _FileName = "DN" + extension;
                    string finalpath = Path.Combine(storepath, _FileName);
                    managesystem.DisplayPicture.SaveAs(finalpath);
                    systemConfiguration7.Value = Path.Combine(("/System Configuration/DefaultImage/"), _FileName);
                    dbobj.SystemConfigurations.Add(systemConfiguration7);
                    dbobj.SaveChanges();
                }
                SystemConfiguration systemConfiguration8 = new SystemConfiguration()
                {
                    Key = "DefaultProfilePicture",
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    isActive = true,
                };
                string storepath1 = Server.MapPath("/System Configuration/DefaultImage/");
                if (managesystem.ProfilePicture != null && managesystem.ProfilePicture.ContentLength > 0)
                {
                    string _FileName1 = Path.GetFileNameWithoutExtension(managesystem.ProfilePicture.FileName);
                    string extension1 = Path.GetExtension(managesystem.ProfilePicture.FileName);
                    _FileName1 = "DP" + extension1;
                    string finalpath1 = Path.Combine(storepath1, _FileName1);
                    managesystem.ProfilePicture.SaveAs(finalpath1);
                    systemConfiguration8.Value = Path.Combine(("/System Configuration/DefaultImage/"), _FileName1);
                    dbobj.SystemConfigurations.Add(systemConfiguration8);
                    dbobj.SaveChanges();
                }
            }

            return RedirectToAction("AdminDashboard", "Admin");
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageAdministrator(string search, int? page, string sortorder, string currentFilter)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Firstnameparam = sortorder == "fname" ? "fname_desc" : "fname";
            ViewBag.Lastnameparam = sortorder == "lname" ? "lname_desc" : "lname";
            ViewBag.Emailparam = sortorder == "Type" ? "Type_desc" : "Type";
            ViewBag.Typeparam = sortorder == "type1" ? "type1_desc" : "type1";
            var users = dbobj.Users.Where(x => x.UserRole.Name == "Admin" && ((x.FirstName.Contains(search) || x.LastName.Contains(search) || x.EmailId.Contains(search)
            || x.CreatedDate.ToString().Contains(search) || x.UserProfiles.Where(a => a.UserId == a.User.ID).Select(a => a.PhoneNumber).Contains(search) || search == null)));
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            switch (sortorder)
            {
                case "Date Desc":
                    users = users.OrderByDescending(x => x.CreatedDate);
                    break;
                case "fname":
                    users = users.OrderByDescending(x => x.FirstName);
                    break;
                case "fname_desc":
                    users = users.OrderBy(x => x.FirstName);
                    break;
                case "lname":
                    users = users.OrderByDescending(x => x.LastName);
                    break;
                case "lname_desc":
                    users = users.OrderBy(x => x.LastName);
                    break;
                case "type1":
                    users = users.OrderByDescending(x => x.IsActive);
                    break;
                case "type1_desc":
                    users = users.OrderBy(x => x.IsActive);
                    break;
                default:
                    users = users.OrderBy(x => x.CreatedDate);
                    break;
            }

            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }
        public ActionResult AddAdministrator(int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            User user = dbobj.Users.Find(id);
            UserProfile userProfile = dbobj.UserProfiles.Where(x => x.UserId == user.ID).FirstOrDefault();
            if (user != null)
            {
                ViewBag.countrycode = dbobj.Countries.Where(x => x.isActive == true);
                UpdateProfile update = new UpdateProfile
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailId = user.EmailId,
                    PhoneNumber = userProfile.PhoneNumber,
                    Phonenumbercountrycode = userProfile.Phonenumbercountrycode,
                };
                return View(update);
            }
            else
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddAdministrator(UpdateProfile updateProfile, int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            ViewBag.countrycode = dbobj.Countries.Where(x => x.isActive == true);
            if (ModelState.IsValid)
            {
                UserProfile userProfile1 = dbobj.UserProfiles.Where(x => x.UserId == id).FirstOrDefault();
                if (userProfile1 != null)
                {
                    userProfile1.User.FirstName = updateProfile.FirstName;
                    userProfile1.User.LastName = updateProfile.LastName;
                    userProfile1.PhoneNumber = updateProfile.PhoneNumber;
                    userProfile1.Phonenumbercountrycode = updateProfile.Phonenumbercountrycode;
                    userProfile1.User.ModifiedBy = user1.ID;
                    userProfile1.User.ModifiedDate = DateTime.Now;
                    userProfile1.ModifiedDate = DateTime.Now;
                    userProfile1.ModifiedBy = user1.ID;
                    dbobj.Entry(userProfile1).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }
                else
                {
                    User user = new User
                    {
                        RoleID = 2,
                        FirstName = updateProfile.FirstName,
                        LastName = updateProfile.LastName,
                        EmailId = updateProfile.EmailId,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1.ID,
                        Password = Crypto.Hash("admin123"),
                    };
                    dbobj.Users.Add(user);
                    dbobj.SaveChanges();
                    UserProfile userProfile = new UserProfile
                    {
                        UserId = user.ID,
                        PhoneNumber = updateProfile.PhoneNumber,
                        Phonenumbercountrycode = updateProfile.Phonenumbercountrycode,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user1.ID,
                    };
                    dbobj.UserProfiles.Add(userProfile);
                    dbobj.SaveChanges();
                    SendVerificationLinkEmail(updateProfile.EmailId.ToString());
                }
            }
            return RedirectToAction("ManageAdministrator", "ManageSystem");
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
        public ActionResult DeleteAdmin(int id)
        {
            UserProfile userProfile = dbobj.UserProfiles.Where(x => x.UserId == id).FirstOrDefault();
            userProfile.IsActive = false;
            userProfile.User.IsActive = false;
            dbobj.Entry(userProfile).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("ManageAdministrator", "ManageSystem");
        }
        [Authorize(Roles = "SuperAdmin,Admin")]

        public ActionResult ManageCategory(string search, int? page, string sortorder, string currentFilter)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.nameparam = sortorder == "fname" ? "fname_desc" : "fname";
            ViewBag.Descriptionparam = sortorder == "lname" ? "lname_desc" : "lname";
            ViewBag.Addedparam = sortorder == "name" ? "name_desc" : "name";
            ViewBag.Typeparam = sortorder == "type1" ? "type1_desc" : "type1";
            var users = dbobj.NoteCategories.Where(x => x.name.Contains(search) || x.Description.Contains(search) || x.Createddate.ToString().Contains(search) ||
            x.User.NoteCategories.Where(a => a.CreatedBy == a.User.ID).Select(a => a.User.FirstName).Contains(search) || search == null);
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            switch (sortorder)
            {
                case "Date Desc":
                    users = users.OrderByDescending(x => x.Createddate);
                    break;
                case "fname":
                    users = users.OrderByDescending(x => x.name);
                    break;
                case "fname_desc":
                    users = users.OrderBy(x => x.name);
                    break;
                case "name":
                    users = users.OrderByDescending(x => x.User.FirstName);
                    break;
                case "name_desc":
                    users = users.OrderBy(x => x.User.FirstName);
                    break;
                case "lname":
                    users = users.OrderByDescending(x => x.Description);
                    break;
                case "lname_desc":
                    users = users.OrderBy(x => x.Description);
                    break;
                case "type1":
                    users = users.OrderByDescending(x => x.isActive);
                    break;
                case "type1_desc":
                    users = users.OrderBy(x => x.isActive);
                    break;

                default:
                    users = users.OrderBy(x => x.Createddate);
                    break;
            }

            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }
        public ActionResult AddCategory(int? id)
        {
            NoteCategory noteCategory = dbobj.NoteCategories.Find(id);
            if (noteCategory != null)
            {
                AddCategory add = new AddCategory
                {
                    name = noteCategory.name,
                    Description = noteCategory.Description,

                };
                return View(add);
            }
            else
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(AddCategory addCategory, int? id)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            if (ModelState.IsValid)
            {
                NoteCategory noteCategory = dbobj.NoteCategories.Find(id);
                if (noteCategory != null)
                {
                    noteCategory.name = addCategory.name;
                    noteCategory.Description = addCategory.Description;
                    noteCategory.Modifiedby = user1.ID;
                    noteCategory.ModifiedDate = DateTime.Now;
                    dbobj.Entry(noteCategory).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }
                else
                {
                    NoteCategory note = new NoteCategory
                    {
                        name = addCategory.name,
                        Description = addCategory.Description,
                        CreatedBy = user1.ID,
                        Createddate = DateTime.Now,
                        isActive = true,
                    };
                    dbobj.NoteCategories.Add(note);
                    dbobj.SaveChanges();
                }
            }
            return RedirectToAction("ManageCategory", "ManageSystem");
        }
        public ActionResult DeleteCategory(int id)
        {
            NoteCategory note = dbobj.NoteCategories.Find(id);
            note.isActive = false;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("ManageCategory", "ManageSystem");
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult ManageType(string search, int? page, string sortorder, string currentFilter)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.nameparam = sortorder == "fname" ? "fname_desc" : "fname";
            ViewBag.Descriptionparam = sortorder == "lname" ? "lname_desc" : "lname";
            ViewBag.Typeparam = sortorder == "type1" ? "type1_desc" : "type1";
            ViewBag.Addedparam = sortorder == "name" ? "name_desc" : "name";
            var users = dbobj.NoteTypes.Where(x => x.Name.Contains(search) || x.Description.Contains(search) || x.CreatedDate.ToString().Contains(search) ||
             x.User.NoteCategories.Where(a => a.CreatedBy == a.User.ID).Select(a => a.User.FirstName).Contains(search) || search == null);
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            switch (sortorder)
            {
                case "Date Desc":
                    users = users.OrderByDescending(x => x.CreatedDate);
                    break;
                case "fname":
                    users = users.OrderByDescending(x => x.Name);
                    break;
                case "fname_desc":
                    users = users.OrderBy(x => x.Name);
                    break;
                case "name":
                    users = users.OrderByDescending(x => x.User.FirstName);
                    break;
                case "name_desc":
                    users = users.OrderBy(x => x.User.FirstName);
                    break;
                case "lname":
                    users = users.OrderByDescending(x => x.Description);
                    break;
                case "lname_desc":
                    users = users.OrderBy(x => x.Description);
                    break;
                case "type1":
                    users = users.OrderByDescending(x => x.IsActive);
                    break;
                case "type1_desc":
                    users = users.OrderBy(x => x.IsActive);
                    break;
                default:
                    users = users.OrderBy(x => x.CreatedDate);
                    break;
            }


            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }
        public ActionResult AddType(int? id)
        {
            NoteType noteCategory = dbobj.NoteTypes.Find(id);
            if (noteCategory != null)
            {
                AddCategory add = new AddCategory
                {
                    name = noteCategory.Name,
                    Description = noteCategory.Description,
                };
                return View(add);
            }
            else
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddType(int? id, AddCategory addCategory)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            if (ModelState.IsValid)
            {
                NoteType noteCategory = dbobj.NoteTypes.Find(id);
                if (noteCategory != null)
                {
                    noteCategory.Name = addCategory.name;
                    noteCategory.Description = addCategory.Description;
                    noteCategory.ModifiedBy = user1.ID;
                    noteCategory.Modifieddate = DateTime.Now;
                    dbobj.Entry(noteCategory).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }
                else
                {
                    NoteType note = new NoteType
                    {
                        Name = addCategory.name,
                        Description = addCategory.Description,
                        CreatedBy = user1.ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                    };
                    dbobj.NoteTypes.Add(note);
                    dbobj.SaveChanges();
                }
            }
            return RedirectToAction("ManageType", "ManageSystem");

        }
        public ActionResult DeleteType(int id)
        {
            NoteType note = dbobj.NoteTypes.Find(id);
            note.IsActive = false;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("ManageType", "ManageSystem");
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult ManageCountry(string search, int? page, string sortorder, string currentFilter)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.nameparam = sortorder == "fname" ? "fname_desc" : "fname";
            ViewBag.Countrycodeparam = sortorder == "lname" ? "lname_desc" : "lname";
            ViewBag.Typeparam = sortorder == "type1" ? "type1_desc" : "type1";
            ViewBag.Addedparam = sortorder == "name" ? "name_desc" : "name";
            var users = dbobj.Countries.Where(x => x.name.Contains(search) || x.CountryCode.Contains(search) || x.Createddate.ToString().Contains(search) ||
            x.User.Countries.Where(a => a.CreatedBy == a.User.ID).Select(a => a.User.FirstName).Contains(search) || search == null);
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            switch (sortorder)
            {
                case "Date Desc":
                    users = users.OrderByDescending(x => x.Createddate);
                    break;
                case "fname":
                    users = users.OrderByDescending(x => x.name);
                    break;
                case "fname_desc":
                    users = users.OrderBy(x => x.name);
                    break;
                case "name":
                    users = users.OrderByDescending(x => x.User.FirstName);
                    break;
                case "name_desc":
                    users = users.OrderBy(x => x.User.FirstName);
                    break;
                case "lname":
                    users = users.OrderByDescending(x => x.CountryCode);
                    break;
                case "lname_desc":
                    users = users.OrderBy(x => x.CountryCode);
                    break;
                case "type1":
                    users = users.OrderByDescending(x => x.isActive);
                    break;
                case "type1_desc":
                    users = users.OrderBy(x => x.isActive);
                    break;
                default:
                    users = users.OrderBy(x => x.Createddate);
                    break;
            }

            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }
        public ActionResult AddCountry(int? id)
        {
            Country noteCategory = dbobj.Countries.Find(id);
            if (noteCategory != null)
            {
                addcountry add = new addcountry
                {
                    name = noteCategory.name,
                    CountryCode = noteCategory.CountryCode,

                };
                return View(add);
            }
            else
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddCountry(int? id, addcountry addcountry)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                Country noteCategory = dbobj.Countries.Find(id);
                if (noteCategory != null)
                {
                    noteCategory.name = addcountry.name;
                    noteCategory.CountryCode = addcountry.CountryCode;
                    noteCategory.ModifiedBy = user1.ID;
                    noteCategory.Modifieddate = DateTime.Now;
                    dbobj.Entry(noteCategory).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }
                else
                {
                    Country note = new Country
                    {
                        name = addcountry.name,
                        CountryCode = addcountry.CountryCode,
                        CreatedBy = user1.ID,
                        Createddate = DateTime.Now,
                        isActive = true,
                    };
                    dbobj.Countries.Add(note);
                    dbobj.SaveChanges();
                }
            }
            return RedirectToAction("ManageCountry", "ManageSystem");

        }
        public ActionResult DeleteCountry(int id)
        {
            Country note = dbobj.Countries.Find(id);
            note.isActive = false;
            dbobj.Entry(note).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("ManageCountry", "ManageSystem");
        }
    }
}