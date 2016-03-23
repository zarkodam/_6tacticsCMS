// Tree view nod as button - _6tacticsPlugin

; (function ($) {
    $.fn.treeViewActionTooltips = function (customSettings) {

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

        });

    };

})(jQuery);
