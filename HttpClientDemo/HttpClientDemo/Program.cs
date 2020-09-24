using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientDemo
{
    class Program
    {
        static async Task Main(string[] args) //method ne e void veche, a e Task!!! t.e. vryshta Task, zaradi mnogozadachnostta!
        {
            //await ReadData();
            await ServeData();
        }

        //Primer kak si pisha kod, ako sym server!!! T.e.pravq se na web server:
        public static async Task ServeData()
        {

            //s TcpListener moga da si poiskam port ot OS i tq da ni dawa vsichko, koeto dojde na tozi port, na men,
            //na moeto prilojenie.
            //TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 12345); //inicializiram TcpListener-a!
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80); //slagam si da otgovarq na defaultniq port - 80!
            //kato e na 80, nqma da e nujno da pisha celiqt address, za da stigna do moqta programa, t.e. nqma nujda da pisha:
            //127.0.0.1:12345, a shte moga da pisha samo 127.0.0.1 ili localhost, i browsera shte prashta na port 80 moqta
            //zaqwka!!!! Defaultniq port na https e 443!!!
            //IPAddress.Loopback == localhost == 127.0.0.1 (IP v.4) == 127.0.0.1::1 (IP v.6) - moqta sobstvena mashina
            //iskam moqta sobstvena mashina da stane server i da mi dade OS port 12345, toj shte se vijda v localnata
            //mreja samo!!!
            tcpListener.Start(); //zadejstvam TcpListener-a, sega OS mi dawa port 12345 i az zapochvam da rabotq na
            //nego!

            //iskam da ne mi se zatvori programata i zatowa go pisha, posle si pravq while cycle i ne mi trqbwa veche:
            //Console.ReadLine();

            //Ako startiram 2-ra programa na edin i syshti port, shte polucha tozi Exception:
            //Unhandled exception. System.Net.Sockets.SocketException(10048): Only one usage of each socket address(protocol / network address / port) is normally permitted.
            //at System.Net.Sockets.Socket.UpdateStatusAfterSocketErrorAndThrowException(SocketError error, String callerName)
            //at System.Net.Sockets.Socket.DoBind(EndPoint endPointSnapshot, SocketAddress socketAddress)
            //at System.Net.Sockets.Socket.Bind(EndPoint localEP)
            //at System.Net.Sockets.TcpListener.Start(Int32 backlog)
            //at System.Net.Sockets.TcpListener.Start()
            //at HttpClientDemo.Program.ServeData() in D:\CSharp\Projects\Web_CSharp_Lectures_Demos\HttpClientDemo\HttpClientDemo\Program.cs:line 27
            //at HttpClientDemo.Program.Main(String[] args) in D:\CSharp\Projects\Web_CSharp_Lectures_Demos\HttpClientDemo\HttpClientDemo\Program.cs:line 15
            //at HttpClientDemo.Program.< Main > (String[] args)

            //sega si pravq service (naricha se demon v Linux), tova e programa, koqto pochva da raboti i nikoga ne spira!
            //towa e edna ot malkoto upotrebi na bezkraen cycle v programiraneto!!!!!
            while (true)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                //tcpListenera mi chaka client!
                //kazwam neshto podobno na Console.ReadLine(); - t.e. chakam client da dojde i kogato toj dojde, az
                //vzimam informaciqta za tozi clinet i tcpListener.AcceptTcpClient() se otblokira syshto kakto 
                //Console.ReadLine() se otblokira, kogato mu vyveda neshto ot konzolata!!!!
                //v TcpClient imam vsichko, koeto mi trqbwa, za da komunikiram s potrebitelq!!!

                //moga da pisha v stream-a ili da cheta stream-a, kojto client mi e pratil, zashtoto kogato pisha ili
                //cheta danni, az rabotq sys Stream! Tozi stream e merjovi stream.
                //kogato cheta - stream pristiga na chunkove, obiknoveno 4096B e razmera na 1 chunk.
                //az gi cheta kakto se obrabotva Stream i gi zapisvam na porcii, na chunkove!!!!
                //ako ne go sloja v using, nqma da zatvorq Stream-a i browsera shte prodyljava da cikli
                //do bezkraj!!!!
                using (var stream = client.GetStream()) //vzimam stream-a i veche moga da pisha i cheta v nego!
                {
                    //kak e naj-pravilniq algorith za rabota sys streamove: ne e tozi, zashtoto trqbwa da cheta
                    //streama na porcii, a  ne taka na kup!!!!
                    //pravq si 1 buffer, za da go polzwam za pisane i chetene
                    int byteLenght = 0;
                    //byte[] buffer = new byte[4096]; //shteshe da e takyv, ako chetqh streama na porcii, ama nqma
                    //da go cheta taka sega.
                    byte[] buffer = new byte[1000000];
                    //dokato chete ot streama, demek ima kakwo da se chete v nego, procheti ot streama ot byteLenght Byte
                    //natatyk, buffer.Lenght broq Bytes i gi zapishi tezi procheteni Bytes v buffera !!!
                    var readLenght = stream.Read(buffer, byteLenght, buffer.Length);

                    //polucheniq bytes[] moga da go razcheta, zashtoto znam, che towa e po http protokola
                    //i az kato polzwam Encoding, moga da si go obyrna v symbols!!!!
                    //Encoding, napravi array-a mi ot bytes na string!!!Towa mu kazwam:
                    string requestString = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(requestString);

                    //startiram si programata!!! Tq raboti i chaka zaqwki!!!
                    //koj moje da mi prashta zaqwki? vseki, kojto ima dostyp do moqta programa na socket: 127.0.0.1:12345
                    //ili na localhost:12345!!!!!
                    //t.e. browsera moga da go pratq da mi naprawi zaqwki na port 12345!!!

                    //tova poluchih kato informacia vyv streama - towa dojde ot browsera po TCP!!!!!!!!!!!!
                    //GET / HTTP / 1.1
                    //Host: 127.0.0.1:12345
                    //Connection: keep - alive
                    //Cache - Control: max - age = 0
                    //Upgrade - Insecure - Requests: 1
                    //User - Agent: Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 85.0.4183.102 Safari / 537.36
                    //Accept: text / html,application / xhtml + xml,application / xml; q = 0.9,image / avif,image / webp,image / apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9
                    //Sec-Fetch-Site: none
                    //Sec-Fetch-Mode: navigate
                    //Sec-Fetch-User: ?1
                    //Sec-Fetch-Dest: document
                    //Accept-Encoding: gzip, deflate, br
                    //Accept-Language: en-US,en;q=0.9,bg;q=0.8

                    //sega shte mu vyrna neshto na browsera :):
                    //edinstveniq pravilen nov red, kogato rabotq s vynshni ustrojstwa, t.e. kogato rabotq po 
                    //mrejata, e \r\n!!!! Ako rabotq na lokalna mashina - nov red e Enviroment.NewLine!!!
                    //No po mrejata, a osobeno za http, nov red e \r\n!!!! Http e prielo tezi \r\n da se interpretirat
                    //kato nov red, zashtoto nikoj ne znae otsreshta Mac, Linux ili Windows stoi!!!!

                    const string NewLine = "\r\n";

                    string html = $"<h1>Hello from Paulina's Server \r {DateTime.Now} часа</h1>" +
                        $"<form method=POST><input name=username />" +
                        $"<input name=password type=\"password\"/>" +
                        $"<input type=submit /></form>";

                    //string response = "HTTP/1.1 307 Redirect" + NewLine +
                    string response = "HTTP/1.1 200 OK" + NewLine +
                        "Server: Paulina's server 2020" + NewLine +
                        //"Location: https://www.google.com" + NewLine +
                        //"Content-Disposition: inline; filename=PaulinaContent.txt" + NewLine + //nishto ne napravi towa!
                        //"Content-Disposition: attachment; filename=PaulinaContent.txt" + NewLine + //savena mi file v Downloads!
                        //"Content-Type: text/plain; charset=utf-8" + NewLine +
                        "Content-Type: text/html; charset=utf-8" + NewLine + //tova e samo za html!!
                        "Content-Lenght: " + html.Length + NewLine +
                        NewLine +
                        html + NewLine;

                    //Encoding, napravi response-to mi na array ot bytes!!! Towa mu kazwam:
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    stream.Write(responseBytes);

                    Console.WriteLine(new string('=', 60));
                }
            }
        }

        //Primer kak si pisha kod, ako sym client!!! T.e. pravq se na browser:
        public static async Task ReadData()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string url = "https://softuni.bg/trainings/3164/csharp-web-basics-september-2020/internal#lesson-18189";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Test", "test......"); //taka si dobavqm moi headers kym requesta.
            var html = await httpClient.GetStringAsync(url);
            //tozi method vryshta Task(string), t.e. towa e zadacha, koqto trqbwa da se svyrshi ot nqkoj i da
            //mi se vyrne string v html.
            //zashto mi trqbwa da go razdelqm na zadachi - zashtoto taka moga da polzwam poveche ot qdrata na CPU-to
            //i te paralelno da rabotqt i da izpylnqwat ednovremenno poveche zadachi!!!
            //Ako imam 1 edinstvena zadacha, samo 1 qdro moje da q vyrshi!!!!
            //Kogato razdelq neshtata na otdelni zadachi, mnogo qdra mogat da gi smqtat tezi malki chasti ednovremenno
            //i da polucha po-byrzo rezultat, otkolkoto ako raboti samo 1 qdro!
            //vseki red ot 3-ta po-gore e otdelna zadacha!!!
            //v html poluchavam tozi pyrvonachalen html, kojto i browsera poluchawa.
            Console.WriteLine(html);

            Console.WriteLine("Drug nachin da vzema neshta ot response-to:***********************************");
            var response = await httpClient.GetAsync(url);
            Console.WriteLine(response.Headers);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.IsSuccessStatusCode); //true
            Console.WriteLine(response);
        }

        //coda na Niki:
        static async Task NikisMain(string[] args) //method ne e void veche, a e Task!!! t.e. vryshta Task, zaradi mnogozadachnostta!
        {
            //await ReadData();
            //await ServeData();

            Console.OutputEncoding = Encoding.UTF8;
            const string NewLine = "\r\n";
            TcpListener tcpListener = new TcpListener(
                IPAddress.Loopback, 12345);
            tcpListener.Start();
            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1000000];
                    var lenght = stream.Read(buffer, 0, buffer.Length);

                    string requestString =
                        Encoding.UTF8.GetString(buffer, 0, lenght);
                    Console.WriteLine(requestString);

                    string html = $"<h1>Hello from NikiServer {DateTime.Now}</h1>" +
                        $"<form action=/tweet method=post><input name=username /><input name=password />" +
                        $"<input type=submit /></form>";

                    string response = "HTTP/1.1 200 OK" + NewLine +
                        "Server: NikiServer 2020" + NewLine +
                        // "Location: https://www.google.com" + NewLine +
                        "Content-Type: text/html; charset=utf-8" + NewLine +
                        // "Content-Disposition: attachment; filename=niki.txt" + NewLine +
                        "Content-Lenght: " + html.Length + NewLine +
                        NewLine +
                        html + NewLine;

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes);

                    Console.WriteLine(new string('=', 70));
                }
            }
        }
    }
}
////async version of the class:
//    class Program
//    {
//        const string NewLine = "\r\n";
//static async Task Main(string[] args) //method ne e void veche, a e Task!!! t.e. vryshta Task, zaradi mnogozadachnostta!
//{
//    //await ReadData();
//    //await ServeData();

