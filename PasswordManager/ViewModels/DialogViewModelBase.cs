using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    internal abstract class DialogViewModelBase : ViewModelBase
    {
        protected abstract bool CanClose();
        public abstract void Close();
    }
}
