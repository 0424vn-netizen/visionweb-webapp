var mdlCreateEdit = mdlCreateEdit || {};

mdlCreateEdit = (function ($) {

    var isChanged = false;

    var isUniqueName = true;

    function getIsUniqueName() {
        return isUniqueName;
    }

    var lbAssigment, lbWorkQueue;

    var oldData = {};

    function onLoadControlRad() {


        lbAssigment = mdlRiskListBox.findControl(lbAssigmentId);
        lbWorkQueue = mdlRiskListBox.findControl(lbWorkQueueId);

        lbAssigment.onLoad();
        lbWorkQueue.onLoad();

        var listenerAssigment = lbAssigment.get_eventListener();
        var listenerWorkQueue = lbWorkQueue.get_eventListener();

        listenerAssigment.on("lbRiskAutoQueue_onChanged", function (sender, args) {
            var parent = args.parent;
            var item = this.get_item(parent);
            if (item.selectable) {
                if (item.checked) {
                    mdlRiskListBox.paramAsgmQws.push(item);
                }
                else {
                    mdlRiskListBox.paramAsgmQws.forEach(function (obj, index) {
                        if (obj.value == item.value) {
                            mdlRiskListBox.paramAsgmQws.splice(index, 1);
                        }
                    })
                }

            }
            this.set_checked(sender, args.currentCheck);

            if (this.get_checked(sender)) {
                this.set_selected(args.parent);
            } else {
                this.set_unSelected(args.parent);
            }
        })

        listenerWorkQueue.on("lbRiskAutoQueue_onChanged", function (sender, args) {
            var parent = args.parent;
            var item = this.get_item(parent);
            if (item.selectable) {
                if (item.checked) {
                    mdlRiskListBox.paramAsgmQws.push(item);
                }
                else {
                    mdlRiskListBox.paramAsgmQws.forEach(function (obj, index) {
                        if (obj.value == item.value) {
                            mdlRiskListBox.paramAsgmQws.splice(index, 1);
                        }
                    })
                }

            }
            this.set_checked(sender, args.currentCheck);
            if (this.get_checked(sender)) {
                this.set_selected(args.parent);
            } else {
                this.set_unSelected(args.parent);
            }

        })

        $("form").change(function (e) {
            isChanged = true;
            showHideSubmitBtn();
        });

        $($get(txtNameId)).keyup(function () {
            isChanged = true;
            showHideSubmitBtn();
        });

        $($get(txtDescriptionId)).keyup(function () {
            isChanged = true;
            showHideSubmitBtn();
        })

        createOldData();
        showHideSubmitBtn();
    }

    function createOldData() {
        oldData.OldName = $get(txtNameId).value;
        oldData.OldDesc = $get(txtDescriptionId).value;
        //oldData.OldArrg = $get(chkAggregateId).checked.toString();
        oldData.OldActive = $get(rdActiveId).checked.toString();
    }

    function isChangedTxtName() {
        if (autoQueueMode == "Edit")
            return $get(txtNameId).value !== oldData.OldName;
        return true;
    }

    //function isChangedAggregate() {
    //    if (autoQueueMode == "Edit")
    //        return $get(chkAggregateId).checked.toString() !== oldData.OldArrg;
    //    return true;
    //}

    function isChangedDescription() {
        if (autoQueueMode == "Edit")
            return $get(txtDescriptionId).value !== oldData.OldDesc;
        return true;
    }

    function isChangedActive() {
        if (autoQueueMode == "Edit")
            return $get(rdActiveId).checked.toString() !== oldData.OldActive;
        return true;
    }

    function onCheckNameAvailableSuccess(response) {
        var res = $.parseJSON(response);
        var strInValidName = res.name;
        var strInValidAssigment = res.assignment;

        if (strInValidName === "True") {
            isUniqueName = false;
            var isValid = ValidateInput();
            AdjustModalSize();
            return isValid;
        }
        else {
            var message = createMessage;

            if (autoQueueMode == "Create") {
                submitWithConfirm(message, strInValidAssigment);
            } else if (autoQueueMode == "Edit") {
                if (strInValidAssigment.length > 0) {
                    strInValidAssigment = anotherAssignmentMessage.replace("[Assignment]", strInValidAssigment)
                    showRadMessage("alert", strInValidAssigment, null, "Warning", 'auto');
                } else {
                    $get(btnSubmitId).click();
                }
            }
        }
    }

    function submitWithConfirm(message, strInValidAssigment) {
        showRadMessage("confirm", message, function (arg) {
            if (arg) {
                if (strInValidAssigment.length > 0) {
                    strInValidAssigment = anotherAssignmentMessage.replace("[Assignment]", strInValidAssigment)
                    showRadMessage("alert", strInValidAssigment, null, "Warning", 'auto');
                } else {
                    $get(btnSubmitId).click();
                }
                return false;
            }
        }, modalConfirm, 'auto')
    }

    function onSubmitClick() {
        if (isCancelSubmit) {
            return false;
        }
        var isValid = false;
        if (mdlRiskListBox.paramAsgmQws.length > 0) {
            var msg = "";
            mdlRiskListBox.paramAsgmQws.forEach(function (item) {
                msg += item.text + ", ";
            })
            msg = msg.replace(/\([á\.\w\&\s]*\)/ig, "").trim();
            msg = assignmentWorkqueueMessage.replace("[WorkQueue]", msg);
            showRadMessage("alert", msg, null, modalWarning, 'auto');
        } else {
            isValid = ValidateInputSpecialCharacters();  
            if (isValid == true) {
                isValid = ValidateInputSpecialCharactersDescription();
            }
            else {
                var txtNameNewValue = removeSpecialCharacters(document.getElementById(txtNameId));
                $(txtNameId).val(txtNameNewValue);
                return isValid;
            }
            if (isValid == true) {
                checkNameAvailable();
            }
            else {
                return isValid;
            }
        }
        return false;
    }

    function checkValidForm() {
        var isValid = true;
        var txtName = $find(txtNameId);

        if (autoQueueMode === "Edit") {

            //44078 - VW - Paysafe - Assignment Processing Status = 'Completed' includes AQ allocation to WQ
            $("#" + isRequiredBe).val(false);
            if ((isChangedActive() && oldData.OldActive == "false") || (lbAssigment.isChanged || lbWorkQueue.isChanged) && ($get(rdActiveId).checked.toString() == "true"))
            {
                $("#" + isRequiredBe).val(true);
            }

            isValid = isChangedActive() || isChangedDescription() || isChangedTxtName() || lbAssigment.isChanged || lbWorkQueue.isChanged;
        }

        isValid = isValid && (lbAssigment.get_itemSeleted().length > 0 && lbWorkQueue.get_itemSeleted().length > 0 && txtName.get_textBoxValue().length > 0);
        return isValid;
    }

    function showHideSubmitBtn() {
        if (autoQueueMode === "Edit" && isChanged == false) {
            $get(btnSubmitCoverId).disabled = true;
        } else {
            $get(btnSubmitCoverId).disabled = !checkValidForm();
        }
    }

    function onDeleteAutoQueue() {
        if (isCancelSubmit) {
            return false;
        }
        checkDeleteAvailable();
        return false;
    }

    function showMessageSuccess() {
        var message = updateMessage;
        if (autoQueueMode == "Edit") {
            showRadMessage("alert", message, function () { closePopupRedirectManagement() }, modalSuccess, 'auto');
        } else {
            closePopupRedirectManagement();
        }
    }

    function closePopupRedirectManagement() {
        parent.HidePopupModal();
        parent.doReloadManageAutoQueue();
    }

    function onCheckDeleteAvailableSuccess(response) {
        if (response !== "") {
            var msg = workQueueDeleteMessage.replace(/\[AutoQueueName\]/g, autoQueueName);
            msg = msg.replace(/\[WorkQueueName\]/g, response);
            showRadMessage("alert", msg, null, modalWarning, 'auto');
        } else {
            showRadMessage("confirm", deleteMessage, function (arg) {
                if (arg) {
                    $get(btnDeleteId).click();
                    parent.HidePopupModal();
                }
            }, modalConfirm, null, null);

            return false;
        }
    }

    return {
        onLoadControlRad: onLoadControlRad,
        onDeleteAutoQueue: onDeleteAutoQueue,
        onSubmitClick: onSubmitClick,
        checkNameAvailableSuccess: onCheckNameAvailableSuccess,
        getIsUniqueName: getIsUniqueName,
        showMessageSuccess: showMessageSuccess,
        checkDeleteAvailableSuccess: onCheckDeleteAvailableSuccess
    };
})($telerik.$);

