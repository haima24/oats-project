function ProfileFields() {
    // Start validation:
    $.validity.start();

    // Validate fields
    $("#userEmail")
        .require()
        .match("email");
    $("#userPwd,#userConfPwd")
        .match(/^.{8,20}$/, "Passwords must be at least 8 characters.")
        .equal("Passwords do not match.");
    $("#userFirstName,#userLastName").match(/^([\sa-zA-Z0-9_-]+){1,30}$/, "Input must be text.");
    var result = $.validity.end();
    return result.valid;
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
        var text = $(this).val().trim();
        var isMatchOldPass = false;
        var twoText=$("#userPwd,#userConfPwd");
        if (text) {
            $.post("/Users/IsMatchOldPass", { pass: text }, function (res) {
                if (res.success) {
                    twoText.enable();
                }
                else {
                    showMessage("error", res.message);
                    twoText.disable();
                }
            });
        } else {
            twoText.disable();
        }
    });
    $("#popup-profile .nt-save-btn").live("click", function (ev) {

    });
});