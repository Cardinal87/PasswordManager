using Microsoft.Extensions.Options;


namespace PasswordManager.ViewModels.Services
{
    public interface IWritableOptions<T> : IOptions<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
