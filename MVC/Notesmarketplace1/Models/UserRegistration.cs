using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notesmarketplace1.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage ="Plz Enter First name")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])[A-Za-z]{1,}",ErrorMessage ="Enter Valid first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Plz Enter Last name")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])[A-Za-z]{1,}", ErrorMessage = "Enter Valid last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = " Plz Enter EmailID")]
        [EmailAddress]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Invalid Email")]
        public string EmailId { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(24)]
        public string Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage ="Password and Confirm Password does not match")]
        public string ConfirmPassword { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}