// LimitDimensionsOnResponsive _6tactics plugin

; (function ($) {
    $.fn.limitDimensionsOnResponsive = function (customSettings) {

        // Instance
        var plugin = this;
        var rootElementTempWidth = null;

        // Defaults
        var settings = $.extend({
            rootElement: null,
            responsiveWrapper: null,
            classNameForRemoveToStopResponsivness: null,
            elementWrapper: null,
            elementMaxHeightelementMinHeight: null,
            elementMaxHeight: null
        }, customSettings);

        function checkIsRequiredPropertiesSet() {
            if (settings.rootElement === null)
                alert("rootElement property is required!");
            if (settings.responsiveWrapper === null)
                alert("responsiveWrapper property is required!");
            if (settings.elementWrapper === null)
                alert("elementWrapper property is required!");
            if (settings.elementMinHeight === null)
                alert("elementMinHeight property is required!");
            if (settings.elementMaxHeight === null)
                alert("elementMaxHeight property is required!");
            if (settings.classNameForRemoveToStopResponsivness === null)
                alert("classToRemoveToStopResponsivness property is required!");
        }

        function stopElementResizeing(pluginsAttachedElement) {

            var heightTollerance = 40;
            var rootElementHeight = $(settings.rootElement).height() - heightTollerance;

            if (pluginsAttachedElement.height() > settings.elementMaxHeight) {
            //if (rootElementTempWidth == null && pluginsAttachedElement.height() > rootElementHeight) {
                console.log("");
                console.log("STOP, TOO BIG!");

                var elementwidth = pluginsAttachedElement.width();
                var elementHeight = pluginsAttachedElement.height();

                $(settings.elementWrapper).removeClass(settings.classNameForRemoveToStopResponsivness);

                pluginsAttachedElement.css('width', elementwidth - 20);
                pluginsAttachedElement.css('height', elementHeight - 20);

                if (rootElementTempWidth == null)
                    rootElementTempWidth = $(settings.rootElement).width();

                console.log('rootElementTempWidth: ' + rootElementTempWidth);
                console.log('$(settings.rootElement).width(): ' + $(settings.rootElement).width());

            }

            //if (rootElementTempWidth != null && rootElementTempWidth < $(settings.rootElement).width()) {
            //    console.log("");
            //    console.log("STOP, TOO SMALL!");

            //    rootElementTempWidth = null;

            //    $(settings.elementWrapper).addClass(settings.classNameForRemoveToStopResponsivness);
            //    pluginsAttachedElement.removeAttr('style');
            //}

            console.log('rootElementTempWidth: ' + rootElementTempWidth);
            console.log('$(settings.rootElement).width(): ' + $(settings.rootElement).width());

        }

        // Constructor
        return plugin.each(function () {
            checkIsRequiredPropertiesSet();

            var pluginsElement = $(this);

            // Limit element size on load
            stopElementResizeing(pluginsElement);

            // Limit element size on resize
            $(window).on("resize", function () {
                stopElementResizeing(pluginsElement);
            });

        });

    };

})(jQuery);


// Example: 

// Limit element size on responsive
//$('.embed-responsive-item').limitDimensionsOnResponsive(
//    {
//        responsiveWrapper: '.elements-main-wrapper',
//        elementWrapper: '.element-first-wrapper',
//        classToRemoveToStopResponsivness: '.embed-responsive',
//        elementMinHeight: 120,
//        elementMaxHeight: 320
//    });