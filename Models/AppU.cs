using Microsoft.AspNetCore.Identity;

namespace WebApp
{
    public class AppU : IdentityUser
    {
        public ICollection<Ord>? Orders { get; set; }
    }
}
