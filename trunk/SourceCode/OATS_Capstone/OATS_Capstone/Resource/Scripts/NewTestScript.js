var questions;
var emptylist;
var listkey="listquestion";

function saveChanges() {
    var tab=$("#checklist[content-tab=true]");
    if (tab.length>0) {
        var content = tab.html();
        setLocalStorage(listkey, content);
    }
}
function loadChanges() {
    var tab = $("#checklist[content-tab=true]");
    if (tab.length>0 && listkey) {
        var content = getLocalStorage(listkey);
        $("#checklist[content-tab=true]").html(content);
        initFunctionalities();
    }
}

function sortByNumberOrLetters() {
    $("#checklist .nt-qnum:not(.nt-qnum-letter)").each(function (i) {
        $(this).html((i + 1) + ". ");
    });
    $("#checklist .nt-qnum.nt-qnum-letter").each(function (i) {
        $(this).html(String.fromCharCode(65 + i) + ". ");
    });
}
function initFunctionalities() {
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
        stop: function (ev, ui) {
            sortByNumberOrLetters();
        },
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
                sortByNumberOrLetters();
                //obj.remove();
            }
        }
    });
    //separator
    $("#test-title").contentEditable({
        "placeholder": "Enter Test Title",
        "onBlur": function (element) {
            var email = element.content;
            $.post("/Students/NewStudentByEmail", { "email": email }, function (response) {
                // do something with response

            });
        },
    });
    //separator
    //$(".nt-qans.nt-qans-edit .nt-qansdesc.nt-qedit").contentEditable({
    //    "placeholder": "Enter Test Title",
    //    "onBlur": function (element) {
    //        var email = element.content;
            
    //    },
    //});
}

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

                saveChanges();
                tabcontent.html(res.tab);
                loadChanges();

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
            sortByNumberOrLetters();
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
        sortByNumberOrLetters();
    });
    //separator
    initFunctionalities();
});