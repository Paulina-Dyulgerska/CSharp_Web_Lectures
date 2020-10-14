using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstMVCApp.Data
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Cards = new HashSet<UserCard>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; } //TODO MinLenght 5

        [Required]
        public string Email { get; set; }

        //Has a Password – a string with min length 6 and max length 20  - hashed in the database(required)
        [Required]
        [MaxLength(20)]
        public string Password { get; set; } //TODO MinLenght 6

        public virtual ICollection<UserCard> Cards { get; set; }
    }
}
