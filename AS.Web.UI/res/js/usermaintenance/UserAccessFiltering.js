
function DateoptionChange() {
    var uxtadDaily = $get(uxtadDaily_ClientID);
    var uxradMonthly = $get(uxradMonthly_ClientID);
    var uxradDateRange = $get(uxradDateRange_ClientID);
    var uxDateFrom = $find(uxDateFrom_ClientID);
    var uxDateTo = $find(uxDateTo_ClientID);

    if (uxradDateRange.checked) {
        uxDateFrom.set_selectedDate(new Date(FromDate));
        uxDateTo.set_selectedDate(new Date());
        $('#' + pnlDateRangecontrolsfrom_ClientID).css('visibility', 'visible');
        $('#' + pnlDateRangecontrolsto_ClientID).css('visibility', 'visible');
    }
    else {
        uxDateFrom.set_selectedDate(new Date());
        $('#' + pnlDateRangecontrolsfrom_ClientID).css('visibility', 'visible');
        $('#' + pnlDateRangecontrolsto_ClientID).css('visibility', 'hidden');
    }
}

function setUserTypeValue(val) {
    if (val != "0") {
        $('#' + pnlFilterVal_ClientID).css('display', 'block');
        $('#' + uxFilterVal_ClientID).val(default_Text);
    }
    else {
        $('#' + pnlFilterVal_ClientID).css('display', 'none');
    }
}

function SearchValueOnClientSelectedIndexChanged(sender, agrs) {
    setUserTypeValue(sender.get_selectedItem().get_value());
}

function SearchTypeOnClientSelectedIndexChanged(sender, agrs) {
    var pnltxtSearchValue = $find(pnltxtSearchValue_ClientID);
    var pnlcbbSearch = $find(pnlcbbSearch_ClientID);

    $('#' + uxSearchValue_ClientID).val('');
    $('#' + uxFilterVal_ClientID).val('');

    if (sender.get_selectedItem().get_value() == 'All') {
        $('#' + uxtr_ClientID).addClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'none');
        $('#' + pnlcbbSearch_ClientID).css('display', 'none');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'none');
        $('#' + pnlFilterVal_ClientID).css('display', 'none');
    }
    else if (sender.get_selectedItem().get_value() == 'USERTYPE') {
        $('#' + uxtr_ClientID).removeClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'none');
        $('#' + pnlcbbSearch_ClientID).css('display', 'block');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'none');
        setUserTypeValue($find(uxcbbSearchValue_ClientID).get_selectedItem().get_value());
    }
    else if (sender.get_selectedItem().get_value() == 'USERROLE') {
        $('#' + uxtr_ClientID).removeClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'none');
        $('#' + pnlcbbSearch_ClientID).css('display', 'none');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'block');
        $('#' + pnlFilterVal_ClientID).css('display', 'none');
    }
    else {
        $('#' + uxtr_ClientID).removeClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'block');
        $('#' + pnlcbbSearch_ClientID).css('display', 'none');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'none');
        $('#' + pnlFilterVal_ClientID).css('display', 'none');
    }
}

$(document).ready(function () {
    var uxradDateRange = $get(uxradDateRange_ClientID);
    if (uxradDateRange.checked) {
        $('#' + pnlDateRangecontrolsfrom_ClientID).css('visibility', 'visible');
        $('#' + pnlDateRangecontrolsto_ClientID).css('visibility', 'visible');
    }
    else {
        $('#' + pnlDateRangecontrolsfrom_ClientID).css('visibility', 'visible');
        $('#' + pnlDateRangecontrolsto_ClientID).css('visibility', 'hidden');
    }
    // Add event click enter from kb

    $('#' + uxFilteringTable_ClientID + ' input[type=text]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            if (isCancelEnterPress) {
                return false;
            }
            if (isIncludeSpecialCharacters(document.getElementById(uxFilterVal_ClientID)) || isIncludeSpecialCharacters(document.getElementById(uxSearchValue_ClientID))) {
                $(e.currentTarget).trigger('change');
                return false;
            }
            setTimeout(function () {
                $('#' + uxbtnSearch_ClientID).click();
            }, 200);
            return false;
        }
    });

    // Add check and remove special character when leave input item
    addCheckSpecialCharactersForDate();
    addCheckSpecialCharacters();
});

