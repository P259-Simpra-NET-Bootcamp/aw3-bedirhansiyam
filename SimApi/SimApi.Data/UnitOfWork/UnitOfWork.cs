﻿using SimApi.Data.Context;
using SimApi.Data;
using SimApi.Data.Repository;
using SimApi.Base;

namespace SimApi.Data.Uow;

public class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<Category> CategoryRepository { get; private set; }
    public IGenericRepository<Product> ProductRepository { get; private set; }


    private readonly SimDbContext dbContext;
    private bool disposed;

    public UnitOfWork(SimDbContext dbContext)
    {
        this.dbContext = dbContext;

        CategoryRepository = new GenericRepository<Category>(dbContext);
        ProductRepository = new GenericRepository<Product>(dbContext);
    }

    public IGenericRepository<Entity> Repository<Entity>() where Entity : BaseModel
    {
        return new GenericRepository<Entity>(dbContext);
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
