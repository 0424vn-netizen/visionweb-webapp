$(document).ready(function () {
    appendHTML();
    $("input[type='checkbox']").each(function () {
        $(this).prop("checked", true);
    })
})
function doSetSelectedExportOptions(optionId, checked) {
    if (!checked)
        $("#btnSelectAll").prop("checked", false);
}

function doSetSelectedAll(isCheck) {
    var exportOptions = $("#optionExport");
    var nodes = exportOptions.find("input[type='checkbox']");
    for (var i = 0; i < nodes.length; i++) {
        if (isCheck) {
            nodes[i].checked = true;
        }
        else {
            nodes[i].checked = false;
        }
    }
}


function uxCancel_Click() {

    return parent.HidePopupModal();
}

function appendHTML() {
    var html = parent.getPanelListSections();
    $('#optionExport').append(html);
}
function uxSubmit_Click(e) {
    var listExportOptions = [];
    var exportOptions = $("#optionExport");
    var nodes = exportOptions.find("input[type='checkbox']");

    for (var i = 0; i < nodes.length; i++) {
        if (nodes[i].checked) {
            listExportOptions.push(nodes[i].value);
        }
    }
    
    if (listExportOptions.length == 0) {
        $(e).blur();
        alert(msgValidation);
        return false;
    }
    else {
        parent.doExportMultisections(listExportOptions);

        return parent.HidePopupModal();
    }
}