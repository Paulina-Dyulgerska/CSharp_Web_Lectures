using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SUS.HTTP
{
    public class HttpServer : IHttpServer
    {
        ICollection<Route> routeTable = new List<Route>();

        public HttpServer(List<Route> routeTable)
        {
            this.routeTable = routeTable;
        }

        //public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
        //{
        //    if (routeTable.ContainsKey(path))
        //    {
        //        routeTable[path] = action;
        //    }
        //    else
        //    {
        //        routeTable.Add(path, action);
        //    }
        //}

        public async Task StartAsync(int port = 80)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                ProcessClientAsync(tcpClient);
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    //TOOD: research if there is faster data structure
                    List<byte> data = new List<byte>();
                    int position = 0;
                    byte[] buffer = new byte[HttpConstants.BufferSize]; //also called chunk
                    while (true)
                    {
                        int count = await stream.ReadAsync(buffer, position, buffer.Length);
                        position = +count;

                        if (count < buffer.Length)
                        {
                            var partialBuffer = new byte[count];
                            Array.Copy(buffer, partialBuffer, count);
                            data.AddRange(partialBuffer);
                            break; //no data left
                        }
                        else
                        {
                            data.AddRange(data);
                        }
                    }

                    //Request
                    //byte[] => string (text)
                    var requestString = Encoding.UTF8.GetString(data.ToArray());
                    var request = new HttpRequest(requestString);
                    //Console.WriteLine(requestString);
                    Console.WriteLine($"{request.Method} {request.Path} => {request.Headers.Count} Headers");

                    //let's go to the route table now and connect the request with the response
                    //Response
                    HttpResponse response;

                    //if (this.routeTable.ContainsKey(request.Path))
                    //{
                    //    var action = this.routeTable[request.Path];
                    //    response = action(request);
                    //}

                    var route = this.routeTable.FirstOrDefault(x => x.Path == request.Path);
                    
                    if (route != null)
                    {
                        var action = route.Action;
                        response = action(request);
                    }
                    else
                    {
                        //404 Not Found
                        response = new HttpResponse("text/html", new byte[0], HttpStatusCode.NotFound);
                    }

                    response.Headers.Add(new Header("Server", "SUS Server 1.0"));
                    response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString()) { HttpOnly = true, MaxAge = 60 * 24 * 60 * 60 });
                    var responseHeaderBytes = Encoding.UTF8.GetBytes(response.ToString());

                    await stream.WriteAsync(responseHeaderBytes, 0, responseHeaderBytes.Length);
                    await stream.WriteAsync(response.Body, 0, response.Body.Length);
                }

                tcpClient.Close();
            }
            catch (Exception e)
            {
                //throw e;
                Console.WriteLine(e);
            }
        }
    }
}
