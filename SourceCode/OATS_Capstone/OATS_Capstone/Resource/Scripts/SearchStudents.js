$(function () {
    searchUsers(".navbar-search .nt-search-input", function (id) {
        window.location.href = "/Students/NewStudent/" + id;
    });
    var userid = parseInt($('#user-id').val());

    $("#container input[type=text].nt-search-input").oatsSearch({
        rendermenu: function (items) {
            $(items).each(function () {
                var html = this.key;
                var obj = this.value;
                if (html && obj) {
                    $(html).attr("data-toggle", "tooltip");
                    if (obj.isCurrentUserOwnTest) {
                        if (obj.running) {
                            $(html).attr("data-original-title", "Assign This Test");
                        } else {
                            $(html).attr("data-original-title", "Assign This Test - This Test Locked due to compatitle problem");
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
        select: function (item,element) {
            if (item.isCurrentUserOwnTest && item.running) {
                var data = {
                    testID: parseInt(item.id),
                    userID: userid
                };
                $.post('/Students/AssignTestToStudent', data, function (res) {
                    if (res.success) {
                        if (res.generatedHtml) {
                            $("#asmsList").html(res.generatedHtml);
                            $(element).fadeOut("slow", function () { $(this).remove(); });
                        }
                    } else { showMessage("error", res.message); }
                });
            }
        },
        source: function (req, res, addedTagIds) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsAssignUserSearch",
                data: JSON.stringify({ userid: userid, tagids: addedTagIds, letter: req }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.IsCurrentUserOwnTest&&obj.TestTitle && obj.TestTitle != "") {
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

    $(".nt-unassignbtn").live("click", function (ev) {
        var test_id = $(this).closest(".nt-list-row").attr("test-id");
        $.post("/Students/UnassignTest", { userId: userid, testId: parseInt(test_id) }, function (res) {
            if (res.success && res.generatedHtml) {
                $("#asmsList").html(res.generatedHtml);
            }
            else {
                showMessage("error", res.message);
            }

        });
    });


});
