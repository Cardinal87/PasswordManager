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

namespace PasswordManager.ViewModels.AppViewModels
{
    internal partial class AppViewModel : ViewModelBase
    {
        public AppViewModel(IDataBaseClient dbClient, IDialogService dialogService, IClipBoardService clipboard)
        {
            this.clipboard = clipboard;
            this.dbClient = dbClient;
            this.dialogService = dialogService;
            LoadViewModelList();
        }
        IClipBoardService clipboard;
        IDataBaseClient dbClient;
        IDialogService dialogService;
        private ItemViewModelBase? currentItem;
        public ObservableCollection<AppItemViewModel> Apps { get; set; } = new ObservableCollection<AppItemViewModel>();
        

        AppDialogViewModel? dialogVM;

        

        public ItemViewModelBase? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(new PropertyChangedEtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        private void LoadViewModelList()
        {
            foreach (var app in dbClient.Load<Models.App>())
            {
                Apps.Clear();
                Apps.Add(new AppItemViewModel(app, new RelayCommand(() => Delete(app)), new RelayCommand(() => Change(app)), ShowData,  clipboard));
            }
        }
        private void Delete(Models.App app)
        {
            dbClient.Delete(app);
            LoadViewModelList();
            
        }
        private void ShowData(ItemViewModelBase vm)
        {
            CurrentItem = vm;
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
                
            }
            dialogService.CloseDialog(dialogVM!);
            dialogVM = null;
        }
    }
}
