function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    SetHeaderRTCBGrid();
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
}

function masterAjax_responseEnd(sender, args) {
    SetHeaderRTCBGrid();
    $('[data-hover="dropdown"]').dropdownHover();
}

function ChangeReviewedType_Click(type) {
    $get(rm_RetCb_uxHiddenWorkedType).value = type;
    $get(rm_RetCb_uxChangeWorkType).click();
}

function SubmitMerchantWorked(row, isWorked) {
    if (isWorked) {
        row.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
        row.find("input.workedBox").prop('checked', true);
    }
    else {
        row.children(".merchantName").find("span:first").css("border-bottom", "2px solid Transparent");
        row.find("input.workedBox").prop('checked', false);
    }
}
function ChangeMerchantReviewed_Click(sender) {
    UpdateMerchantReviewed(sender.checked, sender.value, '');
    let row = $(sender).parents('tr');
    setTimeout(function () { SubmitMerchantWorked(row, sender.checked); }, 300);
}

function hyperlink_Click(sender, merchantNumber, to) {
    UpdateMerchantReviewed(true, merchantNumber, to);
    return false;
}
$(document).ready(function () {
    SetHeaderRTCBGrid();
    DisabledIncludeExcludeFilterExtend();
    $('#' + rm_RetCb_uxReportFiltering + ' .rcbInput').attr('onchange', 'FilterComboBoxChange()');
    addCheckSpecialCharactersForDate();
});


function SetHeaderRTCBGrid() {
    var gridConfig = [];

    if (show_RDR) {
        gridConfig = [
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            [rm_RetCb_js_30Day, 7, 'rgHeader mh'],
            [rm_RetCb_js_90Day, 7, 'rgHeader mh'],
            [rm_RetCb_js_LastBatch, 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh']
        ];
    }
    else {
        gridConfig = [
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            [rm_RetCb_js_30Day, 3, 'rgHeader mh'],
            [rm_RetCb_js_90Day, 3, 'rgHeader mh'],
            [rm_RetCb_js_LastBatch, 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh']
        ];
    }

    addGroupHeadersForStaticRadGrid(rm_RetCb_uxReportGrid, gridConfig);
}
function ajaxRepquestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
    $('[data-hover="dropdown"]').dropdownHover();
}

var filter_extend_width = 0;
function DisabledIncludeExcludeFilterExtend() {
    let extendEle;
    if (rm_RetCb_FilterTextboxElementClientID.length > 0) {
        let arrInEx = rm_RetCb_FilterTextboxElementClientID.split(',');
        if (arrInEx.length > 0) {
            for (let item of arrInEx) {
                if (item.length > 0) {
                    let activeInputExtend = $('#' + rm_RetCb_uxReportFiltering + "_in" + item);
                    if (activeInputExtend != null) {
                        activeInputExtend.attr('disabled', 'disabled');
                    }
                    extendEle = "_in" + item;
                }
            }
            filter_extend_width = $('#' + rm_RetCb_uxReportFiltering + extendEle).width();
        }
    }
}
function FilterComboBoxChange() {
    if (rm_RetCb_HierarchyFilterText.length > 0 && rm_RetCb_HierarchyFilterText[rm_RetCb_HierarchyFilterText.length - 1] == ',') {
        rm_RetCb_HierarchyFilterText = rm_RetCb_HierarchyFilterText.substring(0, rm_RetCb_HierarchyFilterText.length - 1);
    }
    let rcbInput = $('#' + rm_RetCb_uxReportFiltering + ' .rcbInput');
    if (rm_RetCb_HierarchyFilterText.split(',').indexOf(rcbInput.val()) > -1) {
        $('.extend-info').show();
        let isIE = /MSIE/.test(navigator.userAgent);
        if (isIE) {
            $('.extend-info').width(filter_extend_width + $('.filter-left').width() + 71);
            $('.extend-left').width(filter_extend_width + $('.filter-left').width() - 11);
        }
        else {
            $('.extend-info').width(filter_extend_width + $('.filter-left').width() + 83);
            $('.extend-left').width(filter_extend_width + $('.filter-left').width() + 2);
        }
        if ($('#' + rm_RetCb_uxHiddenCurrentHF).val() != rcbInput.val()) {
            RefreshIncludeExcludeItems('', 'true');
        }
        let query = rm_RetCb_FilterExtendCmd.substring(rm_RetCb_FilterExtendCmd.indexOf(rcbInput.val() + '|s|'), rm_RetCb_FilterExtendCmd.indexOf(rcbInput.val() + '|e|')).replace(rcbInput.val() + '|s|', '');
        SetFilterExtendCommand(query);
    }
    else {
        $('.extend-info').hide();
    }
    $('#' + rm_RetCb_uxHiddenCurrentHF).val('');
}

function RefreshIncludeExcludeItems(merchants, isInclude) {
    if (isInclude != null && isInclude.toLowerCase() == 'true') {
        $('.extend-title').html("Include");
    }
    else {
        $('.extend-title').html("Exclude");
    }
    $('#' + rm_RetCb_uxHiddenItems).val(merchants);
    if (merchants != null && merchants.length > 55) {
        let cmdVM = '<a onclick="' + rm_RetCb_FilterExtendVM + '">view more...</a>';
        let text_display = merchants.substring(0, 45);
        text_display = text_display.substring(0, text_display.lastIndexOf(','));
        $('.extend-value').html(text_display + cmdVM);
    }
    else {
        if (merchants != null && merchants.length == 0) {
            merchants = rm_RetCb_NAValue;
        }
        $('.extend-value').html(merchants);
    }
}
function SetFilterExtendCommand(query) {
    let inS = 0;
    if ($('#' + rm_RetCb_uxHiddenItems).val().length > 0) {
        inS = 1;
    }
    $('.extend-command').html('<a onclick="ShowPopupModal(\'rm_MCF_Filter_Hierarchy_Modal.aspx?' + query + '&InS=' + inS + '\');">Edit</a>');
}

function UpdateMerchantReviewed(status, merchant, to) {
    let url = rm_RetCb_URL + "/WebMethodUpdateMerchantWorkStatus";
    let uxReportGrid = $find(rm_RetCb_uxReportGrid);
    let uxReportGrid_Client_masterView = uxReportGrid.get_masterTableView();
    let rm_PageSize = uxReportGrid_Client_masterView.PageSize;
    let rm_PageIndex = uxReportGrid_Client_masterView.CurrentPageIndex;

    let rm_ReviewType = "All";
    if ($('#' + uxRadNotReviewed_ClientId).prop('checked'))
        rm_ReviewType = "NotReviewed";
    if ($('#' + uxRadReviewed_ClientId).prop('checked'))
        rm_ReviewType = "Reviewed";

    $.ajax({
        type: "POST",
        url: url,
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{status:'" + status
            + "',merchant:'" + merchant
            + "',to:'" + to
            + "', begindate:" + rm_BeginDate
            + ", enddate:" + rm_EndDate
            + ", HierarchyMode:''"
            + ", HierarchyValue:''"
            + ", PageSize:" + rm_PageSize
            + ", PageIndex:" + rm_PageIndex
            + ", ReviewedType:'" + rm_ReviewType
            + "'}",

        success: function (response) {
            let objres = JSON.parse(response.d);
            if (objres.ResponseType == 'Msg') {
                alert(objres.ResponseValue);
            }
            if (objres.ResponseType == "OpenPopupWindow") {
                openPopupWindow(objres.ResponseValue, "RiskReport");
            }
            if (objres.ResponseType == "Redirect") {
                window.location.href = objres.ResponseValue;
            }
        }
    });
}
