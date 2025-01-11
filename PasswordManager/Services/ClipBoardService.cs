using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    internal class ClipboardService : IClipboardService
    {
        private static IClipboard GetClipBoard()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            {
                return window.Clipboard!;
            }
            else return null!;
        }

        public void SaveToClipBoard(string text)
        {
            var clipboard = GetClipBoard();
            clipboard.SetTextAsync(text);
        }
    }
}
