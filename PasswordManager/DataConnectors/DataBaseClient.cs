using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataConnectors
{
    internal class DataBaseClient: DbContext, IDataBaseClient
    {
        public DataBaseClient() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(GetConnectionString());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebSite>().UseTpcMappingStrategy();  // Используем стратегию TPC
        }


        public string GetConnectionString() => "Data Source=passwordmanager.db;";

        public void Save(string vmName, object model)
        {
            throw new NotImplementedException();
        }

        public void Delete(string vmName, int id)
        {
            throw new NotImplementedException();
        }

        public void Update(string vmName, object model)
        {
            throw new NotImplementedException();
        }

        public DbSet<TEntity> GetList<TEntity>()
        {
            return null;
        } 

        public DbSet<WebSite> WebSites { get; private set; }

    }
}
