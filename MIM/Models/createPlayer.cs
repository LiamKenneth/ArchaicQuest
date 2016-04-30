using System.ComponentModel.DataAnnotations;

namespace MIM.Models
{
    public class CreatePlayer
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Race { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        public string Strength { get; set; }

        [Required]
        public string Dexterity { get; set; }

        [Required]
        public string Constitution { get; set; }

        [Required]
        public string Intelligence { get; set; }

        [Required]
        public string Wisdom { get; set; }

        [Required]
        public string Charisma { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public void savePlayer()
        {

        }
    }
}