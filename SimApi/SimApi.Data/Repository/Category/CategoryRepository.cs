using SimApi.Data.Context;
using SimApi.Data;
using Microsoft.EntityFrameworkCore;

namespace SimApi.Data.Repository;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(SimDbContext context) : base(context)
    {
    }
}
