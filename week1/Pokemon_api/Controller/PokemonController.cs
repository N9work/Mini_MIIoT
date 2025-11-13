using API_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly AppDBContext _context;

        public PokemonController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPokemon()
        {
            var result = await _context.Pokemon            
                .Select(x => new 
            {
                Pid = x.Pid,
                Pokedex = x.Pokedex,
                PName = x.PName,
                Type = x.Type,
                Region = x.Region,
                CreatedAt = x.CreatedAt,
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchByID(int pId)
        {
            var search = await _context.Pokemon
                .Where(x => x.Pid == pId)
                .ToListAsync();

            return Ok(search);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePokemon([FromBody] Pokemon pokemon)
        {
            _context.Pokemon.Add(pokemon);

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

            return Ok(pokemon);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditPokemon([FromBody] Pokemon pokemon)
        {
            var exist = await _context.Pokemon.Where(x => x.Pid==pokemon.Pid)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.PName, pokemon.PName)
                .SetProperty(x => x.Type, pokemon.Type)
                .SetProperty(x => x.Pokedex, pokemon.Pokedex)
                .SetProperty(x => x.Region, pokemon.Region)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)

                );

            return Ok(exist);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeletePokemon(int pokemonId)
        {
            var rows = await _context.Pokemon.Where(x => x.Pid == pokemonId)
                .ExecuteDeleteAsync();

            return Ok(true);
        }
    }
} 
