using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class DashboardClass
    {
        public SellerNote sellernotedetail { get; set; }
        public NoteCategory notecategorydetail { get; set; }
        public Referencedata referencedatadetail { get; set; }
        public User userdetail { get; set; }
        
    } 
    
}