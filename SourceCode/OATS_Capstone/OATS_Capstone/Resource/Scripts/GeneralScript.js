function IsValidProfileFields() {
    // Start validation:
    $.validity.start();

    // Validate fields
    $("#userEmail")
        .require()
        .match("email");
    $("#userPwd,#userConfPwd")
        .match(/^.{8,20}$/, "Passwords must be at least 8 characters.")
        .equal("Passwords do not match.");
    $("#userName,#userPhone,#userCountry").match(/^([\sa-zA-Z0-9_-]{0,120})$/, "Input must be text.");
    var result = $.validity.end();
    return result.valid;
}
function MakeTest(element) {
    var $element = $(element);
    var modal = $element.closest("#askTestTitleModal");
    var $tb = $("#inputTestTitle");
    if (modal && $tb) {
        $.validity.start();
        $tb.require().match(/^(.|\n){0,512}$/, "Input must be text.");;
        var result = $.validity.end();
        if (result.valid) {
            var testTitle = $tb.val();
            $.post("/Tests/MakeTest", { testTitle: testTitle }, function (res) {
                if (res.success) {
                    var id = res.generatedId;
                    if (id) {
                        window.location.href = "/Tests/NewTest/" + id;
                    }
                }
                else {
                    showMessage("error", res.message);
                }
            });
        }
    }
}
$(function () {
    $("#profile-link").live("click", function (ev) {
        $.post("/Users/ProfilePopup", function (res) {
            if (res.success) {
                var popup = $(res.generatedHtml);
                if (!$("#popup-profile").length > 0) {
                    $(popup).modal();
                } else {
                    $("#popup-profile").replaceWith($(popup));
                }
                $("#popup-profile").modal("show");
            } else {
                showMessage("error", res.message);
            }
        });
    });
    $("#userOldPwd").live("blur", function (ev) {
        var mesEle = $("#userOldPwdMsg");
        var text = $(this).val().trim();
        var isMatchOldPass = false;
        var twoText = $("#userPwd,#userConfPwd");
        if (text) {
            $.post("/Users/IsMatchOldPass", { pass: text }, function (res) {
                if (res.success) {
                    if (res.ismatch) {
                        twoText.enable();
                        mesEle.removeClass("hide");
                    } else {
                        twoText.disable();
                        twoText.val("");
                        mesEle.addClass("hide");
                    }
                }
                else {
                    showMessage("error", res.message);
                    twoText.disable();
                    twoText.val("");
                    mesEle.addClass("hide");
                }
            });
        } else {
            twoText.disable();
            twoText.val("");
            mesEle.addClass("hide");
        }
    });
    $("#popup-profile .nt-save-btn").live("click", function (ev) {
        if (IsValidProfileFields()) {
            var profile = new Object();
            profile.Name = $("#userName").val();
            profile.UserMail = $("#userEmail").val();
            profile.Password = $("#userPwd").val();
            profile.UserPhone = $("#userPhone").val();
            profile.UserCountry = $("#userCountry").val();
            $.post("/Users/UpdateProfile", profile, function (res) {
                if (res.success) {
                    showMessage("success", res.message);
                } else {
                    showMessage("error", res.message);
                }
                $("#popup-profile").modal("hide");
            });
        }
    });
    $("#btnMakeTest").live("click", function () {
        MakeTest(this);
    });
    $("#inputTestTitle").live("keydown", function (e) {
        if (e.keyCode == 13) {
            MakeTest(this);
        }
    });
});