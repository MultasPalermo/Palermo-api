//using Business.Interfaces.IBusinessImplements; // Para ITouristicAttractionService
//using Entity.Domain.Models;
//using Entity.DTOs.Select;
//using Entity.Infrastructure.Contexts;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//[ApiController]
//[Route("api/[controller]")]
//public class TouristicAttractionsController : ControllerBase
//{
//    private readonly ApplicationDbContext _context;

//    public TouristicAttractionsController(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] TouristicAttractionApiDto dto)
//    {
//        if (string.IsNullOrWhiteSpace(dto.name) || string.IsNullOrWhiteSpace(dto.description))
//        {
//            return BadRequest("Name and Description are required.");
//        }

//        var attraction = new TouristicAttraction
//        {
//            name = dto.name,
//            description = dto.description
//        };

//        _context.TouristicAttraction.Add(attraction);//        await _context.SaveChangesAsync();

//        return Ok(attraction);

//        // Aquí puedes agregar PUT, DELETE, etc...
//    }
//}