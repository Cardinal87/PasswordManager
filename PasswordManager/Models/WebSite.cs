using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class WebSite : ModelBase
    {
        public WebSite(string name, string login, string password, string webAddress, bool isFavourite)
        {
            Name = name;
            Login = login;
            Password = password;
            
            WebAddress = webAddress;
            IsFavourite = isFavourite;
        }
        
        public string Login { get;  set; }
        public string Password { get;  set; }
        public string WebAddress { get;  set; }
    }
}
