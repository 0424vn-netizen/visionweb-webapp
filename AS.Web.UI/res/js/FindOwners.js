
var selectedOwners = null;
function doSetSelectedOwners(ownerID, checked) {
    if (ownerID == '') return;
    selectedOwners = document.getElementById(uxSelectedOwnersClientID);
    selectedOwners.value = setValue(selectedOwners.value, ownerID, checked);
}


function setValue(vals, val, isAdd) {
    var newVal = '';
    var arr = vals.split(';');
    if (isAdd) {
        if (!arr.includes(val)) {
            arr.push(val);
        }
        else {
            newVal = vals;
        }
    }
    else {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == val) {
                arr.splice(i, 1)
                break;
            }
        }
    }
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == '') {
            arr.splice(i, 1)
            break;
        }
    }
    return arr.join(';');
}

$(document).ready(function () {
    if (!isEditMode) {
        if (parent.GetSelectedMetric != null) {
            selectedOwners = document.getElementById(uxSelectedOwnersClientID);
            selectedOwners.value = parent.GetSelectedMetric(MetricControlID);
            doSetSelecting();
        }
    }
    else {
        if (parent.GetSelectedMetricEdit != null) {
            selectedOwners = document.getElementById(uxSelectedOwnersClientID);
            selectedOwners.value = parent.GetSelectedMetricEdit(MetricControlID);
            doSetSelecting();
        }
    }
});
function doSubmit() {
    if (parent.SetSelectedMetric != null) {
        selectedOwners = document.getElementById(uxSelectedOwnersClientID);
        parent.SetSelectedMetric(selectedOwners.value);
        parent.HidePopupModal();
    }
}

function doSubmitEdit() {
    if (parent.SetSelectedMetricEdit != null) {
        selectedOwners = document.getElementById(uxSelectedOwnersClientID);
        parent.SetSelectedMetricEdit(selectedOwners.value);
        parent.HidePopupModal();
    }

}

function doSubmitFromChildModal(controlID) {
    if (parent.SetSelectedMetric != null) {
        selectedOwners = document.getElementById(uxSelectedOwnersClientID);
        //console.log(selectedOwners.value);
        parent.SetSelectedMetric(controlID, selectedOwners.value);
        parent.HidePopupModalChild(1);
    }
}

function doSubmitEditFromChildModal(controlID) {
    if (parent.SetSelectedMetricEdit != null) {
        selectedOwners = document.getElementById(uxSelectedOwnersClientID);
        parent.SetSelectedMetricEdit(controlID, selectedOwners.value);
        parent.HidePopupModalChild(1);
    }
}

function doSetSelecting() {
    setTimeout(function () {
        selectedOwners = document.getElementById(uxSelectedOwnersClientID);
        $('#grid-row table').find('input[type="checkbox"]').each(function () {


            if (selectedOwners.value.split(';').includes($(this).val().trim())) {
                $(this).attr("checked", true);
            }

        });
    }, 100);
}
function masterAjax_responseEnd(sender, args) {
    selectedOwners = document.getElementById(uxSelectedOwnersClientID);
    $('#grid-row table').find('input[type="checkbox"]').each(function () {
        if (selectedOwners.value.split(';').includes($(this).val().trim())) {
            $(this).attr("checked", true);
        }
    });
}