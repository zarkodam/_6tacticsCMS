fileManagerUtilities = {
    isElementValid: function (element) {
        return typeof element != 'undefined' && element != null;
    },

    getFileExtension: function (filename) {
        var indexOflastDotInFilename = filename.lastIndexOf('.');
        return filename.substring(indexOflastDotInFilename);
    },

    getFileNameWithoutExtension: function (filename) {
        return filename.toLowerCase().replace(this.getFileExtension(filename).toLowerCase(), '');
    },

    correctedFileName: function (filename) {
        var withRemovedIllegalCharacters = filename.replace(/[¨!"#$%&/()=?*:,;<>~ˇ^˘°˛˙´˝¨¸'+\\|€÷×łŁß¤@{}§]/g, '');
        return withRemovedIllegalCharacters.replace(/\s+/g, '-');
    },

    correctedFileNameWithoutExtension: function (filename) {
        return this.correctedFileName(this.getFileNameWithoutExtension(filename));
    },

    getFileSize: function (fileSize) {
        var sizeUnits = new Array('Bytes', 'Kb', 'Mb', 'Gb');

        var i = 0;
        while (fileSize > 900)
            fileSize /= 1024; i++;

        return (Math.round(fileSize * 100) / 100) + ' ' + sizeUnits[i];
    },

    getFileSizeInMb: function (fileSize) {
        return (fileSize / 1024 / 1024).toFixed(1);
    },

    isFileSizeLimitInMbExceeded: function (fileSize, settings) {
        if (this.getFileSizeInMb(fileSize) <= settings.fileSizeLimitInMb)
            return true;
        return false;
    },

    isFileImage: function (fileType) {
        var imageType = /^image\//;

        if (imageType.test(fileType))
            return true;

        return false;
    },

    isFileSupported: function (filename, settings) {
        for (var i = 0; i < settings.supportedExtensions.length; i++)
            if (this.getFileExtension(filename).toUpperCase() === settings.supportedExtensions[i].toUpperCase())
                return true;

        return false;
    },

    isFileWithSameNameExist: function (fileName) {
        var uploadedFiles = document.querySelectorAll('.filename');

        //console.log("length: " + uploadedFiles.length);

        for (var i = 0; i < uploadedFiles.length; i++)
            if (fileName === uploadedFiles[i].innerHTML) {
                //console.log("fileName: " + fileName);
                //console.log("uploadedFiles[i].innerHTML: " + uploadedFiles[i].innerHTML);

                return true;
            }
        return false;
    },

    isNewFilenameInputIdNotNullAddPointer: function (settings) {
        return settings.newFilenameInputId != null ? 'cursorPointer' : '';
    },

    // Create antiforgery token (for asp.net applications)
    getAntiForgeryToken: function (settings) {
        var tokenInputInStringFromAspMvcHelper = settings.aspNetMvcAntiforgeryToken;

        var startPoint = tokenInputInStringFromAspMvcHelper.indexOf("value=\"") + 7;
        var endPoint = tokenInputInStringFromAspMvcHelper.lastIndexOf("\"");

        return tokenInputInStringFromAspMvcHelper.substring(startPoint, endPoint);
    },

    getFileWrapperElement: function ($fileObjectElement) {
        return $fileObjectElement.closest('.file-wrapper');
    },

    getCurrentFilename: function ($fileObjectElement) {
        return this.getFileWrapperElement($fileObjectElement).find('.filename').html();
    },

    setCurrentFilename: function ($fileObjectElement, newFilename) {
        return this.getFileWrapperElement($fileObjectElement).find('.filename').html(newFilename);
    },

    getProgressElement: function ($fileObjectElement) {
        return this.getFileWrapperElement($fileObjectElement).find('progress');
    },

    getFileObjectElement: function ($fileObjectElement) {
        return this.getFileWrapperElement($fileObjectElement).find('.fileObject');
    }

};
