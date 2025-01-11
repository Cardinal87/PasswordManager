using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager.Configuration.EFModels;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PasswordManager.DataConnectors
{
    internal class DatabaseClient : DbContext 
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
