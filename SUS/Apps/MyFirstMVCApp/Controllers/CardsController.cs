using System;
using MyFirstMVCApp.ViewModels.Cards;
using SUS.HTTP;
using SUS.MvcFramework;
using MyFirstMVCApp.Services;

namespace MyFirstMVCApp.Controllers
{
    public class CardsController : Controller
    {
        //private readonly ApplicationDBContext db; //veche ne mi trqbwa db-to, a samo service!!! Prez servica az dostypvam DB-to!!!
        private readonly ICardsService cardsService;

        public CardsController(/*ApplicationDBContext db,*/ ICardsService cardsService)
        {
            //this.db = db;
            this.cardsService = cardsService;
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

        //[HttpPost("/Cards/Add")]
        ////towa e without InputModel type!!:
        ////public HttpResponse DoAdd(string name, string image, string keyword, int attack, int health, string description)
        //public HttpResponse DoAdd(AddCardInputModel cardInputModel)
        //ne pisha path tuk, zashtoto Add e syshtoto kato Add! Da vidq v UsersController poveche obqsneniq zashto:
        //POST /Cards/Add
        [HttpPost]
        public HttpResponse Add(AddCardInputModel cardInputModel)
        {
            //Niki kaza da pravq tezi validacii dori i tuk v POST methodite, a ne samo v GET!!!!
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            //var dbContext = new ApplicationDBContext(); //tova se iznacq kato vynshno dependency.

            //if (this.Request.FormData["name"].Length < 5) //veche rabotq s modela i zatowa pisha taka:
            if (string.IsNullOrEmpty(cardInputModel.Name) || cardInputModel.Name.Length < 5 || cardInputModel.Name.Length > 15)
            {
                return this.Error($"{nameof(cardInputModel.Name)} should be between 5 and 15 characters long.");
            }

            if (string.IsNullOrWhiteSpace(cardInputModel.Image))
            {
                return this.Error("Image url is required.");
            }

            //taka proverqwam dali mi e validen uri na image, ne me interesuva resultata ot TryCreate, zatova go davam v _:
            if (!Uri.TryCreate(cardInputModel.Image, UriKind.Absolute, out _))
            {
                return this.Error("Image url is nto correct.");
            }

            if (string.IsNullOrWhiteSpace(cardInputModel.Keyword))
            {
                return this.Error("Keyword is required.");
            }

            if (cardInputModel.Attack < 0)
            {
                return this.Error("Attack shoud be non-negative integer.");
            }

            if (cardInputModel.Health < 0)
            {
                return this.Error("Health shoud be non-negative integer.");
            }

            if (string.IsNullOrWhiteSpace(cardInputModel.Description) || cardInputModel.Description.Length > 200)
            {
                return this.Error("Description is required and its length must be at most 200 characters.");
            }

            var cardId = this.cardsService.AddCard(cardInputModel);

            this.cardsService.AddCardToUserCollection(this.GetUserId(), cardId);

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

            var cardViewModels = this.cardsService.GetAll();

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

            var userId = this.GetUserId();

            var cardViewModels = this.cardsService.GetByUserId(userId);

            return this.View(cardViewModels); //tova ne minavashe prez Roslyn!!! Ne mojeshe da napravi pravilno typa, kojto iskam,
            //zashtoto List<CardViewModel> ne mojeshe da dobavi referenciq vyv ViewClass classa 
            //kym assemblyto na CardViewModel i mi gyrmeshe!!!
            //zatowa napravihme novo view koeto sydyrja edin List<CardViewModel> i chrez nego podadohme vsichki cards na view-to!!!
            //return this.View(new AllCardsViewModel { Cards = cardViewModels });
            //no posle go opravih kato napravi da vzima pravilniqt type pri generic types i da slaga referenciq kym assemblyto na
            //tozi generic type!!!!
        }

        //GET /Cards/AddToCollection
        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();

            this.cardsService.AddCardToUserCollection(userId, cardId);

            return this.Redirect("/Cards/All");
        }

        //GET /Cards/AddToCollection
        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var userId = this.GetUserId();
            this.cardsService.RemoveCardFromUserCollection(userId, cardId);

            return this.Redirect("/Cards/Collection");
        }
    }
}
