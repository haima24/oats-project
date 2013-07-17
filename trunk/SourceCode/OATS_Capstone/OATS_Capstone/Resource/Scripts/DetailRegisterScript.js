var userid;
function validateDetailRegister() {
    $.validity.start();
    $("#signup-login-container input[type='password']")
        .require()
        .match(/^.{8,20}$/, "Passwords must be at least 8 characters.")
        .equal("Passwords do not match.");
    $("#signup-login-container input:not(#reg-email)").match(/^([\sa-zA-Z0-9_-]+){1,30}$/, "Input must be text.");
    var result = $.validity.end();
    return result.valid;
}
$(function () {
    var userIdString = $("#user-id").val();
    userid = parseInt(userid);
    $("#detail-login").live("click", function () {
        if (validateDetailRegister()) {
            var pass = $("#detail-pass").val();
            $.post("/Account/UpdateUserPasswordOnRegisterDetail", { userid: userid, password: pass }, function (res) {
                if (res.success) {
                    window.location.href = "/Tests";
                } else {
                    showMessage("error", res.message);
                }
            });
        }
    });
});