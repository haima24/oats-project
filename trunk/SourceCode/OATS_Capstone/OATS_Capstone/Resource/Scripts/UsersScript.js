var currentUserId;
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
    var tbEmail = $("#emailInput");
    var tbName = $("#nameInput");
    $.validity.start();
    $.validity.settings.position = "top";
    tbName.require();
    tbEmail.require().match("email");
    $.validity.settings.position = "left";
    var result = $.validity.end();
    if (result.valid) {
        var name = tbName.val();
        var email = tbEmail.val();
        $.post("/Users/MakeUser", { name:name,email: email }, function (res) {
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
    var curIdString = $("#current-user-id").val();
    currentUserId = parseInt(curIdString);
    //separator
    $("#makeStudent").live("click", function () {
        initPopup("Student");
    });
    $("#makeTeacher").live("click", function () {
        initPopup("Teacher");
    });
    //separator
    $(".nt-dnd-example-text").live("click", function () {
        var $this = $(this);
        $this.addClass("active");
    });
    $(".nt-dnd-example-text").live("paste", function (e) {

    });
    $(".nt-dnd-example-text").clickout({
        callback: function (e,self) {
            self.removeClass("active");
        }
    });
    //separator
    var hub = $.connection.generalHub;
    hub.client.R_notifyNewUserCallBack = function (userid, generatedId, mail, isSuccess) {
        if (!isNaN(currentUserId) && generatedId && userid && mail && typeof (isSuccess) != "undefined") {
            if (currentUserId == userid) {
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
    hub.client.R_AcknowledgeEmailCallback = function (uid, initMailCount, sentCount, unSentCount, listSent) {
        if (uid && uid == currentUserId) {
            if (typeof (initMailCount) != "undefined" && typeof (sentCount) != "undefined" && typeof (unSentCount) != "undefined" && typeof (listSent) != "undefined") {
                var message = "";
                var type = "";
                if (unSentCount) {
                    type = "error";
                    message = "Unable to send all emails from invitation.</br>" + "Total : " + initMailCount + "</br>" + sentCount + " was sent.</br>" + unSentCount + " was Un-sent.";
                } else {
                    type = "info";
                    message = "Sent " + sentCount + " invitation emails.";
                }
                //re-render
                $(listSent).each(function () {
                    var id = this;
                    if (id) {
                        var r = $("#asmsList .nt-list-row[invitation-id=" + id + "]");
                        if (r.length > 0) {
                            r.fadeOut("fast", function () {
                                var row = $(this);
                                var status = $("span.label", row);
                                status.removeClass("label-important").addClass("label-info");
                                status.html("Mail Sent");
                                row.fadeIn("fast");
                            });
                        }
                    }
                });
                showMessage(type, message);
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
});