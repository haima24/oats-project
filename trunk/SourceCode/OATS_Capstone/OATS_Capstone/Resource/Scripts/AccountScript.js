function ValidateSignupFields() {
    // Start validation:
    $.validity.start();

    // Validate fields
    $("#reg-email")
        .require()
        .match("email");

    // Validate password strength & match
    $("#signup-container input[type='password']")
        .require()
        .match(/^.{8,20}$/, "Passwords must be at least 8 characters.")
        .equal("Passwords do not match.");
    $("#signup-container input").match(/^([a-zA-Z0-9_-]+){1,30}$/, "Input must be text.");
    $("#reg-understand").checkboxChecked("Agree terms and conditions");
    // All of the validator methods have been called:
    // End the validation session:
    var result = $.validity.end();

    // Return whether it's okay to proceed with the Ajax:
    return result.valid;
}
function checkLogin(email, pass, onsuccess) {
    $.post("/Account/Login", { email: email, password: pass }, function (res) {
        if (res.success) {
            if (onsuccess && typeof (onsuccess) === "function") {
                onsuccess();
            }
        } else {
            showMessage("error", res.message);
        }
    });
    
}
$(function () {
    $("#submit-btn").live("click", function (ev) {
    });
    $("#login-btn").live("click", function (ev) {
        var email = $("#email").val();
        var pass = $("#password").val();
        checkLogin(email, pass, function () {
            window.location.href = "/Tests";
        });
    });
});