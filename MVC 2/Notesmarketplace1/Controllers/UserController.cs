using Notesmarketplace1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Mail;
using System.Net;

namespace Notesmarketplace1.Controllers
{
    public class UserController : Controller
    {
        // GET: User

        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        public ActionResult userprofile()

        {

            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            UserProfile objuserprofile = dbobj.UserProfiles.Where(x => x.UserId == user.ID).FirstOrDefault();
            ViewBag.Gender = dbobj.Referencedatas.Where(x => x.isActive == true && x.RefCategory == "Gender");
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            ViewBag.countrycode = dbobj.Countries.Where(x => x.isActive == true);
            TempData["ProfilePicture"] = objuserprofile.ProfilePicture;
            if (objuserprofile != null)
            {
                Userprofilee userpr1 = new Userprofilee
                {
                    UserId = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailId = user.EmailId,
                    DOB = objuserprofile.DOB,
                    Gender = objuserprofile.Gender,
                    Phonenumbercountrycode = objuserprofile.Phonenumbercountrycode,
                    PhoneNumber = objuserprofile.PhoneNumber,
                    AddressLine1 = objuserprofile.AddressLine1,
                    AddressLine2 = objuserprofile.AddressLine2,
                    City = objuserprofile.City,
                    State = objuserprofile.State,
                    ZipCode = objuserprofile.ZipCode,
                    Country = objuserprofile.Country,
                    University = objuserprofile.University,
                    College = objuserprofile.College,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                return View(userpr1);
            }
            else
            {

                Userprofilee userpr = new Userprofilee
                {


                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailId = user.EmailId
                };
                return View(userpr);
            }
            return View();
        }
        [HttpPost]
        public ActionResult userprofile(Userprofilee user1)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                UserProfile userprofile1 = dbobj.UserProfiles.Where(x => x.UserId == user1.UserId).FirstOrDefault();
                if (userprofile1 != null)
                {
                    userprofile1.DOB = user1.DOB;
                    userprofile1.Gender = user1.Gender;
                    userprofile1.Phonenumbercountrycode = user1.Phonenumbercountrycode;
                    userprofile1.PhoneNumber = user1.PhoneNumber;
                    userprofile1.AddressLine1 = user1.AddressLine1;
                    userprofile1.AddressLine2 = user1.AddressLine2;
                    userprofile1.City = user1.City;
                    userprofile1.State = user1.State;
                    userprofile1.ZipCode = user1.ZipCode;
                    userprofile1.Country = user1.Country;
                    userprofile1.University = user1.University;
                    userprofile1.College = user1.College;
                    userprofile1.ModifiedDate = DateTime.Now;
                    userprofile1.ModifiedBy = user.ID;
                    userprofile1.IsActive = true;
                    //display picture
                    //generate path to store image
                    string storepath = Path.Combine(Server.MapPath("/UploadFiles/" + user.ID));
                    //check for directory, if not exist ,then create it 
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }


                    if (user1.ProfilePicture != null && user1.ProfilePicture.ContentLength > 0)
                    {

                        string _FileName = Path.GetFileNameWithoutExtension(user1.ProfilePicture.FileName);
                        string extension = Path.GetExtension(user1.ProfilePicture.FileName);
                        _FileName = "DP_" + DateTime.Now.ToString("yymmssfff") + extension;
                        string finalpath = Path.Combine(storepath, _FileName);
                        user1.ProfilePicture.SaveAs(finalpath);


                        userprofile1.ProfilePicture = Path.Combine(("/UploadFiles/" + user.ID + "/"), _FileName);
                        dbobj.SaveChanges();
                    }
                    else
                    {
                        userprofile1.ProfilePicture = "/System Configuration/DefaultImage/user-1.jpg";
                        dbobj.SaveChanges();
                    }
                    dbobj.Entry(userprofile1).State = EntityState.Modified;
                    dbobj.SaveChanges();
                }

