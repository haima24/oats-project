(function ($) {
    $.initCommonValidator = function (handlers) {
        var specialKeys = {
            8: "backspace", 9: "tab", 13: "return", 16: "shift", 17: "ctrl", 18: "alt", 19: "pause",
            20: "capslock", 27: "esc", 32: "space", 33: "pageup", 34: "pagedown", 35: "end", 36: "home",
            37: "left", 38: "up", 39: "right", 40: "down", 45: "insert", 46: "del",
            106: "*", 107: "+", 109: "-", 110: ".", 111: "/",
            112: "f1", 113: "f2", 114: "f3", 115: "f4", 116: "f5", 117: "f6", 118: "f7", 119: "f8",
            120: "f9", 121: "f10", 122: "f11", 123: "f12", 144: "numlock", 145: "scroll", 191: "/", 224: "meta"
        };
        if (handlers && handlers.length > 0) {
            $(handlers).each(function (index, element) {
                var selector = element.selector;
                var regex = element.regex;
                var def = element.def;
                $(selector).live("keydown", function (e) {
                    var $tb = $(this);
                    var code = e.which || e.keyCode || e.charCode;
                    var special = specialKeys;
                    if (!special[code]) {
                        var inputted = String.fromCharCode(code);
                        var current = $tb.val();
                        var str = current + inputted;
                        var reg = regex;
                        if (reg.test) {
                            if (!reg.test(str)) {
                                e.preventDefault();
                            } else {
                                if (current == def.toString()) {
                                    $tb.val(inputted);
                                    e.preventDefault();
                                }
                            }
                        }
                    }
                });
                if (def) {
                    $(selector).live("keyup", function () {
                        var tb = $(this);
                        if (!tb.val()) {
                            tb.val(def);
                        }
                    });
                }
            });
        }
    };
}(jQuery));