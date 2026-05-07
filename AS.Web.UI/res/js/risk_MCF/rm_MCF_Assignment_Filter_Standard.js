$(document).ready(function () {
    loadControlDaytoFundStandard();
})

function loadControlDaytoFundStandard() {
    cbDaystoFundStandard_Changed();
    BindDaystoFundStandard();

}

function cbxDaystoFundStandard_OnClientSelectedIndexChanged(sender, args) {
    var result = false;

    BindDaystoFundStandard();
    return result;
}

function cbDaystoFundStandard_Changed() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFundStandard);
    if (cbDaystoFund != undefined) {
        if ($get(Risk_Assignment_Filter_DaystoFund_cbDaystoFundStandard).checked == false) {

            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).clear();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).clear();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFGtStandard).clear();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFLtStandard).clear();

            $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFundStandard).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFGtStandard).disable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFLtStandard).disable();

        }
        else {
            $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFundStandard).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFGtStandard).enable();
            $find(Risk_Assignment_Filter_DaystoFund_txtDTFLtStandard).enable();

        }
    }
}

function info_closeModalEvent(modalID) {
    if (modalID == "RiskRatingModal") {
        document.getElementById(Risk_Assignment_Filters_uxRebindRiskRating).click();
    }
}

function BindDaystoFundStandard() {
    var filterType = $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFundStandard);
    if (filterType != undefined) {
        var filterTypeValue = filterType.get_value();
        switch (filterTypeValue) {
            case "Between":
                $get("cidDTFBetweenStandard").style.display = "";
                $get("cidDTFGreaterStandard").style.display = "none";
                $get("cidDTFLessStandard").style.display = "none";
                break;
            case "GreaterThan":
                $get("cidDTFBetweenStandard").style.display = "none";
                $get("cidDTFGreaterStandard").style.display = "";
                $get("cidDTFLessStandard").style.display = "none";
                break;
            case "LessThan":
                $get("cidDTFBetweenStandard").style.display = "none";
                $get("cidDTFGreaterStandard").style.display = "none";
                $get("cidDTFLessStandard").style.display = "";
                break;
        }

        $('label[for=' + Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard + ']').removeClass('label-error');
        $('label[for=' + Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard + ']').parent().find(".error").css("display", "none");
    }


}

function ValidateDaystoFundStandard_Required() {
    var cbDaystoFundStandard = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFundStandard);
    var isValid = true;
    //check 60 day count
    if (cbDaystoFundStandard.checked) {
        var filterTypeValue = $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFundStandard).get_value();
        switch (filterTypeValue) {
            case "Between":
                if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).get_textBoxValue() == "" ||
                    $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).get_textBoxValue() == "") {
                    if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).get_textBoxValue() == "") {
                        $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).focus();
                    }
                    else {
                        $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).focus();
                    }
                    isValid = false;
                }
                break;
            case "GreaterThan":
                if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFGtStandard).get_textBoxValue() == "") {
                    $find(Risk_Assignment_Filter_DaystoFund_txtDTFGtStandard).focus();
                    isValid = false;
                }
                break;
            case "LessThan":
                if ($find(Risk_Assignment_Filter_DaystoFund_txtDTFLtStandard).get_textBoxValue() == "") {
                    $find(Risk_Assignment_Filter_DaystoFund_txtDTFLtStandard).focus();
                    isValid = false;
                }
                break;
        }
    }
    return isValid;
}

function ValidateDaystoFundStandard_Between_Greater() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFundStandard);
    var isValid = true;

    if (cbDaystoFund.checked == true) {
        var filterTypeValue = $find(Risk_Assignment_Filter_DaystoFund_cbxDaystoFundStandard).get_value();

        switch (filterTypeValue) {
            case "Between":
                if (!$find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).get_textBoxValue() == "" &&
                    !$find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).get_textBoxValue() == "") {
                    //check The “To” value must be greater than the “From” value.
                    var from = $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenFromStandard).get_value();
                    var to = $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).get_value();

                    if (from >= to) {
                        $find(Risk_Assignment_Filter_DaystoFund_txtDTFBetweenToStandard).focus();
                        return false;
                        result = 'invalid';
                    }
                }
        }

    }
    return isValid;
}

function ValidateDaystoFundStandardFilter() {
    var cbDaystoFund = $get(Risk_Assignment_Filter_DaystoFund_cbDaystoFundStandard);
    var result = 'valid';

    if (ValidateDaystoFundFilterStandard() == false) {
        result = 'invalid';
    }

    if (cbDaystoFund != undefined && cbDaystoFund.checked == false || cbDaystoFund == undefined) {
        result = 'none';
    }
    return result;

}

function ValidateCreditScoreStandard() {
    var result = 'none';
    var creditScoreIsFrom = document.getElementById(Risk_Assignment_Filters_uxChkIsFromStandard);
    var creditScoreIsTo = document.getElementById(Risk_Assignment_Filters_uxChkIsToStandard);
    if ((creditScoreIsFrom != undefined && creditScoreIsFrom.checked) || (creditScoreIsTo != undefined && creditScoreIsTo.checked)) {
        result = 'valid';
    }
    return result;
}