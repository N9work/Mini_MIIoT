using System.ComponentModel.DataAnnotations;

namespace API_1.Models
{
    public class Pokemon
    {
        [Key]public int Pid { get; set; }
        public string? PName { get; set; }
        public string? Type { get; set; }
        public string? Pokedex { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public  DateTime? UpdatedAt { get; set; }
        public  string? Region { get; set; }

        public int? OwnerID { get;set; }

        public Trainer? Trainer { get; set; } // Navigate for FK

    }
}
