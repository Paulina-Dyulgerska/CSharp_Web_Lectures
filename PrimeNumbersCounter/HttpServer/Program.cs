using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    class Program
    {
        static Dictionary<string, int> SessionStorage = new Dictionary<string, int>();

        const string NewLine = "\r\n";

        static async Task Main(string[] args) //method ne e void veche, a e Task!!! t.e. vryshta Task, zaradi mnogozadachnostta!
        {
            //await ReadData();
            //await ServeData();

            Console.OutputEncoding = Encoding.UTF8;
            TcpListener tcpListener = new TcpListener(
                //IPAddress.Loopback, 80);
                IPAddress.Loopback, 12345);
            tcpListener.Start();
            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                ProcessClientAsync(client); //tuk ne slagam await!!!! Zashtoto iskam da ne se chaka da se svyrshi rabotata
                //po 1 client i togawa da se premina na sledvashtiqt, a tochno obratnoto mi e celta - rabotata da si
                //stoi i da si gowori s clienta, no vseki nov klient DA MOJE da vleze i toj syshto da se nabyrka sys
                //syshtiqt mehtod startiran i za nego. Taka klientite shte mogat da rabotqt paralelno, bez da si 
                //prechat, kato rabota na Task Scheduler e koga na koj da vyrshi neshto - t.e. toj shte precenq koga
                //da pravi neshto ili ne spored estestvoto na rabota, koqto idwa ot klientite mi!!!!
            }
        }

        private static async Task ProcessClientAsync(TcpClient client)
        {
            var clientSocket = client.Client.RemoteEndPoint;
            Console.WriteLine(clientSocket); //vryshta mi tova: 127.0.0.1:65245 - tova e IP-to i porta na browsera na clienta

            using (var stream = client.GetStream())
            {
                byte[] buffer = new byte[1000000];
                var lenght = await stream.ReadAsync(buffer, 0, buffer.Length);

                string requestString = Encoding.UTF8.GetString(buffer, 0, lenght);
                Console.WriteLine(requestString);

                //Thread.Sleep(10000); //spiram threada za 10 secunds.

                //variant da vidq dali veche imam session setnal i da spra da prashtam cookie-to za session id:
                bool sessionSet = false;
                if (requestString.Contains("sid"))
                {
                    sessionSet = true;
                }
                var sid = Guid.NewGuid().ToString();
                //tyrsq dali browsera mi e pratil sid v requesta:
                var match = Regex.Match(requestString, @"sid=[^\n]*\r\n");
                //tyrsq neshto da pochva s sid= posle da ne e nov red i da
                //svyrshva na nov red. Ako namerq takowa neshto, towa znachi, 
                //che imam cookie, koeto mi prashta session id-to.
                //edin i syshti client mi prashta edno i syshto sid!!!
                if (match.Success)
                {
                    sid = match.Value.Substring(4);
                }
                Console.WriteLine(sid);

                //v SessionStarage syhranqwam sega prosto kolko pyti e otvarqna dadena stranica:
                if (!SessionStorage.ContainsKey(sid))
                {
                    SessionStorage.Add(sid, 0);
                }
                SessionStorage[sid]++;

                string html = $"<h1>Hello from PaulinaServer {DateTime.Now} for the {SessionStorage[sid]} time.</h1>" +
                    $"<form action=/tweet method=post><input name=username /><input name=password />" +
                    $"<input type=submit /></form>" + DateTime.Now;

                string response = "HTTP/1.1 200 OK" + NewLine +
                    "Server: PaulinaServer 2020" + NewLine +
                    // "Location: https://www.google.com" + NewLine +
                    "Content-Type: text/html; charset=utf-8" + NewLine +
                    //"Set-Cookie: sid2=0; Domein=localhost; Path=/; Expires=" + DateTime.UtcNow.AddHours(1).ToString("R") + NewLine +
                    //"Set-Cookie: sid3=0; Domein=localhost; Path=/; Max-Age=" + (24 * 60 * 60) + "; HttpOnly" + NewLine +
                    //"Set-Cookie: sid4=0; Domein=localhost; Path=/; Max-Age=0" + "; HttpOnly" + NewLine +
                    //"Set-Cookie: sid5=0; Domein=localhost; Path=/; Expires=" + DateTime.Parse("01/01/2020").ToString("R") + NewLine +
                    (!sessionSet ? ($"Set-Cookie: sid={sid}" + NewLine) : null) +
                    //tova znachi: ako ne mi e dalo session kato cookie v requesta, togawa mu prati towa
                    //cookie, no ako mi e dali session, to togawa ne mu prashtaj tozi Set-Cookie header!
                    //"Set-Cookie: sid2=2; Domein=localhost; Path=/account" + NewLine +
                    // "Content-Disposition: attachment; filename=niki.txt" + NewLine +
                    "Content-Lenght: " + html.Length + NewLine +
                    NewLine +
                    html + NewLine;

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes);

                Console.WriteLine(new string('=', 70));
            }
        }
    }
}
