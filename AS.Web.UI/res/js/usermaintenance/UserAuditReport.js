
function ChangeDateOption(chk) {
    var now = new Date();
    var dateOpt = document.getElementById("divDate");
    var dateRangeOpt = document.getElementById("divDateRange");
    if (chk.value == 'uxRange') {
        dateOpt.style.display = "none";
        dateRangeOpt.style.display = "inline";

        var picker1 = $find(uxFromDate_ClientID);
        var picker2 = $find(uxEndDate_ClientID);


        picker2.set_selectedDate(now);
        now.setDate(1);
        picker1.set_selectedDate(now);
    }
    else {

        dateOpt.style.display = "inline";
        dateRangeOpt.style.display = "none";
        var picker = $find(uxDate_ClientID);
        //if (picker.get_selectedDate() == null) {
        //now.setDate(1);
        picker.set_selectedDate(now);
        //}
    }
} function KeepDateOption() {
    var rangeDate = document.getElementById(uxRange_ClientID);
    if (rangeDate.checked) {
        ChangeDateOption(rangeDate);
    }
}
try {
    KeepDateOption();
}
catch (ex) { }

function CompareToday(datePicker, type) {
    var currentDate = new Date();
    var datePickerValue = datePicker.get_textBox(uxDate_ClientID).value;
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(Msg_V1);
        return false;
    }
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        var object;
        switch (type) {
            case 0: object = uxDailyResource1_text; break;
            case 1: object = uxMonthlyResource1_text; break;
            case 2: object = uxRangeResource1_text; break;
        }
        alert(String.format(Msg_V9, object));
        return false;
    }    

    return true;
}

function CheckDate() {
    var beginDate = document.getElementById(uxFromDate_ClientID);
    var endDate = document.getElementById(uxEndDate_ClientID);

    var fromDate = $find(uxFromDate_ClientID);
    var toDate = $find(uxEndDate_ClientID);
    var uxDate = $find(uxDate_ClientID);

    //RadioButton           

    beginDate.value = fromDate.get_textBox().value;
    endDate.value = toDate.get_textBox().value;

    if (uxDaily && uxDaily.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 0))
                return false;
        }
        else {
            alert(uxDailyResource1_text+  ': ' + Msg_V2);
            return false;
        }
    }
    else if (uxMonthly && uxMonthly.checked) {
        if (uxDate.get_textBox().value != "") {
            if (!CompareToday(uxDate, 1))
                return false;
        }
        else {
            alert(uxMonthlyResource1_text +': ' + Msg_V2);
            return false;
        }
    }
    else if (uxDateRange && uxDateRange.checked) {
        if (fromDate.get_textBox().value == "" || toDate.get_textBox().value == "") {
            alert(uxRangeResource1_text+': ' + Msg_V2);
            return false;
        }
        if (!CompareToday(fromDate, 2))
            return false;
        if (!CompareToday(toDate, 2))
            return false;

        if (fromDate.get_selectedDate() > toDate.get_selectedDate()) {
            alert(Msg_V3);
            return false;
        }
    }
    return true;
}
//Show calendar when user click on date text


function showCalendar(type) {
    if (type == '1')
        $find(uxDate_ClientID).showPopup();
    else if (type == '2')
        $find(uxFromDate_ClientID).showPopup();
    else
        $find(uxEndDate_ClientID).showPopup();
}
function SearchEnterOnTextbox(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        var uxSearchButton = document.getElementById(uxSearch_ClientID);
        uxSearchButton.focus();
        if (isIE)
            setTimeout("uxSearchButton.click()", 200);
        else
            uxSearchButton.click();
        return false;
    }
}
function ValidateData() {
    if (isCancelSubmit) {
        return false;
    }
    if (!ValidateInput())
        return false;
    if (CheckDate())
        return true;
    return false;
}

function OnClientKeyPressing(sender, args) {
    var uxAdministratorName = $find(uxAdministratorName_ClientID);
    uxAdministratorName.showDropDown();
}
function validateSpecialCharacterChangedEntity() {
    var textChangedEntity = $get(uxChangedEntity_ClientID).value;
    return checkSpecialCharacter(textChangedEntity);
}
function validateSpecialCharacterChangedBy() {
    var textChangedBy = $get(uxAdministratorName_ClientID).value;
    return checkSpecialCharacter(textChangedBy);
}

$(document).ready(function () {
    addCheckSpecialCharactersForDate();
    addCheckSpecialCharacters();
});

var isCancelSubmit = false;
function addCheckSpecialCharacters() {
    $("#" + uxChangedEntity_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isCancelSubmit = true;
            ValidateInput();
            removeSpecialCharacters(this);
            setTimeout('isCancelSubmit = false;', 500);
            this.focus();
        }
    });

    $("#" + uxAdministratorName_ClientID + "_Input").change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            ValidateInput();
            var newValue = removeSpecialCharacters(this);
            var radCombobox = $find(uxAdministratorName_ClientID);
            radCombobox.set_text(newValue);
            this.focus();
        }
    });
}