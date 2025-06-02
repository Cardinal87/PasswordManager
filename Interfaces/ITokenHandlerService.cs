
namespace Interfaces
{
    public interface ITokenHandlerService
    {
        event Action TokenExpired;
        bool IsExpired { get;}
        string? Token { get; }
        Task FetchToken(string password);
        void HandleExpirate();

    }
}
