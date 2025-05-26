using Microsoft.EntityFrameworkCore;
using Models.DataConnectors.EFModelsConfiguration;
using System.Linq.Expressions;



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
        public IEnumerable<T> GetListOfType<T>() where T : ModelBase
        {
            var list = Set<T>();
            return list;
        }

        public async Task<IEnumerable<TEntity>> GetListOfTypeAsync<TEntity>() where TEntity : ModelBase
        {
            var list = await Task.Run(() => Set<TEntity>().ToList());
            return list;

        }

        public void Insert<T>(T model) where T : ModelBase
        {
            var list = Set<T>();
            list.Add(model);
        }

        public void Delete<T>(T model) where T : ModelBase
        {
            var list = Set<T>();
            list.Remove(model);
        }

        public void Replace<T>(T model) where T : ModelBase
        {
            var list = Set<T>();
            var excist = list.FirstOrDefault(x => x.Id == model.Id);
            if (excist != null)
            {
                list.Remove(excist);
                list.Add(model);
            }
        }
        
        public async Task<IEnumerable<T>> GetByPredicateAsync<T>(Expression<Func<T, bool>> pred) where T: ModelBase
        {
            var set = Set<T>().Where(pred);
            var list = await Task.Run(() => set.ToList());
            return list;
        }

        public T? GetById<T>(int id) where T : ModelBase
        {
            var list = Set<T>();
            return list.FirstOrDefault(x => x.Id == id);
        }

        public async Task<T?> GetByIdAsync<T>(int id) where T : ModelBase
        {
            var list = Set<T>();
            return await Task.Run(() => list.FirstOrDefault(x => x.Id == id));
        }

        public async Task SaveChangesAsync()
        {
            await Task.Run(() => base.SaveChanges());
        }

        private new DbSet<TEntity> Set<TEntity>() where TEntity : ModelBase
        {
            return base.Set<TEntity>();
        }


    }
}
