﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class WebSite
    {
        public WebSite(int id, string name, string? login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
            Id = id;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Login { get; private set; }
        public string Password { get; private set; }
    }
}