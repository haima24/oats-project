﻿function initReplyAreas() {
    $(".reply-container textarea.reply-area").autosize({ append: "\n" });
}
$.fn.hasAttr = function (attribute) {
    return $(this).is(function () {
        var item = $(this);
        return typeof (item.attr(attribute)) != "undefined";
    });
};
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
function showCountDownMessage(type, message, redirectMessage, after) {
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
        var downmessage = redirectMessage + " in " + (i--);
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
    clickout: function (options) {
        $(this).each(function (i, e) {
            var self = $(this);
            $(document).live("click", function (e) {
                var isExcept = $(options.excepts).is(function () {
                    return $(e.target).is(this) || $(e.target).closest(this).length > 0;
                });
                if (!isExcept) {
                    if (!$(e.target).is(self) && $(e.target).closest(self).length == 0) {
                        if (options.callback && typeof (options.callback) === "function") {
                            options.callback(e, self);
                        }
                    }
                }
            });
        });
    }
})
$.fn.extend({
    oatsSearch: function (options) {
        var text;
        var reCalculatePosition = function () {
            var tb = $(".nt-tag-adder input[type=text]", dropdown);
            var pos = tb.position();
            var height = tb.height();
            var list = $(".nt-tag-hitlist", dropdown);
            list.css({ top: pos.top + height, left: pos.left });
        };
        var reSearch = function () {
            $(".nt-hitlist .nt-item", dropdown).remove();
            if (options.source && typeof (options.source) === "function") {
                options.source(text, res, addedTags);
            }
        };
        var addedTags = new Array(); //list of int
        var items = new Array();
        var tagItems = new Array();
        var res = function (response) {
            items = new Array();
            if (response.length > 0) {
                $(".nt-hitlist .nt-empty-list-ph", dropdown).hide();
                $(response).each(function (i, e) {
                    var nElement = $("<div>").addClass("nt-item");
                    var id = $(e).attr("id");
                    if (id) {
                        nElement.attr("uid", id);
                    }
                    var popover = $("<div>").addClass("pop-over");
                    var title = $(e).attr("title");
                    if (title) {
                        popover.append($("<div>").html(title));
                    }
                    var des = $(e).attr("des");
                    if (des) {
                        popover.append($("<div>").addClass("nt-item-desc").html(des));
                    }
                    nElement.append(popover);
                    $(".nt-hitlist", dropdown).append(nElement);
                    items.push({ key: nElement, value: e });
                    if (options.rendermenu && typeof (options.rendermenu) === "function") {
                        options.rendermenu(items);
                    }
                });

            } else {
                $(".nt-hitlist .nt-empty-list-ph", dropdown).show();
            }
        };
        var restag = function (e) {
            tagItems = new Array();
            var list = $(".nt-tag-hitlist", dropdown);
            reCalculatePosition();
            if (e.length > 0) {
                $(e).each(function (i, ele) {
                    var tid = $(ele).attr("id");
                    var isNotIn = $.inArray(tid, addedTags) < 0;
                    var currentCount = $(".nt-item", list).length + 1;
                    if (tid && isNotIn && currentCount <= options.maxTags) {
                        var nElement = $("<div>").addClass("nt-item");
                        nElement.append($("<i>").addClass("t-icon").addClass("i-tag"));
                        var id = $(ele).attr("id");
                        if (id) {
                            nElement.attr("uid", id);
                        }
                        var name = $(ele).attr("name");
                        if (name) {
                            nElement.append($("<div>").html(name));
                        }
                        list.append(nElement);
                        tagItems.push({ id: id, key: nElement, value: ele });
                    }
                });
                list.show();
            } else {
                list.hide();
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
            maxTags: 5,
        }
        options = $.extend(defaults, options);
        $(this).each(function (index, element) {
            var self = $(this);
            var parent = $(element).parent();
            parent.append(dropdown);
            $("input[type=text]", dropdown).on("keyup", function (ev) {
                var text = $(this).val();
                $(".nt-tag-hitlist .nt-item", dropdown).remove();
                if (options.tagsource && typeof (options.tagsource) === "function") {

                    options.tagsource(text, restag);

                }
            });
            $(element).live("focus", function () {
                dropdown.show();
            });
            $(element).live("keyup", function (ev) {
                text = $(this).val();
                reSearch();
            });
            $(document).live("click", function (e) {
                if (!$(e.target).is(self) && $(e.target).closest(dropdown).length == 0) {
                    dropdown.hide();
                }
            });
            $(".nt-taglist", dropdown).on("click", function (ev) {
                var clickItem = $(ev.target).closest(".nt-tag-remove");
                if (clickItem.length > 0) {
                    var tag = clickItem.closest(".nt-tag");
                    if (tag) {
                        var id = tag.attr("tag-id");
                        if (id) {
                            var intId = parseInt(id);
                            var index = addedTags.indexOf(intId);
                            if (index >= 0) {
                                addedTags.splice(index, 1);
                                reSearch();
                                tag.remove();
                                ev.stopPropagation();
                            }
                        }
                    }
                }
            });

            $(".nt-tag-hitlist", dropdown).on("click", function (ev) {
                var list = $(this);
                var clickItem = $(ev.target).closest(".nt-item");
                var tagsContainer = $(".nt-taglist.nt-tags", dropdown);
                if (clickItem.length > 0) {
                    if (tagItems) {
                        var item = $(tagItems).filter(function () {
                            return clickItem.is($(this).attr("key"));
                        });
                        if (item.length > 0) {
                            var bindedObj = item.attr("value");
                            if (($.inArray(bindedObj.id, addedTags)) < 0) {
                                addedTags.push(bindedObj.id);
                                var htmlTag = $("<div>").addClass("nt-tag").attr("tag-id", bindedObj.id).append(
                                    $("<span>").html(bindedObj.name)
                                    ).append("<i class='icon-remove nt-tag-remove'></i>");
                                tagsContainer.prepend(htmlTag);
                                reCalculatePosition();
                                reSearch();
                                var taglist = $(".nt-tag-hitlist", dropdown);
                                taglist.hide();
                                $("input[type=text]", dropdown).val("");
                            }

                        }
                    }
                }
                ev.stopPropagation();
            });
            $(".nt-hitlist", dropdown).on("click", function (ev) {
                var clickItem = $(ev.target).closest(".nt-item");
                if (clickItem.length > 0) {
                    if (items) {
                        var item = $(items).filter(function () {
                            return clickItem.is($(this).attr("key"));
                        });
                        if (item.length > 0) {
                            if (options.select && typeof (options.select) === "function") {
                                options.select(item.attr("value"), clickItem);
                            }
                        }
                    }
                }
            });
        });
    }
});
$.fn.extend({
    reuseTagSearch: function (options) {
        var tagItems = new Array();
        var addedTags = new Array();
        var container;
        var defaults = {
            maxTags: 5,
        }
        options = $.extend(defaults, options);
        var curElement;
        var reCalculatePosition = function (list) {
            if (curElement) {
                var tb = $(curElement);
                var pos = tb.position();
                var height = tb.height();
                list.css({ top: pos.top + height, left: pos.left });
            }
        };
        var restag = function (e) {
            tagItems = new Array();
            if (container) {
                var list = $(".nt-tag-hitlist", container);
                list.empty();
                reCalculatePosition(list);
                if (e.length > 0) {
                    $(e).each(function (i, ele) {
                        var tid = $(ele).attr("id");
                        //var isNotIn = $.inArray(tid, addedTags) < 0;
                        var isNotIn = true;
                        var currentCount = $(".nt-item", list).length + 1;
                        if (tid && isNotIn && currentCount <= options.maxTags) {
                            var nElement = $("<div>").addClass("nt-item");
                            nElement.append($("<i>").addClass("t-icon").addClass("i-tag"));
                            var id = $(ele).attr("id");
                            if (id) {
                                nElement.attr("uid", id);
                            }
                            var name = $(ele).attr("name");
                            if (name) {
                                nElement.append($("<div>").html(name));
                            }
                            list.append(nElement);
                            tagItems.push({ id: id, key: nElement, value: ele });
                        }
                    });
                    list.show();
                } else {
                    list.hide();
                }
            }
        };
        $(this).each(function (index, element) {
            $(element).live("keyup", function (ev) {
                if (options.container) {
                    container = $(this).closest(options.container);
                }
                curElement = $(this);
                var text = $(this).val();
                if (options.tagsource && typeof (options.tagsource) === "function") {
                    options.tagsource(text, restag);
                }
            });
            if (options.container) {
                var curContainer = $(this).closest(options.container);
                $(curContainer).on("click", function (ev) {
                    var clickItem = $(ev.target).closest(".nt-tag-remove");
                    if (clickItem.length > 0) {
                        var tag = clickItem.closest(".nt-tag");
                        if (tag) {
                            var id = tag.attr("tag-id");
                            if (id) {
                                var intId = parseInt(id);
                                var index = addedTags.indexOf(intId);
                                if (index >= 0) {
                                    addedTags.splice(index, 1);
                                    tag.remove();
                                    ev.stopPropagation();
                                    if (options.tagschange && typeof (options.tagschange) === "function") {
                                        options.tagschange(addedTags);
                                    }
                                }
                            }
                        }
                    }
                });
                $(".nt-tag-hitlist", curContainer).on("click", function (ev) {
                    var list = $(this);
                    var clickItem = $(ev.target).closest(".nt-item");
                    var tagsContainer = curContainer;
                    if (clickItem.length > 0) {
                        if (tagItems) {
                            var item = $(tagItems).filter(function () {
                                return clickItem.is($(this).attr("key"));
                            });
                            if (item.length > 0) {
                                var bindedObj = item.attr("value");
                                if (($.inArray(bindedObj.id, addedTags)) < 0) {
                                    addedTags.push(bindedObj.id);
                                    var htmlTag = $("<div>").addClass("nt-tag").attr("tag-id", bindedObj.id).append(
                                        $("<span>").html(bindedObj.name)
                                        ).append("<i class='icon-remove nt-tag-remove'></i>");
                                    tagsContainer.prepend(htmlTag);
                                    reCalculatePosition(list);
                                    list.hide();
                                    $("input[type=text]", curContainer).val("");
                                    if (options.tagschange && typeof (options.tagschange) === "function") {
                                        options.tagschange(addedTags);
                                    }
                                }

                            }
                        }
                    }
                    ev.stopPropagation();
                });
            }
        });
    }
});
$.fn.extend({
    clientSort: function (options) {
        var defaults = {
            activeClass:"active",
            handlers: [],
            sortItems: [],
            parentContainer:""
        };
        options = $.extend(defaults, options);
        var $items = $(this);
        $items.live("click", function () {
            var $btn=$(this);
            var items = $($items.selector);
            items.each(function () {
                $(this).removeClass(options.activeClass)
            });
            $(this).addClass(options.activeClass)
            if (options.handlers && options.sortItems) {
                var sortedItems;
                if (options.parentContainer) {
                    var container=$btn.closest(options.parentContainer);
                    sortedItems = $(options.sortItems, container);
                } else {
                    sortedItems = $(options.sortItems);
                }
                var key = $btn.data("key");
                var order = $btn.data("order") || "asc";
                if (key) {
                    var handle = $(options.handlers).filter(function () {
                        return this.key == key;
                    });
                    if (handle.length == 1) {
                        var hand = handle[0];
                        var handler=hand.handler;
                        if (handler) {
                            if (hand.useSortAttr) {
                                $(sortedItems).tsort(handler, { order: order, attr: "data-sort-value" });
                            }
                            else {
                                $(sortedItems).tsort(handler, { order: order });
                            }
                            
                        }
                    }
                }
            }
        });
    }
});
function random(range) {
    return Math.floor(Math.random() * range);
}
function statusSaving() {
    $("#savestatus .nt-desc").html("Saving...");
    $("#savestatus").fadeIn("slow");
}
function statusSaved() {
    $("#savestatus .nt-desc").html("All changes saved.");
    $("#savestatus").fadeOut("slow");
}
function setLocalStorage(key, value) {
    if (typeof (Storage) !== "undefined") {
        // Yes! localStorage and sessionStorage support!
        // Some code.....
        localStorage.setItem(key, value);
    }
    else {
        // Sorry! No web storage support..
    }
}
function getLocalStorage(key) {
    var obj;
    if (typeof (Storage) !== "undefined") {
        // Yes! localStorage and sessionStorage support!
        // Some code.....
        obj = localStorage.getItem(key);
    }
    else {
        // Sorry! No web storage support..
    }
    return obj;
}
