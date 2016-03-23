using _6tactics.Cms.Web.Areas.Plugins.Models.GoogleMaps;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Areas.Plugins.ViewModels.GoogleMaps
{
    public class GoogleMapsWizardViewModel
    {
        public IEnumerable<SelectListItem> MapListItems { get; set; }
        public Map Map { get; set; }
    }
}