using SharedTrip.Data;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SharedTrip.Services
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext db;

        public TripService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string Add(AddTripInputModel trip)
        {
            var dbTrip = new Trip
            {
                StartPoint = trip.StartPoint,
                EndPoint = trip.EndPoint,
                DepartureTime = DateTime.ParseExact(trip.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Seats = (byte)trip.Seats,
                Description = trip.Description,
                ImagePath = trip.ImagePath,
            };

            this.db.Trips.Add(dbTrip);
            this.db.SaveChanges();

            return dbTrip.Id;
        }

        public IEnumerable<TripViewModel> GetAll()
        {
            var trips = this.db.Trips.Select(x => new TripViewModel
            {
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                DepartureTime = x.DepartureTime,
                AvailableSeats = Convert.ToInt32(x.Seats) - x.UserTrips.Count(),
                Seats = x.Seats,
                Id = x.Id,
            })
                .Where(t => t.AvailableSeats > 0)
                .ToList();

            return trips;
        }

        public TripDetailsViewModel GetDetails(string tripId)
        {
            var dbTrip = this.db.Trips.FirstOrDefault(x => x.Id == tripId);
            return new TripDetailsViewModel
            {
                Id = dbTrip.Id,
                StartPoint = dbTrip.StartPoint,
                EndPoint = dbTrip.EndPoint,
                Seats = dbTrip.Seats,
                DepartureTime = dbTrip.DepartureTime,
                Description = dbTrip.Description,
                ImagePath = dbTrip.ImagePath,
            };
        }

        public bool AddUserToTrip(string userId, string tripId)
        {
            var userTrip = this.db.UserTrips.Add(new UserTrip
            {
                UserId = userId,
                TripId = tripId,
            });
            this.db.SaveChanges();

            return userTrip != null;
        }

        public bool HasSeatsAvailable(string tripId)
        {
            var trip = this.db.Trips.Where(x => x.Id == tripId).Select(x => new
            {
                Seats = x.Seats,
                TakenSeats = x.UserTrips.Count(),
            }).FirstOrDefault();
            var availableSeats = trip.Seats - trip.TakenSeats;

            return availableSeats > 0;
        }

        public bool IsUserAlreadyInTrip(string userId, string tripId)
        {
            return this.db.UserTrips.Any(x => x.UserId == userId && x.TripId == tripId);
        }
    }
}
