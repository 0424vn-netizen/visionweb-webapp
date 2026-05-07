function moveStep(step) {
    for (var i = 1; i <= 5; i++) {
        if (i == step) {
            document.getElementById('Step' + i).style.display = '';
        }
        else {
            document.getElementById('Step' + i).style.display = 'none';
        }
    }
    doCheckUserMenu();
    onShowHideAccessFuncCheckBox();
    AdjustModalSize();
    return false;
}

function closeMe() {
    return parent.HidePopupModal();
}

// Step 1:
function checkRoleName(clientID) {
    var regex = new RegExp("^[ -_A-Za-z0-9]*$");
    var value = document.getElementById(clientID).value;
    if (regex.test(value)) {
        return true;
    } else {
        return false;
    }
}
// Step 2: Check Hierarchy Level
function checkHierarchyLevel() {
    if (document.getElementById(uxHierarchyLevel).value != HL_SELECT_HIERARCHY_VALUE) {
        var combo = $find(uxHierarchyLevel);
        var hierarchy = "," + combo.get_selectedItem().get_value() + ",";
        if (DisableSecondaryUserRoleByEntities.search(hierarchy) >= 0) {
            document.getElementById(uxRadSecondaryUser).checked = false;
            document.getElementById(uxRadSecondaryUser).disabled = true;
        }
        else {
            if (!IsEdit) {
                if (DisableSecondaryUserByClient == 'True') {
                    document.getElementById(uxRadSecondaryUser).disabled = true;
                } else {
                    document.getElementById(uxRadSecondaryUser).disabled = false;
                }
            }
        }
        return moveStep(3);
    }
    else {
        alert(Text_RequiredField);
        return false;
    }
}

// Step 3: Check Hierarchy User Type
function validateUserType() {
    if (document.getElementById(uxRadPrimaryUser).checked
        || document.getElementById(uxRadSecondaryUser).checked) {
        document.getElementById(uxLoadMenuAndHierarchy).click();
        return false;
    }
    else {
        alert(Text_RequiredUserType);
        return false;
    }
}

// Step 4: Menu & Access functions
function checkAllCheckbox(containerId, un_checked) {
    var container = document.getElementById(containerId);
    if (container == null) return;
    var treeViewInstances = container.getElementsByTagName('div');
    var hasNode1099k = false;
    var hasNodePci = false;
    for (var i = 0; i < treeViewInstances.length; i++) {
        if (treeViewInstances[i].control != null && treeViewInstances[i].control) {
            var nodes = treeViewInstances[i].control.get_allNodes();

            for (var j = 0; j < nodes.length; j++) {
                if (un_checked) {
                    nodes[j].uncheck();
                    if (nodes[j].get_value() == SEC_PERMISSION_SITE_JUMP_1099K) {
                        hasNode1099k = true;
                        document.getElementById(uxhdRole1099).value = "false";
                    }

                    if (nodes[j].get_value() == SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI) {
                        hasNodePci = true;
                        document.getElementById(uxhdRolePCI).value = "false";
                    }
                }
                else {
                    nodes[j].check();
                    if (nodes[j].get_value() == SEC_PERMISSION_SITE_JUMP_1099K) {
                        hasNode1099k = true;
                        document.getElementById(uxhdRole1099).value = "true";
                    }

                    if (nodes[j].get_value() == SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI) {
                        hasNodePci = true;
                        document.getElementById(uxhdRolePCI).value = "true";
                    }
                }
            }
        }
    }

    doCheckUserMenu();

    if (hasNode1099k)
        onCheckPermission1099K();
    //if (hasNode1099k && hasNodePci) {
    //    document.getElementById(uxCheckAllMenu).click();
    //}
    //else {
    //    if (hasNode1099k)
    //        onCheckPermission1099K();
    //    if (hasNodePci)
    //        onCheckPermissionPCI();
    //}
    onCheckPermissionBoarding();
    onCheckPermissionLanding(false);
    onShowHideAccessFuncCheckBox();
}

var tree_UM_nodes = null;
function doCheckUserMenu() {
    if (tree_UM_nodes == null) {
        var tree_UM = document.getElementById(uxTreeMenuCS);
        if (tree_UM.control != null) {
            tree_UM_nodes = tree_UM.control.get_allNodes();
        }
    }
    var showNextStep = false;
    var nodeValue = '';
    for (var j = 0; j < tree_UM_nodes.length; j++) {
        nodeValue = tree_UM_nodes[j].get_value();
        if ((nodeValue == SEC_PERMISSION_MAN_USER_MS)
            && tree_UM_nodes[j].get_checked()) {
            showNextStep = true;
            break;
        }
    }

    if (showNextStep) {
        document.getElementById('cidDivContinueStep').style.display = '';
        document.getElementById('cidDivSave').style.display = 'none';
    }
    else {
        document.getElementById('cidDivContinueStep').style.display = 'none';
        document.getElementById('cidDivSave').style.display = '';
    }
}

