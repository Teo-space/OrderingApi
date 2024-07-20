using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace Interfaces.DbContexts;

public interface IBaseDbContext
{
    DatabaseFacade Database { get; }

    ChangeTracker ChangeTracker { get; }

    int SaveChanges();
    int SaveChanges(bool acceptAllChangesOnSuccess);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

    public DbSet<TEntity> Set<TEntity>() where TEntity : class;

    TEntity? Find<TEntity>(params object?[]? keyValues) where TEntity : class;
    ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues) where TEntity : class;
    IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression);

    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;

    void AddRange(params object[] entities);
    void AttachRange(params object[] entities);
    void UpdateRange(params object[] entities);
    void RemoveRange(params object[] entities);
}
