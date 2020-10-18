﻿using MyFirstMVCApp.Data;
using MyFirstMVCApp.ViewModels.Cards;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstMVCApp.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDBContext db;

        public CardsService(ApplicationDBContext db)
        {
            this.db = db;
        }

        public void AddCard(AddCardInputModel input)
        {
            var card = new Card
            {
                Name = input.Name,
                ImageUrl = input.Image,
                Keyword = input.Keyword,
                Attack = input.Attack,
                Health = input.Health,
                Description = input.Description,
            };

            this.db.Cards.Add(card);

            this.db.SaveChanges();
        }

        public IEnumerable<CardViewModel> GetAll()
        {
          return this.db.Cards.Select(c => new CardViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Keyword = c.Keyword,
                Attack = c.Attack,
                Health = c.Health,
                Description = c.Description,
            }).ToList();
        }

        public IEnumerable<CardViewModel> GetByUserId(string userId) 
        {
        return this.db.UserCards.Where(x => x.UserId == userId)
                .Select(c => new CardViewModel()
                {
                    Id = c.Card.Id,
                    Name = c.Card.Name,
                    ImageUrl = c.Card.ImageUrl,
                    Keyword = c.Card.Keyword,
                    Attack = c.Card.Attack,
                    Health = c.Card.Health,
                    Description = c.Card.Description,
                }).ToList(); //ako zabravq da materializiram i mi ostane IQueryable tuk, to zaqwkata shte se materializira
            //chak vyv View-to, koeto ne e problem, ama shte propusna error-ite, koito moga da hvana, 
            //ako q materializiram tuk oshte!!!!
        }

        public void AddCardToUserCollection(string userId, int cardId)
        {
            if (this.db.UserCards.Any(x => x.UserId == userId && x.CardId == cardId))
            {
                return;
            }

            this.db.UserCards.Remove(new UserCard { CardId = cardId, UserId = userId });
            this.db.SaveChanges();
        }

        public void RemoveCardFromUserCollection(string userId, int cardId)
        {
            //this.db.UserCards.Remove(new UserCard { CardId = cardId, UserId = userId });

            var userCard = this.db.UserCards.FirstOrDefault(x => x.CardId == cardId && x.UserId == userId);

            if (userCard == null)
            {
                return;
            }

            this.db.UserCards.Remove(userCard);
            this.db.SaveChanges();
        }
    }
}
