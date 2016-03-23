// Layout fixxxer _6tactics plugin

; (function ($) {
    $.fn.layoutFixxxer = function (customSettings) {

        // Instance
        var plugin = this;

        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({
            elementsAroundTheBody: [''],
            navbarElement: '.navbar',
            timeOutInMs: 10
        }, customSettings);


        // Private methods
        function elementHeightCounter(element) {
            var elementHeight = element.height();
            var elementBorderTopWidth = parseInt(element.css('border-top-width'));
            var elementBorderBottomWidth = parseInt(element.css('border-bottom-width'));
            var elementMarginTop = parseInt(element.css('margin-top'));
            var elementMarginBottom = parseInt(element.css('margin-bottom'));

            elementHeight += elementBorderTopWidth + elementBorderBottomWidth + elementMarginTop + elementMarginBottom;

            //console.log('border-top-width: ' + elementBorderTopWidth);
            //console.log('border-bottom-width: ' + elementBorderBottomWidth);

            //console.log('margin-top: ' + elementMarginTop);
            //console.log('margin-bottom: ' + elementMarginBottom);

            return elementHeight;
        }

        function topFixedElementsHeightCounter() {
            var topFixedElementsHeightCount = 0;

            for (var i = 0; i < settings.elementsAroundTheBody.length; i++)
                topFixedElementsHeightCount += elementHeightCounter($(settings.elementsAroundTheBody[i]));

            return topFixedElementsHeightCount;
        }

        

        function bodyPaddingAndHeightCalculator() {
            setTimeout(function() {
                $('body').css('padding-top', elementHeightCounter($(settings.navbarElement)));
                $(plugin).css('min-height', $(window).height() - topFixedElementsHeightCounter());

                //console.log(elementHeightCounter($(settings.navbarElement)));
                //console.log(topFixedElementsHeightCounter());
            }, settings.timeOutInMs);
        }

        // Constructor
        return plugin.each(function () {
            bodyPaddingAndHeightCalculator();

            $(window).on('resize', function () {
                bodyPaddingAndHeightCalculator();
            });
        });

    };

})(jQuery);


// Attach to body wrapper