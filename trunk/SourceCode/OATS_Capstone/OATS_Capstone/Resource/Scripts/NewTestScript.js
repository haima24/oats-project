var questions;
var emptylist;
var listkey = "listquestion";
var events;
var question_holder = new Array();
var testid;
var currentEditAnswer;

function initTagsOnTest() {
    $("#eventTags .tags-container").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-tag',
        cancel: "[contenteditable],input[type=text],select,textarea",
        stop: function (ev, ui) {
            var ids = $(".nt-tag", this).map(function (i, o) { return $(o).attr("tag-id"); }).convertJqueryArrayToJSArray();
            statusSaving();
            $.ajax({
                type: "POST",
                url: "/Tests/SortTagToTest",
                data: JSON.stringify({ testid: testid, ids: ids }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        statusSaved();
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        }
    });
    $("#test-tags").autocomplete({
        minLength: 0,
        select: function (e, ui) {
            statusSaving();
            var id = ui.item.id;
            if (id) {
                $.post("/Tests/AddTagToTest", { testid: testid, tagid: id }, function (res) {
                    if (res.success) {
                        var tagItem = $(res.generatedHtml);
                        $("#eventTags .nt-tags .tags-container").append(tagItem);
                        statusSaved();
                    }
                    else { showMessage("error", res.message); }
                });
            }
        },
        source: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Tests/SearchTagsOnTest",
                data: JSON.stringify({ testid: testid, term: req.term,maxrows:10 }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, element) {
                            if (element && element.tagname) {
                                return { id: element.id, label: element.tagname, value: element.tagname };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        }
    }).data("ui-autocomplete")._renderItem = function (ul, item) {
        if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
        var li = $("<li>").append("<a>" + item.label + "</a>");
        if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }
        li.appendTo(ul);
        return li;
    };
    $("#eventTags .nt-tag .nt-tag-remove").live("click", function (ev) {
        var item = $(this).closest(".nt-tag");
        var tagIdString = $(item).attr("tag-id");
        var tagId = parseInt(tagIdString);
        statusSaving();
        $.post("/Tests/RemoveTagToTest", { testid: testid, tagid: tagId }, function (res) {
            if (res.success) {
                item.remove();
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        });
    });
}
function initTagsOnQuestion() {
    $("#checklist .tags-container").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-tag',
        cancel: "[contenteditable],input[type=text],select,textarea",
        stop: function (ev, ui) {
            var ids = $(".nt-tag", this).map(function (i, o) { return $(o).attr("tag-id"); }).convertJqueryArrayToJSArray();
            var questionIdString = $(this).closest(".nt-qitem").attr("question-id");
            var questionid = parseInt(questionIdString);
            statusSaving();
            $.ajax({
                type: "POST",
                url: "/Tests/SortTagToQuestion",
                data: JSON.stringify({ questionid: questionid, ids: ids }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        statusSaved();
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        }
    });
    var boxes = $("#checklist .nt-qitem .nt-tag-adder input[type=text]");
    if (boxes.length > 0) {
        boxes.each(function () {
            $(this).autocomplete({
                minLength: 0,
                select: function (e, ui) {
                    var element = $(this);
                    var line = $(this).closest(".nt-tags");
                    var container = $(".tags-container", line);
                    statusSaving();
                    var id = ui.item.id;
                    var questionid = ui.item.questionid;
                    if (id) {
                        $.post("/Tests/AddTagToQuestion", { questionid: questionid, tagid: id }, function (res) {
                            if (res.success) {
                                var tagItem = $(res.generatedHtml);
                                $(container).append(tagItem);
                                statusSaved();
                                var ele = element;
                                ele.val("");
                            }
                            else { showMessage("error", res.message); }
                        });
                    }
                },
                source: function (req, res) {
                    var questionIdString = this.element.closest(".nt-qitem").attr("question-id");
                    var questionid = parseInt(questionIdString);
                    $.ajax({
                        type: "POST",
                        url: "/Tests/SearchTagsOnQuestion",
                        data: JSON.stringify({ questionid: questionid, term: req.term, maxrows: 10 }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (r) {
                            if (r.success) {
                                var result = $(r.resultlist).map(function (index, element) {
                                    if (element && element.tagname) {
                                        return { id: element.id, label: element.tagname, value: element.tagname, questionid: questionid };
                                    }
                                }).convertJqueryArrayToJSArray();
                                res(result);
                            } else {
                                showMessage("error", r.message);
                            }
                        }
                    });
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
                var li = $("<li>").append("<a>" + item.label + "</a>");
                if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }
                li.appendTo(ul);
                return li;
            };
        });
    }
    
}
function initPlot() {
    $('.nt-scores-table-boxplot-container').sparkline('html', {
        type: "box",
        width: '200px',
        spotRadius: 4,
        medianWidth: 2,
        showOutliers: !1,
        cornerRadius: 4,
        strikeThrough: !0,
        tooltipFormat: '<span>{{field:fields}}</span><span style="float: right; padding-left: 5px">{{value}}</span>',
        tooltipFormatFieldlist: ["lw", "lq", "med", "uq", "rw"],
        tooltipFormatFieldlistKey: "field",
        tooltipValueLookups: { fields: { lw: "Minimum", lq: "25th perc", med: "Median", uq: "75th perc", rw: "Maximum" } }
    });
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
function showOrHideDeleteLineAnswer() {
    $(".nt-qitem").each(function (i, o) {
        if ($(".nt-qans.nt-qans-edit", o).length > 2) {
            $(".nt-qansctrls .bt-delete", o).show();
        } else {
            $(".nt-qansctrls .bt-delete", o).hide();
        }
    });


}
function initDragAndDrop() {
    $(".t-question-type").draggable({
        connectToSortable: "#checklist[content-tab=true]",
        helper: "clone",
        revert: "invalid"
    });
    $("#checklist[content-tab=true]").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-qitem',
        placeholder: 'highlight',
        cancel: "[contenteditable],input[type=text],select,textarea",
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

                var questiontitle = $(".nt-qtext", obj).html() == "<i>Enter Question</i>" ? "" : $(".nt-qtext", obj).html();
                var serialorder = obj.index();
                var labelorder = $(".nt-qnum", obj).html();
                var answers = $(".nt-qans", obj).map(function (iAns, ans) {
                    var answer = new Object();
                    answer.AnswerContent = $(".nt-qansdesc", ans).html() == "<i>Enter Answer</id>" ? "" : $(".nt-qansdesc", ans).html();
                    answer.SerialOrder = $(ans).index();
                    return answer;
                }).convertJqueryArrayToJSArray();
                addQuestion(obj, testid, type, questiontitle, answers, serialorder, labelorder, function () {
                    resortInDb();
                });

                initImageUploadFacility();
                initEditable();
                //obj.remove();
            }
        }
    });

    //separator
    $("#checklist[content-tab=true] .nt-qanscont").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-qans',
        placeholder: 'highlight',
        cancel: "[contenteditable],input[type=text],select,textarea",
        stop: function (ev, ui) {
            updateAnswer($(ui.item).closest(".nt-qans"));
        }
    });

}
function initEditable() {
    initTagsOnQuestion();
    //separator
    $("#test-title").contentEditable({
        "placeholder": "<i>Enter Test Title</i>",
        "onBlur": function (element) {
            statusSaving();
            var text = element.content == "<i>Enter Test Title</i>" ? "" : element.content;
            $.post("/Tests/UpdateTestTitle", { testid: testid, text: text }, function (res) {
                if (!res.success) {
                    showMessage("error", res.message);
                } else {
                    statusSaved();
                }
            });
        },
    });
    $("#checklist[content-tab=true] .nt-qans .nt-qansdesc").contentEditable({
        "placeholder": "<i>Enter Answer</i>",
        "onBlur": function (element) {
            updateAnswer($(element).closest(".nt-qans"));
        },
        "onFocusIn": function (element) { currentEditAnswer = element; }
    });
    $("#checklist[content-tab=true] div.nt-qitem[question-type=Text] .nt-qtext").contentEditable({
        "placeholder": "<i>Enter Text</i>",
        "onBlur": function (element) {
            var item = $(element).closest(".nt-qitem");
            var quesid = item.attr("question-id");
            var content = element.content == "<i>Enter Text</i>" ? "" : element.content;
            updateQuestionTitle(quesid, content);
        },
    });
    $("#checklist[content-tab=true] div.nt-qitem[question-type!=Text] .nt-qtext.nt-qedit").contentEditable({
        "placeholder": "<i>Enter Question</i>",
        "onBlur": function (element) {
            var item = $(element).closest(".nt-qitem");
            var quesid = item.attr("question-id");
            var content = element.content == "<i>Enter Question</i>" ? "" : element.content;
            updateQuestionTitle(quesid, content);
        },
    });
}
function initPopover() {
    $(".nt-qsearch-content").popover({
        trigger: "hover",
        html: true,
        content: function () {
            return $(this).closest(".nt-qsearch").find(".nt-qsearch-popover-cont").html();
        }
    });
}
function initImageUploadFacility() {
    if (questions && questions.imagepreview) {
        $(".nt-qimg-ph.nt-clickable").each(function () {
            if (!this.dropzone) {
                var questionidString = $(this).closest(".nt-qitem").attr("question-id");
                var questionid = parseInt(questionidString);
                $(this).dropzone({
                    url: "/Tests/UploadFiles",
                    params: {
                        questionid: questionid
                    },
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
                        if (!res.success) { showMessage("error", res.message); }
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
function updateQuestionType(questionidString, type) {
    var questionid = parseInt(questionidString);
    if (!isNaN(questionid)) {
        statusSaving();
        $.post("/Tests/UpdateQuestionType", { questionid: questionid, type: type }, function (res) {
            if (res.success) {
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        })
    } else {
        showMessage("error", "Error when validating question.");
    }
}
function initSearchTests() {
    $.post("/Tests/TestsSearch", function (res) {
        if (res.success) {
            var source = $(res.resultlist).map(function (index, obj) {
                if (obj.TestTitle && obj.TestTitle != "") {
                    return { label: obj.TestTitle, value: obj.TestTitle, id: obj.Id };
                }
            }).convertJqueryArrayToJSArray();
            $(".navbar-search .nt-search-input").autocomplete({
                minLength: 0,
                source: source,
                focus: function (ev, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);
                    return false;
                },
                select: function (ev, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);
                    window.location.href = "/Tests/NewTest/" + ui.item.id;
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple"); }
                var li = $("<li>").append("<a>" + item.label + "</a>");
                if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }
                li.appendTo(ul);
                return li;
            };
        } else {
            showMessage("error", res.message);
        }
    });
}
function checkConstraintStartEnd(start, end, onsuccessvalidate) {
    if (start.getTime() > end.getTime()) {
        showMessage("error", "Invalid start time to end time.")
    } else {
        if (onsuccessvalidate && typeof (onsuccessvalidate) === "function") {
            onsuccessvalidate();
        }
    }
}
var dFromObj;
var dToObj;
function initDateTimePicker() {

    var dFString = $("#eventDateFrom").attr("current-date");
    var dTString = $("#eventDateTo").attr("current-date");
    var dF = !isNaN(parseInt(dFString)) ? new Date(parseInt(dFString)) : new Date();
    var dT = !isNaN(parseInt(dTString)) ? new Date(parseInt(dTString)) : new Date();

    dFromObj = dF;
    dToObj = dT;

    $("#eventDateFrom").datetimepicker({
        language: 'en',
        pick12HourFormat: true,
        pickSeconds: false
    }).datetimepicker('setLocalDate', dF).on('changeDate', function (ev) {
        dFromObj = ev.localDate;
        checkConstraintStartEnd(dFromObj, dToObj, function () {
            updateStartEndDate(testid, dFromObj, dToObj);
        });
    });
    $("#eventDateTo").datetimepicker({
        language: 'en',
        pick12HourFormat: true,
        pickSeconds: false
    }).datetimepicker('setLocalDate', dT).on('changeDate', function (ev) {
        dToObj = ev.localDate;
        checkConstraintStartEnd(dFromObj, dToObj, function () {
            updateStartEndDate(testid, dFromObj, dToObj);
        });
    });
}

function updateStartEndDate(testid, start, end) {
    statusSaving();

    $.ajax({
        type: "POST",
        url: "/Tests/UpdateStartEnd",
        data: JSON.stringify({ testid: testid, start: start, end: end }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (!res.success) {
                showMessage("error", res.message);
            } else {
                statusSaved();
            }
        }

    });
}

//test facility
function onUpdateSettingSuccess(res) {
}
function updateAnswer(lineElement, target) {
    var parentContainer = lineElement.closest(".nt-qanscont");
    if (target && target.type && target.type == "radio") {
        $(".nt-qans", parentContainer).each(function (index, obj) {
            if (!$(".nt-qanselem input[type=radio],[type=checkbox]", obj).attr("checked")) {
                $(".nt-qansscore input[type=text]", obj).val(0);
            }
        });
    }

    var answers = $(".nt-qans", parentContainer).map(function (index, obj) {
        var answer = new Object();
        answer.AnswerID = parseInt($(obj).attr("answer-id"));
        answer.AnswerContent = $(".nt-qansdesc", obj).html() == "<i>Enter Answer</i>" ? "" : $(".nt-qansdesc", obj).html();
        answer.IsRight = $(".nt-qanselem input[type=radio],[type=checkbox]", obj).attr("checked") ? true : false;
        var scoreString = $(".nt-qansscore input[type=text]", obj).val();
        if (scoreString == "" || scoreString == "0") {
            if (answer.IsRight == true) { $(".nt-qansscore input[type=text]", obj).val(1); }
        } else if (scoreString == "1") {
            if (answer.IsRight == false) { $(".nt-qansscore input[type=text]", obj).val(0); }
        }
        scoreString = $(".nt-qansscore input[type=text]", obj).val();
        var nScore = parseInt(scoreString);
        answer.Score = isNaN(nScore) ? 0 : nScore;
        answer.SerialOrder = index;
        return answer;
    }).convertJqueryArrayToJSArray();

    statusSaving();
    $.ajax({
        type: "POST",
        url: "/Tests/UpdateAnswer",
        data: JSON.stringify({ answers: answers }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (!res.success) {
                showMessage("error", res.message);
            } else {
                answers
                statusSaved();
            }
        }

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
    statusSaving();
    $.ajax({
        type: "POST",
        url: "/Tests/ResortQuestions",
        data: JSON.stringify({ count: questions.length, questions: questions }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (!res.success) {
                showMessage("error", res.message);
            } else {
                statusSaved();
            }
        }

    });
}
function addQuestion(element, testidentify, type, questiontitle, answers, serialorder, labelorder, onaddquestion, textdescription) {
    var data = {
        testid: testidentify,
        type: type,
        questiontitle: type == "Image" ? "" : questiontitle,
        serialorder: serialorder,
        labelorder: labelorder,
        textdescription: textdescription,
        answers: answers
    };
    statusSaving();
    $.ajax({
        type: "POST",
        url: "/Tests/AddNewQuestion",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        async: false,
        success: function (res) {
            if (res && res.success) {
                var newElement = $(res.generatedHtml);
                $(element).replaceWith(newElement);
                if (onaddquestion && typeof (onaddquestion) === "function") {
                    onaddquestion(newElement);
                }
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        }

    });

}
function addListQuestion(list, onAfterAddListQuestion) {
    statusSaving();
    $.ajax({
        type: "POST",
        url: "/Tests/AddListQuestion",
        data: JSON.stringify({ testid: testid, listquestion: list }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.success && res.arraylist) {
                $(res.arraylist).each(function (index, item) {
                    if (item.ClientID && item.QuestionHtml) {
                        var ele = $("#" + item.ClientID);
                        ele.replaceWith($(item.QuestionHtml));
                    }
                });
                statusSaved();
                showMessage("success", res.message);
            } else {
                showMessage("error", res.message);
            }
        },
        complete: function () {
            if (onAfterAddListQuestion && typeof (onAfterAddListQuestion) === "function") {
                onAfterAddListQuestion();
            }
        }

    });
}
function updateQuestionTitle(questionidString, newtext) {
    var questionid = parseInt(questionidString);
    statusSaving();
    $.post("/Tests/UpdateQuestionTitle", { questionid: questionid, newtext: newtext }, function (res) {
        if (!res.success) {
            showMessage("error", res.message);
        } else {
            statusSaved();
        }
    });
}
function deleteQuestion(questionidString, onsuccess) {
    var questionid = parseInt(questionidString);
    statusSaving();
    $.post("/Tests/DeleteQuestion", { questionid: questionid }, function (res) {
        if (res.success) {
            if (onsuccess && typeof (onsuccess) === "function") {
                onsuccess();
            }
            statusSaved();
        } else {
            showMessage("error", res.message);
        }
    });
}
function deleteAnswer(answeridString, onsuccess) {
    var answerid = parseInt(answeridString);
    statusSaving();
    $.post("/Tests/DeleteAnswer", { answerid: answerid }, function (res) {
        if (res.success) {
            if (onsuccess && typeof (onsuccess) === "function") {
                onsuccess();
            }
            statusSaved();
        } else {
            showMessage("error", res.message);
        }
    });
}
function addAnswer(element, qid, onsuccess, noreload) {
    var questionid = parseInt(qid);
    var data = { questionid: questionid };
    statusSaving();
    $.ajax({
        type: "POST",
        url: "/Tests/AddAnswer",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res && res.success) {
                if (!noreload) {
                    $(element).replaceWith($(res.generatedHtml));
                }
                if (onsuccess && typeof (onsuccess) === "function") {
                    onsuccess();
                }
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        }

    });
}
function saveTextDescription(questionidString, text) {
    var questionid = parseInt(questionidString);
    statusSaving();
    $.post("/Tests/UpdateQuestionTextDescription", { questionid: questionid, text: text }, function (res) {
        if (!res.success) {
            showMessage("error", res.message);
        } else {
            statusSaved();
        }
    });
}
$(function () {
    initTagsOnTest();
    initDragAndDrop();
    initEditable();
    initDateTimePicker();
    var testidString = $("#test-id").val();
    testid = parseInt(testidString);

    //separator
    initPopover();
    initSearchTests();
    //separator
    $("#sidebar .nt-ctrl-search input[type=text]").autocomplete({
        minLength: 0,
        source: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Tests/ReuseSearchQuestionTemplate",
                data: JSON.stringify({ maxrows: 4, term: req.term }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var list = $("#sidebar .nt-ctrl-list");
                        list.empty();
                        $(r.resultlist).each(function (i, o) {
                            list.append(o);
                        });
                        initPopover();
                    } else {
                        showMessage("error", r.message);
                    }
                }

            });
        }
    });
    //separator

    //separator
    $(".tab-event").live("click", function (e) {
        e.preventDefault();
        var link = e.target;
        var action = $(link).attr("href");
        var nav = $(link).closest(".nav");
        var li = $(link).closest("li");
        nav.find("li").removeClass("active");
        li.addClass("active");
        $.post(action, { testid: testid }, function (res) {
            if (res.success) {
                var tabcontent = $("#eventTab");
                if (tabcontent && res.generatedHtml) {
                    tabcontent.html(res.generatedHtml);
                    $("#sidebar[content-tab=true]").accordion();
                    sortByNumberOrLetters();
                    initEditable();
                    initImageUploadFacility();
                    initDragAndDrop();
                    initPlot();
                }
            } else { showMessage("error", res.message); }
        });
    });
    //separator
    $.post("/Tests/QuestionTypes", function (res) {
        if (res.success) {
            questions = res.obj;
            initImageUploadFacility();
        } else {
            showMessage("error", res.message);
        }
    });
    //separator
    $(".t-question-type").live("click", function (ev) {
        var cur = $(ev.currentTarget);
        var etab = $("#checklist");
        if (questions && etab) {
            var type = "";
            var obj;
            if (cur.hasClass("t-question-type-radio")) { type = "Radio"; obj = $(questions.radio); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-multiple")) { type = "Multiple"; obj = $(questions.multiple); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-essay")) { type = "Essay"; obj = $(questions.essay); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-short")) { type = "ShortAnswer"; obj = $(questions.shortanswer); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-text")) { type = "Text"; obj = $(questions.text); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            if (cur.hasClass("t-question-type-img")) { type = "Image"; obj = $(questions.image); obj.uniqueId(); $(".nt-empty-list-ph", etab).length == 1 ? etab.html(obj) : etab.append(obj); }
            sortByNumberOrLetters();

            var questiontitle = $(".nt-qtext", obj).html() == "<i>Enter Question</i>" || "<i>Enter Text</i>" ? "" : $(".nt-qtext", obj).html();
            var serialorder = obj.index();
            var labelorder = $(".nt-qnum", obj).html();
            var answers = $(".nt-qans", obj).map(function (iAns, ans) {
                var answer = new Object();
                answer.AnswerContent = $(".nt-qansdesc", ans).html() == "<i>Enter Answer</id>" ? "" : $(".nt-qansdesc", ans).html();
                answer.SerialOrder = $(ans).index();
                return answer;
            }).convertJqueryArrayToJSArray();
            addQuestion(obj, testid, type, questiontitle, answers, serialorder, labelorder, function (newelement) {
                resortInDb();
                sortByNumberOrLetters();
                initEditable();
                initImageUploadFacility();
                etab.scrollToElement(newelement);
            });


        }
    });
    //separator
    $("#checklist[content-tab=true] .bt-duplicate").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        var etab = $("#checklist");
        if (content && etab) {
            var duplicated = $(content).clone();
            duplicated.removeAttr("id");
            duplicated.removeAttr("question-id");
            $(".nt-qans", duplicated).removeAttr("answer-id");
            $(".nt-qanselem input[type=radio][name]", duplicated).removeAttrs("name");
            duplicated.uniqueId();
            duplicated.insertAfter(content);
            sortByNumberOrLetters();

            var type = duplicated.attr("question-type");
            var serialorder = duplicated.index();
            var labelorder = $(".nt-qnum", duplicated).html();
            var questiontitle = $(".nt-qtext", duplicated).html() == "<i>Enter Question</i>" ? "" : $(".nt-qtext", duplicated).html();
            var answers = $(".nt-qans", duplicated).map(function (iAns, ans) {
                var answer = new Object();
                answer.IsRight = $(".nt-qanselem input[type=radio],[type=checkbox]", ans).attr("checked") ? true : false;
                answer.AnswerContent = $(".nt-qansdesc", ans).html() == "<i>Enter Answer</i>" ? "" : $(".nt-qansdesc", ans).html();
                var scoreString = $("input[type=text].nt-on-score", ans).val();
                var nScore = parseInt(scoreString);
                answer.Score = isNaN(nScore) ? 0 : nScore;
                return answer;
            }).convertJqueryArrayToJSArray();
            var textdes = $(".nt-qrespinput", duplicated).val();
            addQuestion(duplicated, testid, type, questiontitle, answers, serialorder, labelorder, function (newelement) {
                etab.scrollToElement(newelement);
                resortInDb();
            }, textdes);

            initImageUploadFacility();
            initEditable();

        }
    });
    //separator
    $("#checklist[content-tab=true] .nt-qhctrls .bt-delete").live("click", function (ev) {
        var content = $(ev.target).closest(".nt-qitem.nt-qitem-edit");
        deleteQuestion($(content).attr("question-id"), function () {
            content.remove()
            if ($("#checklist").children().length == 0) {
                $("#checklist").html(questions.empty);
            }
            sortByNumberOrLetters();
            resortInDb();
        });



    });
    //separator
    $("#checklist[content-tab=true] .nt-btn-text.nt-qansadd").live("click", function (ev) {
        var parent = $(ev.target).closest(".nt-qitem");
        if (currentEditAnswer) { $(currentEditAnswer).blur();}
        addAnswer(parent, parent.attr("question-id"), function () {
            sortByNumberOrLetters();
            showOrHideDeleteLineAnswer();
            initEditable();
        });
    });
    //separator
    $("#checklist[content-tab=true] .nt-qanscont .nt-qansctrls .bt-delete").live("click", function () {
        var parent = $(this).closest(".nt-qitem");
        var ansline = $(this).closest(".nt-qans.nt-qans-edit");
        var ansid = ansline.attr("answer-id");
        deleteAnswer(ansid, function () {
            ansline.remove();
            showOrHideDeleteLineAnswer();
        });
    });
    //separator
    $("#checklist[content-tab=true] .nt-qtype-sel-predef,.nt-qtype-sel").live("change", function (ev) {
        var questiontype = $(this).val();
        var itemParent = $(this).closest(".nt-qitem.nt-qitem-edit");
        var itemid = itemParent.attr("id");
        if ($(this).hasClass("nt-qtype-sel-predef")) {
            //save
            var qaholder = itemParent.map(function () {
                var question = $(".nt-qtext.nt-qedit", itemParent).html();
                var answers = $(".nt-qans", itemParent).map(function (index, ans) {
                    var answer = $(".nt-qansdesc", ans).html();
                    var isright = $(".nt-qanselem input[type=radio],[type=checkbox]", ans).attr("checked") ? true : false;
                    return { answer: answer, isright: isright }
                });
                return { itemid: itemid, question: question, answers: answers };
            });
            question_holder[qaholder.attr("itemid")] = qaholder;
        }

        switch (questiontype) {
            case "Radio":
                var ques = $(questions.radio);
                //answers
                var ansholder = $(".nt-qanscont", ques);

                var savedObj = question_holder[itemid];
                if (savedObj) {
                    //question
                    $(".nt-qtext.nt-qedit", ques).html(savedObj.attr("question"));
                    while ($(".nt-qans.nt-qans-edit", ansholder).length < savedObj.attr("answers").length) {
                        var anstemplate = $(".nt-qans.nt-qans-edit", ansholder).clone().get(0);
                        ansholder.append(anstemplate);
                    }
                    savedObj.attr("answers").each(function (index, o) {
                        var ele = $(".nt-qans.nt-qans-edit", ansholder).get(index);
                        if (ele) {
                            $(".nt-qansdesc", ele).html(o.answer)
                            if (o.isright) {
                                $(".nt-qanselem input[type=radio]", ele).attr("checked", "checked");
                            }

                        };
                    });
                } else {
                    $(".nt-qans", ques).each(function () {
                        addAnswer(ques, itemParent.attr("question-id"), function () {
                            showOrHideDeleteLineAnswer();
                            initEditable();
                        }, true);
                    });

                }
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                var questionid = itemParent.attr("question-id");
                if (questionid) { ques.attr("question-id", questionid); }
                itemParent.replaceWith(ques);
                break;
            case "Multiple":
                var ques = $(questions.multiple);
                //answers
                var ansholder = $(".nt-qanscont", ques);

                var savedObj = question_holder[itemid];

                if (savedObj) {
                    //question
                    $(".nt-qtext.nt-qedit", ques).html(savedObj.attr("question"));

                    while ($(".nt-qans.nt-qans-edit", ansholder).length < savedObj.attr("answers").length) {
                        var anstemplate = $(".nt-qans.nt-qans-edit", ansholder).clone().get(0);
                        ansholder.append(anstemplate);
                    }
                    savedObj.attr("answers").each(function (index, o) {
                        var ele = $(".nt-qans.nt-qans-edit", ansholder).get(index);
                        if (ele) {
                            $(".nt-qansdesc.nt-qedit", ele).html(o.answer)
                            if (o.isright) {
                                $(".nt-qanselem input[type=checkbox]", ele).attr("checked", "checked");
                            }
                        };
                    });
                } else {
                    $(".nt-qans", ques).each(function () {
                        addAnswer(ques, itemParent.attr("question-id"), function () {
                            showOrHideDeleteLineAnswer();
                            initEditable();
                        }, true);
                    });
                }
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                var questionid = itemParent.attr("question-id");
                if (questionid) { ques.attr("question-id", questionid); }
                itemParent.replaceWith(ques);
                break;
            case "Essay":
                var ques = $(questions.essay);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                var questionid = itemParent.attr("question-id");
                if (questionid) { ques.attr("question-id", questionid); }
                itemParent.replaceWith(ques);
                break;
            case "ShortAnswer":
                var ques = $(questions.shortanswer);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                var questionid = itemParent.attr("question-id");
                if (questionid) { ques.attr("question-id", questionid); }
                itemParent.replaceWith(ques);
                break;
            case "Text":
                var ques = $(questions.text);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                var questionid = itemParent.attr("question-id");
                if (questionid) { ques.attr("question-id", questionid); }
                itemParent.replaceWith(ques);
                break;
            case "Image":
                var ques = $(questions.image);
                var quesid = itemParent.attr("id");
                if (quesid) { ques.attr("id", quesid); }
                var questionid = itemParent.attr("question-id");
                if (questionid) { ques.attr("question-id", questionid); }
                itemParent.replaceWith(ques);
                break;
            default:
                break;
        }
        var questionidString = itemParent.attr("question-id");
        if (questionidString) {
            updateQuestionType(questionidString, questiontype);
        }
        sortByNumberOrLetters();
        initImageUploadFacility();
        initEditable();


    });
    //separator
    $("#qpaste textarea").live("paste", function (ev) {
        setTimeout(function () {
            var textarea = $("#qpaste textarea");
            if (textarea) {
                //begin effect
                textarea.addClass("blur");
                $("#qpaste .nt-loader-large").show();
                //begin effect
                var pastedText = textarea.val();
                var questionslist = pastedText.split(/\n[\n]+/);
                var questionGenerated = new Array();
                $(questionslist).each(function (index, element) {
                    var lines = element.split("\n");
                    if (lines.length > 0) {
                        var question = new Object();
                        var htmlQuestion;
                        if (lines.length == 1) {
                            //text
                            question.QuestionType = { Type: "Text" };
                            question.QuestionTitle = lines[0];
                            htmlQuestion = $(questions.text);
                        } else {
                            //radio question
                            question.QuestionType = { Type: "Radio" };
                            question.QuestionTitle = lines[0];
                            question.Answers = $(lines).map(function (i, o) {
                                if (i > 0) {
                                    return { AnswerContent: o };
                                }
                            }).convertJqueryArrayToJSArray();
                            htmlQuestion = $(questions.radio);
                        }
                        htmlQuestion.uniqueId();
                        var clist = $("#checklist");
                        if ($(".nt-empty-list-ph", clist).length == 1) {
                            clist.html(htmlQuestion)
                        } else {
                            clist.append(htmlQuestion);
                        }

                        var clientid = htmlQuestion.attr("id");
                        questionGenerated.push({ clientid: clientid, question: question });
                    }

                });
                sortByNumberOrLetters();
                var listquestion = $(questionGenerated).map(function (idex, ele) {
                    var cid = ele.clientid;
                    var ques = ele.question;
                    if (cid && ques) {
                        var obj = $("#" + cid);
                        ques.SerialOrder = obj.index();
                        ques.LabelOrder = $(".nt-qnum", obj).html();
                        return { ClientID: cid, QuestionItem: ques };
                    }
                }).convertJqueryArrayToJSArray();

                addListQuestion(listquestion, function () {
                    //end effect
                    textarea.val("");
                    $("#qpaste .nt-loader-large").hide();
                    textarea.removeClass("blur");
                    //end effect
                    $("#checklist").animate({ scrollTop: $('#checklist')[0].scrollHeight }, 1000);
                });

            }
        }, 100);
    });
    //separator
    $(".nt-qitem textarea.nt-qrespinput,.nt-qitem input.nt-qrespinput").live("change", function (ev) {
        var pItem = $(this).closest(".nt-qitem");
        var quesidString = pItem.attr("question-id");
        saveTextDescription(quesidString, $(this).val());
    });
    $.initCheckboxAllSub({
        container: ".modal",
        all: ".nt-ctrl-all",
        sub: ".nt-clb-list input[type=checkbox]",
        onchange: function (container) {
            var boxes = $(".nt-clb-list input[type=checkbox]", container);
            boxes.each(function (index, ele) {
                var item = $(ele).closest(".nt-clb-item");
                if ($(ele).attr("checked")) {
                    item.addClass("nt-clb-item-sel");
                } else {
                    item.removeClass("nt-clb-item-sel");
                }
            });
        }

    });
    $.initCheckboxAllSub({
        container: "#sidebar .nt-ctrl-list",
        all: "#sidebar .nt-clb-header-control input[type=checkbox]",
        sub: "#respUsers .nt-clb-item input[type=checkbox]",
        onchange: function (container) {
            var boxes = $("#respUsers .nt-clb-item input[type=checkbox]", container);
            boxes.each(function (index, ele) {
                var item = $(ele).closest(".nt-clb-item");
                if ($(ele).attr("checked")) {
                    item.addClass("nt-clb-item-sel");
                } else {
                    item.removeClass("nt-clb-item-sel");
                }
            });
            var checkIds = $("input[type=checkbox][user-id]:checked").map(function (i, e) {
                return $(e).attr("user-id");
            }).convertJqueryArrayToJSArray();
            if (container.attr("response-tab")) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/NewTest_ResponseTab_CheckUserIds",
                    data: JSON.stringify({ testid: testid, userids: checkIds, count: checkIds.length }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (res) {
                        if (res.success) {
                            $("#response-container").html(res.generatedHtml);
                        } else {
                            showMessage("error", res.message);
                        }
                    }
                });
            }
            if (container.attr("score-tab")) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/NewTest_ScoreTab_CheckUserIds",
                    data: JSON.stringify({ testid: testid, userids: checkIds, count: checkIds.length }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (res) {
                        if (res.success) {
                            $("#score-container").html(res.generatedHtml);
                            initPlot();
                        } else {
                            showMessage("error", res.message);
                        }
                    }
                });
            }
        }
    });

    //separator
    $("#sidebar .nt-ctrl-list .nt-qsearch").live("click", function (ev) {
        var questionidString = $(this).attr("question-id");
        var questionid = parseInt(questionidString);
        var templateQuestion;
        if (!isNaN(questionid)) {
            statusSaving();
            $.post("/Tests/ReuseQuestionTemplate", { questionid: questionid }, function (res) {
                if (res && res.success && res.generatedHtml) {
                    var newItem = $(res.generatedHtml);
                    var type = newItem.attr("question-type");
                    newItem.removeAttr("id");//if exist
                    newItem.removeAttr("question-id");
                    $(".nt-qans", newItem).removeAttr("answer-id");
                    $(".nt-qanselem input[type=radio][name]", newItem).removeAttr("name");
                    newItem.uniqueId();
                    var clist = $("#checklist");
                    if ($(".nt-empty-list-ph", clist).length == 1) {
                        clist.html(newItem)
                    } else {
                        clist.append(newItem);
                    }
                    sortByNumberOrLetters();
                    var type = newItem.attr("question-type");
                    var serialorder = newItem.index();
                    var labelorder = $(".nt-qnum", newItem).html();
                    var questiontitle = $(".nt-qtext", newItem).html();
                    var answers = $(".nt-qans", newItem).map(function (iAns, ans) {
                        var answer = new Object();
                        answer.IsRight = $(".nt-qanselem input[type=radio],[type=checkbox]", ans).attr("checked") ? true : false;
                        answer.AnswerContent = $(".nt-qansdesc", ans).html() == "<i>Enter Answer</id>" ? "" : $(".nt-qansdesc", ans).html();
                        var scoreString = $("input[type=text].nt-on-score", ans).val();
                        var nScore = parseInt(scoreString);
                        answer.Score = isNaN(nScore) ? 0 : nScore;
                        return answer;
                    }).convertJqueryArrayToJSArray();
                    var textdes = $(".nt-qrespinput", newItem).val();
                    addQuestion(newItem, testid, type, questiontitle, answers, serialorder, labelorder, function (newelement) {
                        clist.scrollToElement(newelement);
                        resortInDb();
                    }, textdes);
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    
    //separator
    $("#checklist[content-tab=true] .nt-qitem .nt-scrbtn").live("click", function (ev) {
        var item = $(this).closest(".nt-qitem");
        var pointPanels = $(".nt-qansscore", item);

        if ($(this).hasClass("active")) {
            $(this).removeClass("active");
            pointPanels.hide();
        } else {
            $(this).addClass("active");
            pointPanels.show();
        }
    });
    //separator
    $("#modalPopupUser button.nt-btn-ok").live("click", function (ev) {
        var container = $("#modalPopupUser .nt-clb-list");
        var role = $("#modalPopupUser").attr("role");
        var checkedCheckbox = $("input[type=checkbox]:checked", container);
        var chekcedIds = checkedCheckbox.map(function (index, element) {
            return parseInt($(element).attr("user-id"));

        }).convertJqueryArrayToJSArray();
        statusSaving();
        $.ajax({
            type: "POST",
            url: "/Tests/AddUserToInvitationTest",
            data: JSON.stringify({ testid: testid, count: chekcedIds.length, userids: chekcedIds, role: role }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.success) {
                    $("#eventTab").html($(res.generatedHtml));
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                }
            }

        });
        $("#modalPopupUser").modal('hide');

    });
    $("#modalPopupUser .nt-clb-list input[type=checkbox]").live("click", function (ev) {
        var pr = $(this).closest(".nt-clb-item");
        if ($(this).attr("checked")) { pr.addClass("nt-clb-item-sel"); }
        else { pr.removeClass("nt-clb-item-sel"); }
    });
    //separator
    $(".nt-qitem .nt-qans").live("change", function (ev) {
        updateAnswer($(this).closest(".nt-qans"), ev.target);
    });
    //separator
    $("#eventDuplicate").live("click", function (ev) {
        $.post("/Tests/DuplicateTest", { testid: testid }, function (res) {
            if (res.success && res.id) {
                window.location = "/Tests/NewTest/" + res.id;
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $("#eventTab .nt-asm-settings .nt-section input[type=checkbox]").live("click", function (ev) {
        var settingKey = $(this).attr("setting-key");
        var isactive = $(this).attr("checked") ? true : false;
        $.post("/Tests/UpdateSettings", { testid: testid, settingKey: settingKey, isactive: isactive }, function (res) {
            if (!res.success) {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $(".nt-btn-rm").live("click", function (ev) {
        var userid = parseInt($(this).closest("tr").attr("user-id"));
        $.post("/Tests/RemoveUser", { testid: testid, userid: userid }, function (res) {
            if (res.success) {
                $("#eventTab").html($(res.generatedHtml));
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $("#btn-remove-student").live("click", function (ev) {
        $.post("/Tests/ModalRemovePopupUser", { testid: testid, role: "Student" }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalRemovePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalRemovePopupUser").replaceWith($(html));
                }
                $("#modalRemovePopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
    //Separator
    $("#btn-remove-teacher").live("click", function (ev) {
        $.post("/Tests/ModalRemovePopupUser", { testid: testid, role: "Teacher" }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalRemovePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalRemovePopupUser").replaceWith($(html));
                }
                $("#modalRemovePopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
    //Separator
    $("#modalRemovePopupUser button.nt-btn-ok").live("click", function (ev) {
        var container = $("#modalRemovePopupUser .nt-clb-list");
        var checkedCheckbox = $("input[type=checkbox]:checked", container);
        var chekcedIds = checkedCheckbox.map(function (index, element) {
            return parseInt($(element).attr("user-id"));

        }).convertJqueryArrayToJSArray();
        statusSaving();
        $.ajax({
            type: "POST",
            url: "/Tests/RemoveUserToInvitationTest",
            data: JSON.stringify({ testid: testid, count: chekcedIds.length, userids: chekcedIds }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.success) {
                    $("#eventTab").html($(res.generatedHtml));
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                }
            }

        });
        $("#modalRemovePopupUser").modal('hide');

    });
    //separator
    $("#btn-invite-more-teacher,#btn-invite-more-student").live("click", function (ev) {
        var role = "";
        if ($(this).attr("id") == "btn-invite-more-teacher") { role = "Teacher"; } else { role = "Student"; }
        $.post("/Tests/ModalPopupUser", { testid: testid, role: role }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalPopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalPopupUser").replaceWith($(html));
                }
                $("#modalPopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
    //Separator
    $("#btn-reinvite-student").live("click", function (ev) {
        $.post("/Tests/ModalReinvitePopupUser", { testid: testid, role: "Student" }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalReinvitePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalReinvitePopupUser").replaceWith($(html));
                }
                $("#modalReinvitePopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
    //Separator
    $("#modalReinvitePopupUser button.nt-btn-ok").live("click", function (ev) {
        $("#modalReinvitePopupUser").modal('hide');
    });
    //Separator
    $("#btn-reinvite-teacher").live("click", function (ev) {
        $.post("/Tests/ModalReinvitePopupUser", { testid: testid, role: "Teacher" }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalReinvitePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalReinvitePopupUser").replaceWith($(html));
                }
                $("#modalReinvitePopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $("#eventActiveTest").live("click", function (ev) {
        $.post("/Tests/EnableTest", { testid: testid }, function (res) {
            if (res.success && res.generatedHtml) {
                var container = $("#container");
                container.html(res.generatedHtml);
                container.removeClass("test-disable");
                $("#eventActiveTest").hide("slide", { direction: "up" });
                initDateTimePicker();
                initDragAndDrop();
                initEditable();
                initImageUploadFacility();
                sortByNumberOrLetters();
                initPopover();
                $("#sidebar").accordion();
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $("#checklist .nt-qitem .nt-tag-remove").live("click", function (ev) {
        var item = $(this).closest(".nt-tag");
        var tagIdString = $(item).attr("tag-id");
        var tagId = parseInt(tagIdString);
        var questionIdString = $(this).closest(".nt-qitem").attr("question-id");
        var questionid = parseInt(questionIdString);
        statusSaving();
        $.post("/Tests/RemoveTagToQuestion", { questionid: questionid, tagid: tagId }, function (res) {
            if (res.success) {
                item.remove();
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator

    showOrHideDeleteLineAnswer();
    sortByNumberOrLetters();
    var hub = $.connection.generalHub;
    hub.client.R_deactivetest = function (id, mail) {
        if (id && id == testid) {
            showCountDownMessage("info", "User with email:" + mail + " disabled this test", function () {
                window.location = "/Tests";
            });
        }
    }
    $.connection.hub.start().done(function () {
        $("#eventDel").live("click", function () {
            $.post("/Tests/DeActiveTest", { testid: testid }, function (res) {
                if (!res.success) { showMessage("error", res.message); }
                else {
                    showMessage("success", res.message);
                }
            });
        });
    });
});