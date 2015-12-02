namespace SAML.Idp.Controllers
{
    using System.Web.Mvc;

    using SAML.Idp.Models;

    public class DiscoveryServiceController : Controller
    {
        public ActionResult Index(DiscoveryServiceModel model)
        {
            if(model.isPassive || this.Request.HttpMethod == "POST")
            {
                string delimiter = model.@return.Contains("?") ? "&" : "?";

                return this.Redirect(string.Format(
                    "{0}{1}{2}={3}",
                    model.@return,
                    delimiter,
                    model.returnIDParam,
                    model.SelectedIdp));
            }

            return this.View(model);
        }
    }
}