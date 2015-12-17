using System.Web.Mvc;

namespace SAML.ClientB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SingleSignOut());
        }
    }
}
