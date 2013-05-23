var questions;
var emptylist;
var listkey = "listquestion";
var events;
function initCalendar() {

    $.post("/Tests/TestCalendarObjectResult", function (res) {
        events = res.map(function (obj) {
            return {
                id: obj.id,
                title: obj.testTitle,
                start: convertJsonDatetoDate(obj.startDateTime),
                end: convertJsonDatetoDate(obj.endDateTime)
            };
        });
        $('#calendar').fullCalendar({
            theme: true,
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            editable: true,
            events: events
        });

    });

    $("#asmsOverview").kalendae({
        months: 3,
        mode: 'single',
        selected: Kalendae.moment().subtract({ M: 1 })
    });
}

function saveChanges() {
    var tab = $("#checklist[content-tab=true]");
    if (tab.length > 0) {
        var content = tab.html();
        setLocalStorage(listkey, content);
    }
}
function loadChanges() {
    var tab = $("#checklist[content-tab=true]");
    if (tab.length > 0 && listkey) {
        var content = getLocalStorage(listkey);
        $("#checklist[content-tab=true]").html(content);
        initDragAndDrop();
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
function initDragAndDrop() {
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
            initEditable();
            saveChanges();
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
                initEditable();
                saveChanges();
                //obj.remove();
            }
        }
    });
    //separator


}
function initEditable() {

    $(".nt-qans.nt-qans-edit .nt-qansdesc.nt-qedit").contentEditable({
        "placeholder": "<i>Enter Answer</i>",
        "onBlur": function (element) {
        },
    });
    $(".nt-qtext.nt-qedit").contentEditable({
        "placeholder": "<i>Enter Question</i>",
        "onBlur": function (element) {
        },
    });
}

$(function () {
    emptylist = $(".nt-empty-list-ph");

    

    initCalendar();
    loadChanges();

    $(".nt-qans.nt-qans-edit .nt-qansdesc.nt-qedit").live("mousedown", function (ev) {
        this.focus();
    });
    $(".nt-qtext.nt-qedit").live("mousedown", function (ev) {
        this.focus();
    });


    //separator
    $("#test-title").contentEditable({
        "placeholder": "<i>Enter Test Title</i>",
        "onBlur": function (element) {

        },
    });
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

                initCalendar();
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
            initEditable();
            saveChanges();
        }
    });
    //separator
    $(".bt-duplicate").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        var etab = $("#checklist");
        if (content && etab) {
            etab.append(content.prop("outerHTML"))
            initEditable();
            saveChanges();
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
        saveChanges();
    });
    //separator
    initDragAndDrop();
});