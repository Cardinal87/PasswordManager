using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Configuration.AppConfiguration;
using PasswordManager.Configuration.OptionExtensions;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.ViewModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    internal class StartUpViewModel : ViewModelBase
    {
        
        IServiceCollection _services;
        LoggingOptions _options;
        private ViewModelBase? currentPage;

        public StartUpViewModel(IWritableOptions<LoggingOptions> options, IServiceCollection services)
        {
            _options = options.Value;
            _services = services;
            MenuViewModel = new MenuViewModel(options);
            MenuViewModel.OnDataBaseEncryptng += StartApp;
            CurrentPage = MenuViewModel;
        }

        public MainViewModel? MainViewModel { get; set; }
        public MenuViewModel? MenuViewModel { get; set; }


        public ViewModelBase? CurrentPage
        {

            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private async void StartApp(string password)
        {
            
            string key = GetEncryptionKey(password);
            _services.AddDbContextFactory<DatabaseClient>(opt =>
            {
                opt.UseSqlite(new SqliteConnectionStringBuilder
                {
                    DataSource = _options.ConnectionString,
                    Password = key
                    
                }.ToString());
            });

            var provider = _services.BuildServiceProvider();
            if (!File.Exists(_options.ConnectionString))
            {
                var factory = provider.GetRequiredService<IDbContextFactory<DatabaseClient>>();
                using var context = factory.CreateDbContext();
                context.Database.EnsureCreated();
            }
            
            var vm = await MainViewModel.CreateAsync(provider.GetRequiredService<IViewModelFactory>());
            MainViewModel = vm;
            CurrentPage = MainViewModel;
            

        }
        
        private string GetEncryptionKey(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                var hashString = BitConverter.ToString(hash).ToLower().Replace("-", "");
                return hashString;
            }

        }

    }
}
