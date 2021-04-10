using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class Userlogin
    {
        [Display(Name ="Email Id")]
        
        [Required(AllowEmptyStrings =false,ErrorMessage ="Email is required")]
        public string EmailId { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Remember Me")]
        
        public bool RememberMe { get; set; }
    }
}