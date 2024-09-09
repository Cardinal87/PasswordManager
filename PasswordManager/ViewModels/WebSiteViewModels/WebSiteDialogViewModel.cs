using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteFormViewModel : DialogViewModelBase
    {
        public WebSiteFormViewModel()
        {

        }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        [RelayCommand]
        public void Add()
        {

        }
    }
}
