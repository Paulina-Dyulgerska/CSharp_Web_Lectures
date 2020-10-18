using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SUS.MvcFramework
{
    public class ServiceCollection : IServiceCollection
    {
        private Dictionary<Type, Type> dependencyContainer = new Dictionary<Type, Type>();

        public ServiceCollection()
        {

        }

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

            //iskam tozi s naj-malko parameters, zashtoto toj naj-lesno moje da byde syzddaden.
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
                parameterValues.Add(parameterValue);
            }

            var obj = constructor.Invoke(parameterValues.ToArray());
            return obj;

            //towa ne e pylno, ako samo taka go ostawq!!!!! Gorniqt cod e pravilnata implementaciq na methoda CreateInstance.
            //return Activator.CreateInstance(type);
        }
    }
}
