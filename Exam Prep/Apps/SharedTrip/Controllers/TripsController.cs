using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService tripService;

        public TripsController(ITripService tripService)
        {
            this.tripService = tripService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(input.StartPoint))
            {
                return this.Error("Invalid start point.");
            }

            if (string.IsNullOrWhiteSpace(input.EndPoint))
            {
                return this.Error("Invalid end point");
            }

            if (string.IsNullOrWhiteSpace(input.Description) || input.Description.Length > 80)
            {
                return this.Error("Description is required and has max length of 80 characters.");
            }

            if (input.Seats < 2 || input.Seats > 6)
            {
                return this.Error("Seats should be between 2 and 6.");
            }

            if (!DateTime.TryParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal, out _))
            {
                return this.Error("DateTime should be in format dd.MM.yyyy HH:mm");
            }

            this.tripService.Add(input);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this.tripService.GetAll();

            return this.View(viewModel);
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            
            var viewModel = this.tripService.GetDetails(tripId);

            return this.View(viewModel);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.tripService.HasSeatsAvailable(tripId))
            {
                return this.Error("No available seats.");
            }

            var userId = this.GetUserId();
           
            if (this.tripService.IsUserAlreadyInTrip(userId, tripId))
            {
                //return this.Details(tripId); //Niki ne go napravi taka, zashototo taka ne se pravi Redirect!!! Taka se pravi
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            var result = this.tripService.AddUserToTrip(userId, tripId);

            if (!result)
            {
                return this.Error("User could not be added to this trip.");
            }

            return this.Redirect("/Trips/All");
        }
    }
}
