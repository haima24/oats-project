var questions;
var emptylist;
var events;
var testid;
var userid;
var currentEditAnswer;
var currentScoreDetailTab = "statistic";
var currentFeedBackTab = "student";
var reuseAddedTags = new Array();

function initClientSorting() {
    //sidebar
    var handlers = new Array();
    handlers.push({
        key: "name", handler: ".nt-clb-item-label"
    });
    handlers.push({
        key: "percent", handler: ".nt-clb-item-desc"
    });
    $(".nt-clb-header-label .nt-btn-circle-mini,.nt-clb-header-desc .nt-btn-circle-mini").clientSort({
        sortItems: "#respUsers .nt-clb-item",
        handlers: handlers
    });
    //sidebar
    //score statistic
    var scoreHandlers = new Array();
    scoreHandlers.push({
        key: "tag", handler: ".nt-tag span"
    });
    scoreHandlers.push({
        key: "selection", handler: ".nt-scores-table-selavgcol", useSortAttr: true
    });
    scoreHandlers.push({
        key: "group", handler: ".nt-scores-table-egavgcol", useSortAttr: true
    });
    scoreHandlers.push({
        key: "diff", handler: ".nt-scores-table-avgdiffcol", useSortAttr: true
    });
    scoreHandlers.push({
        key: "stdev", handler: ".nt-scores-table-grpstdevcol", useSortAttr: true
    });
    $("#score-container .nt-scores-table-header .nt-btn-circle-mini").clientSort({
        sortItems: ".nt-scores-table-body .nt-tr-sortable",
        parentContainer: "#score-container",
        handlers: scoreHandlers
    });
    //score statistic
}
function handleScoreStatisticSubEvents() {
    var handleColumn = function (name, per, frac) {
        $(".nt-scores-table-header " + name + " .nt-scores-table-title").live("click", function () {
            var $this = $(this);
            var items = $(".nt-scores-table-body " + name + ",.nt-scores-table-footer " + name);
            if ($this.attr("data-toggle") == "true") {
                $this.removeAttr("data-toggle");
                items.each(function () {
                    $(per, this).hide();
                    $(frac, this).show();
                });
            } else {
                $this.attr("data-toggle", "true");
                items.each(function () {
                    $(per, this).show();
                    $(frac, this).hide();
                });
            }
        });
    };
    handleColumn(".nt-scores-table-selavgcol", ".nt-selavg", ".nt-selavg-nat");
    handleColumn(".nt-scores-table-egavgcol", ".nt-egavg", ".nt-egavg-nat");
    handleColumn(".nt-scores-table-avgdiffcol", ".nt-avgdiff", ".nt-avgdiff-nat");
    handleColumn(".nt-scores-table-grpstdevcol", ".nt-grpstdev", ".nt-grpstdev-nat");
}
function checkMaxScoreAndTotalScore() {
    $.post("/Tests/CheckMaxScoreAndTotalScore", { testid: testid }, function (res) {
        if (res.success) {
            var obj = $("#message-max-score");
            if (typeof (res.IsRunning) != "undefined") {
                if (res.IsRunning) {
                    obj.hide();
                } else {
                    $(".total", obj).html(res.TotalScore);
                    $(".max", obj).html(res.MaxScoreSetting);
                    obj.show();
                }
            }
        }
        else {
            showMessage("error", res.message);
        }
    });
}
function updateTestIntroduction() {
    var text = $("#intro-detail").val();
    statusSaving();
    $.post("/Tests/UpdateTestIntroduction", { testid: testid, introduction: text }, function (res) {
        if (res.success) {
            statusSaved();
        } else {
            showMessage("error", res.message);
        }
    });
}
function postSetting(li) {
    var cb = $("input[type=checkbox]", li);
    var settingKey = cb.attr("setting-key");
    var isactive = cb.attr("checked") ? true : false;
    var num = parseInt($("[data-key=number]", li).val());
    var number = isNaN(num) ? null : num;
    var text = $("[data-key=text]", li).val() || null;
    statusSaving();
    $.post("/Tests/UpdateSettings", { testid: testid, settingKey: settingKey, isactive: isactive, number: number, text: text }, function (res) {
        if (res.success) {
            var html = $(res.generatedHtml);
            if (li) {
                li.html(html);
                checkMaxScoreAndTotalScore();
                statusSaved();
            }
        } else {
            showMessage("error", res.message);
        }
    });
}
function initReuseDragAndDrop() {
    $("#sidebar[content-tab=true] .nt-qsearch", "#modalPopupUser[content-tab=true] .nt-qsearch").draggable({
        connectToSortable: "#checklist[content-tab=true]",
        helper: "clone",
        revert: "invalid"
    });
}
function initDropText() {
    $("#qpaste textarea").filedrop({
        success: function (text) {
            handleImportText(text);
        },
        error: function (msg) {
            showMessage("error", msg);
        }
    });
}
function handleImportText(text) {
    var pastedText = text;
    if (pastedText) {
        var textarea = $("#qpaste textarea");
        //begin effect
        textarea.addClass("blur");
        $("#qpaste .nt-loader-large").show();
        //begin effect
        var questionslist = pastedText.split(/\s*\n\s*[\s*\n\s*]+/);
        var allQuestions = $(questionslist).map(function (index, element) {
            var lines = element.split("\n");
            if (lines.length > 0) {
                var question = new Object();
                var htmlQuestion;
                if (lines.length == 1) {
                    //text
                    question.QuestionType = { Type: "Text" };
                    question.QuestionTitle = lines[0];
                } else {
                    //radio question
                    var trueCount = 0;
                    question.QuestionTitle = lines[0];
                    var isMatching = false;
                    for (var i = 0; i < lines.length; i++) {
                        if (i > 0) {
                            var line = lines[i];
                            if (line.indexOf("~") >= 0) {
                                isMatching = true;
                            }
                        }
                    }
                    question.Answers = $(lines).map(function (i, o) {
                        if (i > 0) {
                            var order = i - 1;
                            var ans = new Object();
                            ans.SerialOrder = order;
                            ans.Score = 0;
                            var indicator = o.substr(0, 1);
                            var length = o.length;
                            if (indicator) {
                                switch (indicator) {
                                    case "-":
                                        ans.IsRight = false;
                                        ans.AnswerContent = o.substr(1, length - 1);
                                        break;
                                    case "=":
                                        trueCount++;
                                        ans.IsRight = true;
                                        ans.AnswerContent = o.substr(1, length - 1);
                                        break;
                                    default:
                                        if (isMatching) {
                                            var begin = "";
                                            var end = "";
                                            var indexSymbolMatching = o.indexOf("~");
                                            if (indexSymbolMatching >= 0) {
                                                begin = o.substring(0, indexSymbolMatching);
                                                end = o.substring(indexSymbolMatching + 1);
                                            } else {
                                                begin = o;
                                            }
                                            ans.IsRight = true;
                                            ans.AnswerContent = begin;
                                            var ansDepend = new Object();
                                            ansDepend.IsRight = false;
                                            ansDepend.AnswerContent = end;
                                            ans.AnswerChilds = new Array(ansDepend);
                                        } else {
                                            ans.IsRight = false;
                                            ans.AnswerContent = o;
                                        }
                                        break;
                                }
                            }
                            return ans;
                        }
                    }).convertJqueryArrayToJSArray();
                    if (trueCount <= 1) {
                        if (isMatching) {
                            question.QuestionType = { Type: "Matching" };
                        } else {
                            question.QuestionType = { Type: "Radio" };
                        }
                    } else {
                        question.QuestionType = { Type: "Multiple" };
                    }
                }
                return question;
            }

        }).convertJqueryArrayToJSArray();
        addListQuestion(allQuestions, function () {
            //end effect
            textarea.val("");
            $("#qpaste .nt-loader-large").hide();
            textarea.removeClass("blur");
            //end effect
            $("#checklist").animate({ scrollTop: $('#checklist')[0].scrollHeight }, 1000);
        });
    }
}
function initScoreOnUserChart() {
    var charts = $("#score-charter div[data=charter]");
    if (charts.length > 0) {
        charts.each(function (i, e) {
            var name = $(e).attr("name");
            var percent = $(e).attr("percent");
            var color = $.findColor(percent, 0, 100);
            $(e).jqbar({ label: name, value: percent, barColor: color, barWidth: 20 });
        });
    }
}
function initTagsOnTest() {
    $("#eventTags .tags-container").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-tag:not([data-not-owner])',
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
            e.stopPropagation();
            $("#test-tags").val("")
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
                data: JSON.stringify({ testid: testid, term: req.term, maxrows: 10 }),
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
    $("#test-tags").live("keydown", function (ev) {
        if (ev.keyCode == 13) {
            statusSaving();
            var text = $(this).val();
            $.post("/Tests/AddTagToTest", { testid: testid, tagid: 0, tagname: text }, function (res) {
                if (res.success) {
                    var tagItem = $(res.generatedHtml);
                    $("#eventTags .nt-tags .tags-container").append(tagItem);
                    statusSaved();
                }
                else { showMessage("error", res.message); }
            });
        }
    });
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
        items: '>.nt-tag:not([data-not-owner])',
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
                    e.stopPropagation();
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

        var type = $(o).attr("question-type");
        if (type == "Matching") {
            if ($(".nt-qmatching", o).length > 2) {
                $(".nt-qansctrls .bt-delete", o).show();
            } else {
                $(".nt-qansctrls .bt-delete", o).hide();
            }
        }
        else {
            if ($(".nt-qans.nt-qans-edit", o).length > 2) {
                $(".nt-qansctrls .bt-delete", o).show();
            } else {
                $(".nt-qansctrls .bt-delete", o).hide();
            }
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
        items: '>.nt-qitem:not([data-not-owner])',
        placeholder: 'highlight',
        cancel: "[contenteditable],input[type=text],select,textarea",
        stop: function (ev, ui) {
            resortInDb();
            initImageUploadFacility();
            initEditable();
        },
        update: function (ev, ui) {
            var cur = $(ui.item);
            var dataAction = cur.attr("data-action");
            var etab = $("#checklist");
            if (dataAction) {
                if (dataAction == "addnew") {
                    var type = cur.attr("type");
                    if (etab && type) {
                        addQuestion(testid, type, function (newelement) {
                            if ($(".nt-empty-list-ph", etab).length > 0) {
                                etab.html(newelement);
                            } else {
                                cur.replaceWith(newelement);
                            }
                            resortInDb();
                            initEditable();
                            initImageUploadFacility();
                            etab.scrollToElement(newelement);
                        });
                    }
                }
                else if (dataAction == "reuse") {
                    var questionIdString = cur.attr("question-id");
                    var questionid = parseInt(questionIdString);
                    if (etab && !isNaN(questionid)) {
                        $.post("/Tests/CloneQuestion", { targetTestID: testid, questionid: questionid }, function (res) {
                            if (res.success) {
                                var newelement = $(res.generatedHtml);
                                if ($(".nt-empty-list-ph", etab).length > 0) {
                                    etab.html(newelement);
                                } else {
                                    cur.replaceWith(newelement);
                                }
                                resortInDb();
                                initEditable();
                                initImageUploadFacility();
                                etab.scrollToElement(newelement);
                                statusSaved();
                            } else {
                                showMessage("error", res.message);
                            }
                        });
                    }
                }
            }
        }
    });

    //separator
    $("#checklist[content-tab=true] .nt-qanscont").sortable({
        revert: true,
        tolerance: 'pointer',
        revert: 50,
        distance: 5,
        items: '>.nt-qans:not([data-not-owner])',
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
    $("#test-title[contenteditable=true]").contentEditable({
        "placeholder": "<i>Enter Test Title</i>",
        "onBlur": function (element) {
            statusSaving();
            var text = element.content == "<i>Enter Test Title</i>" ? "" : element.content;
            $.ajax({
                type: "POST",
                url: "/Tests/UpdateTestTitle",
                data: JSON.stringify({ testid: testid, text: text }),
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
        },
    });
    $("#checklist[content-tab=true] .nt-qans .nt-qansdesc[contenteditable=true]").contentEditable({
        "placeholder": "<i>Enter Answer</i>",
        "onBlur": function (element) {
            updateAnswer($(element).closest(".nt-qans"));
        },
        "onFocusIn": function (element) { currentEditAnswer = element; }
    });
    $("#checklist[content-tab=true] div.nt-qitem[question-type=Text] .nt-qtext[contenteditable=true]").contentEditable({
        "placeholder": "<i>Enter Text</i>",
        "onBlur": function (element) {
            var item = $(element).closest(".nt-qitem");
            var quesid = item.attr("question-id");
            var content = element.content == "<i>Enter Text</i>" ? "" : element.content;
            updateQuestionTitle(quesid, content);
        },
    });
    $("#checklist[content-tab=true] div.nt-qitem[question-type!=Text] .nt-qtext.nt-qedit[contenteditable=true]").contentEditable({
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
function updateQuestionType(questionidString, type, onsuccess) {
    var questionid = parseInt(questionidString);
    if (!isNaN(questionid)) {
        statusSaving();
        $.post("/Tests/UpdateQuestionType", { questionid: questionid, type: type }, function (res) {
            if (res.success) {
                statusSaved();
                if (onsuccess && typeof (onsuccess) === "function") {
                    onsuccess(res.generatedHtml);
                }
            } else {
                showMessage("error", res.message);
            }
        })
    } else {
        showMessage("error", "Error when validating question.");
    }
}
function initSearchTests() {
    $(".navbar-search input[type=text].nt-search-input").oatsSearch({
        rendermenu: function (items) {
            $(items).each(function () {
                var html = this.key;
                var obj = this.value;
                if (html && obj) {
                    $(html).attr("data-toggle", "tooltip");
                    if (obj.isCurrentUserOwnTest) {
                        if (obj.running) {
                            $(html).attr("data-original-title", "Open This Test");
                        } else {
                            $(html).attr("data-original-title", "Open This Test - This Test Locked due to compatitle problem");
                        }
                    } else {
                        if (obj.running) {
                            $(html).attr("data-original-title", "Take This Test");
                        } else {
                            $(html).attr("data-original-title", "This Test Locked");
                        }
                    }
                    $(html).tooltip();
                    if (obj.intro) {
                        $(".pop-over", html).popover({
                            placement: "left",
                            trigger: "hover",
                            html: true,
                            content: function () {
                                var div = $("<div>").html(obj.intro);
                                return div;
                            }
                        });
                    }
                }
            });
        },
        select: function (item) {
            if (item.isCurrentUserOwnTest) {
                window.location.href = "/Tests/NewTest/" + item.id;
            } else {
                if (item.running) {
                    window.location.href = "/Tests/DoTest/" + item.id;
                }
            }
        },
        source: function (req, res, addedTagIds) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsSearch",
                data: JSON.stringify({ term: req, tagids: addedTagIds }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.TestTitle && obj.TestTitle != "") {
                                return { des: obj.DateDescription, title: obj.TestTitle, id: obj.Id, isCurrentUserOwnTest: obj.IsCurrentUserOwnTest, intro: obj.Introduction, running: obj.IsRunning };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        },
        tagsource: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsSearchTag",
                data: JSON.stringify({ term: req }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.TagName && obj.TagName != "") {
                                return { id: obj.TagID, name: obj.TagName };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
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
function ajaxUpdateAnwer(answers) {
    statusSaving();
    $.ajax({
        type: "POST",
        url: "/Tests/UpdateAnswer",
        data: JSON.stringify({ answers: answers }),
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (!res.success) {
                showMessage("error", res.message);
            } else {
                statusSaved();
                checkMaxScoreAndTotalScore();
            }
        }
    });
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
    var isMatching = parentContainer.hasAttr("data-matching");
    var answers = $(".nt-qans", parentContainer).map(function (index, obj) {
        var $obj = $(obj);
        var answer = new Object();
        answer.AnswerID = parseInt($obj.attr("answer-id"));
        answer.AnswerContent = $(".nt-qansdesc", obj).html() == "<i>Enter Answer</i>" ? "" : $(".nt-qansdesc", obj).html();

        if (isMatching) {
            if ($obj.hasAttr("data-matching-begin")) {
                var matchingContainer = $obj.closest(".nt-qmatching");
                var tb = $(".nt-qansscore input[type=text]", matchingContainer);
                var scoreString = tb.val();
                if (scoreString == "" || scoreString == "0") {
                    if (answer.IsRight == true) { tb.val(1); }
                } else if (scoreString == "1") {
                    if (answer.IsRight == false) { tb.val(0); }
                }
                scoreString = tb.val();
                var nScore = parseFloat(scoreString);
                answer.Score = isNaN(nScore) ? 0 : nScore;
                answer.SerialOrder = matchingContainer.index();
                answer.IsRight = true;
            }
            else {
                answer.IsRight = false;
            }
        }
        else {
            answer.IsRight = $(".nt-qanselem input[type=radio],[type=checkbox]", $obj).attr("checked") ? true : false;
            var tb = $(".nt-qansscore input[type=text]", $obj);
            var scoreString = tb.val();
            if (scoreString == "" || scoreString == "0") {
                if (answer.IsRight == true) { tb.val(1); }
            } else if (scoreString == "1") {
                if (answer.IsRight == false) { tb.val(0); }
            }
            scoreString = tb.val();
            var nScore = parseFloat(scoreString);
            answer.Score = isNaN(nScore) ? 0 : nScore;
            answer.SerialOrder = index;
        }

        return answer;
    }).convertJqueryArrayToJSArray();
    ajaxUpdateAnwer(answers);

}
function resortInDb() {
    sortByNumberOrLetters();
    var questions = $("#checklist[content-tab=true] [question-id]").map(function (index, item) {
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
function addQuestion(testidentify, type, onaddquestion) {
    var data = {
        testid: testidentify,
        type: type
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
                var clist = $("#checklist");
                $(res.arraylist).each(function (index, item) {
                    var $item = $(item);
                    if ($(".nt-empty-list-ph", clist).length == 1) {
                        clist.html($item)
                    } else {
                        clist.append($item);
                    }
                });
                resortInDb();
                initEditable();
                initImageUploadFacility();
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
    $.ajax({
        type: "POST",
        url: "/Tests/UpdateQuestionTitle",
        data: JSON.stringify({ questionid: questionid, newtext: newtext }),
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
    initImageUploadFacility();
    initReuseDragAndDrop();
    initDropText();
    initTagsOnTest();
    initDragAndDrop();
    initEditable();
    initDateTimePicker();
    var testidString = $("#test-id").val();
    testid = parseInt(testidString);

    var useridString = $("#user-id").val();
    userid = parseInt(useridString);

    checkMaxScoreAndTotalScore();

    //separator
    initPopover();
    initSearchTests();

    //separator
    $(".tab-event").live("click", function (e) {
        e.preventDefault();
        var link = e.target;
        var action = $(link).attr("href");
        var nav = $(link).closest(".nav");
        var li = $(link).closest("li");
        nav.find("li").removeClass("active");
        li.addClass("active");
        $.post(action, { testid: testid, tab: currentScoreDetailTab, feedbacktab: currentFeedBackTab }, function (res) {
            if (res.success) {
                var tabcontent = $("#eventTab");
                if (tabcontent && res.generatedHtml) {
                    tabcontent.html(res.generatedHtml);
                    $("#sidebar[content-tab=true]").accordion({ heightStyle: "content" });
                    //sortByNumberOrLetters();
                    initEditable();
                    initReuseDragAndDrop();
                    initImageUploadFacility();
                    initDragAndDrop();
                    initPlot();
                    initScoreOnUserChart();
                    initReplyAreas();
                    initDropText();
                    var activeLi = $("#score-detail-tab li").filter(function () {
                        return $("a[tab=" + currentScoreDetailTab + "]", this).length > 0;
                    }).addClass("active");
                }
            } else { showMessage("error", res.message); }
        });
    });
    //separator
    $.post("/Tests/QuestionTypes", function (res) {
        if (res.success) {
            questions = res.obj;
            //initImageUploadFacility();
        } else {
            showMessage("error", res.message);
        }
    });
    //separator
    $(".t-question-type").live("click", function (ev) {
        var type = $(this).attr("type");
        var etab = $("#checklist");
        if (etab && type) {
            addQuestion(testid, type, function (newelement) {
                if ($(".nt-empty-list-ph", etab).length > 0) {
                    etab.html(newelement);
                } else {
                    etab.append(newelement);
                }
                resortInDb();
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
        var questionidString = $(content).attr("question-id");
        var questionid = parseInt(questionidString);
        var templateQuestion;
        if (!isNaN(questionid)) {
            statusSaving();
            $.post("/Tests/CloneQuestion", { targetTestID: testid, questionid: questionid }, function (res) {
                if (res.success) {
                    var newItem = $(res.generatedHtml);
                    var etab = $("#checklist");
                    if ($(".nt-empty-list-ph", etab).length == 1) {
                        etab.html(newItem)
                    } else {
                        etab.append(newItem);
                    }
                    resortInDb();
                    initEditable();
                    initImageUploadFacility();
                    etab.scrollToElement(newItem);
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                }
            });
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
            resortInDb();
        });
    });
    //separator
    $("#checklist[content-tab=true] .nt-btn-text.nt-qansadd").live("click", function (ev) {
        var parent = $(ev.target).closest(".nt-qitem");
        if (currentEditAnswer) { $(currentEditAnswer).blur(); }
        addAnswer(parent, parent.attr("question-id"), function () {
            showOrHideDeleteLineAnswer();
            initEditable();
            initDragAndDrop();
        });
    });
    //separator
    //delete couple of matching
    $("#checklist[content-tab=true] .nt-qanscont .nt-qmatching .nt-qansctrls .bt-delete").live("click", function () {
        var parent = $(this).closest(".nt-qitem");
        var ansline = $(this).closest(".nt-qmatching");
        var ansid = ansline.attr("answer-id");
        deleteAnswer(ansid, function () {
            ansline.fadeOut("fast", function () {
                $(this).remove();
                showOrHideDeleteLineAnswer();
            });

        });
    });
    //delete answer of other types question
    $("#checklist[content-tab=true] .nt-qanscont .nt-qans .nt-qansctrls .bt-delete").live("click", function () {
        var parent = $(this).closest(".nt-qitem");
        var ansline = $(this).closest(".nt-qans.nt-qans-edit");
        var ansid = ansline.attr("answer-id");
        deleteAnswer(ansid, function () {
            ansline.fadeOut("fast", function () {
                $(this).remove();
                showOrHideDeleteLineAnswer();
            });
        });
    });
    //separator
    $("#checklist[content-tab=true] .nt-qtype-sel").live("change", function (ev) {
        var questiontype = $(this).val();
        var itemParent = $(this).closest(".nt-qitem");
        var questionidString = itemParent.attr("question-id");
        if (questionidString) {
            updateQuestionType(questionidString, questiontype, function (html) {
                $(itemParent).replaceWith(html);
                initDragAndDrop();
                initTagsOnQuestion();
                sortByNumberOrLetters();
                initImageUploadFacility();
                initEditable();
            });
        }


    });
    //separator
    $("#qpaste textarea").live("paste", function (ev) {
        setTimeout(function () {
            var textarea = $("#qpaste textarea");
            if (textarea) {
                var pastedText = textarea.val();
                handleImportText(pastedText);
            }
        }, 100);
    });
    $("#qpaste textarea").live("mouseup", function (ev) {
        var text = $(this).val();
        handleImportText(text);
    });
    //separator
    $("#checklist[content-tab=true] .nt-qitem[question-type=Essay] .nt-qoepts input[type=text],#checklist[content-tab=true] .nt-qitem[question-type=ShortAnswer] .nt-qoepts input[type=text]").live("blur", function () {
        var questionIdString = $(this).closest(".nt-qitem").attr("question-id");
        var questionid = parseInt(questionIdString);
        var scoreString = $(this).val();
        var score = parseFloat(scoreString);
        if (!isNaN(questionid) && !isNaN(score)) {
            statusSaving();
            $.post("/Tests/UpdateNoneChoiceScore", { questionid: questionid, score: score }, function (res) {
                if (res.success) {
                    statusSaved();
                    checkMaxScoreAndTotalScore();
                }
                else {
                    showMessage("error", res.message);
                }
            });
        }

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
                    data: JSON.stringify({ testid: testid, userids: checkIds, count: checkIds.length, tab: currentScoreDetailTab }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (res) {
                        if (res.success) {
                            $("#score-container").html(res.generatedHtml);
                            var activeLi = $("#score-detail-tab li").filter(function () {
                                return $("a[tab=" + currentScoreDetailTab + "]", this).length > 0;
                            }).addClass("active");
                            initScoreOnUserChart();
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
    $("#sidebar[content-tab=true] .nt-ctrl-list .nt-qsearch").live("click", function (ev) {
        var questionidString = $(this).attr("question-id");
        var questionid = parseInt(questionidString);
        var templateQuestion;
        if (!isNaN(questionid)) {
            statusSaving();
            $.post("/Tests/CloneQuestion", { targetTestID: testid, questionid: questionid }, function (res) {
                if (res.success) {
                    var newItem = $(res.generatedHtml);
                    var etab = $("#checklist");
                    if ($(".nt-empty-list-ph", etab).length == 1) {
                        etab.html(newItem)
                    } else {
                        etab.append(newItem);
                    }
                    resortInDb();
                    initEditable();
                    initImageUploadFacility();
                    etab.scrollToElement(newItem);
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
    $("#checklist[content-tab=true] .nt-qitem .nt-qans").live("change", function (ev) {
        updateAnswer($(this).closest(".nt-qans"), ev.target);
    });
    $("#checklist[content-tab=true] .nt-qitem .nt-qmatching").live("change", function (ev) {
        var lineContainer = $(this).closest(".nt-qmatching");
        updateAnswer(lineContainer, ev.target);
    });
    //separator
    $("#eventDuplicate").live("click", function (ev) {
        $.post("/Tests/DuplicateTest", { testid: testid }, function (res) {
            if (res.success && res.id) {
                window.location.href = "/Tests/NewTest/" + res.id;
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $("#eventTab .nt-asm-settings .nt-section li").live("change", function (ev) {
        postSetting($(this));
    });
    //separator
    $("button.nt-btn-ri").live("click", function (ev) {
        var row = $(this).closest("tr");
        var userid = parseInt(row.attr("user-id"));
        var userids = new Array();
        userids.push(userid);
        statusSaving();
        $.ajax({
            type: "POST",
            url: "/Tests/ReinviteUserToInvitationTest",
            data: JSON.stringify({ testid: testid, count: userids.length, userids: userids }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.success) {
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                }
            }
        });
    });
    $("button.nt-btn-rm").live("click", function (ev) {
        var row = $(this).closest("tr");
        var userid = parseInt(row.attr("user-id"));
        $.post("/Tests/RemoveUser", { testid: testid, userid: userid }, function (res) {
            if (res.success) {
                row.fadeOut("slow", function () { $(this).remove(); });
                statusSaved();
            } else {
                showMessage("error", res.message);
            }
        });
    });
    //separator
    $("#btn-remove-student,#btn-remove-teacher").live("click", function (ev) {
        var role = "";
        if ($(this).attr("id") == "btn-remove-teacher") { role = "Teacher"; } else { role = "Student"; }
        $.post("/Tests/ModalRemovePopupUser", { testid: testid, role: role }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalRemovePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalRemovePopupUser").html($(html).html());
                }
                $("#modalRemovePopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
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
    $("#modalRemovePopupUser").live("shown", function () {
        var container = $(this);
        var role = $("#role", container).val();
        $(".nt-ctrl-search input[type=text]", container).autocomplete({
            minLength: 0,
            source: function (req, res) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/ModalRemovePopupUser",
                    data: JSON.stringify({ testid: testid, role: role, term: req.term }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        if (r.success) {
                            var list = $(".nt-clb-list", container);
                            list.empty();
                            $(r.resultlist).each(function (i, o) {
                                list.append(o);
                            });
                        } else {
                            showMessage("error", r.message);
                        }
                    }
                });
            }
        });
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
                    $("#modalPopupUser").html($(html).html());
                }
                $("#modalPopupUser").modal("show");
                //separator
            } else {
                showMessage("error", res.message);
            }
        });
    });
    $("#modalPopupUser button.nt-btn-ok").live("click", function (ev) {
        var container = $("#modalPopupUser .nt-clb-list");
        var role = $("#modalPopupUser #role").val();
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
                    $("#eventTab").html(res.generatedHtml);
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                    if (res.generatedHtml) {
                        $("#eventTab").html(res.generatedHtml);
                    }
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
    $("#modalPopupUser .nt-btn-outside").live("click", function () {
        var btn = $(this);
        var toggle = btn.attr("toggle");
        var tb = btn.siblings(".nt-invite-outside");
        var icon = $("i", btn);
        if (toggle) {
            switch (toggle) {
                case "inactive":
                    icon.removeClass("icon-plus").addClass("icon-minus");
                    tb.show("slide", { direction: "left" });
                    btn.attr("toggle", "active");
                    break;
                case "active":
                    icon.removeClass("icon-minus").addClass("icon-plus");
                    tb.hide("slide", { direction: "left" });
                    btn.attr("toggle", "inactive");
                    break;
                default:
                    break;
            }
        }
    });
    $("#modalPopupUser input[type=text].nt-email-outside,#modalPopupUser input[type=text].nt-name-outside").live("keydown", function (ev) {
        var keyCode = ev.keyCode;
        if (keyCode == 13) {
            var control = $(this).closest(".nt-invite-outside");
            var tbName = $(".nt-name-outside", control);
            var tbEmail = $(".nt-email-outside", control);
            $.validity.start();
            $.validity.settings.position = "top";
            tbName.require();
            tbEmail.require().match("email");
            $.validity.settings.position = "left";
            var result = $.validity.end();
            if (result.valid) {
                var container = control.closest(".modal");
                var name = tbName.val();
                var email = tbEmail.val();
                $.post("/Tests/InviteUserOutSide", { testid: testid, email: email, name: name }, function (res) {
                    if (res.success) {
                        tbName.val("");
                        tbEmail.val("");
                        var html = $(res.generatedHtml);
                        var list = $(".nt-clb-list", container);
                        list.append(html);
                        list.scrollToElement(html);
                    }
                    else {
                        showMessage("error", res.message);
                    }
                });
            }
        }
    });
    $("#modalPopupUser").live("shown", function () {
        var container = $(this);
        var role = $("#role", container).val();
        $(".nt-ctrl-search input[type=text]", container).autocomplete({
            minLength: 0,
            source: function (req, res) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/ModalPopupUser",
                    data: JSON.stringify({ testid: testid, term: req.term }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        if (r.success) {
                            var list = $(".nt-clb-list", container);
                            list.empty();
                            $(r.resultlist).each(function (i, o) {
                                list.append(o);
                            });
                        } else {
                            showMessage("error", r.message);
                        }
                    }
                });
            }
        });
    });
    $("#modalPopupUser .button button").live("click", function () {
        var btn = $(this);
        var userid = parseInt(btn.attr("user-id"));
        if (!isNaN(userid)) {
            $.post("/Users/RemoveNonRegisteredUser", { userid: userid }, function (res) {
                if (res.success) {
                    var item = $(btn).closest(".nt-clb-item");
                    item.fadeOut("slow", function () { $(this).remove(); });
                }
                else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    //Separator
    $("#btn-reinvite-student,#btn-reinvite-teacher").live("click", function (ev) {
        var role = "";
        if ($(this).attr("id") == "btn-reinvite-teacher") { role = "Teacher"; } else { role = "Student"; }
        $.post("/Tests/ModalReinvitePopupUser", { testid: testid, role: role }, function (res) {
            if (res.success) {
                var html = res.generatedHtml;
                if (!$("#modalReinvitePopupUser").length > 0) {
                    $(html).modal();
                } else {
                    $("#modalReinvitePopupUser").html($(html).html());
                }
                $("#modalReinvitePopupUser").modal("show");

            } else {
                showMessage("error", res.message);
            }
        });
    });
    $("#modalReinvitePopupUser button.nt-btn-ok").live("click", function (ev) {
        var container = $("#modalReinvitePopupUser .nt-clb-list");
        var checkedCheckbox = $("input[type=checkbox]:checked", container);
        var chekcedIds = checkedCheckbox.map(function (index, element) {
            return parseInt($(element).attr("user-id"));

        }).convertJqueryArrayToJSArray();
        statusSaving();
        $.ajax({
            type: "POST",
            url: "/Tests/ReinviteUserToInvitationTest",
            data: JSON.stringify({ testid: testid, count: chekcedIds.length, userids: chekcedIds }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (res) {
                if (res.success) {
                    statusSaved();
                } else {
                    showMessage("error", res.message);
                }
            }
        });
        $("#modalReinvitePopupUser").modal('hide');
    });
    $("#modalReinvitePopupUser").live("shown", function () {
        var container = $(this);
        var role = $("#role", container).val();
        $(".nt-ctrl-search input[type=text]", container).autocomplete({
            minLength: 0,
            source: function (req, res) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/ModalReinvitePopupUser",
                    data: JSON.stringify({ testid: testid, role: role, term: req.term }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        if (r.success) {
                            var list = $(".nt-clb-list", container);
                            list.empty();
                            $(r.resultlist).each(function (i, o) {
                                list.append(o);
                            });
                        } else {
                            showMessage("error", r.message);
                        }
                    }
                });
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
                //sortByNumberOrLetters();
                initPopover();
                $("#sidebar[content-tab=true]").accordion({ heightStyle: "content" });
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
    $("#score-detail-tab li a[tab]").live("click", function (ev) {
        ev.preventDefault();
        var tab = $(this).attr("tab");
        if (tab) {
            currentScoreDetailTab = tab;
            var checkIds = $("input[type=checkbox][user-id]:checked").map(function (i, e) {
                return $(e).attr("user-id");
            }).convertJqueryArrayToJSArray();
            $.ajax({
                type: "POST",
                url: "/Tests/NewTest_ScoreTab_CheckUserIds",
                data: JSON.stringify({ testid: testid, userids: checkIds, count: checkIds.length, tab: currentScoreDetailTab }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (res) {
                    if (res.success) {
                        $("#score-container").html(res.generatedHtml);
                        var activeLi = $("#score-detail-tab li").filter(function () {
                            return $("a[tab=" + currentScoreDetailTab + "]", this).length > 0;
                        }).addClass("active");
                        initScoreOnUserChart();
                        initPlot();
                    } else {
                        showMessage("error", res.message);
                    }
                }
            });
        }
    });
    //separator
    $(".feedback-tab li a[tab]").live("click", function (ev) {
        ev.preventDefault();
        var tab = $(this).attr("tab");
        if (tab) {
            currentScoreDetailTab = tab;
            $.post("/Tests/NewTest_FeedBackTab", { testid: testid, feedbacktab: currentScoreDetailTab }, function (res) {
                if (res.success) {
                    var evTab = $("#eventTab");
                    if (evTab) { evTab.html(res.generatedHtml); }
                } else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    //separator
    $(".reply-container[toggle-header]").live("click", function () {
        var cur = $(this);
        var detail = cur.siblings(".reply-container[toggle-detail]");
        cur.hide();
        detail.show();
    });
    $(".reply-container[toggle-detail] div[toggle-detail-trigger]").live("click", function () {
        var cur = $(this).closest(".reply-container[toggle-detail]");
        var header = cur.siblings(".reply-container[toggle-header]");
        cur.hide();
        header.show();
    });
    //separator
    $("#checklist .nt-qitem .nt-tag-adder input[type=text]").live("keydown", function (ev) {
        if (ev.keyCode == 13) {
            var tb = $(this);
            var questionIdString = tb.closest(".nt-qitem").attr("question-id");
            var questionid = parseInt(questionIdString);
            var text = tb.val();
            $.post("/Tests/AddTagToQuestion", { questionid: questionid, tagid: 0, tagname: text }, function (res) {
                if (res.success) {
                    var tags = tb.closest(".nt-tags");
                    var container = $(".tags-container", tags);
                    var tagItem = $(res.generatedHtml);
                    $(container).append(tagItem);
                    statusSaved();
                    tb.val("");
                }
                else { showMessage("error", res.message); }
            });
        }
    });
    //separator
    $("#intro-detail").live("change", function () {
        updateTestIntroduction();
    });
    $("#intro-detail").live("blur", function () {
        var btn = $("#eventIntroduction");
        if (btn.hasClass("active")) {
            updateTestIntroduction();
            btn.removeClass("active");
            $("#test-introduction").hide();
        }
    });
    $("#eventIntroduction").live("click", function () {
        var btn = $(this);
        if (btn.hasClass("active")) {
            btn.removeClass("active");
            $("#test-introduction").hide();
        } else {
            btn.addClass("active");
            $("#test-introduction").show();
        }
    });
    $("#test-introduction").clickout({
        callback: function (ev) {
            var btn = $("#eventIntroduction");
            if (btn.hasClass("active")) {
                updateTestIntroduction();
                btn.removeClass("active");
                $("#test-introduction").hide();
            }
        },
        excepts: "#eventIntroduction",
    });


    //separator
    var reuseAutoComplete = $("#sidebar .nt-ctrl-search input[type=text]").autocomplete({
        minLength: 0,
        source: function (req, res) {
            if (reuseAddedTags) {
                $.ajax({
                    type: "POST",
                    url: "/Tests/ReuseSearchQuestionTemplate",
                    data: JSON.stringify({ term: req.term, tagids: reuseAddedTags }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (r) {
                        if (r.success) {
                            var list = $("#sidebar .nt-ctrl-list[data-result]");
                            list.empty();
                            $(r.resultlist).each(function (i, o) {
                                list.append(o);
                            });
                            initReuseDragAndDrop();
                            initPopover();
                        } else {
                            showMessage("error", r.message);
                        }
                    }

                });
            }
        }
    });
    $("#sidebar .nt-tag-adder input[type=text]").reuseTagSearch({
        container: ".nt-taglist.nt-tags",
        tagsource: function (req, res) {
            $.ajax({
                type: "POST",
                url: "/Tests/TestsSearchTag",
                data: JSON.stringify({ term: req }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    if (r.success) {
                        var result = $(r.resultlist).map(function (index, obj) {
                            if (obj.TagName && obj.TagName != "") {
                                return { id: obj.TagID, name: obj.TagName };
                            }
                        }).convertJqueryArrayToJSArray();
                        res(result);
                    } else {
                        showMessage("error", r.message);
                    }
                }
            });
        },
        tagschange: function (addedTags) {
            reuseAddedTags = addedTags;
            reuseAutoComplete.autocomplete("search");
        }
    });
    //separator
    $("#checklist[response-tab=true] .nt-qitem .nt-qoepts input[type=text].nt-pts").live("change", function () {
        var valueString = $(this).val();
        var value = parseFloat(valueString);
        var questionIdString = $(this).attr("question-id");
        var questionid = parseInt(questionIdString);
        var checkedUserCb = $("#respUsers input[type=checkbox]:checked");
        var userid;
        if (checkedUserCb) {
            userid = parseInt(checkedUserCb.attr("user-id"));
        }
        if (!isNaN(value) && questionid && !isNaN(questionid) && userid && !isNaN(userid)) {
            statusSaving();
            $.post("/Tests/UpdateUserNoneChoiceScore", { questionid: questionid, userid: userid, score: value }, function (res) {
                if (res.success) {
                    statusSaved();
                    if (res.data) {
                        var checkedItems = $("#respUsers .nt-clb-item").filter(function () {
                            return $("input[type=checkbox]:checked", this).length > 0;
                        });
                        if (checkedItems.length == 1) {
                            $(".nt-clb-item-desc", checkedItems).html(res.data);
                        }
                    }
                } else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    //separator
    $("#btn-download-score").live("click", function () {
        var checkIds = $("#respUsers input[type=checkbox][user-id]:checked").map(function (i, e) {
            return $(e).attr("user-id");
        }).convertJqueryArrayToJSArray();
        var ids = checkIds.join("&userids=");
        window.location.href = "/Tests/ScoreToExcel?testid=" + testid + "&&userids=" + ids;
    });
    //separator
    $(".nt-qitem .preview-container .nt-qimg-close").live("click", function () {
        var item = $(this).closest(".nt-qitem");
        var id = item.attr("question-id");
        if (!isNaN(id)) {
            statusSaving();
            $.post("/Tests/DeleteImage", { questionid: id }, function (res) {
                if (res.success) {
                    item.replaceWith($(res.generatedHtml));
                    initImageUploadFacility();
                    statusSaved();
                }
                else {
                    showMessage("error", res.message);
                }
            });
        }
    });
    //separator
    showOrHideDeleteLineAnswer();
    sortByNumberOrLetters();
    //separator
    handleScoreStatisticSubEvents();
    //separator
    initClientSorting();
    //separator
    var hub = $.connection.generalHub;
    hub.client.R_removeInvitation = function (tid, userids) {
        if (tid && userids) {
            if (tid == testid) {
                var isIn = $.inArray(userid, userids) >= 0;
                if (isIn) {
                    showCountDownMessage("info", "You are current removed from invitation of this test", "Redirect to Homepage", function () {
                        window.location.href = "/Tests";
                    });
                }
            }
        }
    }
    hub.client.R_AcknowledgeEmailCallback = function (uid, initMailCount, sentCount, unSentCount, listSent) {
        if (uid && uid == userid) {
            if (typeof (initMailCount) != "undefined" && typeof (sentCount) != "undefined" && typeof (unSentCount) != "undefined" && typeof (listSent) != "undefined") {
                var message = "";
                var type = "";
                if (unSentCount) {
                    type = "error";
                    message = "Unable to send all emails from invitation.</br>" + "Total : " + initMailCount + "</br>" + sentCount + " was sent.</br>" + unSentCount + " was Un-sent.";
                } else {
                    type = "info";
                    message = "Sent " + sentCount + " invitation emails.";
                }
                //re-render
                $(listSent).each(function () {
                    var id = this;
                    if (id) {
                        var r = $(".nt-asm-inv-cont table tr[invitation-id=" + id + "]");
                        if (r.length > 0) {
                            r.fadeOut("fast", function () {
                                var row = $(this);
                                row.addClass("sent").removeClass("unsent");
                                var status = $("span.label", row);
                                status.removeClass("label-important").addClass("label-info");
                                status.html("Mail Sent");
                                row.fadeIn("fast");
                            });
                        }
                    }
                });
                showMessage(type, message);
            }
        }
    }
    hub.client.R_teacherAndTeacherCommentFeedback = function (tid, generatedHtml) {
        if (tid && generatedHtml) {
            if (tid == testid) {
                var empty = $("#eventTab .nt-checklist-ctrl");
                if (empty.length > 0) {
                    empty.siblings(".comment-count").show();
                    empty.siblings("#comments[teacher]").show();
                    empty.remove();
                }
                var comments = $("#comments[teacher]");
                if (comments.length > 0) {
                    var ele = $(generatedHtml);
                    comments.prepend(ele);
                    var articleCount = $("article", comments).length;
                    $(".comment-count span").html(articleCount);
                }
            }
        }
    }
    hub.client.R_studentAndTeacherCommentFeedback = function (tid, generatedHtml) {
        if (tid && generatedHtml) {
            if (tid == testid) {

                var comments = $("#comments[student]");
                if (comments.length > 0) {
                    var ele = $(generatedHtml);
                    comments.prepend(ele);
                    var articleCount = $("article", comments).length;
                    $(".comment-count span").html(articleCount);
                }
            }
        }
    }
    hub.client.R_teacherAndTeacherReplyFeedback = function (tid, parentFeedBackId, generatedHtml) {
        if (tid && parentFeedBackId && generatedHtml) {
            if (tid == testid) {
                var article = $("#comments[teacher] article[parent-id=" + parentFeedBackId + "]");
                if (article.length > 0) {
                    var ele = $(generatedHtml);
                    $(".reply-details", article).append(ele);
                    var count = $(".reply-detail", article).length;
                    $(".reply-count-link span[data-count]", article).html(count);
                }
            }
        }
    }
    hub.client.R_studentAndTeacherReplyFeedback = function (tid, parentFeedBackId, generatedHtml) {
        if (tid && parentFeedBackId && generatedHtml) {
            if (tid == testid) {
                var article = $("#comments[student] article[parent-id=" + parentFeedBackId + "]");
                if (article.length > 0) {
                    var ele = $(generatedHtml);
                    $(".reply-details", article).append(ele);
                    var count = $(".reply-detail", article).length;
                    $(".reply-count-link span[data-count]", article).html(count);
                }
            }
        }
    }
    hub.client.R_deactivetest = function (id, mail) {
        if (id && id == testid) {
            showCountDownMessage("info", "User with email:" + mail + " disabled this test", "Redirect to Homepage", function () {
                window.location.href = "/Tests";
            });
        }
    }
    hub.client.R_deleteTestPermanent = function (id, mail) {
        if (id && id == testid) {
            showCountDownMessage("info", "User with email:" + mail + " PERMANENTLY DELETE this test", "Redirect to Homepage", function () {
                window.location.href = "/Tests";
            });
        }
    }

    $.connection.hub.start().done(function () {
        $("#eventDeactive").live("click", function () {
            $.post("/Tests/DeActiveTest", { testid: testid }, function (res) {
                if (!res.success) { showMessage("error", res.message); }
                else {
                    showMessage("success", res.message);
                }
            });
        });
        $("#eventDelete").live("click", function () {
            $("#confirmDelete").modal("show");
        });
        $("#confirmDelete .nt-btn-ok").live("click", function () {
            $("#confirmDelete").modal("hide");
            $.post("/Tests/DeleteTestPermanent", { testid: testid }, function (res) {
                if (res.success) {
                    showMessage("success", res.message);
                }
                else {
                    showMessage("error", res.message);
                }
            });
        });

        $("#comments .reply-button").live("click", function () {
            var button = $(this);
            var parentFeedbackID = parseInt(button.val());
            var container = button.closest(".reply-container");
            var area = $(".reply-area", container);
            var text = area.val();
            if (text) {
                var type = "";
                var comments = $("#comments");
                if (comments.attr("student") == "true") {
                    type = "StudentAndTeacher";
                }
                else if (comments.attr("teacher") == "true") {
                    type = "TeacherAndTeacher";
                }
                var place = $(".reply-details", container);
                $.post("/Tests/UserReplyFeedBack", { testid: testid, parentFeedBackId: parentFeedbackID, replyDetail: text, role: type }, function (res) {
                    if (res.success) {
                        if (area) { area.val(""); }
                    } else {
                        showMessage("error", res.message);
                    }
                });
            }
        });

        $("#teacher-feedback-submit").live("click", function () {
            var area = $("#teacher-feedback-content");
            var text = area.val(); //get text
            if (text) {
                $.post("/Tests/UserCommentFeedBack", { testid: testid, fbDetail: text, role: "TeacherAndTeacher" }, function (res) {
                    if (res.success) {
                        area.val(""); //clear text
                    } else {
                        showMessage("error", res.message);
                    }
                });
            }
        });
    });

});