function checkBoxClick(object, parameterKey) {
    if (object.checked) {
        ParameterKeyList += parameterKey + ","
        document.getElementById(Assignment_Filter_Parameters_hdParamSelected).value = ParameterKeyList.substring(0, ParameterKeyList.length - 1);

    }
    else {
        ParameterKeyList = ParameterKeyList.replace(parameterKey + ",", "");
        document.getElementById(Assignment_Filter_Parameters_hdParamSelected).value = ParameterKeyList.substring(0, ParameterKeyList.length - 1);

    }
}

function chkAllParameters(checkBox) {
    if (checkBox.checked) {
        for (var i = 0; i < ParameterInfo.length; i++) {
            var paramInfo = ParameterInfo[i];
            var cb = $get(paramInfo.CbClientId);
            if (!cb.checked) {
                cb.checked = true;
                ParameterKeyList += paramInfo.ParamKey + ","
                document.getElementById(Assignment_Filter_Parameters_hdParamSelected).value = ParameterKeyList.substring(0, ParameterKeyList.length - 1);
            }
        }
    }
    else {
        for (var i = 0; i < ParameterInfo.length; i++) {
            var paramInfo = ParameterInfo[i];
            var cb = $get(paramInfo.CbClientId);
            if (cb.checked) {
                cb.checked = false;
                ParameterKeyList = ParameterKeyList.replace(paramInfo.ParamKey + ",", "");
                document.getElementById(Assignment_Filter_Parameters_hdParamSelected).value = ParameterKeyList.substring(0, ParameterKeyList.length - 1);
            }
        }
    }
}

function testCheckAll() {
    for (var i = 0; i < ParameterInfo.length; i++) {
        var paramInfo = ParameterInfo[i];
        var cb = $get(paramInfo.CbClientId);
        if (!cb.checked) {
            return;
        }
    }
    document.getElementById(Assignment_Filter_Parameters_uxCheckAll).checked = true;
}