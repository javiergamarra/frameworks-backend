using System.Collections;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.MapGet("/students", () =>
    {
        return new ArrayList { new Student("Javier", "Gamarra"),  new Student("Jorge", "Sanz") };;
    })
    .WithName("GetStudents");

app.Run();

public record Student(string name, string surname);