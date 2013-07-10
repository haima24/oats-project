$(function () {

    $("#submit-btn").live("click", function (ev) {
        $("#checklist .nt-qitem").map(function (i, e) {
            var ans = $(".nt-qans input[type=radio]:checked,[type=checkbox]:checked", e).map(function (index, element) {
                if ($(element).attr("id")) {
                    return $(element).attr("id");
                }
            });
        });

        var userInTest = new Object();
        userInTest.TestID = parseInt($("#test-id").val());


        userInTest.UserInTestDetails = $("#checklist .nt-qitem").map(function (i, e) {
            var userInTestDetail = new Object();
            userInTestDetail.QuestionID = parseInt($(e).attr("question-id"));
            var str = "";
            $(".nt-qans input[type=radio]:checked,[type=checkbox]:checked", e).each(function (index, element) {

                if ($(element).attr("id")) {
                    if (index == 0) {
                        str += $(element).attr("id");
                    } else {
                        str += "," + $(element).attr("id");
                    }
                }
            });
            userInTestDetail.AnswerIDs = str;
            userInTestDetail.AnswerContent = $(".nt-qrespinput", e).val();

            return userInTestDetail;
        }).convertJqueryArrayToJSArray();

        $.ajax({
            type: "POST",
            url: "/Tests/SubmitTest",
            data: JSON.stringify(userInTest),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.success) {
                    showCountDownMessage("info", res.message,"Redirect to Homepage", function () {
                        window.location = "/Tests";
                    });
                } else {
                    showMessage("error", res.message);
                }
            }
        });

    });



});