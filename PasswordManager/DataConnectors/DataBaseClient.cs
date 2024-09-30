using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PasswordManager.DataConnectors
{
    internal class DataBaseClient: DbContext, IDataBaseClient
    {
        public DataBaseClient() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(GetConnectionString());
            Collection.Add(nameof(WebSites), WebSites);
        }
        
        public string GetConnectionString() => "Data Source=passwordmanager.db;";

        public void Save<T>(T model) where T : class 
        {
            DbSet<T> dbSet = GetList<T>();
            dbSet.Add(model);
        }

        public void Delete<T>(T model) where T : class
        {
            DbSet<T> dbSet = GetList<T>();
            dbSet.Remove(model);
        }
        
        public void Update<T>(T model) where T: class
        {
            Delete(model);
            Save(model);
        }

        private DbSet<T> GetList<T>() where T : class
        {
            string key = typeof(T).Name.Replace("ViewModel", "");
            var list = Collection[key];
            return (list as DbSet<T>)!;
        }

        public List<T> Load<T>() where T : class
        {
            var dbSet = GetList<T>();
            return dbSet.ToList();
        }

        public Dictionary<string, object> Collection { get; set; } = new Dictionary<string, object>();
        public DbSet<WebSite> WebSites { get; private set; }

    }
}
