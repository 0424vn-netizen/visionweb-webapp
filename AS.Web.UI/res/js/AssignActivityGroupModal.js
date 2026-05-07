
$(document).ready(function () {
    $("table#uiTable tr[class!='nobackground'][class!='hide']:odd").addClass('AltRow');
    $("table#uiTable tr[class!='nobackground'][class!='hide']:even").addClass('Row');
    removeColumnNotExistData(GROUP_MODE.View);
    removeColumnNotExistData(GROUP_MODE.Edit);
    removeColumnNotExistData(GROUP_MODE.Manage);

    checkAll(GROUP_MODE.View, checkall_view);
    checkAll(GROUP_MODE.Edit, checkall_edit);
    checkAll(GROUP_MODE.Manage, checkall_manage);
});

var GROUP_MODE = {
    View: "view",
    Edit: "edit",
    Manage: "manage"
}

var CHECKBOX_STATE = {
    Checked: "rbToggleCheckboxChecked",
    Unchecked: "rbToggleCheckbox",
    Filled: "rbToggleCheckboxFilled",
    Disabled: "rbDisabled"
}

//Keep state of checkbox all when close modal temporary or open after saved
function checkAll(group, ckAllId)
{
    var isCheckAll = true;
    var isFilled = false;
    $("#uiTable > tbody").find("button").each(function () {
        var $this = $(this);
        if ($(this).attr("as-checkall") != "true") {
            var obj = $this.find("span.rbIcon");
            if ($(this).attr("as-group") == group && obj.hasClass(CHECKBOX_STATE.Unchecked)) {
                isCheckAll = false;
            }
        }
    });

    $("#uiTable > tbody").find("button").each(function () {
        var $this = $(this);
        if ($(this).attr("as-checkall") != "true") {
            var obj = $this.find("span.rbIcon");
            if ($(this).attr("as-group") == group && obj.hasClass(CHECKBOX_STATE.Checked) && !isCheckAll) {
                isFilled = true;
            }
        }
    });
    if (isCheckAll) {
        $("#" + ckAllId).find("span.rbIcon").removeClass(CHECKBOX_STATE.Unchecked).addClass(CHECKBOX_STATE.Checked);
    }
    if (isFilled)
    {
        $("#" + ckAllId).find("span.rbIcon").removeClass(CHECKBOX_STATE.Unchecked).addClass(CHECKBOX_STATE.Filled);
    }
}

//Hide column which contains the checkboxes but doesn't have any checkbox is checked
function removeColumnNotExistData(group)
{
    var isRemove = true;
    $("#uiTable > tbody > tr").find("td." + group).each(function () {
        if ($(this).find("button").length > 0) {
            isRemove = false;
        }
    });
    if (isRemove) {
        $("#uiTable > tbody > tr").find("td." + group).each(function () {
            $(this).addClass("hide-child");
        });
        $("#uiTable > tbody > tr").find("th." + group).each(function () {
            $(this).addClass("hide-child");
        });
    }
}

/* The checkbox's filled state show when in a group has at least a checkbox is checked and an another is unchecked */
function isFilledState(group) {
    var isExsitChecked = false;
    var isExsitUnchecked = false;
    $("#uiTable > tbody").find("button").each(function () {
        var obj = $find($(this).prop("id"));
        if ($(this).attr("as-group") == group) {
            if ($(this).attr("as-checkall") != "true" && obj.get_checked() == false) {
                isExsitUnchecked = true;
            }
        }
    });
    $("#uiTable > tbody").find("button").each(function () {
        var obj = $find($(this).prop("id"));
        if ($(this).attr("as-group") == group) {
            if ($(this).attr("as-checkall") != "true" && obj.get_checked() == true) {
                isExsitChecked = true;
            }
        }
    });
    if (isExsitChecked && isExsitUnchecked)
        return true;
    return false;
}

