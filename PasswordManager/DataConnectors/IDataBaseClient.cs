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
        public List<T> Load<T>() where T : class;
        public void Save<T>(T model) where T: class;
        public void Delete<T>(T model) where T : class;
        public void UpdateList<T>(T model) where T : class;
    }
}
