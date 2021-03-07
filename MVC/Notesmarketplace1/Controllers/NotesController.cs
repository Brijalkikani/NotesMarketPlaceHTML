using Notesmarketplace1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using PagedList.Mvc;


namespace Notesmarketplace1.Controllers
{
    public class NotesController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        // GET: Notes
        [Authorize]
        public ActionResult addnotes()
        {
            //var a = dbobj.NoteCategories.Select(i => i.isActive).ToList();
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
                string storepath = Path.Combine(Server.MapPath("~/UploadFiles/" + user.ID), noteID.ToString());
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


                    objsellernote.DisplayPicture = Path.Combine(("~/UploadFiles/" + user.ID + "/" + noteID + "/"), _FileName);
                    dbobj.SaveChanges();
                }
                else
                {
                    objsellernote.DisplayPicture = "~/C:/Users/Admin/source/repos/Notesmarketplace1/SystemDefault/1.jpg";
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
                    UploadnoteFilePath += Path.Combine(("~/UploadFiles/" + user.ID + "/" + noteID + "/Attachment/"), _FileName);

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


                    objsellernote.NotesPreview = Path.Combine(("~/UploadFiles/" + user.ID + "/" + noteID + "/"), _FileName);
                    dbobj.SaveChanges();
                }

            


                return RedirectToAction("Dashboard", "Notes");

            }

            ViewBag.NoteCategory = dbobj.NoteCategories.Where(x => x.isActive == true);
            ViewBag.NoteType = dbobj.NoteTypes.Where(x => x.IsActive == true);
            ViewBag.Country = dbobj.Countries.Where(x => x.isActive == true);
            return View(note);

        }
        public ActionResult Editnote( int? Id)
        {
           if(Id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);
            if(sellrnote == null)
            {
                return HttpNotFound();
            }
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
                SellingPrice = sellrnote.SellingPrice
            };
          
            ViewBag.NoteCategory =new SelectList(dbobj.NoteCategories.Where(x => x.isActive == true), "ID", "name", sellrnote.NoteCategory);
            ViewBag.NoteType = new SelectList(dbobj.NoteTypes.Where(x => x.IsActive == true), "ID", "Name", sellrnote.NoteType);
            ViewBag.Country = new SelectList(dbobj.Countries.Where(x => x.isActive == true),"ID","name",sellrnote.Country);
            return View(editnote);
        }


        [Authorize]
        public ActionResult Dashboard(string searchtext)
        {


            var Emailid = User.Identity.Name.ToString();
            User user = dbobj.Users.Where(x => x.EmailId == Emailid).FirstOrDefault();
           List<SellerNote> sellerNotes = dbobj.SellerNotes.OrderByDescending(x=>x.CreatedDate).Where(x =>x.SellerID==user.ID && x.IsActive == true && (x.Title.Contains(searchtext) || searchtext == null)).ToList();
            List<NoteCategory> noteCategories = dbobj.NoteCategories.ToList();
            List<Referencedata> referencedatas = dbobj.Referencedatas.Where(x=>x.RefCategory == "note status" && x.value != "rejected" && x.value != "removed").ToList();
            var tabledetails = from s in sellerNotes
                               join n in noteCategories on s.Category equals n.Id into table1
                               from n in table1.ToList()
                               join r in referencedatas on s.Status equals r.Id into table2
                               from r in table2.ToList()
                               where (r.value !="published")
                               select new DashboardClass
                               { sellernotedetail = s, notecategorydetail = n, referencedatadetail = r };

            return View(tabledetails);

        }
        public ActionResult Searchnote()
        {

            return View();
        }
        public ActionResult NotesDetail()
        {

            return View();
        }

    }
}