                else
                {
                    UserProfile userprofile = new UserProfile
                    {
                        UserId = user.ID,
                        DOB = user1.DOB,
                        Gender = user1.Gender,
                        Phonenumbercountrycode = user1.Phonenumbercountrycode,
                        PhoneNumber = user1.PhoneNumber,
                        AddressLine1 = user1.AddressLine1,
                        AddressLine2 = user1.AddressLine2,
                        City = user1.City,
                        State = user1.State,
                        ZipCode = user1.ZipCode,
                        Country = user1.Country,
                        University = user1.University,
                        College = user1.College,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID,
                        IsActive = true
                    };
                    //Profile picture
                    //generate path to store image
                    string storepath = Path.Combine(Server.MapPath("/UploadFiles/" + user.ID));
                    //check for directory, if not exist ,then create it 
                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }
                    if (user1.ProfilePicture != null && user1.ProfilePicture.ContentLength > 0)
                    {

                        string _FileName = Path.GetFileNameWithoutExtension(user1.ProfilePicture.FileName);
                        string extension = Path.GetExtension(user1.ProfilePicture.FileName);
                        _FileName = "DP_" + DateTime.Now.ToString("yymmssfff") + extension;
                        string finalpath = Path.Combine(storepath, _FileName);
                        user1.ProfilePicture.SaveAs(finalpath);
                        userprofile.ProfilePicture = Path.Combine(("/UploadFiles/" + user.ID + "/"), _FileName);
                        dbobj.SaveChanges();
                    }
                    else
                    {
                        userprofile.ProfilePicture = "/System Configuration/DefaultImage/user-1.jpg";
                        dbobj.SaveChanges();
                    }
                    dbobj.UserProfiles.Add(userprofile);
                    dbobj.SaveChanges();
                }
            }
            ViewBag.Gender = dbobj.Referencedatas.Where(x => x.isActive == true && x.RefCategory == "Gender");
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            ViewBag.countrycode = dbobj.Countries.Where(x => x.isActive == true);
            return RedirectToAction("Searchnote", "Notes");
            return View(user1);
        }
       // [Authorize]
            public ActionResult BuyerRequest(string search, string sort, int? page, string currentFilter)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            List<Download> downloads = dbobj.Downloads.Where(x=>x.Seller==user1.ID && x.isSellerhasAllowedDownloaded==false && x.Ispaid==true).ToList();
            List<UserProfile> userProfiles = dbobj.UserProfiles.ToList();
            List<User> user = dbobj.Users.ToList();

            var buyerrequest = (from dn in downloads
                                join u in user on dn.downloader equals u.ID into table1
                                from u in table1.ToList()
                                join up in userProfiles on dn.downloader equals up.UserId into table2
                                from up in table2.ToList()

                                select new DownloadModel
                                { Downloaddetail = dn, Userprofiledetail = up, Userdetail = u }).AsQueryable();
            if (!String.IsNullOrEmpty(search))
            {
                buyerrequest = buyerrequest.Where(x => x.Userdetail.EmailId.Contains(search) || 
                x.Downloaddetail.NoteTitle.Contains(search) || x.Downloaddetail.NoteCategory.Contains(search)||
                 x.Userprofiledetail.PhoneNumber.Contains(search) ||  x.Downloaddetail.PurchasedPrice.ToString().Contains(search) || search == null);

            }
            ViewBag.CurrentSort = sort;
            ViewBag.AddedDateparamPublished = string.IsNullOrEmpty(sort) ? "Date Desc1" : "";
            ViewBag.TitleparamPublished = sort == "Title1" ? "Title_desc1" : "Title1";
            ViewBag.CategoryparamPublished = sort == "Category1" ? "Category_desc1" : "Category1";
            ViewBag.SelltypeparamPublished = sort == "Type" ? "Type_desc" : "Type";
            ViewBag.PricepeparamPublished = sort == "Price" ? "Price_desc" : "Price";
            

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;
            switch (sort)
            {

                case "Date Desc1":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.AttachmentDownloadedDate);
                    break;
                case "Title1":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.NoteTitle);
                    break;
                case "Title_desc1":
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.NoteTitle);
                    break;
                case "Category1":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.NoteCategory);
                    break;
                case "Category_desc1":
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.NoteCategory);
                    break;
                case "Type":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.Ispaid);
                    break;
                case "Type_desc":
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.Ispaid);
                    break;
                case "Price":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.PurchasedPrice);
                    break;
                case "Price_desc":
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.PurchasedPrice);
                    break;
                default:
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.AttachmentDownloadedDate);
                    break;

            }



            return View(buyerrequest.ToPagedList(page??1,5));
        }
        public ActionResult AllowDownload(int id, int buyerid)
        {
            //bool Status = false;
            {
                var v = dbobj.Downloads.Where(a => a.NoteId==id && a.isSellerhasAllowedDownloaded==false && a.downloader==buyerid).FirstOrDefault();
               SellerNotesAttachement sellernote = dbobj.SellerNotesAttachements.Where(x => x.NoteID == id).FirstOrDefault();
                SellerNote objsellernote = dbobj.SellerNotes.Where(x => x.Id == id).FirstOrDefault();
                if (v != null)
                {
                    v.isSellerhasAllowedDownloaded= true;
                    v.AttachmentPath = sellernote.FilePath;
                    v.ModifiedBy = objsellernote.SellerID;
                    dbobj.SaveChanges();
                    
                }
                
                User user = dbobj.Users.Where(x => x.ID==v.downloader).FirstOrDefault();
                SendEmailtoBuyer(user.EmailId.ToString(),v.NoteId);


            }
            return RedirectToAction("Searchnote","Notes");

        }
        public void SendEmailtoBuyer(string emailID,int noteid)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Where(x => x.Id == noteid).FirstOrDefault();
            User objuser = dbobj.Users.Where(x => x.ID == sellrnote.SellerID).FirstOrDefault();
           
            User user = dbobj.Users.Where(x => x.EmailId == emailID).FirstOrDefault();

            var fromEmail = new MailAddress("kikanibrijal23@gmail.com", "Notemarketplace");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "mtnapkudprykaqpe"; // Replace with actual password
            string subject = objuser.FirstName + " " + "Allows you to downnload a note";

            string body = "Hello " + " " + user.FirstName + "<br/>";
            body += "<br/>We would like to inform you that" + " " + objuser.FirstName + "  Allows you to downnload a note.<br/>" +
                "Please login and see my Download tabs to download particular note<br/>";
            

            body += "<br/><br/>Regards,<br/>";
            body += "Notes marketplace";


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



    }
}