
function onAddAssociation() {
    var assoId = $("#" + ManageAssociationExclusion_uxAssoID).val();
    if (assoId == "") {
        showRadMessage("alert", requiredMsg, null, comfirmationTitle);
        return false;
    }
    else {
        var data = {};
        data["assoId"] = assoId;

        postServer("ManageAssociationExclusion.aspx/IsAssoIdExsit", data, function (result) {
            if (result.d) {
                showRadMessage("alert", existMsg, null, comfirmationTitle);
            }
            else {
                showRadConfirm("confirm", passValidationMsg, function (arg) {
                    if (arg) {
                        __doPostBack(ManageAssociationExclusion_uxBntAddAssoID, '');
                    }
                }, comfirmationTitle, Ok, Cancel);
            }
        })
    }
    return false;
}

function allowOnlyNumber() {

    var obj = $("#" + ManageAssociationExclusion_uxAssoID);
    var val = obj.val();
    var rexg = /^([0-9]+)/g;
    val = rexg.exec(val);
    if (val) {
        obj.val(val[0]);
    } else {
        obj.val("");
    }
}

function onRemoveAssociation(e) {
    showRadConfirm("confirm", removeMsg, function (arg) {
        if (arg) {
            var uniqueId = $(e).attr('id').replace(new RegExp('_', 'g'), '$');
            WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(uniqueId, "", true, "", "", false, true))
        }
    }, comfirmationTitle, Ok, Cancel);
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

function masterAjax_requestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}