function doHeaderCheck(sender) {
    var uxIsAdded = $("#" + uxIsAddedClientID);
    var uxIsSelectAll = $("#" + uxIsSelectAllClientID);
    var uxValueCode = $("#" + uxValueCodeClientID);
    var uxAllFlag = $("#" + uxAllFlagClientID);

    uxIsSelectAll.val(IS_SELECT_ALL_FLAG);
    uxIsAdded.val(sender.checked);
    uxValueCode.val("");

    if (sender.checked) {
        uxAllFlag.val(IS_SELECT_ALL_FLAG);
        $("input[id*=chkItem]").prop("checked", true);
    } else {
        uxAllFlag.val(IS_DESELECT_ALL_FLAG);
        $("input[id*=chkItem]").prop("checked", false);
    }
    $("#" + btnHiddenClientID).click();
}

function doItemCheck(sender, sicCode) {
    var uxIsAdded = $("#" + uxIsAddedClientID);
    var uxIsSelectAll = $("#" + uxIsSelectAllClientID);
    var uxValueCode = $("#" + uxValueCodeClientID);

    uxIsSelectAll.val(IS_DESELECT_ALL_FLAG);
    uxIsAdded.val(sender.checked);
    uxValueCode.val(sicCode);
    if (!sender.checked) {
        $("input[id*=chkHeader]").removeAttr("checked");
        $("#" + uxAllFlagClientID).val(IS_DESELECT_ALL_FLAG);
    }
    $("#" + btnHiddenClientID).click();
}

function ClearAll() {
    var uxAllFlag = $("#" + uxAllFlagClientID);
    uxAllFlag.val(IS_DESELECT_ALL_FLAG);
}

function RefreshFilter(haveFilterExpression) {
    var uxAllFlag = $("#" + uxAllFlagClientID);
    if (uxAllFlag.val() == "true")
        $("input[id*=chkHeader]").prop("checked", true);
    if (haveFilterExpression == "True") {
        if (uxAllFlag.val() == "true") {
            $("input[id*=chkItem]").prop("disabled", "disabled");
            $("input[id*=Filter_DataKey]").prop("disabled", "disabled");
        }
        else {
            $("input[id*=chkItem]").removeAttr("disabled");
            $("input[id*=Filter_DataKey]").removeAttr("disabled");
        }
    }
}