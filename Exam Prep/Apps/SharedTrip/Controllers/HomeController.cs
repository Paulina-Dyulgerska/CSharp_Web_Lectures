namespace SharedTrip.Controllers
{
    using SUS.HTTP;
    using SUS.MvcFramework;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                this.Redirect("/Trips/All");
            }

            return this.View();
        }
    }
}
