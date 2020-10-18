using System.Collections.Generic;

namespace MyFirstMVCApp.ViewModels.Cards
{
    public class AllCardsViewModel
    {
        public AllCardsViewModel()
        {
            this.Cards = new List<CardViewModel>();
        }

        public List<CardViewModel> Cards { get; set; }
    }
}
