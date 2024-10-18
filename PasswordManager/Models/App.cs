using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class App
    {
        public App(string name, string password, bool isFavourite) 
        {
            Name = name;
            Password = password;
            IsFavourite = isFavourite;
        }
        public string Name { get; private set; }
        public string Password { get;private set; }
        public bool IsFavourite {  get; private set; }
    }
}
