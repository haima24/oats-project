$(function () {
    searchUsers(".navbar-search .nt-search-input", function (id) {
        window.location = "/Students/NewStudent/" + id;
    });
    var userid = parseInt($('#user-id').val());
    if ($("#container .nt-search-input ").length > 0) {
        $("#container .nt-search-input ").autocomplete({
            minLength: 0,
            source: function (req, res) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/TestsAssignUserSearch",
                    data: JSON.stringify({ userid: userid, letter: req.term }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        if (r.success) {
                            var result = $(r.resultlist).map(function (index, element) {
                                if (element && element.TestTitle && element.Id) {
                                    return { label: element.TestTitle, value: element.TestTitle, id: element.Id };
                                }
                            }).convertJqueryArrayToJSArray();
                            res(result);
                        } else {
                            showMessage("error", r.message);
                        }

                    }

                });
                //res([{ label: 'Example', value: 'ex' }]);
            },

            select: function (ev, ui) {
                var data = {
                    testID: parseInt(ui.item.id),
                    userID: userid
                };
                $.post('/Students/AssignTestToStudent', data, function (res) {
                    if (res.success) {
                        if (res.generatedHtml) {
                            $("#asmsList").html(res.generatedHtml);
                        }
                    } else { showMessage("error", res.message);}
                });
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
