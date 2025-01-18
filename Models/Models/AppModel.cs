﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppModel : ModelBase
    {
        public AppModel(string name, string password, bool isFavourite) : base (name, isFavourite)
        {
            Password = password;
        }
        
        public string Password { get; set; }
        
    }
}
