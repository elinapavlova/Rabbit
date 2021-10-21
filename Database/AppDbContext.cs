using Microsoft.EntityFrameworkCore;
using Models.Responses;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Response> Responses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Response>(response =>
            {
                response.Property(r => r.Message).IsRequired().HasMaxLength(500);
            });
        }
    }
}