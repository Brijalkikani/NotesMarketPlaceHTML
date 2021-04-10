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
    public class AdminController : Controller
    {
        private readonly notemarketplaceEntities dbobj = new notemarketplaceEntities();
        // GET: Admin
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AdminDashboard(string searchtext, string currentFilter, int? page, string sortorder,string Month)
        {
            ViewBag.CurrentSort = sortorder;
            ViewBag.AddedDateparam = string.IsNullOrEmpty(sortorder) ? "Date Desc" : "";
            ViewBag.Titleparam = sortorder == "Title" ? "Title_desc" : "Title";
            ViewBag.Categoryparam = sortorder == "Category" ? "Category_desc" : "Category";
            ViewBag.Selltypeparam = sortorder == "Type" ? "Type_desc" : "Type";
            ViewBag.Attachmentparam = sortorder == "Size" ? "Size_desc" : "Size";
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
                               where (r.value == "published" && ((s.Publisheddate.ToString()==Month || String.IsNullOrEmpty(Month))))
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
                 x.userdetail.FirstName.Contains(searchtext) || searchtext == null);
            }
            switch (sortorder)
            {
                case "Date Desc":
                    tabledetails = tabledetails.OrderByDescending(x => x.sellernotedetail.Publisheddate);
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
            List<SelectListItem> month = new List<SelectListItem>();
            for( int i=0;i<=5; i++)
            {
                var day = DateTime.Now.AddMonths(-i);
                month.Add(new SelectListItem()
                {
                    Text = day.Date.ToString("MMMM") + " ",
                    Value = day.Month.ToString() + "_"

                }) ; 
            }
            var today = DateTime.Today;
            var last = today.AddDays(-7);
            ViewBag.InReview = dbobj.SellerNotes.Where(x => x.Referencedata.value.ToLower() == "in review").Count();
            ViewBag.DownloadNotes = dbobj.Downloads.Where(x => x.isSellerhasAllowedDownloaded == true && x.Createddate >= last).Count();
            ViewBag.Registration = dbobj.Users.Where(x => x.IsEmailVerified == true && x.CreatedDate >= last && x.UserRole.Name == "Member").Count();
            ViewBag.Month = month;

            return View(tabledetails.ToPagedList(page ?? 1, 5));
        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public ActionResult AdminNotesDetail(int Id)
        {
            SellerNote sellrnote = dbobj.SellerNotes.Find(Id);
            SellerNotesAttachement objsellernoteattachment = dbobj.SellerNotesAttachements.Where(x => x.NoteID == Id).FirstOrDefault();
            return View(sellrnote);
        }

        public ActionResult DeleteReview(int id)
        {
            var del = (from report in dbobj.SellerNotesReviews where report.NoteId == id select report).FirstOrDefault();
            dbobj.SellerNotesReviews.Remove(del);
            dbobj.SaveChanges();
            return RedirectToAction("AdminDashboard", "Admin");
        }

    }
}