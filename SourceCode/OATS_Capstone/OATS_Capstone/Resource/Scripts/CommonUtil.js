$.fn.disable = function () {
    return this.each(function () {
        if (typeof this.disabled != "undefined") {
            $(this).data('jquery.disabled', this.disabled);

            this.disabled = true;
        }
    });
};

$.fn.enable = function () {
    return this.each(function () {
        if (typeof this.disabled != "undefined") {
            this.disabled = $(this).data('jquery.disabled');
        }
    });
};
function showMessage(type, message, heading) {
    var popup = $("#message-popup");
    $("#message-popup .close").live("click", function () {
        popup.fadeOut("fast", function () {
            popup.removeClass("alert-error alert-info alert-success")
        });

    });
    $(">div", popup).html(message);
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
function showCountDownMessage(type, message,after) {
    var popup = $("#message-popup");
    $("#message-popup .close").live("click", function () {
        popup.fadeOut("fast", function () {
            popup.removeClass("alert-error alert-info alert-success")
        });

    });
  
    $(".alert-heading").html(message);
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
    var i = 3;
    var countdown = setInterval(function () {
        var downmessage = "To HomePage in " + (i--);
        $(">div", popup).html(downmessage);
    }, 1000);
    setTimeout(function () {
        clearInterval(countdown);
        popup.fadeOut("fast", function () {
            popup.removeClass("alert-error alert-info alert-success")
        });
        if (after && typeof (after) === "function") { after();}
    }, 5000);
}
function convertJsonDatetoDate(jsondate) {
    if (jsondate) {
        return new Date(parseInt(jsondate.substr(6)));
    } else {
        return null;
    }
}
$.fn.extend({
    scrollToElement: function (item) {
        var offset = item.position().top + this.scrollTop();
        this.animate({ scrollTop: offset }, 1000);
    }
});
(function ($) {
    $.initCheckboxAllSub =  function (param) {
        if (param && param.container && param.all && param.sub) {
            //separator
            $(param.all).live("change", function (ev) {
                var modal = $(this).closest(param.container);
                $(param.sub, modal).each(function () {
                    if (ev.target.checked) {
                        $(this).attr("checked", true);
                    } else {
                        $(this).attr("checked", false);
                    }
                });
            });
            //separator
            $(param.sub).live("change", function () {
                var modal = $(this).closest(param.container);
                var isOneUncheck = $(param.sub, modal).is(function (index) {
                    return !this.checked;
                });
                if (isOneUncheck) {
                    $(param.all).attr("checked", false);
                } else {
                    $(param.all).attr("checked", true);
                }
            });
        }
    };
}(jQuery));
$.fn.extend({
    convertToUnsign: function () {
        var signedChars = "àảãáạăằẳẵắặâầẩẫấậđèẻẽéẹêềểễếệìỉĩíịòỏõóọôồổỗốộơờởỡớợùủũúụưừửữứựỳỷỹýỵÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬĐÈẺẼÉẸÊỀỂỄẾỆÌỈĨÍỊÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢÙỦŨÚỤƯỪỬỮỨỰỲỶỸÝỴ";
        var unsignedChars = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
        var pattern = new RegExp("[" + signedChars + "]", "g");
        var result = this.replace(pattern, function (m, key, value) {
            return unsignedChars.charAt(signedChars.indexOf(m));
        });
        return result;
    }
});
$.fn.extend({
    convertJqueryArrayToJSArray: function () {
        var arr = [];
        for (var i = 0; i < this.length; i++) {
            arr[i] = this[i];
        }
        return arr;
    }
});
function statusSaving() {
    $("#savestatus .nt-desc").html("Saving...");
    $("#savestatus").fadeIn("slow");
}
function statusSaved() {
    $("#savestatus .nt-desc").html("All changes saved.");
    $("#savestatus").fadeOut("slow");
}