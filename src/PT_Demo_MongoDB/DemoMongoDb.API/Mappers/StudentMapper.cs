using DemoMongoDb.API.Models;

namespace DemoMongoDb.API.Mappers;

public static class StudentMapper
{
    public static StudentEntity MapToStudentEntity(Student student)
    {
        if (student == null) return null;

        return new StudentEntity
        {
            Id = student.Id,
            StudentId = student.Id.ToString(),
            FirstName = student.FirstName,
            LastName = student.LastName,
            Age = student.Age,
            City = student.City
        };
    }
}
