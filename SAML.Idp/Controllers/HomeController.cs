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

                var manager = SessionManager.Instance;
                

                var response = model.ToSaml2Response();

                manager.AddSession(model.NameId, new Session()
                {
                    Id = Guid.Parse(request.Id.Substring(2)),
                    Ip = Request.UserHostAddress,
                    UserAgent = Request.UserAgent,
                    LogoutUrl = request.Issuer.Id,
                    Issuer = response.Issuer.Id
                });

                var commandResult = Saml2Binding.Get(Saml2BindingType.HttpPost)
                    .Bind(response);

                return commandResult.ToActionResult();
            }

            throw new InvalidOperationException();
        }
    }
}