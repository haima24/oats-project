var questions;
$(function () {
    $(".tab-event").live("click", function (e) {
        e.preventDefault();
        var link = e.target;
        var action = $(link).attr("href");
        var nav = $(link).closest(".nav");
        var li = $(link).closest("li");
        nav.find("li").removeClass("active");
        li.addClass("active");
        $.post(action, function (res) {
            var i = 0;
            var tabcontent = $("#eventTab");
            if (tabcontent && res.tab) {
                tabcontent.html(res.tab);
            }
        });
    });
    //separator
    $.post("/Tests/QuestionTypes", function (res) {
        if (res) {
            questions = res;
        }
    });
    //separator
    $(".t-question-type").live("click", function (ev) {
        var cur = $(ev.currentTarget);
        var etab = $("#checklist");
        if (questions && etab) {
            if (cur.hasClass("t-question-type-radio")) { etab.append(questions.radio);}
            if (cur.hasClass("t-question-type-multiple")) { etab.append(questions.multiple); }
            if (cur.hasClass("t-question-type-essay")) { etab.append(questions.essay); }
            if (cur.hasClass("t-question-type-short")) { etab.append(questions.shortanswer); }
            if (cur.hasClass("t-question-type-text")) { etab.append(questions.text); }
            if (cur.hasClass("t-question-type-img")) { etab.append(questions.image); }
        }
    });
});