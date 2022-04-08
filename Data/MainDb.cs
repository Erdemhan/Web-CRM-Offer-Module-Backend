using CRMWeb.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using crmweb.Data.Configurations;
using crmweb.Data.Entities;
using CRMWeb.Entities;

namespace crmweb.Data
{
    public sealed class MainDb : DbContext
    {
        //Member Variables/////////////////////////////////////////////////////

        private readonly ILoggerFactory LoggerFactory;

        //Constructor//////////////////////////////////////////////////////////

        public MainDb(DbContextOptions options, ILoggerFactory loggerFactory)
            : base(options)
        {
            LoggerFactory = loggerFactory;
        }

        //Functions////////////////////////////////////////////////////////////

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseLoggerFactory(LoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OfferHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new OfferDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyContactConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());

        }
        
        //Actions//////////////////////////////////////////////////////////////

        //Properties///////////////////////////////////////////////////////////
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        public DbSet<OfferHeader> OfferHeaders { get; set; }

        public DbSet<OfferDetail> OfferDetails { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Company> Appointments { get; set; }
        public DbSet<CompanyContact> CompanyContacts{ get; set; }
        public DbSet<CRMWeb.Entities.Appointment> Appointment { get; set; }
        //Static Properties////////////////////////////////////////////////////

        //Computed Properties//////////////////////////////////////////////////

    }
}
