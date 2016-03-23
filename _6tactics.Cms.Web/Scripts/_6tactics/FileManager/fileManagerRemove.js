// _6tactics file editor jQuery plugin

; (function ($) {
    $.fn.fileManagerRemove = function (customSettings, callback) {

        // Instance
        var plugin = this;

        // Utilities
        var utilities = fileManagerUtilities;

        // Locally calling default settings and allowing to override them through customSettings parameter
        var settings = $.extend({
            postActionPage: null,
            aspNetMvcAntiforgeryToken: null,
            removeConfirmationCallback: null,
            removeDoneCallback: null
        }, customSettings);

        function formBuilder($fileObjectElement) {
            var formData = new FormData();

            if (settings.aspNetMvcAntiforgeryToken != null)
                formData.append('__RequestVerificationToken', utilities.getAntiForgeryToken(settings));

            formData.append('filenameWithPath', utilities.getFileObjectElement($fileObjectElement).attr('data-filenamewithpath'));

            return formData;
        }

        function postForm($fileObjectElement) {
            // If is progress is not on 100% then remove just html element
            if (utilities.getProgressElement($fileObjectElement).val() === 100) {
                var xhr = new XMLHttpRequest();
                xhr.open('POST', settings.postActionPage);

                xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');

                var formData = formBuilder($fileObjectElement);

                xhr.onreadystatechange = function () {
                    // On complete
                    if (xhr.readyState === 4 && xhr.status === 200) {

                        if (typeof settings.onUploadCompleteCallback == 'function')
                            settings.onUploadCompleteCallback();
                    }
                }

                xhr.send(formData);
            }
        }

        // Constructor
        return plugin.each(function () {
            $(this).on('click', '.remove', function () {
                var $this = $(this);

                if (typeof settings.removeConfirmationCallback == 'function') {
                    settings.removeConfirmationCallback().done(function () {
                        // Send data to the server
                        postForm($this);
                        // Remove HTML element
                        utilities.getFileWrapperElement($this).remove();

                        if (typeof settings.removeDoneCallback == 'function')
                            settings.removeDoneCallback();

                    }).fail(function () {
                        // on fail...
                    });
                }
            });

            if (typeof callback == 'function') {
                callback();
            }
        });
    };

})(jQuery);
