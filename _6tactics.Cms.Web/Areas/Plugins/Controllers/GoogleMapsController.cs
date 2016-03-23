using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Services.Plugins;
using _6tactics.Cms.Web.Areas.Plugins.Enums.GoogleMaps;
using _6tactics.Cms.Web.Areas.Plugins.Models.GoogleMaps;
using _6tactics.Cms.Web.Areas.Plugins.ViewModels.GoogleMaps;
using _6tactics.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Areas.Plugins.Controllers
{
    public class GoogleMapsController : Controller
    {
        private readonly IPluginsManagerService _pluginsManagerService;
        private static List<Map> _savedMaps = new List<Map>();
        private static GoogleMapPreviewSetup _googleMapPreviewSetup = new GoogleMapPreviewSetup();
        private static string _mapToLoad;

        private void LoadSettings()
        {
            var mapList = _pluginsManagerService.GetPluginData<List<Map>>("GoogleMaps", "savedMaps");
            if (mapList != null)
                _savedMaps = mapList;

            var previewMapSettings = _pluginsManagerService.GetPluginData<GoogleMapPreviewSetup>("GoogleMaps", "googleMapPreviewSetup");
            if (previewMapSettings != null)
                _googleMapPreviewSetup = previewMapSettings;

        }

        public GoogleMapsController(IPluginsManagerService pluginsManagerService)
        {
            _pluginsManagerService = pluginsManagerService;
            LoadSettings();
        }

        [ChildActionOnlyCustom, ExcludeFromPluginActionList]
        public PartialViewResult GoogleMap(string mapName = "")
        {
            Map map = new Map();

            if (_savedMaps.Any())
                map = string.IsNullOrWhiteSpace(mapName)
                    ? _savedMaps.FirstOrDefault()
                    : _savedMaps.FirstOrDefault(i => i.Title.Equals(mapName));

            if (map == null) return PartialView(new PreviewViewModel());

            var model = new PreviewViewModel
            {
                Title = map.Title,
                PreviewSetupJsonString = _pluginsManagerService.GetPluginDataAsJsonString("GoogleMaps", "googleMapPreviewSetup"),
                MapJsonString = _pluginsManagerService.SerializeModelAsJson(map)
            };

            return PartialView(model);
        }

        [ChildActionOnlyCustom, ExcludeFromPluginActionList]
        public PartialViewResult GoogleMapsTitlesList(GoogleMapPreviewOption previewOption = GoogleMapPreviewOption.Normal, string selectedTitle = null)
        {

            IEnumerable<SelectListItem> titlesList;

            if (previewOption == GoogleMapPreviewOption.Fluid)
                titlesList = _savedMaps.Where(i => i.Title.CustomContains("full", StringComparison.InvariantCultureIgnoreCase))
                    .Select(i => new SelectListItem { Value = i.Title, Text = i.Title, Selected = i.Title.Equals(selectedTitle) });
            else
                titlesList = _savedMaps.Where(i => !i.Title.CustomContains("full", StringComparison.InvariantCultureIgnoreCase))
                    .Select(i => new SelectListItem { Value = i.Title, Text = i.Title, Selected = i.Title.Equals(selectedTitle) });

            return PartialView(titlesList);
        }

        [AjaxOnly]
        public PartialViewResult GoogleMapBuilder()
        {
            return PartialView();
        }

        [AjaxOnly]
        public PartialViewResult PreviewSetup()
        {
            return PartialView(_googleMapPreviewSetup);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void PreviewSetup(GoogleMapPreviewSetup model)
        {
            _googleMapPreviewSetup = model;
            _pluginsManagerService.SavePluginData("GoogleMaps", "googleMapPreviewSetup", model);
        }

        [AjaxOnly, ExcludeFromPluginActionList]
        public PartialViewResult ChooseAction()
        {
            var model = new GoogleMapsWizardViewModel
            {
                MapListItems = _savedMaps.Select(i => new SelectListItem { Value = i.Title, Text = i.Title }),
                Map = new Map()
            };

            return PartialView(model);
        }

        [AjaxOnly, ExcludeFromPluginActionList]
        public PartialViewResult MapLocations()
        {
            var model = new MapLocationsViewModel
            {
                Map = _savedMaps.FirstOrDefault(i => i.Title.Equals(_mapToLoad)),
                Location = new Location()
            };
            return PartialView(model);
        }

        [AjaxOnly, ExcludeFromPluginActionList]
        public PartialViewResult MapSettings()
        {
            var model = new MapSettingsViewModel
            {
                Map = _savedMaps.FirstOrDefault(i => i.Title.Equals(_mapToLoad)),
                MapJsonString = _pluginsManagerService.SerializeModelAsJson(_savedMaps.FirstOrDefault(i => i.Title.Equals(_mapToLoad)))
            };

            return PartialView(model);
        }

        [AjaxOnly, ExcludeFromPluginActionList]
        public PartialViewResult Preview()
        {
            var model = new PreviewViewModel
            {
                PreviewSetupJsonString = _pluginsManagerService.GetPluginDataAsJsonString("GoogleMaps", "googleMapPreviewSetup"),
                MapJsonString = _pluginsManagerService.SerializeModelAsJson(_savedMaps.FirstOrDefault(i => i.Title.Equals(_mapToLoad)))
            };

            return PartialView(model);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void MapToLoad(string mapToLoad)
        {
            _mapToLoad = mapToLoad;
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public PartialViewResult RemoveMap(string mapToRemove)
        {
            Map selectedMap = _savedMaps.First(i => i.Title.Equals(mapToRemove));
            _savedMaps.Remove(selectedMap);

            _pluginsManagerService.SavePluginData("GoogleMaps", "savedMaps", _savedMaps);

            var model = new GoogleMapsWizardViewModel
            {
                MapListItems = _savedMaps.Select(i => new SelectListItem { Value = i.Title, Text = i.Title }),
                Map = new Map()
            };

            return PartialView("ChooseAction", model);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public PartialViewResult RemoveLocation(string mapTitle, string formattedAddress)
        {
            Map selectedMap = _savedMaps.First(i => i.Title.Equals(mapTitle));

            List<Location> locations = selectedMap.Locations;
            locations.Remove(locations.First(i => i.FormattedAddress.Equals(formattedAddress)));

            _pluginsManagerService.SavePluginData("GoogleMaps", "savedMaps", _savedMaps);

            return PartialView("_LocationsList", selectedMap);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void AddNewMap(Map map)
        {
            _savedMaps.Add(map);
            _pluginsManagerService.SavePluginData("GoogleMaps", "savedMaps", _savedMaps);
            _mapToLoad = map.Title;
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void AddSettingsToMap(Map map)
        {
            map.Title = _mapToLoad;
            Map mapToAddSettings = _savedMaps.First(i => i.Title.Equals(map.Title));
            mapToAddSettings.MapCenterLatitude = map.MapCenterLatitude;
            mapToAddSettings.MapCenterLongitude = map.MapCenterLongitude;
            mapToAddSettings.Zoom = map.Zoom;

            _pluginsManagerService.SavePluginData("GoogleMaps", "savedMaps", _savedMaps);
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddLocationToMap(MapLocationsViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return Content("<div class=\"location-error-message\"><b>Search</b> location first!</div>");

            Map selectedMap = _savedMaps.First(i => i.Title.Equals(model.Map.Title));

            if (selectedMap.Locations == null)
                selectedMap.Locations = new List<Location> { model.Location };
            else
            {
                if (!selectedMap.Locations.Any(i => i.FormattedAddress.Equals(model.Location.FormattedAddress)))
                    selectedMap.Locations.Add(model.Location);
            }

            _pluginsManagerService.SavePluginData("GoogleMaps", "savedMaps", _savedMaps);

            return PartialView("_LocationsList", selectedMap);
        }
    }
}