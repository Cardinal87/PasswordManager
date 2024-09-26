using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataConnectors
{
    internal interface IDataBaseClient
    {
        string GetConnectionString();
        public void Save(string vmName, object model);
        public void Delete(string vmName, int id);
        public void Update(string vmName, object model);
    }
}
