using System.ComponentModel.DataAnnotations;

namespace API_1.Models
{
    public class Trainer
    {
        [Key]public int Tid { get; set; }
        public string? TName { get; set; }
        public string? Gym { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
    }
}
