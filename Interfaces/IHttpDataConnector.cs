
namespace Interfaces
{
    public interface IHttpDataConnector<T> where T: class
    {
        Task<List<T>> GetList();

        Task<int> Post(T model);

        Task Put(T model, int id);

        Task Delete(int id);
    } 
}
