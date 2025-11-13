using API_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly AppDBContext _context;
        public TrainerController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTrainer()
        {
            var trainer = await _context.Trainer.Select(x => new 
            {
                Tid = x.Tid,
                TName = x.TName,
                Gym = x.Gym,
            })
            .ToListAsync();

            return Ok(trainer);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchByID(int tId)
        {
            var search = await _context.Trainer
                .Where(x => x.Tid == tId)
                .Select(x => new
                {
                    Tid = x.Tid,
                    TName = x.TName,
                    Gym = x.Gym,
                })
                .ToListAsync();

            return Ok(search);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTrainer([FromBody] Pokemon trainer)
        {
            _context.Pokemon.Add(trainer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Error while saving changes",
                    detail = ex.InnerException?.Message ?? ex.Message
                });
            }

            return Ok(trainer);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditTrainer([FromBody] Trainer trainer)
        {
            var exist = await _context.Trainer.Where(x => x.Tid == trainer.Tid)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.TName, trainer.TName)
                .SetProperty(x => x.Gym, trainer.Gym)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );

            return Ok(exist);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteTrainer(int tId)
        {
            var rows = await _context.Trainer.Where(x => x.Tid == tId)
                .ExecuteDeleteAsync();

            return Ok(true);
        }
    }
}