/* Change checkbox's state between check, uncheck and filled */
function onChangeStateCheckboxAll(group, isChecked) {
    //Always check will "View" group
    if (isFilledState(GROUP_MODE.View)) {
        //Add "filled" state
        $("#" + checkall_view).find("span.rbIcon").removeClass(CHECKBOX_STATE.Unchecked).removeClass(CHECKBOX_STATE.Checked).addClass(CHECKBOX_STATE.Filled);
        $find(checkall_view).set_enabled(true);

    } else {
        if (isChecked) {
            $("#" + checkall_view).find("span.rbIcon").removeClass(CHECKBOX_STATE.Filled).addClass(CHECKBOX_STATE.Checked);
        }
        else {
            $("#" + checkall_view).find("span.rbIcon").removeClass(CHECKBOX_STATE.Filled).removeClass(CHECKBOX_STATE.Checked).addClass(CHECKBOX_STATE.Unchecked);
        }
    }
    if (group == GROUP_MODE.Edit || group == GROUP_MODE.Manage) {
        if (isFilledState(GROUP_MODE.Edit)) {
            $("#" + checkall_edit).find("span.rbIcon").removeClass(CHECKBOX_STATE.Unchecked).removeClass(CHECKBOX_STATE.Checked).addClass(CHECKBOX_STATE.Filled);
            $find(checkall_edit).set_enabled(true);
        } else {
            if (isChecked) {
                $("#" + checkall_edit).find("span.rbIcon").removeClass(CHECKBOX_STATE.Filled).addClass(CHECKBOX_STATE.Checked);
            }
            else {
                $("#" + checkall_edit).find("span.rbIcon").removeClass(CHECKBOX_STATE.Filled).removeClass(CHECKBOX_STATE.Checked).addClass(CHECKBOX_STATE.Unchecked);
            }
            $find(checkall_view).set_enabled(!isChecked);
            $find(checkall_edit).set_enabled(true);
        }
    }
    if (group == GROUP_MODE.Manage) {
        if (isFilledState(GROUP_MODE.Manage)) {
            $("#" + checkall_manage).find("span.rbIcon").removeClass(CHECKBOX_STATE.Unchecked).removeClass(CHECKBOX_STATE.Checked).addClass(CHECKBOX_STATE.Filled);
            $find(checkall_manage).set_enabled(true);
        } else {
            if (isChecked) {
                $("#" + checkall_manage).find("span.rbIcon").removeClass(CHECKBOX_STATE.Filled).addClass(CHECKBOX_STATE.Checked);
            }
            else {
                $("#" + checkall_manage).find("span.rbIcon").removeClass(CHECKBOX_STATE.Filled).removeClass(CHECKBOX_STATE.Checked).addClass(CHECKBOX_STATE.Unchecked);
            }
            $find(checkall_manage).set_enabled(true);
            $find(checkall_view).set_enabled(!isChecked);
            $find(checkall_edit).set_enabled(!isChecked);
        }
    }
    $("#" + checkall_view).removeClass("rbHovered");
    $("#" + checkall_edit).removeClass("rbHovered");
}

function checkAllStatus(group, isChecked)
{
    var isCheckAll = true;
    $("#uiTable > tbody").find("button").each(function () {
        var $this = $(this);
        var obj = $find($this.prop("id"));
        //Check/uncheck all "manage"
        if ($this.attr("as-group") == group && obj.get_checked() == false) {
            isCheckAll = false;
        }
    });
    if (isCheckAll == true && isChecked == true)
        return false;
    return isChecked;
}
// Check all function
function onCheckAll(sender, args) {
    
    var group = $(sender.get_element()).attr('as-group');
    var isChecked = checkAllStatus(group, sender.get_checked());
    if ($(sender.get_element()).find("span.rbIcon").hasClass(CHECKBOX_STATE.Filled))
        isChecked = true;
    if (group == GROUP_MODE.Manage) {
        //Check all and disable "view" and "edit"
        $("#uiTable > tbody").find("button").each(function () {
            var $this = $(this);
            var obj = $find($this.prop("id"));
            //Check/uncheck all "manage"
            if ($this.attr("as-group") == group && !$this.hasClass("disabled")) {
                obj.set_checked(isChecked);

                //Check with checkbox the same tr
                $this.parents("tr").find("button").each(function (key, args) {
                    if ($(args).attr("as-group") != GROUP_MODE.Manage) {
                        $find(args.id).set_checked(isChecked);
                        $find(args.id).set_enabled(!isChecked);
                        $(args).attr("as-readonly", isChecked);
                    }
                });
            }
        });
    }

    if (group == GROUP_MODE.Edit) {
        $("#uiTable > tbody").find("button").each(function () {
            var $this = $(this);
            var obj = $find($this.prop("id"));
            if ($this.attr("as-group") == group && $this.attr("as-readonly") != "true") {
                obj.set_checked(isChecked);
                obj.set_enabled(true);
                $this.attr("as-readonly", false);
                //Check with checkbox the same tr
                $this.parents("tr").find("button").each(function (key, args) {
                    if ($(args).attr("as-group") == GROUP_MODE.View) {
                        $find(args.id).set_checked(isChecked);
                        $find(args.id).set_enabled(!isChecked);
                        $(args).attr("as-readonly", isChecked);
                    }
                });
            }
        });
    }

    $("#uiTable > tbody").find("button").each(function () {
        var $this = $(this);
        var obj = $find($this.prop("id"));
        if (obj != null && $this.attr("as-group") == GROUP_MODE.View && !$this.hasClass("disabled")) {
            // For viewall
            if (group == GROUP_MODE.View) {
                if ($this.attr("as-readonly") != "true")
                    obj.set_checked(isChecked);
            }
        }
    });
    onChangeStateCheckboxAll(group, isChecked);
}

