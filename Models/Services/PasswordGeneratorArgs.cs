using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Services
{
    public class PasswordGeneratorArgs
    {
        public PasswordGeneratorArgs(int length, bool activateDigits, bool activateSpecSymbs, bool activateUpcase, bool activateLowcase)
        {
            Length = length;
            ActivateDigits = activateDigits;
            ActivateSpecSymbs = activateSpecSymbs;
            ActivateUpcase = activateUpcase;
            ActivateLowcase = activateLowcase;
        }

        public int Length { get; set; }
        public bool ActivateUpcase { get; set; }
        public bool ActivateLowcase { get; set; }
        public bool ActivateDigits { get; set; }
        public bool ActivateSpecSymbs { get; set; }

    }
}
