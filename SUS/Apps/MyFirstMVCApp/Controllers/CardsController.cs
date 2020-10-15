using MyFirstMVCApp.Data;
using MyFirstMVCApp.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Linq;

namespace MyFirstMVCApp.Controllers
{
    public class CardsController : Controller
    {
        public HttpResponse Add()
        {
            return this.View();
        }

        [HttpPost("/Cards/Add")]
        public HttpResponse DoAdd()
        {


            var dbContext = new ApplicationDBContext();

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

            var viewModel = new DoAddViewModel
            {
                Attack = card.Attack,
                Health = card.Health,
            };
            return this.View(viewModel);

            //return this.Redirect("/");
        }

        public HttpResponse All()
        {
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

            return this.View(cardViewModels); //tova ne minavashe prez Roslyn!!! Ne mojeshe da napravi pravilno typa, kojto iskam!!!
            //zatowa napravihme novo view koeto sydyrja List<CardViewModel> i chrez nego podadohme vsichki cards na view-to!!!
            //return this.View(new AllCardsViewModel { Cards = cardViewModels });
            //no posle go opravih kato napravi da vzima pravilniqt type pri generic types!!!!
        }
        public HttpResponse Collection()
        {
            return this.View();
        }
    }
}
