// Fix file url width
function fixFileUrlWidth(fileUrl, inputId) {
    var tollerance = 48;
    var isFileUrlExist = typeof fileUrl !== 'undefined';
    var isInputIdExist = typeof inputId !== 'undefined';

    $('#file-url-panel, .file-url-panel').each(function () {
        var filePanelUrl = $(this);

        var previewImageWrapper = $(this).find('.prview-image-wrapper');
        var imageLink = $(this).find('.image-link');

        var fileUrlInputId = '#' + $(this).find('.file-url-input').attr('id');

        var fileUrlButton = $(this).find('.file-url-button');
    
        var validFileUrl = isFileUrlExist ? fileUrl : $(fileUrlInputId).val();

        if (isInputIdExist && (inputId !== fileUrlInputId))
            return true;

        var img = new Image();
        img.src = validFileUrl;

        img.onload = function () {
            var imageWidth = 0;
            var maxWidth = 200;
            var imageHeight = 40;
            var imageMarginRight = 4;
            var imageWidthWithMargin = 0;

            // Calculate image width
            imageWidth = img.width / (img.height / imageHeight);

            // Check for width limit
            if (imageWidth > maxWidth)
                imageWidth = maxWidth;

            imageWidthWithMargin = Math.round(imageWidth + imageMarginRight);

            // For create form - when is fileUrl added manualy
            if (isFileUrlExist) {
                // Remove display none from prview-image-wrapper element
                previewImageWrapper.show();

                // Add new fileUrl to image link for popup
                imageLink.attr('href', validFileUrl);

                // Add class to image element
                this.className = 'preview-for-uploaded-image';
                imageLink.html(this);

                // Reinitialize popup
                imageLink.magnificPopup({ type: 'image' });
            }

            // Calculate input field with image size included
            $(fileUrlInputId).width(filePanelUrl.width() - (fileUrlButton.width() + tollerance + imageWidthWithMargin));
        }
        img.onerror = function () {
            // Remove image element if file is not image type
            if (imageLink.find('img').length > 0)
                imageLink.html('');
            // If file by path is not image or if there's no path at all
            $(fileUrlInputId).width(filePanelUrl.width() - (fileUrlButton.width() + tollerance));
        }
    });
}