using SUS.MvcFramework;
using System;
using System.Collections.Generic;

namespace MyFirstMVCApp.Data
{
    public class User : IdentityUser<string> //reshila sym Id-to da mi e string i nego podavam, ako iskam drug type
        //kojto ne e string, a int naprimer, nqma dert da go podam i nego tuk.
    { 
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Role = IdentityRole.User; //by default usera shte se vodi s rolq User, a ako iskam shte mu q smenq.
            //s dobavqne na tovo property Role sega, az na praktika dobawqm nowa kolona v DB-a i trqbwa da naprawq Migration!!!!
            this.Cards = new HashSet<UserCard>();
        }

        public virtual ICollection<UserCard> Cards { get; set; }
    }
}
