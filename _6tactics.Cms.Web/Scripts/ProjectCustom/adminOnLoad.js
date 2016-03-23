var contentItemsList = $('#content-items-list');
var contentItemPreview = $('#content-item-preview');

var foldersList = $('#folders-list');
var filesPreview = $('#files-preview');

var pluginList = $('#plugin-list');
var pluginPreview = $('#plugin-preview');

//var selectedTreeItemOpacity = '.4';

// Elements which has to be resized by height
function elementsToResizeHeight(windowHeight) {
    contentItemsList.css('height', windowHeight - 140);
    contentItemPreview.height(windowHeight - 140);

    foldersList.css('height', windowHeight - 140);
    filesPreview.height(windowHeight - 140);

    pluginList.css('height', windowHeight - 190);
    pluginPreview.height(windowHeight - 190);
}

// TreeView - get/set position to/from local storage
function setContentItemsTreeViewScrollTop(scrollTop) {
    localStorage.setItem("contentItemsTreeViewScrollTop", scrollTop);
}

function getContentItemsTreeViewScrollTop() {
    return typeof localStorage.getItem("contentItemsTreeViewScrollTop") === "undefined"
        ? 0
        : localStorage.getItem("contentItemsTreeViewScrollTop");
}

function setFoldersTreeViewScrollTop(scrollTop) {
    localStorage.setItem("foldersTreeViewScrollTop", scrollTop);
}

function getFoldersTreeViewScrollTop() {
    return typeof localStorage.getItem("foldersTreeViewScrollTop") === "undefined"
        ? 0
        : localStorage.getItem("foldersTreeViewScrollTop");
}

function setPluginsTreeViewScrollTop(scrollTop) {
    localStorage.setItem("pluginsTreeViewScrollTop", scrollTop);
}

function getPluginsTreeViewScrollTop() {
    return typeof localStorage.getItem("pluginsTreeViewScrollTop") === "undefined"
        ? 0
        : localStorage.getItem("pluginsTreeViewScrollTop");
}

function loadingStart() {
    $('.gears-wrapper').show();
}

function loadingStop() {
    $('.gears-wrapper').hide();
}


// On document ready
(function ($) {
    // On load
    $(window).on('load', function () {
        elementsToResizeHeight($(this).height());

        // Prevent middle click on scroll
        $('body').mousedown(function (e) {
            if (e.button === 1) return false;
            return true;
        });

        // Get scrollTop position from local storage
        var contentItemsTreeViewScrollTop = getContentItemsTreeViewScrollTop();
        var foldersTreeViewScrollTop = getFoldersTreeViewScrollTop();
        var pluginsTreeViewScrollTop = getPluginsTreeViewScrollTop();

        // Load scrollbars
        $('#content-items-list-wrapper').scrollbar({
            onScroll: function () {
                //console.log($('#root').offset().top);
                // Get scroll position
                var scrollTop = $('#content-items-list-wrapper').scrollTop();

                // Set scrollTop position to local storage
                setContentItemsTreeViewScrollTop(scrollTop);
            }
        });

        // Set scrollTop position from local storage
        $('#content-items-list-wrapper').scrollTop(contentItemsTreeViewScrollTop);

        $('#content-item-preview-wrapper').scrollbar();


        $('#folders-list-wrapper').scrollbar({
            onScroll: function () {
                // Get scroll position
                var scrollTop = $('#folders-list-wrapper').scrollTop();

                // Set scrollTop position to local storage
                setFoldersTreeViewScrollTop(scrollTop);
            }
        });
        // Set scrollTop position from local storage
        $('#folders-list-wrapper').scrollTop(foldersTreeViewScrollTop);

        $('#files-preview-wrapper').scrollbar();

        $('#plugin-list-wrapper').scrollbar({
            onScroll: function () {
                // Get scroll position
                var scrollTop = $('#plugin-list-wrapper').scrollTop();

                // Set scrollTop position to local storage
                setPluginsTreeViewScrollTop(scrollTop);
            }
        });
        // Set scrollTop position from local storage
        $('#plugin-list-wrapper').scrollTop(pluginsTreeViewScrollTop);

        $('#plugin-preview-wrapper').scrollbar();

        // Enable html5 history api for content item actions
        $('.tree-view-action').html5HistoryHandler(
        {
            targetSource: '#action-form',
            targetDestination: '#content-item-preview-wrapper',
            progressbar: '.gears-wrapper',
            callback: function () {
                fixFileUrlWidth();
            }
        });

        $('.plugin').html5HistoryHandler(
        {
            targetSource: '#action-form',
            targetDestination: '#content-item-preview-wrapper',
            progressbar: '.gears-wrapper'
        });

        treeViewSelectedItemHandler.contentItemTreeViewInit();
    });

    // On resize
    $(window).on('resize', function () {
        elementsToResizeHeight($(this).height());
        fixFileUrlWidth();
    });
})(jQuery);
