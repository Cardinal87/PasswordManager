using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PasswordManager.DataConnectors
{
    internal class DataBaseConnector : IDataConnector
    {
        public void Delete(int id)
        {
            
            SqlConnection connection = GetConnection();
            SqlCommand cmd = new SqlCommand("DELETE FROM Resources WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            connection.Close();

        }

        public List<Resource> Load()
        {
            SqlConnection connection = GetConnection();
            List<Resource> resources = new List<Resource>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Resoursces", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                string name = reader.GetString(1);
                string? login = reader.GetString(2);
                string password = reader.GetString(3);
                Resource res = new Resource(id, name, login, password);
                resources.Add(res);
            }
            reader.Close();
            connection.Close();
            return resources;
        }

        public void Save(Resource resourse)
        {
            SqlConnection connection = GetConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO Resources (Name, Login, Password) VALUES (@Name, @Loign, @Password",connection);
            cmd.Parameters.AddWithValue("@Name", resourse.Name);
            cmd.Parameters.AddWithValue("@Login", resourse.Login);
            cmd.Parameters.AddWithValue("@Password", resourse.Password);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(Resource resourse)
        {
            SqlConnection connection = GetConnection();
            SqlCommand cmd = new SqlCommand("UPDATE Resources SET Name=@Name,Login=@Loign,Password=@Password WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Name", resourse.Name);
            cmd.Parameters.AddWithValue("@Login", resourse.Login);
            cmd.Parameters.AddWithValue("@Password", resourse.Password);
            cmd.Parameters.AddWithValue("@Id", resourse.Id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private SqlConnection GetConnection()
        {
            string connectionString = "Server=DESKTOP-SQHQMVO\\SQLEXPRESS;DataBase=PasswordManagers;TrustedConnection=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
