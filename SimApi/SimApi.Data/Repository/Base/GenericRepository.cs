using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimApi.Base;
using SimApi.Data.Context;
using System.Linq.Expressions;

namespace SimApi.Data.Repository;

public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : BaseModel
{
    protected readonly SimDbContext dbContext;
    private bool disposed;

    public GenericRepository(SimDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void Delete(Entity entity)
    {
        dbContext.Set<Entity>().Remove(entity);
    }

    public void DeleteById(int id)
    {
        var entity = dbContext.Set<Entity>().Find(id);
        dbContext.Set<Entity>().Remove(entity);
    }

    public IEnumerable<Entity> GetAll(params Expression<Func<Entity, object>>[] includes)
    {
        IQueryable<Entity> query = dbContext.Set<Entity>().Include(includes[0]);
        foreach(var include in includes.Skip(1))
        {
            query = query.Include(include);
        }
        return query.ToList();
    }

    public Entity GetById(int id, params Expression<Func<Entity, object>>[] includes)
    {
        IQueryable<Entity> query = dbContext.Set<Entity>().Include(includes[0]);
        foreach (var include in includes.Skip(1))
        {
            query = query.Include(include);
        }
        return query.FirstOrDefault(x => x.Id == id);
    }

    public void Insert(Entity entity)
    {
        dbContext.Set<Entity>().Add(entity);
    }

    public void Update(Entity entity)
    {
        dbContext.Set<Entity>().Update(entity);
    }

    public IEnumerable<Entity> Where(Expression<Func<Entity, bool>> expression)
    {
        return dbContext.Set<Entity>().Where(expression).AsQueryable();
    }

    public void Complete()
    {
        dbContext.SaveChanges();
    }

    public void CompleteWithTransaction()
    {
        using (var dbDcontextTransaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                dbContext.SaveChanges();
                dbDcontextTransaction.Commit();
            }
            catch (Exception ex)
            {
                // logging
                dbDcontextTransaction.Rollback();
            }
        }
    }


    private void Clean(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }

        disposed = true;
        GC.SuppressFinalize(this);
    }
    public void Dispose()
    {
        Clean(true);
    }

}
