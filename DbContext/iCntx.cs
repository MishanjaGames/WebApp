using Microsoft.EntityFrameworkCore;

namespace WebApp
{
    public class iCntx :DbContext
    {
        public DbSet<Itm> Items { get; set; }
        public iCntx(DbContextOptions<iCntx> options) : base(options) { }
    }
}
