using Microsoft.CodeAnalysis;

using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataConnectors
{
    internal interface ITableConnector
    {
        
        public List<WebSite> Load();
        public void Save(WebSite resourse);
        public void Update(WebSite resourse);
        public void Delete(int id);
        static public string GetConnectionString() => "Data Source=passwordManager.db";
       
            
       
    }
}
