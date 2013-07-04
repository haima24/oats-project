function searchUsers(selector, onselect, onvalidatesource) {
    $(selector).autocomplete({
        minLength: 0,
        focus: function (ev, ui) {
            $(selector).val(ui.item.label);
            return false;
        },
        select: function (ev, ui) {
            if (onselect && typeof (onselect) === "function") {
                onselect(ui.item.id);
            }
            return false;
        },
        source: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Users/UsersSearch",
                data: JSON.stringify({ term: req.term }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.FirstName || obj.LastName) {
                                var fName = obj.FirstName ? obj.FirstName : "";
                                var lName = obj.LastName ? obj.LastName : "";
                                return { label: fName + " " + lName, value: fName + " " + lName, id: obj.UserID, rolename: obj.RoleName };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
        var li = $("<li>").append("<a>" + item.label + "</a>");

        if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }

        li.appendTo(ul);
        return li;
    };

}