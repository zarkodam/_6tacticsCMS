// ReSmenu plugin by Alessandro Benoit upgraded by _6tactics 

; (function ($, window, dropDownElementsCounter) {
    $.fn.resmenu = function (customSettings) {

        var plugin = this;

        // Default settings
        var settings = $.extend({
            menuContainerClass: 'menus_wrapper',
            dropDownMenuWrapperClass: 'dropdown_menu-wrapper', // Responsive menu class
            dropDownMenuId: 'dropdown-menu', // Dropdown id
            selectedItemClass: 'selected-menu-item', // Active menu li class

            submenuClass: 'submenu', // Submenu
            sumbenuLeftArrowClass: 'left-arrow',
            sumbenuRightArrowClass: 'right-arrow',

            maxWidth: 480, // Size to which the menu is responsive
            menuActualWidthTolerance: 0, // Tollerance of main menu full width

            textBefore: false, // Text to add before the mobile menu
            selectOption: false, // First select option
            fluidMenu: false
        }, customSettings);

        // Checks if element is valid
        function isValid(elemenet) {
            if (typeof elemenet === "undefined" || elemenet == null)
                return false;
            return true;
        }

        // Convert the menu to select
        function createDropDownOptions(listElement, selectElement, optionItemPrefix) {

            // If add is undfefined add empty string
            if (!isValid(optionItemPrefix))
                optionItemPrefix = '';

            // Create parent items 
            $(listElement).children('li').each(function () {
                var url = $(this).children('a').attr('href');

                // If url is undfefined add empty string
                if (!isValid(url))
                    url = '';

                // Get path of current selected page
                var currentSelectedPage = window.location.pathname;

                // Check is current page equals to url
                var isCurrentSelectedPageEqualsToUrl = url === currentSelectedPage;

                // Create option item
                $('<option/>', {
                    value: url,
                    html: optionItemPrefix + $(this).children('a').text(),
                    disabled: !isValid(url) ? true : false,
                    selected: ($(this).hasClass(settings.selectedItemClass) && !settings.selectOption) || isCurrentSelectedPageEqualsToUrl ? true : false
                }).appendTo(selectElement);

                // If listElement contains childrens 
                if ($(this).children('ul').length > 0)
                    // Add recursiverly children items
                    createDropDownOptions($(this).children('ul'), selectElement, optionItemPrefix + "&nbsp;&nbsp;&nbsp;");
            });
        }

        // Create dropdown wrapper, select element, label
        function createDropdown(listElement, dropDownCounter) {
            // Create dropdown wrapper and select item
            var responsiveMenu = $('<div/>', { 'class': settings.dropDownMenuWrapperClass }).appendTo($(listElement).parent()),
                select = $('<select/>', { id: settings.dropDownMenuId + dropDownCounter }).appendTo(responsiveMenu);

            // Bind change to select
            $(select).bind('change', function () {
                var optionValue = $(this).val();
                if (optionValue.length > 0 && optionValue.search('#') != 0) {
                    window.location.href = optionValue;
                }
            });

            // Create label
            if (settings.textBefore)
                $('<label/>', { html: settings.textBefore, 'for': settings.dropDownMenuId + dropDownCounter }).prependTo(responsiveMenu);

            // Create first option
            if (settings.selectOption)
                $('<option/>', { text: settings.selectOption, value: '' }).appendTo(select);

            // Create options for dropDown
            createDropDownOptions($(listElement), select);

            return responsiveMenu;
        }

        // Calculating list actual width elementh width
        function getListMenuActualWidth(listElement) {
            var actualWidth = settings.menuActualWidthTolerance;
            $(listElement).children('li').each(function () {
                actualWidth += $(this).width();

                var icon = $(this).find('i');
                actualWidth += isValid(icon) ? 1 : 0;
            });

            return actualWidth;
        }

        // The responsive menu is built if the page size is or goes under maxWidth
        function showHideMenu(listMenu, dropDownMenu, menuWrapperWidth) {

            var currentWindowWidth = $(window).width();
            var listMenuMaxWidth = settings.maxWidth;
            var wrapperOfMenuWrapper = $('.' + settings.menuContainerClass).parent();
            var menuWrapper = $('.' + settings.menuContainerClass);
            menuWrapper.width(menuWrapperWidth);

            if ((isValid(wrapperOfMenuWrapper) && menuWrapper.width() > wrapperOfMenuWrapper.width())
                || listMenuMaxWidth > currentWindowWidth) {
                $(listMenu).hide();
                $(dropDownMenu).show();
                menuWrapper.width('100%');
            }
            else {
                $(listMenu).show();
                $(dropDownMenu).hide();
            }
        }

        // 
        function subMenuOpeningSide() {
            var submenu = $('.' + settings.submenuClass);
            var windowWidth = $(window).width();

            if (isValid(submenu)) {
                submenu.each(function () {
                    var submenuParentListItem = $(this).parent();
                    var subemnuOffset = Math.round(submenuParentListItem.offset().left);
                    var sumbemuWidth = $(this).width();

                    var leftArrow = $('.' + settings.sumbenuLeftArrowClass);
                    var rightArrow = $('.' + settings.sumbenuRightArrowClass);

                    // Open submenu on left side
                    if ((subemnuOffset + sumbemuWidth * 2) > windowWidth) {
                        $(this).removeAttr('style');
                        $(this).css('right', '100%').css('z-index', '1');

                        submenuParentListItem.css('text-align', 'right');
                        submenuParentListItem.find(leftArrow).css('display', 'inline-block');
                        submenuParentListItem.find(rightArrow).css('display', 'none');

                        //console.log('open submenu on left side');
                    }

                    // Open submenu on right side
                    else {
                        $(this).removeAttr('style');
                        $(this).css('left', '100%').css('z-index', '1');

                        submenuParentListItem.css('text-align', 'left');
                        submenuParentListItem.find(leftArrow).css('display', 'none');
                        submenuParentListItem.find(rightArrow).css('display', 'inline-block');

                        //console.log('open submenu on right side');
                    }
                });
            }
        }

        // Constructor
        return plugin.each(function () {
            var listMenu = $(this);

            // Decides on which side to open subemnu
            subMenuOpeningSide();

            var dropDownMenu = createDropdown(listMenu, dropDownElementsCounter += 1);
            var menuWrapperWidth = !settings.fluidMenu ? getListMenuActualWidth(listMenu) : '100%';

            // Initialize list menu or dropDown menu 
            showHideMenu(listMenu, dropDownMenu, menuWrapperWidth);

            $(window).on("resize", function () {
                // Reinitialize list menu or dropDown menu on window resize
                showHideMenu(listMenu, dropDownMenu, menuWrapperWidth);
            });

            // Reload subMenuOpeningSide (on which side menu will open)
            $(this).find('li').on('mouseover', function () {
                subMenuOpeningSide();
            });
        });

    };
})(jQuery, window, 0);
