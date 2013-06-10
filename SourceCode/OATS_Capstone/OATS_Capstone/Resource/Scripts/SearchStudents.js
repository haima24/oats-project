$(function () {
    $.post("/Students/StudentsSearch", function (res) {
        if (res) {
            var source = $(res.listStudentsSearch).map(function (index, obj) {
                if(obj.FirstName||obj.LastName)
                {
                    var fName = obj.FirstName ? obj.FirstName : "";
                    var lName = obj.LastName?obj.LastName:"";
                    return { label: fName + " " + lName, value: fName + " " + lName, id: obj.UserID };
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
                    window.location.href = "/Students/NewStudent/" + ui.item.id;
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
    });


    var userid = parseInt($('#user-id').val());
    if ($("#container .nt-search-input ").length > 0) {
        $("#container .nt-search-input ").autocomplete({
            minLength: 0,
            source: function (req, res) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/TestsAssignStudentSearch",
                    data: JSON.stringify({ userid: userid, letter: req.term }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        if (r.success && r.listTests) {
                            var result = $(r.listTests).map(function (index, element) {
                                return { label: element.TestTitle, value: element.Id, id: element.Id };
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
                $.post('/Students/AssignTestToStudent', data, function (response) {
                    if (response.generatedHtml) {
                        $("#asmsList").html(response.generatedHtml);
                    }
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
            if (res.success&& res.generatedHtml) {
                $("#asmsList").html(res.generatedHtml);
            }
            else {
                showMessage("error",res.message);
            }

        });
    });


});