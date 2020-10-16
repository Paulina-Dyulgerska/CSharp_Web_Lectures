namespace SUS.MvcFramework.ViewEngine
{
   public interface IView
    {
        string ExecuteTemplate(object viewModel, string user);
        //mnogo e vajno towa, che tozi interface e interfaca zad ViewClass classa mi, kojto ViewClass se generira runtime kato assembly!!!
        //ako ne sloja na ExecuteTemplate syshtata definiciq, kato tazi, koqto imam v stringovoto opisanie na ViewClass, to shte mi gyrmi
        //za nespazwane na interface!!! Da ne go zabrawqm towa, zashtoto compile time NQMA da imam greshka!!!
    }
}
