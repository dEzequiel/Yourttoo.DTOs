using Microsoft.EntityFrameworkCore;
using Yourttoo.Api.DataAccess.Configuration;
using Yourttoo.Api.DataAccess.Configuration.Tagging;
using Yourttoo.Api.DataAccess.Configuration.FrequentlyAskedQuestions;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;
using Yourttoo.DTOs.Models.Geolocation;
using Yourttoo.Api.DataAccess.Configuration.Geolocation;
using Yourttoo.DTOs.Models.Users;
using Yourttoo.Api.DataAccess.Configuration.User;

namespace Yourttoo.Api.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<AdditionalText> AdditionalText { get; set; }
        public DbSet<FAQSection> FAQSection { get; set; }
        public DbSet<FAQContent> FAQContent { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AccountEntityConfiguration());

            modelBuilder.ApplyConfiguration(new TagEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdditionalTextEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FAQSectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FAQContentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CountryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RegionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CityEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AirportEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ZoneEntityConfiguration());
        }
    }
}
