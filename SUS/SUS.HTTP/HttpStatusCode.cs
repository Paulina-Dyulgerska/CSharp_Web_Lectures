namespace SUS.HTTP
{
    public enum HttpStatusCode
    {
        Undefined = 0,
        Ok = 200,
        MovedPermanently = 301,
        Found = 302,
        TemporaryRedirect = 307,
        NotFound = 404,
        ServerError = 500,
    }
}
