using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorStore.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a first name")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "Please Enter a password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public Account()
        {
        }
    }
}
