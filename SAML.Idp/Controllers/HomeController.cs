using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Kentor.AuthServices.HttpModule;
using Kentor.AuthServices.Mvc;
using Kentor.AuthServices.Saml2P;
using Kentor.AuthServices.WebSso;
using SAML.Idp.Models;

namespace SAML.Idp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index(Guid? idpId)
        {
            var requestData = Request.ToHttpRequestData();
            if (requestData.QueryString["SAMLRequest"].Any())
            {
                var decodedXmlData = Saml2Binding.Get(Saml2BindingType.HttpRedirect)
                    .Unbind(requestData);

                var request = Saml2AuthenticationRequest.Read(decodedXmlData);

                var model = new AssertionModel();

                model.InResponseTo = request.Id;
                model.AssertionConsumerServiceUrl = request.AssertionConsumerServiceUrl.ToString();
                model.AuthnRequestXml = decodedXmlData;
                model.NameId = ((ClaimsIdentity) User.Identity).Name;

                var response = model.ToSaml2Response();

                var commandResult = Saml2Binding.Get(Saml2BindingType.HttpPost)
                    .Bind(response);

                return commandResult.ToActionResult();
            }

            throw new InvalidOperationException();
        }
    }
}