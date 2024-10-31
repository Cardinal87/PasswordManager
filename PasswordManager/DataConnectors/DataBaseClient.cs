using Microsoft.EntityFrameworkCore;

using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PasswordManager.DataConnectors
{
    internal class DataBaseClient : DbContext, IDatabaseClient 
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=passwordmanager.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
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
                list.Remove(list.First(x => x.Id == model.Id));
                list.Add(model);
            }
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
