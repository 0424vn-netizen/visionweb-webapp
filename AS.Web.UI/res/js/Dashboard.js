$(document).ready(function () {
    updateClientFrameInfo();
    $("table#tblCardVolume tr:last").addClass("Footer").removeClass("Row");

    var space = /Chrome/i.test(window.navigator.userAgent) ? 6 : 0; // if browser is Chrome then return 6 else 0
    $('.snob-custom-format').css('margin-left', ($(".snob").textWidth() / 2 - space) + 'px');

    $('table#tblVolumeAnalysis tr:odd').addClass('Row');
    $('table#tblVolumeAnalysis tr:even').addClass('AltRow');
    if (isFultonClient == 'True') {
        $('table#tblVolumeAnalysis td#keyedCount').addClass('indented');
    }
});
$(window).load(function () {
    $(".as-animated-tabstrip").asAnimatedTab();

});
$(window).resize(function () {
    $(".as-animated-tabstrip").asAnimatedTab();
});

function updateClientFrameInfo() {
    PageMethods.UpdateClientFrameInfo(window.name);
}

function CallFailed(res) {
    return false;
}
function CallBackSuccess(res) {
    alert(Dashboard_js_Updated + res);
    return false;
}

function tabSelected(sender, e) {
    var tab = e.get_tab();
    $(".uc-content").addClass("hide");
    $("#" + tab.get_value()).removeClass("hide");
    // refresh kendo charts
    refreshKendoChart();
}
jQuery.fn.textWidth = function () {
    var _t = jQuery(this);
    var html_org = _t.html();
    if (_t[0].nodeName == 'INPUT') {
        html_org = _t.val();
    }
    var html_calcS = '<span>' + html_org + '</span>';
    jQuery('body').append(html_calcS);
    var _lastspan = jQuery('span').last();

    _lastspan.css({
        'font-size': _t.css('font-size'),
        'font-family': _t.css('font-family')
    })
    var width = _lastspan.width() + 5;
    _lastspan.remove();
    return width;
};
$(".snob").snob({
    'format': function (value) {
        return value == 100 ? value : value.toFixed(2);
    }
});