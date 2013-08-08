function initCommonValidation() {
    var handlers = new Array();
    handlers.push({ selector: "#message", regex: /^(.|\n){0,1024}$/ });
    handlers.push({ selector: "#modalPopupFeedback textarea.reply-area", regex: /^(.|\n){0,1024}$/ });
    $.initCommonValidator(handlers);
}
function initClientSorting() {
    //sidebar
    var handlers = new Array();
    handlers.push({
        key: "name", handler: ".nt-clb-item-label"
    });
    $(".nt-clb-header-label .nt-btn-circle-mini,.nt-clb-header-desc .nt-btn-circle-mini").clientSort({
        sortItems: ".nt-clb-item",
        handlers: handlers,
        parentContainer: ".nt-ctrl-list"
    });
    //sidebar
}
function handleOverviewCheckedUsersAndTests() {
    var userids = $("#sidebar #menuUsers .nt-ctrl-list input[type=checkbox]:checked").map(function () { return $(this).attr("user-id"); }).convertJqueryArrayToJSArray();
    var testids = $("#sidebar #menuTests .nt-ctrl-list input[type=checkbox]:checked").map(function () { return $(this).attr("test-id"); }).convertJqueryArrayToJSArray();
    $.ajax({
        type: "POST",
        url: "/Tests/Index_OverViewTab",
        data: JSON.stringify({type:"overview", userids: userids, testids: testids }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.success) {
                $("#overview-container").html(res.generatedHtml);
                initFrezen();
            }
            else {
                showMessage("error", res.message);
            }
        }
    });
}
function initAccordition() {
    $("#sidebar").accordion({ heightStyle: "content" });
    $.initCheckboxAllSub({
        container: "#sidebar #menuUsers .nt-ctrl-list",
        all: "#sidebar #menuUsers .nt-clb-header-control input[type=checkbox]",
        sub: ".detail-list .nt-clb-item input[type=checkbox]",
        onchange: function (container) {
            var boxes = $(".detail-list .nt-clb-item input[type=checkbox]", container);
            boxes.each(function (index, ele) {
                var item = $(ele).closest(".nt-clb-item");
                if ($(ele).attr("checked")) {
                    item.addClass("nt-clb-item-sel");
                } else {
                    item.removeClass("nt-clb-item-sel");
                }
            });
            handleOverviewCheckedUsersAndTests();
        }
    });
    $.initCheckboxAllSub({
        container: "#sidebar #menuTests .nt-ctrl-list",
        all: "#sidebar #menuTests .nt-clb-header-control input[type=checkbox]",
        sub: ".detail-list .nt-clb-item input[type=checkbox]",
        onchange: function (container) {
            var boxes = $(".detail-list .nt-clb-item input[type=checkbox]", container);
            boxes.each(function (index, ele) {
                var item = $(ele).closest(".nt-clb-item");
                if ($(ele).attr("checked")) {
                    item.addClass("nt-clb-item-sel");
                } else {
                    item.removeClass("nt-clb-item-sel");
                }
            });
            handleOverviewCheckedUsersAndTests();
        }
    });
}
function initFrezen() {
    $('#overview').gridviewScroll({
        width: 695,
        height: 380,
        railcolor: "#F0F0F0",
        barcolor: "#CDCDCD",
        barhovercolor: "#606060",
        bgcolor: "#F0F0F0",
        varrowtopimg: "Bootstrap/img/arrowvt.png",
        varrowbottomimg: "Bootstrap/img/arrowvb.png",
        harrowleftimg: "Bootstrap/img/arrowhl.png",
        harrowrightimg: "Bootstrap/img/arrowhr.png",
        freezesize: 1,
        arrowsize: 30,
        headerrowcount: 2,
        railsize: 16,
        barsize: 8
    });
}
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
                var resultObj = new Object();
                resultObj.id = obj.id;
                resultObj.title = obj.testTitle;
                resultObj.start = convertJsonDatetoDate(obj.startDateTime);
                resultObj.end= convertJsonDatetoDate(obj.endDateTime);
                if (obj.isOwner) {
                    resultObj.backgroundColor = "#EAEFFA";
                    resultObj.url = "/Tests/NewTest/" + parseInt(obj.id);
                } else {
                    resultObj.backgroundColor = "#FAEAEA",
                    resultObj.url = "/Tests/DoTest/" + parseInt(obj.id);
                }
                resultObj.textColor = "#222222";
                return resultObj;
                
            }).convertJqueryArrayToJSArray();
            var fCalen=$('#calendar').fullCalendar({
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

            var calendar = $("#asmsOverview").kalendae({
                months: 3,
                mode: 'single',
                selected: Kalendae.moment(),
                subscribe: {
                    'change': function (date) {
                        fCalen.fullCalendar('gotoDate',date.year(),date.month(),date.date());
                    }
                }
            });

        } else {
            showMessage("error", res.message);
        }
    });
}
$(function () {
    initCommonValidation();
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
                var ele = res.generatedHtml;
                if (tabcontent && ele) {

                    tabcontent.html(ele);
                    initCalendar();
                    $("[data-toggle=tooltip]").tooltip();
                    initPopover();
                    initFrezen();
                    initAccordition();
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
                        if (obj.running) {
                            $(html).attr("data-original-title", "Open This Test");
                        } else {
                            $(html).attr("data-original-title", "Open This Test - This Test Locked due to compatitle problem");
                        }
                    } else {
                        if (obj.running) {
                            $(html).attr("data-original-title", "Take This Test");
                        } else {
                            $(html).attr("data-original-title", "This Test Locked");
                        }
                    }
                    $(html).tooltip();
                    if (obj.intro) {
                        $(".pop-over", html).popover({
                            placement: "left",
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
            if (item.isCurrentUserOwnTest) {
                window.location.href = "/Tests/NewTest/" + item.id;
            } else {
                if (item.running) {
                    window.location.href = "/Tests/DoTest/" + item.id;
                }
            }
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
                                return { des: obj.DateDescription, title: obj.TestTitle, id: obj.Id, isCurrentUserOwnTest: obj.IsCurrentUserOwnTest, intro: obj.Introduction, running: obj.IsRunning };
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
    //separator
    $("div.nt-ctrl[data-action=control] .nt-dupbtn").live("click", function () {
        var testid = parseInt($(this).attr("test-id"));
        if (!isNaN(testid)) {
            $.post("/Tests/DuplicateTest", { testid: testid }, function (res) {
                if (res.success && res.id) {
                    window.location.href = "/Tests/NewTest/" + res.id;
                } else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    $("div.nt-ctrl[data-action=control] .nt-delbtn").live("click", function () {
    });
    //separator
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
    $(".btn-test-history").live("click", function () {
        var button = $(this);
        var testIdString = button.attr("test-id");
        var testid = parseInt(testIdString);
        $.post("/Tests/ModalTestHistoryPopup", { testid: testid }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalPopupTestHistory").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalPopupTestHistory").replaceWith($(html));
                }
                initReplyAreas();
                $("#modalPopupTestHistory").modal("show");
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
    //separator
    $("#btn-download-all-score").live("click", function () {
        var userids = $("#sidebar #menuUsers .nt-ctrl-list input[type=checkbox]:checked").map(function () { return $(this).attr("user-id"); }).convertJqueryArrayToJSArray();
        var testids = $("#sidebar #menuTests .nt-ctrl-list input[type=checkbox]:checked").map(function () { return $(this).attr("test-id"); }).convertJqueryArrayToJSArray();
        if (userids && testids) {
            var uidsString = "userids=" + userids.join("&userids=");
            var tidsString = "testids=" + testids.join("&testids=");
            window.location.href = "/Tests/AllScoreToExcel?" + uidsString + "&&" + tidsString;
        }
        else {
            showMessage("info", "Please select users and tests");
        }
    });
    //separator
    initClientSorting();
    //separator
    var hub = $.connection.generalHub;
    hub.client.R_studentAndTeacherCommentFeedback = function (tid, generatedHtml) {
        var popTestIdString = $("#modalPopupFeedback #test-id").val();
        var popTestId = parseInt(popTestIdString);
        if (tid && generatedHtml) {
            if (!isNaN(popTestId)) {
                if (tid == popTestId) {
                    var comments = $("#comments[student]");
                    if (comments.length > 0) {
                        var ele = $(generatedHtml);
                        comments.prepend(ele);
                        var articleCount = $("article", comments).length;
                        $("#modalPopupFeedback .comment-count span").html(articleCount);
                    }
                }
            }
        }
    }
    hub.client.R_studentAndTeacherReplyFeedback = function (tid, parentFeedBackId, generatedHtml) {
        var popTestIdString = $("#modalPopupFeedback #test-id").val();
        var popTestId = parseInt(popTestIdString);
        if (tid && parentFeedBackId && generatedHtml) {
            if (!isNaN(popTestId)) {
                if (tid == popTestId) {
                    var article = $("#comments[student] article[parent-id=" + parentFeedBackId + "]");
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
                $.post("/Tests/UserCommentFeedBack", { testid: testid, fbDetail: text, role: "StudentAndTeacher" }, function (res) {
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
            var area = $(".reply-area", container);
            var text = area.val();
            if (text) {
                var place = $(".reply-details", container);
                $.post("/Tests/UserReplyFeedBack", { testid: testid, parentFeedBackId: parentFeedbackID, replyDetail: text, role: "StudentAndTeacher" }, function (res) {
                    if (res.success) {
                        if (area) { area.val(""); }
                    } else {
                        showMessage("error", res.message);
                    }
                });
            }

        });
    });


});