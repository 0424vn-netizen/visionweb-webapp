function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
    $('[data-hover="dropdown"]').dropdownHover();  //Re-register dropdown hover
}

function UpdateResult() {
    document.location.reload();
}
function UpdateHierachy() {
  document.getElementById(uxReloadHierachyId).click();
}


function ShowExportModal(e) {
    $(e).blur();
    return ShowPopupModal('ExportOptionModal.aspx', 'auto');
}

function ShowHideReportAccess(show) {
    if (show.toLowerCase() == 'true') {
        $('#li_report_access').show();
    }
    else {
        $('#li_report_access').hide();
    }

}

//OnClientClose
var isSubmitAddDefaultSetting = false;
function PopupModalClose(sender, eventArgs) {
    if (sender._navigateUrl == "SensitiveDataDetectedModal.aspx") {
        if (!isClientClose) {
            parent.SubmitAddNote('', false);
        }
    }
    if (sender._navigateUrl.indexOf("DefaultSettingModal.aspx") >= 0) {
        if (isSubmitAddDefaultSetting) {
            bindSourceAndRole();
            isSubmitAddDefaultSetting = false;
        }

        if (isCMSubmitAddDefaultSetting) {
            document.getElementById(uxFinishSaveDefaultSettingCH).click();
            isCMSubmitAddDefaultSetting = false;
        }
    }
}

var x, y = 0;
function masterAjax_responseEnd(sender, args) {
    //reCreateMultiSelector();
    if ($('#' + uxSourceList).length > 0 && $('#' + uxSourceList).length > 0 && $('#' + uxAddedByList)) {
        $('#' + uxSourceList).chosen('');
        $('#' + uxRoleList).chosen('');
        $('#' + uxAddedByList).chosen('');
    }

    $('[data-hover="dropdown"]').dropdownHover();

    if ($('#' + uxStatuses).length > 0 && $('#' + uxTypes).length > 0 && $('#' + uxPriorityLevel)) {
        $('#' + uxStatuses).chosen('');
        $('#' + uxTypes).chosen('');
        $('#' + uxPriorityLevel).chosen('');
    }
    if ($("#" + AddNote_uxApplyFilter).length > 0) {
        $("#" + AddNote_uxApplyFilter).prop("disabled", false);
    }

    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
    //TK:39919 - #35650
    window.scrollTo(x, y) // Fix issue auto resize content height on Chrome

    //Adjust position of Default Setting link
    if ($("#merchantnotes").find(".report-export a").css("display") == "none") {
        $("#merchantnotes").find("#defaultSetting").css("right", "20px");
    } else {
        $("#merchantnotes").find("#defaultSetting").css("right", "84px");
    }


    if ($("#casehistory").find(".report-export a").css("display") == "none") {
        $("#casehistory").find(".mn-default-link").css("right", "20px");
    } else {
        $("#casehistory").find(".mn-default-link").css("right", "84px");
    }
}

function rebindRole() {
    bindRole();
    bindUser(false);
}

var isOpenNewCase = false;
function rf_DrilldownReportFilterValuesCM(hID, hValue, hInputValue, hPreID, hPreValue, hPreInputValue) {
    isOpenNewCase = true;
    _drilldownContainer.value = hPreID + '|' + hPreValue + '|' + hPreInputValue;
    rf_SubmitReportFilterValuesCM(hID, hValue, hInputValue);
}
function rf_SubmitReportFilterValuesCM(hID, hValue, hInputValue, dOption, dFrom, dTo) {
    var elements;
    var selItem;
    var combobox;

    if (_lastSelectedItem != null) {
        //hide input control of _lastSelectedItem
        _lastSelectedItem.hierarchyObj.display_InputControl(false);
        if (_lastSelectedItem.hierarchyObj.rfProp_inputClientID != null)
            _lastSelectedItem.hierarchyObj.set_InputControlValue(_lastSelectedItem.hierarchyObj.rfProp_defaultInputValue);
    }

    // Select the RadComboBox and then all RadComboBoxItems
    combobox = $find(_idTelerikCombobox);
    elements = combobox.get_items();

    for (var i = 0; i < elements.get_count(); i++) {
        if (elements.getItem(i).get_attributes().getAttribute("hID") === hID) {
            selItem = elements.getItem(i);
            break;
        }
    }
    _selectedItem = selItem;

    rf_ActivateHierarchyDropDownOption(_selectedItem, true);


    // Select date control by select inputs which class is "date_time"
    var el = document.getElementById(_reportFilterId).getElementsByTagName("input");
    //date
    if (dOption != null) {
        elements = helperFunctions.get_elementByClassName(el, "date_item");
        for (var i = 0; i < elements.length; i++) {

            if (helperFunctions.get_valueByAttribute(elements[i].parentNode, 'mode') == dOption) {
                selItem = elements[i];
                break;
            }
        }
        selItem.checked = true;
        rf_DateOptionRadioClick(selItem);
        $find(hierarchyObj.rfProp_dtFromID).set_selectedDate(dFrom);
        if (dTo != null) $find(hierarchyObj.rfProp_dtToID).set_selectedDate(dTo);
    }
    // ready to submit
    var readyToSubmit = false;
    // Assign selected text from Grid to current RadComboboxItem's Input Control
    var hierarchyObj = _selectedItem.hierarchyObj;
    if (hierarchyObj.rfProp_inputClientID != null) {
        // console.log(hInputValue);
        _currentHierachyValue = "";
        var result = hierarchyObj.set_InputControlValue(hInputValue);
        if (!result) {

            var combobox = $find(hierarchyObj.rfProp_inputClientID);
            if (combobox != null && typeof combobox.requestItems != 'undefined' && hInputValue != '') {
                _firingEvent = true;
                _currentHierachyValue = hInputValue;
                combobox.requestItems(hInputValue, false);

            }
        }
        else {
            readyToSubmit = true;
        }
    }

    if (readyToSubmit) {
        //_submitButton.click();
    }
    return false;

}


function ComboboxHierarchyFilterOnClientItemsRequested(sender, eventArgs) {
    if (_firingEvent) {
        _firingEvent = false;
        sender.clearSelection();
        if (sender.findItemByValue(_currentHierachyValue)) {
            sender.findItemByValue(_currentHierachyValue).select();
        }
        if (typeof isOpenNewCase === 'undefined' || !isOpenNewCase) {
            setTimeout(function () {
                _submitButton.click();
            }, 100);
        }
    }
    if (_firingEventBind) {
        _firingEventBind = false;
        sender.findItemByValue(_currentHierachyValue).select();
        var t = setInterval(function () {
            document.onreadystatechange = function () {
                if (document.readyState === 'complete') {
                    var ele = document.getElementById('rf_loading');
                    if (ele != null) {
                        document.body.removeChild(ele);
                        t = null;
                    }
                }
            }();
        }, 100);
    }
}

function siteJumpClick(sender, userid) {
    var $sjCtrl = $(sender);

    //Exisiting request in queue, just ignore to prevent multiple click
    if ($sjCtrl.data("isLoading")) {
        return;
    }

    $sjCtrl.data("isLoading", true);    

    $.ajax({
        type: "POST",
        url: currentPageURL + "/GetSiteJumpUrl",
        data: JSON.stringify({ encUser: userid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var returnData = result.d;

            if (!returnData) {
                console.log("Site Jump fail.");
                return;
            }
            
            window.open(returnData);
        },
        complete: function (response) {
            $sjCtrl.data("isLoading", false);

            //process not authorize
            var statusCode = response.statusCode().status;
            if (statusCode === 401) {
                window.location.href = _LOGIN_URL;
            }
        }
    });
}