function onCheckPermission1099K() {
    if (tree_UM_nodes == null) {
        var tree_UM = document.getElementById(uxTreeMenuCS);
        tree_UM_nodes = tree_UM.control.get_allNodes();
    }
    var nodeValue = '';
    for (var j = 0; j < tree_UM_nodes.length; j++) {
        nodeValue = tree_UM_nodes[j].get_value();

        if (nodeValue == SEC_PERMISSION_SITE_JUMP_1099K) {
            if (tree_UM_nodes[j].get_checked()) {
                document.getElementById(uxhdRole1099).value = "true";
            }
            else {
                document.getElementById(uxhdRole1099).value = "false";
            }
            document.getElementById(uxbtnCheckRole1099).click();
            break;
        }
    }
}

//function onCheckPermissionPCI() {
//    if (tree_UM_nodes == null) {
//        var tree_UM = document.getElementById(uxTreeMenuCS);
//        tree_UM_nodes = tree_UM.control.get_allNodes();
//    }
//    var nodeValue = '';
//    for (var j = 0; j < tree_UM_nodes.length; j++) {
//        nodeValue = tree_UM_nodes[j].get_value();

//        if (nodeValue == SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI) {
//            if (tree_UM_nodes[j].get_checked()) {
//                document.getElementById(uxhdRolePCI).value = "true";
//            }
//            else {
//                document.getElementById(uxhdRolePCI).value = "false";
//            }
//            document.getElementById(uxbtnCheckPCI).click();
//            break;
//        }
//    }
//}

function onCSMenuNodeChecked(sender, args) {
    doCheckUserMenu();
    if (args.get_node().get_value() == SEC_PERMISSION_SITE_JUMP_1099K) {
        onCheckPermission1099K();
    }

    //if (args.get_node().get_value() == SEC_PERMISSION_HIERARCHY_SITE_ACCESS_PCI) {
    //    onCheckPermissionPCI();
    //}

    onCheckPermissionBoarding();
    //TK39894
    var nodes = args._node._children._array;
    var ckText = "";
    if (nodes.length > 0) {
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i]._itemData != undefined && nodes[i]._itemData.length > 0) {
                for (var j = 0; j < nodes[i]._itemData.length; j++) {
                    if (!nodes[i]._itemData[j].checked) {
                        ckText += "P_" + nodes[i]._itemData[j].attributes.SiteMapId + ",";
                    }
                }
            }
            if (!nodes[i]._checkBoxElement.checked) {
                ckText += "P_" + nodes[i]._attributes._data.SiteMapId + ",";
            }
        }
    } else {
        ckText = "P_" + args.get_node()._attributes._data.SiteMapId;
    }
    onCheckPermissionLanding(true, ckText);
    onShowHideAccessFuncCheckBox();
}

// Step 5: Check Assign Role
function checkSelectRole() {
    if (document.getElementById(uxHddSelectedRole).value == "False") {
        alert(Text_RequiredAtLeastOneRole);
        return false;
    }
    return true;
}

function ValidateData() {
    if (isCancelSubmit) {
        return false;
    }
    if (isIncludeSpecialCharacters(document.getElementById(uxRoleName_ClientID))) {
        $('#' + uxRoleName_ClientID).trigger('blur');
        return false;
    }
    if (ValidateInput()) {
        return true;
    }
    else {
        AdjustModalSize();
        return false;
    }
}

function checkAllCheckboxAccessFunction(containerId) {
    var checkboxes = document.getElementById(containerId).getElementsByTagName('input');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].type.toLowerCase() == 'checkbox') {
            if (!checkboxes[i].disabled)
                checkboxes[i].checked = true;
        }
    }

}

