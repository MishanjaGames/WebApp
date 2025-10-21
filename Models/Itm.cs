using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    public class Itm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public bool IsAvaliable { get; set; }

        [Required]
        public int SalePerc { get; set; }

        [Required]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        [Required]
        [Precision(10, 2)]
        public decimal OldPrice { get; set; }

        public byte[]? Image { get; set; }
        public string? ImgFormat { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}" +
                $"Name: {Name}" +
                $"Description: {Description}" +
                $"Price: {OldPrice}" +
                $"Sale discount: {SalePerc}" +
                $"New Price: {(OldPrice * SalePerc) / 100}" +
                $"Is in Stock: {IsAvaliable}";
        }
    }
}
