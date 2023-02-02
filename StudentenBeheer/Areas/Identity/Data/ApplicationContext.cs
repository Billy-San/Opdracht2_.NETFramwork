using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentenBeheer.Areas.Identity.Data;

namespace StudentenBeheer.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }
    public DbSet<StudentenBeheer.Models.Student> Student { get; set; }

    public DbSet<StudentenBeheer.Models.Gender> Gender { get; set; }

    public DbSet<StudentenBeheer.Models.Module> Module { get; set; }

    public DbSet<StudentenBeheer.Models.Inschrijvingen> Inschrijvingen { get; set; }

    public DbSet<StudentenBeheer.Models.Language> Language { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
    }

    public DbSet<StudentenBeheer.Models.Docent> Docent { get; set; }
    public DbSet<StudentenBeheer.Models.Docenten_modules> Docenten_modules { get; set; }


}
