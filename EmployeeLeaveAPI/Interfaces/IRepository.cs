namespace EmployeeLeaveAPI.Interfaces;

public interface IRepository<T>
{
    Task<T?> Create(T entity);
    Task<T?> Get(int id);
    Task<IEnumerable<T?>>? GetAll();
    Task<T?> Update(int id, T entity);
    Task<T?> Delete(int id);
}