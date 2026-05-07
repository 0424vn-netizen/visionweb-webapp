const merchantFraudFilter = (function() {
    function changeDateOption(chk) {
        let now = new Date();
        const dateOpt = document.getElementById("divDate");
        const dateRangeOpt = document.getElementById("divDateRange");
        if (chk.value === 'uxRange') {
            dateOpt.style.display = "none";
            dateRangeOpt.style.display = "inline";

            const picker1 = $find(uxFromDate_ClientID);
            const picker2 = $find(uxEndDate_ClientID);

            if (!picker2.get_selectedDate())
                picker2.set_selectedDate(now);
            now.setDate(now.getDate() - 29);
            if (!picker1.get_selectedDate())
                picker1.set_selectedDate(now);
        }
        else {

            dateOpt.style.display = "inline";
            dateRangeOpt.style.display = "none";
            const picker = $find(uxDate_ClientID);
            if (!picker.get_selectedDate())
                picker.set_selectedDate(now);
        }
    }
    function keepDateOption() {
        const rangeDate = document.getElementById(uxRange_ClientID);
        if (rangeDate.checked) {
            changeDateOption(rangeDate);
        }
    }
    function compareToday(datePicker) {
        let currentDate = new Date();
        const datePickerValue = datePicker.get_textBox(uxDate_ClientID).value;
        const isValid = isDate(datePickerValue);
        if (!isValid) {
            alert(invalidDateMsg);
            return false;
        }
        const selectedDate = new Date(datePickerValue);
        if (selectedDate > currentDate) {
            alert(dateMustNotBeGreaterThanTodayMsg);
            return false;
        }
        return true;
    }
    function checkDate() {       
        const uxDate = $find(uxDate_ClientID);      

        if (uxDaily?.checked || uxMonthly?.checked) {            
            if (uxDate.get_textBox().value !== "") {
                if (!compareToday(uxDate))
                    return false;
            }
            else {
                alert(invalidDateMsg);
                return false;
            }
        }
        else {            
            return checkFromDateToDate();
        }
        return true;
    }

    function checkFromDateToDate() {
        const fromDate = $find(uxFromDate_ClientID);
        const toDate = $find(uxEndDate_ClientID);

        if (fromDate.get_textBox().value === "" || toDate.get_textBox().value === "") {
            alert(invalidDateMsg);
            return false;
        }

        if (!compareToday(fromDate))
            return false;

        if (!compareToday(toDate))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(endDateMustBeGreaterThanFromDateMsg);
            return false;
        }

        const fromDateObj = new Date(fromDate.get_selectedDate());
        const endDateObj = new Date(toDate.get_selectedDate());

        const diffTime = Math.abs(endDateObj - fromDateObj);
        const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
        if (diffDays > 30) {
            alert(dateRangeNotExceedDayMsg);
            return false;
        }

        return true;
    }

    function checkMerchantNumber() {
        let cbPaymentEntitieTypes = $find(uxPaymentEntitieTypes_ClientID);
        let cbPaymentEntitieTypeValue = cbPaymentEntitieTypes.get_selectedItem().get_value();
                        
        let tbMerchant = $('#' + uxMerchantNumber_ClientID)[0];
        
        if (cbPaymentEntitieTypeValue == '3') {
            let merchantNumber = $('#' + uxMerchantNumber_ClientID).val().trim();
            if (merchantNumber.length > 0) {
                if (merchantNumber.length < 3) {
                    alert(validationMIDLessThan3Chars);
                    tbMerchant.focus();
                    tbMerchant.select();
                    return false;
                }

                const reg = /^\d{1,16}$/;
                if (!reg.test(merchantNumber)) {
                    alert(inValidationMessagesMerchantNumber);
                    tbMerchant.focus();
                    tbMerchant.value = '';
                    return false;
                }

                return true;
            }
            else {
                alert(validationMIDLessThan3Chars);
                return false;
            }
        }
        return true;
    }

    function showCalendar(type) {
        if (type === '1')
            $find(uxDate_ClientID).showPopup();
        else if (type === '2')
            $find(uxFromDate_ClientID).showPopup();
        else
            $find(uxEndDate_ClientID).showPopup();
    }
    function validateData() {
        let valid = checkMerchantNumber() && checkDate();
        return valid;
    }

    return {
        changeDateOption: changeDateOption,
        keepDateOption: keepDateOption,
        showCalendar: showCalendar,
        validateData: validateData
    };
})();

let isSelectedItemInRadComboBox = false;
function radComboBox_OnClientTextChange(sender, eventArgs) {
    let comboClientID = sender._element.id;
    let combo = $find(comboClientID);
    let comboText = combo.get_text();
    if (isContainsScriptTagSpecialCharacter(comboText)) {
        alert(fieldMustNotIncludeScriptTagSpecialChars);
        combo.set_text("");
        return false;
    }    
}

function radComboBox_OnClientKeyPressing(sender, eventArgs) {
    let isIE = /MSIE/.test(navigator.userAgent);
    if (eventArgs.get_domEvent().keyCode == 13 && isSelectedItemInRadComboBox) {
        if (isIE) {
            setTimeout("uxSearchButton.click()", 200);
        }
        else {
            $get(uxSearchButton).click();
        }
        return false;
    }
}

function radComboBox_OnClientSelectedIndexChanged(sender, args) {
    if (args.get_item() != null) {
        isSelectedItemInRadComboBox = true;
    }
}

function uxPaymentEntitieTypes_OnClientSelectedIndexChanged(sender, args) {    
    showHideAcquirerMerchantControl();   
}

function showHideAcquirerMerchantControl() {
    let cbPaymentEntitieTypes = $find(uxPaymentEntitieTypes_ClientID);
    let cbPaymentEntitieTypeValue = cbPaymentEntitieTypes.get_selectedItem().get_value();
    let cbAcquirer = $('#' + uxAcquirerList_ClientID)[0];
    let cbMerchantList = $('#' + uxMerchantList_ClientID)[0];
    let tbMerchant = $('#' + uxMerchantNumber_ClientID)[0];
    if (cbPaymentEntitieTypeValue == '1') {
        cbAcquirer.style.display = 'block';
        cbMerchantList.style.display = 'none';
        tbMerchant.style.display = 'none';
    }
    else if (cbPaymentEntitieTypeValue == '2') {
        cbAcquirer.style.display = 'none';
        cbMerchantList.style.display = 'block';
        tbMerchant.style.display = 'none';
    }
    else {
        cbAcquirer.style.display = 'none';
        cbMerchantList.style.display = 'none';
        tbMerchant.style.display = 'block';
    }
}

function Callback(sender, e) {
    UxExport_OnResponseEnd(sender, e);
}

function ajaxRequestStart(sender, args) {
    var target = sender.__EVENTTARGET;
    if (target.indexOf("imgExcel") >= 0 || target.indexOf("imgCSV") >= 0) {
        args.set_enableAjax(false);
    }
}

function WhenResponseEnd(sender, eventArgs) {
    var gridID = uxReportGrid_ClientID;
    ShowHideExportControl(gridID);
}

FilteringByColumnWithDataFieldModule.setInvalidMessage('Invalid value. Please enter a valid value.');
FilteringByColumnWithDataFieldModule.extendValidators({
    IntegerGreaterThanEqualZero: function (value) {
        return !!(this.Integer(value) && value >= 0);
    }
});

$(document).ready(function () {
    merchantFraudFilter.keepDateOption();
    addCheckSpecialCharactersForDate();
    showHideAcquirerMerchantControl();
});