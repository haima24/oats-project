$(function () {

    $("#submit-btn").live("click", function (ev) {
        $("#checklist .nt-qitem").map(function (i, e) {
            var ans = $(".nt-qans input[type=radio]:checked,[type=checkbox]:checked",e).map(function (index, element) {
                if ($(element).attr("id")) {
                    return $(element).attr("id");
                }
            });
        })
    });

});