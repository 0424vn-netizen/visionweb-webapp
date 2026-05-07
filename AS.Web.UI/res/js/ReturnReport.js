function doCheckVerifyAll(val) {

    var frm = document.forms[0];
    var ctrl;
    for (i = 0; i < frm.length; i++) {
        ctrl = frm.elements[i];
        if (ctrl.id.indexOf('chkVerify') != -1) {
            ctrl.checked = val;
        }
        if (ctrl.id.indexOf('chkAllVerify') != -1) ctrl.checked = val;
    }
}
function doVerifyReturns(recordid) {
    var mess;

    if (recordid == '-1') {
        doCheckVerifyAll(true);
        mess = ReturnReport_Returns_ComfirmVerify_All;
    } else {
        mess = ReturnReport_Returns_ComfirmVerify;
    }
    if (confirm(mess)) {
        document.getElementById(ReturnReport_uxHidArgs).value = recordid;
        document.getElementById(ReturnReport_uxHidClick).value = "ClickedVerify";
        document.getElementById(ReturnReport_uxVerifyMerchant).click();
    }
    else {
        doCheckVerifyAll(false);
    }
}

$(document).ready(function () {
    //SetHeaderGrid();
    addCheckSpecialCharactersForDate();
});

function ajaxResponseEnd(sender, args) {
    //SetHeaderGrid();
}

function OnGridDataBound() {
    SetHeaderGrid();
}

function SetHeaderGrid() {
    setTimeout(function () {
        if (isEnableEntityName == 'True') {
            addGroupHeadersForStaticRadGrid(ReturnReport_uxDrilldownGrid,
                   [
                       ['', 1, 'rgHeader mh'],
                       ['', 1, 'rgHeader mh'],
                       ['', 1, 'rgHeader mh'],
                       ['', 1, 'rgHeader mh'],
                       [ReturnReport_js_Returns, 4, 'rgHeader mh'],
                       ['', 1, 'rgHeader mh']
                   ]);
        }
        else {
        addGroupHeadersForStaticRadGrid(ReturnReport_uxDrilldownGrid,
        [
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            [ReturnReport_js_Returns, 4, 'rgHeader mh'],
            ['', 1, 'rgHeader mh']
        ]);
        }
        appendHeaderforPrinter(ReturnReport_uxDrilldownGrid);
    }, 1);
}