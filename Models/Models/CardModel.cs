
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PasswordManager.Models.Models
{
    public class CardModel : ModelBase
    {
        
        
        public CardModel(string number, string month, string year, string cvc, string owner, string name, bool isFavourite) : base(name, isFavourite)
        {
            Number = number;
            Year = year;
            Month = month;
            Cvc = cvc;
            Owner = owner;
        }

        public string Number { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Cvc { get; set; }
        public string Owner { get; set; }
        
    }
}
