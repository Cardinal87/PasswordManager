using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Configuration.OptionExtensions
{
    internal interface IWritableOptions<T> : IOptions<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
