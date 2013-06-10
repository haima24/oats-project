$(function () {

    //Update User Email on Blur Event
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
    //Update UserName on Blur Event
    $("#headerUserName").contentEditable({
        "placeholder": "Enter Required User Name",
        "onBlur": function (element) {
            var username = element.content;
            var userIdString = $("#user-id").val();
            var userId = parseInt(userIdString);
            $.post("/Students/UpdateUserName/", { userId: userId, userName: username }, function (response) {

            });
        },
    });
});
