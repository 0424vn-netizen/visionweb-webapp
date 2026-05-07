function ChangeAllOrMyAssignment() {
    $get(rm_DetectionQueue_btnChangeAllOrMy).click();
    $($get(rm_DetectionQueue_uxSearchDetectionQueue)).attr("disabled", true);
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

function uxAssignmentList_OnClientSelectedIndexChanged(sender, args) {
    var uxAssignmentList = $find(rm_DetectionQueue_uxAssignmentList_ClientID);
    var selectedItem = uxAssignmentList.get_items().getItem(uxAssignmentList.get_selectedIndex());
    if (selectedItem != null) {
        var assignmentType = selectedItem.get_attributes().getAttribute(rm_DetectionQueue_AssignmentType);

        //switch (assignmentType) {
        //    case rm_DetectionQueue_enum_DetectionQueue:
        //        $("#clRequeueOptions").css('display', '');
        //        //$('[id="clFilterOrderBy"]').css('display', '');
        //        break;
        //    case rm_DetectionQueue_enum_WorkQueue:
        //        $("#clRequeueOptions").css('display', '');
        //        //$('[id="clFilterOrderBy"]').css('display', '');
        //        break;
        //    case rm_DetectionQueue_enum_DetectionQueueDistinct:
        //        $("#clRequeueOptions").css('display', 'none');
        //        //$('[id="clFilterOrderBy"]').css('display', 'none');
        //        break;
        //    case rm_DetectionQueue_enum_AggregateQueue:
        //        $("#clRequeueOptions").css('display', 'none');
        //        //$('[id="clFilterOrderBy"]').css('display', 'none');
        //        break;
        //}
    }
    else {
        $("#clRequeueOptions").css('display', '');
        $('[id="clFilterOrderBy"]').css('display', '');
    }
    $($get(rm_DetectionQueue_uxSearchDetectionQueue)).attr("disabled", false);
}

$(document).ready(function () {
    setTimeout("uxAssignmentList_OnClientSelectedIndexChanged();", 500);
    $('#filterBlock').css('min-width', '587px');
    addCheckSpecialCharacters();
});

function uxAssignmentList_OnClientKeyPressing(sender, args) {
    var uxAssignmentList = $find(rm_DetectionQueue_uxAssignmentList_ClientID);
    uxAssignmentList.showDropDown();
}

function openNextQueue(url) {
    openPopupWindow(url, 'DQMCFWindow4');
}

function refreshClient() {
    $("#" + uxbtn_BarometerReport_ClientID).attr("disabled", true);
    $("#" + uxbtn_SecurityReport_ClientID).attr("disabled", true);
    $("#" + uxbtn_NextQueue_ClientID).attr("disabled", true);
}

function addCheckSpecialCharacters() {
    $("#" + rm_DetectionQueue_uxReportDate + "_dateInput").change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            alert(content.AlertMsgSpecialCharacters);
            removeSpecialCharacters(this);
        }
    });
}

function refreshAssignmentGrid() {
    $get(uxbtn_btnRefreshAssignmentGrid_ClientID).click();
}