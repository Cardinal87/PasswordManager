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

        public string GetConnectionString() => "Data Source=passwordmanager.db;";

        public DbSet<WebSite> WebSites { get; private set; }

    }
}
