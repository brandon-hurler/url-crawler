$(document).ready(function () {
    var formElement = $('.js-ajax-form');
    var contentArea = $("#content-area");
    var validationArea = $("#validation-area");

    // Use event delegation to correctly target submit if partial has been reloaded.
    formElement.on('click', ':submit', function (e) {
        // Prevent standard post-back for SPA AJAX submit
        e.preventDefault();

        var form = $(e.currentTarget).closest('form');

        post(form, formElement, contentArea, validationArea);
    });
});

var post = function (form, formContainer, contentArea, validationArea) {
    $.ajax({
        type: 'POST',
        url: form.attr('action'),
        data: form.serialize(),
        dataType: 'json',
        success: function (data) {
            if (!data.success) {
                // Update form with data validation errors
                formContainer.html(data.payload);
            }
            else {
                // Reset validation on success
                validationArea.html("");

                // Display content
                contentArea.html(data.payload);
            }
        },
        error: function (data) {
            // Show stacktrace
            contentArea.html(data);
        }
    });
}