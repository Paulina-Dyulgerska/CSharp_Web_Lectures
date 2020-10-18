using MyFirstMVCApp.Data;
using System;
using MyFirstMVCApp.ViewModels.Cards;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Linq;

namespace MyFirstMVCApp.Controllers
{
    public class CardsController : Controller
    {
        private readonly ApplicationDBContext db;

        public CardsController(ApplicationDBContext db)
        {
            this.db = db;
        }

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
        //towa e without InputModel type!!:
        //public HttpResponse DoAdd(string name, string image, string keyword, int attack, int health, string description)
        public HttpResponse DoAdd(AddCardInputModel cardInputModel)
        {
            //Niki kaza da pravq tezi validacii dori i tuk v POST methodite, a ne samo v GET!!!!
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            //var dbContext = new ApplicationDBContext(); //tova se iznacq kato vynshno dependency.

            if (this.Request.FormData["name"].Length < 5)
            {
                return this.Error("Name should be at least 5 characters long.");
            }

            var card = new Card
            {
                Name = cardInputModel.Name,
                ImageUrl = cardInputModel.Image,
                Keyword = cardInputModel.Keyword,
                Attack = cardInputModel.Attack,
                Health = cardInputModel.Health,
                Description = cardInputModel.Description,
            };

            this.db.Cards.Add(card);

            this.db.SaveChanges();

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

            var cardViewModels = this.db.Cards.Select(c => new CardViewModel()
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
