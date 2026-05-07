function showWin() {
    var win = $find(addInvestigationModal);
    var btn = $('#' + rm_RiskReport_uxBtnAddInvestigation);
    $(btn).blur();
    $(btn).attr('disabled', 'disabled');
    $('body').addClass('print-investigation');
    if (win) {
        win.SetTitle(rm_RiskReport_js_TitleAddInvestigation);
        var viewport = win._getViewportBounds();
        win.show();

        var winEle = $(win.get_popupElement());
        winEle.data("isClosing", false);

        winEle.addClass("pos-fixed-bottom-right");
    }
}

function winOnClientActivate(sender, e) {
    if (!isDrag) {
        sender.autoSize();
    }
}

var isDrag = false;
function winOnClientDragStart(sender) {
    var winEle = $(sender.get_popupElement());
    winEle.removeClass("pos-fixed-bottom-right");
    isDrag = true;
}

function winOnClientDragEnd(sender) {
    var win = $find(addInvestigationModal);
    var winEle = $(sender.get_popupElement());
    var x = $('body').innerWidth() - winEle.width() - 20;
    var y = $('body').innerHeight() - winEle.height();
    var posLeft = parseInt($(winEle).css("left").replace("px", ""));
    var posTop = parseInt($(winEle).css("top").replace("px", ""));

    if (posLeft > x && posTop > y) {
        win.moveTo(x, y)
    }
    else if (posLeft > x) {
        win.moveTo(x, posTop);
    }
    else if (posTop > y) {
        win.moveTo(posLeft, y);
    }
}

function winOnClientCommand(sender, e) {
    var win = $find(addInvestigationModal);
    var winEle = $(sender.get_popupElement());

    if (!winEle.hasClass('pos-fixed-bottom-right')) {
        winEle.addClass("pos-fixed-bottom-right");
    }
    if (e.get_commandName() == "Restore") {
        setTimeout(function () {
            if (winEle.data("isClosing")) {
                return;
            }
        }, 50);

    }
    isDrag = false;
}

function winOnClientBeforeClose(sender, e) {
    var winEle = $(sender.get_popupElement());
    winEle.data("isClosing", true);
}

function winOnClientClose(sender, e) {
    var btn = $('#' + rm_RiskReport_uxBtnAddInvestigation);
    $(btn).removeAttr('disabled');
    $('body').removeClass('print-investigation');
}

function riskCbxCustomDropdown(sender) {
    var dropdown = sender.get_dropDownElement();
    if (dropdown) {
        var containerDropdown = $(dropdown).parents(".rcbSlide");
        if (!containerDropdown.hasClass("onTopModal")) {
            $(dropdown).parents(".rcbSlide").addClass("onTopModal");
        }
    }
}
//39919-Add
if (document.getElementById("uxIframeCaseHitory") != null) {
    document.getElementById("uxIframeCaseHitory").onload = function () {
        var obj = $("#uxIframeCaseHitory").contents().find("html").find(".grid-title").parent();
        if (obj.data('target') != ".report-filter-panel") {
            obj.removeAttr('data-toggle');
            obj.removeAttr('data-target');
            obj.parents(".grid-title").css("cursor", "default");
            obj.children(".grid-title").css("cursor", "default");
            obj.css("cursor", "default");
        }
    }
}

$(document).ready(function () {
    var datatarget = $(document).find('[data-toggle="collapse"]');
    datatarget.each(function () {
        if ($(this).data('target') != ".report-filter-panel") {
            $(this).removeAttr('data-toggle');
            $(this).removeAttr('data-target');
            $(this).parents(".grid-title").css("cursor", "default");
            $(this).children(".grid-title").css("cursor", "default");
            $(this).css("cursor", "default");
        }
    });
});


//$(document).ready(doAdjustHeader());


function clickCloseInves() {
    var uxCloseInvestigation = $get(rm_RiskReport_uxCloseInvestigation);
    var uxchbFollowupDate = $get(rm_RiskReport_uxIsFollowupDate);
    var uxFollowUpdate = $get(rm_RiskReport_uxFollowUpdate);
    var currentfollowdate = rm_RiskReport_CurrentFollowUpdateDate;
    var IsCheckFollowUpdate = rm_RiskReport_IsCheckFollowUpdate;

    if (uxCloseInvestigation.checked) {
        $(uxchbFollowupDate).attr('disabled', 'disabled');
        $find(rm_RiskReport_uxFollowUpdate).set_enabled(false);

    }
    else {
        $(uxchbFollowupDate).removeAttr('disabled');
        if (!uxchbFollowupDate.checked)
            $find(rm_RiskReport_uxFollowUpdate).set_enabled(false);
        else
            $find(rm_RiskReport_uxFollowUpdate).set_enabled(true);

    }
}