function onCheckPermissionBoarding() {
    if (tree_UM_nodes == null) {
        var tree_UM = document.getElementById(uxTreeMenuCS);
        tree_UM_nodes = tree_UM.control.get_allNodes();
    }
    var nodeValue = '';
    var isViewMPAList = false;
    var isViewTaskList = false;

    var inputs = $("#" + uxAccessFunctionList_ClientID + "").find('input');
    var viewAllMPA = $(inputs).filter(function () { return $(this).val() == 'MSViewAllMPAs' });
    var keyMerchantApplications = $(inputs).filter(function () { return $(this).val() == 'MSKeyMerchantApplications' });
    var manageMPAAssignments = $(inputs).filter(function () { return $(this).val() == 'MSManageMPAAssignments' });
    var viewFullSensitiveData = $(inputs).filter(function () { return $(this).val() == 'MSViewFullSensitiveData' });
    var withdrawMPA = $(inputs).filter(function () { return $(this).val() == 'MSWithdrawMPA' });
    var manageDocuments = $(inputs).filter(function () { return $(this).val() == 'MSManageDocuments' });

    for (var j = 0; j < tree_UM_nodes.length; j++) {
        nodeValue = tree_UM_nodes[j].get_value();
        if (nodeValue == 'MSViewMPAList') {
            if (tree_UM_nodes[j].get_checked()) {
                isViewMPAList = true;
                $(viewAllMPA).prop("disabled", false);
                $(keyMerchantApplications).prop("disabled", false);
                $(manageMPAAssignments).prop("disabled", false);
                $(viewFullSensitiveData).prop("disabled", false);
                $(withdrawMPA).prop("disabled", false);
                $(manageDocuments).prop("disabled", false);
            }
        }
        else if (nodeValue == 'MSViewTaskList') {
            if (tree_UM_nodes[j].get_checked()) {
                isViewTaskList = true;
                $(manageDocuments).prop("disabled", false);
            }
        }
    }
    if (!isViewMPAList) {
        $(viewAllMPA).prop("disabled", true);
        $(viewAllMPA).prop("checked", false);
        $(keyMerchantApplications).prop("disabled", true);
        $(keyMerchantApplications).prop("checked", false);
        $(manageMPAAssignments).prop("disabled", true);
        $(manageMPAAssignments).prop("checked", false);
        $(viewFullSensitiveData).prop("disabled", true);
        $(viewFullSensitiveData).prop("checked", false);
        $(withdrawMPA).prop("disabled", true);
        $(withdrawMPA).prop("checked", false);
        if (!isViewTaskList) {
            $(manageDocuments).prop("disabled", true);
            $(manageDocuments).prop("checked", false);
        }
    }
}

var isCancelSubmit = false;
function onRoleNameBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ValidateRoleName() == false || isIncludeSpecialCharacters(element)) {
        isCancelSubmit = true;
        removeSpecialCharacters(element);
        setTimeout('isCancelSubmit = false;', 500);
        if (ignore != null) {
            handleClickElement(ignore);
        }
        AdjustModalSize();
    }
}

function onRoleDesBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ValidateRoleDes() == false || isIncludeSpecialCharacters(element)) {
        isCancelSubmit = true;
        removeSpecialCharacters(element);
        setTimeout('isCancelSubmit = false;', 500);
        if (ignore != null) {
            handleClickElement(ignore);
        }
        AdjustModalSize();
    }
}

function handleClickElement(element) {
    if ($(element).val() == 'Cancel') {
        $(element).click();
    }
}

$(document).ready(
    function () {
        setTimeout(function () {
            onCheckPermissionBoarding();
            onCheckPermissionLanding(false);
            onShowHideAccessFuncCheckBox();
        }, 500);
    });

function validateDropdownTree() {
    var tree = $find(uxDefaultLandingPage_ClientID);
    if (tree != null) {
        if (tree._selectedValue != '')
            return true;
        else
            return false;
    }
    else {
        return false;
    }

}

function ValidateDefaultLandingPage(callback) {

    if (ValidateDefaultLandingPageInput()) {
        if (callback) {
            callback();
            return false;
        }
        else {
            return true;
        }
    }
    else {
        AdjustModalSize();
    }
    return false;
}


var tree_Landing_nodes = null;


function GetNodeByValue(nodelist, value) {
    for (var i = 0; i < nodelist.length; i++) {
        if (nodelist[i].get_value() == value) {
            return nodelist[i];
        }
    }
}


function SetParentVisible(childNode) {
    var parentNode = childNode.get_parent();
    var elens = 0;
    var haschild = false;
    if (parentNode != null && parentNode.get_allNodes().length > 0) {
        for (var i = 0; i < parentNode.get_allNodes().length ; i++) {
            if (!parentNode.get_allNodes()[i].get_enabled()) {
                elens++;
            }
            if (parentNode.get_allNodes()[i]._hasChildren()) {
                haschild = true;
            }
        }
        if (!haschild) {

            if (elens != parentNode.get_allNodes().length) {
                parentNode._removeClassFromContentElement("hide");
            }
            else {
                parentNode._addClassToContentElement("hide");
            }


        }

    }
}

function isHasChildNodeChecked(parentNode) {
    var hasChildChecked = false;
    if (parentNode.get_allNodes().length > 0) {
        for (var i = 0; i < parentNode.get_allNodes().length ; i++) {
            if (parentNode.get_allNodes()[i].get_checked()) {
                hasChildChecked = true;
            }
        }
    }
    return hasChildChecked;
}

