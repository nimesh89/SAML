namespace SAML.Idp.Controllers
{
    using System.Web.Mvc;

    using SAML.Idp.Models;

    public class DiscoveryServiceController : Controller
    {
        public ActionResult Index(DiscoveryServiceModel model)
        {
            string delimiter = model.@return.Contains("?") ? "&" : "?";

            return this.Redirect(string.Format(
                "{0}{1}{2}={3}",
                model.@return,
                delimiter,
                model.returnIDParam,
                model.SelectedIdp));
        }
    }
}