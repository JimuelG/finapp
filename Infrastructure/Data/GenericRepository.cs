using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(x => x.Id == id);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T?>> ListAllAsync(CancellationToken cancellationToken)
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Update(T entity)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = context.Set<T>().AsQueryable();

        query = spec.ApplyCriteria(query);

        return await query.CountAsync();
    }
}
