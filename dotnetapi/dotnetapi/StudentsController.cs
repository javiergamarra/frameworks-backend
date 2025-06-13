using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace dotnetapi;

[ApiController]
[Route("api/students")]
public class StudentsController(IAlumnoService alumnoService, HttpClient httpClient, AppDbContext appDbContext) : ControllerBase
{
    private List<Student> _students = [new(0, "Javier", "Gamarra"), new(1, "Jorge", "Sanz")];

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetAll()
    {
        return appDbContext.Students.ToList();
        


        var async = await httpClient.GetAsync("https://cdn.contentful.com/spaces/7bqz4c5fa32k/environments/master/entries?access_token=zERw6wiDlFsQKZy4zfj5MMnzHQ2luwlrk3FMC1Psf1g");

        var readFromJsonAsync = await async.Content.ReadFromJsonAsync<Object>();

        var requestHeader = Request.Headers["Authorization"];
        
        _students = [new Student(0, "Javier", "Gamarra"), new Student(1, "Jorge", "Sanz")];
        return _students;
    }

    [HttpGet("{id}")]
    public ActionResult<Student> GetById(int id)
    {

        return appDbContext.Students.First(p => p.Id == id);

        
        var student = _students.FirstOrDefault(p => p.Id == id);
        if (student == null)
            return NotFound();

        return student;
    }
    
    
    [HttpPost]
    public ActionResult<StudentDTO> Create(Student student)
    {
        
        appDbContext.Students.Add(student);
        appDbContext.SaveChanges();
        
        var studentDto = alumnoService.create(student);
        return Ok(studentDto);
    }
}

public interface IAlumnoService
{
    StudentDTO create(Student student);
}