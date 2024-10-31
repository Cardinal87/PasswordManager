using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.Models;
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
        public AppViewModel(IDatabaseClient dbclient, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            this.itemFactory = itemFactory;
            this.dbClient = dbclient;
            this.dialogService = dialogService;
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            LoadViewModelList();
        }
        IItemViewModelFactory itemFactory;
        IDatabaseClient dbClient;
        IDialogService dialogService;
        private ItemViewModelBase? currentItem;
        public ObservableCollection<AppItemViewModel> Apps { get; set; } = new ObservableCollection<AppItemViewModel>();
        

        AppDialogViewModel? dialogVM;

        public RelayCommand AddNewCommand { get; set; }

        public ItemViewModelBase? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(new PropertyChangedExtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        
        private void Delete(Models.App model)
        {
            var a = Apps.FirstOrDefault(x => x.Id == model.Id);
            if (a != null)
            {
                dbClient.Delete(model);
                Apps.Remove(a);
            }
            dbClient.Save();

        }
        private void ShowDataOfItem(ItemViewModelBase vm)
        {
            CurrentItem = vm;
        }
        private void ShowChangeDialog(Models.App app)
        {
            dialogVM = new AppDialogViewModel(app);
            ShowDialog();
        }
        private void ShowAddNewDialog()
        {
            dialogVM = new AppDialogViewModel();
            ShowDialog();
        }

        private void ShowDialog()
        {
            if (dialogVM != null)
            {
                dialogVM.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(dialogVM);
            }
        }
        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender != null)
            {
                AppDialogViewModel vm = (AppDialogViewModel)sender;
                Models.App model = new(vm.Name ,vm.Password, false);
                if (vm.IsNew)
                {
                    dbClient.Insert(model);
                    dbClient.Save();
                    AppItemViewModel item = itemFactory.CreateAppItem(model, new RelayCommand(() => Delete(model)), new RelayCommand(() => ShowChangeDialog(model)), ShowDataOfItem);
                    Apps.Add(item);

                }
                else
                {
                    dbClient.Replace(model);
                    var a = Apps.FirstOrDefault(x => x.Id == model.Id);
                    a?.UpdateModel(model);
                }


            }
            dialogService.CloseDialog(dialogVM!);
            dialogVM = null;
        }

        private void LoadViewModelList()
        {
            foreach (var model in dbClient.GetListOfType<Models.App>())
            {
                var item = itemFactory.CreateAppItem(model, new RelayCommand(() => Delete(model)), new RelayCommand(() => ShowChangeDialog(model)), ShowDataOfItem);
                Apps.Add(item);
            }
        }
    }
}
