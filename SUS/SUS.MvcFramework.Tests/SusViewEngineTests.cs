using SUS.MvcFramework.ViewEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SUS.MvcFramework.Tests
{
    public class SusViewEngineTests
    {
        //[Fact] //tova e == na [Test] v NUnit test labraryto
        [Theory] //pozvolqwa mi da napravq edin tests s parameters.
        [InlineData("CleanHtml")]
        [InlineData("Foreach")]
        [InlineData("IfElseFor")]
        [InlineData("ViewModel")]

        public void TestGetHtml(string fileName)
        {
            var viewModel = new TestViewModel
            {
                Name = "Doggo Arghentino",
                Price = 12345.67M,
                DateOfBirth = new DateTime(2019, 6, 1),
            };

            IViewEngine viewEngine = new SusViewEngine();
            var view = File.ReadAllText($"ViewTests/{fileName}.html");
            var result = viewEngine.GetHtml(view, viewModel, null);
            var expectedResult = File.ReadAllText($"ViewTests/{fileName}.Result.html");
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestTemplateViewModel()
        {
            IViewEngine viewEngine = new SusViewEngine();
            var result = viewEngine.GetHtml(@"@foreach(var num in Model)
{
<span>@num</span>
}
", new List<int> { 1, 2, 3 }, null);

            var expectedResult = @"<span>1</span>
<span>2</span>
<span>3</span>
";
            Assert.Equal(expectedResult, result);
        }
    }

    //Kogato imam class vytre v drug class, FullName na typa na vytreshniq class e kofti:
    //SUS.MvcFramework.Tests.SusViewEngineTests+TestViewModel Model = viewModel;
    //var html = new StringBuilder();
    //TestViewModel e class vytre v classa SusViewEngineTests i zatowa kato poiskam imeto na typa na tozi TestViewModel mi se vryshta:
    //imeto na classa, v kojto e definiran TestViewModel + imeto na typa na TestViewModel!!! Towa mi chupi logikite.
    //zatowa da ne slagam class vytre v class, a da gi dyrja vseki class da e samostoqtelen i obshtoto m/u classovete da e namespaca, v kojto
    //te sa declarirani!!!
    public class TestViewModel
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
