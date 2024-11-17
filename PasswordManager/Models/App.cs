using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class App : ModelBase
    {
        public App(string name, string password, bool isFavourite) 
        {
            Name = name;
            Password = password;
            IsFavourite = isFavourite;
        }
        
        public string Password { get;private set; }
        
    }
}