//    Console.OutputEncoding = Encoding.UTF8;
//    TcpListener tcpListener = new TcpListener(
//        //IPAddress.Loopback, 80);
//        IPAddress.Loopback, 12345);
//    tcpListener.Start();
//    while (true)
//    {
//        var client = tcpListener.AcceptTcpClient();
//        ProcessClientAsync(client); //tuk ne slagam await!!!! Zashtoto iskam da ne se chaka da se svyrshi rabotata
//                                    //po 1 client i togawa da se premina na sledvashtiqt, a tochno obratnoto mi e celta - rabotata da si
//                                    //stoi i da si gowori s clienta, no vseki nov klient DA MOJE da vleze i toj syshto da se nabyrka sys
//                                    //syshtiqt mehtod startiran i za nego. Taka klientite shte mogat da rabotqt paralelno, bez da si 
//                                    //prechat, kato rabota na Task Scheduler e koga na koj da vyrshi neshto - t.e. toj shte precenq koga
//                                    //da pravi neshto ili ne spored estestvoto na rabota, koqto idwa ot klientite mi!!!!
//    }
//}

//private static async Task ProcessClientAsync(TcpClient client)
//{
//    using (var stream = client.GetStream())
//    {
//        byte[] buffer = new byte[1000000];
//        var lenght = await stream.ReadAsync(buffer, 0, buffer.Length);

//        string requestString = Encoding.UTF8.GetString(buffer, 0, lenght);
//        Console.WriteLine(requestString);

//        Thread.Sleep(10000); //spiram threada za 10 secunds.

//        string html = $"<h1>Hello from PaulinaServer {DateTime.Now}</h1>" +
//            $"<form action=/tweet method=post><input name=username /><input name=password />" +
//            $"<input type=submit /></form>" + DateTime.Now;

//        string response = "HTTP/1.1 200 OK" + NewLine +
//            "Server: NikiServer 2020" + NewLine +
//            // "Location: https://www.google.com" + NewLine +
//            "Content-Type: text/html; charset=utf-8" + NewLine +
//            // "Content-Disposition: attachment; filename=niki.txt" + NewLine +
//            "Content-Lenght: " + html.Length + NewLine +
//            NewLine +
//            html + NewLine;

//        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
//        await stream.WriteAsync(responseBytes);

//        Console.WriteLine(new string('=', 70));
//    }
//}
//    }
