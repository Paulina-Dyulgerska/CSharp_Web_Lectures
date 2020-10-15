using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SUS.HTTP
{
    public class HttpRequest
    {
        public static IDictionary<string, Dictionary<string, string>> Sessions = new Dictionary<string, Dictionary<string, string>>();
        //vseki pyt kogato vleze potrebitelq i napravi nqkakwa zaqwka, 

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

            var sessionCookie = this.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionCookieName);
            //SUS_SID e imeto na session cookie-to mi!!! S tova Cookie, koeto shte ima Path "/", t.e. shte vaji za vseki edin sait ot 
            //moq sait, az veche kydeto i da se razhojdam, towa cookie shte e edno i syshto i shte e neizmenno s men. Taka az imam
            //veche edin unikalen identifikator na usera i znam, koga usera e edin i sysht i koga ne e!!!! User s edno i syshto
            //session cookie, za servera shte e edin i sysht user!!!
            //Veche vseki edin Cotnroller moje da se vyzpolzwa ot sessiona i da pishe i da chete ot neq. Dostypva sessiona chrez
            //this.request.Session!!!!

            if (sessionCookie == null)
            //ako nqma session cookie ili ako usera e podal session cookie, no to (sessionId-to mu) ne se sydyrja v spisyka sys sessiite:
            //togawa pravim nova session cookie!!!
            {
                var sessionId = Guid.NewGuid().ToString();
                this.Session = new Dictionary<string, string>();
                Sessions.Add(sessionId, this.Session);
                //shte si pazq Id-to na sessiona.
                //Za nova seesionId, shte se pravi nov Dictionary i tam shte se pazqt na novata session .....coookietata za sessiona li?????
                this.Cookies.Add(new Cookie(HttpConstants.SessionCookieName, sessionId));
            }
            else if (!Sessions.ContainsKey(sessionCookie.Value))
                //ako nqma session s takowa Id, to dobavi q s towa tochno Id v Sessions!!!
                //tova e za sluchaite, v koito sym otvorila sajta, posle sym zatvorila prilojenieto i posle pak sym otvorila saita:
            {
                this.Session = new Dictionary<string, string>();
                Sessions.Add(sessionCookie.Value, this.Session);
            }
            else
            {
                this.Session = Sessions[sessionCookie.Value]; //po id-to shte vzema tekushtata sessiq!!! sessionCookie ima name SUS_SID
                //i value, koeto e sessionId-to!!!!! Taka shte znam, che usera mi e syshtiq kato ot predi malko i shte razpoznawam
                //koj mi se razhojda po stranicata!!!!
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

        public Dictionary<string, string> Session { get; set; }

        public string Body { get; set; }
    }
}