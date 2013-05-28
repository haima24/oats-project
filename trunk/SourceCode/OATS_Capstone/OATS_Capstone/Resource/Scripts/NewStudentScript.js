$(function () {
    $("#headerUserEmail").contentEditable({
        "placeholder": "Enter Required Email Address",
        "onBlur": function (element) {
            var email = element.content;
            var userIdString = $("#user-id").val();
            var userId = parseInt(userIdString);
            $.post("/Students/UpdateUserEmail", {  userId : userId, userEmail: email }, function (response) {
                // do something with response
              
            });
        },
    });
});