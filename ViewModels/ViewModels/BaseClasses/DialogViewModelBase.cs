using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.BaseClasses
{
    public abstract class DialogViewModelBase : ViewModelBase
    {
        public abstract bool CanClose { get;}
        protected abstract void Close();
    }
}
