function showError(id, msg) {
    $('#list-attribute-name-container').find('label[type=input][for=' + id + ']').addClass('label-error');
    $('#list-attribute-name-container').find('label.error[for=' + id + ']').text(msg);
    $('#list-attribute-name-container').find('label.error[for=' + id + ']').show();
}
function clearError(id) {
    $('#list-attribute-name-container').find('label[type=input][for=' + id + ']').removeClass('label-error');
    $('#list-attribute-name-container').find('label.error[for=' + id + ']').hide();
}

function doValidation(s, id) {
    var inputValue = $(s).parents('.item-panel').find('.toggle-input').first();
    //if (inputValue.val().length == 0) {
    //    showError($(inputValue).attr('id'), requiredMsg);
    //    return false;
    //}
    //else
    if (!checkName(inputValue.val())) {
        showError($(inputValue).attr('id'), invalidMsg);
        return false;
    }
    else {
        __doPostBack(id, '');
    }
}

function customLoadingPanel_OnClientShowing() {
}

function checkName(value) {
    var reg = new RegExp("^((?!(<[^ \t]))(?!(&#)).)*$");
    return reg.test(value);
}
function hideAll() {
    $('.toggle-control').each(function () {
        $(this).find('input').each(function () {
            $(this).addClass('hide');
        })
    });
}

function ShowHideControls(isHidebuttons) {
    $('.toggle-input').focusin(function () {
        hideAll();
        $(this).parents('.item-panel').find('.toggle-control').find('input').each(function () {
            $(this).removeClass('hide');
        })
    });
    $('.btn-cancel').click(function () {
        $(this).parents('.toggle-control').find('input').each(function () {
            $(this).addClass('hide');
        });
        $(this).parents('.item-panel').find('.toggle-input').val($(this).parents('.item-panel').find('.toggle-input-oldvalue').val());
        clearError($(this).parents('.item-panel').find('.toggle-input').attr('id'));
        return false;
    });
    $(document).click(function (e) { if ($(e.target).parents('div.actived').length == 0) $('input[type="button"]').addClass('hide') });
    $('input[type="text"]').click(function () { $(this).parents('div.item-panel').addClass('actived') });
}

$(document).ready(function () {
    ShowHideControls();
    $(document).click(function (e) { if ($(e.target).parents('div.actived').length == 0) $('input[type="button"]').addClass('hide') });
    $('input[type="text"]').click(function () { $(this).parents('div.item-panel').addClass('actived') });

});