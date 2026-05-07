function OpenFlagColorTable() {
    ShowPopupModal("rm_ColorLegend.aspx", 'auto')
    return false;
}

function ChangeAllOrMyAssignment() {
    $get(rm_DetectionQueue_btnChangeAllOrMy).click();
}

function validate_Date(radDateInput) {
    if (radDateInput.get_value() == "") {
        alert(msgAlert1);
        return false;
    }
    else {
        var currentTime = new Date()
        if (IsDateGreater(radDateInput.get_value(), currentTime)) {
            alert(msgAlert2);
            return false;
        }
    }
}

function IsDateGreater(DateValue1, DateValue2) {
    var DaysDiff;
    Date1 = new Date(DateValue1);
    Date2 = new Date(DateValue2);
    DaysDiff = (Date1.getTime() - Date2.getTime()) / (1000 * 60 * 60 * 24);
    if (DaysDiff > 0)
        return true;
    else
        return false;
}

function uxSearchDetectionQueue_ClientClick() {
    var rdp = $find(rm_DetectionQueue_uxReportDate);
    if (rdp != null)
        return validate_Date(rdp.get_dateInput());
    return false;
}

function date_KeyPress(sender, eventArgs) {
    if (eventArgs.get_keyCode() == 10 || eventArgs.get_keyCode() == 13) {
        var retVal = validate_Date(sender);
        if (retVal == false)
            eventArgs.set_cancel(true);
    }
}

//Search when user focus on textbox
function DefaultEnterOnTextBox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        var uxSubmit = $get(rm_DetectionQueue_uxSearchDetectionQueue);
        if (uxSubmit) {
            uxSubmit.focus();
            if (isIE)
                setTimeout("SubmitDetectionQueue();", 200);
            else
                uxSubmit.click();
        }
        return false;
    }
}

function SubmitDetectionQueue() {
    $get(rm_DetectionQueue_uxSearchDetectionQueue).click();
}

function ChangeWorkedStatusOption() {
    $('#btnRequeueBoundary').addClass('hide');
    $('#divRemoveWorkQueue').addClass('hide');
    $get(rm_DetectionQueue_btnChangeWorkedStatusOption).click();
}

function ReloadHover()
{
    $('[data-hover="dropdown"]').dropdownHover();
}

//40706
function CheckMerchantWorked(merchantNumber)
{
    var actionUrl = rootURL + "Risk/rm_DetectionQueue.aspx/CheckMerchantWorked";
    var isWorked = false;
    $.ajax({
        type: "post",
        url: actionUrl,
        async: false,
        data: '{"merchantNumber":"' + merchantNumber + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d[0].toLowerCase() == 'true')
            {
                isWorked = true;
                var message = msgMerchantWorked.replace("{0}", result.d[1]).replace("{1}", result.d[2]);
                alert(message);
            }
        },
    });
    return isWorked;
}

function ChangeMerchantWorked(chk, volume) {
    
    var curentTR = $(chk).parents('tr');
    var curentTD = $(chk).parents('td');
    var merchantNameCard = curentTD.find("div[id*='merchantName']").find("a.link");
    if (chk.checked) {
        curentTR.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
        curentTD.addClass("row-checked");
        //For card view
        merchantNameCard.css("border-bottom", "2px solid #ffb6c1");
    }
    else {
        curentTR.children(".merchantName").find("span:first").css("border-bottom", "2px solid Transparent");
        curentTD.removeClass("row-checked");
        //For card view
        merchantNameCard.css("border-bottom", "2px solid Transparent");
    }
    UpdateAssigmentSummary(volume, chk.checked);
    //Asynch updating Worked status
    if (chk.checked) {
        var isWorked = CheckMerchantWorked(chk.value);
        if (isWorked)
            return;
    }
    PageMethods.UpdateMerchantWorkedStatus(chk.checked + ";" + chk.value);
}

function MerchantNumberClick(merchantNumber) {
    var actionUrl = rootURL + "Risk/rm_DetectionQueue.aspx/MerchantNumberClick";
    $.ajax({
        type: "post",
        url: actionUrl,
        async: true,
        data: '{"status":"true","merchantNumber":"' + merchantNumber + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var url = result.d[0];
            var reloadRainbow = result.d[1].toLowerCase() === 'true';

            if (reloadRainbow && $("body").find("div[id*='uxRainbowReport']").children().length > 0)
                reloadRainbowReport();

            openPopupWindow(url, 'RiskReport');
        },
        error: function (result) {
            //alert(result.responseText);
        }

    });
}

function AajxRequestStart(sender, args) {
    var reportType = rm_DetectionQueue_reportType;
    if (reportType == 'RainbowReport')
        UxExporter_OnRequestStart(sender, args);
    else
        AjaxFlat_OnRequestStart(sender, args);
}

function AajxResponseEnd(sender, args) {    
    var reportType = rm_DetectionQueue_reportType;
    if (reportType == 'RainbowReport')
        UxExporter_OnResponseEnd(sender, args);
    else
        AjaxFlat_OnResponseEnd(sender, args);
}

