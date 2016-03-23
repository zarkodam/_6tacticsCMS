// Animated scroll navigation _6tactics plugin

; (function ($) {
    $.fn.animatedScrollNavigation = function (customSettings) {

        // Instance
        var plugin = this;

        // Default settings
        var settings = $.extend({
            pluginActionType: 'classicMenu', // classicMenu, dropDownMenu

            navbarClass: '.navbar',
            sectionTitleClass: '.row-headline',
            scrollDetectorClass: '.scrollDetector',

            animationSpeed: 500,
            easingEffect: 'easeInExpo', // easing effects: http://api.jqueryui.com/easings/

            menuItemSelectingType: 'noAction', // noAction, onScroll, immediately 
            selectedMenuItemStyle: [{ 'border': '#0096db solid 1px' }, { 'background-color': 'black' }], // add multiple styles for selected menu itme
        }, customSettings);

        // Returns element tag name
        function isParentOfTagName(element, tagName) {
            if ($(element).prop('tagName').toLowerCase() == tagName)
                return true;
            return false;
        }

        // Remove all added styles from element
        function removeStyleFromSelectedElement(element) {
            $(element).find('li').each(function () {
                $(this).removeAttr('style');
            });
        }

        // Add multiple styles on element
        function addStylesToSelectedElement(element) {
            for (var i = 0; i < settings.selectedMenuItemStyle.length; i++)
                element.css(settings.selectedMenuItemStyle[i]);
        }

        // Add styles on menu element when section title element is clicked
        function setMenuElementStyleOnSectionTitleElementClick(sectionTitle) {
            removeStyleFromSelectedElement(settings.navbarClass);

            $(settings.navbarClass).find('a').each(function () {
                if ($(this).html().toLowerCase() == sectionTitle.toLowerCase())
                    addStylesToSelectedElement($($(this).parent()));
            });
        }

        // Add styles on menu element when menu element item is clicked
        function setMenuElementStyleOnMenuElementClick(element) {
            removeStyleFromSelectedElement(settings.navbarClass);
            var listItem = element.parent();

            if (isParentOfTagName(listItem, "li"))
                addStylesToSelectedElement(listItem);
        }

        // Add styles on menu element on scroll event
        function setMenuElementStyleOnMenuElementOnScroll(linkElemetFromScroll) {
            removeStyleFromSelectedElement(settings.navbarClass);

            var currentScrolledId = '#' + linkElemetFromScroll.find('a').attr('id');

            $(settings.navbarClass).find('li').each(function () {

                var elementsHrefContent = $(this).find('a').attr('href');

                if (elementsHrefContent == currentScrolledId)
                    addStylesToSelectedElement($(this));
            });
        }

        // Scroll animation handler
        function scrollElement(elementForAnimate, offsetForelement, offsetTollerance) {
            $(elementForAnimate).bind("scroll mousedown DOMMouseScroll mousewheel keyup", function () {
                $(elementForAnimate).stop();
            });

            $(elementForAnimate).animate({
                scrollTop: $(offsetForelement).offset().top - ($(settings.navbarClass).height() + offsetTollerance)
            }, settings.animationSpeed, settings.easingEffect, function () {
                $(elementForAnimate).unbind("scroll mousedown DOMMouseScroll mousewheel keyup");
            });
        }

        // Constructor
        return plugin.each(function () {

            $(settings.sectionTitleClass).css('cursor', 'pointer');

            // Animate scroll from dropdown
            $(this).on('change', function () {
                scrollElement('html, body', this.value, 10);
                return false;
            });

            // Animate scroll from a href
            $(this).find('a').on('click', function () {

                scrollElement('html, body', $.attr(this, 'href'), 10);

                if (settings.menuItemSelectingType == 'immediately')
                    setMenuElementStyleOnMenuElementClick($(this));

                return false;
            });

            // Animate scroll on section title 
            $(settings.sectionTitleClass).on('click', function () {

                scrollElement('html, body', this, 7);

                if (settings.menuItemSelectingType == 'immediately')
                    setMenuElementStyleOnSectionTitleElementClick($(this).find("a").html());

                return false;
            });


            // If menuItemSelectingType == 'onScroll' use onScroll event
            if (settings.menuItemSelectingType === 'onScroll') {


                $(window).on('scroll', function () {
                    var windowsScrollTopPosition = $(window).scrollTop() + $(settings.navbarClass).height();

                    $(settings.scrollDetectorClass).each(function () {
                        var childElement = $(this).find('a');
                        var childElementOffset = parseInt(childElement.offset().top);
                        var childElementPossition = childElementOffset - windowsScrollTopPosition;

                        if ((childElementPossition > -110 && childElementPossition < 120)) //260 //145
                            setMenuElementStyleOnMenuElementOnScroll($(this), childElement);

                    });
                });

            }
        });

    };

})(jQuery);




