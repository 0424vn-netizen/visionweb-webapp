function OpenInstanceWindow(url, name) {
    var width = screen.availWidth * 0.9;
    var height = screen.availHeight * 0.8;
    var left = (screen.availWidth / 2) - (width / 2);
    var top = (screen.availHeight / 2) - (height / 2);
    var pwin = window.open(url, "_blank", "scrollbars=1,menubar=0,toolbar=0,resizable=1,location=0,width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=" + top.toString(), true);
    if (pwin != null) {
        pwin.focus();
    }
}

function MerchantNumberClick() {
    $get(rm_DQNextQReportPopup_btnClickMerchantNumber).click();
}

function ReloadParent(url) {
    window.opener.location.href = url;
}

var isAllowClickNext;
$(document).ready(function () {
    doPreloadMerchant();
    doInitialNextButton();
    keepSessionActive();
});

//$(window).bind("load", function () {
//    doPreloadMerchant();
//});

function doInitialNextButton() {
    isAllowClickNext = true;
    $('#' + rm_DQNextQReportPopup_uxNext).removeAttr('disabled');
    $('#' + rm_DQNextQReportPopup_uxNext).click(function (e) { return DisableNextQueueButton(); });
}
function DisableNextQueueButton() {
    // Only allow server submission with the first click
    if (isAllowClickNext) {
        isAllowClickNext = false;
        setTimeout(function () {
            $('#' + rm_DQNextQReportPopup_uxNext).blur();
            $('#' + rm_DQNextQReportPopup_uxNext).attr('disabled', 'disabled');
        }, 100);
        return true;
    }
    return false;
}

var preloadFailedAttemp = 0;
/*Reload merchant*/
function doPreloadMerchant() {
    //PageMethods.PreloadMerchant("", doPreloadSuccess, doPreloadFailure);
    var url = "rm_DQNextQWebMethod.aspx/PreloadMerchant";
    $.ajax({
        type: "POST",
        url: url,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //success: function (response) {
        //    //doPreloadSuccess(response["d"]);
        //    // allow the button to be clicked
        //},
        error: function (result) {
            doPreloadFailure(result);
        }
    });
}

//function doPreloadSuccess(data) { 
//    if (data != 'end' && parseInt(data) != rm_NextQueueModal_LENGHT_OF_QUEUE) {
//        doPreloadMerchant();
//    }
//}

function doPreloadFailure(error) {
    preloadFailedAttemp++;
    //parent.checkSession();
    if (preloadFailedAttemp <= 3) {
        doPreloadMerchant();
    }
}

//Refresh "rm_MerchantWorkedReport.aspx" page when close child window
var isRefresh = true;
function doAssign() {
    isRefresh = false;
}
window.onunload = function () {
    if (window.opener.location.href.indexOf("rm_MerchantWorkedReport.aspx") > 0 && isRefresh) {
        window.opener.location.reload();
    }
};
function keepSessionActive() {
    setTimeout("UpdateSessionActive()", 120000); //2 minutes - 120000
}
var updateFailedAttemp = 2;
function UpdateSessionActive() {
    var url = "rm_DQNextQReportPopup.aspx/KeepSessionActive";
    $.ajax({
        type: "POST",
        url: url,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            keepSessionActive();
        },
        error: function (result) {
            if (updateFailedAttemp > 0)
                keepSessionActive();
            updateFailedAttemp--;
        }
    });
}

function PageChanged(index) {
    var url = "rm_DQNextQReportPopup.aspx/GetNQTransactions"; // document.URL.replace('#', '') + '/CheckSSOUser';
    $("#uxProgress").show();
    $.ajax({
        type: "POST",
        url: url,
        data: '{"pageIndex":"' + index + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#uxProgress").hide();
            var data = result.d[0];
            var pager = result.d[1];

            $("#transactionTbl tr:not(:first)").remove();
            $("#transactionTbl").append(data);

            $("#" + pagerID).html(pager);
        },
        error: function (result) {
            $("#uxProgress").hide();
            console.log(result);
        }
    });
}

function RequeueSingleMerchant(merchantNumber) {
    var actionUrl = rootURL + "Risk/rm_DQNextQReportPopup.aspx/UpdateRequeuedMerchant";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"merchantNumber":"' + merchantNumber + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
        }
    });
}

function AddOrRemoveWorkQueue(isRemove) {
    var actionUrl = rootURL + "Risk/rm_DQNextQReportPopup.aspx/GetWorkQueueAssignment";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var data = result.d[0];
            if (data == "0") {
                alert(msgRisk_HasNoWorkQueueAs);
            }
            else {
                if (isRemove == 'true') {
                    ShowPopupModal('rm_DQRemoveWorkQueue.aspx', 'auto');
                }
                else {
                    ShowPopupModal('rm_DQAddToQueueModal.aspx?fromNextQReport=1', 'auto');
                }
            }
        }
    });
    return false;
}
function DoDismissAndNext() {
    $('#' + rm_uxNextHide_clientID).click();
}

function showNextQueueNodata(msg) {
    setTimeout(function () {
        var c = confirm(msg);
        if (true) {
            window.close();
        }
    }, 500);
}