namespace SAML.Idp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class DiscoveryServiceModel
    {
        public DiscoveryServiceModel()
        {
            this.returnIDParam = "entityID";
            this.SelectedIdp = UrlResolver.MetadataUrl.ToString();
        }

        [Display(Name="SP Entity ID")]
        public string entityID { get; set; }

        [Display(Name="Return URL to SP")]
        [Required]
        public string @return { get; set; }

        [Display(Name="Return ID Param")]
        [Required]
        public string returnIDParam { get; set; }

        public bool isPassive { get; set; }

        [Display(Name="Entity ID of Selected Idp")]
        [Required]
        public string SelectedIdp { get; set; }
    }
}