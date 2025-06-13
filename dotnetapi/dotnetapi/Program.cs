using System.Collections;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using dotnetapi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers().AddControllersAsServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=password"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



// Configure exception handling
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature.Error;
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred!",
            Detail = exception.Message,
            Instance = context.Request.Path
        };
        
        // Log the exception
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Unhandled exception");
        
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        
        await context.Response.WriteAsJsonAsync(problemDetails, options: null,contentType: "application/problem+json");
    });
});



app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/students", () =>
    {
        var students = new ArrayList { new Student(0, "Javier", "Gamarra"),  new Student(1, "Jorge", "Sanz") };
        return students;
    })
    .WithName("GetStudents");

app.Run();

public record Student(long Id, [MinLength(3)] string Name, string Surname);

public class Solicitud {
    public int Id { get; set; }
    public required Student Student { get; set; }
    public required List<Parents> Parents { get; set; }
}

public record Parents(long Id, [MinLength(3)] string Name, string Surname)
{
}


public class AlumnoService(IMapper imapper) : IAlumnoService

{
    public StudentDTO create(Student student)
    {
        var studentDto = imapper.Map<StudentDTO>(student);
        return studentDto;
    }
}

public class StudentDTO
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDTO>();
    }
}

public class AppDbContext : DbContext {
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<Student> Students { get; set; }
    
    public DbSet<Parents> Parents { get; set; }
    
    public DbSet<Solicitud> Solicitud { get; set; }
}
