using System.Collections;
using System.ComponentModel.DataAnnotations;
using dotnetapi;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAlumnoService, AlumnoService>();

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
        context.Response.ContentType = "application/problem+json";
        
        await context.Response.WriteAsJsonAsync(problemDetails);
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

public record Student(long id, [MinLength(3)] string name, string surname);

public class AlumnoService : IAlumnoService

{
    public Student create(Student student)
    {
        return student;
    }
}