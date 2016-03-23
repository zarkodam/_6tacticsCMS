// Check for dimension changes _6tactics plugin

; (function ($) {
    $.fn.checkForDimensionChanges = function (customSettings) {

        // Instance
        var plugin = this;

        var heightTemp = null;
        var widthTemp = null;

        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({
            actionToCall: null,
            updateInterval: 1
        }, customSettings);

        // Private methods
        function callActionOnChange() {
            if (settings.actionToCall != null) {
                settings.actionToCall();
            }
        }

        function onBehaviorChanged() {
            if (heightTemp == null || heightTemp !== $(plugin).height()) {
                heightTemp = $(plugin).height();
                callActionOnChange();
            }

            if (widthTemp == null || widthTemp !== $(plugin).width()) {
                widthTemp = $(plugin).width();
                callActionOnChange();
            }
        }

        // Constructor
        return plugin.each(function () {
            setInterval(function () {
                onBehaviorChanged();
            }, settings.updateInterval);
        });

    };

})(jQuery);
