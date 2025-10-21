using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public class uCntx : IdentityDbContext
    {
        public uCntx(DbContextOptions<uCntx> options) : base(options) { }
    }
}
