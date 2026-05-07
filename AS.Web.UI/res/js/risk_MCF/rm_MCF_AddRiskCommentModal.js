var limitNum = 2000;
var message = rm_AddRiskCommentModal_js_msg1 + ' ' + limitNum + ' ' + rm_AddRiskCommentModal_js_characters + '!';
var counter = $get('counter');

function LimitLength() {
    setTimeout(function () {
        var rtfEditor = $find(rm_AddRiskCommentModal_uxComment);
        var oValue = rtfEditor.get_text(true).trim();
        counter.innerHTML = "<label class=\"text-muted\">" + rm_AddRiskCommentModal_js_CharacterUpper + ": </label><strong><strong>" + oValue.length + " / " + limitNum + "</strong>";
    }, 500);
}

function AttachHandlers() {
    var rtfEditor = $find(rm_AddRiskCommentModal_uxComment),
        textContentArea = rtfEditor.get_contentArea();

    if (rtfEditor) {
        rtfEditor.attachEventHandler("onkeyup", LimitLength);
        rtfEditor.attachEventHandler("onpaste", LimitLength);
        rtfEditor.attachEventHandler("onblur", LimitLength);
    }
    if (textContentArea) {
        textContentArea.addEventListener("keyup", LimitLength);
        textContentArea.addEventListener("paste", LimitLength);
        textContentArea.addEventListener("blur", LimitLength);
    }
}

function OnClientLoad(editor, args) {
    rtfEditor = editor;
    AttachHandlers();
    LimitLength();

    editor.add_modeChange(function (sender, args) {
        AttachHandlers();
        LimitLength();
    });

    editor.add_commandExecuted(function (sender, args) {
        if (args.get_commandName().toLocaleLowerCase() == "paste") {
            LimitLength();
        }
    });

    // Set background color
    var style = editor.get_contentArea().style;
    style.backgroundColor = "#ffffff";
    style.paddingTop = "10px";

    //Disable menu context
    var toolAdapter = editor.get_toolAdapter();
    if (toolAdapter) {
        toolAdapter.enableContextMenus(false);
    }
}

function ReloadParent() {
    alert(rm_AddRiskCommentModal_js_msg2);
    window.opener.ReloadComment();
    window.close();
}

function Validate() {
    var rtfEditor = $find(rm_AddRiskCommentModal_uxComment);
    var oValue = rtfEditor.get_text(true).trim();

    if (oValue.length > limitNum) {
        $("#ciCommentsMsg").removeClass("display-none");
        $("#ciCommentsMsg").text(message);
        //AdjustWindowSize(657);
        return false;
    }
    $("#ciCommentsMsg").addClass("display-none");

    return true;
}
$(document).ready(function () {
    AdjustWindowSize(748);
});

Telerik.Web.UI.Editor.ClipboardImagesProvider.prototype.supportsClipboardData = function (event) {
    return false;
}