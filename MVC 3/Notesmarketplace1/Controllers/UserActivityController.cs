using Ionic.Zip;
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
    public class UserActivityController : Controller
    {
        // GET: UserActivity
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        [Authorize(Roles = "Member")]
        public ActionResult MyDownload(string search, string sort, int? page, string currentFilter)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            List<Download> downloads = dbobj.Downloads.Where(x => x.downloader == user1.ID && x.isSellerhasAllowedDownloaded == true).ToList();
            List<UserProfile> userProfiles = dbobj.UserProfiles.ToList();
            List<User> user = dbobj.Users.ToList();

            var buyerrequest = (from dn in downloads
                                join u in user on dn.downloader equals u.ID into table1
                                from u in table1.ToList()
                                join up in userProfiles on dn.downloader equals up.UserId into table2
                                from up in table2.ToList().DefaultIfEmpty()

                                select new DownloadModel
                                { Downloaddetail = dn, Userprofiledetail = up, Userdetail = u }).AsQueryable();
            if (!String.IsNullOrEmpty(search))
            {
                buyerrequest = buyerrequest.Where(x => x.Userdetail.EmailId.Contains(search) ||
                x.Downloaddetail.NoteTitle.Contains(search) || x.Downloaddetail.NoteCategory.Contains(search) ||
                 x.Userprofiledetail.PhoneNumber.Contains(search) || x.Downloaddetail.PurchasedPrice.ToString().Contains(search) || search == null);

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
            return View(buyerrequest.ToPagedList(page ?? 1, 5));

        }
        public ActionResult Download(int Id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);

            using (ZipFile zip = new ZipFile())
            {

                zip.AddDirectory(Server.MapPath("~/UploadFiles/" + sellrnote.SellerID + "/" + Id + "/" + "Attachments"));
                MemoryStream output = new MemoryStream();
                zip.Save(output);
                return File(output.ToArray(), "Attachments/zip", "Note.zip");
            }
        }
        public ActionResult Review()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Review(int id, string comments, int rate)
        {
            Download objdownload = dbobj.Downloads.Where(x => x.NoteId == id).FirstOrDefault();
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            SellerNote sellrnote = dbobj.SellerNotes.Where(x => x.Id == id).FirstOrDefault();
            SellerNotesReview objseller = dbobj.SellerNotesReviews.Where(x => x.NoteId == sellrnote.Id && x.ReviewedByid == user.ID).FirstOrDefault();
            if (objseller != null)
            {
                objseller.Ratings = rate;
                objseller.Comments = comments;
                objseller.ModifiedBy = user.ID;
                objseller.Modifieddate = DateTime.Now;
                dbobj.Entry(objseller).State = EntityState.Modified;
                dbobj.SaveChanges();
            }
            else
            {
                SellerNotesReview objseller1 = new SellerNotesReview
                {
                    NoteId = sellrnote.Id,
                    Ratings = rate,
                    Comments = comments,
                    ReviewedByid = user.ID,
                    AgainstDownloadsId = objdownload.Id,
                    CreatedBy = objdownload.downloader,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                dbobj.SellerNotesReviews.Add(objseller1);
                dbobj.SaveChanges();
            }

            return RedirectToAction("Dashboard", "Notes");
        }
        [HttpPost]
        public ActionResult ReportanIssue(int id, string remarks)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            SellerNote sellrnote = dbobj.SellerNotes.Where(x => x.Id == id).FirstOrDefault();
            Download objdownload = dbobj.Downloads.Where(x => x.NoteId == id).FirstOrDefault();
            SellerNotesReportedIssue reportedIssue = dbobj.SellerNotesReportedIssues.Where(x => x.NoteId == sellrnote.Id && x.ReportedByid == user.ID).FirstOrDefault();
            if (reportedIssue != null)
            {
                reportedIssue.Remarks = remarks;
                reportedIssue.Modifieddate = DateTime.Now;
                reportedIssue.ModifiedBy = user.ID;
                dbobj.Entry(reportedIssue).State = EntityState.Modified;
                dbobj.SaveChanges();
            }
            else
            {
                SellerNotesReportedIssue objseller1 = new SellerNotesReportedIssue
                {
                    NoteId = sellrnote.Id,
                    Remarks = remarks,
                    ReportedByid = user.ID,
                    againstDownloadId = objdownload.Id,
                    CreatedBy = objdownload.downloader,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                dbobj.SellerNotesReportedIssues.Add(objseller1);
                dbobj.SaveChanges();

            }
            SystemConfiguration system = dbobj.SystemConfigurations.Where(x => x.Key == "SupportEmail").FirstOrDefault();
            SystemConfiguration system1 = dbobj.SystemConfigurations.Where(x => x.Key == "Password").FirstOrDefault();
            SystemConfiguration system2 = dbobj.SystemConfigurations.Where(x => x.Key == "EmailAddress").FirstOrDefault();
            SendEmailtoAdmin(system.Value, system1.Value, system2.Value, id);
            return RedirectToAction("Dashboard", "Notes");

        }
        [NonAction]
        public void SendEmailtoAdmin(string support, string password, string emailID, int id)
        {

            SellerNote seller = dbobj.SellerNotes.Where(x => x.Id == id).FirstOrDefault();
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            User user1 = dbobj.Users.Where(x => x.ID == seller.SellerID).FirstOrDefault();
            var fromEmail = new MailAddress(support, "Notemarketplace");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = user.FirstName + " " + "Reported an issue for" + " " + seller.Title;
            string body = "Hello  Admin," + "<br/>";
            body += "<br/>We want to inform you that" + " " + user.FirstName + "  Reported an issue for" + " " + user1.FirstName + "'s   Note with";
            body += "<br/>title" + " " + seller.Title + "." + "Please look at the notes and take required actions.";
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
        public ActionResult Clone(int id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(id);
            if (sellrnote != null && sellrnote.Referencedata.value == "rejected")
            {
                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "draft").Select(x => x.Id).FirstOrDefault();
            }
            dbobj.Entry(sellrnote).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("Dashboard", "Notes");
        }
        [Authorize(Roles = "Member")]
        public ActionResult MyRejected(string searchtext, string currentFilter, int? page, string sortorder)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Statuspeparam = sortorder == "Status" ? "Status_desc" : "Status";


            List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.IsActive == true && x.SellerID == user1.ID).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatas = dbobj.Referencedatas.Where(x => x.RefCategory == "note status" && x.value == "rejected").ToList();
            List<User> userdatas = dbobj.Users.ToList();
            if (searchtext != null)
            {
                page = 1;
            }
            else
            {
                searchtext = currentFilter;
            }
            ViewBag.CurrentFilter = searchtext;
            var tabledetails = from s in sellerNotes
                               join n in noteCategories on s.Category equals n.Id into table1
                               from n in table1.ToList()
                               join r in referencedatas on s.Status equals r.Id into table2
                               from r in table2.ToList()
                               join u in userdatas on s.SellerID equals u.ID into table3
                               from u in table3.ToList()
                               where (r.value != "published")
                               select new DashboardClass
                               { sellernotedetail = s, notecategorydetail = n, referencedatadetail = r, userdetail = u };
            if (!String.IsNullOrEmpty(searchtext))
            {
                tabledetails = tabledetails.Where(x => x.notecategorydetail.name.Contains(searchtext) ||
                 x.sellernotedetail.Title.Contains(searchtext)
                 || x.sellernotedetail.AdminRemarks.Contains(searchtext) || searchtext == null);
            }
            switch (sortorder)
            {
                case "Date Desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.ModifiedDate);
                    break;
                case "Title":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.Title);
                    break;
                case "Title_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.Title);
                    break;
                case "Category":
                    tabledetails = tabledetails.OrderByDescending(x => x.notecategorydetail.name);
                    break;
                case "Category_desc":
                    tabledetails = tabledetails.OrderBy(x => x.notecategorydetail.name);
                    break;
                case "Status":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.AdminRemarks);
                    break;
                case "Status_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.AdminRemarks);
                    break;

                default:
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.ModifiedDate);
                    break;
            }
            ViewBag.Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(tabledetails.ToPagedList(page ?? 1, 5));



        }
        [Authorize(Roles = "Member")]
        public ActionResult MySoldNotes(string search, string sort, int? page, string currentFilter)

        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            List<Download> downloads = dbobj.Downloads.Where(x => x.Seller == user1.ID && x.isSellerhasAllowedDownloaded == true).ToList();
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
                x.Downloaddetail.NoteTitle.Contains(search) || x.Downloaddetail.NoteCategory.Contains(search) ||
                 x.Userprofiledetail.PhoneNumber.Contains(search) || x.Downloaddetail.PurchasedPrice.ToString().Contains(search) || search == null);

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

            return View(buyerrequest.ToPagedList(page ?? 1, 5));


        }
    }
}