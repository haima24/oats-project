function initReplyAreas() {
    $(".reply-container textarea.reply-area").autosize({ append: "\n" });
}

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
function showCountDownMessage(type, message, after) {
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
        var downmessage = "Redirect to HomePage in " + (i--);
        $(">div", popup).html(downmessage);
    }, 1000);
    setTimeout(function () {
        clearInterval(countdown);
        popup.fadeOut("fast", function () {
            popup.removeClass("alert-error alert-info alert-success")
        });
        if (after && typeof (after) === "function") { after(); }
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
    $.initCheckboxAllSub = function (param) {
        if (param && param.container && param.all && param.sub && param.onchange && typeof (param.onchange) === "function") {
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
        var process = function (num) {

            // adjust lightness
            var n = Math.floor(num + settings.lightness * (256 - num));

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
            ? 0.5 * position + 1.7 * (1 - position)
            : position + 0.2 + 5.5 * (1 - position);

        // scale will be multiplied by the cos(x) + 1 
        // (value from 0 to 2) so it comes up to a max of 255
        var scale = 128;

        // period is 2Pi
        var period = 2 * Math.PI;

        // x is place along x axis of cosine wave
        var x = shft + position * period;

        // shift to negative if greentored
        x = settings.colorStyle != 'roygbiv'
            ? -x
            : x;

        var r = process(Math.floor((Math.cos(x) + 1) * scale));
        var g = process(Math.floor((Math.cos(x + Math.PI / 2) + 1) * scale));
        var b = process(Math.floor((Math.cos(x + Math.PI) + 1) * scale));

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

$.fn.extend({
    filedrop: function (options) {
        var defaults = {
            success: null,
            rFilter: /^(?:text\/plain)$/i,
            error: null
        }
        options = $.extend(defaults, options)
        return this.each(function () {
            var files = []
            var $this = $(this)

            // Stop default browser actions
            $this.bind('dragover', function (event) {
                $(this).css("background-color", "#EAEFFA");
                event.stopPropagation()
                event.preventDefault()
            })
            $this.bind('dragleave', function (event) {
                $(this).css("background-color", "#ffffff");
                event.stopPropagation()
                event.preventDefault()
            })
            // Catch drop event
            $this.bind('drop', function (event) {
                $(this).css("background-color", "#ffffff");
                // Stop default browser actions
                event.stopPropagation()
                event.preventDefault()

                // Get all files that are dropped
                files = event.originalEvent.target.files || event.originalEvent.dataTransfer.files
                var file = files[0];
                if (!options.rFilter.test(file.type)) {
                    if (options.error && typeof (options.error) === "function") {
                        options.error("Use plain text file.");
                    }
                } else {
                    // Convert uploaded file to data URL and pass trought callback
                    if (options.success && typeof (options.success) === "function") {
                        var reader = new FileReader()
                        reader.onload = function (event) {
                            options.success(event.target.result)
                        }
                        reader.readAsText(file)
                    }
                }
                return false
            })
        })
    }
})
$.fn.extend({
    oatsSearch: function (options) {
        var response;
        var res = function (e) {
            response = e;
            if (response.length > 0) {
                $(".nt-hitlist .nt-empty-list-ph", dropdown).hide();
                $(response).each(function (i, e) {
                    var nElement = $("<div>").addClass("nt-item");
                    var id = $(e).attr("id");
                    if (id) {
                        nElement.attr("uid", id);
                    }
                    var title = $(e).attr("title");
                    if (title) {
                        nElement.append($("<div>").html(title));
                    }
                    var des = $(e).attr("des");
                    if (des) {
                        nElement.append($("<div>").addClass("nt-item-desc").html(des));
                    }
                    $(".nt-hitlist", dropdown).append(nElement);
                });
            } else {
                $(".nt-hitlist .nt-empty-list-ph", dropdown).show();
            }
        };
        var restag = function (e) {
            if (e.length > 0) {
                $(e).each(function (i, ele) {
                    var nElement = $("<div>").addClass("nt-item");
                    var id = $(ele).attr("id");
                    if (id) {
                        nElement.attr("uid", id);
                    }
                    var name = $(ele).attr("name");
                    if (name) {
                        nElement.append($("<div>").html(name));
                    }
                    $(".nt-tag-hitlist", dropdown).append(nElement);
                });

                $(".nt-tag-hitlist", dropdown).show();
            } else {
                $(".nt-tag-hitlist", dropdown).hide();
            }
        };
        var dropdown =
            $("<div>")
            .addClass("nt-search-results")
            .append(
                $("<div>").addClass("nt-taglist").addClass("nt-tags")
                .append(
                    $("<div>").addClass("nt-tag-adder").append
                        (
                        $("<input type='text' class='input-small nt-as-input' placeholder='Search Tag...'>")
                        )
                ).append(
                    $("<div>").addClass("nt-tag-hitlist")
                )
            )
            .append(
            $("<div>").addClass("nt-hitlist").append($("<div>").addClass("nt-empty-list-ph").append($("<p>").html("Start typing a test title or a tag.")))
            );
        var defaults = {
            minLength: 0,
        }
        options = $.extend(defaults, options);
        $(this).each(function (index, element) {
            var self = $(this);
            var parent = $(element).parent();
            parent.append(dropdown);
            $("input[type=text]", dropdown).on("keydown", function (ev) {
                var text = $(this).val() + String.fromCharCode(ev.keyCode);
                $(".nt-tag-hitlist .nt-item", dropdown).remove();
                if (options.tagsource && typeof (options.tagsource) === "function") {
                    options.tagsource(text, restag);

                }
            });
            $(element).live("focus", function () {
                dropdown.show();
            });
            $(element).live("keydown", function (ev) {
                $(".nt-hitlist .nt-item", dropdown).remove();
                var text= $(this).val() + String.fromCharCode(ev.keyCode);
                if (options.source && typeof (options.source) === "function") {
                    options.source(text, res);
                    
                }
            });
            $(document).live("click", function (e) {
                if (!$(e.target).is(self) && $(e.target).closest(dropdown).length == 0) {
                    dropdown.hide();
                }
            });
        });
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
