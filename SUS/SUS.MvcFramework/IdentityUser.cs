using System.ComponentModel.DataAnnotations;

namespace SUS.MvcFramework
{
    public class IdentityUser<T> //pozwolqwam otvyn da mi kazwat kakyv shte e typa na Id-to na Usera.
    {
        public T Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; } //TODO MinLenght 5

        [Required]
        public string Email { get; set; }

        //Has a Password – a string with min length 6 and max length 20  - hashed in the database(required)
        [Required]
        public string Password { get; set; } //TODO MinLenght 6,   [MaxLength(20)] - ne moga da go zadam taka tuk, zashtoto hashiranata 
        //password ot 20 symbols, shte vyrne resultat > ot 20 symbols!

        public IdentityRole Role { get; set; }

    }
}
