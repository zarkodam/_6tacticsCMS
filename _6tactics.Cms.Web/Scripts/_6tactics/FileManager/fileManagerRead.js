// _6tactics file editor jQuery plugin

; (function ($) {
    $.fn.fileManagerRead = function (customSettings, callback) {

        // Instance
        var plugin = this;

        // Utilities
        var utilities = fileManagerUtilities;

        // Preview components
        var previewComponent = fileManagerPreviewComponent;

        // Locally calling default settings and allowing to override them through customSettings parameter
        var settings = $.extend({
            urlToLoad: null,
            previewAreaId: null,
            isImagePreviewSupported: true,
            useThumbnailsInsteadTitles: true,
            isEditingAllowed: true
        }, customSettings);

        function filePreviewComponent(file) {
            var previewComponentData = {
                fileStatus: settings.fileStatus,
                progress: 100,
                fileNameWithPath: file.filenamewithpath,
                isFileImage: settings.isImagePreviewSupported && utilities.isFileImage(file.type),
                extension: file.extension.toLowerCase(),
                filenameForId: utilities.correctedFileNameWithoutExtension(file.name),
                filenameWithExtension: file.name,
                path: file.path,
                size: utilities.getFileSize(file.size),
                isEditingAllowed: settings.isEditingAllowed
            };

            if (settings.useThumbnailsInsteadTitles)
                return previewComponent.thumbnail(previewComponentData);

            return previewComponent.title(previewComponentData);
        }

        // Preview image on drop event
        function handleFiles(files) {
            // Remove previously loaded files
            $(settings.previewAreaId).html('');

            $.each(files, function (i) {
                var file = files[i];
                $(settings.previewAreaId).append(filePreviewComponent(file));
            });
        }

        function settingValidation() {
            var returnBool = true;

            if (settings.urlToLoad == null) {
                alert('urlToLoad is required!');
                returnBool = false;
            }

            if (settings.previewAreaId == null) {
                alert('previewAreaId is required!');
                returnBool = false;
            }

            return returnBool;
        }

        // Constructor
        return plugin.each(function () {

            // Check is plugin well configured 
            if (!settingValidation())
                return;

            if (settings.urlToLoad != null)
              $.getJSON(settings.urlToLoad).done(function (files) {
                    handleFiles(files);

                    if (typeof callback == 'function') {
                        callback();
                    }
                });
        });
    };

})(jQuery);
