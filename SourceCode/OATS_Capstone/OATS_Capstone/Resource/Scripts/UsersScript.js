function searchUsers(selector, onselect, onvalidatesource) {
    $(selector).autocomplete({
        minLength: 0,
        focus: function (ev, ui) {
            $(selector).val(ui.item.label);
            return false;
        },
        select: function (ev, ui) {
            if (onselect && typeof (onselect) === "function") {
                onselect(ui.item.id);
            }
            return false;
        },
        source: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Users/UsersSearch",
                data: JSON.stringify({ term: req.term }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.Name) {
                                return { label: obj.Name, value: obj.Name, id: obj.UserID, rolename: obj.RoleName };
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
}
function initPopup(role) {
    $.post("/Users/EmailInput", { role: role }, function (res) {
        var html = res.generatedHtml;
        if (!$("#emailInputModal").length > 0) {
            $(html).modal();
        } else {
            $("#emailInputModal").html($(html).html());
        }
        $("#emailInputModal").modal("show");
    });
}
function createUser() {
    var modal = $("#emailInputModal");
    var tb = $("#emailInput");
    $.validity.start();
    tb.require().match("email");
    var result = $.validity.end();
    if (result.valid) {
        var email = tb.val();
        $.post("/Users/MakeUser", { email: email }, function (res) {
            if (res.success && res.generatedId) {
                modal.modal("hide");
                showMessage("Sending email")
            } else {
                showMessage("error", res.message);
            }
        });
    }
}
$(function () {
    $("#makeStudent").live("click", function () {
        initPopup("Student");
    });
    $("#makeTeacher").live("click", function () {
        initPopup("Teacher");
    });
    var hub = $.connection.generalHub;
    hub.client.R_notifyNewUserCallBack = function (userid, generatedId, mail, isSuccess) {
        var curIdString = $("#current-user-id").val();
        var curId = parseInt(curIdString);
        if (!isNaN(curId) && generatedId && userid && mail && typeof (isSuccess) != "undefined") {
            if (curId == userid) {
                if (isSuccess) {
                    showMessage("success", "Success on sending email to: " + mail);
                } else {
                    showMessage("error", "Failed on sending email to: " + mail);
                }
                var loc = "/Tests";
                var role = $("#emailInputModal #role").val();
                if (role) {
                    switch (role) {
                        case "Student":
                            loc = "/Students/NewStudent/" + generatedId;
                            break;
                        case "Teacher":
                            loc = "/Teachers/NewTeacher/" + generatedId;
                            break;
                        default:
                            break;
                    }
                }
                setTimeout(function () {
                    window.location.href = loc;
                }, 3000);
            }
        }
    }
    $.connection.hub.start().done(function () {
        $("#emailInputModal").live("keydown", function (ev) {
            if (ev.keyCode == 13) {
                createUser();
            }
        });
        $("#emailInputOk").live("click", function () {
            createUser();
        });
    });

    //separator
    
});