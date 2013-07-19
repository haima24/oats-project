var userid;
function login() {
    $.validity.start();
    $("#signup-login-container input[type='password']")
        .require()
        .match(/^.{8,20}$/, "Passwords must be at least 8 characters.")
        .equal("Passwords do not match.");
    var result = $.validity.end();
    if (result.valid) {
        var pass = $("#detail-pass").val();
        $.post("/Account/UpdateUserPasswordOnRegisterDetail", { userid: userid, password: pass }, function (res) {
            if (res.success) {
                var forward = "/Tests";
                var url = $("#forward").val();
                if (url) { forward = url; }
                window.location.href = forward;
            } else {
                showMessage("error", res.message);
            }
        });
    }
}
$(function () {
    var userIdString = $("#user-id").val();
    userid = parseInt(userIdString);
    $("#detail-login").live("click", function () {
        login();
    });
    $("#signup-login-container").live("keydown", function (ev) {
        if (ev.keyCode == 13) {
            login();
        }
    });
});