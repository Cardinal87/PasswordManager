using PasswordManager.DataConnectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Factories
{
    internal class ContextFactory : IContextFactory
    {
        public IDatabaseClient CreateContext()
        {
            return new DataBaseClient();
        }
    }
}
