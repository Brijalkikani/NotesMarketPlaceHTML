using Ionic.Zip;
using Notesmarketplace1.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Notesmarketplace1.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult SpamReport(string search, int? page, string sortorder, string currentFilter)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Reportedbyparam = sortorder == "name" ? "name_desc" : "name";
            ViewBag.Descriptionparam = sortorder == "lname" ? "lname_desc" : "lname";

            var users = dbobj.SellerNotesReportedIssues.Where(x => x.SellerNote.Title.Contains(search) || x.SellerNote.NoteCategory.name.Contains(search) || x.CreatedDate.ToString().Contains(search)
            || x.Remarks.Contains(search) || x.User.SellerNotesReportedIssues.Where(a => a.ReportedByid == a.User.ID).Select(a => a.User.FirstName).Contains(search) || search == null);
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
                case "Title":
                    users = users.OrderByDescending(x => x.SellerNote.Title);
                    break;
                case "Title_desc":
                    users = users.OrderBy(x => x.SellerNote.Title);
                    break;
                case "name":
                    users = users.OrderByDescending(x => x.User.FirstName);
                    break;
                case "name_desc":
                    users = users.OrderBy(x => x.User.FirstName);
                    break;
                case "Category":
                    users = users.OrderByDescending(x => x.SellerNote.NoteCategory.name);
                    break;
                case "Category_desc":
                    users = users.OrderBy(x => x.SellerNote.NoteCategory.name);
                    break;
                case "lname":
                    users = users.OrderByDescending(x => x.Remarks);
                    break;
                case "lname_desc":
                    users = users.OrderBy(x => x.Remarks);
                    break;
                default:
                    users = users.OrderBy(x => x.CreatedDate);
                    break;
            }

            return View(users.ToList().ToPagedList(page ?? 1, 5));
        }
        public ActionResult Delete(int id)
        {
            var del = (from report in dbobj.SellerNotesReportedIssues where report.NoteId == id select report).FirstOrDefault();
            dbobj.SellerNotesReportedIssues.Remove(del);
            dbobj.SaveChanges();
            return RedirectToAction("SpamReport");
        }
        public ActionResult Download(int Id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Where(x => x.Id == Id && x.Referencedata.value == "published").FirstOrDefault();
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(Server.MapPath("~/UploadFiles/" + sellrnote.SellerID + "/" + Id + "/" + "Attachments"));
                MemoryStream output = new MemoryStream();
                zip.Save(output);
                return File(output.ToArray(), "Attachments/zip", "Note.zip");
            }
            return View();
        }
    }
}