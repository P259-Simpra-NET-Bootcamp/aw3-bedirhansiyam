using Microsoft.EntityFrameworkCore;
using SimApi.Data.Context;

namespace SimApi.Data.Repository;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(SimDbContext context) : base(context)
    {
    }
}
