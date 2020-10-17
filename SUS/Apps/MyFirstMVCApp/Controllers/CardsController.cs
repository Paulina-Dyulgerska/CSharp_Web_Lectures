using MyFirstMVCApp.Data;
using MyFirstMVCApp.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Linq;

namespace MyFirstMVCApp.Controllers
{
    public class CardsController : Controller
    {
        //GET /Cards/Add
        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {
            //Niki kaza da pravq tezi validacii dori i tuk v POST methodite, a ne samo v GET!!!!
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var dbContext = new ApplicationDBContext();

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }

            var card = new Card
            {
                Name = this.Request.FormData["name"],
                ImageUrl = this.Request.FormData["image"],
                Keyword = this.Request.FormData["keyword"],
                Attack = int.Parse(this.Request.FormData["attack"]),
                Health = int.Parse(this.Request.FormData["health"]),
                Description = this.Request.FormData["description"],
            };

            dbContext.Cards.Add(card);

            dbContext.SaveChanges();

            return this.Redirect("/Cards/All");

            //var viewModel = new DoAddViewModel
            //{
            //    Attack = card.Attack,
            //    Health = card.Health,
            //};
            //return this.View(viewModel);
        }

        //GET /Gards/All
        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var db = new ApplicationDBContext();

            var cardViewModels = db.Cards.Select(c => new CardViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Keyword = c.Keyword,
                Attack = c.Attack,
                Health = c.Health,
                Description = c.Description,
            }).ToList();

            return this.View(cardViewModels); //tova ne minavashe prez Roslyn!!! Ne mojeshe da napravi pravilno typa, kojto iskam,
            //zashtoto List<CardViewModel> ne mojeshe da dobavi referenciq vyv ViewClass classa 
            //kym assemblyto na CardViewModel i mi gyrmeshe!!!
            //zatowa napravihme novo view koeto sydyrja edin List<CardViewModel> i chrez nego podadohme vsichki cards na view-to!!!
            //return this.View(new AllCardsViewModel { Cards = cardViewModels });
            //no posle go opravih kato napravi da vzima pravilniqt type pri generic types i da slaga referenciq kym assemblyto na
            //tozi generic type!!!!
        }

        //GET /Cards/Collection
        public HttpResponse Collection()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }
    }
}
