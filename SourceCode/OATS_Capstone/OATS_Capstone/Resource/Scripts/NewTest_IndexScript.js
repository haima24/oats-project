function initCalendar() {
    $('#calendar').addClass("loading");
    $.post("/Tests/TestCalendarObjectResult", function (res) {
        if (res.success) {
            events = $(res.listTestCalendar).map(function (index,obj) {
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
});