function OpenInstanceWindow(url, name) {
    var width = screen.availWidth * 0.9;
    var height = screen.availHeight * 0.8;
    var left = (screen.availWidth / 2) - (width / 2);
    var top = (screen.availHeight / 2) - (height / 2);
    var pwin = window.open(url, name,
        "scrollbars=1,menubar=0,toolbar=0,resizable=1,titlebar=0,location=1,width=" + width.toString()
        + ",height=" + height.toString()
        + ",left=" + left.toString()
        + ",top=" + top.toString(), true);
    if (pwin != null) {
        pwin.focus();
    }
}
function uxAssignmentList_OnClientSelectedIndexChanged(sender, args) {
    var uxAssignmentList = $find(rm_DetectionQueue_uxAssignmentList_ClientID);
    var selectedItem = uxAssignmentList.get_items().getItem(uxAssignmentList.get_selectedIndex());
    if (selectedItem != null) {
        var assignmentType = selectedItem.get_attributes().getAttribute(rm_DetectionQueue_AssignmentType);

        switch (assignmentType) {
            case rm_DetectionQueue_enum_DetectionQueue:
                $("#clRequeueOptions").css('display', '');
                $('[id="clFilterOrderBy"]').css('display', '');
                break;
            case rm_DetectionQueue_enum_WorkQueue:
                $("#clRequeueOptions").css('display', '');
                $('[id="clFilterOrderBy"]').css('display', '');
                break;
            case rm_DetectionQueue_enum_DetectionQueueDistinct:
                $("#clRequeueOptions").css('display', 'none');
                $('[id="clFilterOrderBy"]').css('display', 'none');
                $get(rm_DetectionQueue_optRainbow).checked = true;
                break;
            case rm_DetectionQueue_enum_AggregateQueue:
                $("#clRequeueOptions").css('display', 'none');
                $('[id="clFilterOrderBy"]').css('display', 'none');
                $get(rm_DetectionQueue_optRainbow).checked = true;
                break;
        }
    }
    else {
        $("#clRequeueOptions").css('display', '');
        $('[id="clFilterOrderBy"]').css('display', '');
    }
}

function doHeaderCheck(sender, isWQButtonOnly) {
    if (sender.checked) {
        $("input[id*=chkItem]").prop("checked", true);
        $("input[id*=chkItem]").prop("disabled", true);
        $('#btnRequeueBoundary').removeClass('hide');
        $('#divRemoveWorkQueue').removeClass('hide');
        if (isWQButtonOnly) {
            $('#divRemoveWorkQueue').addClass('hide');
        }
    } else {
        $("input[id*=chkItem]").prop("checked", false);
        $("input[id*=chkItem]").prop("disabled", false);

        $('#btnRequeueBoundary').addClass('hide');
        $('#divRemoveWorkQueue').addClass('hide');
    }
}

function RequeueAllMerchants(chk, isWQButtonOnly) {
    var actionUrl = rootURL + "Risk/rm_DetectionQueue.aspx/UpdateAllRequeuedMerchant";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"status":"' + chk.checked + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var requeuedCount = result.d[0];
            var requeuedVolume = result.d[1];
            //UpdateRequeuedMerchantSummary(requeuedCount, requeuedVolume);

            //hide requeue button
            if (parseInt(requeuedCount) == 0) {
                $('#btnRequeueBoundary').addClass('hide');
                $('#divRemoveWorkQueue').addClass('hide');
            }
        }
    });
    doHeaderCheck(chk, isWQButtonOnly);
    //PageMethods.UpdateAllRequeuedMerchant(chk.checked);
}

function RequeueSingleMerchant(chk, isWQButtonOnly) {
    var actionUrl = rootURL + "Risk/rm_DetectionQueue.aspx/UpdateRequeuedMerchant";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"status":"' + chk.checked + '","merchantNumber":"' + chk.value + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var requeuedCount = result.d[0];
            var requeuedVolume = result.d[1];
            var isAllSelected = result.d[2];
            //UpdateRequeuedMerchantSummary(requeuedCount, requeuedVolume);

            //hide requeue button
            if (parseInt(requeuedCount) == 0) {
                $('#btnRequeueBoundary').addClass('hide');
                $('#divRemoveWorkQueue').addClass('hide');
            }

            if (isAllSelected.toLowerCase() == 'true') {
                $("input[id*=chkHeader]").prop("checked", true);
                $("input[id*=chkHeader]").prop("disabled", true);
            }
            else {
                $("input[id*=chkHeader]").prop("disabled", false);
            }
        }
    });
    doItemCheck(chk, isWQButtonOnly);
}

function doItemCheck(sender, isWQButtonOnly) {

    if (!sender.checked) {
        $("input[id*=chkHeader]").prop("checked", false);
    }
    else if (isWQButtonOnly) {
        $('#btnRequeueBoundary').removeClass('hide');
    }
    else {
        $('#btnRequeueBoundary').removeClass('hide');
        $('#divRemoveWorkQueue').removeClass('hide');
    }
}

function RebindAndShowStausWhenCloseModal() {
    HidePopupModal();
    $('#uxProgress').css('display', '');
    document.getElementById(rm_DetectionQueue_uxSearchDetectionQueue).click();
}

$(document).ready(function () {
    setTimeout("uxAssignmentList_OnClientSelectedIndexChanged();", 500);
    $('#filterBlock').css('min-width', '587px');
});

function checkSession() {
    document.getElementById(rm_NextQueue_uxReload).click();
}

function scrollToTop() {    
    $("html, body").animate({ scrollTop: 0 }, "slow");
    return false;
}

function uxAssignmentList_OnClientKeyPressing(sender, args) {
    var uxAssignmentList = $find(rm_DetectionQueue_uxAssignmentList_ClientID);
    uxAssignmentList.showDropDown();
}

function AddOrRemoveWorkQueue(isRemove) {
    var actionUrl = rootURL + "Risk/rm_DetectionQueue.aspx/GetWorkQueueAssignment";
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
                    ShowPopupModal('rm_DQAddToQueueModal.aspx', 'auto');
                }
            }
        }
    });

    return false;
}
