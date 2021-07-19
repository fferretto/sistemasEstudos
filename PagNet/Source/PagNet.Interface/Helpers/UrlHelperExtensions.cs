using Microsoft.AspNetCore.Http;
using System;

namespace PagNet.Interface.Helpers
{
    public static class UrlHelperExtensions
    {
        private static IHttpContextAccessor HttpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        private static Uri GetAbsoluteUri()
        {
            var request = HttpContextAccessor.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            //uriBuilder.Path = request.Path.ToString();
            //uriBuilder.Query = request.QueryString.ToString();
            return uriBuilder.Uri;
        }

        // Similar methods for Url/AbsolutePath which internally call GetAbsoluteUri
        public static string GetAbsoluteUrl() { return GetAbsoluteUri().ToString(); }
        //public static string GetAbsolutePath() { }
    }
}
