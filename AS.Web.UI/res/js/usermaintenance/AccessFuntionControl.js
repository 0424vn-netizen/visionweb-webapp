$(document).ready(function () {
    $('.treeview .parent').click(function () {
        var target = $(this).attr('data-target');
        $(this).parent('.treeview').find('.child').each(function () {
            if ($(this).attr('data-target') == target) {
                if ($(this).hasClass('display-none')) {
                    $(this).parent('.treeview').find('.parent').each(function () {
                        if ($(this).attr('data-target') == target) {
                            $(this).addClass('minus');
                            return;
                        }
                    });
                    $(this).removeClass('display-none');
                }
                else {
                    $(this).addClass('display-none');
                    $(this).parent('.treeview').find('.parent').each(function () {
                        if ($(this).attr('data-target') == target) {
                            $(this).removeClass('minus');
                            return;
                        }
                    });
                }
            }
        });
        ShowHideGroupHeader();
    });
    ShowHideGroupHeader();
});

function ShowHideGroupHeader() {
    $('.treeview .parent').each(function () {
        var it = $(this);
        var target = it.attr('data-target');
        var s = $('.treeview .child[data-target="' + target + '"]').length;
        var h = $('.treeview .child.hide[data-target="' + target + '"]').length;
        if (s = h) {
            it.addClass('hide');
        }
        else {
            it.removeClass('hide');
        }
    });
}
