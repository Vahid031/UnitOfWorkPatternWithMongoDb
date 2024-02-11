namespace UnitOfWorkWithMongo;

public abstract class EntityBase<TId> where TId : IIdBase
{
    public TId Id { get; set; }
}
