using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VitaCraft.Models;

namespace VitaCraft.Data
{ 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<PortFolio> PortFolios { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Projectt> Projectts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PortFolio>()
                .HasOne(p => p.EndUser)
                .WithMany(e => e.portFolios)
                .HasForeignKey(p => p.EndUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Projectt>()
                .HasOne(p => p.portFolio)
                .WithMany(e => e.projects)
                .HasForeignKey(p => p.portFolioID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Service>()
                .HasOne(p => p.portFolio)
                .WithMany(e => e.services)
                .HasForeignKey(p => p.portFolioID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Resume>()
                .HasOne(p => p.EndUser)
                .WithMany(e => e.resumes)
                .HasForeignKey(p => p.EndUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Education>()
                .HasOne(p => p.resume)
                .WithMany(e => e.educations)
                .HasForeignKey(p => p.resumeID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Experience>()
                .HasOne(p => p.resume)
                .WithMany(e => e.experiences)
                .HasForeignKey(p => p.resumeID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Skill>()
                .HasOne(p => p.resume)
                .WithMany(e => e.skills)
                .HasForeignKey(p => p.resumeID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Skill>()
                .HasOne(p => p.portFolio)
                .WithMany(e => e.skills)
                .HasForeignKey(p => p.portFolioId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Language>()
                .HasOne(p => p.resume)
                .WithMany(e => e.languages)
                .HasForeignKey(p => p.resumeID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Certificate>()
               .HasOne(p => p.resume)
               .WithMany(e => e.certificates)
               .HasForeignKey(p => p.resumeID)
               .OnDelete(DeleteBehavior.Cascade);



        }


    }

}

