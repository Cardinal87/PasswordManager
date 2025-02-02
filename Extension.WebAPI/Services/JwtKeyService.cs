using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Extension.WebAPI.Services
{
    public class JwtKeyService
    {
        private readonly object _lock = new object();
        private byte[] _currentKey;

        public JwtKeyService() 
        { 
            GenerateJwtKey();
        }

        [MemberNotNull(nameof(_currentKey))]
        public void GenerateJwtKey()
        {
            lock (_lock)
            {
                _currentKey = GenerateBytes();
            }
        }
        public byte[] GetJwtKey()
        {
            lock (_lock)
            {
                return _currentKey;
            }
        }

        private byte[] GenerateBytes()
        {
            using var rng = RandomNumberGenerator.Create();
            var key = new byte[64];
            rng.GetBytes(key);
            return key;
        }


    }
}
