// Selected page panel _6tactics plugin

; (function ($) {
    $.fn.selectedPagePanel = function (customSettings) {

        // Instance
        var plugin = this;

        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({
            elementsBeforePanel: ['.navbar'],
            onWhichSizeHideTitle: 860,
            selectedPage: '.selected-page',
            selectedPagePath: '.selected-page-path'
        }, customSettings);

        // Private methods
        function elementHeightCounter(element) {
            var elementHeight = element.height();
            var elementBorderTopWidth = parseInt(element.css('border-top-width'));
            var elementBorderBottomWidth = parseInt(element.css('border-bottom-width'));
            var elementMarginTop = parseInt(element.css('margin-top'));
            var elementMarginBottom = parseInt(element.css('margin-bottom'));
            var paddingTop = parseInt(element.css('padding-top'));
            var paddingBottom = parseInt(element.css('padding-bottom'));

            return elementHeight + elementBorderTopWidth + elementBorderBottomWidth + elementMarginTop +
                   elementMarginBottom + paddingTop + paddingBottom;
        }

        function elementsBeforePanelHeightCounter() {
            var topFixedElementsHeightCount = 0;

            for (var i = 0; i < settings.elementsBeforePanel.length; i++)
                topFixedElementsHeightCount += elementHeightCounter($(settings.elementsBeforePanel[i]));

            return topFixedElementsHeightCount;
        }

        function showHidePagePath() {
            if ($(window).width() < settings.onWhichSizeHideTitle) {
                $(settings.selectedPage).hide();
                $(settings.selectedPagePath).css('font-size', '0.8em');
                $(settings.selectedPagePath).css('float', 'left');
            } else {
                $(settings.selectedPage).show();
                $(settings.selectedPagePath).css('font-size', '0.9em');
                $(settings.selectedPagePath).css('float', 'right');
            }
        }

        function applyChangesToPanel() {
            setTimeout(function () {
                showHidePagePath();
                plugin.find('.wrapper').css('top', elementsBeforePanelHeightCounter());
            }, 1);
        }

        // Constructor
        return plugin.each(function () {
            applyChangesToPanel();

            $(window).on("resize", function () {
                applyChangesToPanel();
            });

        });

    };

})(jQuery);
