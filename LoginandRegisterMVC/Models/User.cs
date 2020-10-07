using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;

namespace LoginandRegisterMVC.Models
{
    public class User
    {
        [Key]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserId { get; set; }
        [Required]
        [Display(Name ="Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        //[StringLength(100, ErrorMessage = "The {0} must be atleast {2} characters long.", MinimumLength = 8)]
        //[MembershipPassword(MinRequiredNonAlphanumericCharacters = 1, MinNonAlphanumericCharactersError = "Your Password needs to contain atleast !,@,#,$ etc.", ErrorMessage = "Your Password must be 8 characters long and contain atleast one symbol(!,@,#,etc).")]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password required")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name ="Role")]
        public string Role { get; set; }
    }
}