
function SearchTypeOnClientSelectedIndexChanged(sender, agrs) {
    var pnltxtSearchValue = $find(pnltxtSearchValue_ClientID);
    var pnlcbbSearch = $find(pnlcbbSearch_ClientID);

    if (sender.get_selectedItem().get_value() == 'All') {
        $('#' + uxtr_ClientID).addClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'none');
        $('#' + pnlcbbSearch_ClientID).css('display', 'none');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'none');
    }
    else if (sender.get_selectedItem().get_value() == 'USERTYPE') {
        $('#' + uxtr_ClientID).removeClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'none');
        $('#' + pnlcbbSearch_ClientID).css('display', 'block');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'none');
    }
    else if (sender.get_selectedItem().get_value() == 'USERROLE') {
        $('#' + uxtr_ClientID).removeClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'none');
        $('#' + pnlcbbSearch_ClientID).css('display', 'none');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'block');
    }
    else {
        $('#' + uxSearchValue_ClientID).val('');
        $('#' + uxtr_ClientID).removeClass('display-none');
        $('#' + pnltxtSearchValue_ClientID).css('display', 'block');
        $('#' + pnlcbbSearch_ClientID).css('display', 'none');
        $('#' + pnlcbbSearchRole_ClientID).css('display', 'none');
    }
}


function addCheckSpecialCharacters() {
    $('#' + uxSearchValue_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            alert(content.AlertMsgSpecialCharacters);
            removeSpecialCharacters(this);
            this.focus();
        }
    });
}

$(document).ready(function () {
    // Add event click enter from kb

    $('#' + uxFilteringTable_ClientID + ' input[type=text]').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            if (isIncludeSpecialCharacters(document.getElementById(uxSearchValue_ClientID))) {
                $(e.currentTarget).trigger('change');
                return false;
            }
            setTimeout(function () {
                $('#' + uxbtnSearch_ClientID).click();
            }, 200);
            return false;
        }
    });

    addCheckSpecialCharacters();
});

function validation() {

    var uxSearchValue = $find(uxSearchValue_ClientID);
    var uxSearchType = $find(uxSearchType_ClientID);

    if (uxSearchType.get_selectedItem().get_value() == 'USERID'
        || uxSearchType.get_selectedItem().get_value() == 'FIRSTNAME'
        || uxSearchType.get_selectedItem().get_value() == 'LASTNAME'
        || uxSearchType.get_selectedItem().get_value() == 'EMAIL') {
        if ($.trim($('#' + uxSearchValue_ClientID).val()).length == 0) {
            alert(MsgField(uxSearchType.get_selectedItem().get_value()) + ' : ' + Text_RequiredField);
            return false;
        }

        var element = document.getElementById(uxSearchValue_ClientID);
        if (isIncludeSpecialCharacters(element)) {
            alert(content.AlertMsgSpecialCharacters);
            removeSpecialCharacters(element);
            element.focus();
            return false;
        }
    }

    return true;
}

function MsgField(val) {
    switch (val) {
        case 'USERID':
            return Text_UserID;
            break;
        case 'FIRSTNAME':
            return Text_FirstName;
            break;
        case 'LASTNAME':
            return Text_LastName;
            break;
        case 'EMAIL':
            return Text_Email;
            break
    }
}