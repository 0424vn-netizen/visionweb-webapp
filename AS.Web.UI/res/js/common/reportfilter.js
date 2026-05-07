function rf_Textbox_DoValidateInput(control_input_client_id){
    var obj = document.getElementById(control_input_client_id);
    return rf_textboxCoreValidate(control_input_client_id, obj.getAttribute('maxLength'), obj.getAttribute('minLength'), obj.getAttribute('only'), obj.getAttribute('except'), obj.getAttribute('format'), obj.getAttribute('maxMsg'), obj.getAttribute('minMsg'), obj.getAttribute('invalidMsg'), obj.getAttribute('invalidFormat'));
}

var isCancelEnterPressMPSReportFilter = false;
function addCheckSpecialCharactersMPSReportFilter() {
    $(".filter-item .rf_TextBox, .filter-right .rf_TextBox").each(function () {
        $(this).change(function (e) {
            isCancelEnterPressMPSReportFilter = false;
            if (isIncludeSpecialCharacters(this)) {
                isCancelEnterPressMPSReportFilter = true;
                var invalidMsg = "";
                var minLength = $(this).attr("minlength");
                if (minLength != undefined && this.value.length < minLength) {
                    invalidMsg = $(this).attr("minmsg");
                } else {
                    invalidMsg = $(this).attr("invalidmsg");
                }

                if (invalidMsg == undefined || invalidMsg.trim() == "") {
                    invalidMsg = $(this).attr("invalidformat");
                }

                if (invalidMsg != undefined && invalidMsg.trim() != "") {
                    alert(invalidMsg);
                    removeSpecialCharacters(this);
                    this.focus();
                    setTimeout('isCancelEnterPressMPSReportFilter = false;', 100);
                }
            }
        });
        $(this).keypress(function (e) {
            var isIE = /MSIE/.test(navigator.userAgent);
            var keyCode = isIE ? e.keyCode : e.which;
            if (keyCode == 13) {
                if (isIncludeSpecialCharacters(this) || isCancelEnterPressMPSReportFilter) {
                    $(".filter-left .rcbInputCell input, .filter-item .rcbInputCell input")[0].focus();
                    e.preventDefault();
                    return false;
                }
            }
        });
    });
    // handle for Combobox Items
    $(".filter-item .rcbInput, .filter-right .rcbInput").each(function () {
        $(this).change(function (e) {
            if (isIncludeSpecialCharacters(this)) {
                var newValue = removeSpecialCharacters(this);
                var radComboboxId = this.id.replace('_Input', '');
                var radCombobox = $find(radComboboxId);
                radCombobox.set_text(newValue);

            }
        });
    });
}

function rf_ValidateDateTime_Ext(mode, fromDateId, toDateId) {
    //Input Mode = Daily, Monthly, DateRange
    //fromDateId = id from date picker
    //toDateid = id to date picker
    var fromDate = $find(fromDateId).get_selectedDate();
    var toDate = $find(toDateId).get_selectedDate();
    var Msg_Date_Invalid = AS_ReportFilterJS_Msg_Date_Invalid;
    var Msg_Date_MoreThanToday = AS_ReportFilterJS_Msg_Date_MoreThanToday;
    var Msg_Date_FromThanTo = AS_ReportFilterJS_Msg_Date_FromThanTo;
    var Mode_Daily = 'Daily'
        , Mode_Monthly = 'Monthly'
        , Mode_DateRange = 'DateRange'

    //check valid date
    var Msg_TheDateFormat_Text = AS_ReportFilterJS_Msg_TheDateFormat;

    if (mode == Mode_DateRange) {
        if (fromDate == null) {
            alert(String.format(Msg_Date_Invalid, Msg_TheDateFormat_Text));
            return false;
        }
        if (toDate == null) {
            alert(String.format(Msg_Date_Invalid, Msg_TheDateFormat_Text));
            return false;
        }
    }
    if (mode == Mode_Daily || mode == Mode_Monthly) {
        if (fromDate == null) {
            alert(String.format(Msg_Date_Invalid, Msg_TheDateFormat_Text));
            return false;
        }
    }


    //check dateFrom,dateTo > Today
    var today = new Date();
    if (mode == Mode_DateRange) {
        if (fromDate > today) {
            alert(String.format(Msg_Date_MoreThanToday, AS_ReportFilterJS_Msg_TheDate));
            return false;
        }
        if (toDate > today) {
            alert(String.format(Msg_Date_MoreThanToday, AS_ReportFilterJS_Msg_TheDate));
            return false;
        }
    }
    if (mode == Mode_Daily || mode == Mode_Monthly) {
        if (fromDate > today) {
            alert(String.format(Msg_Date_MoreThanToday, AS_ReportFilterJS_Msg_TheDate));
            return false;
        }
    }

    //check dateFrom > dateTo
    if (mode == Mode_DateRange) {
        if (fromDate > toDate) {
            alert(String.format(Msg_Date_FromThanTo, AS_ReportFilterJS_Msg_TheEndDate, AS_ReportFilterJS_Msg_TheBeginDate));
            return false;
        }
    }
    return true;


}