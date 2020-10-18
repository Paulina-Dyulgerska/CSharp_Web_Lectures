using MyFirstMVCApp.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMVCApp.Services
{
    public class CardsService : ICardsService
    {
        private readonly ApplicationDBContext db;

        public CardsService(ApplicationDBContext db)
        {
            this.db = db;
        }

        public void AddCard()
        {
            throw new NotImplementedException();
        }
    }
}
