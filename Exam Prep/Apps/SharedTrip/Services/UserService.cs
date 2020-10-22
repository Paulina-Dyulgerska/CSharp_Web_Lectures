using SharedTrip.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedTrip.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = this.ComputeHash(password),
            };
            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var hashPassword = this.ComputeHash(password);
            var user = this.db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashPassword);
            return user?.Id;
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.db.Users.Any(x => x.Username == username);
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db.Users.Any(x => x.Email == email);
        }

        private string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                return hashedInputStringBuilder.ToString();
            }
        }
    }
}
