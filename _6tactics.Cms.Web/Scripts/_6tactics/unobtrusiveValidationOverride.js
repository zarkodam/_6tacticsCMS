(function ($) {
    $.validator.unobtrusive.addValidation = function (selector) {
        var form = $(selector);
        form.removeData('validator');
        form.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse(form);
    }
})(jQuery);
