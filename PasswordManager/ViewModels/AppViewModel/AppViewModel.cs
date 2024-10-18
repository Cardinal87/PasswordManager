using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels.DialogInterfaces;
using PasswordManager.ViewModels.WebSiteViewModels;
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
        }
        IClipBoardService clipboard;
        IDataBaseClient dbClient;
        IDialogService dialogService;
        ObservableCollection<AppItemViewModel> Apps { get; set; } = new ObservableCollection<AppItemViewModel>();
        

        AppDialogViewModel? dialogVM;

        private void LoadViewModelList()
        {
            foreach (var app in dbClient.Load<Models.App>())
            {
                Apps.Clear();
                Apps.Add(new AppItemViewModel(app, Delete, Change, clipboard));
            }
        }
        public void Delete(Models.App app)
        {
            dbClient.Delete(app);
            LoadViewModelList();
        }
        
        public void Change(Models.App app)
        {
            dialogVM = new AppDialogViewModel(app);
            ShowDialog();
        }
        
        public void AddNew()
        {
            dialogVM = new AppDialogViewModel();
            ShowDialog();
        }

        public void ShowDialog()
        {
            if (dialogVM != null)
            {
                dialogVM.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(dialogVM);
            }
        }
        public void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender != null)
            {
                AppDialogViewModel vm = (AppDialogViewModel)sender;
                Models.App app = new(vm.Name ,vm.Password, false);
                if (vm.IsNew) dbClient.Save(app);
                else dbClient.UpdateList(app);
                LoadViewModelList();
            }
            dialogService.CloseDialog(dialogVM!);
            dialogVM = null;
        }
    }
}
