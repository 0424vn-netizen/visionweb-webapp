

//==================================== API =====================================================
function doSave() {
    var pm = checkParameterValid();
    if (!pm)
        return false;

    //do postback
    var bt = $get(Risk_Parameter_btSave);
    bt.click();
}

function doSaveParameterList(isNeedSelectParam) {
    if (isNeedSelectParam) {
        var pm = checkParameterValid();
        if (!pm)
            return false;
    }
    //do postback
    var bt = $get(Risk_Parameter_btSave);
    bt.click();
}

function checkParameterValid() {
    var count = countSelectedParameters();
    if (count == 0) {
        alert(Risk_Parameter_js_ParameterMustBeSelected);
        return false;
    }
    return true;
}

function checkParameterValid1() {
    var count = countSelectedParameters();
    if (count == 0) {
        return false;
    }
    return true;
}
