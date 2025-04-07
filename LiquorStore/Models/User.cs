using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using LiquorStore.Models;

namespace LiquorStore.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a first name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 6)]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter a last name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 6)]
        public String LastName { get; set; }


        [EmailAddress (ErrorMessage ="The email address is not valid")]
        [Required(ErrorMessage = "Email Address is required")]

        public String Email { get; set; }

        [Required (ErrorMessage ="Please enter a phone number")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Invalid format.")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a username")]
        [Remote ("IsUserNameAvailable", "UsersController", ErrorMessage = "Username already exists")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public string Role { get; set; } = "User";

        public User()
        {

        }



    }
}
