var maxChars = 7000;
function radEditorLoad(editor) {
    var contentArea = editor.get_contentAreaElement();
    contentArea.removeAttribute("title");
}

function onPasteComment(editor, args) {
    removeHtml(editor, args);
}

function DoClose() {
    try {
        parent.HidePopupModal();
    }
    catch (err) { }
}


function validateCommentLength() {
    var lengthComment = $find(EditNote_uxCommentID).get_text().length;
    var value = $find(EditNote_uxCommentID).get_text();

    if (lengthComment > maxChars) {
        $('#textRemaining').html("<span style='color:red;'>" + textAlert + "<span>");
        document.getElementById(EditNote_uxSubmitID).setAttribute("disabled", "disabled");
        return false;
    }
    else {
        document.getElementById(EditNote_uxSubmitID).removeAttribute("disabled");
        $('#textRemaining').html(StringFormatText(EditNote_js_Characters, (maxChars - value.length)));
    }
}

function OnClientLoad(editor, args) {

    editor.attachEventHandler("oninput", function (e) {
        validateCommentLength();
    })
    editor.attachEventHandler("onkeyup", function (e) {
        validateCommentLength();
    })
}

$(document).ready(function () {
    validateCommentLength();    
})

function Validate() {

    var rtfEditor = $find(EditNote_uxCommentID);
    var oValue = rtfEditor.get_text(true).trim();
    if (oValue.length == 0) {
        $("#ciCommentsMsg").removeClass("display-none");
        $("#ciCommentsMsg").text(EditNote_js_Required);
        return false;
    }
    if (oValue.length > maxChars) {
        $("#ciCommentsMsg").removeClass("display-none");
        $("#ciCommentsMsg").text(StringFormatText(EditNote_js_msg1, maxChars));
        return false;
    }
    $("#ciCommentsMsg").addClass("display-none");
    CheckSensitiveData();
}
function CheckSensitiveData() {
    var rtfEditor = $find(EditNote_uxCommentID);
    var oValue = rtfEditor.get_text(true).trim();
    var data = {};
    data["comment"] = oValue;
    var url = "rm_MCF_RiskReport.aspx/CheckSensitiveData";
    postServer(url, data, function (result) {
        var cardValid = result.d;
        if (cardValid != "" && cardValid != null) {
            window.cardNumbers = cardValid;
            ShowPopupModal(rootURL + 'SensitiveDataDetectedModal.aspx?Action=Edit', 'auto');
        } else {         
            __doPostBack(EditNote_uxSubmitUniqueID, '');
        }
    })
    return false;
}
function postServer(url, data, callback) {
    $.ajax({
        type: "post",
        url: url,
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            if (callback) {
                callback(result);
            }
        },
        error: function (result) {
            //console.log(result.responseText);
        }
    });
}

SubmitEditNote = function (cardKeyList) {
    HidePopupModal();
    document.getElementById(EditNote_hdCardDetected).value = cardKeyList.substring(0, cardKeyList.length - 1);
    __doPostBack(EditNote_uxSubmitUniqueID, '');
}
StringFormatText = function (str, ...values) {
    return str.replace(/{(\d+)}/g, function (match, index) {
        return typeof values[index] !== 'undefined' ? values[index] : match;
    });
}
