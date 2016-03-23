// Plugin example _6tactics plugin

; (function ($) {
    $.fn.examplePlugin = function (customSettings, callback) {

        // Instance
        var plugin = this;

        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({
            opt1: "opt1",
            opt2: "opt2",
            opt3: "opt3"
        }, customSettings);

        // Private methods
        function privateMethod() {

        }

        // Checks if element is valid
        function isValid(elemenet) {
            if (typeof elemenet === "undefined" || elemenet == null)
                return false;
            return true;
        }

        // Constructor
        return plugin.each(function () {
            // plugin code...

            if (typeof callback == "function") {
                callback();
            }
        });

    };

})(jQuery);
