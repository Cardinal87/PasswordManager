using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class Card : ModelBase
    {
        public Card(string number, int month, int year, string cVC, string owner)
        {
            Number = number;
            ValidUntil = new DateTime(year, month, 1);
            CVC = cVC;
            Owner = owner;
        }

        public string Number { get; set; }
        public DateTime ValidUntil { get; set; }
        public string CVC { get; set; }
        public string Owner { get; set; }
    }
}
