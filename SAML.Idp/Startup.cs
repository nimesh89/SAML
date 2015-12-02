using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAML.Idp.Startup))]
namespace SAML.Idp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
