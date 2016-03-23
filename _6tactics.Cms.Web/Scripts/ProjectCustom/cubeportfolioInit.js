// Cubeportfolio initializer(jquery.cubeportfolio plugin required!)

$(function () {
    var gridContainer = $('#grid-container');
    var filtersContainer = $('#filters-container');


    var options = {
        defaultFilter: '*',
        animationType: 'fadeOutTop',
        gapHorizontal: 30,
        gapVertical: 30,
        gridAdjustment: 'responsive',
        //gridAdjustment: 'alignCenter',
        caption: 'zoom',
        displayType: 'lazyLoading',
        displayTypeSpeed: 0
    };

    // init cubeportfolio
    gridContainer.cubeportfolio(options);

    //activate filters for cubeportfolio
    filtersContainer.on('click', 'button', function (e) {

        // add class cbp-filter-item-active on the current button clicked and remove from other buttons
        $(this).addClass('cbp-filter-item-active').siblings().removeClass('cbp-filter-item-active');

        // call cubeportfolio filter function
        gridContainer.cubeportfolio('filter', $(this).data('filter'));

    });

    // activate counter on filters for cubeportfolio
    //gridContainer.cubeportfolio('showCounter', filtersContainer.find('button'));

});