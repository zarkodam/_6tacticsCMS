// _6tactics file editor jQuery plugin

; (function ($) {
    $.fn.fileManagerEdit = function (customSettings, target) {

        // Instance
        var plugin = this;

        // Utilities
        var utilities = fileManagerUtilities;

        // Locally calling default settings and allowing to override them through customSettings parameter
        var settings = $.extend({
            newFilenameInputId: null,
            postActionPage: null,

            aspNetMvcAntiforgeryToken: null,

            // Callbacks
            onFilenameChange: null,
            onEditCompleteCallback: null

        }, customSettings);

        function formBuilder(oldNameWithPath, newNameWithPath) {
            var formData = new FormData();

            if (settings.aspNetMvcAntiforgeryToken != null)
                formData.append('__RequestVerificationToken', utilities.getAntiForgeryToken(settings));

            if (settings.newFilenameInputId != null) {
                formData.append('oldFilenameWithPath', oldNameWithPath);
                formData.append('newFilenameWithPath', newNameWithPath);
            }

            return formData;
        }

        function postForm($fileObjectElement) {
            var oldNameWithPath = $fileObjectElement.attr('data-filenamewithpathold');
            var newNameWithPath = $fileObjectElement.data('path') + '/' + utilities.getCurrentFilename($fileObjectElement);

            // Check does file have conditions to be renamed
            if (oldNameWithPath.length < 1 || newNameWithPath.length < 1 || oldNameWithPath === newNameWithPath) return;

            var $progressElement = utilities.getProgressElement($fileObjectElement);

            // Do not upload if file progress is on 100% it means that file is already uploaded/edited
            if ($progressElement.val() === 100) return;

            var xhr = new XMLHttpRequest();
            xhr.open('POST', settings.postActionPage);

            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');

            var formData = formBuilder(oldNameWithPath, newNameWithPath);

            // Progress event (handling progress bar)
            xhr.upload.onprogress = $.proxy(function (event) {
                if (event.lengthComputable) {
                    var percentage = Math.round((event.loaded * 100) / event.total);

                    $progressElement.val(percentage);
                } else
                    console.log('Unable to compute progress information since the total size is unknown');
            }, this);

            xhr.onreadystatechange = function () {
                // On complete
                if (xhr.readyState === 4 && xhr.status === 200) {
                    $progressElement.val(100);

                    if (typeof settings.onEditCompleteCallback == 'function')
                        settings.onEditCompleteCallback();
                }
            }

            xhr.send(formData);
        }

        function checkForDuplicatedNames($fileObjects) {
            var checkedNamesCache = [];

            $fileObjects.each(function () {
                var $fileObject = $(this);
                var currentFilename = utilities.getCurrentFilename($fileObject);

                var duplicatesCounter = 2;
                checkedNamesCache.forEach(function (name) {
                    if (currentFilename === name) {
                        var newFilename = utilities.getFileNameWithoutExtension(name) + duplicatesCounter++ + utilities.getFileExtension(name);
                        utilities.setCurrentFilename($fileObject, newFilename);
                        //console.log(newFilename);
                    }
                });

                //console.log(checkedNamesCache);

                checkedNamesCache.push(currentFilename);
            });
        }

        function updateDataValuesBeforePost($fileObjectElement) {
            $fileObjectElement.attr('data-filenamewithpathold', $fileObjectElement.attr('data-filenamewithpath'));
        }

        function updateDataValuesAfterPost($fileObjectElement) {
            var currentFilename = utilities.getCurrentFilename($fileObjectElement);
            var currentPathWithFilename = $fileObjectElement.data('path') + '/' + currentFilename;

            $fileObjectElement.attr('data-filename', currentFilename);
            $fileObjectElement.attr('data-filenamewithpath', currentPathWithFilename);
        }

        function formSender(plugin) {
            var fileObjects = plugin.find('.fileObject');

            checkForDuplicatedNames(fileObjects);

            fileObjects.each(function () {
                if ($(this).data('status') === 'uploaded') {
                    updateDataValuesBeforePost($(this));
                    postForm($(this));
                }

                updateDataValuesAfterPost($(this));
            });
        }

        function settingValidation() {
            var returnBool = true;

            if (settings.postActionPage == null) {
                alert('postActionPage is required!');
                returnBool = false;
            }

            if (settings.newFilenameInputId == null) {
                alert('if you want to rename files enable newFilenameInputId has to be added!');
                returnBool = false;
            }

            return returnBool;
        }

        function domEventsInitialization() {
            // New filename update

            $(document.body).on('click', '.file-info-wrapper', function () {
                if (typeof settings.onFilenameChange == 'function')
                    settings.onFilenameChange();

                // Enable input field
                $(settings.newFilenameInputId).attr('disabled', false);

                // Find elements 
                var progressElement = $(this).find('progress');
                var filenameElement = $(this).find('.filename');

                // Grab filename from element
                var fileNameWithoutExtension = utilities.getFileNameWithoutExtension(filenameElement.html());
                // Put filename into input value
                $(settings.newFilenameInputId).val(fileNameWithoutExtension);

                // Grab data-filename from current element 
                var progressElementId = progressElement.attr('id');
                var filenameElementId = filenameElement.attr('id');

                // Memorize grabbed data-filename
                $(settings.newFilenameInputId).data('progressElementId', progressElementId);
                $(settings.newFilenameInputId).data('filenameElementId', filenameElementId);

                // Add filename to input
                $(settings.newFilenameInputId).val(fileNameWithoutExtension);

                // Select text inside input
                $(settings.newFilenameInputId).select();
            });

            $(document.body).on('keyup', settings.newFilenameInputId, function () {
                // Grab data-indexOfActiveElemet to findout which element is active
                var progressElementId = $(this).data('progressElementId');
                var filenameElementId = $(this).data('filenameElementId');

                console.log(progressElementId);
                console.log(filenameElementId);

                // Set progress to 0% on change 
                var idForCurrentProgressElement = '#' + progressElementId;
                $(idForCurrentProgressElement).val(0);

                var activeElementId = '#' + filenameElementId;

                // Active element
                var elementToEdit = $(activeElementId);

                // Get extension from active element
                var fileExtension = elementToEdit.data('fileextension');

                // Bind validated and correctd filename plus extension
                var newFileNameWithExtension = utilities.correctedFileName($(this).val()) + fileExtension;

                // Add changed filename with extension to element
                elementToEdit.html(newFileNameWithExtension);

                // Add new file name in data-newfilename tag
                elementToEdit.data('newfilename', newFileNameWithExtension);
            });
        }


        // Constructor
        return plugin.each(function () {
            if (settings.newFilenameInputId != null)
                domEventsInitialization();

            // Check is plugin well configured 
            if (!settingValidation())
                return;

            var $this = $(this);

            // Use to search for element from document.body
            if (typeof target != 'undefined' || target != null)
                // Post form
                $this.on('click', '#upload-edit-files', function () {
                    formSender($this);
                });
            else
                // Post form
                $this.on('click', function () {
                    formSender($this);
                });
        });
    };

})(jQuery);
