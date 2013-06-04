function initCalendar() {
    $('#calendar').addClass("loading");
    $.post("/Tests/TestCalendarObjectResult", function (res) {
        if (res.success) {
            events = $(res.listTestCalendar).map(function (index, obj) {
                    return {
                        id: obj.id,
                        title: obj.testTitle,
                        start: convertJsonDatetoDate(obj.startDateTime),
                        end: convertJsonDatetoDate(obj.endDateTime)
                    };
            }).convertJqueryArrayToJSArray();
            $('#calendar').fullCalendar({
                theme: true,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                editable: true,
                events: events
            });
            $('#calendar').removeClass("loading");
        } else {
            showMessage("error", res.message);
        }
        

    });

    $("#asmsOverview").kalendae({
        months: 3,
        mode: 'single',
        selected: Kalendae.moment().subtract({ M: 1 })
    });
}
$(function () {
    initCalendar();
    $(".tab-event").live("click", function (e) {
        e.preventDefault();
        var link = e.target;
        var action = $(link).attr("href");
        var nav = $(link).closest(".nav");
        var li = $(link).closest("li");
        nav.find("li").removeClass("active");
        li.addClass("active");
        $.post(action, function (res) {
            var tabcontent = $("#eventTab");
            if (tabcontent && res.tab) {

                tabcontent.html(res.tab);

                initCalendar();

            }
        });
    });
    $.post("/Tests/TestsSearch", function (res) {
        if (res.success) {
            var source = $(res.listTestsSearch).map(function (index, obj) {
                if (obj.TestTitle && obj.TestTitle != "") {
                    return { label: obj.TestTitle, value: obj.TestTitle, id: obj.Id };
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
                    window.location.href = "/Tests/NewTest/" + ui.item.id;
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
                var li = $("<li>").append("<a>" + item.label + "</a>");
                if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }
                li.appendTo(ul);
                return li;
            };
        } else {
            showMessage("error", res.message);
        }
    });
});