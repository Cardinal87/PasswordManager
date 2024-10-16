using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AppViewModel
{
    internal class AppViewModel : ViewModelBase
    {
        AppViewModel(IClipBoardService clipboard, IDataBaseClient dbClient, IDialogService dialogService)
        {
            this.clipboard = clipboard;
            this.dbClient = dbClient;
            this.dialogService = dialogService;
            LoadViewModelList();

            DeleteCommand = new RelayCommand<Models.App>(Delete);
            ChangeCommand = new RelayCommand<Models.App>(Change);
                
            
        }
        IClipBoardService clipboard;
        IDataBaseClient dbClient;
        IDialogService dialogService;
        ObservableCollection<AppItemViewModel> Apps { get; set; } = new ObservableCollection<AppItemViewModel>();
        RelayCommand<Models.App> DeleteCommand;
        RelayCommand<Models.App> ChangeCommand;


        private void LoadViewModelList()
        {
            foreach (var app in dbClient.Load<Models.App>())
            {
                Apps.Clear();
                Apps.Add(new AppItemViewModel(app, DeleteCommand, ChangeCommand, clipboard));
            }
        }
        public void Delete(Models.App? app)
        {
            dbClient.Delete(app!);
            LoadViewModelList();
        }
        public void Change(Models.App? app)
        {
            
        }
        public void ShowDialog()
        {

        }

    }
}
