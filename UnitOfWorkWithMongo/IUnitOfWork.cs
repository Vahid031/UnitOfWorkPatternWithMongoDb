using MongoDB.Driver;

namespace UnitOfWorkWithMongo;

public interface IUnitOfWork
{
    IDisposable Session { get; }

    void AddOperation(Action operation);

    void CleanOperations();

    Task CommitChanges();
}

public sealed class UnitOfWork : IUnitOfWork
{
    private IClientSessionHandle session { get; }
    public IDisposable Session => session;

    private List<Action> _operations { get; set; }

    public UnitOfWork(IMongoClient mongoClient)
    {
        session = mongoClient.StartSession();

        _operations = new List<Action>();
    }

    public void AddOperation(Action operation)
    {
        _operations.Add(operation);
    }

    public void CleanOperations()
    {
        _operations.Clear();
    }

    public async Task CommitChanges()
    {
        session.StartTransaction();

        _operations.ForEach(o =>
        {
            o.Invoke();
        });

        await session.CommitTransactionAsync();

        CleanOperations();
    }
}