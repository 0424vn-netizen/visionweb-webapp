// can get rid of this and set proper displays/settings in code behind
setTimeout("uxFilterType_SelectedIndexChanged();", 300);

$(document).ready(function () {
    addCheckSpecialCharacters();
});

function addCheckSpecialCharacters() {
    // add check when lost focus for Date items
    $(".filter-item .riTextBox").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                alert(rm_Assignment_Management_js_ReportedDate_Invalid);
                removeSpecialCharacters(this);
            }
        })
    });
}

function SetHeaderAssGrid() {
    var arrayHeader =
            [['', 1, 'GridHeader_FirstColumn rgHeader mh'],
            [rm_Assignment_Management_js_GridHeaderMerchantCount, 3, 'rgHeader mh'],
            [rm_Assignment_Management_js_GridHeaderWorked, 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'GridHeader_LastColumn rgHeader mh']];

    if (rm_Assignment_Management_hasQueuingMechanism) {
        arrayHeader =
            [['', 1, 'GridHeader_FirstColumn rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            [rm_Assignment_Management_js_GridHeaderMerchantCount, 3, 'rgHeader mh'],
            [rm_Assignment_Management_js_GridHeaderWorked, 2, 'rgHeader mh'],
            [rm_Assignment_Management_js_GridHeaderRe_queued, 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['', 1, 'GridHeader_LastColumn rgHeader mh']];
    }

    if (!rm_Assignment_Management_EnabledByReadOnly) {
        arrayHeader = arrayHeader.splice(0, arrayHeader.length - 1);
    }


    // Set header for Assignment grid.
    addGroupHeadersForStaticRadGrid(rm_Assignment_Management_uxAssignmentGrid, arrayHeader);


    // Set header for User & Group grid.
    var uxGridIds = document.getElementById(rm_Assignment_Management_uxListOfGridId);
    if (uxGridIds == null || uxGridIds.value == "") return;
    var gridIds = uxGridIds.value.split(';');

    for (var i = 0; i < gridIds.length; i++) {
        addGroupHeadersForStaticTable(gridIds[i], arrayHeader);
    }

}


function masterAjax_responseEnd(sender, args) {
    SetHeaderAssGrid();
    //SetHeaderUserGroupGrid();
    if (typeof initAllProgressBar == 'function') {
        initAllProgressBar();
    }
}


function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function') {
        UxExporter_OnRequestStart(sender, args);

    }
}
function ajaxResponseEnd(sender, args) {
    SetHeaderAssGrid();
    if (typeof (UxExporter_OnResponseEnd) == 'function') {
        UxExporter_OnResponseEnd(sender, args);
    }
}
function refreshUIElements(flagshow) {
    if (flagshow == true) {
        document.getElementById('ctl00_ctl00_ContentPage_uxExporterPanel').style.display = 'block';
        document.getElementById('ctl00_ctl00_ContentPage_uxAssignmentGridPanel').style.display = 'block';
    }
    else {
        document.getElementById('ctl00_ctl00_ContentPage_uxExporterPanel').style.display = 'none';
        document.getElementById('ctl00_ctl00_ContentPage_uxAssignmentGridPanel').style.display = 'none';
    }
}
function entersubmit(e) {
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);
    var keyCode = isIE ? e.keyCode : e.which;
    isEnter = (keyCode == 13);
    if (isEnter) {
        return false;
    }
    return true;
}

var needToFireEvent = true;
var FilterTypes = { All: 0, User: 1, Group: 2, Assignment: 3 };
var FilterGroupTypes = { None: 0, DropDown: 1, Text: 2 };
var FilterTypeList = [
        { Type: FilterTypes.All },
        { Type: FilterTypes.User, PanelID: rm_Assignment_Management_uxFilterValuePanelUser },
        { Type: FilterTypes.Group, PanelID: rm_Assignment_Management_uxFilterValuePanelGroup },
        { Type: FilterTypes.Assignment, PanelID: rm_Assignment_Management_uxFilterValuePanelAssignment }
];
var filterTypeValue = 0;

function uxFilterType_Checked(isAuto) {
    for (var idx = 0; idx < FilterTypeList.length; idx++) {
        var filterType = FilterTypeList[idx];

        if (filterType.PanelID != null) {
            $get(filterType.PanelID).style.display = "none";
        }
    }


    filterTypeValue = 0;

    for (var idx = 0; idx < FilterTypeList.length; idx++) {
        var filterType = FilterTypeList[idx];

        if (filterType.Type == isAuto) {
            filterTypeValue = filterType.Type;

            if (filterType.PanelID != null) {
                $get(filterType.PanelID).style.display = "inline-block";
            }

            //if (isAuto != true) {
            //}
            break;
        }
    }
}

