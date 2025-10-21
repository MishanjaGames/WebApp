using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    public class Grp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public IEnumerable<IdentityUser> Users { get; set; }

        public IEnumerable<Subj> Subjects { get; set; }
        public IEnumerable<Status> Subjects_Status { get; set; }
        private string UsersToString()
        {
            string _r = string.Empty;
            int _it = 1;
            foreach (var user in Users) { _r += user.Id + user.UserName + user.Email + user.PhoneNumber + "\n"; }
            return _r;
        }
        public override string ToString()
        {
            return $"Id: {Id}" +
                $"Name: {Name}" +
                $"List of Students: " +
                $"Id | Name | Email | Phone" +
                UsersToString();
        }
    }
}
