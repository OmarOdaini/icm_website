using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// Models are new types of vars that are costumizable to fit any design 
namespace icm_first.Models
{
    public class ContactModel   
    {
        [Required (ErrorMessage ="Please Re-enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please use a correct email format")]
        [RegularExpression(@".*@.*", ErrorMessage ="Email format is required")]
        public string Email { get; set; }
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

    }
}