$.fn.disable = function () {
    this.each(function () {
        $(this).attr("disabled", "disabled");
    });
};

$.fn.enable = function () {
    this.each(function () {
        $(this).removeAttr("disabled")
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
        if (param && param.container && param.all && param.sub&&param.onchange&& typeof(param.onchange)==="function") {
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
                param.onchange(modal);
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
                param.onchange(modal);
            });
        }
    };
    $.findColor = function (curval, mn, mx) {
        var settings = {
            elementFunction: function () { return jQuery(this); },
            minval: 0,
            maxval: 0,
            lightness: 0.75,
            colorStyle: 'roygbiv',
            reverseOrder: false
        };
        var process= function( num ) {
			
            // adjust lightness
            var n = Math.floor( num + settings.lightness * (256 - num));
			
            // turn to hex
            var s = n.toString(16);
			
            // if no first char, prepend 0
            s = s.length == 1 ? '0' + s : s;
			
            return s;		
        };
	
        // value between 1 and 0
        var position = (curval - mn) / (mx - mn); 
			
        // this adds 0.5 at the top to get red, and limits the bottom at x= 1.7 to get purple
        var shft = settings.colorStyle == 'roygbiv'
            ? 0.5*position + 1.7*(1-position)
            : position + 0.2 + 5.5*(1-position);
			
        // scale will be multiplied by the cos(x) + 1 
        // (value from 0 to 2) so it comes up to a max of 255
        var scale = 128;
			
        // period is 2Pi
        var period = 2*Math.PI;
			
        // x is place along x axis of cosine wave
        var x = shft + position * period;
			
        // shift to negative if greentored
        x = settings.colorStyle != 'roygbiv'
            ? -x
            : x;
				
        var r = process( Math.floor((Math.cos(x) + 1) * scale) );
        var g = process( Math.floor((Math.cos(x+Math.PI/2) + 1) * scale) );
        var b = process( Math.floor((Math.cos(x+Math.PI) + 1) * scale) );
			
        return '#' + r + g + b;
		
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
