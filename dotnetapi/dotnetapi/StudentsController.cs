using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapi;

[ApiController]
[Route("api/students")]
public class StudentsController(IAlumnoService alumnoService) : ControllerBase
{
    private List<Student> _students = [new(0, "Javier", "Gamarra"), new(1, "Jorge", "Sanz")];

    [HttpGet]
    public ActionResult<IEnumerable<Student>> GetAll()
    {
        var requestHeader = Request.Headers["Authorization"];

        throw new Exception();
        _students = [new Student(0, "Javier", "Gamarra"), new Student(1, "Jorge", "Sanz")];
        return _students;
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {
        var student = _students.FirstOrDefault(p => p.id == id);
        if (student == null)
            return NotFound();

        return student;
    }
    
    
    [HttpPost]
    public ActionResult<Student> Create(Student student)
    {
        alumnoService.create(student);
        return student;
    }
}

public interface IAlumnoService
{
    Student create(Student student);
}