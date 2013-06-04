/// <reference path="SearchStudents.js" />
$(function () {
    $.post("/Teachers/TeachersSearch", function (res) {
        if (res) {
            var source = $(res.listTeachersSearch).map(function (index, obj) {
                if (obj.FirstName && obj.LastName && (obj.FirstName != "" || obj.LastName != "")) {
                    return { label: obj.FirstName + " " + obj.LastName, value: obj.FirstName + " " + obj.LastName };
                }
            }).convertJqueryArrayToJSArray();
            $(".navbar-search .nt-search-input").autocomplete({
                minLength: 0,
                source: source,
                focus: function (ev, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);
                    return false;
                },
                select: function (ev, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
                var li = $("<li>").append("<a>" + item.label + "</a>");

                if(!li.hasClass("search-autocomplete-hover-item")){li.addClass("search-autocomplete-hover-item");}

                li.appendTo(ul);
                return li;
            };
        }
    });

});