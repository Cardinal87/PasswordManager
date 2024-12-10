using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AllEntriesViewModels
{
    internal class AllEntriesViewModel : ViewModelBase
    {
        
        public AllEntriesViewModel() 
        {
            Items = new ObservableCollection<ItemViewModelBase>();
        }
        
        public ObservableCollection<ItemViewModelBase> Items { get; private set; } = new ObservableCollection<ItemViewModelBase>();
        private ItemViewModelBase? currentItem;
        public ItemViewModelBase? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }
        public void UpdateViewModelList(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is ItemViewModelBase add) Items.Add(add);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is ItemViewModelBase del) Items.Remove(del);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldItems?[0] is ItemViewModelBase old && e.NewItems?[0] is ItemViewModelBase nw)
                    {
                        Items.Remove(old);
                        Items.Add(nw);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems is List<ItemViewModelBase> list)
                    {
                        foreach(var a in list)
                        {
                            Items.Remove(a);
                        }
                    }
                    break;
            }
        }
        public void SetItem(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentItem) && e is PropertyChangedExtendedEventArgs a && a.NewValue != null)
            {
                currentItem = (ItemViewModelBase)a.NewValue;
            }
        } 

        
    }
}
