$(function () {
    $("#headerUserEmail").contentEditable({
        "placeholder": "Enter Required Email Address",
        "onBlur": function (element) {
            var email = element.content;
            $.post("/Teachers/NewTeacherByEmail", { "email": email }, function (response) {
                // do something with response

            });
        },
    });
})