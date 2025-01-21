using Microsoft.Data.Sqlite;
using Models.DataConnectors;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.Services;
using ViewModels.Services.AppConfiguration;
namespace Extension.WebAPI.Services
{
    public class DbConnectionStringFactory
    {

        private static DbConnectionStringFactory? instance;
        
        public static DbConnectionStringFactory GetInstance()
        {
            if (instance == null)
            {
                var inst = new DbConnectionStringFactory();
                instance = inst;
            }
            return instance;
        }
        
        private DbConnectionStringFactory() 
        {
            
        }

        public string ConnectionString { get; set; } = string.Empty;


    }
}
