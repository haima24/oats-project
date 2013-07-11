function initPopover() {
    $(".nt-asms-list .nt-list-row .nt-name").popover({
        trigger: "hover",
        html: true,
        content: function () {
            return $(this).closest(".nt-list-row").find(".popover-introduction").html();
        }
    });
}
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
                    $("[data-toggle=tooltip]").tooltip();
                    initPopover();
                }
            } else { showMessage("error", res.message); }
        });
    });
    $(".navbar-search input[type=text].nt-search-input").oatsSearch({
        rendermenu: function (items) {
            $(items).each(function () {
                var html = this.key;
                var obj = this.value;
                if (html && obj) {
                    $(html).attr("data-toggle", "tooltip");
                    if (obj.isCurrentUserOwnTest) {
                        $(html).attr("data-original-title", "Open This Test");
                    } else {
                        $(html).attr("data-original-title", "Take This Test");
                    }
                    $(html).tooltip();
                    if (obj.intro) {
                        $(".pop-over", html).popover({
                            placement:"left",
                            trigger: "hover",
                            html: true,
                            content: function () {
                                var div = $("<div>").html(obj.intro);
                                return div;
                            }
                        });
                    }
                }
            });
            
        },
        select: function (item) {
            //if (item.isCurrentUserOwnTest) {
            //    window.location.href = "/Tests/NewTest/" + item.id;
            //} else {
            //    window.location.href = "/Tests/DoTest/" + item.id;
            //}
        },
        source: function (req, res, addedTagIds) {
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
                                return { des: obj.DateDescription, title: obj.TestTitle, id: obj.Id, isCurrentUserOwnTest: obj.IsCurrentUserOwnTest, intro: obj.Introduction };
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

    var hub = $.connection.generalHub;
    hub.client.R_commentFeedback = function (tid, generatedHtml) {
        var popTestIdString = $("#modalPopupFeedback #test-id").val();
        var popTestId = parseInt(popTestIdString);
        if (tid && generatedHtml) {
            if (!isNaN(popTestId)) {
                if (tid == popTestId) {
                    var comments = $("#comments");
                    if (comments.length > 0) {
                        var ele = $(generatedHtml);
                        comments.prepend(ele);
                        var articleCount = $("article", comments).length;
                        $("#modalPopupFeedback .comment-count").html("All Comments " + articleCount );
                    }
                }
            }
        }
    }
    hub.client.R_replyFeedback = function (tid, parentFeedBackId, generatedHtml) {
        var popTestIdString = $("#modalPopupFeedback #test-id").val();
        var popTestId = parseInt(popTestIdString);
        if (tid && parentFeedBackId && generatedHtml) {
            if (!isNaN(popTestId)) {
                if (tid == popTestId) {
                    var article = $("#comments article[parent-id=" + parentFeedBackId + "]");
                    if (article.length > 0) {
                        var ele = $(generatedHtml);
                        $(".reply-details", article).append(ele);
                        var count = $(".reply-detail", article).length;
                        $(".reply-count-link span[data-count]", article).html(count);
                    }
                }
            }
        }
    }
   
    $.connection.hub.start().done(function () {
        $("#contact-submit").live("click", function () {
            var testid = parseInt($("#test-id").val());
            var text = $("#message").val(); //get text
            if (text) {
                $.post("/Tests/StudentCommentFeedBack", { testid: testid, fbDetail: text }, function (res) {
                    if (res.success) {
                        $("#message").val(""); //clear text
                    } else {
                        showMessage("error", res.message);
                    }
                });
            }
        });
        $("#modalPopupFeedback .reply-button").live("click", function () {
            var button = $(this);
            var testid = parseInt($("#test-id").val());
            var parentFeedbackID = parseInt(button.val());
            var container = button.closest(".reply-container");
            var area=$(".reply-area", container);
            var text = area.val();
            var place = $(".reply-details", container);
            $.post("/Tests/UserReplyFeedBack", { testid: testid, parentFeedBackId: parentFeedbackID, replyDetail: text }, function (res) {
                if (res.success) {
                    if (area) { area.val(""); }
                } else {
                    showMessage("error", res.message);
                }
            });

        });
    });
});