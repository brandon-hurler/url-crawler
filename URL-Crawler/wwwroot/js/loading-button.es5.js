"use strict";

$(document).ajaxStart(function () {
    $(".js-btn-submit:submit").hide();
    $(".js-btn-submit:disabled").show();
});

$(document).ajaxStop(function () {
    $(".js-btn-submit:submit").show();
    $(".js-btn-submit:disabled").hide();
});

