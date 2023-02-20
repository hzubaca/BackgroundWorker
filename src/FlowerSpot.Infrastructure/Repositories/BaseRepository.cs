using FlowerSpot.Infrastructure.Persistence;
using FlowerSpot.SharedKernel.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlowerSpot.Infrastructure.Repositories;
public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly FlowerSpotContext _flowerSpotContext;

    public BaseRepository(FlowerSpotContext flowerSpotContext)
    {
        _flowerSpotContext = flowerSpotContext;
    }

    public async Task<T> Add(T entity)
    {
        await _flowerSpotContext.Set<T>().AddAsync(entity);
        await _flowerSpotContext.SaveChangesAsync();
        return entity;
    }

    public async Task AddMultiple(List<T> entities)
    {
        await _flowerSpotContext.AddRangeAsync(entities);
        await _flowerSpotContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _flowerSpotContext.Entry(entity).State = EntityState.Modified;
        await _flowerSpotContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAll()
    {
        return await _flowerSpotContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetById(int id)
    {
        return await _flowerSpotContext.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetSingleMatch(Expression<Func<T, bool>> predicate)
    {
        return await _flowerSpotContext.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public async Task<IReadOnlyCollection<T>> GetMultipleMatches(Expression<Func<T, bool>> predicate)
    {
        return await _flowerSpotContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task Delete(T entity)
    {
        _flowerSpotContext.Set<T>().Remove(entity);
        await _flowerSpotContext.SaveChangesAsync();
    }
}
