// Tree view - _6tacticsPlugin

; (function ($) {
    $.fn.treeView = function (options) {

        // Instance
        var plugin = this;

        // Defaults
        var defaults = {
            expandCollapseClass: '.expand-collapse-icon',
            childContentElementClass: '.child-content-element',

            expandIconClassName: 'glyphicon-plus-sign',
            collapseIconClassName: 'glyphicon-minus-sign',

            animationDuration: 'fast',

            expandMessage: 'expand this branch',
            collapseMessage: 'collapse this branch',
            expandCollapseCallback: null
        };

        // Locally calling default settings and allowing to override them with options
        var settings = $.extend({}, defaults, options);

        // Private methods
        function expandCollapseCallback() {
            if (settings.expandCollapseCallback != null)
                settings.expandCollapseCallback();
        }

        function expandCollape(element) {
            element.on('click', function () {
                var children = $(this).parent().parent('li.parent_li').find(' > ul > li');

                if (children.is(":visible")) {
                    children.hide(settings.animationDuration, function () { expandCollapseCallback(); });
                    $(this).find(' > i').attr('title', settings.expandMessage).addClass(settings.expandIconClassName).removeClass(settings.collapseIconClassName);

                } else {
                    children.show(settings.animationDuration, function () { expandCollapseCallback(); });
                    $(this).find(' > i').attr('title', settings.collapseMessage).addClass(settings.collapseIconClassName).removeClass(settings.expandIconClassName);
                }

                
            });
        }

        function collapseChildrens() {
            $(settings.childContentElementClass).parent().parent().find('i').attr('title', settings.expandMessage).addClass(settings.expandIconClassName).removeClass(settings.collapseIconClassName);
            $(settings.childContentElementClass).hide();
        }

        // Constructor
        return plugin.each(function () {
            plugin.find("li:has(ul)").addClass('parent_li');

            //collapseChildrens();

            expandCollape($(settings.expandCollapseClass));
        });
        
    };

})(jQuery);
