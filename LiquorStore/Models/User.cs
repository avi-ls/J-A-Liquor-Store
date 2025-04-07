using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace LiquorStore.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a first name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} chatacers long", MinimumLength = 6)]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter a last name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} chatacers long", MinimumLength = 6)]
        public String LastName { get; set; }


        [EmailAddress (ErrorMessage ="The email address is not valid")]
        [Required(ErrorMessage = "Email Address is required")]

        public String Email { get; set; }

        [Required (ErrorMessage ="Please enter a phone number")]
        [RegularExpression(@"^([1-9]{1}[0-9]{2}-[0-9]{3}-[0-9]{4})$", ErrorMessage = "Invalid format.")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        [Remote ("IsUserNameAvailable", "AccountController", ErrorMessage = "Username already exists")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public String Password { get; set; }



        public User()
        {

        }



    }
}
