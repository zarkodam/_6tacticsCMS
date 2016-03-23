function captchaOverrideInit() {
    $(window).on('load', function () {
        $('.LBD_CaptchaImage').parent().css('cursor', 'default');
    });

    $('.LBD_CaptchaImage').parent().on('click', function () {
        event.preventDefault();
    });
}