function clickFollowUpdate() {
    var uxchbFollowupDate = $get(rm_RiskReport_uxIsFollowupDate);
    var followUpdate = $find(rm_RiskReport_uxFollowUpdate);
    var dateInput = followUpdate.get_dateInput();
    if (!uxchbFollowupDate.checked) {
        dateInput.set_textBoxValue("");
        followUpdate.set_enabled(false);
    }
    else {
        followUpdate.set_enabled(true);
    }
    $(dateInput.get_element()).removeClass("riError").focus();
}

function uxMerchantList_OnClientTextChange(sender, args) {
    var combo = $find(rm_RiskReport_uxMerchantList);
    if (!!combo && !CheckMerchantNameIsScript(combo.get_text())) {
        alert(rm_RiskReport_js_ValidationSpecialCharacter);
        combo.set_text("");
        return false;
    }
}
var IsSelectedMerchant = false;
function uxMerchantList_OnClientSelectedIndexChanged(sender, args) {
    if (args.get_item() != null) {
        var value = args.get_item().get_value().trim();
        var uxMerchantNumber = $find(rm_RiskReport_uxMerchantNumber);
        uxMerchantNumber.set_value(value);
        IsSelectedMerchant = true;
    }
}

function CheckMerchantNumber(value) {
    var merchantNumber = value;

    if (trim(merchantNumber) == '') {
        alert(String.format(rm_RiskReport_Generic_CheckLengthOfMerchantNumber));
        return false;
    }
    var reg = /^[\-a-z0-9]+$/i;
    if (!reg.test(merchantNumber)) {
        alert(String.format(rm_RiskReport_InvalidFormatMerchantID_js));
        return false;
    }
    return true;
}

function ValidateData() {
    var combo = $find(rm_RiskReport_uxMerchantList);
    if (!!combo) {
        if (!CheckMerchantNameIsScript(combo.get_text())) {
            alert(rm_RiskReport_js_ValidationSpecialCharacter);
            combo.set_text("");
            return false
        }
    }
    var merchantNumber = $find(rm_RiskReport_uxMerchantNumber).get_textBoxValue().trim();
    if (!CheckMerchantNumber(merchantNumber)) {
        $find(rm_RiskReport_uxMerchantNumber).focus();
        return false;
    } else {
        return true;
    }
}

