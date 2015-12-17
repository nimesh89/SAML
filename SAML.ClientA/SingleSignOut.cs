using System.Collections.Concurrent;
using System.IdentityModel.Metadata;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kentor.AuthServices.Mvc;

namespace SAML.ClientA
{
    public class SingleSignOut : FilterAttribute, IActionFilter
    {
        private static readonly ConcurrentDictionary<string, string> UserForLogOutDictionary = new ConcurrentDictionary<string, string>();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User != null && (filterContext.HttpContext.User.Identity as ClaimsIdentity).Claims.Any())
            {
                var currentUserClaim = (filterContext.HttpContext.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (currentUserClaim != null)
                {
                    var currentUser = currentUserClaim.Value;
                    if (currentUser != null)
                        if (UserForLogOutDictionary.ContainsKey(currentUser))
                        {
                            FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                            filterContext.HttpContext.Response.Redirect(UrlHelper.GenerateContentUrl("~/", filterContext.HttpContext));
                            filterContext.HttpContext.Response.End();
                            string temp;
                            UserForLogOutDictionary.TryRemove(currentUserClaim.Value, out temp);
                        }
                }

            }

            var extendeduser = filterContext.HttpContext.Request.Form.Get("SAMLLogoutUser");

            
            if (string.IsNullOrEmpty(extendeduser))
            {
                return;
            }

            var user = extendeduser.Split(";".ToCharArray())[0];
            var issuer = HttpUtility.UrlDecode(extendeduser.Split(";".ToCharArray())[1]);

            var signed = filterContext.HttpContext.Request.Form.Get("hash");
            var cert = AuthServicesController.Options.IdentityProviders[new EntityId() { Id = issuer }];

            RSAPKCS1SignatureDeformatter RSADeformatter = new RSAPKCS1SignatureDeformatter(cert.SigningKeys.First());
            RSADeformatter.SetHashAlgorithm("SHA1");
            SHA1Managed SHhash = new SHA1Managed();
            if (RSADeformatter.VerifySignature(
             SHhash.ComputeHash(new UnicodeEncoding().GetBytes(HttpUtility.UrlEncode(extendeduser))),
             System.Convert.FromBase64String(signed))
             )
            {
                UserForLogOutDictionary[user] = "true";
                return;
            }

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            return;
        }
    }
}