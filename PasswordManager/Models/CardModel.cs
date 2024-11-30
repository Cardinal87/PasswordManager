using PasswordManager.ViewModels.CardViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    internal class CardModel : ModelBase
    {
        
        
        public CardModel(string number, int month, int year, int cvc, string owner, string name, bool isFavourite) : base(name, isFavourite)
        {
            Number = number;
            Year = year;
            Month = month;
            Cvc = cvc;
            Owner = owner;
        }

        public string Number { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Cvc { get; set; }
        public string Owner { get; set; }
        
    }
}
