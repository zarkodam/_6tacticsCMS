// FileManager popup handler _6tactics plugin

; (function ($) {
    $.fn.fileManagerPopupHandler = function (customSettings, callback) {

        // Instance
        var plugin = this;

        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({
            actionType: null,
            isQueryOnLinkEnabled: false,
            popupId: null,
            popupContentClass: ".popup-content",
            classToRemove: '',
            classToAdd: '',
            elementIdPrefix: '',
            urlToLoad: null,
            onLoadStartCallback: null,
            onLoadCompleteCallback: null
        }, customSettings);

        function settingValidation() {
            var returnBool = true;

            if (settings.popupId == null) {
                alert("popupId is required!");
                returnBool = false;
            }

            return returnBool;
        }

        function loadContentToPopup($popup, $this) {
            var deferred = $.Deferred();
            if (settings.urlToLoad != null) {

                var url = settings.urlToLoad;

                if (settings.isQueryOnLinkEnabled) {
                    var path = $this.attr('id').replace(settings.elementIdPrefix, '');
                    url += path;
                }

                $popup.find(settings.popupContentClass).load(url, function () {
                    if (typeof callback == 'function' && (settings.actionType === 'create' || settings.actionType === 'delete')) {
                        if ($(':submit').length > 0)
                            $(':submit').on('click', function () {
                                callback();
                            });
                    }

                    deferred.resolve();
                });
            }

            return deferred.promise();
        }

        function openPopup($mfp, popupElement, popupContentCache) {
            var deferred = $.Deferred();

            //console.log($.magnificPopup.instance.isOpen);

            $.magnificPopup.open({
                items: {
                    src: popupElement,
                    type: 'inline'
                },
                callbacks: {
                    beforeOpen: function () {
                        popupElement.addClass(settings.classToAdd).removeClass(settings.classToRemove);

                    },
                    afterClose: function () {
                        //popupElement.html(popupContentCache);
                        //if (settings.actionType === 'createFolder' || settings.actionType === 'deleteFolder')
                        //    $(settings.popupContentId).html("");
                    }
                }
            });

            return deferred.promise();
        }

        // Constructor
        return plugin.each(function () {
            settingValidation();

            var $this = $(this);

            var $popup = $(settings.popupId);
            var popupContentCache = $popup.html();
            var $mfp = $.magnificPopup.instance;

            $this.on('click', function (element) {
                element.preventDefault();
                element.stopImmediatePropagation();

                if (typeof settings.onLoadStartCallback === 'function')
                    settings.onLoadStartCallback();

                loadContentToPopup($popup, $this).done(function () {

                    openPopup($mfp, $popup, popupContentCache);

                    if (typeof settings.onLoadCompleteCallback === 'function')
                        settings.onLoadCompleteCallback();
                });
            });
        });
    };

})(jQuery);