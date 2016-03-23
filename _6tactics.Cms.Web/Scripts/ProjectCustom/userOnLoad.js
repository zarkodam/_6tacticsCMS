//$(window).on('load', function () {
//    // Set footer to bottom
//    if ($('footer').length > 0)
//        $('.body-content').layoutFixxxer({ elementsAroundTheBody: ['.navbar', '.selected-page-panel', 'footer'] });
//});

$(function () {
    function updateImageSize() {
        $('.mfp-wrap').removeAttr('style');
        $('.mfp-wrap').css('overflow', 'no-display');

        var windowsHeight = $(window).height();
        var imageGalleryTitle = $('.image-gallery-image-title').height();
        var imageGalleryDescription = $('.image-gallery-image-description').height();
        var wantedImageSize = windowsHeight - (imageGalleryTitle + imageGalleryDescription) - 100;
        var maxHeight = { 'max-height': wantedImageSize + 'px' };

        $('.mfp-figure figure img').animate(maxHeight, 100);
    }

    function magnificPopupInit(id) {
        $(id).magnificPopup({
            delegate: 'a',
            type: 'image',
            tLoading: 'Loading image #%curr%...',
            mainClass: 'mfp-img-mobile',
            gallery: {
                enabled: true,
                navigateByImgClick: true,
                preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
            },
            image: {
                lFit: true,
                tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
                titleSrc: function (item) {
                    return '<span class="image-gallery-image-title">' + item.el.attr('title') + '</span><div class="image-gallery-image-description">' + item.el.find('img').attr('alt') + '</div>';
                }
            },

            callbacks: {
                imageLoadComplete: function () {
                    updateImageSize();
                },

                resize: function () {
                    updateImageSize();
                }
            }
        });
    }

    // Popup gallery
    $('.popup-gallery').each(function() {
        var idForPoup = '#' + $(this).attr('id');
        magnificPopupInit(idForPoup);
    });


    // Initialize CS slider
    $('#da-slider').cslider({
        bgincrement: 10
    });


    // Set footer to bottom
    if ($('footer').length > 0)
        $('.body-content').layoutFixxxer({ elementsAroundTheBody: ['.navbar', '.selected-page-panel', 'footer'] });


    // Limit element size on responsive
    //$('.embed-responsive-item').limitDimensionsOnResponsive(
    //{
    //    rootElement: '.root-element',
    //    responsiveWrapper: '.responsive-wrapper',
    //    elementWrapper: '.element-wrapper',
    //    classNameForRemoveToStopResponsivness: 'embed-responsive',
    //    elementMinHeight: 120,
    //    elementMaxHeight: 320
    //});

    // Owl Carausel initializer

    var owl = $('.image-slider');
    owl.owlCarousel({
        //nav: owl.children().length > 1,
        //navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        items: 1,
        loop: owl.children().length > 1,
        autoplay: true,
        smartSpeed: 300,
        fluidSpeed: true,
        fallbackEasing: 'swing',
        animateOut: 'fadeOut',
        animateIn: 'bounce',
        margin: 30,
        stagePadding: 15,
        responsiveRefreshRate: 0
    });
    //http://www.owlcarousel.owlgraphic.com/demos/animate.html
    //http://daneden.github.io/animate.css/
});