Sys.Application.add_load(function () {
    mdlCreateEdit.onLoadControlRad();
});

function customValidate() {
    return mdlCreateEdit.getIsUniqueName();
}
function validateSpecialCharacters() {
    if (isIncludeSpecialCharacters(document.getElementById(txtNameId))) {
        return false;
    }
    return true;
}

function validateSpecialCharactersDescription() {
    if (isIncludeSpecialCharacters(document.getElementById(txtDescriptionId))) {
        return false;
    }
    return true;
}

var isCancelSubmit = false;
function onBriefDescriptionBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ValidateInputSpecialCharactersDescription() == false || isIncludeSpecialCharacters(element)) {
        isCancelSubmit = true;
        var newvalue = removeSpecialCharacters(element);
        $(element).val(newvalue);
        setTimeout('isCancelSubmit = false;', 500);
        if (ignore != null) {
            handleClickElement(ignore);
        } else if (eventMouseDown != undefined) {
            handleClickLabelElement(eventMouseDown);
        }
    }
}

function onNameBlur(element, e) {
    var ignore = e.relatedTarget || e.rangeParent;
    if (ignore == undefined) {
        if (e.originalEvent != undefined)
            ignore = e.originalEvent.explicitOriginalTarget;
    }
    if (ValidateInputSpecialCharacters() == false || isIncludeSpecialCharacters(element)) {
        isCancelSubmit = true;
        removeSpecialCharacters(element);
        setTimeout('isCancelSubmit = false;', 500);
        if (ignore != null) {
            handleClickElement(ignore);
        } else if (eventMouseDown != undefined) {
            handleClickLabelElement(eventMouseDown);
        }
    }
}

function handleClickElement(element) {
    if ($(element).val() == 'Cancel') {
        $(element).click();
    } else if ($(element).val() == 'rdActive' || $(element).val() == 'rdInactive') {
        element.checked = true;
    }
}

function handleClickLabelElement(eventMouseDown) {
    $(eventMouseDown.currentTarget).click();
    eventMouseDown.currentTarget.classList.add('disabled-pointer-events');
    setTimeout(function () {
        if (eventMouseDown != undefined) {
            eventMouseDown.currentTarget.classList.remove('disabled-pointer-events');
            eventMouseDown = undefined;
        }
    }, 300);
}

var eventMouseDown = undefined;
$(".list-group-action label").mousedown(function (e) {
    if (eventMouseDown == undefined) {
        eventMouseDown = e;
    }
});

$(".list-group-action label").mouseup(function (e) {
    if (eventMouseDown != undefined && e.currentTarget.id == eventMouseDown.currentTarget.id) {
        eventMouseDown.currentTarget.classList.remove('disabled-pointer-events');
        eventMouseDown = undefined;
    }
});





