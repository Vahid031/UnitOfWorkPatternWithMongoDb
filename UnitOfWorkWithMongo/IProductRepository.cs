using MongoDB.Driver;

namespace UnitOfWorkWithMongo;

public interface IProductRepository : IRepository<Product, ProductId>
{

}

public sealed class ProductRepository : RepositoryBase<Product, ProductId>, IProductRepository
{
    public ProductRepository(IUnitOfWork unitOfWork, IMongoCollection<Product> collection)
        : base(unitOfWork, collection)
    {
    }
}