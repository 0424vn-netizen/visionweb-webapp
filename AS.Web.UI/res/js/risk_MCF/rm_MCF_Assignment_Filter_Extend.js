
$(document).ready(function () {
    loadControlDaytoFund();
})

function loadControlDaytoFund() {
    cbDaystoFund_Changed();
    BindDaystoFund();

}

function cbxDaystoFund_OnClientSelectedIndexChanged(sender, args) {
    var result = false;

    BindDaystoFund();
    return result;
}


function cbDaystoFund_Changed() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFund);
    if (cbDaystoFund != undefined) {
        if ($get(Risk_Assignment_Filter_DaystoFund_cbDaystoFund).checked == false) {

            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).clear();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).clear();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFGt).clear();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFLt).clear();

            $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFund).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFGt).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFLt).disable();

        }
        else {
            $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFund).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFGt).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFLt).enable();

        }
    }
}


function BindDaystoFund() {
    var filterType = $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFund);
    if (filterType != undefined) {
        filterTypeValue = filterType.get_value();
        switch (filterTypeValue) {
            case "Between":
                $get("cidDTFBetween").style.display = "";
                $get("cidDTFGreater").style.display = "none";
                $get("cidDTFLess").style.display = "none";
                break;
            case "GreaterThan":
                $get("cidDTFBetween").style.display = "none";
                $get("cidDTFGreater").style.display = "";
                $get("cidDTFLess").style.display = "none";
                break;
            case "LessThan":
                $get("cidDTFBetween").style.display = "none";
                $get("cidDTFGreater").style.display = "none";
                $get("cidDTFLess").style.display = "";
                break;
        }

        $('label[for=' + Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom + ']').removeClass('label-error');
        $('label[for=' + Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom + ']').parent().find(".error").css("display", "none");
    }
    

}

function ValidateDaystoFund_Required() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFund);
    var isValid = true;
    //check 60 day count
    if (cbDaystoFund.checked == true) {
        var filterTypeValue = $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFund).get_value();
        switch (filterTypeValue) {
            case "Between":
                if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).get_textBoxValue() == "" ||
                    $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).get_textBoxValue() == "") {
                    if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).get_textBoxValue() == "") {
                        $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).focus();
                    }
                    else {
                        $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).focus();
                    }
                    isValid = false;
                }
                break;
            case "GreaterThan":
                if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFGt).get_textBoxValue() == "") {
                    $find(Risk_Assignment_Filter_DaystoFund_txtDTFGt).focus();
                    isValid = false;
                }
                break;
            case "LessThan":
                if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFLt).get_textBoxValue() == "") {
                    $find(Risk_Assignment_Filter_DaystoFund_txtDTFLt).focus();
                    isValid = false;
                }
                break;
        }
    }
    return isValid;
}

function ValidateDaystoFund_Between_Greater() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFund);
    var isValid = true;

    if (cbDaystoFund.checked == true) {
        var filterTypeValue = $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFund).get_value();

        switch (filterTypeValue) {
            case "Between":
                if (!$find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).get_textBoxValue() == "" &&
                    !$find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).get_textBoxValue() == "") {
                    //check The “To” value must be greater than the “From” value.
                    var from = $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFrom).get_value();
                    var to = $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).get_value();

                    if (from >= to) {
                        $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenTo).focus();
                        return false;
                        result = 'invalid';
                    }
                }
        }

    }
    return isValid;
}

function ValidateDaystoFundFilter() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFund);
    var result = 'valid';

    if (ValidateDaystoFundFilterExt() == false) {
        result = 'invalid';
    }

    if (cbDaystoFund != undefined && cbDaystoFund.checked == false || cbDaystoFund == undefined) {
        result = 'none';
    }
    return result;

}


function watchFutureDeliveryStatusNAIsChecked() {
    var item = document.getElementById(Risk_Assignment_Filters_rdFutureDeliveryStatusNA);
    if (item == undefined) return true;
    return item.checked;
}

function ValidateCreditScoreCustom() {
    var result = 'none';
    var creditScoreIsFrom = document.getElementById(Risk_Assignment_Filters_uxChkIsFromCustom);
    var creditScoreIsTo = document.getElementById(Risk_Assignment_Filters_uxChkIsToCustom);
    if ((creditScoreIsFrom != undefined && creditScoreIsFrom.checked) || (creditScoreIsTo != undefined && creditScoreIsTo.checked)) {
        result = 'valid';
    }
    return result;
}