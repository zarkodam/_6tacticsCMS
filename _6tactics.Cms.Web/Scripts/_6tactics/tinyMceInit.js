// TinyMCE initializer(tinyMCE plugin required!) _6tactics plugin

; (function ($) {
    $.fn.tinyMceInit = function (options) {

        // Instance
        var plugin = this;

        // Defaults
        var defaults = {
            isBasicConfig: true,
            elementHeight: 'auto',
            elementWidth: 'auto',
            readonly: false,
        };


        // Locally calling default settings and allowing to override them through options parameter
        var settings = $.extend({}, defaults, options); //settings.opt1

        // Private methods
        function basicOptions(elementSelector) {
            return { selector: elementSelector, height: settings.elementHeight, readonly: settings.readonly };
        }

        function fullOptions(elementSelector) {
            return {
                selector: elementSelector,
                readonly: settings.readonly,
                theme: "modern",
                //body_class: "mceBlackBody",
                height: settings.elementHeight,
                plugins: [
                    "advlist autolink link image lists charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                    "save table contextmenu directionality emoticons template paste textcolor"
                ],
                //content_css: "/Scripts/tinymce/skins/lightgray/content.min.css",
                toolbar: "insertfile undo redo | styleselect fontselect fontsizeselect | bold italic | alignleft aligncenter alignright alignjustify " +
                    "| bullist numlist outdent indent | link image media | forecolor backcolor emoticons " +
                    "| hr removeformat | subscript superscript | charmap | spellchecker | visualchars visualblocks nonbreaking template pagebreak " +
                    "| print preview | fullscreen " +
                    "",
                style_formats: [
                    { title: 'Bold text', inline: 'b' },
                    { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
                    { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
                    { title: 'Example 1', inline: 'span', classes: 'example1' },
                    { title: 'Example 2', inline: 'span', classes: 'example2' },
                    { title: 'Table styles', selector: 'tr', styles: { background: '#999' } },
                    { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
                ]
            };
        }

        // Constructor
        return plugin.each(function () {
            var config;
            var elementIdName = $(this).attr('id');

            if (settings.isBasicConfig)
                config = basicOptions($(this));
            else
                config = fullOptions($(this));

            tinymce.init(config);
            tinyMCE.execCommand('mceRemoveEditor', false, elementIdName);
            tinyMCE.onAddEditor(alert("asd"));
            tinyMCE.settings = config;
            tinyMCE.execCommand('mceAddEditor', false, elementIdName);
        });

    };

})(jQuery);