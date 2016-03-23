// Tree view sortable(addon for treeView) - _6tacticsPlugin

; (function ($) {
    $.fn.treeViewSortable = function (options) {

        // Instance
        var plugin = this;

        // Defaults
        var defaults = {
            priorityIdClass: '.priority-id',
            setPriorityIdClass: '.set-priority-id'
        };

        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({}, defaults, options); //settings.opt1

        // Private methods
        function updatePriorities(element) {

            element.each(function () {
                $(this).each(function (i) {
                    var elementById = $("#" + $(this).attr("id"));
                    var elementIndex = elementById.index() + 1;
                    elementById.find(settings.priorityIdClass).html(elementIndex + ".");
                    elementById.find(settings.setPriorityIdClass).val(elementIndex);
                });
            });
        }

        // Constructor
        return plugin.each(function () {
            $(this).find('ul').addClass('sortable');
            $(this).find('li:has(ul)').addClass('ui-state-default');

            // https://github.com/voidberg/html5sortable
            $(".sortable").sortable({ forcePlaceholderSize: true }).bind('sortupdate', function (e, ui) {
                updatePriorities($(".sortable > li"));
            });
        });

    };

})(jQuery);
