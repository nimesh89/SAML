using System;
using System.IdentityModel.Services;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Kentor.AuthServices.Mvc;

namespace SAML.ClientB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FederatedAuthentication.SessionAuthenticationModule.SignedOut += SessionAuthenticationModule_SignedOut;
        }

        private void SessionAuthenticationModule_SignedOut(object sender, EventArgs e)
        {
            var provider = AuthServicesController.Options.IdentityProviders.Default;
            Response.Redirect(provider.SingleSignOnServiceUrl + "Account/Logoff");
        }
    }
}
