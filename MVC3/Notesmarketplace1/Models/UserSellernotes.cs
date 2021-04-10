using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class UserSellernotes
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="plz enter title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please Select Note Category")]
        public int Category { get; set; }

        public HttpPostedFileBase DisplayPicture { get; set; }
        [Required(ErrorMessage = "Please upload a Note")]
        public List<HttpPostedFileBase> UploadNotes { get; set; }
        public Nullable<int> NoteType { get; set; }
        public Nullable<int> NumberofPages { get; set; }

        [Required(ErrorMessage = "plz discript your note")]
        public string Description { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        [Required(ErrorMessage = "Please Select type")]
        public bool IsPaid { get; set; }
        
        public Nullable<decimal> SellingPrice { get; set; }
        public HttpPostedFileBase NotesPreview { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

    }
}