function CheckMerchantNameIsScript(value) {
    if (!!value) {
        //var reg = /^[ ,.A-Za-z0-9]*$/;
        var reg = /^((?!(\<|\>))(?!(\&\#)).)*$/g;
        if (reg.test(value) == false) {
            return false;
        }
    }
    return true;
}


function DefaultEnterOnTextBox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    uxSearch = document.getElementById(rm_RiskReport_uxSearchButton);
    if (isEnter) {
        if (uxSearch) {
            uxSearch.focus();
            if (isIE)
                setTimeout('document.getElementById(' + rm_RiskReport_uxSearchButton + ').click();', 200);
            else
                uxSearch.click();
        }
        return false;
    }
}
var jReason = 1;
var jStatus = 2;
var jAssigned = 3;
var jDate = 4;
var jResolution = 5;
var jComment = 6;
function DisplayMessage(id, message) {
    if (id == jReason) {
        $("#ciReasonMsg").removeClass("display-none");
        $("#ciReasonMsg").text(message);
        $("#ciReason").addClass("error");
    } else if (id == jStatus) {
        $("#ciStatusMsg").removeClass("display-none");
        $("#ciStatusMsg").text(message);
        $("#ciStatus").addClass("error");
    } else if (id == jAssigned) {
        $("#ciAssignedMsg").removeClass("display-none");
        $("#ciAssignedMsg").text(message);
        $("#ciAssigned").addClass("error");
    } else if (id == jResolution) {
        $("#ciResolutionMsg").removeClass("display-none");
        $("#ciResolutionMsg").text(message);
        $("#ciResolution").addClass("error");
    } else if (id == jDate) {
        $("#ciFollowUpdateMsg").removeClass("display-none");
        $("#ciFollowUpdateMsg").text(message);
        $("#ciFollowUpdate").addClass("error");
    } else if (id == jComment) {
        $("#cidMessageComments").removeClass("display-none");
        $("#cidMessageComments").text(message);
    }
}

function HideMessage(id) {
    if (id == jReason) {
        $("#ciReasonMsg").addClass("display-none");
        if ($("#ciReason").hasClass("error"))
            $("#ciReason").removeClass("error");
    } else if (id == jStatus) {
        $("#ciStatusMsg").addClass("display-none");
        if ($("#ciStatus").hasClass("error"))
            $("#ciStatus").removeClass("error");
    } else if (id == jAssigned) {
        $("#ciAssignedMsg").addClass("display-none");
        if ($("#ciAssigned").hasClass("error"))
            $("#ciAssigned").removeClass("error");
    } else if (id == jResolution) {
        $("#ciResolutionMsg").addClass("display-none");
        if ($("#ciResolution").hasClass("error"))
            $("#ciResolution").removeClass("error");
    } else if (id == jDate) {
        $("#ciFollowUpdateMsg").addClass("display-none");
        if ($("#ciFollowUpdate").hasClass("error"))
            $("#ciFollowUpdate").removeClass("error");
    } else if (id == jComment) {
        $("#cidMessageComments").addClass("display-none");
    }
}

function ValidateInvestigate(btn) {
    var win = $find(addInvestigationModal);
    var Reason = checkReason();
    var Status = checkStatus();
    var Assigned = checkAssignedTo();
    var Resolution = checkResolution();
    var Date = checkDate();
    var Comment = checkComment();
    if (Reason && Status && Assigned && Resolution && Date && Comment) {
        win.close();
        __doPostBack(btn.name, '');
        return true;
    }
    else {
        win.autoSize();
        return false;
    }

}

function checkReason() {
    var uxReasonList = $find(rm_RiskReport_uxReasonList);
    if (rm_RiskReport_IsRequiredReason == "true" && uxReasonList.get_selectedItem().get_value() == '0') {
        msg = rm_RiskReport_js_MustSelectReason;
        DisplayMessage(jReason, msg);
        return false;
    }
    HideMessage(jReason);
    return true;
}

function checkStatus() {
    var statusValue = $find(rm_RiskReport_uxStatusList).get_value();
    if (statusValue.length == 0) {
        var msg = rm_RiskReport_js_MustSelectStatus;
        DisplayMessage(jStatus, msg);
        return false;
    }
    HideMessage(jStatus);
    return true;
}

function checkAssignedTo() {
    var assignedToValue = $find(rm_RiskReport_uxAssignedToList).get_value();
    if (assignedToValue.length == 0) {
        msg = rm_RiskReport_js_MustSelectAssignedTo;
        DisplayMessage(jAssigned, msg);
        return false;
    }
    HideMessage(jAssigned);
    return true;
}

function checkResolution() {
    var uxCloseInvestigation = $get(rm_RiskReport_uxCloseInvestigation);
    if (uxCloseInvestigation != null && uxCloseInvestigation.checked) {
        var resolutionValue = $find(rm_RiskReport_uxResolutionList).get_value();
        if (resolutionValue.length == 0) {
            var msg = rm_RiskReport_js_MustSelectResolution;
            DisplayMessage(jResolution, msg);
            return false;
        }
    }
    HideMessage(jResolution);
    return true;
}

function checkDate() {
    var uxCloseInvestigation = $get(rm_RiskReport_uxCloseInvestigation);
    var uxIsFollowupDate = $get(rm_RiskReport_uxIsFollowupDate);
    if (uxCloseInvestigation == null && uxIsFollowupDate != null && uxIsFollowupDate.checked) {
        var uxFollowUpdate = $find(rm_RiskReport_uxFollowUpdate).get_selectedDate();
        if (uxFollowUpdate == null) {
            var msg = rm_RiskReport_js_FollowUpDateFormat;
            DisplayMessage(jDate, msg);
            return false;
        }
        else if (!CheckSelectDate(parseInt(uxFollowUpdate.localeFormat("yyyy/MM/dd").split('/')[0]), parseInt(uxFollowUpdate.localeFormat("yyyy/MM/dd").split('/')[1]), parseInt(uxFollowUpdate.localeFormat("yyyy/MM/dd").split('/')[2]))) {
            var msg = rm_RiskReport_js_FollowUpDateNotPast;
            DisplayMessage(jDate, msg);
            return false;
        }

    }
    else if (uxCloseInvestigation != null && !uxCloseInvestigation.checked && uxIsFollowupDate != null && uxIsFollowupDate.checked) {
        var uxFollowUpdate = $find(rm_RiskReport_uxFollowUpdate).get_selectedDate();
        if (uxFollowUpdate == null) {
            var msg = rm_RiskReport_js_FollowUpDateFormat;
            DisplayMessage(jDate, msg);
            return false;
        }
        else if (!CheckSelectDate(parseInt(uxFollowUpdate.localeFormat("yyyy/MM/dd").split('/')[0]), parseInt(uxFollowUpdate.localeFormat("yyyy/MM/dd").split('/')[1]), parseInt(uxFollowUpdate.localeFormat("yyyy/MM/dd").split('/')[2]))) {
            var msg = rm_RiskReport_js_FollowUpDateNotPast;
            DisplayMessage(jDate, msg);
            return false;
        }
    }
    HideMessage(jDate);
    return true;
}

function checkComment() {
    var uxComments = document.getElementById(uxCommentsClientID);
    var text = uxComments.value;
    var textlength = text.length;
    if (textlength > 1000) {
        uxComments.value = text.substr(0, 990);
        var msg = rm_RiskReport_js_CommentNotExceed1000;
        DisplayMessage(jComment, msg);
        return false;
    } else
        HideMessage(jComment);
    return true;
}

function escalation_Click(escalationNumber) {
    $get(rm_RiskReport_hddProcessData).value = "escalation;" + escalationNumber;
    $get(rm_RiskReport_btnProcess).click();
    return true;
}

function ReloadComment() {
    //$get(rm_RiskReport_btnComment).click();
    return true;
}

//for batch summary grid
function doAdjustHeader() {
    addGroupHeadersForStaticRadGrid(rm_RiskReport_uxBatchHistoryGrid,
        [[rm_RiskReport_js_GridHeaderBS, 6, 'rgHeader mh'],
        [rm_RiskReport_js_GridHeaderBC, 3, 'rgHeader mh'],
        [rm_RiskReport_js_GridHeaderNBC, 3, 'rgHeader mh'],
        [rm_RiskReport_js_GridHeaderToTal, 3, 'rgHeader mh']]);
    //if (IS_IPMT_CLIENT == 'False') {
    //    addGroupHeadersForStaticRadGrid(rm_RiskReport_uxBatchHistoryGrid,
    //        [[rm_RiskReport_js_GridHeaderBS, 6, 'rgHeader mh'],
    //        [rm_RiskReport_js_GridHeaderBC, 3, 'rgHeader mh'],
    //        [rm_RiskReport_js_GridHeaderNBC, 3, 'rgHeader mh'],
    //        [rm_RiskReport_js_GridHeaderToTal, 3, 'rgHeader mh']]);
    //}
    //else {
    //    addGroupHeadersForStaticRadGrid(rm_RiskReport_uxBatchHistoryGrid,
    //        [[rm_RiskReport_js_GridHeaderBS, 7, 'rgHeader mh'],
    //        [rm_RiskReport_js_GridHeaderBC, 3, 'rgHeader mh'],
    //        [rm_RiskReport_js_GridHeaderNBC, 3, 'rgHeader mh'],
    //        [rm_RiskReport_js_GridHeaderToTal, 3, 'rgHeader mh']]);
    //}
}
doAdjustHeader();


function OpenInstanceWindow(url, name) {
    var width = 1000;
    var height = 700;
    var left = (screen.availWidth / 2) - (width / 2);
    var top = (screen.availHeight / 2) - (height / 2);
    var pwin = window.open(url, name, "_blank", "scrollbars=1,menubar=0,toolbar=0,resizable=1,location=1,width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=" + top.toString(), true);
    if (pwin != null) {
        pwin.focus();
    }
}

function CheckSelectDate(y, m, d) {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();

    if (y > yyyy) {
        return true;
    }
    else if (yyyy == y && m > mm) {
        return true;
    }
    else if (yyyy == y && m == mm && d >= dd) {
        return true;
    }
    else {
        return false;
    }
}
function OnClientKeyPressing(sender, eventArgs) {
    var isIE = /MSIE/.test(navigator.userAgent);
    if (eventArgs.get_domEvent().keyCode == 13 && isIE && IsSelectedMerchant) {
        $('#' + rm_RiskReport_uxSearchButton).click();
        return false;
    }
}

$(function () {
    // Add event click enter from kb                 
    $('#' + rm_RiskReport_uxFilteringOptionsContainer + ' input[type=text]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            $('#' + rm_RiskReport_uxSearchButton).click();
            return false;
        }
    });

});

