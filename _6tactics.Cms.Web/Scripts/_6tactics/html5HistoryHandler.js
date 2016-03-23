// Html5 History Handler (alternative to PJAX) _6tactics plugin

; (function ($) {
    $.fn.html5HistoryHandler = function (customSettings) {

        var plugin = this; // plugin instance
        var historyCache = ""; // remember last item

        // Defaults
        var settings = $.extend({
            targetSource: "",
            targetDestination: null,
            callback: null,
            asMenuNavigation: true,
            progressbar: null
        }, customSettings);

        // Progress bar handler
        function progressBarHandler(status) {
            if (settings.progressbar != null && status === 'show') {
                $(settings.progressbar).show();
                //alert(status);
            }
            if (settings.progressbar != null && status === 'hide') {
                $(settings.progressbar).hide();
                //alert(status);
            }
        }

        // Get content from source
        function getContent(url, addEntry) {

            // Checks is targetLocation added
            if (settings.targetDestination == null) {
                alert('Add target destination!');
                return;
            }

            $.get(url, function (content) {

                //alert($("<div>").html(content).find(settings.targetSource).html());

                //$(settings.targetDestination).html('');

                $(settings.targetDestination).html($("<div>").html(content).find(settings.targetSource).html());

                if (addEntry) {

                    // Add history entry using pushState
                    if (historyCache !== ("/" + url)) {
                        historyCache = ("/" + url);
                        history.pushState(null, null, url);
                    }


                }
            }).done(function () {
                // Remove progress bars
                progressBarHandler('hide');

                if (settings.callback != null)
                    settings.callback();

            }).fail(function () {
                alert("request error!");
                // Remove progress bars
                progressBarHandler('hide');
            });
        }

        // Constructor
        return plugin.each(function () {

            if (window.history && history.pushState) {
                $(this).on('click', function (e) {
                    e.preventDefault();
                    var href = $(this).attr('href');

                    if (typeof href == 'undefined')
                        alert('href is undefined!');

                    // Add progress bar
                    progressBarHandler('show');

                    // Getting content in desirable location
                    getContent(href, true);
                });


                // Adding popstate event listener to handle browser's back button
                window.addEventListener('popstate', function (e) {
                    if (historyCache !== location.pathname) {
                        historyCache = location.pathname;
                        //alert(historyCache);

                        // todo: add cache to fix returning to history item created without history api
                        getContent(historyCache, false);
                    }
                });

            } else
                console.log('HTML5 history api is not supported for your browser!');
        });
    };

})(jQuery);


//$('.some-link').html5HistoryHandler(
//    {
//        targetSource: '#target',
//        targetDestination: '#destination',
//        //progressbar: '.progress',
//        callback: function () {
//            alert('callback');
//        }
//    });