using AutoMapper;
using SimApi.Base;
using SimApi.Data.Uow;

namespace SimApi.Operation;

public class BaseService<TEntity, TRequest, TResponse> : IBaseService<TEntity, TRequest, TResponse> where TEntity : BaseModel
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public virtual ApiResponse Delete(int Id)
    {
        try
        {
            var entity = unitOfWork.Repository<TEntity>().Where(x => x.Id == Id);
            if (entity is null)
            {
                return new ApiResponse("Record not found");
            }

            unitOfWork.Repository<TEntity>().DeleteById(Id);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public virtual ApiResponse<IEnumerable<TResponse>> GetAll()
    {
        try
        {
            var entityList = unitOfWork.Repository<TEntity>().GetAll();
            var mapped = mapper.Map<IEnumerable<TEntity>, IEnumerable<TResponse>>(entityList);
            return new ApiResponse<IEnumerable<TResponse>>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TResponse>>(ex.Message);
        }
    }

    public virtual ApiResponse<TResponse> GetById(int id)
    {
        try
        {
            var entity = unitOfWork.Repository<TEntity>().GetById(id);
            if (entity is null)
            {
                return new ApiResponse<TResponse>("Record not found");
            }

            var mapped = mapper.Map<TEntity, TResponse>(entity);
            return new ApiResponse<TResponse>(mapped);
        }
        catch (Exception ex)
        {
            return new ApiResponse<TResponse>(ex.Message);
        }
    }

    public virtual ApiResponse Insert(TRequest request)
    {
        try
        {
            var entity = mapper.Map<TRequest, TEntity>(request);
            unitOfWork.Repository<TEntity>().Insert(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }

    public virtual ApiResponse Update(int Id, TRequest request)
    {
        try
        {
            var entity = mapper.Map<TRequest, TEntity>(request);

            var exist = unitOfWork.Repository<TEntity>().Where(x => x.Id == Id);
            if (exist is null)
            {
                return new ApiResponse("Record not found");
            }

            entity.Id = Id;
            unitOfWork.Repository<TEntity>().Update(entity);
            unitOfWork.Complete();
            return new ApiResponse();
        }
        catch (Exception ex)
        {
            return new ApiResponse(ex.Message);
        }
    }
}
