var mdlRiskListBox = mdlRiskListBox || {};

mdlRiskListBox = (function ($) {
    var customListBox = {};
    var paramAsgmQws = [];

    Array.prototype.compareWith = function (newData) {
        var that = this;
        if (that.length != newData.length) return false;

        for (var i = 0; i < that.length; i++) {
            if (that[i].value !== newData[i].value)
                return true;
        }
        return false;
    }

    function findControl(controlId) {
        var riskListBox = $("#" + controlId) || {};

        var eventListener = function () {
            this.events = [];
        }

        eventListener.prototype.on = function (event, fn) {
            this.events[event] = this.events[event] || [];
            this.events[event].push(fn);
        }

        eventListener.prototype.fire = function (eventName, sender, args) {
            if (this.events[eventName]) {
                this.events[eventName].forEach(function (fn) {
                    fn.call(riskListBox, sender, args);
                })
            }
        }

        var evListener = new eventListener();

        riskListBox.isChanged = false;
        riskListBox.oldData = {};
        riskListBox.isAjaxUpdate = false;

        riskListBox.get_eventListener = function () {
            return evListener;
        }

        riskListBox.get_item = function (ctrl) {
            var item = {};
            item.checkbox = $(ctrl).find('input[type="checkbox"]').eq(0);
            item.hiddenState = $(ctrl).find('input[type="hidden"][name*="uxValue"]').eq(0);
            item.hiddenFocus = $(ctrl).find('input[type="hidden"][name*="Focus"]').eq(0);
            item.checked = item.checkbox.prop("checked");
            item.value = item.hiddenState.val();
            item.text = ($(ctrl).find('label[id*="uxLabel"]').eq(0)).text();
            item.selectable = $(ctrl).find(".select-italic-item").length > 0;
            return item;
        }

        riskListBox.get_item_elements_selected = function () {
            var items = riskListBox.get_itemSeleted();
            var arrItem = [];
            items.each(function () {
                var that = this;
                var parent = riskListBox.get_parentCheckBox(that);
                var item = riskListBox.get_item(parent);
                arrItem.push(item);
            })
            return arrItem;
        }

        riskListBox.get_items = function () {
            var items = riskListBox.find('.rlbItem');

            return items;
        }

        riskListBox.get_checkBoxs = function () {
            return riskListBox.find('input[type="checkbox"]');
        }

        riskListBox.get_parentCheckBox = function (ctrlCheckBox) {
            return $(ctrlCheckBox).parents('.rlbItem')[0];
        }

        riskListBox.set_hover_addnew = function (ctrlLi) {
            $(ctrlLi).addClass("rlbAddNew");
        }

        riskListBox.set_remove_hover_addnew = function (ctrlLi) {
            $(ctrlLi).removeClass("rlbAddNew");
        }

        riskListBox.set_canSelected = function (ctrlLi) {
            $(ctrlLi).removeClass("rlbRemoveable")
        }

        riskListBox.set_canNotSelected = function (ctrlLi) {
            $(ctrlLi).addClass("rlbRemoveable")
        }

        riskListBox.set_selected = function (ctrlLi) {
            $(ctrlLi).addClass("rlbSelected")
        }

        riskListBox.set_focus = function (ctrlLi) {
            $(ctrlLi).addClass("focus");
        }

        riskListBox.set_remove_focus = function (ctrlLi) {
            $(ctrlLi).removeClass("focus");
        }

        riskListBox.set_unSelected = function (ctrlLi) {
            $(ctrlLi).removeClass("rlbSelected")
        }

        riskListBox.set_checked = function (ctrlCheckBox, value) {
            $(ctrlCheckBox).prop("checked", value);
        }

        riskListBox.get_checked = function (ctrlCheckBox) {
            return $(ctrlCheckBox).prop("checked");
        }

        riskListBox.reset_all_checkBoxes = function () {
            this.get_checkBoxs().each(function () {
                var that = this;
                riskListBox.set_checked(that, false);
                riskListBox.get_items().removeClass("rlbSelected");
            })
        }

        riskListBox.get_hidden_state = function (ctrlCheckBox) {
            var parent = riskListBox.get_parentCheckBox(ctrlCheckBox);
            return $(parent).find('input[type="hidden"][name*="uxFocus"]');
        }

        riskListBox.hasSelected = function (ctrlLi) {
            return $(ctrlLi).hasClass("rlbSelected")
        }

        riskListBox.get_itemSeleted = function () {

            return riskListBox.find('input[type="checkbox"]:checked');
        }

        riskListBox.get_itemSeleted_element = function () {
            var chks = riskListBox.get_itemSeleted();
            var list = [];
            chks.each(function () {
                var chk = this;
                var parent = riskListBox.get_parentCheckBox(chk);
                list.push(parent);
            })
            return list;
        }

        riskListBox.check_changed_data = function () {
            var oldData = riskListBox.oldData;
            var newData = riskListBox.get_item_elements_selected();
            if (oldData.length != newData.length) return true;

            return oldData.compareWith(newData);
        }

        riskListBox.get_checkBox = function (ctrlLi) {
            return $(ctrlLi).find('input[type="checkbox"]');
        }

        riskListBox.get_hidden_all = function () {
            return riskListBox.get_items().find('input[type="hidden"][name*="uxFocus"]');
        }

        riskListBox.get_element = function () {
            return document.getElementById("controlId");
        }

        riskListBox.set_state_scroll = function () {
            var nameHidden = riskListBox.attr("id") + "_hidden";
            var hiddenState = $("input[name=" + nameHidden + "]");

            if (hiddenState.length == 0) {
                var stateElement = document.createElement("input");
                stateElement.setAttribute("type", "hidden");
                stateElement.setAttribute("name", riskListBox.attr("id") + "_hidden");
                stateElement.setAttribute("value", "0");
                $("form").append($(stateElement));
            }
        }

        riskListBox.scrollToByState = function () {
            var nameHidden = riskListBox.attr("id") + "_hidden";
            var hiddenState = $("input[name=" + nameHidden + "]");
            riskListBox.find(".list-group-action").eq(0).scrollTop(hiddenState.val());
        }

        riskListBox.onLoad = function () {

            if (riskListBox.isAjaxUpdate) {
                riskListBox.set_state_scroll();
                riskListBox.scrollToByState();
                riskListBox.get_hidden_all().each(function () {
                    var hd = this;
                    var parent = $(hd).parents(".rlbItem")[0]
                    if ($(hd).val() === "focus") {
                        riskListBox.set_focus(parent);
                    }
                })

                riskListBox.find(".list-group-action").eq(0).scroll(function () {
                    var that = this;
                    var nameHidden = riskListBox.attr("id") + "_hidden";
                    var hiddenState = $("input[name=" + nameHidden + "]");
                    var pos = $(that).scrollTop();
                    hiddenState.val(pos);
                });
            }


            riskListBox.get_checkBoxs().change(function (e) {
                var sender = this;
                var parent = customListBox.get_parentCheckBox(sender);
                var currentCheck = customListBox.get_checked(sender);
                var hd = riskListBox.get_hidden_state(sender);
                riskListBox.isChanged = riskListBox.check_changed_data();

                var isSelected = riskListBox.get_checked(sender);
                if (isSelected) {
                    hd.val("focus");
                    riskListBox.set_focus(riskListBox.get_parentCheckBox(sender));
                }

                var args = {
                    event: e,
                    currentCheck: currentCheck,
                    parent: parent,
                };
                evListener.fire("lbRiskAutoQueue_onChanged", sender, args);
            })

            riskListBox.get_checkBoxs().each(function (e) {
                var ctrlChk = this;
                var parent = riskListBox.get_parentCheckBox(ctrlChk);

                if (riskListBox.get_checked(ctrlChk)) {
                    riskListBox.set_selected(parent);

                } else {
                    riskListBox.set_unSelected(parent);
                }
            })

            riskListBox.get_items().mouseout(function (e) {
                var item = this;
                riskListBox.set_remove_focus(item);
                var chk = riskListBox.get_checkBox(item)
                var hd = riskListBox.get_hidden_state(chk);
                hd.val("");
            })

            riskListBox.get_items().click(function (e) {
                var that = this;
                var args = {
                    event: e
                }

                var isSelected = $(that).find(".unSelectedItem").length > 0
                if (isSelected) {
                    e.preventDefault();
                }

                evListener.fire("lbRiskAutoQueue_onClick", that, args);
            })

            riskListBox.oldData = riskListBox.get_item_elements_selected();
        }

        customListBox = riskListBox;
        return customListBox;
    }

    return {
        findControl: findControl,
        paramAsgmQws: paramAsgmQws
    }

})($);
