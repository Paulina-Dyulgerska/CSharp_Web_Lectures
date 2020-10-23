using SharedTrip.ViewModels.Trips;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface ITripService
    {
        string Add(AddTripInputModel trip);

        IEnumerable<TripViewModel> GetAll();

        TripDetailsViewModel GetDetails(string tripId);

        bool AddUserToTrip(string userId, string tripId);

        bool HasSeatsAvailable(string tripId);

        bool IsUserAlreadyInTrip(string userId, string tripId);
    }
}
