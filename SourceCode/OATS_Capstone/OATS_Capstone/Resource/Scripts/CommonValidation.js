(function ($) {
    $.initCommonValidator = function (handlers) {
        var convertToUnsign = function (str) {
            var signedChars = "àảãáạăằẳẵắặâầẩẫấậđèẻẽéẹêềểễếệìỉĩíịòỏõóọôồổỗốộơờởỡớợùủũúụưừửữứựỳỷỹýỵÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬĐÈẺẼÉẸÊỀỂỄẾỆÌỈĨÍỊÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢÙỦŨÚỤƯỪỬỮỨỰỲỶỸÝỴ";
            var unsignedChars = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            var pattern = new RegExp("[" + signedChars + "]", "g");
            var result = str.replace(pattern, function (m, key, value) {
                return unsignedChars.charAt(signedChars.indexOf(m));
            });
            return result;
        }
        var specialKeys = {
            8: "backspace", 9: "tab", 16: "shift",
            17: "ctrl",
            18: "alt", 19: "pause",
            20: "capslock", 27: "esc", 32: "space", 33: "pageup", 34: "pagedown", 35: "end", 36: "home",
            37: "left", 38: "up", 39: "right", 40: "down", 45: "insert", 46: "del",
            106: "*", 110: ".", 111: "/",
            112: "f1", 113: "f2", 114: "f3", 115: "f4", 116: "f5", 117: "f6", 118: "f7", 119: "f8",
            120: "f9", 121: "f10", 122: "f11", 123: "f12", 144: "numlock", 145: "scroll", 191: "/", 224: "meta"
        };
        var allowKeys = {13:"\n", 107: "+", 109: "-", 189: "-",190:"." };
        if (handlers && handlers.length > 0) {
            $(handlers).each(function (index, element) {
                var selector = element.selector;
                var regex = element.regex;
                var def = element.def;
                var unicode = element.unicode;
                var min = element.min;
                var max = element.max;
                $(selector).live("keydown", function (e) {
                    //var handlersKeys = e.ctrlKey || e.altKey || e.shiftKey;
                    //if (!handlersKeys) {
                        var $tb = $(this);
                        var isEditable = $tb.is('[contenteditable]');
                        var code = e.which || e.keyCode || e.charCode;
                        var special = specialKeys;
                        if (!special[code]) {
                            var inputted = allowKeys[code] || String.fromCharCode(code);
                            var current = isEditable ? $tb.html() : $tb.val();
                            var str = current + inputted;
                            var reg = regex;
                            if (reg.test) {
                                if (!reg.test(str)) {
                                    e.preventDefault();
                                } else {
                                    if (def) {
                                        if (current == def.toString()) {
                                            isEditable ? $tb.html(inputted) : $tb.val(inputted);
                                            e.preventDefault();
                                            $tb.change();
                                        }
                                    }
                                    if (min) {
                                        var fInputed = parseFloat(inputted);
                                        if (!isNaN(fInputed) && !isNaN(min)) {
                                            if (fInputed < min) {
                                                isEditable ? $tb.html(min) : $tb.val(min);
                                                e.preventDefault();
                                                $tb.change();
                                            }
                                        }
                                    }
                                    if (max) {
                                        var maxValue = 0;
                                        if (typeof (max) === "function") {
                                            maxValue = max($tb);
                                        }
                                        else {
                                            maxValue = max;
                                        }

                                        var fInputed = parseFloat(inputted);
                                        if (!isNaN(fInputed) && !isNaN(maxValue)) {
                                            if (fInputed > maxValue) {
                                                isEditable ? $tb.html(maxValue) : $tb.val(maxValue);
                                                e.preventDefault();
                                                $tb.change();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    //}
                });
                if (def) {
                    $(selector).live("keyup", function (e) {
                        var tb = $(this);
                        var isEditable = tb.is('[contenteditable]');
                        var value = isEditable ? tb.html() : tb.val();
                        if (!value) {
                            isEditable ? tb.html(def) : tb.val(def);
                            e.preventDefault();
                            tb.change();
                        }
                    });
                }
            });
        }
    };
}(jQuery));