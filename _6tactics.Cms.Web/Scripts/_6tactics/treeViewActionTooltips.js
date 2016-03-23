// Tree view content expanding - _6tacticsPlugin

; (function ($) {
    $.fn.treeViewActionTooltips = function (customSettings, callback) {

        // Instance
        var plugin = this;

        // Defaults
        var settings = $.extend({
            opt: ''
        }, customSettings);

        function bootstrapTooltips() {
            $(settings.actionIconsClass).on('click', function (e) {
                e.stopPropagation();
            });

            $('.create').tooltip({ placement: 'left' });
            $('.edit').tooltip({ placement: 'left' });
            $('.details').tooltip({ placement: 'left' });
            $('.delete').tooltip({ placement: 'left' });
        }

        // Constructor
        return plugin.each(function () {
            bootstrapTooltips();

            if (typeof callback == "function")
                callback();
        });

    };

})(jQuery);
