using Notesmarketplace1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity;
using Ionic.Zip;
using System.Net.Mail;
namespace Notesmarketplace1.Controllers
{
    public class NotesController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        // GET: Notes
        [Authorize]
        public ActionResult addnotes()
        {
            ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
            ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            return View();
        }
        [HttpPost]
        public ActionResult addnotes(UserSellernotes note)
        {
            if (ModelState.IsValid)
            {
                var Emailid = User.Identity.Name.ToString();
                User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
                Referencedata rf = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status").FirstOrDefault();
                SellerNote objsellernote = new SellerNote
                {
                    SellerID = user.ID,
                    Status = rf.Id,
                    Title = note.Title,
                    Category = note.Category,
                    NoteType = note.NoteType,
                    Professor = note.Professor,
                    Description = note.Description,
                    IsPaid = note.IsPaid,
                    NumberofPages = note.NumberofPages,
                    UniversityName = note.UniversityName,
                    Country = note.Country,
                    Course = note.Course,
                    CourseCode = note.CourseCode,
                    SellingPrice = note.SellingPrice,
                    CreatedDate = DateTime.Now,
                    IsActive = true

                };
                dbobj.SellerNotes.Add(objsellernote);
                dbobj.SaveChanges();
                var noteID = objsellernote.Id;
                //display picture
                //generate path to store image
                string storepath = Path.Combine(Server.MapPath("/UploadFiles/" + user.ID), noteID.ToString());
                //check for directory, if not exist ,then create it 
                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }
                if (note.DisplayPicture != null && note.DisplayPicture.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(note.DisplayPicture.FileName);
                    string extension = Path.GetExtension(note.DisplayPicture.FileName);
                    _FileName = "DP_" + DateTime.Now.ToString("yymmssfff") + extension;
                    string finalpath = Path.Combine(storepath, _FileName);
                    note.DisplayPicture.SaveAs(finalpath);
                    objsellernote.DisplayPicture = Path.Combine(("/UploadFiles/" + user.ID + "/" + noteID + "/"), _FileName);
                    dbobj.SaveChanges();
                }
                else
                {
                    objsellernote.DisplayPicture = "/System Configuration/DefaultImage/4.jpg";
                    dbobj.SaveChanges();
                }
                //upload notes
                string storeuploadpath = Path.Combine(storepath, "Attachments");
                //check for directory, if not exist ,then create it 
                if (!Directory.Exists(storeuploadpath))
                {
                    Directory.CreateDirectory(storeuploadpath);
                }
                SellerNotesAttachement objsellernoteattachment = new SellerNotesAttachement
                {
                    NoteID = noteID,
                    IsActive = true,
                    CreatedBy = user.ID,
                    Createddate = DateTime.Now
                };
                //
                int count = 1;
                var UploadnoteFilePath = "";
                var UploadnoteFileName = "";
                foreach (var File in note.UploadNotes)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(File.FileName);
                    string extension = Path.GetExtension(File.FileName);
                    _FileName = "Attachment" + count + "_" + DateTime.Now.ToString("ddmyyyy") + extension;
                    string finalpath = Path.Combine(storeuploadpath, _FileName);
                    File.SaveAs(finalpath);
                    UploadnoteFileName += _FileName + ";";
                    UploadnoteFilePath += Path.Combine(("/UploadFiles/" + user.ID + "/" + noteID + "/Attachment/"), _FileName);
                    count++;
                }
                objsellernoteattachment.FileName = UploadnoteFileName;
                objsellernoteattachment.FilePath = UploadnoteFilePath;
                dbobj.SellerNotesAttachements.Add(objsellernoteattachment);
                dbobj.SaveChanges();
                //notes preview
                if (note.IsPaid == true)
                {
                    if (note.NotesPreview == null)
                    {
                        ViewBag.ErrorMessage = "plz upload preview";
                        ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
                        ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
                        ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
                        return View(note);
                    }
                }

