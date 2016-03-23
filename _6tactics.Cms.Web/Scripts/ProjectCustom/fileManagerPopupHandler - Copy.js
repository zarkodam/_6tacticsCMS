//// FileManager popup handler _6tactics plugin

//; (function ($) {
//    $.fn.fileManagerPopupHandler = function (customSettings, callback) {

//        // Instance
//        var plugin = this;

//        // Locally calling default settings and allowing to override them through options parameter
//        var settings = $.extend({
//            popupId: null,
//            popupContentClass: ".popup-content",
//            classToRemove: '',
//            classToAdd: '',
//            elementIdPrefix: '',
//            urlToLoad: null
//        }, customSettings);

//        function settingValidation() {
//            var returnBool = true;

//            if (settings.popupId == null) {
//                alert("popupId is required!");
//                returnBool = false;
//            }

//            return returnBool;
//        }

//        function loadContentToPopup($popup, $this) {
//            var deferred = $.Deferred();
//            if (settings.urlToLoad != null) {
//                var path = $this.attr('id').replace(settings.elementIdPrefix, '');
//                var url = settings.urlToLoad + path;

//                $popup.find(settings.popupContentClass).load(url, function () {
//                    if (typeof callback == 'function') {
//                        if ($(':submit').length > 0)
//                            $(':submit').on('click', function () {
//                                callback();
//                            });
//                    }

//                    deferred.resolve();
//                });
//            }

//            return deferred.promise();
//        }

//        function openPopup(popupElement, popupContentCache) {
//            var deferred = $.Deferred();

//            $.magnificPopup.open({
//                items: {
//                    src: popupElement,
//                    type: 'inline'
//                },
//                callbacks: {
//                    beforeOpen: function () {
//                        popupElement.addClass(settings.classToAdd).removeClass(settings.classToRemove);
//                    },
//                    afterClose: function () {
//                        deferred.resolve();
//                        //this.close();
//                        //$.magnificPopup.close();
//                        //popupElement.html(popupContentCache);
//                        //if (settings.actionType === 'createFolder' || settings.actionType === 'deleteFolder')
//                        //    $(settings.popupContentId).html("");
//                    }
//                }
//            });

//            return deferred.promise();
//        }

//        // Constructor
//        return plugin.each(function () {
//            settingValidation();

//            var $this = $(this);

//            var $popup = $(settings.popupId);
//            var popupContentCache = $popup.html();

//            $this.on('click', function (element) {
//                element.preventDefault();

//                loadContentToPopup($popup, $this).done(function () {
//                    //alert($popup.find(settings.popupContentClass).html());
//                    openPopup($popup, popupContentCache).done(function () {
//                        $.magnificPopup.instance.close();
//                    });
//                });
//            });
//        });
//    };

//})(jQuery);