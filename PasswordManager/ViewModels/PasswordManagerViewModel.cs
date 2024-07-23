using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    internal class PasswordManagerViewModel : ViewModelBase
    {
        public PasswordManagerViewModel(DataConnectors.IDataBaseConnector con)
        {
            connector = con;
        }
        public DataConnectors.IDataBaseConnector connector;
    }
}
