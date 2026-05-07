function validateSubmit() {
    $("#uxValidSummary").html('');
    var isValid = validaInput();
    if (isUpdateMode != 'True') {
        if (typeof valRepeaterItem !== "undefined" && $("#list-white .item-upload").length > 0)
            isValid = valRepeaterItem() && isValid;
        else {
            var messages = $("#uxValidSummary");
            messages.html(messages.html() + '<li>' + msg_RequireDocument + '</li>');
            isValid = false;
        }
    }

    var checkIs = checkExistIssue();
    if (!isValid || checkIs) {
        ShowPopupModal('ValidationModal.aspx', 'auto');
        return false;
    }
    else {
        var urlConfirm = "ConfirmSubmitModal.aspx?IsUpdate=" + isUpdateMode + "&NumberOfFile=" + $("#list-white .item-upload").length;
        ShowPopupModal(urlConfirm, 'auto');
        return false;
    }
}

function submitDocument() {
    $("#" + btnConfirmSubmit).click();
}

function getMessages() {
    return $("#uxValidSummary").html();
}

function checkExistIssue() {
    return $("#list-white .fileIssue:not(.hidden)").length > 0;
}

function getMessagesUpload() {
    var listInvalid = [];
    var listFileOver = [];
    var messinvalid = "";
    var messOver = "";
    if ($("#list-white .fileIssue:not(.hidden)").length > 0) {
        $("#list-white [data-upload]").each(function () {
            if ($(this).find(".fileIssue:not(.hidden)").length > 0) {
                if ($(this).find(".fileIssue:not(.hidden) span").data("issue") === "Invalid") {
                    listInvalid.push($(this).find("[id*='lblText']")[0].innerText);
                }
                else
                    listFileOver.push($(this).find("[id*='lblText']")[0].innerText);
            }
        });

        if (listInvalid.length > 0) {
            messinvalid = "<p>" + ((listInvalid.length > 1) ? msg_Invalid_File_Type_Multi : msg_Invalid_File_Type) + "</p><ul class='text-left ul-default'  style='width: 320px'>";
            for (var i = 0; i < listInvalid.length; i++) {
                messinvalid += '<li class="text-overflow- llipsis" title=' + listInvalid[i] + '>' + listInvalid[i] + '</li>';
            }
            messinvalid += "</ul>";
        }
        if (listFileOver.length > 0) {
            messOver = "<p>" + ((listFileOver.length > 1) ? msg_File_Size_Over_50MB_Multi:msg_File_Size_Over_50MB) + "</p><ul class='text-left ul-default' style='width: 320px'>";
            for (var j = 0; j < listFileOver.length; j++) {
                messOver += '<li class="text-overflow-ellipsis" title=' + listFileOver[j] + '>' + listFileOver[j] + '</li>';
            }
            messOver += "</ul>";
        }
    }
    return messOver + messinvalid;
}

function CheckRequireShareUserorGroup() {
    var shareUsers = $("#" + uxShareWithUsersID).val();
    var shareGroups = $("#" + uxShareWithGroupsID).val();
    if (shareUsers.length == 0 && shareGroups.length == 0) return false;

    return true;
}

window.isInvalidCharacter = false;
function inputDescription(sender, args) {
    if (!checkSpecialCharacter(sender.get_value())) {
        $("label[data-message='" + sender._clientID + "']").addClass("active");
        $(".icon-delete").addClass("disabled");

        if ($("label[data-message='descriptionSpecialCharacter']").length > 0) {
            $("label[data-message='descriptionSpecialCharacter']").addClass("active");
        }
        if ($(".drap-drop-file").length > 0) {
            $(".drap-drop-file").addClass("disable-session");
        }
           
        $('#' + uxShareWithUsersID).prop('disabled', true).trigger("chosen:updated");
        $('#' + uxShareWithGroupsID).prop('disabled', true).trigger("chosen:updated");
  
        sender.focus();
        isInvalidCharacter = true;
        $("#" + uxSubmitID).attr("disabled", "disabled");
    } else {
        $("label[data-message='" + sender._clientID + "']").removeClass("active");
        $(".icon-delete").removeClass("disabled");

        if ($("label[data-message='descriptionSpecialCharacter']").length > 0) {
            $("label[data-message='descriptionSpecialCharacter']").removeClass("active");
        }
        if ($(".drap-drop-file").length > 0) {
            $(".drap-drop-file").removeClass("disable-session");
        }

        isInvalidCharacter = false;
        $("#" + uxSubmitID).removeAttr("disabled");
        $('#' + uxShareWithUsersID).prop('disabled', false).trigger("chosen:updated");
        $('#' + uxShareWithGroupsID).prop('disabled', false).trigger("chosen:updated");
    }

   // AdjustModalSize();
}