function onCheckPermissionLanding(refersh, ckValue) {
    var tree_UM = $find(uxDefaultLandingPage_ClientID);
    var tree_MenuCS = document.getElementById(uxTreeMenuCS);

    //TK39894
    if (refersh && ckValue.indexOf("P_" + tree_UM._selectedValue.split('_')[1]) >= 0) {
        tree_UM.get_entries().clear();
    }

    if (tree_Landing_nodes == null) {
        tree_Landing_nodes = tree_UM.get_embeddedTree();
    }
    if (tree_UM_nodes == null) {
        tree_UM_nodes = tree_MenuCS.control.get_allNodes();
    }

    var nodeMenu;
    var nodeLanding;
    for (var j = 0; j < tree_UM_nodes.length; j++) {
        nodeMenu = tree_UM_nodes[j];

        nodeLanding = GetNodeByValue(tree_Landing_nodes.get_allNodes(), nodeMenu.get_value() + "_" + nodeMenu.get_attributes().getAttribute("SiteMapId"));

        if (nodeLanding != null) {
            if (nodeMenu._hasChildren() == true) {
                nodeLanding.set_enabled(false);

                if (!isHasChildNodeChecked(nodeMenu)) {
                    $(nodeLanding.get_element()).addClass("hide");
                }
                else {
                    $(nodeLanding.get_element()).removeClass("hide");
                }

            }
            else {
                if (nodeMenu.get_checked()) {
                    nodeLanding.set_enabled(true);
                    nodeLanding._removeClassFromContentElement("hide");
                }
                else {
                    nodeLanding.set_enabled(false);
                    nodeLanding._addClassToContentElement("hide");
                }
                SetParentVisible(nodeLanding);
            }

        }
    }
}

function masterAjax_requestStart(sender, args) {
    var loadingPanel = $find(uxLoadingPanel_ClientID);
    if (loadingPanel != null) {
        loadingPanel.show('cidModalContainer');
    }
}
function masterAjax_responseEnd(sender, args) {

    var loadingPanel = $find(uxLoadingPanel_ClientID);
    if (loadingPanel != null) {
        loadingPanel.hide('cidModalContainer');
    }
}

function DefaultLandingPageOnClientDropDownClosed(sender, args) {
    var tree_UM = $find(uxDefaultLandingPage_ClientID);
    $('#' + uxDefaultLandingPageSelectedValue_ClientID).val(tree_UM._selectedValue);
}


function isCheckedPermission(per) {
    if (per == null || per == '') {
        return true;
    }

    if (tree_UM_nodes == null) {
        var tree_UM = document.getElementById(uxTreeMenuCS_ClientID);
        tree_UM_nodes = tree_UM.control.get_allNodes();
    }

    // Check OR condition permissions require
    var pers = per.split("|");
    if (pers.length > 1) {
        for (i = 0; i < pers.length; i++) {
            if (CheckedPermissionRequire(pers[i], tree_UM_nodes))
                return true;
        }
    }

    //Check AND condition permissions require
    pers = per.split("&");
    for (i = 0; i < pers.length; i++) {
        if (!CheckedPermissionRequire(pers[i], tree_UM_nodes))
            return false;
    }
    return true;
}

function CheckedPermissionRequire(per, tree_UM_nodes) {
    for (var j = 0; j < tree_UM_nodes.length; j++) {
        var node = tree_UM_nodes[j];
        var value = node.get_value();
        var isChecked = node.get_checked();
        var hasChild = node._hasChildren();
        //if (nodeValues == per && isNodeChecked == true && nodeValues.indexOf(',') == -1 && !hasChild) {
        if (isChecked == true && per == value && !hasChild) {
            return true;
        }
    }
    return false;
}

function ShowHideGroupHeader() {
    $('.treeview .parent').each(function () {
        var target = $(this).attr('data-target');
        var s = $('.treeview .child[data-target="' + target + '"]').length;
        var h = $('.treeview .child.hide[data-target="' + target + '"]').length;
        if (s == h) {
            $(this).addClass('hide');
        }
        else {
            $(this).removeClass('hide');
        }
    });
}

function onShowHideAccessFuncCheckBox() {
    $('#clientAccessFunction').find('.child').each(function () {
        var isAFChecked = isCheckedPermission($(this).attr("data-target-required"));
        if (isAFChecked) {
            $(this).removeClass('hide');
        }
        else {
            $(this).addClass('hide');
            $(this).find('input[type="checkbox"]').prop('checked', '');
        }
    });
    ShowHideGroupHeader();
}
function customMoveStep4()
{
    return moveStep(backStepFromStep4);
}