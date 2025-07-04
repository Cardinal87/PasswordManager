﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AppConfiguration
{
    public class AppAuthorizationOptions
    {
        public const string Section = "Authorization";

        public string Hash { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

    }
}
