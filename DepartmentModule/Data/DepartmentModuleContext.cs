using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DepartmentModule.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace DepartmentModule.Data
{
    public class DepartmentModuleContext : IdentityDbContext
    {
        public DepartmentModuleContext (DbContextOptions<DepartmentModuleContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Subject>()
                .HasOne(x => x.Program)
                .WithMany(x => x.Programs)
                .HasForeignKey(x => x.ProgramID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Subject>()
                .HasOne(x => x.Themes)
                .WithMany(x => x.Themess)
                .HasForeignKey(x => x.ThemesID)
                .OnDelete(DeleteBehavior.Restrict);
            
            //builder.Entity<SubjectLiterature>().HasKey(k => new { k.SubjectId, k.LiteratureId });

            builder.Entity<SubjectLiterature>()
                .HasOne(x => x.Literature)
                .WithMany(x => x.Literatures)
                .HasForeignKey(x => x.LiteratureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubjectLiterature>()
               .HasOne(x => x.Subject)
               .WithMany(x => x.Literatures)
               .HasForeignKey(x => x.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<SubjectAdditionalLiterature>().HasKey(k => new { k.SubjectId, k.AdditionalLiteratureId });

            builder.Entity<SubjectAdditionalLiterature>()
                .HasOne(x => x.AdditionalLiterature)
                .WithMany(x => x.AdditionalLiteratures)
                .HasForeignKey(x => x.AdditionalLiteratureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SubjectAdditionalLiterature>()
               .HasOne(x => x.Subject)
               .WithMany(x => x.AdditionalLiteratures)  
               .HasForeignKey(x => x.SubjectId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Subject>().ToTable("Subject");
            builder.Entity<Book>().ToTable("Book");
            builder.Entity<SubjectLiterature>().ToTable("SubjectLiterature");
            builder.Entity<SubjectAdditionalLiterature>().ToTable("SubjectAdditionalLiterature");
            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Book>())
            {
                if (this.Book.Find(entry) != null)
                    return 0;
            }

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Book>())
            {
                if (this.Book.Find(entry.Entity.BookID) != null)
                    entry.State=EntityState.Detached;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Book> Book { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<SubjectLiterature> SubjectLiterature { get; set; }
        public DbSet<SubjectAdditionalLiterature> SubjectAdditionalLiterature { get; set; }
    }
}
