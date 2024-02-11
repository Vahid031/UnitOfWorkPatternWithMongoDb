

using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using UnitOfWorkWithMongo;

var services = new ServiceCollection();

services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    return mongoClient.GetDatabase("UnitOfWorkWithMongoDb");
});
services.AddSingleton<IMongoClient>(sp =>
{
    var mongoClient = new MongoClient("mongodb://vahid:admin1234@localhost:27017/UnitOfWorkWithMongoDb?retryWrites=false");
    return mongoClient;
});
services.AddSingleton<IMongoCollection<Product>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<Product>("Products");
});
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<IProductRepository, ProductRepository>();

BsonSerializer.RegisterSerializer(new ProductIdBsonSerializer());

BsonClassMap.RegisterClassMap<EntityBase<ProductId>>(cm =>
{
    cm.MapMember(product => product.Id).SetElementName("_id");
});

BsonClassMap.RegisterClassMap<Product>(cm =>
{
    cm.MapMember(product => product.Code).SetElementName("code");
    cm.MapMember(product => product.Name).SetElementName("name");
    cm.MapMember(product => product.Sku).SetElementName("sku");
    cm.MapMember(product => product.Price).SetElementName("price");
});


using var serviceProvider = services.BuildServiceProvider();
var productRepo = serviceProvider.GetRequiredService<IProductRepository>();

await productRepo.Add(new Product
{
    Id = new ProductId(2),
    Code = "00012",
    Name = "Product2",
    Price = 125300,
    Sku = "145236"
});



await productRepo.Commit();