function onCheckAllEditClientLoad(sender, args) {
    var obj = $(sender.get_element());
    var group = obj.attr("as-group");

    if (!$("#" + checkall_manage).parent().hasClass("hide-child")
        && $("#" + checkall_manage).find("span.rbIcon").hasClass(CHECKBOX_STATE.Checked))
        sender.set_enabled(false);
}

function onCheckAllViewClientLoad(sender, args) {
    var obj = $(sender.get_element());
    var group = obj.attr("as-group");

    if ((!$("#" + checkall_manage).parent().hasClass("hide-child")
        && $("#" + checkall_manage).find("span.rbIcon").hasClass(CHECKBOX_STATE.Checked))
        || (!$("#" + checkall_edit).parent().hasClass("hide-child")
        && $("#" + checkall_edit).find("span.rbIcon").hasClass(CHECKBOX_STATE.Checked)))
        sender.set_enabled(false);
}

function onClientLoad(sender, args) {
    var group = $(sender.get_element()).attr('as-group');
    var isChecked = sender.get_checked();

    if (isChecked && group != GROUP_MODE.View) {
        var trParent = $(sender.get_element()).parents("tr");
        trParent.find("button").each(function (key, args) {
            var $this = $(args);
            if (group == GROUP_MODE.Manage) {
                //Disable checkbox "edit"
                if ($this.attr("as-group") == GROUP_MODE.Edit) {
                    $find(args.id).set_enabled(false);
                    $find(args.id).set_checked(true);
                    $this.attr("as-readonly", isChecked);
                }
            }
            if ($this.attr("as-group") == GROUP_MODE.View) {
                $find(args.id).set_enabled(false);
                $this.attr("as-readonly", isChecked);
            }
        })
    }
}

function setValue(sender) {
    
    var jqObj = $(sender.get_element());
    var group = jqObj.attr('as-group');

    if (group != GROUP_MODE.View) {
        var trParent = jqObj.parents("tr");
        var isChecked = sender.get_checked();
        trParent.find("button").each(function (key, args) {

            var $this = $(args);
            var obj = $find(args.id);

            if (group == GROUP_MODE.Manage && !$this.hasClass("disabled")) {
                if ($this.attr("as-group") == GROUP_MODE.Edit && !$this.hasClass("disabled")) {
                    obj.set_checked(isChecked);
                    obj.set_enabled(!isChecked);
                    $this.attr("as-readonly", isChecked);
                }
            }
            if (group == GROUP_MODE.Edit && !$this.hasClass("disabled")) {
                if ($this.attr("as-group") == GROUP_MODE.Manage && !$this.hasClass("disabled")) {
                    obj.set_checked(false);
                }
            }

            if ($this.attr("as-group") == GROUP_MODE.View && !$this.hasClass("disabled")) {
                obj.set_checked(isChecked);
                obj.set_enabled(!isChecked);
                $this.attr("as-readonly", isChecked);
            }

        });
    }
    onChangeStateCheckboxAll(group, sender.get_checked());
}

function onCheckedChange(sender, args) {
    setValue(sender);
}

function GetPers(e) {
    var listGroupPers = [];
    var groupPers = $("#groupPers > table > tbody");
    var nodes = groupPers.find("button");
    groupPers.find("button").each(function () {
        var obj = $find($(this).prop("id"));
        //Ignore checkbox all
        if (obj.get_checked() == true && $(this).attr("as-checkall") != "true")
            listGroupPers.push(obj.get_value());
    });
    $("#" + hdSelectedPers).val(listGroupPers);
    //Call button temp in case FF browser
    $("#" + btnSubmit).click();
    ClosePopupModal();
    return false;
}