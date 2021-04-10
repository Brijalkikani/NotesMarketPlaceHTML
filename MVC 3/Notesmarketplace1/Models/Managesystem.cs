using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class Managesystem
    {
        
        
        public string SupportEmailid{ get; set; }
        public string Password { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Linkedin { get; set; }

        public HttpPostedFileBase ProfilePicture { get; set; }
        public HttpPostedFileBase DisplayPicture { get; set; }
        
        public string FileName { get; set; }
        public string FilePath { get; set; }
       

    }
}