using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;
    public class TreeItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }
    public string? name { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? parentId { get; set; }
    public List<TreeItem>? children { get; set; }
    public int level { get; set; }
}

