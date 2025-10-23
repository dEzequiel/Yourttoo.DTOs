using Microsoft.EntityFrameworkCore;
using Yourttoo.Api.DataAccess.Configuration;
using Yourttoo.Api.DataAccess.Configuration.Tagging;
using Yourttoo.Api.DataAccess.Configuration.FrequentlyAskedQuestions;
using Yourttoo.Api.DataAccess.Configuration.Authentication;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;
using Yourttoo.DTOs.Models.Authentication;
using Yourttoo.DTOs.Models.Geolocation;
using Yourttoo.Api.DataAccess.Configuration.Geolocation;

namespace Yourttoo.Api.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AdditionalText> AdditionalText { get; set; }
        public DbSet<FAQSection> FAQSection { get; set; }
        public DbSet<FAQContent> FAQContent { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TagEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdditionalTextEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FAQSectionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FAQContentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CountryEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RegionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CityEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AirportEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ZoneEntityConfiguration());
        }
    }
}
