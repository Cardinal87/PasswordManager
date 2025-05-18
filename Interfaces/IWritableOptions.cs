using Microsoft.Extensions.Options;


namespace Interfaces
{
    public interface IWritableOptions<T> : IOptions<T> where T : class, new()
    {
        public void Update(Action<T> applyChanges);
    }
}
