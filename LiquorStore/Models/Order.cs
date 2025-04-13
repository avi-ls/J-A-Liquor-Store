using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace LiquorStore.Models
{
    [Table("Orders")]
    public class Order
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [Required]
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public Order()
        {

        }
    }
}