using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    class Program
    {
        static async Task Main(string[] args) //method ne e void veche, a e Task!!! t.e. vryshta Task, zaradi mnogozadachnostta!
        {
            //await ReadData();
            //await ServeData();

            Console.OutputEncoding = Encoding.UTF8;
            const string NewLine = "\r\n";
            TcpListener tcpListener = new TcpListener(
                //IPAddress.Loopback, 80);
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

                    Thread.Sleep(10000); //spiram threada za 10 secunds.

                    string html = $"<h1>Hello from PaulinaServer {DateTime.Now}</h1>" +
                        $"<form action=/tweet method=post><input name=username /><input name=password />" +
                        $"<input type=submit /></form>" + DateTime.Now;

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
