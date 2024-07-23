using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.DataConnectors
{
    internal interface IDataBaseConnector
    {
        public List<Resource> Load();
        public void Save(Resource resourse);
        public void Update(Resource resourse);
        public void Delete(int id);
        static public SqlConnection GetConnection()
        {
            string connectionString = "Server=DESKTOP-SQHQMVO\\SQLEXPRESS;DataBase=PasswordManagers;TrustedConnection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
