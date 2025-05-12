using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions options) : base(options)
        {
        }

     public DbSet<CustomUser> customUsers { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser() { Id = 1, Email = "ysnlms@gmail.com", Password = "password", City = "Ankara", UserName = "yasin42" },
                new CustomUser() { Id = 2, Email = "ahmet35@gmail.com", Password = "password", City = "İzmir", UserName = "ahmet35" },
                new CustomUser() { Id = 3, Email = "mehmet06@gmail.com", Password = "password", City = "Antalya", UserName = "mehmet06" }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