function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}

function ajaxResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
    $('[data-hover="dropdown"]').dropdownHover();
}

function GetTransactionVolumeAnalysisGrid() {
    var uxTransactionVolumeAnalysisGrid = $find(rm_RiskReport_TransactionVolumeAnalysis);
    if (!!uxTransactionVolumeAnalysisGrid) {
        return uxTransactionVolumeAnalysisGrid;
    }
    return null;
}

function GetButtonRebind() {
    var uxButtonRebind = $get(rm_Button_Rebind_ClientID);
    if (!!uxButtonRebind) {
        return uxButtonRebind;
    }
    return null;
}


function switchMID(merchant) {
    var uxMerchantNumber = $find(rm_RiskReport_uxMerchantNumber);
    uxMerchantNumber.set_value(merchant);
    IsSelectedMerchant = true;
    $('#' + rm_RiskReport_uxSearchButton).click();
}

function showSupplemental() {
    $("#divFundingStatus").removeClass("hide");
    $("#divUrl").removeClass("hide");
    $("#divCreditScore").removeClass("hide");

    $("#divFundingStatus").removeClass("col-md-3").addClass("col-md-4");
}


function showSupplementalSIGNAPAY() {
    $("#divUrl").removeClass("hide");
}

function RestyleMerchantInformationGrid() {
    $("#divAuthMonitorExclude").removeClass("hide");
    $("#divWorked").removeClass("hide");

    if ($("#divFundingStatus").hasClass("hide")) {
        $("#divAuthMonitorExclude").removeClass("col-md-4").addClass("col-md-3");
    }
    else {
        $("#divFundingStatus").removeClass("col-md-4").addClass("col-md-3");
        $("#divProfile").removeClass("col-md-4").addClass("col-md-3");
    }
}

