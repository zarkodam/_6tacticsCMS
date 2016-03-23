using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.GoogleMaps
{
    public class Map
    {
        [Required]
        [RegularExpression("^[0-9A-Za-z]+$", ErrorMessage = "Should be 0-9 a-z A-Z without whitespace")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "zoom")]
        public int Zoom { get; set; }

        [JsonProperty(PropertyName = "mapCenterLatitude")]
        public string MapCenterLatitude { get; set; }

        [JsonProperty(PropertyName = "mapCenterLongitude")]
        public string MapCenterLongitude { get; set; }

        [JsonProperty(PropertyName = "locations")]
        public List<Location> Locations { get; set; }

        public Map()
        {
            Locations = new List<Location>();
        }
    }
}