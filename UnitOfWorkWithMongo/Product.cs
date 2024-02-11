namespace UnitOfWorkWithMongo;

public class Product : EntityBase<ProductId>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Sku { get; set; }
    public long Price { get; set; }
}
