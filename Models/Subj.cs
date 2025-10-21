using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    public enum Status
    {
        Canceled = -1,
        Default,
        InProgress,
        Complete
    }
    public class Subj
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(1025)]
        public string Description { get; set; } = "Not Given";
        [Required]
        public IdentityUser Tutor { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}" +
                $"Name: {Name}" +
                $"Description: {Description}" +
                $"Tutor: {Name}";
        }
    }
}
