using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MIMWebClient.Models
{
    using System.Web.Mvc.Routing.Constraints;

    public class CreatePlayer
    {
        [Required(ErrorMessage = "You won't become a legend without a name!")]
        [MinLength(3, ErrorMessage = "What kind of name is that? Try a longer one than 2 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "A name like that doesn't sound very fantasy. Try using only A to Z")]
        [Remote("Isname_Available", "Home")]
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

        [Required(ErrorMessage = "We need your email incase you forget your password.")]
        [EmailAddress(ErrorMessage = "Email address needs to be valid, enter abc@email.com if you don't want to leave one. ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "We need your password to protect your character.")]
        [MinLength(6, ErrorMessage = "Password needs to be atleast 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "You can copy & paste your password if you like, but don't blame me if it's wrong.")]
        [MinLength(6, ErrorMessage = "Password needs to be atleast 6 characters long.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Your passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public void savePlayer()
        {

        }
    }
}