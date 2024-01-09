using DemoMongoDb.API.Mappers;
using DemoMongoDb.API.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DemoMongoDb.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MongoDbController : ControllerBase
{
    private ILogger<MongoDbController> _logger;

    private readonly IMongoDatabase _database;
    private IMongoCollection<Student> _students;

    public MongoDbController(ILogger<MongoDbController> logger)
    {
        _logger = logger;

        _database = new MongoClient("mongodb://localhost").GetDatabase("School");
        _students = _database.GetCollection<Student>("Students");
    }

    [HttpGet(Name = "GetAllStudents")]
    public async Task<ActionResult<List<Student>>> GetAll()
    {
        _students = _database.GetCollection<Student>("Students");

        var students = await _students.Find(student => true).ToListAsync();

        return Ok(students.Select(x => StudentMapper.MapToStudentEntity(x)));
    }

    [HttpGet("{id:length(24)}", Name = "GetStudentById")]
    public async Task<ActionResult<Student>> GetById(string id)
    {
        var student = await _students.Find<Student>(student => student.Id == new ObjectId(id)).FirstOrDefaultAsync();
        
        if (student == null)
        {
            _logger.LogWarning("Student with id {0} was not found!", id);
            return NotFound();
        }

        return Ok(StudentMapper.MapToStudentEntity(student));
    }

    [HttpPost]
    public async Task<ActionResult<Student>> Create(Student student)
    {
        await _students.InsertOneAsync(student);

        return CreatedAtRoute("GetStudentById", new { id = student.Id.ToString() }, StudentMapper.MapToStudentEntity(student));
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Student studentIn)
    {
        var objectId = new ObjectId(id);
        var student = await _students.Find(student => student.Id == objectId).FirstOrDefaultAsync();
        if (student == null)
        {
            _logger.LogWarning("Student with id {0} was not found!", id);
            return NotFound();
        }

        // Set the _id of studentIn to the _id of the original document
        studentIn.Id = student.Id;

        await _students.ReplaceOneAsync(student => student.Id == objectId, studentIn);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var student = await _students.Find(student => student.Id == new ObjectId(id)).FirstOrDefaultAsync();

        if (student == null)
        {
            _logger.LogWarning("Student with id {0} was not found!", id);
            return NotFound();
        }

        await _students.DeleteOneAsync(student => student.Id == new ObjectId(id));

        return NoContent();
    }
}