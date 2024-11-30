using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PasswordManager.DataConnectors
{
    internal class DataBaseClient : DbContext, IDatabaseClient 
    {
       public DataBaseClient() 
       {
            Database.EnsureCreated();
       }
        
        
        public DbSet<WebSiteModel> WebSites { get; set; }
        public DbSet<Models.AppModel> Apps { get; set; }
        public DbSet<CardModel> Cards { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=passwordmanager.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebSiteModel>().ToTable("WebSites").HasKey(e => e.Id);

            modelBuilder.Entity<Models.AppModel>().ToTable("Apps").HasKey(e => e.Id);
            
            modelBuilder.Entity<CardModel>().ToTable("Cards").HasKey(e => e.Id);
           


        }
        public IEnumerable<T> GetListOfType<T>() where T : ModelBase
        {
            var list = Set<T>();
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
        public T? GetById<T>(int id) where T : ModelBase
        {
            var list = Set<T>();
            return list.FirstOrDefault(x => x.Id == id);
        }
        public void Save()
        {
            SaveChanges();
        }

        private new DbSet<TEntity> Set<TEntity>() where TEntity : ModelBase
        {
            return base.Set<TEntity>();
        }
    }
}
