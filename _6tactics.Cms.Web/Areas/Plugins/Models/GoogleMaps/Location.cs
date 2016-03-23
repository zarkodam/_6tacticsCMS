using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.GoogleMaps
{
    public class Location
    {
        [Required(ErrorMessage = "Required"), JsonProperty(PropertyName = "formattedAddress")]
        public string FormattedAddress { get; set; }

        [Required(ErrorMessage = "Required"), JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Required"), JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "zIndex")]
        public int ZIndex { get; set; }

        [JsonProperty(PropertyName = "customLogoUrl")]
        public string CustomLogoUrl { get; set; }
    }
}