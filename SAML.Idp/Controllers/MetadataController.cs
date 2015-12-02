namespace SAML.Idp.Controllers
{
    using System.Web.Mvc;

    using Kentor.AuthServices.Metadata;

    using SAML.Idp.Models;

    public class MetadataController : Controller
    {
        // GET: Metadata
        public ActionResult Index()
        {
            return this.Content(
                MetadataModel.CreateIdpMetadata().ToXmlString(),
                "application/samlmetadata+xml");
        }

        public ActionResult BrowserFriendly()
        {
            return this.Content(
                MetadataModel.CreateIdpMetadata().ToXmlString(),
                "text/xml");
        }
    }
}