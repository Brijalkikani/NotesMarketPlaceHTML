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
    public class MemberDetailController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        // GET: MemberDetail
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult Member(string search, int? page, string sortorder, string currentFilter)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Firstnameparam = sortorder == "fname" ? "fname_desc" : "fname";
            ViewBag.Lastnameparam = sortorder == "lname" ? "lname_desc" : "lname";
            ViewBag.Emailparam = sortorder == "Type" ? "Type_desc" : "Type";
            ViewBag.UnderReviewparam = sortorder == "Approve" ? "Approve_desc" : "Approve";
            ViewBag.Publishnotes = sortorder == "Publisher" ? "Publisher_desc" : "Publisher";
            ViewBag.Downloadnotes = sortorder == "Download" ? "Download_desc" : "Download";
            ViewBag.Expenseparam = sortorder == "Price" ? "Price_desc" : "Price";
            ViewBag.Earningparam = sortorder == "Price1" ? "Price1_desc" : "Price1";
            var users = (dbobj.Users.Where(x => x.UserRole.Name.ToLower() == "member" && x.IsEmailVerified == true && (x.FirstName.Contains(search) || x.LastName.Contains(search) || x.EmailId.Contains(search)
              || x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.Seller == a.User.ID).Sum(a => a.PurchasedPrice).ToString().Contains(search)
              || x.Downloads1.Where(a => a.isSellerhasAllowedDownloaded == true && a.downloader == a.User1.ID).Sum(a => a.PurchasedPrice).ToString().Contains(search) || search == null))).AsQueryable();
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
                case "Type":
                    users = users.OrderByDescending(x => x.EmailId);
                    break;
                case "Type_desc":
                    users = users.OrderBy(x => x.EmailId);
                    break;
                case "Approve":
                    users = users.OrderByDescending(x => x.SellerNotes.Where(a => a.Referencedata.RefCategory == "note status" && (a.Referencedata.value == "submitted for review" || a.Referencedata.value == "in review")).Select(a => a.Id).Count());
                    break;
                case "Appove_desc":
                    users = users.OrderBy(x => x.SellerNotes.Where(a => a.Referencedata.RefCategory == "note status" && (a.Referencedata.value == "submitted for review" || a.Referencedata.value == "in review")).Select(a => a.Id).Count());
                    break;
                case "Publisher":
                    users = users.OrderByDescending(x => x.SellerNotes1.Where(a => a.Referencedata.RefCategory == "note status" && a.Referencedata.value == "published").Select(a => a.Id).Count());
                    break;
                case "Publisher_desc":
                    users = users.OrderBy(x => x.SellerNotes1.Where(a => a.Referencedata.RefCategory == "note status" && a.Referencedata.value == "published").Select(a => a.Id).Count());
                    break;
                case "Download":
                    users = users.OrderByDescending(x => x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.Seller == x.ID).Count());
                    break;
                case "Download_desc":
                    users = users.OrderBy(x => x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.Seller == x.ID).Count());
                    break;
                case "Price":
                    users = users.OrderByDescending(x => x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.downloader == x.ID).Sum(a => a.PurchasedPrice));
                    break;
                case "Price_desc":
                    users = users.OrderBy(x => x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.downloader == x.ID).Sum(a => a.PurchasedPrice));
                    break;
                case "Price1":
                    users = users.OrderByDescending(x => x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.Seller == x.ID).Sum(a => a.PurchasedPrice));
                    break;
                case "Price1_desc":
                    users = users.OrderBy(x => x.Downloads.Where(a => a.isSellerhasAllowedDownloaded == true && a.Seller == x.ID).Sum(a => a.PurchasedPrice));
                    break;
                default:
                    users = users.OrderBy(x => x.CreatedDate);
                    break;
            }
            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }
        public ActionResult Deactivate(int id)
        {

            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            User objuser = dbobj.Users.Where(x => x.ID == id).FirstOrDefault();
            objuser.IsActive = false;
            dbobj.Entry(objuser).State = EntityState.Modified;
            dbobj.SaveChanges();

            var sellerNote = dbobj.SellerNotes.Where(x => x.SellerID == id && x.Referencedata.RefCategory.ToLower() == "note status" && x.Referencedata.value.ToLower() == "published").ToList();

            foreach (var i in sellerNote)
            {
                i.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "removed").Select(x => x.Id).FirstOrDefault();
                i.ModifiedBy = user.ID;
                i.ModifiedDate = DateTime.Now;
                i.ActionedBy = user.ID;
                dbobj.Entry(i).State = EntityState.Modified;
            }
            dbobj.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult MemberDetail(int id, string currentFilter, int? page, string sortorder)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Statuspeparam = sortorder == "Status" ? "Status_desc" : "Status";
            ViewBag.Publishdate = sortorder == "Date1" ? "Date1_desc" : "Date1";
            ViewBag.Earningparam = sortorder == "Price1" ? "Price1_desc" : "Price1";
            var users = dbobj.Users.Where(x => x.ID == id);
            var sellernotes = dbobj.SellerNotes.Where(x => x.SellerID == id && (x.Referencedata.RefCategory == "note status" && x.Referencedata.value.ToLower() != "draft"));
            switch (sortorder)
            {
                case "Date Desc":
                    sellernotes = sellernotes.OrderBy(x => x.CreatedDate);
                    break;
                case "Title":
                    sellernotes = sellernotes.OrderByDescending(x => x.Title);
                    break;
                case "Title_desc":
                    sellernotes = sellernotes.OrderBy(x => x.Title);
                    break;
                case "Category":
                    sellernotes = sellernotes.OrderByDescending(x => x.NoteCategory.name);
                    break;
                case "Category_desc":
                    sellernotes = sellernotes.OrderBy(x => x.NoteCategory.name);
                    break;
                case "Status":
                    sellernotes = sellernotes.OrderByDescending(x => x.Referencedata.value);
                    break;
                case "Status_desc":
                    sellernotes = sellernotes.OrderBy(x => x.Referencedata.value);
                    break;
                case "Date1":
                    sellernotes = sellernotes.OrderByDescending(x => x.Publisheddate);
                    break;
                case "Date1_desc":
                    sellernotes = sellernotes.OrderBy(x => x.Publisheddate);
                    break;
                case "Price1":
                    sellernotes = sellernotes.OrderByDescending(x => x.Downloads.Where(a => a.Seller == a.User.ID && a.isSellerhasAllowedDownloaded == true).Sum(a => a.PurchasedPrice));
                    break;
                case "Price1_desc":
                    sellernotes = sellernotes.OrderBy(x => x.Downloads.Where(a => a.Seller == a.User.ID && a.isSellerhasAllowedDownloaded == true).Sum(a => a.PurchasedPrice));
                    break;
                default:
                    sellernotes = sellernotes.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            ViewBag.users = users.ToList();
            ViewBag.sellernotes = sellernotes.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult UpdateProfile()
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            UserProfile objuserprofile = dbobj.UserProfiles.Where(x => x.UserId == user.ID).FirstOrDefault();

            ViewBag.countrycode = dbobj.Countries.Where(x => x.isActive == true);

            if (objuserprofile != null)
            {
                UpdateProfile userpr1 = new UpdateProfile
                {
                    UserId = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailId = user.EmailId,
                    SecondaryEmailAddress = objuserprofile.SecondaryEmailAddress,
                    Phonenumbercountrycode = objuserprofile.Phonenumbercountrycode,
                    PhoneNumber = objuserprofile.PhoneNumber,

                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                TempData["ProfilePicture"] = objuserprofile.ProfilePicture;
                return View(userpr1);
            }
            else
            {
                UpdateProfile userpr = new UpdateProfile
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
        public ActionResult UpdateProfile(UpdateProfile user1)
        {
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                UserProfile userprofile1 = dbobj.UserProfiles.Where(x => x.UserId == user1.UserId).FirstOrDefault();
                if (userprofile1 != null)
                {
                    userprofile1.User.FirstName = user1.FirstName;
                    userprofile1.User.LastName = user1.LastName;
                    userprofile1.SecondaryEmailAddress = user1.SecondaryEmailAddress;
                    userprofile1.Phonenumbercountrycode = user1.Phonenumbercountrycode;
                    userprofile1.PhoneNumber = user1.PhoneNumber;

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
                        userprofile1.ProfilePicture = "/System Configuration/DefaultImage/DP.jpg";
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

                        SecondaryEmailAddress = user1.SecondaryEmailAddress,
                        Phonenumbercountrycode = user1.Phonenumbercountrycode,
                        PhoneNumber = user1.PhoneNumber,

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
                        userprofile.ProfilePicture = "/System Configuration/DefaultImage/DP.jpg";
                        dbobj.SaveChanges();
                    }
                    dbobj.UserProfiles.Add(userprofile);
                    dbobj.SaveChanges();
                }
            }

            ViewBag.countrycode = dbobj.Countries.Where(x => x.isActive == true);
            return RedirectToAction("AdminDashboard", "Admin");
            return View(user1);

        }
    }
}