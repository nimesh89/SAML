using System.Collections.Concurrent;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kentor.AuthServices.Mvc;

namespace SAML.ClientA
{
    public class SingleSignOut : FilterAttribute, IActionFilter
    {
        private static readonly ConcurrentDictionary<string, string> UserForLogOutDictionary = new ConcurrentDictionary<string, string>();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
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
            }

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            return;
        }
    }
}