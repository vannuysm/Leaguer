using System.Net;

namespace Leaguerly.Api.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static HttpStatusCode UnprocessableEntity { get { return (HttpStatusCode) 422; } }
    }
}