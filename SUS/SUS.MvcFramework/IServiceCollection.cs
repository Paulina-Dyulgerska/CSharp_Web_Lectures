using System;

namespace SUS.MvcFramework
{
    public interface IServiceCollection //towa mi e dependency container-a!
    {
        //pozlwa se taka tozi method:
        //.Add<IUserService, UserService> //taka shte se znae, che kojto iska IUserService, na tozi nqkoj trqbwa da mu se dade
        //UserService!!! Taka se dobawqt wsichki vryski m/u service kato interface i reanite implementacii za tezi interfaces, 
        //t.e. realnite service classove!!! To towa e edno Dictionary na praktika maj....
        //vsqko prilojenie v negoviqt ConfigureServices method shte poluchawa kato parameter IService collection i v tozi 
        //collection shte si registrira dependency-tata, koito towa prilojenie ima vytre po classovete si!!!
        void Add<TSource, TDestination>();

        //trqbwa da napravi instanciq ot iskaniq type:
        object CreateInstance(Type type);
    }
}
