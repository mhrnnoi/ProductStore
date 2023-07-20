namespace ProductStore.Application.Interfaces.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
}
