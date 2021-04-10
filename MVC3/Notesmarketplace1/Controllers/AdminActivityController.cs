using Notesmarketplace1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Notesmarketplace1.Controllers
{
    public class AdminActivityController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        // GET: AdminActivity
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult NotesUnderReview(string searchtext, string currentFilter, int? page, string sortorder, string Seller)
        {

            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Seller1 = sortorder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.Statuspeparam = sortorder == "Status" ? "Status_desc" : "Status";

            List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.IsActive == true).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatas = dbobj.Referencedatas.Where(x => x.RefCategory == "note status" && x.value == "submitted for review" || x.value == "in review").ToList();
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
                               where (r.value == "submitted for review" || r.value == "in review" && ((u.ID.ToString() == Seller || String.IsNullOrEmpty(Seller))))
                               select new DashboardClass
                               { sellernotedetail = s, notecategorydetail = n, referencedatadetail = r, userdetail = u };
            if (!String.IsNullOrEmpty(searchtext))
            {
                tabledetails = tabledetails.Where(x => x.notecategorydetail.name.Contains(searchtext) ||
                 x.sellernotedetail.Title.Contains(searchtext) || x.referencedatadetail.value.Contains(searchtext) ||
                 x.sellernotedetail.CreatedDate.ToString().Contains(searchtext) ||
                 x.userdetail.FirstName.Contains(searchtext) || searchtext == null);
            }
            switch (sortorder)
            {
                case "Date Desc":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.CreatedDate);
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
                    tabledetails = tabledetails.OrderByDescending(x => x.referencedatadetail.value);
                    break;
                case "Status_desc":
                    tabledetails = tabledetails.OrderBy(x => x.referencedatadetail.value);
                    break;
                case "Seller":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.User1.FirstName);
                    break;
                case "Seller_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.User1.FirstName);
                    break;
                default:
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.CreatedDate);
                    break;
            }
            ViewBag.Seller2 = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(tabledetails.ToPagedList(page ?? 1, 5));


        }

        public ActionResult InReview(int id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(id);
            if (sellrnote != null && sellrnote.Referencedata.value == "submitted for review")
            {
                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "in review").Select(x => x.Id).FirstOrDefault();
            }
            dbobj.Entry(sellrnote).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");
        }
        public ActionResult Approve(int id)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            SellerNote sellrnote = dbobj.SellerNotes.Find(id);
            if (sellrnote != null && (sellrnote.Referencedata.value == "submitted for review" || sellrnote.Referencedata.value == "in review" || sellrnote.Referencedata.value == "rejected"))
            {
                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "published").Select(x => x.Id).FirstOrDefault();
                sellrnote.ActionedBy = user.ID;
                sellrnote.Publisheddate = DateTime.Now;
            }
            dbobj.Entry(sellrnote).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");

        }
        [HttpPost]
        public ActionResult RejectNote(int id, string remarks)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            SellerNote sellrnote = dbobj.SellerNotes.Find(id);

            if (sellrnote != null && (sellrnote.Referencedata.value == "submitted for review" || sellrnote.Referencedata.value == "in review"))
            {

                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "rejected").Select(x => x.Id).FirstOrDefault();
                sellrnote.ActionedBy = user.ID;
                sellrnote.ModifiedDate = DateTime.Now;
                sellrnote.AdminRemarks = remarks;

            }
            dbobj.Entry(sellrnote).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");

        }


        [HttpPost]
        public ActionResult Unpublish(int id, string remarks)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            SellerNote sellrnote = dbobj.SellerNotes.Find(id);
            SystemConfiguration system = dbobj.SystemConfigurations.Where(x => x.Key == "SupportEmail").FirstOrDefault();
            SystemConfiguration system1 = dbobj.SystemConfigurations.Where(x => x.Key == "Password").FirstOrDefault();

            if (sellrnote != null && (sellrnote.Referencedata.value == "published"))
            {
                User objuser = dbobj.Users.Where(x => x.ID == sellrnote.SellerID).FirstOrDefault();
                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "removed").Select(x => x.Id).FirstOrDefault();
                sellrnote.ActionedBy = user.ID;
                sellrnote.ModifiedDate = DateTime.Now;
                sellrnote.AdminRemarks = remarks;
                SendEmailtoSellerforUnPublish(objuser.EmailId.ToString(), id, system.Value, system1.Value);
            }
            dbobj.Entry(sellrnote).State = EntityState.Modified;
            dbobj.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");

        }
        [NonAction]
        public void SendEmailtoSellerforUnPublish(string emailID, int id, string support, string password)
        {

            User objuser = dbobj.Users.Where(x => x.EmailId == emailID).FirstOrDefault();
            SellerNote objseller = dbobj.SellerNotes.Where(x => x.SellerID == objuser.ID && x.Id == id).FirstOrDefault();
            var fromEmail = new MailAddress(support, "Notemarketplace");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = "Sorry! We need to remove your notes from our portal.";
            string body = "Hello " + " " + objuser.FirstName + "<br/>";
            body += "<br/>We want like to inform you that, your note" + " " + objseller.Title + " " + " has been removed from the portal.<br/>" +
                "Please find our remarks as below-<br/>"
                + objseller.AdminRemarks;
            ;
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
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AdminPublishedNotes(string searchtext, string currentFilter, int? page, string sortorder, string Seller)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Selltypeparam = sortorder == "Type" ? "Type_desc" : "Type";
            ViewBag.Approveparam = sortorder == "Approve" ? "Approve_desc" : "Approve";
            ViewBag.Publisher = sortorder == "Publisher" ? "Publisher_desc" : "Publisher";
            ViewBag.Download = sortorder == "Download" ? "Download_desc" : "Download";
            ViewBag.Pricepeparam = sortorder == "Price" ? "Price_desc" : "Price";
            List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.IsActive == true).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatas = dbobj.Referencedatas.Where(x => x.RefCategory == "note status" && x.value != "rejected" && x.value != "removed").ToList();
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
                               where (r.value == "published" && ((u.ID.ToString() == Seller || String.IsNullOrEmpty(Seller))))
                               select new DashboardClass
                               {
                                   sellernotedetail = s,
                                   notecategorydetail = n,
                                   referencedatadetail = r,
                                   userdetail = u
                               };
            if (!String.IsNullOrEmpty(searchtext))
            {
                tabledetails = tabledetails.Where(x => x.notecategorydetail.name.Contains(searchtext) ||
                 x.sellernotedetail.Title.Contains(searchtext) || x.sellernotedetail.SellingPrice.ToString().Contains(searchtext) ||
                 x.sellernotedetail.Publisheddate.ToString().Contains(searchtext) ||
                 x.userdetail.FirstName.Contains(searchtext) || x.sellernotedetail.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true).Select(a => a.NoteId).Count().ToString().Contains(searchtext) || searchtext == null);
            }
            switch (sortorder)
            {
                case "Date Desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.Publisheddate);
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
                case "Type":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.IsPaid);
                    break;
                case "Type_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.IsPaid);
                    break;
                case "Approve":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.User.FirstName);
                    break;
                case "Appove_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.User.FirstName);
                    break;
                case "Publisher":
                    tabledetails = tabledetails.OrderByDescending(x => x.userdetail.FirstName);
                    break;
                case "Publisher_desc":
                    tabledetails = tabledetails.OrderBy(x => x.userdetail.FirstName);
                    break;
                case "Download":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true).Select(m => m.NoteId).Count());
                    break;
                case "Download_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true).Select(m => m.NoteId).Count());
                    break;
                case "Price":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.SellingPrice);
                    break;
                case "Price_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.SellingPrice);
                    break;
                default:
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.Publisheddate);
                    break;
            }
            ViewBag.Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(tabledetails.ToPagedList(page ?? 1, 5));
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AdminDownlodNotes(string search, string sort, int? page, string currentFilter, string Seller, string Buyer, string Note)
        {
            List<Download> downloads = dbobj.Downloads.Where(x => x.isSellerhasAllowedDownloaded == true).ToList();
            List<UserProfile> userProfiles = dbobj.UserProfiles.ToList();
            List<User> user = dbobj.Users.ToList();

            var buyerrequest = (from dn in downloads
                                join u in user on dn.downloader equals u.ID into table1
                                from u in table1.ToList()
                                join up in userProfiles on dn.downloader equals up.UserId into table2
                                from up in table2.ToList().DefaultIfEmpty()
                                where ((dn.NoteTitle.ToString() == Note || String.IsNullOrEmpty(Note)) && (u.ID.ToString() == Seller || String.IsNullOrEmpty(Seller)) && (u.FirstName.ToString() == Buyer || String.IsNullOrEmpty(Buyer)))
                                select new DownloadModel
                                { Downloaddetail = dn, Userprofiledetail = up, Userdetail = u }).AsQueryable();
            if (!String.IsNullOrEmpty(search))
            {
                buyerrequest = buyerrequest.Where(x => x.Downloaddetail.AttachmentDownloadedDate.ToString().Contains(search) || x.Userdetail.FirstName.Contains(search) || x.Userdetail.LastName.Contains(search)
               || x.Downloaddetail.NoteTitle.Contains(search) || x.Downloaddetail.NoteCategory.Contains(search)
                  || x.Downloaddetail.PurchasedPrice.ToString().Contains(search) || search == null);

            }
            ViewBag.CurrentSort = sort;
            ViewBag.AddedDateparamPublished = string.IsNullOrEmpty(sort) ? "Date Desc1" : "";
            ViewBag.TitleparamPublished = sort == "Title1" ? "Title_desc1" : "Title1";
            ViewBag.CategoryparamPublished = sort == "Category1" ? "Category_desc1" : "Category1";
            ViewBag.SelltypeparamPublished = sort == "Type" ? "Type_desc" : "Type";
            ViewBag.PricepeparamPublished = sort == "Price" ? "Price_desc" : "Price";
            ViewBag.Buyerparam = sort == "Buyer" ? "Buyer_desc" : "Buyer";
            ViewBag.SellerParam = sort == "Seller" ? "Seller_desc" : "Seller";

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
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.AttachmentDownloadedDate);
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
                case "Buyer":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.User1.FirstName);
                    break;
                case "Buyer_desc":
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.User1.FirstName);
                    break;
                case "Seller":
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.User.FirstName);
                    break;
                case "Seller_desc":
                    buyerrequest = buyerrequest.OrderBy(x => x.Downloaddetail.User.FirstName);
                    break;
                default:
                    buyerrequest = buyerrequest.OrderByDescending(x => x.Downloaddetail.AttachmentDownloadedDate);
                    break;

            }


            ViewBag.Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            ViewBag.Buyer = dbobj.Downloads.Where(x => x.downloader == x.User.ID && x.User.IsEmailVerified == true).Select(x => x.User.FirstName).Distinct();
            ViewBag.Note = dbobj.Downloads.Where(x => x.isSellerhasAllowedDownloaded == true && x.NoteId == x.SellerNote.Id).Select(x => x.NoteTitle).Distinct();
            return View(buyerrequest.ToPagedList(page ?? 1, 5));

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AdminRejectedNotes(string searchtext, string currentFilter, int? page, string sortorder, string Seller)
        {

            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Sellerparam = sortorder == "Seller" ? "Seller_desc" : "Seller";
            ViewBag.Statuspeparam = sortorder == "Status" ? "Status_desc" : "Status";
            ViewBag.Rejectbyparam = sortorder == "Rejectby" ? "Rejectby_desc" : "Rejectby";

            List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.IsActive == true).ToList();
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
                               where (r.value != "published" && ((u.ID.ToString() == Seller || String.IsNullOrEmpty(Seller))))
                               select new DashboardClass
                               { sellernotedetail = s, notecategorydetail = n, referencedatadetail = r, userdetail = u };
            if (!String.IsNullOrEmpty(searchtext))
            {
                tabledetails = tabledetails.Where(x => x.notecategorydetail.name.Contains(searchtext) ||
                 x.sellernotedetail.Title.Contains(searchtext) || x.referencedatadetail.value.Contains(searchtext) ||
                 x.sellernotedetail.ModifiedDate.ToString().Contains(searchtext) ||
                 x.sellernotedetail.User1.FirstName.Contains(searchtext) || x.sellernotedetail.AdminRemarks.Contains(searchtext) || x.sellernotedetail.User.FirstName.Contains(searchtext) | searchtext == null);
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
                case "Seller":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.User1.FirstName);
                    break;
                case "Seller_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.User1.FirstName);
                    break;
                case "Rejectby":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.User.FirstName);
                    break;
                case "Rejectby_desc":
                    tabledetails = tabledetails.OrderBy(x => x.sellernotedetail.User.FirstName);
                    break;
                default:
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.ModifiedDate);
                    break;
            }
            ViewBag.Seller = dbobj.Users.Where(x => x.IsEmailVerified == true && x.RoleID == dbobj.UserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(tabledetails.ToPagedList(page ?? 1, 5));


        }
    }
}