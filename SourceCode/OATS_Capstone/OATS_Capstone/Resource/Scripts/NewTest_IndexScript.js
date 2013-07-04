function initCalendar() {
    $('#calendar').addClass("loading");
    $.post("/Tests/TestCalendarObjectResult", function (res) {
        if (res.success) {
            events = $(res.resultlist).map(function (index, obj) {
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
            if (res.success) {
                var tabcontent = $("#eventTab");
                if (tabcontent && res.generatedHtml) {

                    tabcontent.html(res.generatedHtml);

                    initCalendar();

                }
            } else { showMessage("error", res.message);}
        });
    });
    $(".navbar-search input[type=text].nt-search-input").autocomplete({
        minLength: 0,
        focus: function (ev, ui) {
            $(".navbar-search input[type=text].nt-search-input").val(ui.item.label);
            return false;
        },
        select: function (ev, ui) {
            $(".navbar-search input[type=text].nt-search-input").val(ui.item.label);
            window.location.href = "/Tests/NewTest/" + ui.item.id;
            return false;
        },
        source: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsSearch",
                data: JSON.stringify({term:req.term} ),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.TestTitle && obj.TestTitle != "") {
                                return { label: obj.TestTitle, value: obj.TestTitle, id: obj.Id };
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
    $(".btn-feedback").live("click", function () {
        var button = $(this);
        var testIdString = button.attr("test-id");
        var testid = parseInt(testIdString);
        $.post("/Tests/ModalFeedBackPopup", { testid: testid }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalRemovePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalRemovePopupUser").replaceWith($(html));
                }
                initReplyAreas();
                $("#modalRemovePopupUser").modal("show");
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $(".reply-container[toggle-header]").live("click", function () {
        var cur = $(this);
        var detail = cur.siblings(".reply-container[toggle-detail]");
        cur.hide();
        detail.show();
    });
    $(".reply-container[toggle-detail] div[toggle-detail-trigger]").live("click", function () {
        var cur = $(this).closest(".reply-container[toggle-detail]");
        var header = cur.siblings(".reply-container[toggle-header]");
        cur.hide();
        header.show();
    });
});