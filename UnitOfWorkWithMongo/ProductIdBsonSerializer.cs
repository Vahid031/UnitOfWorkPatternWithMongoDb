using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace UnitOfWorkWithMongo;

internal class ProductIdBsonSerializer : IBsonSerializer<ProductId>
{
    public Type ValueType => typeof(ProductId);

    public ProductId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => new ProductId(context.Reader.ReadInt64());
    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProductId value)
        => context.Writer.WriteInt64(value.Value);

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        => Serialize(context, args, value is ProductId id ? id : null);

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        => Deserialize(context, args);
}