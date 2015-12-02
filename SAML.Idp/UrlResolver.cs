﻿namespace SAML.Idp
{
    using System;
    using System.Linq;
    using System.Web;

    public static class UrlResolver
    {
        private static Uri GetCombinedUrl(string path)
        {
            var applicationPathSegmentsCount = new Uri(HttpContext.Current.Request.Url, HttpContext.Current.Request.ApplicationPath).Segments.Length;
            var namedIdpSegment = HttpContext.Current.Request.Url.Segments.Skip(applicationPathSegmentsCount).FirstOrDefault(); // find guid part if any
            if (!string.IsNullOrEmpty(namedIdpSegment) && !namedIdpSegment.EndsWith("/"))
            {
                namedIdpSegment = namedIdpSegment + "/";
            }
            Guid parsedGuid;
            if (!string.IsNullOrEmpty(namedIdpSegment) && Guid.TryParse(namedIdpSegment.TrimEnd('/'), out parsedGuid))
            {
                return new Uri(HttpContext.Current.Request.Url, HttpContext.Current.Request.ApplicationPath + namedIdpSegment + path);
            }
            return new Uri(HttpContext.Current.Request.Url, HttpContext.Current.Request.ApplicationPath + path);
        }

        public static Uri RootUrl
        {
            get
            {
                return GetCombinedUrl("");
            }
        }


        public static Uri SsoServiceUrl
        {
            get
            {
                return RootUrl;
            }
        }

        public static Uri MetadataUrl
        {
            get
            {
                return GetCombinedUrl("Metadata");
            }
        }

        public static Uri ManageUrl
        {
            get
            {
                return GetCombinedUrl("Manage");
            }
        }
    }
}