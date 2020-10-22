using SharedTrip.Services;
using SharedTrip.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel input)
        {
            var userId = this.userService.GetUserId(input.Username, input.Password);

            if (userId == null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel input)
        {
            //username validation:
            if (string.IsNullOrWhiteSpace(input.Username) || input.Username.Length < 5 || input.Username.Length > 20)
            {
                return this.Error("Username should be between 5 and 20 characters long.");
            }

            if (!this.userService.IsUsernameAvailable(input.Username))
            {
                return this.Error("Username is already taken.");
            }

            if (!Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\._-]+$"))
            {
                return this.Error("Invalid username. Only alphanumeric characters, dashes and dots are allowed.");
            }

            //password validation:
            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 6 || input.Password.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters long.");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords do not match.");
            }

            //password validation:
            if (string.IsNullOrWhiteSpace(input.Email) || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Invalid email address.");
            }

            if (!this.userService.IsEmailAvailable(input.Email))
            {
                return this.Error("Email is already taken.");
            }

            this.userService.Create(input.Username, input.Email, input.Password);

            return this.Redirect("/Users/Login");
        }

    }
}
