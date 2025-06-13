using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("solicitud")]
public class SolicitudController(AppDbContext appDbContext) : ControllerBase
{
    [HttpPost]
    public Solicitud Post([FromBody] Solicitud solicitud)
    {
        var entityEntry = appDbContext.Solicitud.Add(solicitud);

        appDbContext.SaveChanges();

        var find = appDbContext.Solicitud.Find(entityEntry.Entity.Id);

        return find;
    }

    [HttpGet("{id}")]
    public Solicitud Get(int id)
    {
        var find = appDbContext.Solicitud
            .Include(solicitud => solicitud.Student).Include(solicitud => solicitud.Parents)
            .First(solicitud => solicitud.Id == id);

        return find;
    }
}