using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    class Program
    {
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
            using (var stream = client.GetStream())
            {
                byte[] buffer = new byte[1000000];
                var lenght = await stream.ReadAsync(buffer, 0, buffer.Length);

                string requestString = Encoding.UTF8.GetString(buffer, 0, lenght);
                Console.WriteLine(requestString);

                bool sessionSet = false;

                if (requestString.Contains("sid"))
                {
                    sessionSet = true;
                }

                //Thread.Sleep(10000); //spiram threada za 10 secunds.

                string html = $"<h1>Hello from PaulinaServer {DateTime.Now}</h1>" +
                    $"<form action=/tweet method=post><input name=username /><input name=password />" +
                    $"<input type=submit /></form>" + DateTime.Now;

                string response = "HTTP/1.1 200 OK" + NewLine +
                    "Server: PaulinaServer 2020" + NewLine +
                    // "Location: https://www.google.com" + NewLine +
                    "Content-Type: text/html; charset=utf-8" + NewLine +
                    "Set-Cookie: sid2=0; Domein=localhost; Path=/; Expires=" +
                                            DateTime.UtcNow.AddSeconds(20).ToString("R") + NewLine +
                    (!sessionSet ? ("Set-Cookie: sid=daeqweqwe921302932131231" + NewLine) : null) +
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
