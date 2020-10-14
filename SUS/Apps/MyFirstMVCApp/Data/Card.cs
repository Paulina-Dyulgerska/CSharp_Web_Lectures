using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFirstMVCApp.Data
{
    public class Card
    {
        public Card()
        {
            this.Users = new HashSet<UserCard>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Name { get; set; } //TODO MinLength 5

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Keyword { get; set; }

        public int Attack { get; set; } //TODO cannot be negative

        public int Health { get; set; } //TODO cannot be negative

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public virtual ICollection<UserCard> Users { get; set; }
    }
}
