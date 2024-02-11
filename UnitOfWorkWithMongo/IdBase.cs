namespace UnitOfWorkWithMongo;

public interface IIdBase
{

}

public abstract class IdBase<TKey> : ValueObject<IdBase<TKey>>, IIdBase
{
    public TKey Value { get; private protected set; }

    public override int ObjectGetHashCode() => Value.GetHashCode();

    public override bool ObjectIsEqual(IdBase<TKey> other) => Value.Equals(other);
}
