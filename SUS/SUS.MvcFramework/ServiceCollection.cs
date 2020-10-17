using System;
using System.Collections.Generic;
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
            return Activator.CreateInstance(type);
        }
    }
}
