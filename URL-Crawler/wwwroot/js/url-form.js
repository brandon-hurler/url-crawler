$(document).ready(function () {
    var formElement = $('.js-ajax-form');

    // Use event delegation to correctly target submit if partial has been reloaded.
    formElement.on('click', ':submit', function (e) {
        // Prevent standard post-back for SPA AJAX submit
        e.preventDefault();

        var form = $(e.currentTarget).closest('form');

        post(form, formElement);
    });
});

var post = function (form, container) {
    $.ajax({
        type: 'POST',
        url: form.attr('action'),
        data: form.serialize(),
        dataType: 'json',
        success: function (data) {
            container.html(data);
        },
        error: function () {
            //todo handle error
        }
    });
}