                if (note.NotesPreview != null && note.NotesPreview.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(note.NotesPreview.FileName);
                    string extension = Path.GetExtension(note.NotesPreview.FileName);
                    _FileName = "Preview_" + DateTime.Now.ToString("yymmssfff") + extension;
                    string finalpath = Path.Combine(storepath, _FileName);
                    note.NotesPreview.SaveAs(finalpath);
                    objsellernote.NotesPreview = Path.Combine(("/UploadFiles/" + user.ID + "/" + noteID + "/"), _FileName);
                    dbobj.SaveChanges();
                }
                return RedirectToAction("Dashboard", "Notes");
            }

            ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
            ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            return View(note);
        }
        public ActionResult Editnote(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);
            SellerNotesAttachement objsellernoteattachment = dbobj.SellerNotesAttachements.Where(x => x.NoteID == Id).FirstOrDefault();
            UserSellernotes editnote = new UserSellernotes
            {
                ID = sellrnote.Id,
                Title = sellrnote.Title,
                Category = sellrnote.Category,
                Description = sellrnote.Description,
                IsPaid = sellrnote.IsPaid,
                NoteType = sellrnote.NoteType,
                NumberofPages = sellrnote.NumberofPages,
                UniversityName = sellrnote.UniversityName,
                Country = sellrnote.Country,
                Course = sellrnote.Course,
                CourseCode = sellrnote.CourseCode,
                Professor = sellrnote.Professor,
                SellingPrice = sellrnote.SellingPrice,
                CreatedDate = sellrnote.CreatedDate
            };
            if (sellrnote == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
            ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            return View(editnote);
        }
        [HttpPost]

        public ActionResult Editnote(int? Id, UserSellernotes editnote, string SAVE)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);
            sellrnote.Title = editnote.Title;
            sellrnote.Category = editnote.Category;
            sellrnote.Description = editnote.Description;
            sellrnote.IsPaid = editnote.IsPaid;
            sellrnote.NoteType = editnote.NoteType;
            sellrnote.NumberofPages = editnote.NumberofPages;
            sellrnote.UniversityName = editnote.UniversityName;
            sellrnote.Country = editnote.Country;
            sellrnote.Course = editnote.Course;
            sellrnote.CourseCode = editnote.CourseCode;
            sellrnote.Professor = editnote.Professor;
            sellrnote.SellingPrice = editnote.SellingPrice;
            sellrnote.ModifiedDate = DateTime.Now;
            sellrnote.IsActive = true;

            if (SAVE == "save")
            {
                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "draft").Select(x => x.Id).FirstOrDefault();
            }
            else
            {
                sellrnote.Status = dbobj.Referencedatas.Where(x => x.RefCategory.ToLower() == "note status" && x.value.ToLower() == "submitted for review").Select(x => x.Id).FirstOrDefault(); ;
            }

            dbobj.Entry(sellrnote).State = EntityState.Modified;

            dbobj.SaveChanges();
            if (sellrnote == null)
            {
                return HttpNotFound();
            }

            ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
            ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            return RedirectToAction("Dashboard", "Notes");
            return View(sellrnote);
        }

        [Authorize]
        public ActionResult Dashboard(string searchtext, string currentFilter, int? page, string sortorder, string search, string sort, int? pagee, string currentFilter1)
        {
            //For Progress notes
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Statusparam = sortorder == "Status" ? "Status_desc" : "Status";
            if (searchtext != null)
            {
                page = 1;
            }
            else
            {
                searchtext = currentFilter;
            }
            ViewBag.CurrentFilter = searchtext;

            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
             List<SellerNote> sellerNotes = dbobj.SellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatas = dbobj.Referencedatas.Where(x => x.RefCategory == "note status" && x.value != "rejected" && x.value != "removed").ToList();
            var tabledetails = from s in sellerNotes
                               join n in noteCategories on s.Category equals n.Id into table1
                               from n in table1.ToList()
                               join r in referencedatas on s.Status equals r.Id into table2
                               from r in table2.ToList()
                               where (r.value != "published")
                               select new DashboardClass
                               { sellernotedetail = s, notecategorydetail = n, referencedatadetail = r };
            if (!String.IsNullOrEmpty(searchtext))
            {
                tabledetails = tabledetails.Where(x => x.notecategorydetail.name.Contains(searchtext) ||
                x.referencedatadetail.value.Contains(searchtext) || x.sellernotedetail.Title.Contains(searchtext) || searchtext == null);
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
                default:
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.CreatedDate);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.tabledetails = tabledetails.ToList().ToPagedList(pageNumber, pageSize);
            //For Published Notes
            ViewBag.CurrentSort1 = sort;
            ViewBag.AddedDateparamPublished = string.IsNullOrEmpty(sort) ? "Date Desc1" : "";
            ViewBag.TitleparamPublished = sort == "Title1" ? "Title_desc1" : "Title1";
            ViewBag.CategoryparamPublished = sort == "Category1" ? "Category_desc1" : "Category1";
            ViewBag.SelltypeparamPublished = sort == "Type" ? "Type_desc" : "Type";
            ViewBag.PricepeparamPublished = sort == "Price" ? "Price_desc" : "Price";
            if (search != null)
            {
                pagee = 1;
            }
            else
            {
                search = currentFilter1;
            }

            ViewBag.CurrentFilter1 = search;
            List<SellerNote> sellerNotesPublished = dbobj.SellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true).ToList();
            List<NoteCategory> noteCategoriesPublished = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatasPublished = dbobj.Referencedatas.Where(x => x.RefCategory == "note status" && x.value != "rejected" && x.value != "removed").ToList();
            var publishednotes = from s in sellerNotesPublished
                                 join n in noteCategoriesPublished on s.Category equals n.Id into table1
                                 from n in table1.ToList()
                                 join r in referencedatasPublished on s.Status equals r.Id into table2
                                 from r in table2.ToList()
                                 where (r.value == "published")
                                 select new DashboardClass
                                 { sellernotedetail = s, notecategorydetail = n, referencedatadetail = r };
            if (!String.IsNullOrEmpty(search))
            {
                publishednotes = publishednotes.Where(x => x.notecategorydetail.name.Contains(search) || x.sellernotedetail.Title.Contains(search) || search == null);
            }
            switch (sort)
            {

                case "Date Desc1":
                    publishednotes = publishednotes.OrderByDescending(x => x.sellernotedetail.CreatedDate);
                    break;
                case "Title1":
                    publishednotes = publishednotes.OrderByDescending(x => x.sellernotedetail.Title);
                    break;
                case "Title_desc1":
                    publishednotes = publishednotes.OrderBy(x => x.sellernotedetail.Title);
                    break;
                case "Category1":
                    publishednotes = publishednotes.OrderByDescending(x => x.notecategorydetail.name);
                    break;
                case "Category_desc1":
                    publishednotes = publishednotes.OrderBy(x => x.notecategorydetail.name);
                    break;
                case "Type":
                    publishednotes = publishednotes.OrderByDescending(x => x.sellernotedetail.IsPaid);
                    break;
                case "Type_desc":
                    publishednotes = publishednotes.OrderBy(x => x.sellernotedetail.IsPaid);
                    break;
                case "Price":
                    publishednotes = publishednotes.OrderByDescending(x => x.sellernotedetail.SellingPrice);
                    break;
                case "Price_desc":
                    publishednotes = publishednotes.OrderBy(x => x.sellernotedetail.SellingPrice);
                    break;
                default:
                    publishednotes = publishednotes.OrderByDescending(x => x.sellernotedetail.CreatedDate);
                    break;

            }
            
            ViewBag.publishednotes = publishednotes.ToList().ToPagedList(pagee ?? 1, 5);
            return View();
        }
        public ActionResult Delete(int id)
        {
            var del1 = (from Seller1 in dbobj.SellerNotesAttachements where Seller1.NoteID == id select Seller1).FirstOrDefault();
            var del = (from Seller in dbobj.SellerNotes where Seller.Id == id select Seller).FirstOrDefault();
            dbobj.SellerNotesAttachements.Remove(del1);
            dbobj.SellerNotes.Remove(del);
            dbobj.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        public ActionResult Searchnote(string searchtext, int? page, string NoteType, string Category, string UniversityName, string Course, string Country)
        {
           
            List<SellerNote> sellerNotesPublished = dbobj.SellerNotes.OrderBy(x => x.Title).ToList();
            List<NoteCategory> noteCategoriesPublished = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatasPublished = dbobj.Referencedatas.Where(x => x.RefCategory == "note status" && x.value == "published").ToList();
            List<SellerNotesReview> sellerNotesreviewPublished = dbobj.SellerNotesReviews.ToList();
            UserProfile objuserprofile = dbobj.UserProfiles.FirstOrDefault();
            var publishednotes = from s in sellerNotesPublished
                                 join n in noteCategoriesPublished on s.Category equals n.Id into table1
                                 from n in table1.ToList()
                                 join r in referencedatasPublished on s.Status equals r.Id into table2
                                 from r in table2.ToList()
                                 
                                 where (r.value == "published" && ((s.NoteType.ToString() == NoteType || String.IsNullOrEmpty(NoteType))
                                 && (s.Category.ToString() == Category || String.IsNullOrEmpty(Category)) &&
                                 (s.UniversityName == UniversityName || String.IsNullOrEmpty(UniversityName))
                                 && (s.Course == Course || String.IsNullOrEmpty(Course))
                                 && (s.Country.ToString() == Country || String.IsNullOrEmpty(Country))))

                                 select new SearchClass
                                 {
                                     sellernotedetail = s,
                                     notecategorydetail = n,
                                     referencedatadetail = r,
                                    
                                 };
            if (!String.IsNullOrEmpty(searchtext))
            {
                publishednotes = publishednotes.Where(x => x.sellernotedetail.Title.Contains(searchtext) || searchtext == null);

            }
            ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
            ViewBag.University = dbobj.SellerNotes.Where(x => x.UniversityName != null).Select(x => x.UniversityName).Distinct();
            ViewBag.Course = dbobj.SellerNotes.Where(x => x.Course != null).Select(x => x.Course).Distinct();
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            ViewBag.Rating = dbobj.SellerNotesReviews.Where(x => x.IsActive == true);
            TempData["ProfilePicture"] = objuserprofile.ProfilePicture;
            ViewBag.Totalcount = publishednotes.Count();
            return View(publishednotes.ToPagedList(page ?? 1, 9));
        }

        public ActionResult NotesDetail(int Id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);
            SellerNotesAttachement objsellernoteattachment = dbobj.SellerNotesAttachements.Where(x => x.NoteID == Id).FirstOrDefault();
            return View(sellrnote);
        }
        [Authorize]
        public ActionResult Download(int Id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);
            SellerNotesAttachement objsellernoteattachment = dbobj.SellerNotesAttachements.Where(x => x.NoteID == Id).FirstOrDefault();
            NoteCategory notecategory = dbobj.NoteCategories.Where(x => x.Id == sellrnote.Category).FirstOrDefault();
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            if (sellrnote.IsPaid == false)
            {
                Download download = new Download
                {
                    NoteId = sellrnote.Id,
                    Seller = sellrnote.SellerID,
                    downloader = user.ID,
                    isSellerhasAllowedDownloaded = true,
                    AttachmentPath = objsellernoteattachment.FilePath,
                    IsAttachmentDownloaded = true,
                    AttachmentDownloadedDate = DateTime.Now,
                    Ispaid = sellrnote.IsPaid,
                    PurchasedPrice = sellrnote.SellingPrice,
                    NoteTitle = sellrnote.Title,
                    NoteCategory = notecategory.name,
                    Createddate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedBy = user.ID,
                    isActive = true
                };
                dbobj.Downloads.Add(download);
                dbobj.SaveChanges();
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(Server.MapPath("~/UploadFiles/" + sellrnote.SellerID + "/" + Id + "/" + "Attachments"));
                    MemoryStream output = new MemoryStream();
                    zip.Save(output);
                    return File(output.ToArray(), "Attachments/zip", "Note.zip");
                }
            }
            else
            {
                User objuser = dbobj.Users.Where(x => x.ID == sellrnote.SellerID).FirstOrDefault();
                SendEmailtoSeller(objuser.EmailId.ToString());
                Download download = new Download
                {
                    NoteId = sellrnote.Id,
                    Seller = sellrnote.SellerID,
                    downloader = user.ID,
                    isSellerhasAllowedDownloaded = false,
                    AttachmentDownloadedDate = DateTime.Now,
                    Ispaid = sellrnote.IsPaid,
                    PurchasedPrice = sellrnote.SellingPrice,
                    NoteTitle = sellrnote.Title,
                    NoteCategory = notecategory.name,
                    Createddate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedBy = user.ID,
                    isActive = true
                };
                dbobj.Downloads.Add(download);
                dbobj.SaveChanges();
            }
            return RedirectToAction("Searchnote", "Notes");
        }
        public void SendEmailtoSeller(string emailID)
        {
            
            User objuser = dbobj.Users.Where(x => x.EmailId==emailID).FirstOrDefault();
            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
            var fromEmail = new MailAddress("kikanibrijal23@gmail.com", "Notemarketplace");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "mtnapkudprykaqpe"; // Replace with actual password
            string subject = user.FirstName + " " + "wants to purchase your notes";
            string body = "Hello " +" " + objuser.FirstName + "<br/>";
            body += "<br/>We would like to inform you that"+ " " + user.FirstName + "  wants to purchase your notes.Please see" +" "+
                "Buyer Requests tab and allow Download access to Buyer if you have received the payment  from him.<br/>";
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


