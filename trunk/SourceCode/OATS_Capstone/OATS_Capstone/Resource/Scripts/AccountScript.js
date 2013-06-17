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
    $("#signup-container input:not(#reg-email)").match(/^([\sa-zA-Z0-9_-]+){1,30}$/, "Input must be text.");
    $("#reg-understand").checkboxChecked("Agree terms and conditions");
    // All of the validator methods have been called:
    // End the validation session:
    var result = $.validity.end();

    // Return whether it's okay to proceed with the Ajax:
    return result.valid;
}
function checkLogin(email, pass,remembered, onsuccess) {
    $.post("/Account/Login", { email: email, password: pass, remembered: remembered }, function (res) {
        if (res.success) {
            if (onsuccess && typeof (onsuccess) === "function") {
                onsuccess();
            }
        } else {
            showMessage("error", res.message);
        }
    });

}
function login() {
    var email = $("#email").val();
    var pass = $("#password").val();
    var checkedElement = $("#test-holder input[type=radio][name=test_holder_radio]:checked");
    var remembered = $("#remember-checkbox").attr("checked") ? true : false;
    checkLogin(email, pass,remembered, function () {
        window.location.href = "/Tests";
    });
}
$(function () {
   
    $("#submit-btn").live("click", function (ev) {
        if (ValidateSignupFields()) {
            var obj = new Object();
            obj.FirstName = $("#reg-firstname").val();
            obj.LastName = $("#reg-lastname").val();
            obj.UserMail = $("#reg-email").val();
            obj.Password = $("#reg-password").val();
            obj.UserCountry = $("#reg-country").val();
            obj.UserPhone = $("#reg-phone").val();
            $.ajax({
                type: "POST",
                url: "/Account/SignUp",
                data: JSON.stringify(obj),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (res) {
                    if (!res.success) {
                        showMessage("error", res.message);
                    } else {
                        showMessage("success", res.message);
                        if (res.generatedHtml) { $("#signup-container").html(res.generatedHtml); }
                    }
                }
            });
        }
    });
    $("#login-btn").live("click", function (ev) {
        login();
    });
    $("#login-container").live("keypress", function (ev) {
        if (ev.keyCode == 13) {
            login();
        }
    });
   
});