using Microsoft.Data.Sqlite;
using System.Net.Http.Headers;
using System.Text;

namespace Models.DataConnectors
{
    public class DbConnectionStringSingleton
    {

        private static DbConnectionStringSingleton? instance;

        public static void SetPassword(string password)
        {
            if (instance != null)
            { 
                instance._password = password;
            }
            throw new InvalidOperationException("instance had not been setted");
        }
        public static DbConnectionStringSingleton SetInstance(string datasource)
        {
            var inst = new DbConnectionStringSingleton(datasource);
            instance = inst;
            return inst;
        }

        public static DbConnectionStringSingleton GetInstance()
        {
            if (instance != null) return instance;
            else throw new InvalidOperationException("instance had not been setted");
        }

        private DbConnectionStringSingleton(string datasource)
        {
            _datasource = datasource;
            _password = "";

        }
        private string _datasource;
        private string _password;
        public string? ConnectionString 
        { 
            get
            {
                return new SqliteConnectionStringBuilder
                {
                    ConnectionString = _datasource,
                    Password = _password
                }.ToString();
                
            }
        }


    }
}
