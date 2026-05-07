function closeMe(reload) {
    if (IsFromWindowPopup) {
        window.close();
    }
    else if (reload) {
        parent.doRebindUserList();
        parent.HidePopupModal();
    }
    else {
        parent.HidePopupModal();
    }
    return false;
}
function AllowToChangeStatus(type) {
    if (type == "Inactive") {
        if ($("#" + uxActive_ClientID).val() == "N" && $("#" + uxHddUrlParameter_ClientID).val() != "") {
            $("#" + uxActive_ClientID).attr('checked', 'checked');
            openChildModal(1, $("#" + uxHddUrlParameter_ClientID).val(), "1100", "600");
        }
    }
}

function ShowHideGroupRow(value) {
    document.getElementById("uxTrGroup").style.display = value;
    //AdjustModalSize();
}

function ShowHideRiskGroupRow(value) {
    document.getElementById("uxTrRiskGroup").style.display = value;
    AdjustModalSize();
}
function ShowHideOnwerGroup(value) {
    document.getElementById("trOwnershipGroupLable").style.display = value;
    document.getElementById("trOwnershipGroupcontrol").style.display = value;
    AdjustModalSize();
}
function openChildModal(index, url, width, height) {
    if (IsFromWindowPopup) {
        ShowPopupModal(url, width, height);
    }
    else {
        parent.ShowPopupModalChild(index, url, width, height);
    }
}


function CheckShowHideRiskGroup() {

}

//TK 36296 - Climate Control
//43401 - Error when creating multiple users on the FE
function reBindOrgs() {
    setTimeout(function () {
        var elem = document.getElementById(uxRebindOrgSelected);
        if (elem != undefined)
            document.getElementById(uxRebindOrgSelected).click();
    }, 1000);
}

