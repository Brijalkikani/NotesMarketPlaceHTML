using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class Forgotpwd
    {
        

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string EmailId { get; set; }
    }
}