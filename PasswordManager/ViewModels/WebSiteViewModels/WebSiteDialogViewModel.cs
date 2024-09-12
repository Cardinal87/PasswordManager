using CommunityToolkit.Mvvm.Input;
using PasswordManager.ViewModels.DialogInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteDialogViewModel : ViewModelBase, IDialogViewModel, IDialogResultHelper
    {

        private bool dialogResult;
        public WebSiteDialogViewModel()
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
        }

        public WebSiteDialogViewModel(WebSitesItemViewModel item)
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            Name = item.Name;
            WebAddress = item.WebAddress;
            Login = item.Login;
            Password = item.Password;
        }


        public string? Name { get; private set; }
        public string? WebAddress { get; private set; }
        public string? Login { get; private set; }
        public string? Password { get; private set; }

        RelayCommand AddCommand;
        RelayCommand CloseCommand;

        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;

        public void Add()
        {
            dialogResult = true;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }

        public bool CanClose()
        {
            return true;
        }

        public void Close()
        {
            dialogResult = false;
            if (CanClose())
            {
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
            }
        }
    }
}
