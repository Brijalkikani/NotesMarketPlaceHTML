using Ionic.Zip;
using Notesmarketplace1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notesmarketplace1.Controllers
{
    public class UserActivityController : Controller
    {
        // GET: UserActivity
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        public ActionResult MyDownload(string search, string sort, int? page, string currentFilter)
        {
            var Emailid = User.Identity.Name.ToString();
            User user1 = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();

            List<Download> downloads = dbobj.Downloads.Where(x => x.downloader == user1.ID && x.isSellerhasAllowedDownloaded ==true).ToList();
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
        public ActionResult Review(int id,string comments, int rate)
        {
            Download objdownload = dbobj.Downloads.Where(x => x.NoteId == id).FirstOrDefault();
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            SellerNote sellrnote = dbobj.SellerNotes.Where(x => x.Id == id).FirstOrDefault();
            SellerNotesReview objseller = dbobj.SellerNotesReviews.Where(x => x.NoteId == sellrnote.Id && x.ReviewedByid==user.ID).FirstOrDefault();
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
            public ActionResult MyRejected()
        {
            
            return View();
        }
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

            return View();
        }
    }
}