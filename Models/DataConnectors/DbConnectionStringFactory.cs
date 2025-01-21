using Microsoft.Data.Sqlite;
using System.Net.Http.Headers;
using System.Text;

namespace Models.DataConnectors
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

        public string? ConnectionString { get; set; }


    }
}
