using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PasswordManager.Views;
using PasswordManager.Models;

namespace PasswordManager.DataConnectors
{
    internal class WebSitesTableConnector : IDataBaseConnector
    {
        
        public void Delete(int id)
        {

            string connectionStr = IDataBaseConnector.GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM WebSites WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
            connection.Close();
            

        }

        public List<WebSite> Load()
        {
            string connectionStr = IDataBaseConnector.GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            List<WebSite> resources = new List<WebSite>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM WebSites", connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = Convert.ToInt32(reader["ID"]);
                string name = reader.GetString(1);
                string? login = reader.GetString(2);
                string password = reader.GetString(3);
                WebSite res = new WebSite(id, name, login, password);
                resources.Add(res);
            }
            reader.Close();
            connection.Close();
            return resources;
        }

        public void Save(WebSite resourse)
        {
            string connectionStr = IDataBaseConnector.GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO WebSites (Name, Login, Password) VALUES (@Name, @Loign, @Password", connection);
            cmd.Parameters.AddWithValue("@Name", resourse.Name);
            cmd.Parameters.AddWithValue("@Login", resourse.Login);
            cmd.Parameters.AddWithValue("@Password", resourse.Password);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void Update(WebSite resourse)
        {
            string connectionStr = IDataBaseConnector.GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionStr);
            connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE WebSites SET Name=@Name,Login=@Loign,Password=@Password WHERE Id=@Id", connection);
            cmd.Parameters.AddWithValue("@Name", resourse.Name);
            cmd.Parameters.AddWithValue("@Login", resourse.Login);
            cmd.Parameters.AddWithValue("@Password", resourse.Password);
            cmd.Parameters.AddWithValue("@Id", resourse.Id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        
    }
}
