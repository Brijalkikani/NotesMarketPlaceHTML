using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class DownloadModel
    {
        public Download Downloaddetail { get; set; }
        public UserProfile Userprofiledetail { get; set; }
        public User Userdetail { get; set; }
        public SellerNotesReview sellernotesreviewdetail { get; set; }
        
    }
}