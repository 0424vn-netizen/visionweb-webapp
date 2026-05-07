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

function ChangeWorkType_Click(type) {
    $get(rm_RetCb_uxHiddenWorkedType).value = type;
    $get(rm_RetCb_uxChangeWorkType).click();
}

function SubmitMerchantWorked(row, isWorked) {
    //$get(rm_RetCb_uxChangeMerchantWorked).click();
    if (isWorked) {
        row.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
        row.find("input.workedBox").prop('checked', true);
    }
    else {
        row.children(".merchantName").find("span:first").css("border-bottom", "2px solid Transparent");
        row.find("input.workedBox").prop('checked', false);
    }
}
function ChangeMerchantWorked_Click(sender) {
    //$get(rm_RetCb_uxHiddenMerchantWorked).value = sender.checked + ";" + sender.value;
    UpdateMerchantWorked(sender.checked, sender.value, '');
    var row = $(sender).parents('tr');
    setTimeout(function () { SubmitMerchantWorked(row, sender.checked); }, 300);
}

function hyperlink_Click(sender, merchantNumber, to) {
    //$get(rm_RetCb_uxHiddenMerchantWorked).value = "true;" + merchantNumber + ";" + to;
    //setTimeout(function () { $get(rm_RetCb_uxChangeMerchantWorked).click(); }, 300);
    UpdateMerchantWorked(true, merchantNumber, to);

    var row = $(sender).parents('tr');
    if (rm_IsAutoCheckWork) {
        setTimeout(function () { SubmitMerchantWorked(row, true); }, 300);
    }
    return false;
}
$(document).ready(function () {
    SetHeaderRTCBGrid();
    DisabledIncludeExcludeFilterExtend();
    $('#' + rm_RetCb_uxReportFiltering + ' .rcbInput').attr('onchange', 'FilterComboBoxChange()');
});

function SetHeaderRTCBGrid() {
    addGroupHeadersForStaticRadGrid(rm_RetCb_uxReportGrid,
        [
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
        ]);
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
    var extendEle;
    if (rm_RetCb_FilterTextboxElementClientID.length > 0) {
        var arrInEx = rm_RetCb_FilterTextboxElementClientID.split(',');
        if (arrInEx.length > 0) {
            for (i = 0; i < arrInEx.length; i++) {
                if (arrInEx[i].length > 0) {
                    var activeInputExtend = $('#' + rm_RetCb_uxReportFiltering + "_in" + arrInEx[i]);
                    if (activeInputExtend != null) {
                        activeInputExtend.attr('disabled', 'disabled');
                    }
                    extendEle = "_in" + arrInEx[i];
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
    var rcbInput = $('#' + rm_RetCb_uxReportFiltering + ' .rcbInput');
    if (rm_RetCb_HierarchyFilterText.split(',').indexOf(rcbInput.val()) > -1) {
        $('.extend-info').show();
        var isIE = /MSIE/.test(navigator.userAgent);
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
        var query = rm_RetCb_FilterExtendCmd.substring(rm_RetCb_FilterExtendCmd.indexOf(rcbInput.val() + '|s|'), rm_RetCb_FilterExtendCmd.indexOf(rcbInput.val() + '|e|')).replace(rcbInput.val() + '|s|', '');
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
        var cmdVM = '<a onclick="' + rm_RetCb_FilterExtendVM + '">view more...</a>';
        var text_display = merchants.substring(0, 45);
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
    var inS = 0;
    if ($('#' + rm_RetCb_uxHiddenItems).val().length > 0) {
        inS = 1;
    }
    $('.extend-command').html('<a onclick="ShowPopupModal(\'rm_Filter_Hierarchy_Modal.aspx?' + query + '&InS=' + inS + '\');">Edit</a>');
}

function UpdateMerchantWorked(status, merchant, to) {
    var url = rm_RetCb_URL + "/WebMethodUpdateMerchantWorkStatus";
    var uxReportGrid = $find(rm_RetCb_uxReportGrid);
    var uxReportGrid_Client_masterView = uxReportGrid.get_masterTableView();
    var rm_PageSize = uxReportGrid_Client_masterView.PageSize;
    var rm_PageIndex = uxReportGrid_Client_masterView.CurrentPageIndex;

    var rm_WorkType = "All";
    if ($('#' + uxRadNotWorked_ClientId).prop('checked'))
        rm_WorkType = "NotWorked";
    if ($('#' + uxRadWorked_ClientId).prop('checked'))
        rm_WorkType = "Worked";

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
            + ", HierarchyMode:'" + rm_HierarchyMode
            + "', HierarchyValue:'" + rm_HierarchyValue
            + "', PageSize:" + rm_PageSize
            + ", PageIndex:" + rm_PageIndex
            + ", WorkType:'" + rm_WorkType
            + "'}",

        success: function (response) {
            var objres = JSON.parse(response.d);
            if (objres.ResponseType == 'Msg') {
                alert(objres.ResponseValue);
            }
            if (objres.ResponseType == "OpenPopupWindow") {
                openPopupWindow(objres.ResponseValue);
            }
            if (objres.ResponseType == "Redirect") {
                window.location.href = objres.ResponseValue;
            }
        }
    });
}
