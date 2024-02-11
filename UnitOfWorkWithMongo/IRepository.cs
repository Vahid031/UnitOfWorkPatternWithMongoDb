using MongoDB.Driver;

namespace UnitOfWorkWithMongo;

public interface IRepository<TEntity, TId>
    where TId : IIdBase
    where TEntity : EntityBase<TId>
{
    Task Add(TEntity entity);
    Task Update(TEntity entity);
    Task Remove(TId id);
    Task Commit();
}

public abstract class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : IIdBase
{
    private readonly IMongoCollection<TEntity> _collection;
    private readonly IUnitOfWork _unitOfWork;

    protected RepositoryBase(IUnitOfWork unitOfWork, IMongoCollection<TEntity> collection)
    {
        _unitOfWork = unitOfWork;
        _collection = collection;
    }

    public Task Add(TEntity entity)
    {
        Action operation = () => _collection.InsertOne(_unitOfWork.Session as IClientSessionHandle, entity);
        _unitOfWork.AddOperation(operation);

        return Task.CompletedTask;
    }

    public Task Update(TEntity entity)
    {
        Action operation = () => _collection.ReplaceOne(_unitOfWork.Session as IClientSessionHandle, x => x.Id.Equals(entity.Id), entity);
        _unitOfWork.AddOperation(operation);

        return Task.CompletedTask;
    }

    public Task Remove(TId id)
    {
        Action operation = () => _collection.DeleteOne(_unitOfWork.Session as IClientSessionHandle, x => x.Id.Equals(id));
        _unitOfWork.AddOperation(operation);

        return Task.CompletedTask;
    }

    public async Task Commit()
    {
        await _unitOfWork.CommitChanges();
    }
}