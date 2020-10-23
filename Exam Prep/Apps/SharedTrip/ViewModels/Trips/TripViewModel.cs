using System;
using System.Globalization;

namespace SharedTrip.ViewModels.Trips
{
  public  class TripViewModel
    {
        public string Id { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public string FullName => $"{this.StartPoint} => {this.EndPoint}";

        public DateTime DepartureTime { get; set; }

        public string DetartureTimeFormated => this.DepartureTime.ToString(CultureInfo.GetCultureInfo("bg-BG"));
        //public string DetartureTimeFormated => this.DepartureTime.ToString("dd.MM.yyyy HH:mm");

        public int AvailableSeats { get; set; }

        public byte Seats { get; set; }
    }
}
