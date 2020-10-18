using MyFirstMVCApp.ViewModels.Cards;
using System.Collections.Generic;

namespace MyFirstMVCApp.Services
{
    public interface ICardsService
    {
        void AddCard(AddCardInputModel input);

        IEnumerable<CardViewModel> GetAll();

        IEnumerable<CardViewModel> GetByUserId(string userId);

        void AddCardToUserCollection(string userId, int cardId);

        void RemoveCardFromUserCollection(string userId, int cardId);

    }
}