function addCheckSpecialCharacters() {
    $('#' + uxFilterVal_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            alert(MsgField('USERID') + ' ' + msgNotAllowSpecialCharacter);
            removeSpecialCharacters(this);
            this.focus();
        }
    });
    $('#' + uxSearchValue_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            var uxSearchField = $find(uxSearchType_ClientID).get_selectedItem().get_value();
            alert(MsgField(uxSearchField) + ' ' + msgNotAllowSpecialCharacter);
            removeSpecialCharacters(this);
            this.focus();
        }
    });
}

function validation() {
    var uxradDateRange = $get(uxradDateRange_ClientID);
    var uxDateFrom = $find(uxDateFrom_ClientID);
    var uxDateTo = $find(uxDateTo_ClientID);
    var uxSearchType = $find(uxSearchType_ClientID);

    if (uxradDateRange.checked) {
        if (uxDateFrom.get_selectedDate() == null) {
            alert(UserAccessFiltering_ascx_js_InvalidDate);
            return false;
        }
        else if (uxDateTo.get_selectedDate() == null) {
            alert(UserAccessFiltering_ascx_js_InvalidDate);
            return false;
        }
        else if (uxDateFrom.get_selectedDate() > uxDateTo.get_selectedDate()) {
            alert(UserAccessFiltering_ascx_js_GreaterThan);
            return false;
        }

        else if (new Date(uxDateTo.get_selectedDate()) > new Date()) {
            alert(UserAccessFiltering_ascx_js_GreaterThanToday);
            return false;
        }

    }
    else {
        if (uxDateFrom.get_selectedDate() == null) {
            alert(UserAccessFiltering_ascx_js_InvalidDate);
            return false;
        }
        else if (new Date(uxDateFrom.get_selectedDate()) > new Date()) {
            alert(UserAccessFiltering_ascx_js_NotGreaterThanToday);
            return false;
        }
    }   

    if (uxSearchType.get_selectedItem().get_value() == 'USERID' || uxSearchType.get_selectedItem().get_value() == 'FIRSTNAME' || uxSearchType.get_selectedItem().get_value() == 'LASTNAME' || uxSearchType.get_selectedItem().get_value() == 'EMAIL') {
        if ($.trim($('#' + uxSearchValue_ClientID).val()).length == 0) {
            alert(MsgField(uxSearchType.get_selectedItem().get_value()) + ' : ' + UserAccessFiltering_ascx_js_RequriedField);
            return false;
        }
    }

    var uxFilterVal = $('#' + uxFilterVal_ClientID).val();
    if (uxFilterVal.length > 0) {
        if (!checkSpecialCharacter(uxFilterVal)) {
            alert(MsgField('USERID') + ' ' + msgNotAllowSpecialCharacter);
            return false;
        }
    }

    var vlSearchTxt = $('#' + uxSearchValue_ClientID).val();
    if (vlSearchTxt.length > 0) {
        if (!checkSpecialCharacter(vlSearchTxt)) {
            alert(MsgField(uxSearchType.get_selectedItem().get_value()) + ' ' + msgNotAllowSpecialCharacter);
            return false;

        }
    }

    return true;
}

function MsgField(val) {
    switch (val) {
        case 'USERID':
            return RadComboBoxItemUserID_Text;
            break;
        case 'FIRSTNAME':
            return RadComboBoxItemFirstName_Text;
            break;
        case 'LASTNAME':
            return RadComboBoxItemLastName_Text;
            break;
        case 'EMAIL':
            return UserAccessFiltering_ascx_js_Email;
            break
    }
}