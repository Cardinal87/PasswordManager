﻿namespace PasswordManager.WebAPI.DTO
{
    public class JwtOptions
    {
        public const string Section = "Jwt";
        
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

    }
}
