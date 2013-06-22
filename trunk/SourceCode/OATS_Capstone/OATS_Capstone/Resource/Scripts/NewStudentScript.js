$(function () {

    //Update User Email on Blur Event
    $("#headerUserEmail").contentEditable({
        "placeholder": "Enter Required Email Address",
        "onBlur": function (element) {
            var email = element.content;
            var userIdString = $("#user-id").val();
            var userId = parseInt(userIdString);
            $.post("/Users/UpdateUserEmail", { userId: userId, userEmail: email }, function (res) {
                // do something with response
                if (!res.success) {
                    showMessage("error", res.message);
                }
            });
        },
    });
    //Update UserName on Blur Event
    $("#headerUserName").contentEditable({
        "placeholder": "Enter Required User Name",
        "onBlur": function (element) {
            var username = element.content;
            var userIdString = $("#user-id").val();
            var userId = parseInt(userIdString);
            $.post("/Users/UpdateUserName/", { userId: userId, userName: username }, function (res) {
                if (!res.success) {
                    showMessage("error", res.message);
                }
            });
        },
    });
});
