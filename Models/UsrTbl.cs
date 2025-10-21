using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    public class UsrTbl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public IdentityUser User { get; set; }

        [Required]
        public List<string> Roles { get; set; }

        public override string ToString()
        {
            return $"Item #{Id}" +
                $"User: {User.UserName}" +
                $"Roles: {Roles}";
        }
    }
}
