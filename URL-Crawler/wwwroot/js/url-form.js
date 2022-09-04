$(document).ready(function () {
    var form = $('.js-ajax-form');
    var contentArea = $("#content-area");

    // Use event delegation to correctly target submit if partial has been reloaded.
    form.on('click', ':submit', function (e) {
        // Prevent standard post-back for SPA AJAX submit
        e.preventDefault();

        post(form, contentArea, initializeCarousel);
    });
});

var post = function (form, contentArea, initialize) {
    var validationArea = form.find("#validation-area");

    $.ajax({
        type: 'POST',
        url: form.attr('action'),
        data: form.serialize(),
        dataType: 'json',
        success: function (data) {
            if (!data.success) {
                // Update form with data validation errors
                form.html(data.payload);
            }
            else {
                // Clear errors on success if any
                validationArea.html("");

                // Display content
                contentArea.html(data.payload);

                // Can init any passed function into form post to bind events to newly generated partial content
                initialize();
            }
        },
        error: function (data) {
            // Show stacktrace, can be replaced by user-friendly errors
            contentArea.html(data);
        }
    });
}

var initializeCarousel = function () {
    $("#image-carousel").carousel();
}