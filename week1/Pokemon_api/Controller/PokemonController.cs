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

        [HttpGet("GetPokemon")]
        public async Task<IActionResult> GetPokemon()
        {
            var result = await _context.Pokemon.Select(x => new
            {
                id = x.id,
                Pokedex = x.Pokedex,
                Name = x.Name,
                Type = x.Type,
                Region = x.Region,
                CreatedAt = x.CreatedAt,
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpGet("Pokemon")]
        public async Task<IActionResult> Pokemon(
           [FromQuery] string pokedex,
           [FromQuery] string type)
        {
            var query = _context.Pokemon.AsQueryable();
            if (!string.IsNullOrEmpty(pokedex))
            {
                query = query.Where(p => p.Pokedex.Contains(pokedex));
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(p => p.Type.Contains(type));
            }
            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpPost("CreatePokemon")]
        public async Task<IActionResult> CreatePokemon([FromBody] Pokemon pokemon)
        {
            _context.Pokemon.Add(pokemon);
            await _context.SaveChangesAsync();

            return Ok(pokemon);
        }

        [HttpPut("EditPokemon")]
        public async Task<IActionResult> EditPokemon([FromBody] Pokemon pokemon)
        {
            var exist = await _context.Pokemon.Where(x => x.id==pokemon.id)
                .ExecuteUpdateAsync(x => x
                .SetProperty(x => x.Name, pokemon.Name)
                .SetProperty(x => x.Type, pokemon.Type)
                .SetProperty(x => x.Pokedex, pokemon.Pokedex)
                .SetProperty(x => x.Region, pokemon.Region)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)

                );

            return Ok(exist);
        }

        [HttpDelete("DeletePokemon")]
        public async Task<IActionResult> DeletePokemon(int pokemonId)
        {
            var rows = await _context.Pokemon.Where(x => x.id == pokemonId)
                .ExecuteDeleteAsync();

            return Ok(true);
        }
    }
}
