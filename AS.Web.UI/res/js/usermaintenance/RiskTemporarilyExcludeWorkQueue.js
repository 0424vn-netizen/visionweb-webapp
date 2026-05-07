
var uxSubmit = document.getElementById(TemporarilyExcludeWorkQueue_uxUpdate_ClientID);
function DefaultEnterOnTextBox(e) {

    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        if (isIncludeSpecialCharacters(e.currentTarget)) {
            return false;
        }
        if (isIE)
            setTimeout("uxSubmit.click()", 200);
        else {
            uxSubmit.focus();
            uxSubmit.click();
        }
        return false;
    }
}

function ValidateData(workQueue, sDate, eDate, mode) {
    var isValidOther = doValidation(false);

    if (isValidOther) {
        var msg = rm_TemporarilyExcludeWorkQueue_ConfirmExclude;
        var workQueueName = workQueue;
        if (mode == 'add')
            workQueueName = $("#" + TemporarilyExcludeWorkQueue_uxExcludeWorkQueue_ClientID).val();
        var autoQueues = $("#" + TemporarilyExcludeWorkQueue_uxAutoQueueName_ClientID + "_chosen li.search-choice span");
        var autoQueueNames = '';
        autoQueues.each(function () {
            autoQueueNames = autoQueueNames + this.innerText + ', ';
        });
        autoQueueNames = autoQueueNames.substring(0, autoQueueNames.length - 2);
        msg = msg.replace('[WorkQueueName]', workQueueName);
        msg = msg.replace('[AutoQueueNames]', autoQueueNames);
        msg = msg.replace('[FromDate]', document.getElementById(sDate + '_dateInput').value);
        msg = msg.replace('[ToDate]', document.getElementById(eDate + '_dateInput').value);
        showRadMessage('confirm', msg, function (arg) {
            if (arg) $("#" + TemporarilyExcludeWorkQueue_uxCreate_ClientID).click();
        }, modalTitleConfirm);
        return false;
    }
    else {
        //cheat to show correct layout for error message belong to "To" textbox
        var errToDate = $("label[for='" + TemporarilyExcludeWorkQueue_uxEDate_ClientID + "'].error").text();
        if (errToDate != "") {
            $("#uxEDateErrMsgContainer").addClass("height-20");
        }
        return false;
    }
}

function addCheckSpecialCharacters() {
    $('#' + TemporarilyExcludeWorkQueue_uxSDate_ClientID + '_dateInput').change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            doValidationStartDate();
            removeSpecialCharacters(this);
        }
    });
    $('#' + TemporarilyExcludeWorkQueue_uxEDate_ClientID + '_dateInput').change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            doValidationEndDate();
            removeSpecialCharacters(this);
        }
    });
}

$(document).ready(function () {
    addCheckSpecialCharacters();
});
