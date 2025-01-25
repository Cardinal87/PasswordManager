using Microsoft.Data.Sqlite;
using System.Net.Http.Headers;
using System.Text;

namespace Models.DataConnectors
{
    public class DbConnectionStringSingleton
    {

        private static DbConnectionStringSingleton? instance;

        public static void SetCreditals(string password, string datasource)
        {
            if (instance == null)
            {
                instance = new DbConnectionStringSingleton();
            }
            instance._password = password;
            instance._datasource = datasource;
        }
        
        public static DbConnectionStringSingleton GetInstance()
        {
            if (instance != null) return instance;
            else
            {
                instance = new DbConnectionStringSingleton();
                return instance;
            }
        }

        private DbConnectionStringSingleton()
        {
            
        }
        private string _datasource = "";
        private string _password = "";
        public string? ConnectionString 
        { 
            get
            {
                
                return new SqliteConnectionStringBuilder
                {
                    DataSource = _datasource,
                    Password = _password
                }.ToString();
               
            }
        }


    }
}
