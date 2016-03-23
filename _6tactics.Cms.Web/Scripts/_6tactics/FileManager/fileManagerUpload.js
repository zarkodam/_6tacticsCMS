// _6tactics file uploader jQuery plugin

; (function ($) {
    $.fn.fileManagerUpload = function (customSettings, callback) {

        // Instance
        var plugin = this;

        // Utilities
        var utilities = fileManagerUtilities;

        // Preview components
        var previewComponent = fileManagerPreviewComponent;

        // Locally calling default settings and allowing to override them through customSettings parameter
        var settings = $.extend({
            fileStatus: 'readyForUpload',
            dropAreaId: null,
            filePathInputId: null,
            newFilenameInputId: null,
            postActionPage: null,

            aspNetMvcAntiforgeryToken: null,
            isImagePreviewSupported: true,
            useThumbnailsInsteadTitles: true,

            fileSizeLimitInMb: 4, // maximum allowed on server side by default (ASP.NET) 

            // Callbacks
            fileAlreadyExistCallback: null,
            fileExtensionIsNotAllowedCallback: null,
            onUploadCompleteCallback: null,
            fileSizeLimitExceededCallback: null,
            uploadingAbortCallback: null,

            supportedExtensions: [
                ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".svg", ".ico", // images
                ".txt", ".rtf", ".csv", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".epub", // documents
                ".7z", ".zip", ".rar" // compressed
            ]

        }, customSettings);

        function filePreviewComponent(file) {
            var previewComponentData = {
                fileStatus: 'readyForUpload',
                fileNameWithPath: '',
                isFileImage: settings.isImagePreviewSupported && utilities.isFileImage(file.type),
                extension: utilities.getFileExtension(file.name),
                filenameForId: utilities.correctedFileNameWithoutExtension(file.name),
                filenameWithExtension: file.name,
                path: '',
                size: utilities.getFileSize(file.size),
                file: file,
                isEditingAllowed: settings.newFilenameInputId != null ? true : false
            };

            if (settings.filePathInputId != null)
                previewComponentData.path = $(settings.filePathInputId).val();


            if (settings.useThumbnailsInsteadTitles)
                return previewComponent.thumbnail(previewComponentData);

            return previewComponent.title(previewComponentData);
        }

        // Preview image on drop event
        function handleFiles(files) {
            $.each(files, function (i) {
                var file = files[i];

                if (!utilities.isFileSupported(file.name, settings)) {
                    if (typeof settings.fileExtensionIsNotAllowedCallback == "function")
                        settings.fileExtensionIsNotAllowedCallback();

                    console.log("File extension: " + utilities.getFileExtension(file.name).toLowerCase() + " is not supported!");
                    return;
                }

                if (!utilities.isFileSizeLimitInMbExceeded(file.size, settings)) {
                    if (typeof settings.fileSizeLimitExceededCallback == "function")
                        settings.fileSizeLimitExceededCallback();

                    console.log("File by name: " + file.name + " exceeds size limit!");
                    return;
                }

                if (utilities.isFileWithSameNameExist(file.name)) {
                    if (typeof settings.fileAlreadyExistCallback == "function")
                        settings.fileAlreadyExistCallback();

                    console.log("File: " + file.name + " already exist!");
                    return;
                }

                $(settings.dropAreaId).append(filePreviewComponent(file));
            });
        }

        function formBuilder(fileObject) {
            var formData = new FormData();

            if (settings.aspNetMvcAntiforgeryToken != null)
                formData.append("__RequestVerificationToken", utilities.getAntiForgeryToken(settings));

            if (settings.filePathInputId != null)
                formData.append("filePath", $(settings.filePathInputId).val());

            if (settings.newFilenameInputId != null)
                formData.append("newFilename", utilities.getCurrentFilename($(fileObject)));

            formData.append("file", fileObject.file);

            return formData;
        }

        function postForm(fileObject) {
            var xhr = new XMLHttpRequest();
            xhr.open("POST", settings.postActionPage);

            xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");

            var formData = formBuilder(fileObject);

            var $progress = utilities.getProgressElement($(fileObject));

            var isFileStillUploading = true;

            // Progress event (handling progress bar)
            xhr.upload.onprogress = $.proxy(function (event) {
                if (event.lengthComputable) {
                    var percentage = Math.round((event.loaded * 100) / event.total);

                    $progress.val(percentage);
                } else
                    console.log("Unable to compute progress information since the total size is unknown");
            }, this);

            xhr.onreadystatechange = function () {
                // On complete
                if (xhr.readyState === 4 && xhr.status === 200) {
                    $progress.val(100);
                    isFileStillUploading = false;

                    $(fileObject).attr('data-status', 'uploaded');
                    $(fileObject).data('filenamewithpath', $(settings.filePathInputId).val() + '/' + fileObject.file.name);
                }
            }

            // Abort uploading
            $(document).on('click', '.remove', function () {
                if (isFileStillUploading) {
                    xhr.abort();
                    console.log("Posting form with file: " + fileObject.file.name + " aborted!");

                    if (typeof settings.uploadingAbortCallback == "function")
                        settings.uploadingAbortCallback();
                }
            });

            xhr.send(formData);
        }

        // TODO: add cancel all option
        function formSender() {
            var deferred = $.Deferred();

            var isFilesUploadComplete = false;

            var fileObjects = $(document.body).find('.fileObject');

            fileObjects.each(function () {
                var fileObject = $(this)[0];
                var file = fileObject.file;
                var status = $(this).data('status');

                if (typeof file == 'undefined' || file == null || status === 'uploaded') return;

                postForm(fileObject);

                $(this).data('status', 'uploaded');
                $(this).data('filenamewithpath', $(settings.filePathInputId).val() + '/' + utilities.getCurrentFilename($(this)));

                isFilesUploadComplete = true;
            });

            if (isFilesUploadComplete) {
                deferred.resolve();
            }
            return deferred.promise();
        }

        function settingValidation() {
            var returnBool = true;

            if (settings.postActionPage == null) {
                alert("postActionPage is required!");
                returnBool = false;
            }

            if (settings.dropAreaId == null) {
                alert("dropAreaId is required!");
                returnBool = false;
            }
            return returnBool;
        }

        function domEventsInitialization() {
            // Drag and drop events
            $(settings.dropAreaId).on("dragenter", function (event) {
                event.stopPropagation();
                event.preventDefault();
            });

            $(settings.dropAreaId).on("dragover", function (event) {
                event.stopPropagation();
                event.preventDefault();
            });

            $(settings.dropAreaId).on("drop", function (event) {
                event.stopPropagation();
                event.preventDefault();
                handleFiles(event.originalEvent.dataTransfer.files);
            });
        }


        // Constructor
        return plugin.each(function () {

            domEventsInitialization();

            // Check is plugin well configured 
            if (!settingValidation())
                return;

            // Post form
            $(this).on("click", function () {
                formSender().done(function () {
                    if (typeof settings.onUploadCompleteCallback == "function")
                        settings.onUploadCompleteCallback();
                });
            });

            if (typeof callback == "function") {
                callback();
            }
        });
    };

})(jQuery);
