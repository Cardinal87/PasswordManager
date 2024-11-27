﻿using Microsoft.EntityFrameworkCore;
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
        
        
        public DbSet<WebSite> WebSites { get; set; }
        public DbSet<Models.App> Apps { get; set; }
        public DbSet<Card> Cards { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=passwordmanager.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebSite>().ToTable("WebSites").HasKey(e => e.Id);

            modelBuilder.Entity<Models.App>().ToTable("Apps").HasKey(e => e.Id);
            
            modelBuilder.Entity<Card>().ToTable("Cards").HasKey(e => e.Id);
           


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
