var questions;
var emptylist;
var listkey = "listquestion";
var events;
var question_holder = new Array();


function loadChanges() {
    //var tab = $("#checklist[content-tab=true]");
    //if (tab.length > 0 && listkey) {
    //    var content = getLocalStorage(listkey);
    //    $("#checklist[content-tab=true]").html(content);
    //    initDragAndDrop();
    //    initEditable();
    //}
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
function resortInDb() {
    var questions = $("[question-id]").map(function (index, item) {
        var question = new Object();
        question.QuestionID = parseInt($(item).attr("question-id"));
        question.SerialOrder = index;
        question.LabelOrder = $(".nt-qnum", item).html();
        return question;
    }).convertJqueryArrayToJSArray();
    //$.post("/Tests/ResortQuestions", { questions: questions }, function (res) {
    //    //addtional doing here
    //});
    $.ajax({
        type: "POST",
        url: "/Tests/ResortQuestions",
        data: JSON.stringify({ questions: questions }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
        }

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
            resortInDb();
            initImageUploadFacility();
            initEditable();
        },
        update: function (ev, ui) {
            var cur = ui.item;
            var etab = $("#checklist");
            if (etab && cur.hasClass("t-question-type") && questions) {
                var type = "";
                if (cur.hasClass("t-question-type-radio")) { type = "Radio"; var obj = $(questions.radio); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : cur.replaceWith(obj); }
                if (cur.hasClass("t-question-type-multiple")) { type = "Multiple"; var obj = $(questions.multiple); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : cur.replaceWith(obj); }
                if (cur.hasClass("t-question-type-essay")) { type = "Essay"; var obj = $(questions.essay); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : cur.replaceWith(obj); }
                if (cur.hasClass("t-question-type-short")) { type = "ShortAnswer"; var obj = $(questions.shortanswer); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.shortanswer) : cur.replaceWith(obj); }
                if (cur.hasClass("t-question-type-text")) { type = "Text"; var obj = $(questions.text); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : cur.replaceWith(obj); }
                if (cur.hasClass("t-question-type-img")) { type = "Image"; var obj = $(questions.image); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : cur.replaceWith(obj); }
                sortByNumberOrLetters();

                var eleid = obj.attr("id")
                var testid = $("#test-id").val();
                var questiontitle = $(".nt-qtext", obj).html() == "<i>Enter Question</i>" ? "" : $(".nt-qtext", obj).html();
                var serialorder = obj.index();
                var labelorder = $(".nt-qnum", obj).html();
                var answers = $(".nt-qans", obj).map(function (iAns, ans) {
                    var answer = new Object();
                    answer.AnswerContent = $(".nt-qansdesc", ans).html() == "<i>Enter Answer</id>" ? "" : $(".nt-qansdesc", ans).html();
                    return answer;
                }).convertJqueryArrayToJSArray();
                addQuestion(eleid, testid, type, questiontitle, answers, serialorder, labelorder, function () {
                    resortInDb();
                });

                initImageUploadFacility();
                initEditable();
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
        $(".nt-qimg-ph.nt-clickable").each(function () {
            if (!this.dropzone) {
                $(this).dropzone({
                    url: "/Tests/UploadFiles",
                    paramName: "files",
                    previewTemplate: questions.imagepreview,
                    previewsContainer: ".preview-container",
                    thumbnailWidth: 200,
                    thumbnailHeight: 200,
                    uploadprogress: function (ev) {
                        $(this.element).hide();
                        $(this.element).parent().find(".nt-qimg-upload").show();
                    },
                    success: function (file, res) {
                    },
                    complete: function (ev) {
                        $(this.element).parent().find(".nt-qimg-upload").hide();
                        $(this.element).parent().find(".nt-qimg-cont").show();
                    },
                });
            }
        });
    }
}

//test facility
function addQuestion(elementid, testid, type, questiontitle, answers, serialorder, labelorder, onaddquestion) {
    var data = {
        testid: parseInt(testid),
        type: type,
        questiontitle: type=="Image"?"":questiontitle,
        serialorder: serialorder,
        labelorder: labelorder,
        answers: answers
    };
    $.ajax({
        type: "POST",
        url: "/Tests/AddNewQuestion",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (res) {
            if (res && res.success && res.questionHtml != "") {
                $("#" + elementid).replaceWith($(res.questionHtml));
            }
        }

    });
    if (onaddquestion && typeof (onaddquestion) === "function") {
        onaddquestion();
    }
}
function addAnswer(element, qid) {
    var questionid = parseInt(qid);
    var data = { questionid: questionid };
    $.ajax({
        type: "POST",
        url: "/Tests/AddAnswer",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (res) {
            if (res && res.success && res.questionHtml != "") {
                $(element).replaceWith($(res.questionHtml));
            }
        }

    });
}

$(function () {

    initDragAndDrop();
    initEditable();


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
            initImageUploadFacility();
        }
    });
    //separator
    $(".t-question-type").live("click", function (ev) {
        var cur = $(ev.currentTarget);
        var etab = $("#checklist");
        if (questions && etab) {
            var type = "";
            if (cur.hasClass("t-question-type-radio")) { type = "Radio"; var obj = $(questions.radio); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-multiple")) { type = "Multiple"; var obj = $(questions.multiple); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-essay")) { type = "Essay"; var obj = $(questions.essay); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-short")) { type = "ShortAnswer"; var obj = $(questions.shortanswer); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(questions.shortanswer) : etab.append(obj); }
            if (cur.hasClass("t-question-type-text")) { type = "Text"; var obj = $(questions.text); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-img")) { type = "Image"; var obj = $(questions.image); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            sortByNumberOrLetters();

            var eleid = obj.attr("id")
            var testid = $("#test-id").val();
            var questiontitle = $(".nt-qtext", obj).html() == "<i>Enter Question</i>" ? "" : $(".nt-qtext", obj).html();
            var serialorder = obj.index();
            var labelorder = $(".nt-qnum", obj).html();
            var answers = $(".nt-qans", obj).map(function (iAns, ans) {
                var answer = new Object();
                answer.AnswerContent = $(".nt-qansdesc", ans).html() == "<i>Enter Answer</id>" ? "" : $(".nt-qansdesc", ans).html();
                return answer;
            }).convertJqueryArrayToJSArray();
            addQuestion(eleid, testid, type, questiontitle, answers, serialorder, labelorder);


            initEditable();
            initImageUploadFacility();
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

        }
    });
    //separator
    $(".nt-qhctrls .bt-delete").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        content.remove()
        if ($("#checklist").children().length == 0) {
            $("#checklist").html(questions.empty);
        }
        sortByNumberOrLetters();

    });
    //separator
    $(".nt-btn-text.nt-qansadd").live("click", function (ev) {
        var parent = $(ev.target).closest(".nt-qitem.nt-qitem-edit");

        addAnswer(parent, parent.attr("question-id"));

        //when add an answer, show delete button on answer line
        if ($(".nt-qans.nt-qans-edit", parent).length > 2) {
            $(".nt-qansctrls .bt-delete", parent).show();
        } else {
            $(".nt-qansctrls .bt-delete", parent).hide();
        }
        initEditable();
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
                while ($(".nt-qans.nt-qans-edit", ansholder).length < savedObj.attr("answers").length) {
                    var anstemplate = $(".nt-qans.nt-qans-edit", ansholder).clone().get(0);
                    ansholder.append(anstemplate);
                }
                savedObj.attr("answers").each(function (index, o) {
                    var ele = $(".nt-qans.nt-qans-edit", ansholder).get(index);
                    if (ele) { $(".nt-qansdesc.nt-qedit", ele).html(o.answer) };
                });

                var quesid = itemParent.attr("id");
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
                    if (ele) { $(".nt-qansdesc.nt-qedit", ele).html(o.answer) };
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

    });
    //separator
    $(".nt-qanselem input[type=checkbox],[type=radio]").live("change", function (ev) {

    });
});
