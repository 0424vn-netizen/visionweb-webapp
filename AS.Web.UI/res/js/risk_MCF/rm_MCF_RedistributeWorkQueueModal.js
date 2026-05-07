var mdlRedistribute = mdlRedistribute || {};
var mdlRiskListBox = mdlRiskListBox || {};

mdlRedistribute = (function ($) {

    function onLoadControlRad() {
        checkValidForm();
        var lbRiskAutoQueue = mdlRiskListBox.findControl(lbAutoQueueId);
        var lbWorkQueueRedis = mdlRiskListBox.findControl(lbWorkQueueRedisId);
        var lbWorkQueueDes = mdlRiskListBox.findControl(lbWorkQueueDesId);

        lbRiskAutoQueue.isAjaxUpdate = true;
        lbWorkQueueRedis.isAjaxUpdate = true;
        lbWorkQueueDes.isAjaxUpdate = true;

        lbRiskAutoQueue.onLoad();
        lbWorkQueueRedis.onLoad();
        lbWorkQueueDes.onLoad();

        var listenerAutoQueue = lbRiskAutoQueue.get_eventListener();
        var listenerQueueRedis = lbWorkQueueRedis.get_eventListener();
        var listenerQueueDes = lbWorkQueueDes.get_eventListener();

        listenerAutoQueue.on("lbRiskAutoQueue_onChanged", function (sender, args) {

            this.reset_all_checkBoxes();
            this.set_checked(sender, args.currentCheck);
            if (this.get_checked(sender)) {
                this.set_selected(args.parent);
            } else {
                this.set_unSelected(args.parent);
            }

            checkValidForm()
        })

        listenerQueueRedis.on("lbRiskAutoQueue_onChanged", function (sender, args) {
            this.set_checked(sender, args.currentCheck);
            if (this.get_checked(sender)) {
                this.set_selected(args.parent);
            } else {
                this.set_unSelected(args.parent);
            }
            checkValidForm()
        })

        listenerQueueDes.on("lbRiskAutoQueue_onChanged", function (sender, args) {
            this.set_checked(sender, args.currentCheck);
            if (this.get_checked(sender)) {
                this.set_selected(args.parent);
            } else {
                this.set_unSelected(args.parent);
            }

            checkValidForm()
        })
    }

    function checkRedistributeAvailableSuccess(response) {
        if (response.length > 0) {
            var message = messageInvalidRedistribute.replace("[paramsWorkQueueRedis]", response)
            showRadMessage("alert", message, function (arg) {
                reloadRedistributeForm();
            }, modalSuccess, null, null);
            return false;
        }
        var messageConfirm = getMessageConfirm();
        showRadMessage("confirm", messageConfirm, function (arg) {
            if (arg) {
                $get(btnSubmitId).click();
            }
        }, modalConfirm, null, null);
    }

    function checkValidForm() {
        var lbWorkQueueDest = mdlRiskListBox.findControl(lbWorkQueueDesId);
        var lbWorkQueueRedis = mdlRiskListBox.findControl(lbWorkQueueRedisId);
        var lbAutoQueue = mdlRiskListBox.findControl(lbAutoQueueId);

        var isValid = lbAutoQueue.get_itemSeleted().length > 0 && lbWorkQueueRedis.get_itemSeleted().length > 0 && lbWorkQueueDest.get_itemSeleted().length > 0;
        $get(btnSubmitCoverId).disabled = !isValid;
        return isValid;
    }

    function getMessageParamsConfirm(lb) {
        var message = "";
        var item = lb.get_item_elements_selected();
        item.forEach(function (item, index) {
            message += item.text + ", ";
        })

        message = message.substring(0, message.lastIndexOf(", ")).trim();
        return message;
    }

    function getMessageConfirm() {
        var lbWorkQueueDest = mdlRiskListBox.findControl(lbWorkQueueDesId);
        var lbWorkQueueRedis = mdlRiskListBox.findControl(lbWorkQueueRedisId);
        var lbAutoQueue = mdlRiskListBox.findControl(lbAutoQueueId);

        var paramsAutoQueue = getMessageParamsConfirm(lbAutoQueue);
        var paramsWorkQueueRedis = getMessageParamsConfirm(lbWorkQueueRedis);
        var paramsWorkQueueDest = getMessageParamsConfirm(lbWorkQueueDest);

        var message = messageConfirm;
        message = message.replace("[paramsWorkQueueRedis]", paramsWorkQueueRedis);
        message = message.replace("[paramsAutoQueue]", paramsAutoQueue);
        message = message.replace("[paramsWorkQueueDest]", paramsWorkQueueDest);

        return message;
    }

    function onSubmitClick() {
        checkRedistributeAvailable();
        return false;
    }

    function reloadRedistributeForm() {
        var oWindow = GetRadWindow();
        oWindow.reload();
    }

    return {
        onLoadControlRad: onLoadControlRad,
        onSubmitClick: onSubmitClick,
        checkRedistributeAvailableSuccess: checkRedistributeAvailableSuccess
    };

})($telerik.$);

Sys.Application.add_load(function () {
    mdlRedistribute.onLoadControlRad();
});

