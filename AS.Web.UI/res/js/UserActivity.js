
function ValidateData() {
    if (!CheckDate()) {
        return false;
    }
    if (!Validate()) {
        return false;
    }
    if (!CheckMerchantNumber()) {
        return false;
    }
    
}
function Validate() {
    var filterRisk = $find(UserActivity_uxUserList);
    if (filterRisk.get_value() == 0) {
        alert(String.format(UserActivity_ValidationMessages_V2, UserActivity_js_RiskUser));
        filterRisk.showDropDown();
        return false;
    }
    return true;
}
function CompareToday(datePicker) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox().value;
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(UserActivity_ValidationMessages_V1);
        datePicker.get_dateInput().focus();
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(String.format(UserActivity_ReportFilter_V9, UserActivity_js_DateFilter));
        datePicker.get_dateInput().focus();
        return false;
    }
    return true;
}
function CheckDate() {
    var filterDate = $find(UserActivity_uxDate);

    if (filterDate.get_textBox().value == "") {
        alert(String.format(UserActivity_ValidationMessages_V2, UserActivity_js_DateFilter));
        filterDate.get_dateInput().focus();
        return false;
    }
    else
        if (!CompareToday(filterDate))
            return false;
    return true;
}
function CheckMerchantNumber() {
    _filterValue = document.getElementById(UserActivity_uxMerchantNumber);
    _filterValue.value = trim(_filterValue.value);
    _filterValue = document.getElementById(UserActivity_uxMerchantNumber);
    _textObj = UserActivity_js_MerchantNumber;
    if (trim(_filterValue.value) == '') {
        alert(String.format(UserActivity_ReportFilter_V2, _textObj));
        return false;
    }
    if (_filterValue.value.trim().length > 16) {
        alert(String.format(UserActivity_ReportFilter_V4, _textObj, 16));
        _filterValue.focus();
        _filterValue.select();
        return false;
    }
    var reg = /^[0-9]{1,16}$/;
    if (reg.test(_filterValue.value.trim()) == false) {
        alert(String.format(UserActivity_ReportFilter_V7, _textObj));
        _filterValue.focus();
        _filterValue.select();
        return false;
    }
    return true;
}