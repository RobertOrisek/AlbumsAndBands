using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZavrsniTest.Models;

namespace ZavrsniTest.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Band> Bands { get; set; }
        public DbSet<Album> Albums { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Band>().HasData(
                new Band() { Id = 1, Name = "Pink Floyd", FoundationYear = 1965 },
                new Band() { Id = 2, Name = "Europe", FoundationYear = 1979 },
                new Band() { Id = 3, Name = "The Doors", FoundationYear = 1965 }
            );

            builder.Entity<Album>().HasData(
                new Album() { Id = 1, Name = "LA Woman", PublishingYear = 1971, Genre = "rock", CopiesSold = 4, BandId = 3 },
                new Album() { Id = 2, Name = "The Wall", PublishingYear = 1979, Genre = "art rock", CopiesSold = 30, BandId = 1 },
                new Album() { Id = 3, Name = "The Final Countdown", PublishingYear = 1986, Genre = "glam metal", CopiesSold = 4, BandId = 2 },
                new Album() { Id = 4, Name = "Meddle", PublishingYear = 1971, Genre = "rock", CopiesSold = 2, BandId = 1 },
                new Album() { Id = 5, Name = "Strange Days", PublishingYear = 1969, Genre = "rock", CopiesSold = 1, BandId = 3 }
            );
            base.OnModelCreating(builder);
        }
    }
}
