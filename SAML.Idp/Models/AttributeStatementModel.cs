using System.ComponentModel.DataAnnotations;

namespace SAML.Idp.Models
{
    public class AttributeStatementModel
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
