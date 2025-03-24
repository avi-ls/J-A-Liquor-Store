using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorStore.Models
{
    [Table("Employees")]
    public class Employee
    {
        public Employee () { }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        [Required]
        public string Position { get; set; }

        [Required]
        public double Salary { get; set; }
        


    }
}