function ShowHideSaleRepCodeRow(value) {
    document.getElementById("uxTrSaleRepCode").style.display = value;
    AdjustModalSize();
}
function validateEmail() {
    var objEmail = document.getElementById(uxEmail_ClientID);
    if (objEmail == null) return true;

    var reg = /^\w+([-+.']+\w+)*@\w+([-.]+\w+)*\.\w+([-.]\w+)*$/;
    if (objEmail.value != '') {
        if (reg.test(objEmail.value))
            return true;
        return false;
    }
    return true;
}

function ValidateData() {
    if (isDisabledSubmitAdd) {
        return false;
    }
    updateOwnershipValue();    
    var isValid = ValidateInput();
    if (document.getElementById("uxTrSaleRepCode").style.display != 'none') {    
        var isValidSalRep = ValidateInput1();
        isValid = isValid && isValidSalRep;        
    }

    var isValidUserControl = ValidateMasterUserControl();
    isValid = isValid && isValidUserControl;

    if (isValid) {
        $("#" + uxSave_ClientID).attr('style', 'color: #3E3E3E !important');
    }
    return isValid;
}


function ValidateMasterUserControl() {
    var isValid = true;
    var hdfValidation = document.getElementById(UMMasterUserControl_ValidationFunction);
    if (!!hdfValidation) {

        var validationFunctions = hdfValidation.value.split(',');

        for (var i = 0; i < validationFunctions.length; i++) {
            if (!!validationFunctions[i].trim()) {
                var expression = 'return ' + validationFunctions[i] + '()';
                if (isValid) {
                    isValid = (new Function(expression)());
                }
                else {
                    (new Function(expression)());
                }
            }
        }
    }
    return isValid;
}

function validateSaleRepCode() {
    var dropdownRoleValue = document.getElementById(uxRole_ClientID).value;
    var saleRepCodeValue = document.getElementById(uxSaleRepCode).value;
    if ((saleRepCodeValue == null || saleRepCodeValue.length == 0) && dropdownRoleValue == 'Sales Rep')
        return false;
    return true;

}

function setDisplayTableRowStyle() {
    $("#lbManageUserTable > tbody > tr").removeClass("AltRow");
    $("#lbManageUserTable > tbody > tr").removeClass("Row");
    var i = 0;
    $("#lbManageUserTable > tbody > tr").each(function () {
        if ($(this).css('display') != 'none') {
            i++;
            if (i % 2 > 0) {
                $(this).addClass("Row");
            }
            else {
                $(this).addClass("AltRow");
            }
        }

    });

}
$(document).ready(function () {
    setDisplayTableRowStyle();
    addCheckSpecialCharacters();
});


var isDisabledSubmitAdd = false;
function addCheckSpecialCharacters() {
    $('#' + uxUsernameId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateUsername();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    $('#' + uxUsernameSignOnId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateUsernameSignOn();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // First Name
    $('#' + uxFirstNameId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateFirstName();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // Last Name
    $('#' + uxLastNameId).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateLastName();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // Email
    $('#' + uxEmail_ClientID).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateEmail();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
    // Sale Rep Code
    $('#' + uxSaleRepCode).change(function (e) {
        if (isIncludeSpecialCharacters(this)) {
            isDisabledSubmitAdd = true;
            ValidateInput1();
            removeSpecialCharacters(this);
            setTimeout('isDisabledSubmitAdd = false;', 500);
        }
    });
}



function showViewAddOrganizationRow(mode, value) {
    document.getElementById("uxTrOrgazition").style.display = 'none';
    document.getElementById("uxTrViewOrgazition").style.display = 'none';
    switch (mode) {
        case "Add":
            {
                document.getElementById("uxTrOrgazition").style.display = value;
                break;
            }
        case "View":
            {
                document.getElementById("uxTrViewOrgazition").style.display = value;
                if ($('#' + uxSaleRepCode).val().length == 4) {
                    if (currentSaleRepCode != $('#' + uxSaleRepCode).val()) {
                        currentSaleRepCode = $('#' + uxSaleRepCode).val();
                        GetAssociatedOrg($('#' + uxSaleRepCode).val());
                    }
                }
                else if ($('#' + uxSaleRepCode).val().length == 0) {
                    $('#' + lbNumAssociatedOrg_ClientID).html('');
                }
                $('#' + uxSaleRepCode).unbind('change');
                $('#' + uxSaleRepCode).change(function () {
                    if ($('#' + uxSaleRepCode).val().length == 4) {
                        currentSaleRepCode = $('#' + uxSaleRepCode).val();
                        GetAssociatedOrg(currentSaleRepCode);
                    }
                });
                break;
            }
    }
    AdjustModalSize();
}

function getAssociatedOrgResponse(numAssociatedOrg) {
    $('#' + lbNumAssociatedOrg_ClientID).html(numAssociatedOrg + lbOrganizationAssociated);
}

function SetCheckboxIndexChanged(isChecked, isPredifineRole, isSaleRep) {
    if (isPredifineRole)
        ShowHideSaleRepCodeRow('');
    else
        ShowHideSaleRepCodeRow('none');

    if (isChecked) {
        if (isSaleRep) {
            showViewAddOrganizationRow('View', '');
        }
        else
            showViewAddOrganizationRow('Add', '');
    }
    else {
        showViewAddOrganizationRow('', '');

    }
    setDisplayTableRowStyle();
}

/*When User checked in check box, it will add item to dropdown list
If there is no checked, dropdown list will disable otherwise enable
*/
function visibleOwnershipGroupDropdown(element) {
    var _this = $(element);
    var isOwnershipChecked = 0;
    var menuItem = $find(uxOwnershipGroupDefault_ClientID);
    var item = menuItem.findItemByValue(_this.val());
    var ownershipGroup = $('#uiOwnershipGroupList').find('input');
    if (_this.is(':checked')) {
        $(item.get_element()).removeClass("hide");
        menuItem.enable();
    } else {
        $(item.get_element()).addClass("hide");
        if (menuItem.get_value() == _this.val()) {
            menuItem.clearSelection();
        }
        for (var i = 0; i < ownershipGroup.length; i++) {
            if (ownershipGroup[i].checked) {
                isOwnershipChecked++;
            }
        }
        if (isOwnershipChecked == 0) {
            menuItem.disable();
        } else {
            menuItem.enable();
        }
    }
}
/* get value ownership checked to update to db */
function updateOwnershipValue() {
    var listItem = "";
    var menuItem = $find(uxOwnershipGroupDefault_ClientID);
    for (var i = 0; i < menuItem._itemData.length; i++) {
        var item = menuItem.findItemByValue(menuItem._itemData[i].value);
        if (!$(item.get_element()).hasClass("hide")) {
            listItem += menuItem._itemData[i].value + ",";
        }
    }
    var txtOwnerShip = $("#" + txtOwnershipGroup_ClientID);
    txtOwnerShip.val(listItem.trim('').slice(0, -1));
}

/* Validate required, however if there is no ownership checked, this validate is bypass */
function validateOwnership() {
    var ownership = $find(uxOwnershipGroupDefault_ClientID);
    var emptyMess = uxOwnershipGroupDefault_EmptyMessage;
    var isShowOwnerShipGroup = document.getElementById("trOwnershipGroup").style.display != 'none';
    var ownershipGroupCheckedItem = 0;
    for (var i = 0; i < ownership._itemData.length; i++) {
        var item = ownership.findItemByValue(ownership._itemData[i].value);
        if (!$(item.get_element()).hasClass("hide")) {
            ownershipGroupCheckedItem++;
        }
    }

    if (isShowOwnerShipGroup && ownershipGroupCheckedItem > 0) {
        if (ownership.get_selectedItem() == null) {
            return false;
        }
    }
    return true;
}

function ShowHideOwnershipGroup(value, isRoleChanged) {
    document.getElementById("trOwnershipGroup").style.display = value;
    //if role is changed, ownership set to empty
    if (isRoleChanged == 'true') {
        setOwnershipGroupEmpty();
    }
}

function setOwnershipGroupEmpty() {
    var ownershipGroup = $('#uiOwnershipGroupList').find('input');
    var menuItem = $find(uxOwnershipGroupDefault_ClientID);
    for (var i = 0; i < ownershipGroup.length; i++) {
        ownershipGroup[i].checked = false;
    }

    for (var i = 0; i < menuItem._itemData.length; i++) {
        var item = menuItem.findItemByValue(menuItem._itemData[i].value);
        $(item.get_element()).addClass("hide");
    }
    menuItem.clearSelection();
    menuItem.disable();
}
function showApiPassword_Click(sender) {
    var txtShowApiPassword = $('#' + txtAPIPassword_ClientID);
    var txtHideApiPassword = $("#txtHideApiPassword");
    var divApiPassword = $("#divApiPassword");
    if (divApiPassword.hasClass("hide-pass")) {
        divApiPassword.removeClass("hide-pass");
        txtShowApiPassword.removeClass("hide");
        txtHideApiPassword.html('');
    }
    else {
        divApiPassword.addClass("hide-pass");
        txtShowApiPassword.addClass("hide");
        var numberOfPassword = txtShowApiPassword.text().length;
        for (var i = 0; i < numberOfPassword; i++) {
            txtHideApiPassword.append(".");
        }
    }
    return false;
}
function ShowHideApiPassword(isShow) {
    var txtShowApiPassword = $('#' + txtAPIPassword_ClientID);
    var txtHideApiPassword = $("#txtHideApiPassword");
    var divApiPassword = $("#divApiPassword");
    if (isShow == "True") {
        divApiPassword.addClass("hide-pass");
        txtShowApiPassword.addClass("hide");
        var numberOfPassword = txtShowApiPassword.text().length;
        for (var i = 0; i < numberOfPassword; i++) {
            txtHideApiPassword.append(".");
        }
    } else {
        txtHideApiPassword.html('');
    }
    return false;
}

function DisableClientContol() {
    
    $('a.aspNetDisabled').each(function () {
        if (!this.hasAttribute('js-ignore-disable-rule')) {
            $(this).attr('href', 'javascript:void(0);');
        } else {
            $(this).attr('style', 'cursor:pointer !important');
        }
    })
}
function onSelectedAddDecisions(sender) {  
    showHideUWApproverGroup($(sender).prop('checked'));
}

function hideUWApproverGroup() {
    showHideUWApproverGroup(false);
}

function showHideUWApproverGroup(isShow) {
    if (isShow)
        $('#' + uxApproveGroupRow).removeClass('hide');
    else {
        $('#' + uxApproveGroupRow).addClass('hide');
        $('.ck-approver-group-js:checkbox').each(function () {
            this.checked = false;
        })
    }
}