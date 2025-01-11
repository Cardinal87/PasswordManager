using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Configuration.AppConfiguration
{
    internal class LoggingOptions
    {
        public const string Section = "logging";

        public string Hash { get; set; }
        public string ConnectionString { get; set; }

    }
}
