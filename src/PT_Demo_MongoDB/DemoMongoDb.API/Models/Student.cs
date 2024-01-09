using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DemoMongoDb.API.Models;

public class Student
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}
