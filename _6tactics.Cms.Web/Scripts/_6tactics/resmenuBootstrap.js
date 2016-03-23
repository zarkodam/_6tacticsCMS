// ReSmenu plugin by Alessandro Benoit updated by _6tactics 

; (function ($) {
    $.fn.resmenuBootstrap = function (customSettings) {

        // Instance
        var plugin = this;

        // Default settings
        var settings = $.extend({
            logoHeaderWrapperClass: '.header-logo-wrapper',
            menuHeaderWrapperClass: '.header-menu-wrapper',
            containerWidth: 1024
        }, customSettings);


        // Checks if element is valid
        function isValid(elemenet) {
            if (typeof elemenet === "undefined" || elemenet == null)
                return false;
            return true;
        }

        // The responsive menu is built if the page size is or goes under maxWidth
        function setMenuWrapperSize() {

            var logoHeaderWrapperWidth = $(settings.logoHeaderWrapperClass).width();
            var menuHeaderWrapperWidth = $(settings.menuHeaderWrapperClass).width();

            var navigationWidth = logoHeaderWrapperWidth + menuHeaderWrapperWidth;
            var navigationWrapperWidth = $(settings.menuHeaderWrapperClass).parent().width();

            if (navigationWrapperWidth <= navigationWidth) {
                $(settings.menuHeaderWrapperClass).css('width', '100%');
                $(plugin).parent().css('width', '100%');
            }
            //else
            //    $(defaultSettings.menuHeaderWrapperClass).css('width', '100%');

        }

        // Constructor
        return plugin.each(function () {

            setMenuWrapperSize();

            $(window).on("resize", function () {
                setMenuWrapperSize();
            });
        });

    };

})(jQuery);
