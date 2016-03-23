fileManagerPreviewComponent = {
    //isFileImage, mousePointerClass, extension, imagePath, filenameForId, filenameWithExtension, size
    settingTest: function (previewComponentData) {
        console.log("mousePointerClass: " + previewComponentData.mousePointerClass);
        console.log("extension: " + previewComponentData.extension);
        console.log("extension upper: " + previewComponentData.extension.toUpperCase());
        console.log("fileNameWithPath: " + previewComponentData.fileNameWithPath);
        console.log("filenameForId: " + previewComponentData.filenameForId);
        console.log("filenameWithExtension: " + previewComponentData.filenameWithExtension);
        console.log("size: " + previewComponentData.size);
    },

    appendFileWithImageOrExtension: function (previewHtmlElement, previewComponentData) {

        // Work with fileManagerUpload
        if (fileManagerUtilities.isElementValid(previewComponentData.file)) {
            if (previewComponentData.isFileImage) {
                previewHtmlElement.find('img')[0].file = previewComponentData.file;

                var reader = new FileReader();

                reader.onload = (function (img) {
                    return function (event) {
                        img.src = event.target.result;
                    };
                })(previewHtmlElement.find('img')[0]);

                reader.readAsDataURL(previewComponentData.file);
            } else
                previewHtmlElement.find('.extension')[0].file = previewComponentData.file;

        } else
            // Work with fileManagerEdit
            if (fileManagerUtilities.isElementValid(previewComponentData.fileNameWithPath))
                previewHtmlElement.find('img').attr('src', previewComponentData.fileNameWithPath);

        return previewHtmlElement;
    },

    title: function (previewComponentData) {
        //<div class="file-wrapper file-wrapper-title">
        //    <div class="icon-content-wrapper icon-content-wrapper-title">
        //        @*<span class="extension-text-title">.DOCX</span>*@ // if file is not image
        //        <img class="fileObject" attr="dropped-image" src="/Content/Images/back-the-booster-kickstarter.jpg"> // if file is image
        //    </div>
        //    <span class="remove remove-button-title">x</span>

        //    <div class="file-info-wrapper file-info-wrapper-title">
        //        <div id="file-name-filename" data-filename="filename..." data-fileextension=".extension" class="filename filename-wrapper-title">Filenameaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa</div>
        //        <progress id="progress-filename..." class="upload-progress-bar upload-progress-bar-title" min="0" max="100" value="100"></progress>
        //        <div class="file-size-wrapper-title">321 Mb</div>
        //    </div>
        //</div>

        var fileWrapperPointer = previewComponentData.isEditingAllowed ? '' : 'cursorPointer';
        var fileInfoWrapperPointer = previewComponentData.isEditingAllowed ? 'cursorPointer' : '';

        var generatedIdName = previewComponentData.filenameForId + previewComponentData.extension.replace('.', '-');

        var timeStamp = Date.now();

        var filenameId = 'file-name-' + generatedIdName + '-' + timeStamp;
        var progressId = 'progress-' + generatedIdName + '-' + timeStamp;

        var progress = typeof previewComponentData.progress != 'undefined' || previewComponentData.progress != null ? previewComponentData.progress : 0;

        var fileWrapper = $('<div class="file-wrapper file-wrapper-title ' + fileWrapperPointer + '">');
        var iconContentWrapper = $('<div class="icon-content-wrapper icon-content-wrapper-title"></div>');
        var extensionText = $('<span data-filename="' + previewComponentData.filenameWithExtension + '" data-path="' + previewComponentData.path + '" data-filenamewithpath="' + previewComponentData.fileNameWithPath + '" data-filenamewithpathold="' + previewComponentData.fileNameWithPath + '" data-status="' + previewComponentData.fileStatus + '" class="fileObject extension extension-title">' + previewComponentData.extension.toUpperCase() + '</span>');
        var droppedImage = $('<img data-filename="' + previewComponentData.filenameWithExtension + '" data-path="' + previewComponentData.path + '" data-filenamewithpath="' + previewComponentData.fileNameWithPath + '" data-filenamewithpathold="' + previewComponentData.fileNameWithPath + '" data-status="' + previewComponentData.fileStatus + '" class="fileObject" attr="dropped-image">');
        var removeButton = $('<span class="remove remove-button-title">x</span>');
        var fileInfo = $('<div class="file-info-wrapper file-info-wrapper-title ' + fileInfoWrapperPointer + '">');
        var filenameWrapper = $('<div id="' + filenameId + '" data-filename="' + previewComponentData.filenameForId + '" data-fileextension="' + previewComponentData.extension + '" class="filename filename-wrapper-title" title="' + previewComponentData.filenameWithExtension + '">' + previewComponentData.filenameWithExtension + '</div>');
        var uploadProgressBar = $('<progress id="' + progressId + '" class="upload-progress-bar upload-progress-bar-title" min="0" max="100" value="' + progress + '"></progress>');
        var fileSizeWrapper = $('<div class="file-size-wrapper-title">' + previewComponentData.size + '</div>');

        fileWrapper.append(iconContentWrapper);

        if (previewComponentData.isFileImage)
            iconContentWrapper.append(droppedImage);
        else
            iconContentWrapper.append(extensionText);

        if (previewComponentData.isEditingAllowed)
            fileWrapper.append(removeButton);

        fileWrapper.append(fileInfo);
        fileInfo.append(filenameWrapper);
        fileInfo.append(uploadProgressBar);
        fileInfo.append(fileSizeWrapper);

        return this.appendFileWithImageOrExtension(fileWrapper, previewComponentData);
    },

    thumbnail: function (previewComponentData) {
        //<div class="file-wrapper file-wrapper-thumbnail">
        //    <span class="remove remove-button-thumbnail">x</span>
        //    <div class="icon-content-wrapper icon-content-wrapper-thumbnail">
        //        <span class="fileObject upload-element extension-text-thumbnail">.DOCX</span> // if file is not image
        //        <img class="fileObject upload-element" attr="dropped-image" src="/Content/Images/kocka.png"> // if file is image
        //    </div>
        //    <div class="file-info-wrapper file-info-wrapper-thumbnail">
        //        <div class="file-info-thumbnail">
        //            <progress id="progress-filename..." class="upload-progress-bar upload-progress-bar-thumbnail" min="0" max="100" value="0"></progress>
        //            <div id="file-name-filename" data-filename="filename..." data-fileextension=".extension" class="filename filename-wrapper-thumbnail">Filenameaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa</div>
        //            <div class="file-size-wrapper-thumbnail">321 Mb</div>
        //        </div>
        //    </div>
        //</div>

        //this.settingTest(previewComponentData);

        var fileWrapperPointer = previewComponentData.isEditingAllowed ? '' : 'cursorPointer';
        var fileInfoWrapperPointer = previewComponentData.isEditingAllowed ? 'cursorPointer' : '';

        var generatedIdName = previewComponentData.filenameForId + previewComponentData.extension.replace('.', '-');

        var timeStamp = Date.now();

        var progressId = 'progress-' + generatedIdName + '-' + timeStamp;
        var filenameId = 'file-name-' + generatedIdName + '-' + timeStamp;

        var progress = typeof previewComponentData.progress != 'undefined' || previewComponentData.progress != null ? previewComponentData.progress : 0;

        var fileWrapper = $('<div class="file-wrapper file-wrapper-thumbnail ' + fileWrapperPointer + '">');
        var iconContentWrapper = $('<div class="icon-content-wrapper-thumbnail"></div>');
        var removeButton = $('<span class="remove remove-button-thumbnail">x</span>');
        var extensionText = $('<span data-filename="' + previewComponentData.filenameWithExtension + '" data-path="' + previewComponentData.path + '" data-filenamewithpath="' + previewComponentData.fileNameWithPath + '" data-filenamewithpathold="' + previewComponentData.fileNameWithPath + '" data-status="' + previewComponentData.fileStatus + '" class="fileObject extension extension-thumbnail">' + previewComponentData.extension.toUpperCase() + '</span>');
        var droppedImage = $('<img data-filename="' + previewComponentData.filenameWithExtension + '" data-path="' + previewComponentData.path + '" data-filenamewithpath="' + previewComponentData.fileNameWithPath + '" data-filenamewithpathold="' + previewComponentData.fileNameWithPath + '" data-status="' + previewComponentData.fileStatus + '" class="fileObject" attr="dropped-image">');
        var fileInfoWrapper = $('<div class="file-info-wrapper file-info-wrapper-thumbnail ' + fileInfoWrapperPointer + '">');
        var filenameInfo = $('<div class="file-info-thumbnail"></div>');
        var uploadProgressBar = $('<progress id="' + progressId + '" class="upload-progress-bar upload-progress-bar-thumbnail" min="0" max="100" value="' + progress + '"></progress>');
        var filenameWrapper = $('<div id="' + filenameId + '" data-filename="' + previewComponentData.filenameForId + '" data-fileextension="' + previewComponentData.extension + '" class="filename filename-wrapper-thumbnail" title="' + previewComponentData.filenameWithExtension + '">' + previewComponentData.filenameWithExtension + '</div>');
        var fileSizeWrapper = $('<div class="file-size-wrapper-thumbnail">' + previewComponentData.size + '</div>');

        fileWrapper.append(iconContentWrapper);

        if (previewComponentData.isEditingAllowed)
            iconContentWrapper.append(removeButton);

        if (previewComponentData.isFileImage)
            iconContentWrapper.append(droppedImage);
        else
            iconContentWrapper.append(extensionText);

        fileWrapper.append(fileInfoWrapper);
        fileInfoWrapper.append(filenameInfo);
        filenameInfo.append(uploadProgressBar);
        filenameInfo.append(filenameWrapper);
        filenameInfo.append(fileSizeWrapper);

        return this.appendFileWithImageOrExtension(fileWrapper, previewComponentData);
    }
}