using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    
    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> Create(T entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> Get(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T?>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> Update(int id, T entity)
    {
        var entityToUpdate = await _dbSet.FindAsync(id);
        
        if (entityToUpdate == null)
        {
            return null;
        }
        
        _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return entityToUpdate;
    }

    public async Task<T?> Delete(int id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        
        if (entityToDelete == null)
        {
            return null;
        }
        
        _dbSet.Remove(entityToDelete);
        await _context.SaveChangesAsync();
        return entityToDelete;
    }
}