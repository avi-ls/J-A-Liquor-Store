using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LiquorStore.Models
{
    public class LoginModel
    {

            [Required(ErrorMessage = "Please Enter a first name")]
            public String UserName { get; set; }

            [Required(ErrorMessage = "Please Enter a password")]
            [DataType(DataType.Password)]
            public String Password { get; set; }


        
    }
}
