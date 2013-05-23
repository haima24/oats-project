var questions;
var emptylist;
var listkey = "listquestion";
var events;
var question_holder = new Array();
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
    var savestatus = $("#savestatus");
    savestatus.show();
    $(".nt-desc", savestatus).html("Saving...");
    var tab = $("#checklist[content-tab=true]");
    if (tab.length > 0) {
        var content = tab.html();
        setLocalStorage(listkey, content);
    }
    $(".nt-desc", savestatus).html("All changes saved.");
}
function loadChanges() {
    var tab = $("#checklist[content-tab=true]");
    if (tab.length > 0 && listkey) {
        var content = getLocalStorage(listkey);
        $("#checklist[content-tab=true]").html(content);
        initDragAndDrop();
        initEditable();
    }
}

function sortByNumberOrLetters() {
    $("#checklist .nt-qnum:not(.nt-qnum-letter)").each(function (i) {
        $(this).html((i + 1) + ". ");
        var radioes = $(this).parent().find("input[type=radio]:not([name])");
        radioes.attr("name", "ans_" + (i + 1));
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
            initImageUploadFacility();
            initEditable();
            saveChanges();
        },
        update: function (ev, ui) {
            var cur = ui.item;
            var etab = $("#checklist");
            if (etab && obj.hasClass("t-question-type") && questions) {
                if (cur.hasClass("t-question-type-radio")) { var obj = $(questions.radio); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : obj.replaceWith(obj); }
                if (cur.hasClass("t-question-type-multiple")) var obj = $(questions.multiple); obj.uniqueId(); { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : obj.replaceWith(obj); }
                if (cur.hasClass("t-question-type-essay")) { var obj = $(questions.essay); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : obj.replaceWith(obj); }
                if (cur.hasClass("t-question-type-short")) { var obj = $(questions.shortanswer); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.shortanswer) : obj.replaceWith(obj); }
                if (cur.hasClass("t-question-type-text")) { var obj = $(questions.text); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : obj.replaceWith(obj); }
                if (cur.hasClass("t-question-type-img")) { var obj = $(questions.image); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : obj.replaceWith(obj); }
                sortByNumberOrLetters();
                initImageUploadFacility();
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

function initImageUploadFacility() {
    if (questions && questions.imagepreview) {
        $(".nt-qimg-ph.nt-clickable").dropzone({
            url: "/Tests/UploadFiles",
            paramName: "files",
            previewTemplate: questions.imagepreview,
            uploadprogress: function (ev) {
                $(this.element).hide();
                $(this.element).parent().find(".nt-qimg-upload").show();
            },
            success: function (file, res) {
            },
            complete: function (ev) {
                $(this.element).parent().find(".nt-qimg-upload").hide();
            },
        });
    }
}

$(function () {
    emptylist = $(".nt-empty-list-ph");



    initCalendar();
    initImageUploadFacility();
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
            if (cur.hasClass("t-question-type-radio")) { var obj = $(questions.radio); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-multiple")) var obj = $(questions.multiple); obj.uniqueId(); { $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-essay")) { var obj = $(questions.essay); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-short")) { var obj = $(questions.shortanswer); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.shortanswer) : etab.append(obj); }
            if (cur.hasClass("t-question-type-text")) { var obj = $(questions.text); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-img")) { var obj = $(questions.image); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            sortByNumberOrLetters();
            initEditable();
            initImageUploadFacility();
            saveChanges();
        }
    });
    //separator
    $(".bt-duplicate").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        var etab = $("#checklist");
        if (content && etab) {
            content.uniqueId();
            etab.append(content.prop("outerHTML"))
            initImageUploadFacility();
            initEditable();
            saveChanges();
        }
    });
    //separator
    $(".nt-qhctrls .bt-delete").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        content.remove()
        if ($("#checklist").children().length == 0) {
            $("#checklist").html(emptylist);
        }
        sortByNumberOrLetters();
        saveChanges();
    });
    //separator
    $(".nt-btn-text.nt-qansadd").live("click", function (ev) {
        var parent = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        var anstemplate = $(".nt-qans.nt-qans-edit", parent).clone().get(0);
        $(".nt-qansdesc.nt-qedit", anstemplate).html("");
        $(".nt-qanscont", parent).append(anstemplate);
        //when add an answer, show delete button on answer line

        if ($(".nt-qans.nt-qans-edit", parent).length > 2) {
            $(".nt-qansctrls .bt-delete", parent).show();
        } else {
            $(".nt-qansctrls .bt-delete", parent).hide();
        }
        initEditable();
        saveChanges();
    });
    //separator
    $(".nt-qanscont .nt-qansctrls .bt-delete").live("click", function () {
        var parent = $(this).closest(".nt-qitem.nt-qitem-edit");
        var ansline = $(this).closest(".nt-qans.nt-qans-edit");
        ansline.remove();
        if ($(".nt-qans.nt-qans-edit", parent).length > 2) {
            $(".nt-qansctrls .bt-delete", parent).show();
        } else {
            $(".nt-qansctrls .bt-delete", parent).hide();
        }
        saveChanges();
    });
    //separator
    $(".nt-qtype-sel-predef,.nt-qtype-sel").live("change", function (ev) {
        var questiontype = $(this).val();
        var itemParent = $(this).closest(".nt-qitem.nt-qitem-edit");
        var itemid = itemParent.attr("id");
        if ($(this).hasClass("nt-qtype-sel-predef")) {
            //save
            var qaholder = itemParent.map(function () {
                var question = $(".nt-qtext.nt-qedit", itemParent).html();
                var answers = $(".nt-qansdesc.nt-qedit", itemParent).map(function (index, ans) {
                    return { answer: $(ans).html() }
                });
                return { itemid: itemid, question: question, answers: answers };
            });
            question_holder[qaholder.attr("itemid")] = qaholder;
        }
        
        switch (questiontype) {
            case "radio":
                var ques = $(questions.radio);

                var savedObj = question_holder[itemid];
                //question
                $(".nt-qtext.nt-qedit", ques).html(savedObj.attr("question"));
                //answers
                var ansholder = $(".nt-qanscont", ques);
                while ($(".nt-qans.nt-qans-edit", ansholder).length < savedObj.attr("answers").length)
                {
                    var anstemplate = $(".nt-qans.nt-qans-edit", ansholder).clone().get(0);
                    ansholder.append(anstemplate);
                }
                savedObj.attr("answers").each(function (index,o) {
                    var ele = $(".nt-qans.nt-qans-edit", ansholder).get(index);
                    if (ele) { $(".nt-qansdesc.nt-qedit", ele).html(o.answer) };
                });

                var quesid=itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                itemParent.replaceWith(ques);
                break;
            case "multiple":
                var ques = $(questions.multiple);

                var savedObj = question_holder[itemid];
                //question
                $(".nt-qtext.nt-qedit", ques).html(savedObj.attr("question"));
                //answers
                var ansholder = $(".nt-qanscont", ques);
                while ($(".nt-qans.nt-qans-edit", ansholder).length < savedObj.attr("answers").length) {
                    var anstemplate = $(".nt-qans.nt-qans-edit", ansholder).clone().get(0);
                    ansholder.append(anstemplate);
                }
                savedObj.attr("answers").each(function (index, o) {
                    var ele = $(".nt-qans.nt-qans-edit", ansholder).get(index);
                    if (ele) { $(".nt-qansdesc.nt-qedit",ele).html(o.answer) };
                });

                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                itemParent.replaceWith(ques);
                break;
            case "essay":
                var ques = $(questions.essay);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                itemParent.replaceWith(ques);
                break;
            case "shortanswer":
                var ques = $(questions.shortanswer);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                itemParent.replaceWith(ques);
                break;
            case "text":
                var ques = $(questions.text);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                itemParent.replaceWith(ques);
                break;
            case "img":
                var ques = $(questions.image);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                itemParent.replaceWith(ques);
                break;
            default:
                break;
        }
        sortByNumberOrLetters();
        initImageUploadFacility();
        initEditable();
        saveChanges();
    });
});