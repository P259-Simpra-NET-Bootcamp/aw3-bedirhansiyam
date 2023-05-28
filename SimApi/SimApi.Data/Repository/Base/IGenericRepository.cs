using SimApi.Base;
using System.Linq.Expressions;

namespace SimApi.Data.Repository;

public interface IGenericRepository<Entity> where Entity : BaseModel
{
    Entity GetById(int id, params Expression<Func<Entity, object>>[] includes);
    void Insert(Entity entity);
    void Update(Entity entity);
    void DeleteById(int id);
    void Delete(Entity entity);
    IEnumerable<Entity> GetAll(params Expression<Func<Entity, object>>[] includes);
    IEnumerable<Entity> Where(Expression<Func<Entity, bool>> expression);

    void Complete();
    void CompleteWithTransaction();
}
