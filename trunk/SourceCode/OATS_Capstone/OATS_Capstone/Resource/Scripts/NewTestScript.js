var questions;
var emptylist;
$(function () {
    emptylist = $(".nt-empty-list-ph");
    //separator
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

                //rebind accordition
                $("#sidebar").accordion();
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
            if (cur.hasClass("t-question-type-radio")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.radio) : etab.append(questions.radio); }
            if (cur.hasClass("t-question-type-multiple")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.multiple) : etab.append(questions.multiple); }
            if (cur.hasClass("t-question-type-essay")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.essay) : etab.append(questions.essay); }
            if (cur.hasClass("t-question-type-short")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.shortanswer) : etab.append(questions.shortanswer); }
            if (cur.hasClass("t-question-type-text")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.text) : etab.append(questions.text); }
            if (cur.hasClass("t-question-type-img")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.image) : etab.append(questions.image); }
        }
    });
    //separator
    $(".bt-duplicate").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        var etab = $("#checklist");
        if (content && etab) {
            etab.append(content.prop("outerHTML"))
        }
    });
    //separator
    $(".bt-delete").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        content.remove()
        if ($("#checklist").children().length == 0) {
            $("#checklist").html(emptylist);
        }
    });
    //separator
    $(".t-question-type").draggable({
        connectToSortable: "#checklist",
        helper: "clone",
        revert: "invalid"
    });
    $("#checklist").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-qitem',
        placeholder: 'highlight',
        update: function (ev, ui) {
            var obj = ui.item;
            var etab = $("#checklist");
            if (etab && obj.hasClass("t-question-type") && questions) {
                if (obj.hasClass("t-question-type-radio")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.radio) : obj.replaceWith(questions.radio); }
                if (obj.hasClass("t-question-type-multiple")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.multiple) : obj.replaceWith(questions.multiple); }
                if (obj.hasClass("t-question-type-essay")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.essay) : obj.replaceWith(questions.essay); }
                if (obj.hasClass("t-question-type-short")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.shortanswer) : obj.replaceWith(questions.shortanswer); }
                if (obj.hasClass("t-question-type-text")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.text) : obj.replaceWith(questions.text); }
                if (obj.hasClass("t-question-type-img")) { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.image) : obj.replaceWith(questions.image); }
                //obj.remove();
            }
        }
    });
    //separator
});