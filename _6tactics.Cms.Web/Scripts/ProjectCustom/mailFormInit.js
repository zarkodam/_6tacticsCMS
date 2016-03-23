function mailFormInit(callback) {
    function reloadCaptcha() {
        $.getJSON('/ajax/regeneratecaptcha?captchaFor=MailFormCaptcha', function (newCaptcha) {
            $('#MailForm_MailFormCaptcha').val(newCaptcha);
            $('#MailForm_CaptchaCode').val('');
        });
    }

    function reloadCaptchaAndRevalidateForm() {
        reloadCaptcha();
        $('form').valid();
    }

    var progress = $('.progress');
    var bar = $('.progress-bar');
    var percent = $('.progress-bar');
    progress.hide();

    $.validator.unobtrusive.addValidation('form');

    if ($('#MailForm_CaptchaCode').length > 0) {
        //$('#MailForm_CaptchaCode').on('keyup focusout', function(e) {
        $('#MailForm_CaptchaCode').on('keyup', function (e) {
            e.stopPropagation();
        });

        $('#MailForm_CaptchaCode').rules("add", {
            remote: {
                url: '/ajax/iscaptchavalid',
                data: {
                    'captchaFor': function () { return 'MailFormCaptcha' },
                    'solution': function () { return $('#MailForm_CaptchaCode').val() }
                },
                complete: function (data) {
                    if (data.responseText !== "true") {
                        reloadCaptchaAndRevalidateForm();
                    }
                }
            }
        });
    }

    $('form').ajaxForm({
        beforeSend: function () {
            progress.show();
            var percentVal = '0%';
            bar.width(percentVal);
            percent.html(percentVal);
            $("form :input").prop('readonly', true);
        },
        uploadProgress: function (event, position, total, percentComplete) {
            var percentVal = percentComplete + '%';
            bar.width(percentVal);
            percent.html(percentVal);
        },
        success: function () {
            var percentVal = '100%';
            bar.width(percentVal);
            percent.html(percentVal);
            progress.hide();
            $('form').resetForm();
            $("form :input").prop('readonly', false);

            if (typeof callback !== 'undefined')
                callback();

            reloadCaptcha();
        },
        complete: function (xhr) {
            //console.log(xhr);
        }
    });
}

function handleFile(droppedFile) {

    function isFileContainsExtension(file, extensiontoCheck) {
        return file.name.indexOf(extensiontoCheck) >= 0;
    }

    var allowedExtension = ['.rar', '.zip', '.7z'];

    function isExtensionsAllowed(file) {
        for (var i = 0; i < allowedExtension.length; i++)
            if (isFileContainsExtension(file, allowedExtension[i])) return true;

        return false;
    }

    var file = droppedFile[0];
    var fileSizeInMb = (file.size / 1024 / 1024).toFixed(2);
    var fileSizeAllowedByServerInMb = 3;

    if (fileSizeInMb > fileSizeAllowedByServerInMb || !isExtensionsAllowed(file))
        $('#MailForm_File_Validation').show();
    else
        $('#MailForm_File_Validation').hide();
}



