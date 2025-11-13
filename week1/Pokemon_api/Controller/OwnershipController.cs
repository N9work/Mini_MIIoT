using Microsoft.AspNetCore.Mvc;
using API_1.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnershipController : ControllerBase
    {
        private readonly AppDBContext _context;

        public OwnershipController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> OwnedStatus()
        {
            var result = await _context.Pokemon
                .Include(x => x.Trainer)
                .Select(x => new
                {       
                    Pid = x.Pid,
                    PName = x.PName,
                    Owner_Name = x.Trainer.TName, //local variable               
                }
                ).ToListAsync();

            return Ok(result);
        }
        [HttpGet("GetOwned")]
        public async Task<IActionResult> FindOwned()
        {
            var result = await _context.Pokemon
                .Where(x => x.OwnerID != null)
                .Include(x => x.Trainer)
                .Select(x => new
                {         
                    Pid = x.Pid,
                    PName = x.PName,
                    Owner_Name = x.Trainer.TName, //local variable                               
                }
                ).ToListAsync();

            return Ok(result);
        }

        [HttpGet("NotOwned")]
        public async Task<IActionResult> FindNotOwned()
        {
            var result = await _context.Pokemon
                .Where(x => x.OwnerID == null)
                .Include(x => x.Trainer)
                .Select(x => new
                {         
                    Pid = x.Pid,
                    PName = x.PName,
                    Owner_Name = x.Trainer.TName, //local variable                               
                }
                ).ToListAsync();

            return Ok(result);
        }

        [HttpGet("TrainerBag")]
        public async Task<IActionResult> TrainerBag()
        {
            var result = await _context.Trainer
                .Include(t => t.Pokemons)
                .Select(t => new
                {
                    Tid = t.Tid,
                    TName = t.TName,
                    PokemonCount = t.Pokemons.Count(),
                    Pokemon = t.Pokemons.Select(p => new
                    {
                        PName = p.PName
                    }).ToList()
                }
                ).ToListAsync();       

            return Ok(result);
        }

        [HttpPut("SetOwner")]
        public async Task<IActionResult> SetOwner([FromBody] Pokemon ownership)
        {
            var exist = await _context.Pokemon.Where(x => x.Pid == ownership.Pid)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.OwnerID, ownership.OwnerID)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                );
            return Ok(exist);
        }
    }
}
