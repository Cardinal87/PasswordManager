using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels.Interfaces;
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
    internal partial class AppViewModel : ViewModelBase, ICanChangeDataBase
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

        public event Func<ObservableCollection<ItemViewModelBase>>? OnDataBaseChanged;

        private void LoadViewModelList()
        {
            foreach (var app in dbClient.Load<Models.App>())
            {
                Apps.Clear();
                Apps.Add(new AppItemViewModel(app, new RelayCommand(() => Delete(app)), new RelayCommand(() => Change(app)), new RelayCommand(ShowData),  clipboard));
            }
        }
        private void Delete(Models.App app)
        {
            dbClient.Delete(app);
            LoadViewModelList();
            OnDataBaseChanged?.Invoke();
        }
        private void ShowData()
        {

        }
        private void Change(Models.App app)
        {
            dialogVM = new AppDialogViewModel(app);
            ShowDialog();
        }
        [RelayCommand]
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
                OnDataBaseChanged?.Invoke();
            }
            dialogService.CloseDialog(dialogVM!);
            dialogVM = null;
        }
    }
}
