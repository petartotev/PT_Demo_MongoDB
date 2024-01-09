using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DemoMongoDb.API.Models;

public class StudentEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}
