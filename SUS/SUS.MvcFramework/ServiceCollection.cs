using System;
using System.Collections.Generic;
using System.Linq;

namespace SUS.MvcFramework
{
    public class ServiceCollection : IServiceCollection
    {
        private Dictionary<Type, Type> dependencyContainer = new Dictionary<Type, Type>();

        public void Add<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public object CreateInstance(Type type)
        {
            //iskam serviceCollectiona, kogato nqkoj mu poiska instanciq, toj da pravi instanciq, syobrazqwajki se s constructorite.
            //tozi method shte go polzwam tam, kydeto konstuiram controllera!!!

            if (dependencyContainer.ContainsKey(type))
            {
                type = this.dependencyContainer[type];
            }
            //ako type-to ne e registrirano v dependencyContainera, az pak shte mu naprawq instanciq, zashtoto ne hvyrlqm greshka
            //i taka ako nqkoj mi iska ApplicationDbContext v nqkoj constructor, az shte napravq takyv object i shte go instanciram,
            //a nqma da mqtna greshka, poneje tozi type ApplicationDbContext NE E registriran
            //v serviceCollectiona (dependencyContainera mi)!!!!!

            //iskam constructora s naj-malko parameters, zashtoto toj naj-lesno moje da byde syzddaden.
            //v ASP.NET Core e analogichno, no tam se vzima pyrviqt, kojto moje da byde udovletvoren.
            //dobre e servica i controllera da imat po edin edinstwen constructor.
            var constructor = type.GetConstructors()
                .OrderBy(x => x.GetParameters().Count())
                .FirstOrDefault();

            var parameters = constructor.GetParameters();
            var parameterValues = new List<object>();

            foreach (var parameter in parameters)
            {
                var parameterValue = CreateInstance(parameter.ParameterType);
                //za vseki parameter na constructora, izvikaj tozi method recursivno kato parameter na methoda shte byde 
                //typa na parametera na construktora. Taka edin sled drug shte se izpylnqwa methoda za wseki edin parameter
                //na constructora i na constructora na parametera i za constructora na parametera na parameterea i t.n.
                //taka ot controllerskiq constructor se iska iservice, toj se pravi, no servickiq constructor iska db context,
                //db contexta se pravi!!!! Taka ot edin controller se stiga do DB-to!!!!
                parameterValues.Add(parameterValue);
            }

            var obj = constructor.Invoke(parameterValues.ToArray());
            return obj;

            //towa ne e pylno, ako samo taka go ostawq!!!!! Gorniqt cod e pravilnata implementaciq na methoda CreateInstance.
            //return Activator.CreateInstance(type);
        }
    }
}