function ValidateFilters() {
    var selectedFilterTypeValue = parseInt($find(rm_Assignment_Management_uxFilterType).get_value(), 10);

    if (selectedFilterTypeValue == FilterTypes.User && $find(rm_Assignment_Management_uxComboUser).get_value().length == 0) {
        alert(rm_Assignment_Management_js_UsermustSelect);
        $find(rm_Assignment_Management_uxComboUser).showDropDown();
        return false;
    }
    else if (selectedFilterTypeValue == FilterTypes.Group && $find(rm_Assignment_Management_uxComboGroup).get_value().length == 0) {
        alert(rm_Assignment_Management_js_GroupmustSelect);
        $find(rm_Assignment_Management_uxComboGroup).showDropDown();
        return false;
    }
    else if (selectedFilterTypeValue == FilterTypes.Assignment && $find(rm_Assignment_Management_uxComboAssignment).get_value().length == 0) {
        alert(rm_Assignment_Management_js_AssignmentmustSelect);
        $find(rm_Assignment_Management_uxComboAssignment).showDropDown();
        return false;
    }

    // Validate Date
    var datePickerValue = trim($find(rm_Assignment_Management_uxReportDate).get_textBox().value);
    var currentDate = new Date();

    //Check Required
    if (datePickerValue == "") {
        alert(rm_Assignment_Management_js_ReportedDate_Invalid);
        return false;
    }

    //Check Invalid
    var isValid = isDate(datePickerValue);
    if (!isValid) {
        alert(rm_Assignment_Management_js_ReportedDate_Invalid);
        return false;
    }

    //Check greater than Current Date 
    var selectedDate = new Date(datePickerValue);
    if (selectedDate > currentDate) {
        alert(rm_Assignment_Management_js_ReportedDate_GreaterToday);
        return false;
    }

    SaveFilterValues();
    document.getElementById(rm_Assignment_Management_uxProxyButton).click();
    return false;
}

function SaveFilterValues() {
    var selectedFilterTypeValue = parseInt($find(rm_Assignment_Management_uxFilterType).get_value(), 10);
    var filterTypeMode = "";
    var filterTypeValue = "";
    var GroupBy = "";
    if (selectedFilterTypeValue == FilterTypes.All) {
        filterTypeMode = "All";
        filterTypeValue = "";
    }
    else if (selectedFilterTypeValue == FilterTypes.User) {
        filterTypeMode = "User";
        filterTypeValue = $find(rm_Assignment_Management_uxComboUser).get_value();
    }
    else if (selectedFilterTypeValue == FilterTypes.Group) {
        filterTypeMode = "Group";
        filterTypeValue = $find(rm_Assignment_Management_uxComboGroup).get_value();
    }
    else if (selectedFilterTypeValue == FilterTypes.Assignment) {
        filterTypeMode = "Assignment";
        filterTypeValue = $find(rm_Assignment_Management_uxComboAssignment).get_value();
    }
    GroupBy = $find(rm_Assignment_Management_uxGroupBy).get_value();

    var date = $find(rm_Assignment_Management_uxReportDate).get_dateInput().get_value();

    document.getElementById(rm_Assignment_Management_hddFilter).value = filterTypeMode + "|" + filterTypeValue + "|" + GroupBy + "|" + date;
}

function uxDateFilter_SelectedChange() {
    var uxFilterType = $find(rm_Assignment_Management_uxFilterType);
    if (uxFilterType != null) {
        var selectedFilterTypeValue = parseInt(uxFilterType.get_value(), 10);
        uxFilterType_Checked(selectedFilterTypeValue);
    }
    return true;
}


function uxFilterType_SelectedIndexChanged(sender, eventArgs) {
    var uxFilterType = $find(rm_Assignment_Management_uxFilterType);
    if (uxFilterType != null) {
        var selectedFilterTypeValue = parseInt(uxFilterType.get_value(), 10);
        uxFilterType_Checked(selectedFilterTypeValue);
    }
    return true;
}

function masterAjax_responseEnd(sender, args) {
    uxDateFilter_SelectedChange();
    uxFilterType_SelectedIndexChanged();
}

function uxGroupBy_SelectedIndexChanged(sender, eventArgs) {
    var item = eventArgs.get_item();
    document.getElementById("ctl00_ContentPage_uxComboGroup_Input").value = item.get_text();
    $get(rm_Assignment_Management_hf1).value = item.get_text();
}

function uxAssignmentList_SelectedIndexChanged(sender, eventArgs) {
    var item = eventArgs.get_item();
    document.getElementById("ctl00_ContentPage_uxComboAssignment_Input").value = item.get_text();
    $get(rm_Assignment_Management_hf2).value = item.get_text();
}




function OpenPicker() {
    needToFireEvent = true;
}

//function doNextClick() {
//    var txtPage = $get("ctl00_ContentPage_uxUserGroupGrid_ctl00_ctl03_ctl01_uxPageNum");
//    var btnNext = txtPage.nextSibling.nextSibling.nextSibling;
//    if (btnNext != null)
//        btnNext.click();
//    return true;
//}

function textBox_Blur(sender, eventArgs) {
    if (sender.get_textBoxValue() == "") {
        sender.focus();
        sender.set_value(1);
    }
}

