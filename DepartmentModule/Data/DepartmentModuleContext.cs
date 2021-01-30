using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DepartmentModule.Models;

namespace DepartmentModule.Data
{
    public class DepartmentModuleContext : DbContext
    {
        public DepartmentModuleContext (DbContextOptions<DepartmentModuleContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }


        public DbSet<DepartmentModule.Models.Book> Book { get; set; }


        public DbSet<DepartmentModule.Models.Subject> Subject { get; set; }
    }
}
