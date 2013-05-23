$(function () {
    $.post("/Tests/TestsSearch", function (res) {
        if (res)
        {
            //window.StudentsList = res;
            var source = res.map(function (obj, index)
            {
                return { label: obj.TestTitle, value: obj.Id };
            });

            $(".navbar-search .nt-search-input").autocomplete({
                minLength: 0,
                source: source,

                focus: function (event, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);
                    return false;
                },              

                select: function (event, ui) {
                    $(".navbar-search .nt-search-input").val(ui.item.label);

                    var obj = null;
                    for (var i = 0; i < res.length; i++) {
                        if (res[i].Id == ui.item.value) {
                            obj = res[i];
                            break;
                        }
                    }
                    if (obj) {
                        var str = '<div class="">'
                            + '<span>' + obj.TestTitle + '</span> | '
                            + '<span>' + obj.StartDate + '</span> | '
                            + '<span>' + obj.EndDate + '</span>'
                            + '</div>';

                    /*    var element = $('#running-and-upcoming-panel .nt-list-cont div');
                        if (element.size() == 1) {
                            $(element.get(0)).hasClass('nt-empty-list-ph').remove();
                        }
                        */

                        $('#running-and-upcoming-panel .nt-list-cont div').html(str);
                    } else {
                        $('#running-and-upcoming-panel .nt-list-cont').html('<div class="nt-empty-list-ph"><p>No running or upcoming tests available.</p></div>');
                    }
                    //console.log(ui);
                    return false;
                }

            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                if (!ul.hasClass("search-autocomple")) { ul.addClass("search-autocomple");}
                var li = $("<li>").append("<a>" + item.label + "</a>");
                if (!li.hasClass("search-autocomplete-hover-item")) { li.addClass("search-autocomplete-hover-item"); }
                li.appendTo(ul);
                return li;
            };

           
            
        }
    });
   
});

