using System.ComponentModel.DataAnnotations;

namespace MyFirstMVCApp.Data
{
    public class UserCard
    {
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int CardId { get; set; }
        public virtual Card Card { get; set; }

        //public int Id { get; internal set; } //tova da go vklyuchvam samo ako ne si pravq compositen key, zashtoto se obyrkwat 
        //inache i EF mi gyrmi!!!!!!!! Sega imam composite Key ot dvete id-ta i zatowa e typo da pravq novo ID, no generalno
        //naj-dobroto neshto e da imam samo Id, koeto da e primary key na tazi tablica, a userId i cardId da sa prosti coloni
        //v neq, koito sa foreign keys kym drugite dve tablcici samo!!!!
    }
}
