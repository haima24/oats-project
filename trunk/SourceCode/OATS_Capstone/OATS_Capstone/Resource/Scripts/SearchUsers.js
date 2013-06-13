function searchUsers(selector,onselect,onvalidatesource){
    $.post("/Users/UsersSearch", function (res) {
        if (res) {
            var source = $(res.listuser).map(function (index, obj) {
                if(obj.FirstName||obj.LastName)
                {
                    var fName = obj.FirstName ? obj.FirstName : "";
                    var lName = obj.LastName?obj.LastName:"";
                    return { label: fName + " " + lName, value: fName + " " + lName, id: obj.UserID,rolename:obj.RoleName };
                }
            }).convertJqueryArrayToJSArray();
            if (onvalidatesource && typeof (onvalidatesource) === "function") {
                source = onvalidatesource(source);
            }
            $(selector).autocomplete({
                minLength: 0,
                source: source,
                focus: function (ev, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);
                    return false;
                },
                select: function (ev, ui) {
                    if (onselect && typeof (onselect) === "function") {
                        onselect(ui.item.id);
                    }
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
                var li = $("<li>").append("<a>" + item.label + "</a>");

                if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }

                li.appendTo(ul);
                return li;
            };
        }
    });
}