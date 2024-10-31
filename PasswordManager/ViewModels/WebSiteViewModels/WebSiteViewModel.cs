using Avalonia.Markup.Xaml.MarkupExtensions;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.Interfaces;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteViewModel : ViewModelBase
    {

        public WebSiteViewModel(IDatabaseClient dbClient, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            this.itemFactory = itemFactory;
            this.dbClient = dbClient;
            this.dialogService = dialogService;
            AddNewCommand = new RelayCommand(ShowAddNewDialog);

            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
            LoadViewModelsList();  
        }
        private IItemViewModelFactory itemFactory;
        private IDialogService dialogService;
        private IDatabaseClient dbClient;
        private ItemViewModelBase? currentItem;
        public RelayCommand AddNewCommand { get; }


        public WebSiteDialogViewModel? Dialog { get; private set; }
        public ObservableCollection<WebSiteItemViewModel> WebSites { get; private set; }
        
        public ItemViewModelBase? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(new PropertyChangedExtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        private void ShowChangeDialog(WebSite model)
        {
            Dialog = new WebSiteDialogViewModel(model);
            ShowDialog();
        }
        
        private void ShowAddNewDialog() 
        { 
            Dialog = new WebSiteDialogViewModel();
            ShowDialog();
        }
        private void ShowDataOfItem(ItemViewModelBase vm)
        {
            CurrentItem = vm;
        }

        private void Delete(WebSite model)
        {
            var a = WebSites.FirstOrDefault(x => x.Id == model.Id);
            if (a != null) 
            {
                dbClient.Delete(model);
                WebSites.Remove(a);
            }
            dbClient.Save();
        }

        private void ShowDialog()
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(Dialog!);
            }
        }


        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender != null)
            {
                WebSiteDialogViewModel vm = (WebSiteDialogViewModel)sender;
                WebSite model = vm.Model!;
                
                if (vm.IsNew)
                {
                    dbClient.Insert(model);
                    dbClient.Save();
                    WebSiteItemViewModel item = itemFactory.CreateWebSiteItem(model, new RelayCommand(() => Delete(model)), new RelayCommand(() => ShowChangeDialog(model)), ShowDataOfItem);
                    WebSites.Add(item);
                }
                else
                {
                    dbClient.Replace(model);
                    var a = WebSites.FirstOrDefault(x => x.Id == model.Id);
                    a?.UpdateModel(model);
                    dbClient.Save();
                }
                
            }
            dialogService.CloseDialog(Dialog!);
            Dialog = null;
        }

        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.GetListOfType<WebSite>())
            {
                var item = itemFactory.CreateWebSiteItem(a, new RelayCommand(() => Delete(a)), new RelayCommand(() => ShowChangeDialog(a)), ShowDataOfItem);
                WebSites.Add(item);
            }

        }
    }    
    
}
