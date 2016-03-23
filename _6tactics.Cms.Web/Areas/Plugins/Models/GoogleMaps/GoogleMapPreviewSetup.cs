using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace _6tactics.Cms.Web.Areas.Plugins.Models.GoogleMaps
{
    public class GoogleMapPreviewSetup
    {
        #region MapSettings

        // searchAction, zoomAndCenterAction, previewAction
        //[Required, JsonProperty(PropertyName = "action")]
        //public string Action { get; set; }

        //// when id is clicked map position is set to center
        //[Required, Display(Name = "Map resize event activator id"), JsonProperty(PropertyName = "mapResizeEventActivatorId")]
        //public string MapResizeEventActivatorId { get; set; }

        // continent country town street street number
        [Required, Display(Name = "Default address"), JsonProperty(PropertyName = "defaultAddress")]
        public string DefaultAddress { get; set; }

        // default zoom
        [Required, Display(Name = "Map zoom"), JsonProperty(PropertyName = "zoom")]
        public int Zoom { get; set; }

        // enable or disable scroll wheel
        [Required, Display(Name = "Support for scroll wheel"), JsonProperty(PropertyName = "scrollwheel")]
        public bool Scrollwheel { get; set; }

        // is marker visible
        [Required, Display(Name = "Marker visibility"), JsonProperty(PropertyName = "markerVisibility")]
        public bool MarkerVisibility { get; set; }

        // marker image url
        [Required, Display(Name = "Marker image url"), JsonProperty(PropertyName = "markerImageUrl")]
        public string MarkerImageUrl { get; set; }

        // is info window visible
        [Required, Display(Name = "Infowindow visibility"), JsonProperty(PropertyName = "infowindowVisibility")]
        public bool InfowindowVisibility { get; set; }

        // enable map styling
        [Required, Display(Name = "GoogleMap style"), JsonProperty(PropertyName = "isGoogleMapStyleEnabled")]
        public bool IsGoogleMapStyleEnabled { get; set; }

        // info window max width
        [Required, Display(Name = "Infowindow max width css value"), JsonProperty(PropertyName = "infowindowMaxWidth")]
        public string InfowindowMaxWidth { get; set; }

        // info window max height
        [Required, Display(Name = "Infowindow max height css value"), JsonProperty(PropertyName = "infowindowMaxHeight")]
        public string InfowindowMaxHeight { get; set; }

        // info windows image url
        [Display(Name = "Info window image url"), JsonProperty(PropertyName = "infowindowImageUrl")]
        public string InfowindowImageUrl { get; set; }

        #endregion


        #region SearchActionSettings

        //// id for input field where the searched(formatted) address will be shown 
        //[Required, Display(Name = "Formatted address id"), JsonProperty(PropertyName = "formattedAddressId")]
        //public string FormattedAddressId { get; set; }

        //// id for input field where the searched latitude will be shown 
        //[Required, Display(Name = "Address latitude id"), JsonProperty(PropertyName = "addressLatitudeId")]
        //public string AddressLatitudeId { get; set; }

        //// id for input field where the searched latitude will be shown 
        //[Required, Display(Name = "Address longitude id"), JsonProperty(PropertyName = "addressLongitudeId")]
        //public string AddressLongitudeId { get; set; }

        // TODO: look that in plugin
        ////addresszIndexId: "0", // id for input field where the zIndex will be set 
        //[Required, Display(Name = "Addressz index id"), JsonProperty(PropertyName = "addresszIndexId")]
        //public string AddresszIndexId { get; set; }

        //// id for search input field 
        //[Required, Display(Name = "Search submit element id"), JsonProperty(PropertyName = "searchSubmitElementId")]
        //public string SearchSubmitElementId { get; set; }

        //// function(){}
        //[Required, Display(Name = "Add location form callback"), JsonProperty(PropertyName = "addLocationFormCallback")]
        //public string AddLocationFormCallback { get; set; }

        #endregion


        #region ZoomAndCenterActionSettings

        //// id for input field where the current zoom will be shown 
        //[Required, Display(Name = "Map zoom id"), JsonProperty(PropertyName = "mapZoomId")]
        //public string MapZoomId { get; set; }

        //// input id where the current latitude will be shown
        //[Required, Display(Name = "Map latitude id"), JsonProperty(PropertyName = "mapLatitudeId")]
        //public string MapLatitudeId { get; set; }

        //// input id where the current longitude will be shown
        //[Required, Display(Name = "Map longitude id"), JsonProperty(PropertyName = "mapLongitudeId")]
        //public string MapLongitudeId { get; set; }

        //// get saved locations from json (look at the savedLocationsExample.json file)
        //[Required, Display(Name = "SavedLocations"), JsonProperty(PropertyName = "savedLocations")]
        //public string SavedLocations { get; set; }

        #endregion


        #region PreivewActionSettings

        //automaticallyCenterLocations: true // sets added locations to center of the map (avoids mapLatitudeId and mapLongitudeId)
        [Required, Display(Name = "Automatically center locations"), JsonProperty(PropertyName = "automaticallyCenterLocations")]
        public bool AutomaticallyCenterLocations { get; set; }

        #endregion

        public GoogleMapPreviewSetup()
        {
            // Default values

            #region MapSettings

            DefaultAddress = "europe";
            Zoom = 2;
            MarkerVisibility = true;
            InfowindowVisibility = true;
            Scrollwheel = true;
            InfowindowMaxWidth = "330px";
            InfowindowMaxHeight = "auto";

            #endregion


            #region PreivewActionSettings

            AutomaticallyCenterLocations = true;

            #endregion

        }
    }
}