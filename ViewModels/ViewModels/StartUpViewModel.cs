﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

using ViewModels.Services;
using Models.DataConnectors;

using ViewModels.Services.AppConfiguration;
using ViewModels.BaseClasses;

using System.Security.Cryptography;
using System.Text;


namespace ViewModels
{
    public class StartUpViewModel : ViewModelBase
    {
        
        IServiceCollection _services;
        AppAuthorizationOptions _options;
        private ViewModelBase? currentPage;

        public StartUpViewModel(IWritableOptions<AppAuthorizationOptions> options, IServiceCollection services)
        {
            _options = options.Value;
            _services = services;
            MenuViewModel = new MenuViewModel(options, StartApp);
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

        private async Task StartApp(string password)
        {

            string key = await EncodingKeys.GetEcryptionKey(password, _options.Salt);
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
            
            var vm = await MainViewModel.CreateAsync(provider);
            MainViewModel = vm;
            CurrentPage = MainViewModel;
            

        }
        
        

    }
}
