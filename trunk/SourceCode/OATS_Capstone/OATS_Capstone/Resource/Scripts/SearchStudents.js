/// <reference path="SearchStudents.js" />
$(function () {
    $.post("/Students/StudentsSearch", function (res) {
        if (res) {
            var source = res.map(function (obj, index) {
                return { label: obj.FirstName, value: obj.FirstName };
            });
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
                li.appendTo(ul);
                return li;
            };
        }
    });

});