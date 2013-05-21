$(function () {
    $("#headerUserEmail").contentEditable({
        "placeholder": "Enter Required Email Address",
        "onBlur": function (element) {
            var email = element.content;
            $.post("/Students/NewStudentByEmail", { "email": email }, function (response) {
                // do something with response

            });
        },
    });
});