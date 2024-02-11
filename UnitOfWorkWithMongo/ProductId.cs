namespace UnitOfWorkWithMongo;

public class ProductId : IdBase<long>
{
	public ProductId(long value)
	{
		Value = value;
	}
}
