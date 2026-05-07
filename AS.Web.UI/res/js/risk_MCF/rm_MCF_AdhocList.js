//<![CDATA[
function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
    
    $('[data-hover="dropdown"]').dropdownHover();
    resizeFrozenScrollWidth();
}

function cBox_Click(sender) {
    $get(rm_AdhocList_hddCheckBox).value = sender.checked + ";" + sender.value;
    $get(rm_AdhocList_btPost).click();
}

function hyperLink_Click(merchantNumber) {
    var actionUrl = rootURL + "risk_MCF/rm_MCF_AdhocList.aspx/MerchantNumberClick";
    $.ajax({
        type: "post",
        url: actionUrl,
        async: false,
        data: '{"status":"true","merchantNumber":"' + merchantNumber + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var url = result.d[0];
            openPopupWindow(url, 'RiskReport');

            //reload grid
            $find(rm_AdhocList_uxGrid).get_masterTableView().rebind();
        },
        error: function (result) {
            //alert(result.responseText);
        }

    });
}

function SubmitMerchantWorked(row, isWorked) {
    $get(rm_AdhocList_btnChangeMerchantWorked).click();
    if (isWorked) {
        row.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
        row.find("input.workedBox").attr("checked", true);
    }
    else {
        row.children(".merchantName").find("span:first").css("border-bottom", "2px solid Transparent");
        row.find("input.workedBox").removeAttr("checked");
    }
}

function ChangeMerchantWorked(chk) {
    $get(rm_AdhocList_hddUpdateMerchantWorked).value = chk.checked + ";" + chk.value;
    var row = $(chk).parent().parent();
    setTimeout(function () { SubmitMerchantWorked(row, chk.checked); }, 300);
}
function OpenFlagColorTable() {
    ShowPopupModal("rm_MCF_ColorLegend.aspx", 'auto');
    return false;
}


function OpenRiskReport(merchantNumber, reportDate) {
    var exp = "rm_MCF_AdhocList.aspx";
    var expIndex = window.location.href.indexOf(exp);
    var http = window.location.href.substr(0, expIndex);
    window.location.href = http + "rm_MCF_RiskReport.aspx?MerchantNumber=" + merchantNumber;
    return false;
}


function ViewRainbowReport(assignID) {
    document.getElementById(rm_AdhocList_hddAssignID).value = assignID;
    document.getElementById(rm_AdhocList_btnViewResult).click();
    return true;
}

function ReviewAdhoc(assignID) {
    document.getElementById(rm_AdhocList_hddAssignID).value = assignID;
    document.getElementById(rm_AdhocList_btnReview).click();
    return true;
}

function CloseAndRebind() {
    HidePopupModal();
    document.getElementById(rm_AdhocList_btnRebind).click();
}

function OpenInstanceWindow(url, name) {
    var width = screen.availWidth * 0.9;
    var height = screen.availHeight * 0.8;
    var left = (screen.availWidth / 2) - (width / 2);
    var top = (screen.availHeight / 2) - (height / 2);
    var pwin = window.open(url, name,
        "scrollbars=1,menubar=0,toolbar=0,resizable=1,location=1,width=" + width.toString()
        + ",height=" + height.toString()
        + ",left=" + left.toString()
        + ",top=" + top.toString(), true);

    if (pwin != null) {
        pwin.focus();
    }
}
function adjustGridHeight() {
    $('.freeze-table').css('height','auto');
}

function resizeFrozenScrollWidth() {
    if ($find(rm_AdhocList_uxGrid)) {
        var gridHeaderID = $find(rm_AdhocList_uxGrid).GridHeaderDiv.children[0].id;
        var headerCols = "#" + gridHeaderID + " > colgroup > col";
        var totalWidth = 0;

        $(headerCols).each(function (i) {
            var colNum = i - 1;
            var $colHeader = $(headerCols).eq(colNum);
            var widthAttr = $colHeader.css('width').replace("px", "");
            totalWidth += parseInt(widthAttr);
        });

        var frozenScroll = $find(rm_AdhocList_uxGrid)._scrolling._frozenScroll.firstChild;
        var frozenScrollWidth = $(frozenScroll).css('width').replace("px", "");

        if (parseInt(frozenScrollWidth) < totalWidth)
            $(frozenScroll).css('width', totalWidth + 100 + "px");       
    }
}
//]]>