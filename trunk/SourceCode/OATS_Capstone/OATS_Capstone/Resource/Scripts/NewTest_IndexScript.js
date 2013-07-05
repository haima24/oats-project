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
            } else { showMessage("error", res.message); }
        });
    });
    $(".navbar-search input[type=text].nt-search-input").oatsSearch({
        select: function (item) {
            window.location.href = "/Tests/NewTest/" + item.id;
        },
        source: function (req, res,addedTagIds) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsSearch",
                data: JSON.stringify({ term: req, tagids: addedTagIds }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.TestTitle && obj.TestTitle != "") {
                                return { des: obj.DateDescription, title: obj.TestTitle, id: obj.Id };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        },
        tagsource: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsSearchTag",
                data: JSON.stringify({ term: req }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.TagName && obj.TagName != "") {
                                return { id: obj.TagID, name: obj.TagName };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        }
    });

        
    $(".btn-feedback").live("click", function () {
        var button = $(this);
        var testIdString = button.attr("test-id");
        var testid = parseInt(testIdString);
        $.post("/Tests/ModalFeedBackPopup", { testid: testid }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalPopupFeedback").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalPopupFeedback").replaceWith($(html));
                }
                initReplyAreas();
                $("#modalPopupFeedback").modal("show");
            } else {
                showMessage("error", res.message);
            }
        });
    });

    $("#contact-submit").live("click", function () {
        var testid = parseInt($("#test-id").val());
        var text = $("#message").val();
        $.post("/Tests/StudentCommentFeedBack", { testid: testid, fbDetail: text }, function (res) {
            if (res.success) {
                var html = $(res.generatedHtml);
                $("#modalPopupFeedback .nt-panel").prepend(html);
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