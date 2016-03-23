using System.Web.Optimization;

namespace _6tactics.Cms.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // COMMON INCLUDES
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/Scripts/googleMaps", "http://maps.googleapis.com/maps/api/js?v=3.exp&libraries=places"));

            // WEB LAYOUT - STYLES
            bundles.Add(new StyleBundle("~/Content/web-styles").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/owl.carousel.css",
                        "~/Content/owl.theme.css",
                        "~/Content/owl.transitions.css",
                        "~/Content/animate.css",
                        "~/Content/cubeportfolio.css",
                        "~/Content/magnificPopup.css",
                        "~/Content/font-awesome.css",
                        "~/Content/ProjectCustomStyle/Web/parallax-slider.css",
                        "~/Content/ProjectCustomStyle/Web/resmenu.css",
                        "~/Content/ProjectCustomStyle/Web/display-types-default.css",
                        "~/Content/ProjectCustomStyle/Web/web-common.css",
                        "~/Content/ProjectCustomStyle/Web/6tacticsCMS.css",
                        //"~/Content/ProjectCustomStyle/Web/peglanje-snova.css",
                        "~/Content/ProjectCustomStyle/Common/validation-helpers-style.css",
                        "~/Content/ProjectCustomStyle/Common/common.css",
                        "~/Content/ProjectCustomStyle/Common/popups.css",
                        "~/Content/ProjectCustomStyle/Common/mail-form.css",
                        "~/Content/ProjectCustomStyle/Common/google-maps-wizard.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/peglanje-snova-style").Include(
                        "~/Content/peglanje-snova.css"
                 ));

            bundles.Add(new StyleBundle("~/Content/contente-sustavi-style").Include(
                        "~/Content/contente-sustavi.css"
                 ));

            // ADMIN LAYOUT - STYLES
            bundles.Add(new StyleBundle("~/Content/admin-styles").Include(
                      //"~/Content/jquery-ui-1.10.4.custom.min.css",
                      "~/Content/magnificPopup.css",
                      "~/Content/jquery.scrollbar.css",
                      //"~/Content/jquery.mCustomScrollbar.css",
                      "~/Content/bootstrap.css",
                      "~/Content/ProjectCustomStyle/Admin/gear-animation.css",
                      "~/Content/ProjectCustomStyle/Admin/admin.css",
                      "~/Content/ProjectCustomStyle/Admin/current-location-bar.css",
                      "~/Content/ProjectCustomStyle/Admin/tree-view.css",
                      "~/Content/ProjectCustomStyle/Admin/file-manager.css",
                      "~/Content/ProjectCustomStyle/Admin/jquery.steps.css",
                      "~/Content/ProjectCustomStyle/Admin/jquery.steps-custom.css",
                      "~/Content/ProjectCustomStyle/Common/validation-helpers-style.css",
                      "~/Content/ProjectCustomStyle/Common/common.css",
                      "~/Content/ProjectCustomStyle/Common/popups.css",
                      "~/Content/ProjectCustomStyle/Common/mail-form.css",
                      "~/Content/ProjectCustomStyle/Common/google-maps-wizard.css"
                      ));






            // WEB LAYOUT - SCRIPTS BEFORE BODY
            bundles.Add(new ScriptBundle("~/Scripts/web-scripts-before-body").Include(
                        "~/Scripts/modernizr.*",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/_6tactics/resmenu.js",
                        "~/Scripts/_6tactics/resmenuBootstrap.js",
                        "~/Scripts/_6tactics/animatedScrollNavigation.js",
                        "~/Scripts/_6tactics/selectedPagePanel.js"

                        ));

            // WEB LAYOUT - SCRIPTS AFTER BODY
            bundles.Add(new ScriptBundle("~/Scripts/web-scripts-after-body").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/magnificPopup.js",
                        "~/Scripts/owl.carousel.min.js",
                        "~/Scripts/jquery.form.js",
                        "~/Scripts/jquery.cslider.js",
                        "~/Scripts/jquery.cubeportfolio.js",
                        "~/Scripts/_6tactics/layoutFixxxer.js",
                        "~/Scripts/_6tactics/duplicatedIdHandler.js",
                        "~/Scripts/_6tactics/googleMapsWizard.js",
                        "~/Scripts/ProjectCustom/userOnLoad.js",
                        "~/Scripts/ProjectCustom/googleMapsWizardCustomStyles.js",
                        "~/Scripts/ProjectCustom/googleMapWizardInit.js",
                        "~/Scripts/ProjectCustom/cubeportfolioInit.js",
                        "~/Scripts/ProjectCustom/magnificPopupIframeConfig.js",
                        "~/Scripts/ProjectCustom/mailFormInit.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/_6tactics/unobtrusiveValidationOverride.js"
                        ));

            // ADMIN LAYOUT - SCRIPTS BEFORE BODY
            bundles.Add(new ScriptBundle("~/Scripts/admin-scripts-before-body").Include(
                        "~/Scripts/modernizr.*",
                        "~/Scripts/jquery-{version}.js",
                        //"~/Scripts/jquery-ui-{version}.custom.js",
                        //"~/Scripts/bootstrap.js",
                        "~/Scripts/_6tactics/html5HistoryHandler.js"
                        //"~/Scripts/jquery.scrollbar.js",
                        //"~/Scripts/ProjectCustom/adminOnLoad.js"
                        ));

            // ADMIN LAYOUT - SCRIPTS AFTER BODY
            bundles.Add(new ScriptBundle("~/Scripts/admin-scripts-after-body").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        //"~/Scripts/jquery.cookie.js",
                        "~/Scripts/jquery.scrollbar.js",
                        "~/Scripts/html.sortable.min.js",
                        //"~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/magnificPopup.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/jquery.form.js",
                        //"~/Scripts/respond.js",
                        //"~/Scripts/jquery.steps.js",
                        "~/Scripts/_6tactics/diacritics-fixer.js",
                        "~/Scripts/_6tactics/treeView.js",
                        "~/Scripts/_6tactics/treeViewSortable.js",
                        "~/Scripts/_6tactics/treeViewActionTooltips.js",
                        "~/Scripts/_6tactics/duplicatedIdHandler.js",
                        "~/Scripts/_6tactics/googleMapsWizard.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerUtilities.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerPreviewComponent.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerSettings.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerUpload.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerRead.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerEdit.js",
                        "~/Scripts/_6tactics/FileManager/fileManagerRemove.js",
                        "~/Scripts/_6tactics/colorCodeConverter.js",
                        "~/Scripts/ProjectCustom/googleMapsWizardCustomStyles.js",
                        "~/Scripts/ProjectCustom/googleMapWizardInit.js",
                        "~/Scripts/ProjectCustom/fixFileUrlWidth.js",
                        "~/Scripts/ProjectCustom/treeViewSelectedItemHandler.js",
                        "~/Scripts/ProjectCustom/adminOnLoad.js",
                        "~/Scripts/ProjectCustom/jqueryStepsCustom.js",
                        "~/Scripts/ProjectCustom/magnificPopupAdminDeleteConfig.js",
                        "~/Scripts/ProjectCustom/fileManagerPopupHandler.js",
                        "~/Scripts/ProjectCustom/fileManagerInit.js",
                        "~/Scripts/ProjectCustom/mailFormInit.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/_6tactics/unobtrusiveValidationOverride.js",
                        "~/Scripts/ProjectCustom/adminFormFieldsVisibilityHandler.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.validate.unobtrusive.js"
                        //"~/Scripts/_6tactics/unobtrusiveValidationOverride.js"
                        ));


            // CKEDITOR 

            // Add before includs in HEADER
            //<script type="text/javascript">
            //var CKEDITOR_BASEPATH = "@Url.Content("~/Scripts/ckeditor/")";
            //</script>

            //bundles.Add(new ScriptBundle("~/Scripts/ckeditor").Include(
            //         "~/Scripts/ckeditor/ckeditor.js"
            //         ));

            //bundles.Add(new ScriptBundle("~/Scripts/ckeditor/adapters").Include(
            //          "~/Scripts/ckeditor/adapters/jquery.js"
            //          ));


            //bundles.Add(new ScriptBundle("~/Scripts/knockout").Include(
            //       "~/Scripts/knockout-{version}.js",
            //       "~/Scripts/knockout.mapping-latest.js"
            //         ));

            //bundles.Add(new ScriptBundle("~/Scripts/uploads").Include(
            //      "~/Scripts/lodash.js",
            //      "~/Scripts/upload/utilis.Class.js",
            //      "~/Scripts/upload/utilis.js",
            //      "~/Scripts/upload/utilis.UploadHtml5.js",
            //      "~/Scripts/upload/jquery.helpers.js",
            //      "~/Scripts/upload/jquery.upload.js"
            //        ));


#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
