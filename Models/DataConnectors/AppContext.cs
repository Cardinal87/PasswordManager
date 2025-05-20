using Microsoft.EntityFrameworkCore;
using Models.DataConnectors.EFModelsConfiguration;

namespace Models.DataConnectors
{
    class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppModelConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppModel> Apps { get; set; }

        public IEnumerable<AppModel> List()
        {
            return Apps.ToList();
        }

        public AppModel? GetById(int id)
        {
            return Apps.FirstOrDefault(t => t.Id == id);
        }

        public void Delete(AppModel model)
        {
            Apps.Remove(model);
        }

        public AppModel Update(AppModel model, int id)
        {
            var old_model = GetById(id);
            if (old_model == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} was not found");
            }
            old_model.Name = model.Name;
            old_model.Password = model.Password;
            old_model.IsFavourite = model.IsFavourite;
            return old_model;

        }

        public void Insert(AppModel model)
        {
            Apps.Add(model);
        }
    }
}
