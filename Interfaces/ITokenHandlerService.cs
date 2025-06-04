
using System.Net;

namespace Interfaces
{
    public interface ITokenHandlerService
    {
        event Action TokenExpired;
        bool IsExpired { get;}
        string? Token { get; }
        Task<HttpStatusCode> FetchToken(string password);
        void HandleExpirate();

    }
}
