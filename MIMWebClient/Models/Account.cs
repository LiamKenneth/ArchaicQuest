using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMWebClient.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class Account
    {

        [Required(ErrorMessage = "You won't become a legend without a name!")]
        [MinLength(3, ErrorMessage = "What kind of name is that? Try a longer one than 2 characters.")]
        [RegularExpression(@"^[a-zA-Z']+$", ErrorMessage = "A name like that doesn't sound very fantasy. Try using only A to Z")]
       
        public string Name { get; set; }

        [Required(ErrorMessage = "We need your password to allow to enter the realm.")]
        [MinLength(6, ErrorMessage = "Password needs to be atleast 6 characters long.")]
        [Remote("ValidateUser", "Home", AdditionalFields = "Name")]
        public string Password { get; set; }
    }
}
