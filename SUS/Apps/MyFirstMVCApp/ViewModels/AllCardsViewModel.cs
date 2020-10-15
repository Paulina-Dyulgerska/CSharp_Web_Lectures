using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMVCApp.ViewModels
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
