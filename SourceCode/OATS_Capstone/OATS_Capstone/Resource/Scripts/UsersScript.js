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
        var users = new Array();
        users.push({ Name: tbName.val(), UserMail: tbEmail.val() });
        $.ajax({
            type: "POST",
            url: "/Users/CreateUsers",
            data: JSON.stringify({ users: users, type: "Create" }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.success) {
                    var list = $("#emailInputModal .nt-clb-list");
                    $(res.resultlist).each(function (i, e) {
                        if ($(".nt-empty", list).length > 0) {
                            list.html(e);
                        } else {
                            list.append(e);
                        }
                        list.scrollEnd();
                    });
                    showMessage("info", res.message);
                } else {
                    showMessage("error", res.message);
                }
            }
        });
    }
}
function initImportArea() {
    var handleImportUsers = function (pastedText) {
        if (pastedText) {
            var couples = pastedText.split(/\s*[\n;,](?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)\s*/);
            var users = $(couples).map(function (i, e) {
                var couples = e.split(/\s+/);
                if (couples.length == 2) {
                    var name = couples[0].replace(/"/g, "");
                    var mail = couples[1].replace(/</g, "").replace(/>/g, "");
                    return { Name: name, UserMail: mail };
                }
            }).convertJqueryArrayToJSArray();
            var role = $("#created-role").val();
            if (users&&role) {
                $.ajax({
                    type: "POST",
                    url: "/Users/CreateUsers",
                    data: JSON.stringify({ users: users, type: "Import", role: role }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (res) {
                        if (res.success) {
                            var html = res.generatedHtml;
                            if (!$("#emailInputModal").length > 0) {
                                $(html).modal();
                            } else {
                                $("#emailInputModal").html($(html).html());
                            }
                            $("#emailInputModal").modal("show");
                            showMessage("info", res.message);
                        } else {
                            showMessage("error", res.message);
                        }
                    }
                });
            }
        }
    };
    $("textarea.nt-dnd-example-text").placeholder();
    $("textarea.nt-dnd-example-text").live("paste", function (ev) {
        var $tb = $(this);
        setTimeout(function () {
            var textarea = $tb;
            if (textarea) {
                var pastedText = textarea.val();
                handleImportUsers(pastedText);
            }
        }, 100);
    });
    $("textarea.nt-dnd-example-text").filedrop({
        success: function (text) {
            handleImportUsers(text);
        },
        error: function (msg) {
            showMessage("error", msg);
        }
    });
}
$(function () {
    var curIdString = $("#current-user-id").val();
    currentUserId = parseInt(curIdString);
    //separator

    //separator
    $("#makeStudent").live("click", function () {
        initPopup("Student");
    });
    $("#makeTeacher").live("click", function () {
        initPopup("Teacher");
    });
    //separator
    initImportArea();
    $("#emailInputModal .nt-clb-item-control button.btn-remove-invite").live("click", function () {
        var btn = $(this);
        var userid = parseInt(btn.attr("user-id"));
        if (!isNaN(userid)) {
            $.post("/Users/RemoveNonRegisteredUser", { userid: userid }, function (res) {
                if (res.success) {
                    var item = $(btn).closest(".nt-clb-item");
                    item.fadeOut("slow", function () { $(this).remove(); });
                }
                else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    $("#emailInputModal .btn-remove-assign-test").live("click", function () {
        var box=$("#emailInputModal .nt-search-box-result");
        box.fadeOut("fast",function(){
            var span = $("span.assign-test[test-id]",this);
            span.html("");
            span.removeAttr("test-id");
        });
        
    });
    $("#emailInputModal #emailInputOk").live("click", function () {
        var modal = $("#emailInputModal");
        var holder = $("span.assign-test[test-id]", modal);
        if (holder.length > 0) {
            var testid = parseInt(holder.attr("test-id"));
            var checkedIds = $(".nt-clb-item[user-id]", modal).map(function (i, e) {
                var id = parseInt($(e).attr("user-id"));
                if (id) {
                    return id;
                }
            }).convertJqueryArrayToJSArray();
            var role = $("#role", modal).val();
            if (!isNaN(testid)&&checkedIds&&role) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/AddUserToInvitationTest",
                    data: JSON.stringify({ testid: testid, count: checkedIds.length, userids: checkedIds, role: role }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (res) {
                        if (res.success) {
                            window.location.href = "/Tests/NewTest/" + testid + "?tab=Invitation";
                        } else {
                            showMessage("error", res.message);
                        }
                    }
                });
            }
        }
        modal.modal("hide");
    });
    $("#emailInputModal").live("shown", function () {
        $("#emailInputModal input[type=text].nt-search-input").oatsSearch({
            hideOnSelect:true,
            select: function (item) {
                if (item.id) {
                    var searchBox = $("#emailInputModal .nt-search-box-result");
                    searchBox.show();
                    var spanHolder = $("span.assign-test", searchBox);
                    spanHolder.html(item.title || "");
                    spanHolder.attr("test-id", item.id);
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
                                if (obj.IsCurrentUserOwnTest && obj.TestTitle && obj.TestTitle != "") {
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
    });
    $("#emailInputModal #nameInput,#emailInputModal #emailInput").live("keydown", function (ev) {
        if (ev.keyCode == 13) {
            createUser();
        }
    });
    $("#emailInputModal .nt-btn-add-input").live("click", function () {
        createUser();
    });
    //separator
    var hub = $.connection.generalHub;
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
        
    });
});