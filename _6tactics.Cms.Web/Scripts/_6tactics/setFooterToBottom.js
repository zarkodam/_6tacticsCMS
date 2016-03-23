// SetFooterToBottom _6tactics plugin

; (function ($) {
    $.fn.setFooterToBottom = function (customSettings) {

        // Instance
        var plugin = this;

        // Default settings
        var settings = $.extend({
            bodyWrapperClass: '.body-content', // bootstrap default
            defaultMinHeight: 0,
            footerMarginTopTollerance: 0,
            elementsForStartWithDelay: [''],
            startTimeOut: 0
        }, customSettings);

        // Private 
        function checkIsElementForStartWithDelayOnPage() {
            for (var i = 0; i < settings.elementsForStartWithDelay.length; i++) {
                if ($('body').find(settings.elementsForStartWithDelay[i]).length > 0)
                    return true;
            }
            return false;
        }

        function setFooterPosition() {
            var windowHeightCounter = ($(window).height() - 100) <= settings.defaultMinHeight ? settings.defaultMinHeight : $(window).height() - 100;
            var bodyAndFooterCount = $(settings.bodyWrapperClass).height() + $(plugin).height();
            var footerMarginTopCalc = Math.floor(windowHeightCounter - bodyAndFooterCount);

            //console.log("windowHeightCounter: " + windowHeightCounter);
            //console.log("bodyAndFooterCount: " + bodyAndFooterCount);
            //console.log("footerMarginTopCalc: " + footerMarginTopCalc);

            $('body').css('overflow', 'auto');
            $('body').css('min-height', windowHeightCounter);

            if (footerMarginTopCalc > 0)
                $(plugin).css("margin-top", footerMarginTopCalc - settings.footerMarginTopTollerance);
            else
                $(plugin).css("margin-top", '0');
        }

        function setFooterPositionDelayHandler() {
            $('body').css('overflow', 'hidden');
            $(plugin).css('margin-top', '8000px');

            checkIsElementForStartWithDelayOnPage();
            if (checkIsElementForStartWithDelayOnPage()) {
                setTimeout(function() { setFooterPosition() }, settings.startTimeOut);
            } else {
                setFooterPosition();
            }
        }


        // Constructor
        return plugin.each(function () {
            $(function () {
                setFooterPositionDelayHandler();
            });

            $(window).on('resize', function () {
                setFooterPositionDelayHandler();
            });

        });

    };

})(jQuery);
