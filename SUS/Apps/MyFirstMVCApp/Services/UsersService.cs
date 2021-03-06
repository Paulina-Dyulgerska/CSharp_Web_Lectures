﻿using Microsoft.EntityFrameworkCore.Internal;
using MyFirstMVCApp.Data;
using SUS.MvcFramework;
using System.Linq;
using System.Text;

namespace MyFirstMVCApp.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDBContext db; //moje da readonly zashtoto ne se promenq nikyde, osven v constructora.

        //v constructora na vseki edin service deklariram kakvi shte sa negovite nujdi.
        public UsersService(ApplicationDBContext db)
        {
            //this.db = new ApplicationDBContext(); //userServica nqma pravo da pravi new object, toj trqbwa da poluchawa vsichko
            //otvyn, da ima dependencies, a ne da izbira kakvo da si syzdade sam vytre v classa!!!!
            //otvyn osven ApplicationDBContext moga da poluchavam i naslednici na ApplicationDBContext bez nikakyv problem!!!
            this.db = db;
        }

        public string CreateUser(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                Password = this.ComputeHash(password),
                Email = email.ToLower(),
                Role = IdentityRole.User,
            };

            this.db.Users.Add(user);

            this.db.SaveChanges();

            return user.Id;
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db.Users.Any(x => x.Email == email.ToLower());
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.db.Users.Any(x => x.Email == username.ToLower());
        }

        public string GetUserId(string username, string password)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == username && this.ComputeHash(password) == x.Password);

            return user?.Id;
        }

        //tozi method moje da static, zashtoto toj ne polzwa danni na classa!!!!!!
        //Edin method moje da go ostavqm da e static, kogato toj NE POLZWA danni na classa!!!! 
        //Tozi moje da e static ako iskam, zashoto ne zavisi
        //rabotata mu ot nishto, koeto e svyrzano s classa(s obekta ot type tozi class)!!!
        private string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits = 64 B [in byte] * 2 symbols for byte 
                //zashtoto vseki edin byte se predstavq s 2 symbols v stringa!!!! zatowa 64 bytes * 2 = 128 bytes mi trqbwat
                //za da pazq stringa!!!
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                //ToString("X2") prints the byte in Hexadecimal format.
                //No format string: 13
                //'X2' format string: 0D
                // 1 byte = 256 characters
                //(i.e. 0 - 255).Eight bits are called a byte.One byte character sets can represent 256 unique characters.
                //The current standard, though, is Unicode. Unicode uses two bytes to represent all characters in all writing 
                //systems in the world in a single set.
                //SHA-512 generates a 512-bit hash value. You can use CHAR(128) or BINARY(64) in the DB shema.
                //1 byte se predstawq ot  8 bits = 2 * 4 bits, a 4 bits = edno hexadecimal chislo! Zatowa za da predstawq kato string
                //1 byte, trqbwa da imam 2 hexadecila digits, koito da zapisha v 2 symbols!!! Zatowa za SHA-512 mi 
                //trqbwa string s dyljina 64Bytes * 2Symbols/Byte = 128 Symbols!!!! Tezi 128 symbol shte sa fixnata dyljina 
                //za wsqka edna criptirana parola, nezawisimo ot dyljinata na originalnata necriptirana, dadena ot usera, password!
                //Stringa produjiran e textovata reprezentaziq na cryptiranite bits, polucheni ot SHA-512 algorithm-a!!!!

                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