//OnClientClose
var isSubmitAddDefaultSetting = false;
function PopupModalClose(sender, eventArgs) {
    if (sender._navigateUrl == "SensitiveDataDetectedModal.aspx") {
        if (!isClientClose) {
            parent.SubmitAddNote('', false);
        }
    }
    if (sender._navigateUrl.indexOf("DefaultSettingModal.aspx") >= 0) {
        if (isSubmitAddDefaultSetting) {
            bindSourceAndRole();
            isSubmitAddDefaultSetting = false;
        }

        if (isCMSubmitAddDefaultSetting) {
            document.getElementById(uxFinishSaveDefaultSettingCH).click();
            isCMSubmitAddDefaultSetting = false;
        }
    }
}

var x, y = 0;
function masterAjax_responseEnd(sender, args) {
    //reCreateMultiSelector();
    if ($('#' + uxSourceList).length > 0 && $('#' + uxSourceList).length > 0 && $('#' + uxAddedByList)) {
        $('#' + uxSourceList).chosen('');
        $('#' + uxRoleList).chosen('');
        $('#' + uxAddedByList).chosen('');
    }
    $('[data-hover="dropdown"]').dropdownHover();

    if ($('#' + uxStatuses).length > 0 && $('#' + uxTypes).length > 0 && $('#' + uxPriorityLevel)) {
        $('#' + uxStatuses).chosen('');
        $('#' + uxTypes).chosen('');
        $('#' + uxPriorityLevel).chosen('');
    }

    if ($("#" + AddNote_uxApplyFilter).length > 0) {
        $("#" + AddNote_uxApplyFilter).prop("disabled", false);
    }

    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
    //TK:39919 - #35650
    window.scrollTo(x, y) // Fix issue auto resize content height on Chrome

    //Adjust position of Default Setting link
    if ($("#merchantnotes").find(".report-export a").css("display") == "none") {
        $("#merchantnotes").find("#defaultSetting").css("right", "20px");
    } else {
        $("#merchantnotes").find("#defaultSetting").css("right", "84px");
    }


    if ($("#casehistory").find(".report-export a").css("display") == "none") {
        $("#casehistory").find(".mn-default-link").css("right", "20px");
    } else {
        $("#casehistory").find(".mn-default-link").css("right", "84px");
    }
}