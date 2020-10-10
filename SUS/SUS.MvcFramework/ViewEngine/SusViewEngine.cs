using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SUS.MvcFramework.ViewEngine
{
    public class SusViewEngine : IViewEngine
    {
        public string GetHtml(string templateCode, object viewModel)
        {
            string csharpCode = GenerateCSharpFromTemplate(templateCode);

            IView executableObject = GenerateExecutableCode(csharpCode, viewModel);
            //napravi si IView, samo za da opishe v interface nalichieto
            //na method ExecuteTemplate(viewModel)!!! 
            //Ako beshte ostavil executableObject da e object, to object nqma method ExecuteTemplate(viewModel)
            //i towa shteshe da mu spyva pisaneto na coda nadolu i compilera vse mu mrynkashe za nego.
            //t.e. interface-ite sa neshto kato obeshtaniq kakwo shte mogat da pravqt obektite, koito implementirat tezi interfaces!!!!
            //chrez tozi interface az moga da rabotq po coda i da polzwam executableObject s methoda mu ExecuteTemplate(viewModel) oshte 
            //predi da sym syzdala realniqt method ExecuteTemplate(viewModel)!!!!

            string html = executableObject.ExecuteTemplate(viewModel);

            return html;
        }

        private string GenerateCSharpFromTemplate(string templateCode)
        {
            string methodBody = GetMethodBody(templateCode);
            string csharpCode = @"
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using SUS.MvcFramework.ViewEngine;

namespace ViewNamespace
{
    public class ViewClass : IView
    {
        public string ExecuteTemplate(object viewModel)
        {
            var html = new StringBuilder();
            
            " + methodBody + @"

            return html.ToString();
        }
    }
}
";

            return csharpCode;
        }

        private string GetMethodBody(string templateCode)
        {
            return string.Empty;
        }

        private IView GenerateExecutableCode(string csharpCode, object viewModel)
        {
            //with Roslyn
            // C# -> executable -> IView -> ExecuteTemplate
            //tova po-dolu se naricha generirane i compilirane v pametta (in memory) na compa na cod!

            var compileResult = CSharpCompilation.Create("ViewAssembly") //davam ime na assemblyto, koeto shte se compilene v pametta na kompa
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                //kazwam mu kakyv type shte e towa, koeto shte se compilene in memory, pravq prazen class library v pametta na compa!
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                //tova e libraryto, koeto vseki edin project referira - tam e bazoviq class object!!! Tq mi trqbwa, zashtoto moqt class 
                //e dlyjen da nasledqwa OBJECT!!!!
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                //iskam da referira i libraryto, v koeto se namira interfeisa IView, zashtoto v koda mi ima using SUS.MvcFramework.ViewEngine
                //i zatowa trqbva da imam referenciq kym tova library, za da pozlwam IView!!!
                ;

            if (viewModel != null)
            {
                compileResult = compileResult.AddReferences(MetadataReference.CreateFromFile(viewModel.GetType().Assembly.Location));
                //vzimam asseblyto, v koeto se namira typa na viewModel, zashtoto az ne go znam predvaritelno kakyv type e viewModel,
                //moje da e vsqkakyv type, no s reflecion moga da vzema typa mu i ot tam da vzema locationa na assemblyto na tozi type
                //i da si dobavq path-qt kym towa assembly, za da otide to kato referenciq na novoto mi in memory syzdadeno assembly.
            }

            //trqbwa da dobavq vsichki onezi sublibraries, koito sa chast ot .NET Standard!!!!
            //shte gi vrytna s edin reflection!

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            //zaredi mi libraryto, koeto se kazwa NetStandard
            //netstandard e standartnata min sbirshtina ot libraryta, koito predstavlqwat NET Standard!!! Net Core ima mnogo poveche neshta!
            //Net Standard e naj-malkoto obshto kratno na wsichki .NET neshta, vsichki te nadgrajdat NET Standard!!!Na men mi trqbwa samo NET Standard
            //i zatowa sega shte si vzema samo neq.
            //vzimam spisyka s asseblitata, koito netstandard referira s .GetReferencedAssemblies()!!!
            //tozi spisyk shte si go foreachna i shte si go dobavq v moqt project, t.e. v moeto novo assembly

            foreach (var library in libraries)
            {
                compileResult = compileResult.AddReferences(MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            //dobavqm C# coda kym novoto mi assembly
            compileResult = compileResult.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(csharpCode));

            //compileResult.Emit("view.dll"); //ne mi trqbwa dll, a tezi bytes ot IL cod da sa mi v pametta
            //Da vnimawam adski mnogo kak si pisha usinging v C# string-a!!!
            //Bqh zabrawila; sled edin using i bez obyrkala imeto na System.Collections.Generic mi gyrmeshe s error:
            //BadImageFormatException: Image is too small !!!!I mi praveshe dll, ama s OB razmer i nishto v nego!!!
            //Sled kato opravih greshki, vsichko trygna!!!Mnogo da vnimawam s C# coda s stringa!!!

            using (MemoryStream memoryStream = new MemoryStream())
            {
                EmitResult result = compileResult.Emit(memoryStream);

                if (!result.Success)
                {
                    //have compile errors! Poluchavam info za errors, inache sym zagubena bez towa!
                    return new ErrorView(result.Diagnostics
                        .Where(e => e.Severity == DiagnosticSeverity.Error)
                        .Select(e => e.GetMessage())
                        .AsEnumerable(), csharpCode);
                }

                //za da se vyrna da cheta streama ot nachaloto, trqbwa da pozicioniram glavata v nachaloto i ot tam da pochna cheteneto mu:
                memoryStream.Seek(0, SeekOrigin.Begin);
                var byteAssembly = memoryStream.ToArray(); //pravq memoryStreama na array
                var assembly = Assembly.Load(byteAssembly); //pravq array-a na Assembly
                var viewType = assembly.GetType("ViewNamespace.ViewClass"); //vzimam si typa na view-to
                var instance = Activator.CreateInstance(viewType);
                return instance as IView; //sigurna sym che tozi obekt implementira IView!!!
            }
        }
    }
}
