﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface IClipboardService
    {
        void SaveToClipBoard(string text);
    }
}
