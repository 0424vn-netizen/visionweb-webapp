window.onload = function () {
    setTimeout(loadControl, 100);
}

function loadControl() {
    cb60dayTransCnt_Changed();
    cbMinimumVol_Changed();
    cbContractualVol_Changed();
    BindTransCntType();
}

function cbxTransCntType_OnClientSelectedIndexChanged(sender, args) {
    var result = false;

    BindTransCntType();

    return result;
}

function cb60dayTransCnt_Changed() {
    if ($get(Risk_Assignment_Filter_Transactional_cb60dayTransCnt).checked == false) {

        $find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).clear();
        $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).clear();
        $find(Risk_Assignment_Filter_Transactional_txtGt).clear();
        $find(Risk_Assignment_Filter_Transactional_txtLt).clear();

        $find(Risk_Assignment_Filter_Transactional_cbxTransCntType).disable();
        $find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).disable();
        $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).disable();
        $find(Risk_Assignment_Filter_Transactional_txtGt).disable();
        $find(Risk_Assignment_Filter_Transactional_txtLt).disable();

    }
    else {
        $find(Risk_Assignment_Filter_Transactional_cbxTransCntType).enable();
        $find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).enable();
        $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).enable();
        $find(Risk_Assignment_Filter_Transactional_txtGt).enable();
        $find(Risk_Assignment_Filter_Transactional_txtLt).enable();

    }
}

function cbMinimumVol_Changed() {
    if ($get(Risk_Assignment_Filter_Transactional_cbMinimumVol).checked == false) {
        $find(Risk_Assignment_Filter_Transactional_txtMinimumVol).clear();
        $find(Risk_Assignment_Filter_Transactional_txtMinimumVol).disable();
    }
    else {
        $find(Risk_Assignment_Filter_Transactional_txtMinimumVol).enable();
    }
}

function cbContractualVol_Changed() {
    if ($get(Risk_Assignment_Filter_Transactional_cbContractualVol).checked == false) {
        $find(Risk_Assignment_Filter_Transactional_txtContractualVol).clear();
        $find(Risk_Assignment_Filter_Transactional_txtContractualVol).disable();
    }
    else {
        $find(Risk_Assignment_Filter_Transactional_txtContractualVol).enable();
    }
}

function BindTransCntType() {
    filterTypeValue = $find(Risk_Assignment_Filter_Transactional_cbxTransCntType).get_value();

    switch (filterTypeValue) {
        case "Between":
            $get("cidBetween").style.display = "";
            $get("cidGreater").style.display = "none";
            $get("cidLess").style.display = "none";
            break;
        case "GreaterThan":
            $get("cidBetween").style.display = "none";
            $get("cidGreater").style.display = "";
            $get("cidLess").style.display = "none";
            break;
        case "LessThan":
            $get("cidBetween").style.display = "none";
            $get("cidGreater").style.display = "none";
            $get("cidLess").style.display = "";
            break;
    }
    $('label[for=' + Risk_Assignment_Filter_Transactional_txtBetweenFrom + ']').removeClass('label-error');
    $('label[for=' + Risk_Assignment_Filter_Transactional_txtBetweenFrom + ']').parent().find(".error").css("display","none");

}



function Validate60dayTransCnt_Required() {
    var cb60dayTransCnt = $get(Risk_Assignment_Filter_Transactional_cb60dayTransCnt);
    var isValid = true;
    //check 60 day count
    if (cb60dayTransCnt.checked == true) {
        var filterTypeValue = $find(Risk_Assignment_Filter_Transactional_cbxTransCntType).get_value();


        switch (filterTypeValue) {
            case "Between":
                if ($find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).get_textBoxValue() == "" || $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).get_textBoxValue() == "") {
                    if ($find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).get_textBoxValue() == "") {
                        $find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).focus();
                    }
                    else {
                        $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).focus();
                    }
                    isValid = false;
                }
                break;
            case "GreaterThan":
                if ($find(Risk_Assignment_Filter_Transactional_txtGt).get_textBoxValue() == "") {
                    $find(Risk_Assignment_Filter_Transactional_txtGt).focus();
                    isValid = false;
                }
                break;
            case "LessThan":
                if ($find(Risk_Assignment_Filter_Transactional_txtLt).get_textBoxValue() == "") {
                    $find(Risk_Assignment_Filter_Transactional_txtLt).focus();
                    isValid = false;
                }
                break;
        }
    }
    return isValid;
}


function Validate60dayTransCnt_Between_Greater() {
    var cb60dayTransCnt = $get(Risk_Assignment_Filter_Transactional_cb60dayTransCnt);
    var isValid = true;

    if (cb60dayTransCnt.checked == true) {
        var filterTypeValue = $find(Risk_Assignment_Filter_Transactional_cbxTransCntType).get_value();

        switch (filterTypeValue) {
            case "Between":
                if (!$find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).get_textBoxValue() == "" && !$find(Risk_Assignment_Filter_Transactional_txtBetweenTo).get_textBoxValue() == "") {
                    //check The “To” value must be greater than the “From” value.
                    var from = $find(Risk_Assignment_Filter_Transactional_txtBetweenFrom).get_value();
                    var to = $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).get_value();

                    if (from > to) {
                        $find(Risk_Assignment_Filter_Transactional_txtBetweenTo).focus();
                        return false;
                        result = 'invalid';
                    }
                }
        }

    }
    return isValid;
}


function ValidateMinimumVol_Required() {
    var cbMinimumVol = $get(Risk_Assignment_Filter_Transactional_cbMinimumVol);

    //check minimun vol
    if (cbMinimumVol.checked == true) {
        if ($find(Risk_Assignment_Filter_Transactional_txtMinimumVol).get_textBoxValue() == "") {
            $find(Risk_Assignment_Filter_Transactional_txtMinimumVol).focus();
            return false;
        }
    }
    return true;
}

function ValidateContractualVol_Required() {
    var cbContractualVol = $get(Risk_Assignment_Filter_Transactional_cbContractualVol);
    //check contractual vol
    if (cbContractualVol.checked == true) {
        if ($find(Risk_Assignment_Filter_Transactional_txtContractualVol).get_textBoxValue() == "") {
            $find(Risk_Assignment_Filter_Transactional_txtContractualVol).focus();
            return false;
        }
    }
    return true;
}



function ValidateTransactionalFilter() {

    var cb60dayTransCnt = $get(Risk_Assignment_Filter_Transactional_cb60dayTransCnt);
    var cbMinimumVol = $get(Risk_Assignment_Filter_Transactional_cbMinimumVol);
    var cbContractualVol = $get(Risk_Assignment_Filter_Transactional_cbContractualVol);
    var result = 'valid';


    if (ValidateTransactionalFilterExt() == false) {
        result = 'invalid';
    }

    if (cb60dayTransCnt.checked == false && cbContractualVol.checked == false && cbMinimumVol.checked == false) {
        result = 'none';
    }

    return result;
}
