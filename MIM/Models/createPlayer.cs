using System.ComponentModel.DataAnnotations;

namespace MIM.Models
{
    public class createPlayer
    {
        [Required]
        public string Name { get; set; }

        public void savePlayer()
        {

        }
    }
}