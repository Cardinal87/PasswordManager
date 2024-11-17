using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class ModelBase
    {
        
        public int Id { get; protected set; }
        public string? Name { get; protected set; }
        public bool IsFavourite { get; protected set; }
    }
}
