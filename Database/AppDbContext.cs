using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>(message =>
            {
                message.Property(r => r.Text).IsRequired().HasMaxLength(500);
            });
        }
    }
}