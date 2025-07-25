using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Data;
using RestAPI.Models;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {

        private readonly AppDbContext _context;

        public SensorDataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<SensorData>> CreateData(SensorData data)
        {
            _context.SensorData.Add(data);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetData), new { id = data.Id }, data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SensorData>> GetData(int id)
        {
            var item = await _context.SensorData.FindAsync(id);
            if (item == null)
                return NotFound();

            return item;
        }
    }
}
