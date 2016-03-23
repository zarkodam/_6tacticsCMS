treeViewSelectedItemHandler = {
    setSelectedItemToLocalStorage: function (itemName, toSave) {
        localStorage.setItem(itemName, toSave);
    },

    getSelectedItemFromLocalStorage: function (itemName) {
        return typeof localStorage.getItem(itemName) === "undefined"
            ? null
            : localStorage.getItem(itemName);
    },

    contentItemTreeViewOnCreateAndEdit: function () {
        // Get selected tree view content items element
        var selectedContentItem = treeViewSelectedItemHandler.getSelectedItemFromLocalStorage('selectedContentItem');
        if (selectedContentItem != null) {
            var contentItemElement = $('#' + selectedContentItem);
            contentItemElement.css('opacity', '.4');
        }
    },

    contentItemTreeViewInit: function () {
        $('.tree-view-action').on('click', function () {
            $('.tree-node').css('opacity', '1');
            var treeNode = $(this).parent().parent();
            treeNode.css('opacity', '.4');
            treeViewSelectedItemHandler.setSelectedItemToLocalStorage('selectedContentItem', treeNode.attr('id'));
        });
    },

    selectItemInFolderAndPluginTree: function (element) {
        // Restore all
        var borderNormal = '1px solid #808080';
        $('.tree-node').css('background-color', '#808080').css('border-right', borderNormal).css('border-top', borderNormal);

        // Select current
        var borderSelected = '1px solid #6b6a6a';
        element.css('background-color', '#6b6a6a').css('border-right', borderSelected).css('border-top', borderSelected);
    },

    folderTreeViewInit: function () {
        // Select on click
        $('.folder-tree-node').on('click', function () {
            treeViewSelectedItemHandler.selectItemInFolderAndPluginTree($(this));
        });

        // Select by selected path
        $('.folder-path').each(function () {
            if ($(this).html() === $('#current-location-path').html())
                treeViewSelectedItemHandler.selectItemInFolderAndPluginTree($(this).closest('.folder-tree-node'));
        });
    }
}