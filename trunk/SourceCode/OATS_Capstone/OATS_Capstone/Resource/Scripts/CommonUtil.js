﻿function showMessage(type, message,heading)
{
    var popup = $("#message-popup");
    $("#message-popup .close").live("click", function () {
        popup.fadeOut("fast", function () {
            popup.removeClass("alert-error alert-info alert-success")
        });
        
    });
    $(">div",popup).html(message);
    $(".alert-heading").html(heading);
    switch (type) {
        case "error":
            popup.addClass("alert-error");
            break;
        case "info":
            popup.addClass("alert-info");
            break;
        case "success":
            popup.addClass("alert-success");
            break;
        default:
            break;
    }
    popup.fadeIn("fast");
    setTimeout(function () {
        popup.fadeOut("fast", function () {
            popup.removeClass("alert-error alert-info alert-success")
        });
    }, 5000);
}
function convertJsonDatetoDate(jsondate)
{
    if (jsondate) {
        return new Date(parseInt(jsondate.substr(6)));
    } else {
        return null;
    }
}
$.fn.extend({
    convertJqueryArrayToJSArray: function () {
        var arr = [];
        for (var i = 0; i < this.length; i++) {
            arr[i] = this[i];
        }
        return arr;
    }
});
function showInfoMessage(message) {

}
function showErrorMessage(message) {
}