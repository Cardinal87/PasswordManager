using Microsoft.EntityFrameworkCore;
using Models.DataConnectors.EFModelsConfiguration;

namespace Models.DataConnectors
{
    public class DatabaseClient : DbContext 
    {
       public DatabaseClient(DbContextOptions<DatabaseClient> options) : base(options) 
       {
            
       }
        public DbSet<WebSiteModel> WebSites { get; set; }
        public DbSet<AppModel> Apps { get; set; }
        public DbSet<CardModel> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WebSiteModelConfiguration());
            modelBuilder.ApplyConfiguration(new AppModelConfiguration());
            modelBuilder.ApplyConfiguration(new CardModelConfiguration());
        }
    }
}
