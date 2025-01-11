using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    internal interface IClipboardService
    {
        void SaveToClipBoard(string text);
    }
}
