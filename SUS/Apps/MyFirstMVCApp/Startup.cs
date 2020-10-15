﻿using Microsoft.EntityFrameworkCore;
using MyFirstMVCApp.Controllers;
using MyFirstMVCApp.Data;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Collections.Generic;

namespace MyFirstMVCApp
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices()
        {
        }

        public void Configure(List<Route> routeTable)
        {
            //tezi sa mi izlichni, zashtoto gi vkarvam avtomatichno chrez methoda v Host:  
            //private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application)
            ////routes for the dynamic views:
            //routeTable.Add(new Route("/", HttpMethod.Get, new HomeController().Index));
            //routeTable.Add(new Route("/home/about", HttpMethod.Get, new HomeController().About));
            //routeTable.Add(new Route("/users/login", HttpMethod.Get, new UsersController().Login));
            //routeTable.Add(new Route("/users/login", HttpMethod.Post, new UsersController().DoLogin));
            //routeTable.Add(new Route("/users/register", HttpMethod.Get, new UsersController().Register));
            //routeTable.Add(new Route("/cards/add", HttpMethod.Get, new CardsController().Add));
            //routeTable.Add(new Route("/cards/all", HttpMethod.Get, new CardsController().All));
            //routeTable.Add(new Route("/cards/collection", HttpMethod.Get, new CardsController().Collection));

            ////StaticFileController classa stana izlishen, zashtoto vsichko towa go pravq v Host avtomatichno s dobavqneto v RouteTable!!!!!!
            ////s tozi method:  private static void AutoRegisterStaticFile(List<Route> routeTable)
            ////routes for the static files:
            //routeTable.Add(new Route("/favicon.ico", HttpMethod.Get, new StaticFilesController().Favicon));
            //routeTable.Add(new Route("/css/bootstrap.min.css", HttpMethod.Get, new StaticFilesController().BootstrapCss));
            //routeTable.Add(new Route("/css/custom.css", HttpMethod.Get, new StaticFilesController().CustomCss));
            //routeTable.Add(new Route("/js/bootstrap.bundle.min.js", HttpMethod.Get, new StaticFilesController().BootstrapJs));
            //routeTable.Add(new Route("/js/custom.js", HttpMethod.Get, new StaticFilesController().CustomJs));
            //routeTable.Add(new Route("/img/cat.png", HttpMethod.Get, new StaticFilesController().CatImg));

            //new ApplicationDBContext().Database.Migrate();

        }
    }
}