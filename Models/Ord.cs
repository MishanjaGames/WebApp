using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApp
{
    public class Ord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Order date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Total amount")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Status")]
        public string? Status { get; set; }
        public string? UserId { get; set; }
        //Navigation property
        [ForeignKey("UserId")]
        public AppU? User { get; set; }
        //Navigation property
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
