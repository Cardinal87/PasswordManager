using Microsoft.EntityFrameworkCore;
using Models.DataConnectors.EFModelsConfiguration;

namespace Models.DataConnectors
{
    public class WebSiteContext : DbContext
    {
        public WebSiteContext(DbContextOptions<WebSiteContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WebSiteModelConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<WebSiteModel> WebSites { get; set; }

        public IEnumerable<WebSiteModel> List()
        {
            return WebSites.ToList();
        }

        public WebSiteModel? GetById(int id)
        {
            return WebSites.FirstOrDefault(t => t.Id == id);
        }

        public void Delete(WebSiteModel model)
        {
            WebSites.Remove(model);
        }

        public WebSiteModel Update(WebSiteModel model, int id)
        {
            var old_model = GetById(id);
            if (old_model == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} was not found");
            }
            old_model.Name = model.Name;
            old_model.WebAddress = model.WebAddress;
            old_model.Password = model.Password;
            old_model.IsFavourite = model.IsFavourite;
            old_model.Login = model.Login;
            return old_model;

        }

        public void Insert(WebSiteModel model)
        {
            WebSites.Add(model);
        }

    }
}
