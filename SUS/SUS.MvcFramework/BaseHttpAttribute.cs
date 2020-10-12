using SUS.HTTP;
using System;

namespace SUS.MvcFramework
{
    //prez tezi attributes shte moga da zadavm cusstom url, kojto url da wika methoda pod attributa!!! Defaultniq attribute shte e:
    //taka: /ControllerName/MethodName, no az iskam da moje da se kaje, che daden method shte otgovarq na izbran ot men route, a ne
    //defaultniq - towa shte go naprawq prez attributite i vyzmojnostta da se pishe HttpGet("/paulina") i kato se otvarq
    ///paulina da se vika tozi method, a ne defaultniq route da vika tozi method pod attributa!!!!!! 
    //Ili pyk Home page, kojto se otvarq na /, a ne na /home, zatowa ni e nujen custom route i kojto izbere "/" da se prashta na tozi
    //method!!!
    public abstract class BaseHttpAttribute : Attribute
    {
        public string Url { get; set; }

        public abstract HttpMethod Method { get; } //iskam da zadylja vseki edin naslednik da implementira towa property!!
        //Vseki naslednik iskam da ima Method i da e opredeleno kakyv mu e Methoda!!!
    }
}
