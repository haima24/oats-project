$(function () {
    $("#sidebar").accordion();
    
    $("#checklist[content-tab=true] .nt-conted-ph-cont").hover(function (ev) {
        var ea = $(ev.currentTarget).find('[contenteditable=true]')[0];
        $(ea).addClass("nt-contedhover");
    }, function (ev) {
        var er = $(ev.currentTarget).find('[contenteditable=true]')[0];
        $(er).removeClass("nt-contedhover");
    });
    $(':not(.nt-qitem)').live("click", function (ev) {
        if ($("#checklist[content-tab=true]").length > 0) {
            if (!($(ev.target).parents('.nt-qitem').length || $(ev.target).is('#main'))) {
                var all = $(".nt-qitem");
                all.removeClass('nt-qitem-edit-act');
                all.removeClass('nt-qitem-edit-inact');
                all.addClass('nt-qitem-edit-inact');
                $(".nt-qansscore").hide();
                $(".nt-scrbtn").removeClass("active");
                if (!$(ev.target).is('select')) {
                    all.find('.nt-qctrls').hide();
                }
                all.find('.nt-qtagcont').show();
                all.find('.nt-tag-adder').show();
                all.find('.tags-container').show();
                all.find('button.nt-btn-sqr.nt-qctrls-qtags-toggle').hide();
            }
        }
    });
    $("#checklist[content-tab=true] .nt-qitem").live('click', function (ev) {
        if($(this).parent().length!=0){
            var all = $(".nt-qitem");
            all.removeClass('nt-qitem-edit-act');
            all.removeClass('nt-qitem-edit-inact');
            $(this).addClass('nt-qitem-edit-act').siblings().addClass('nt-qitem-edit-inact');
            var current = ev.target;
            var pr = $(current).closest('.nt-qitem');
            var tTag = $(current).is(function () {
                return $(this).is("i.i-tag") || $("i.itag", this).length > 0;
            });
            if (current.parentNode && (tTag || $(current).closest(".nt-tag-adder").hasClass('nt-tag-adder') || $(current).closest(".tags-container").hasClass('tags-container'))) {
                var btn = pr.find('button.nt-btn-sqr.nt-qctrls-qtags-toggle')[0];
                $(pr.find('.nt-qctrls')[0]).hide();
                $(pr.find('.nt-qtagcont')[0]).show();
                $(btn).find('i').removeClass('i-tag').addClass('i-setting');
                $(btn).show();
                $(".nt-tag-adder", pr).show();
            } else {
                var btn = pr.find('button.nt-btn-sqr.nt-qctrls-qtags-toggle')[0];
                $(pr.find('.nt-qctrls')[0]).show();
                $(pr.find('.nt-tag-adder')[0]).hide();
                $(".tags-container", pr).hide();
                $(btn).find('i').removeClass('i-setting').addClass('i-tag');
                $(btn).show();
                var btnAdd = pr.find('button.nt-btn-text.nt-qansadd')[0];
                $(btnAdd).show();
            }
        }
    });
});