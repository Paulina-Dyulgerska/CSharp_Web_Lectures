namespace SharedTrip.ViewModels.Trips
{
    public class TripDetailsViewModel : TripViewModel
    {
        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string DepartureTimeAsDateTimeLocalForHtml => this.DepartureTime.ToString("s");
        //tova e formata, kojto 
        //chroma chete za type="datetime-local"
        //Kogato imam problem s nachina, po kojto browsera mi vizualizira datetime, da obyrna vnimanie kakwo iska html-a 
        //kato type i da potyrsq
        //kak trqbwa da si formatiram DateTime tipa s ToString(), za da syotvetstwam na iskaniq type.Imah pole s
        //type= "datetime-local" i go
        //opravih s tozi format "s". Eto towa vreme se vryshta s "s" formatiraneto:
        //> DateTime.Now.ToString("s")
        //"2020-10-23T14:33:49" --tochno tova vreme s T posledata go razbra Chrome-to!!!!
    }
}
