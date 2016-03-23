$(function () {
    //$('.image-link').magnificPopup({ type: 'image' });

    // Initialize Magnific popup
    $('.delete-content-item-popup').magnificPopup({
        type: 'inline', midClick: true, callbacks: {
            close: function () {
                $('#delete-popup-content').html("");
            }
        }
    });

    // Delete popup with confirmation
    $('.delete-content-item-popup').attr("href", "#delete-content-item-popup");
    $('.delete-content-item-popup').on('click', function (element) {
        element.preventDefault();
        var url = "/Admin/Delete/" + $(this).attr('id');
        $('#delete-popup-content').load(url);
    });
});