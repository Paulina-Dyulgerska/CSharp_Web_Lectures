using SUS.HTTP;

namespace SUS.MvcFramework
{
    public class HttpGetAttribute : BaseHttpAttribute
    {
        public HttpGetAttribute()
        {
        }

        public HttpGetAttribute(string ulr)
        {
            this.Url = ulr;
        }

        public override HttpMethod Method => HttpMethod.Get;
    }
}
