using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DepartmentModule.Data
{
    public class DepartmentModuleContext : IdentityDbContext
    {
        public DepartmentModuleContext (DbContextOptions<DepartmentModuleContext> options)
            : base(options)
        {
          //  Database.EnsureCreated();
        }


        public DbSet<DepartmentModule.Models.Book> Book { get; set; }


        public DbSet<DepartmentModule.Models.Subject> Subject { get; set; }
    }
}
