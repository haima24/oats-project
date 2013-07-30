﻿var testid;
function initWYSIWYG() {
    $("[data-original-title]").tooltip();
    $("#checklist .nt-qitem[question-type=Essay] div.normal-editable.nt-qrespinput").wysiwyg();
}
function submitTest() {
    var userInTest = new Object();
    userInTest.TestID = testid;
    userInTest.UserInTestDetails = $("#checklist .nt-qitem").map(function (i, e) {
        var userInTestDetail = new Object();
        userInTestDetail.QuestionID = parseInt($(e).attr("question-id"));
        var str = "";
        $(".nt-qans input[type=radio]:checked,.nt-qans input[type=radio]:checked", e).each(function (index, element) {

            if ($(element).attr("id")) {
                if (index == 0) {
                    str += $(element).attr("id");
                } else {
                    str += "," + $(element).attr("id");
                }
            }
        });
        $(".nt-qmatching .begin", e).each(function (index, element) {
            var $element = $(element);
            if ($element.attr("answer-id")) {
                if (!str) {
                    str += $element.attr("answer-id") + "," + ($element.attr("matching-id") || "");
                } else {
                    str += ";" + $element.attr("answer-id") + "," + ($element.attr("matching-id") || "");
                }
            }
        });
        userInTestDetail.AnswerIDs = str;
        userInTestDetail.AnswerContent = $(".nt-qrespinput", e).is("textarea,input") ? $(".nt-qrespinput", e).val() : $(".nt-qrespinput", e).html();

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
                showCountDownMessage("info", res.message, "Redirect to Homepage", function () {
                    window.location.href = "/Tests";
                });
            } else {
                showMessage("error", res.message);
            }
        }
    });
}

$.fn.extend({
    _matchingConnectorItem: function () {
        var curColor = 0;
        var $this = this;
        var keymap = new Array();
        var colours = ["purple", "red", "orange", "yellow", "lime", "green", "blue", "navy", "black", "teal", "fuchsia"];
        var curLines = [];
        var svg;
        var _updateHtmlFromKeyMap = function () {
            var qContainer = $this;
            //update html attributes here
            $(".nt-qmatching .begin[answer-id]", qContainer).each(function () { $(this).removeAttr("matching-id"); });
            $(keymap).each(function () {
                var qAnsContainer = qContainer;
                var map = this;
                var beginId = this.start;
                var endId = this.end;
                var htmlElement = $(".nt-qmatching .begin[answer-id=" + beginId + "]", qAnsContainer);
                if (htmlElement) {
                    htmlElement.attr("matching-id", endId);
                }
            });
        };
        var _onPutKeyMap = function (obj) {
            keymap = $.grep(keymap, function (item, i) {
                if (item.start == obj.start || item.end == obj.end) {
                    item.line.remove();
                    return true;
                }
            }, true);

            keymap.push(obj);
            _updateHtmlFromKeyMap();
        };
        var _onDeleteLine = function (line) {
            keymap = $.grep(keymap, function (e, i) {
                if (e.line == line) {
                    line.remove();
                    return true;
                }
            }, true);
            _updateHtmlFromKeyMap();
        };
        var svgClear = function () {
            svg.clear();
        };
        var svgDrawLine = function (eTarget, eSource) {
            setTimeout(function () {

                var $source = eSource;
                var $target = eTarget;

                // origin -> ending ... from left to right
                // 10 + 10 (padding left + padding right) + 2 + 2 (border left + border right)
                var originX = $source.parent().position().left + $source.width() + 12 + 4;
                var originY = $source.parent().position().top + (($source.height() + 12 + 4) / 2);

                var endingX = $target.parent().position().left;
                var endingY = $target.parent().position().top + (($target.height() + 12 + 4) / 2);

                var space = 0;

                if (curColor >= colours.length) { curColor = 0; }
                var color = colours[++curColor];

                // draw lines
                // http://raphaeljs.com/reference.html#path         
                var a = "M" + originX + " " + originY + " L" + (originX + space) + " " + originY; // beginning
                var b = "M" + (originX + space) + " " + originY + " L" + (endingX - space) + " " + endingY; // diagonal line
                var c = "M" + (endingX - space) + " " + endingY + " L" + endingX + " " + endingY; // ending
                var all = a + " " + b + " " + c;


                // write line
                var line = svg.path(all);
                line.attr({
                    "stroke": color,
                    "stroke-width": 2
                });
                line.mouseover(function () {
                    this.attr({ "stroke-opacity": 0.5 });
                }).mouseout(function () {
                    this.attr({ "stroke-opacity": 1 });
                });
                line.dblclick(function () {
                    _onDeleteLine(this);
                });
                curLines[curLines.length] = line;
                var obj = new Object();
                var beginId = parseInt(eSource.closest(".nt-qans").attr("answer-id"));
                var endId = parseInt(eTarget.closest(".nt-qans").attr("answer-id"));
                if (!isNaN(beginId) && !isNaN(endId)) {
                    obj.start = beginId;
                    obj.end = endId;
                    obj.line = line;
                    if (_onPutKeyMap && typeof (_onPutKeyMap) === "function") {
                        _onPutKeyMap(obj);
                    }
                }

            }, 1000);
        }
        var painter = $(".painter", $this);
        $(".begin .matcher", $this).draggable({
            connectWith: ".end .matcher",
            helper: "clone",
            revert: "invalid"
        });
        $(".end .matcher", $this).droppable({
            hoverClass: "matcher-hover",
            helper: "clone",
            cursor: "move",
            drop: function (event, ui) {
                svgDrawLine($(this), $(ui.draggable));
            }
        });
        svg = Raphael(painter.get(0), $this.width(), $this.height());
    },
    matchingConnector: function () {
        $(this).each(function (index, element) {
            var $qItem = $(element);
            var qCount = $(".nt-qanscont", $qItem);
            $(qCount)._matchingConnectorItem();
        });
    }
});


$(function () {
    testid = parseInt($("#test-id").val());
    var duration = parseInt($("#test-duration").val()),
        ts = (new Date()).getTime() + duration * 60 * 1000;
    $('#countdown').countdown({
        timestamp: ts,
        timeout: function () {
            submitTest();
        }
    });


    $("#checklist .nt-qitem .nt-qnum:not(.nt-qnum-letter)").each(function (index, element) {
        $(element).html(index + 1 + ". ");
    });
    $("#checklist .nt-qitem .nt-qnum.nt-qnum-letter").each(function (index, element) {
        $(element).html(String.fromCharCode(65 + index) + ". ");
    });

    initWYSIWYG();
    $(".nt-qitem[question-type=Matching]").matchingConnector();
    $("#submit-btn").live("click", function (ev) {
        submitTest();
    });

    var hub = $.connection.generalHub;
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
    $.connection.hub.start();
});