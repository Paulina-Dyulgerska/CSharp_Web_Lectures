﻿using Microsoft.VisualBasic;
using System;

namespace SharedTrip.ViewModels.Trips
{
  public class AddTripInputModel
    {
        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        //public DateTime DepartureTime { get; set; } //i taka moje
        public string DepartureTime { get; set; }

        public string ImagePath { get; set; }

        public int Seats { get; set; }

        public string Description { get; set; }
    }
}
