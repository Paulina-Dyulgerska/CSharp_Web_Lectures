using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SUS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
            this.FormData = new Dictionary<string, string>();

            var lines = requestString.Split(new string[] { HttpConstants.NewLine }, System.StringSplitOptions.None);

            //GET /somepage HTTP/1.1
            var headerLine = lines[0];
            var headerLineParts = headerLine.Split(' ');
            //parse string to enum HttpMethod ignoring the case types (upper or lower)
            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            this.Path = headerLineParts[1];

            int lineIndex = 1;
            bool isInHeaders = true;
            StringBuilder bodyBuilder = new StringBuilder();

            while (lineIndex < lines.Length)
            {
                var line = lines[lineIndex];
                lineIndex++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    //read headers
                    this.Headers.Add(new Header(line));
                }
                else
                {
                    //read body
                    bodyBuilder.AppendLine(line);
                }

            }

            if (this.Headers.Any(x => x.Name == HttpConstants.RequestCookieHeader))
            {
                var cookiesAsString = this.Headers.FirstOrDefault(x => x.Name == HttpConstants.RequestCookieHeader).Value;

                var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookieAsString in cookies)
                {
                    this.Cookies.Add(new Cookie(cookieAsString));

                }
            }

            this.Body = bodyBuilder.ToString();

            if (this.Body != "")
            {
                //var parameters = this.Body.Split('&');
                var parameters = this.Body.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var parameter in parameters)
                {
                    var parameterParts = parameter.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    var name = parameterParts[0];
                    var value = 0.ToString();

                    if (parameterParts.Length > 1)
                    {
                       value = WebUtility.UrlDecode(parameterParts[1]); //pravi mi normalen url string s ://, inache te sa zameneni s %3M i dr.podobni
                    }

                    if (!this.FormData.ContainsKey(name))
                    {
                        this.FormData.Add(name, value);
                    }
                }
            }
        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public IDictionary<string, string> FormData { get; set; }

        public string Body { get; set; }
    }
}