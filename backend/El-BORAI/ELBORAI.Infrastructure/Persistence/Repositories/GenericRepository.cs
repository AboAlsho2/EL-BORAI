using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Specifications;
using ELBORAI.Domain.Entities;
using ELBORAI.Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ELBORAI.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity 
{
    protected readonly ElBoraiDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(ElBoraiDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetBySpecificationAsync(
        BaseSpecification<T> specification)
    {
        return await ApplySpecification(specification)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllBySpecificationAsync(
        BaseSpecification<T> specification)
    {
        return await ApplySpecification(specification)
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    private IQueryable<T> ApplySpecification(
        BaseSpecification<T> specification)
    {
        return SpecificationEvaluator<T>
            .GetQuery(_dbSet.AsQueryable(), specification);
    }
}