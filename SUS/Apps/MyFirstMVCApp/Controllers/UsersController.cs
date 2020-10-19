using MyFirstMVCApp.Services;
using MyFirstMVCApp.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyFirstMVCApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        //GET /Users/Login ////ne e nujno da utochnqwam pytq! zashtoto toj se vzima avtomatichno
        public HttpResponse Login()
        {
            //this is done by the base class Controller:
            //var responseHtml = File.ReadAllText("Views/Users/Login.html");
            //var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            //return new HttpResponse("text/html", responseBodyBytes);
            //return this.View("Views/Users/Login");

            //ako imam attributa [Authorize], kojto go ima v ASP.NET, nqma da pisha tezi raboti.....
            //ima i [Notauthorised] attribute syshto.
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        //<form class="mx-auto w-50" method="post"> //tova v view-to znachi, che pri submit formata shte prati post request
        //Ako nqmam upomenat nakyde da q prati, zaqwkata se izprashta na syshtiqt url, na kojto v zaredena formata: 
        //<form class="mx-auto w-50" method="post"> v sluchaq prashta POST request na http://localhost:12345/Users/Login
        //NO, ako imam slojen attribute action, to formata shte izprati POST zaqwka kym posocheniqt v action url!!!!
        //<form class="mx-auto w-50" method="post" action="/users/check"> shte prati PORT zaqwka na http://localhost:12345/Users/Check
        //[HttpPost("/Users/Login")] //posochvam pytq tuk, zashtoto Login e razlichno ot DoLogin!!!
        //public HttpResponse DoLogin(string username, string password)
        //kogato imam razlichni signaturi na methodite Login() i Login(string username, string password) moga da gi polzwam i 
        //dwata, kato ediniq e pri GET, a drugiq e pri POST, no veche sa razlichni methodi i moga da polzwa edno ime za tqh,
        //a kogato polzwam syshtoto ime, kakvoto mi e na addressa na url-to, to ne e nujno da pisha path v attributa,
        //zatowa veche shte mahna path ot attributa i shte prekrystq methoda!!!
        //POST /Users/Login
        [HttpPost] // ne posochvam pytq tuk, zashtoto Login e syshtoto kato Login!!!
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            //TODO: read data
            //TODO: check user
            //TODO: log user

            var userId = this.usersService.GetUserId(username, password);

            if (userId == null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);

            return this.Redirect("/Cards/All");
        }

        //GET /Users/Register //ne e nujno da utochnqwam pytq! zashtoto toj se vzima avtomatichno
        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        //[HttpPost("/Users/Register")] //posochvam pytq tuk, zashtoto Register e razlichno ot DoRegister!!!
        //moqt method se kazwashe v nachaloto DoRegister, zashoto nqmashe parametri i nqmashe kak da go krystq kato 
        //gorniq Register pri GET, kojto se vika. No kato vzeh parametrite ot formata, veche imam argumenti na methoda i
        //moga da go prekrystq na Register!!!
        //kogato veche imam razlichni argumenti na methoda Register pri Get i razlichni pri Post, moga da mahna 
        //tozi path ot attributa i da ostavq samo vida na zaqwkata, zashoto te shte sa dva razlichni method i az
        //shte gi krystq s edno i syshto ime, no shte imat razlichni parametri!!!
        //public HttpResponse DoRegister(string username, string email, string password, string confirmPassword)
        //POST /Users/Register
        [HttpPost] //ne posochvam pytq tuk, zashtoto Register e syshtoto kato Register!!!
        public HttpResponse Register(RegisterInputModel input)
        //podawam v constructora tova, ot koeto shte imam nujda, iskam da mi se dawat tezi parameter otvyn!!! A ne 
        //az da gi izmykvam ot this.request.FromData[parameterName];
        //vsichko, koeto mi trqbwa kato parameter na methoda, shte se tyrsi v samata zaqwka, v samiq request!!!!
        //towa se naricha DataBinding!!! Realizirano e samoto vzimane na argumentite ot request.FormData v Host.cs!!!!
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            //zaradi DataBindinga veche nqma da e nujno az sama da si gi izvlicham tezi danni:
            //var username = this.Request.FormData["username"];
            //var email = this.Request.FormData["email"];
            //var password = this.Request.FormData["password"];
            //var confirmPassword = this.Request.FormData["confirmPassword"];

            //validations: tova e server-side validation!
            //rabota na UserController e da pravi tezi validations, ne e rabota na UserService tova!
            if (input.Username == null || input.Username.Length < 5 || input.Username.Length > 20)
            {
                return this.Error("Invalid username. Username should be between 5 and 20 characters.");
            }

            if (!Regex.IsMatch(input.Username, @"^[a-zA-Z0-9\.]+$"))
            {
                return this.Error("Invalid username. Only alphanumeric characters and dot are allowed.");
            }

            //proverqwam blagodarenie na dyrjavniq attribute, dali emaila pone prilicha na validen email address:
            if (string.IsNullOrWhiteSpace(input.Email) || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Invalid email.");
            }

            if (input.Password == null || input.Password.Length < 6 || input.Password.Length > 20)
            {
                return this.Error("Invalid password. Password must be between 6 and 20 characters.");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords should be the same.");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("Username already taken.");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                this.Error("This email is already registered.");
            }

            var userId = this.usersService.CreateUser(input.Username, input.Password, input.Email);
            //this.SignIn(userId);//po uslovie mi iskat da pratq potrebitelq kym Login stranicata, t.e. ne trqbwa da mi e
            //lognat tuk.....

            return this.Redirect("/Users/Login");
        }

        //GET /Users/Logout
        public HttpResponse Logout() //ne e nujno da utochnqwam pytq! zashtoto toj se vzima avtomatichno
        {
            if (!this.IsUserSignedIn())
            {
                return this.Error("Only logged-in users can logout.");
            }

            this.SignOut();
            return this.Redirect("/");
        }
    }
}
