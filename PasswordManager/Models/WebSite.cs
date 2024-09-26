using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class WebSite : IModel
    {
        public WebSite(int id, string name, string? login, string password, string webAddress, bool isFavourite)
        {
            Name = name;
            Login = login;
            Password = password;
            Id = id;
            WebAddress = webAddress;
            IsFavourite = isFavourite;
        }
        [Key]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Login { get; private set; }
        public string Password { get; private set; }
        public string WebAddress { get; private set; }
        public bool IsFavourite { get; private set; }
    }
}
