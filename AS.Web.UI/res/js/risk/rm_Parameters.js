$().ready(function () {
    $('.rgGroupCol').attr('style', 'border-right:none');
    SetSizeForFixArea();
    minHeightData();
});
$(window).resize(function () {
    SetSizeForFixArea();
    minHeightData();
});

function SetSizeForFixArea() {
    var widthPage = $("#fixCommandArea").parent().width();
    var bgBodyColor = $("body").css("background-color");
    $("#fixCommandArea").css({ "margin-left": "-25px", "bottom": "0px", "position": "fixed", "width": (widthPage + 50) + "px", "border-bottom": "21px solid " + bgBodyColor });
    $("#commandArea").css({ "background": "#FFF", "box-shadow": "0 -2px 1px rgb(238, 238, 238)", "margin-left": "25px", "text-align": "right", "width": widthPage + "px" });
}

function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxOnResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}
//function FixButton() {
//    var isIE6 = navigator.userAgent.toLowerCase().indexOf('msie 6') != -1;
//    if (isIE6) {
//        //alert((window.getScreenHeight() - 25) + 'px');
//        jQuery('pnStatus').css('top', (window.getScreenHeight() - 25) + 'px');
//    }
//    else {
//        //alert((window.getScreenHeight() - 25) + 'px');
//        var bt = $get('pnStatus');
//        bt.style.top = (window.getScreenHeight() - 25) + 'px';
//    }

//    //process on window resize.
//    window.onresize = window_Resize;
//}

//function window_Resize() {
//    ///<summary>
//    ///Process on window resizing.
//    ///</summary>

//    var bt = $get('pnStatus');
//    bt.style.top = (window.getScreenHeight() - 25) + 'px';   
//}

////setTimeout("FixButton();", 600);
//FixButton();

function btnSave_Click() {
    //call API from UxParameter user control.
    doSave();
}
function minHeightData() {
    if ($("#fixCommandArea").length > 0) {
        var heightEl = $(window).height() - ($(".site-header").outerHeight() + $(".welcomebar.navbar").height() + $(".report-title-no-filter").outerHeight(true) + $("#fixCommandArea").height());
        $(".parameter-data").css("min-height", heightEl);
    }
}