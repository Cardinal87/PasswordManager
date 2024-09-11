using CommunityToolkit.Mvvm.Input;
using PasswordManager.ViewModels.DialogInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteFormViewModel : ViewModelBase, IDialogViewModel, IDialogResultHelper
    {
        public WebSite Result { get; set; }
        
        public WebSiteFormViewModel()
        {
            AddCommand = new RelayCommand(Add);
        }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        RelayCommand AddCommand;

        public event EventHandler<DialogResultEventArgs> dialogResultRequest;

        public void Add()
        {

        }

        public bool CanClose()
        {
            return true;
        }

        public void Close()
        {
            if (CanClose())
            {

            }
        }
    }
}
