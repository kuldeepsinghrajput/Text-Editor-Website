using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using test_project.Models;

namespace test_project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<SavedText> SavedTexts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SavedText>()
                .HasOne(text=>text.User)
                .WithMany()
                .HasForeignKey(text=>text.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
