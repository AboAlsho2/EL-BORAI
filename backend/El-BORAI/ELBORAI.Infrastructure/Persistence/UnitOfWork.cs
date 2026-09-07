using ELBORAI.Application.Interfaces;

namespace ELBORAI.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ElBoraiDbContext _context;

    public UnitOfWork(ElBoraiDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}