function textBox_KeyPress(sender, eventArgs) {
    ///<summary>
    ///Disable Enter key on radnumeric textbox
    ///</summary>
    if (eventArgs.get_keyCode() == 13) {
        eventArgs.set_cancel(true);
    }
}


var uxBtnSubmit = document.getElementById(rm_Assignment_Management_uxSubmit);
function RefreshContent() {
    var isIE = /MSIE/.test(navigator.userAgent);

    if (isIE)
        setTimeout("uxBtnSubmit.click()", 150);
    else {
        document.getElementById(rm_Assignment_Management_uxSubmit).focus();
        document.getElementById(rm_Assignment_Management_uxSubmit).click();
    }
}

function GetAssignementID(obj) {
    document.getElementById(rm_Assignment_Management_hddAssignmentID).value = obj.id;
}

function RebindAndShowStausWhenCloseModal() {
    HidePopupModal();
    document.getElementById(rm_Assignment_Management_btnRebind).click();
}

var assignmentDeleteID = 0;
function DeleteAssignment(assignmentID) {
    assignmentDeleteID = assignmentID;
    checkDeleteAvailable(assignmentID);
    return false;

}

function checkDeleteAvailableSuccess(response) {
    if (response.length > 0) {
        alert(response);
    } else {
        if (confirm(rm_Assignment_Management_js_DeleteAssignment)) {
            document.getElementById(rm_Assignment_Management_hddAssignmentID).value = assignmentDeleteID;
            document.getElementById(rm_Assignment_Management_btnDeleteAssignment).click();
            assignmentDeleteID = 0;
            return true;
        }
        else
            return false;
    }
}


function ExtendAssignment(sender, eventArgs) {
    if (!needToFireEvent)
        return false;

    var today = new Date(rm_Assignment_Management_DateTimeToNow);
    
    var selectedDate = new Date(eventArgs.get_newValue());
    selectedDate = new Date(selectedDate.toDateString());

    if (selectedDate < today) {
        alert(rm_Assignment_Management_js_ExpirationDate);
        var oldDate = eventArgs.get_oldDate();
        //var oldValue = eventArgs.get_oldValue();
        //sender.get_dateInput().set_value(oldValue);
        needToFireEvent = false;
        sender.set_selectedDate(oldDate);
        //                    if (oldDate >= today)
        //                        sender.get_dateInput().set_selectedDate(oldDate);
        //                    else
        //                        sender.get_dateInput().set_selectedDate(
        eventArgs.set_cancel(true);
        return false;
    }

    document.getElementById(rm_Assignment_Management_hddExtendDate).value = eventArgs.get_newValue();
    document.getElementById(rm_Assignment_Management_btnExtendAssignment).click();
}





var currentTextBox = null;
var currentDatePicker = null;

//This method is called to handle the onclick and onfocus client side events for the texbox
function showPopup(sender, e) {
    GetAssignementID(sender.parentNode);
    //this is a reference to the texbox which raised the event
    //see the methods exposed through the $telerik static client library here - http://www.telerik.com/help/aspnet-ajax/telerik-static-client-library.html 

    currentTextBox = sender.tagName == "INPUT" ? sender : $telerik.getPreviousHtmlNode(sender);

    //this gets a reference to the datepicker, which will be shown, to facilitate
    //the selection of a date
    var datePicker = $find(rm_Assignment_Management_uxSharedDatePicker);

    //this variable is used to store a reference to the date picker, which is currently 
    //active
    currentDatePicker = datePicker;
    var today = new Date();
    var inputDate = datePicker.get_dateInput().parseDate(currentTextBox.value);

    datePicker.set_minDate(inputDate > today ? today : inputDate);
    needToFireEvent = false;
    //this method first parses the date, that the user entered or selected, and then
    //sets it as a selected date to the picker
    datePicker.set_selectedDate(currentDatePicker.get_dateInput().parseDate(currentTextBox.value));
    needToFireEvent = true;
    //the code lines below show the calendar, which is used to select a date. The showPopup
    //function takes three arguments - the x and y coordinates where to show the calendar, as 
    //well as its height, derived from the offsetHeight property of the textbox
    var position = $(currentTextBox).offset();
    datePicker.showPopup(position.left, position.top + currentTextBox.offsetHeight);
}

//this handler is used to set the text of the TextBox to the value of selected from the popup 
function dateSelected(sender, args) {
    if (currentTextBox != null) {
        //currentTextBox is the currently selected TextBox. Its value is set to the newly selected
        //value of the picker
        currentTextBox.value = args.get_newValue();
    }
}

//this function is used to parse the date entered or selected by the user
function parseDate(sender, e) {
    if (currentDatePicker != null) {
        var date = currentDatePicker.get_dateInput().parseDate(sender.value);
        var dateInput = currentDatePicker.get_dateInput();

        if (date == null) {
            date = currentDatePicker.get_selectedDate();
        }

        var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());
        sender.value = formattedDate;
    }
}