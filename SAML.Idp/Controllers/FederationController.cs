namespace SAML.Idp.Controllers
{
    using System.Web.Mvc;

    using Kentor.AuthServices.Metadata;

    using SAML.Idp.Models;

    public class FederationController : Controller
    {
        // GET: Federation
        public ActionResult Index()
        {
            return this.Content(
                MetadataModel.CreateFederationMetadata().ToXmlString(),
                "application/samlmetadata+xml");
        }

        public ActionResult BrowserFriendly()
        {
            return this.Content(
                MetadataModel.CreateFederationMetadata().ToXmlString(),
                "text/xml");
        }
    }
}