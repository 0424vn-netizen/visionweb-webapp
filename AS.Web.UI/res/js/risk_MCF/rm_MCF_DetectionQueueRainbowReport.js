var listCurrentWork = [];
var listNewWork = [];

function deleteWorkInList(targetId) {
    //var itemCurrentWork = listCurrentWork.find(function (x) { return x.targetId == targetId });
    var itemCurrentWork = null;
    for (var index = 0; index < listCurrentWork.length; ++index) {
        if (listCurrentWork[index].targetId === targetId) {
            itemCurrentWork = listCurrentWork[index];
            break;
        }
    }
    var itemNewWork = null;
    for (var index = 0; index < listNewWork.length; ++index) {
        if (listNewWork[index].targetId === targetId) {
            itemNewWork = listNewWork[index];
            break;
        }
    }
    if (itemCurrentWork) {
        listCurrentWork = listCurrentWork.filter(function (item) { return item !== itemCurrentWork });
    }
    if (itemNewWork) {
        listNewWork = listNewWork.filter(function (item) { return item !== itemNewWork });
    }
}

function ajaxRequestStart(sender, args) {
    if (args.get_eventTarget().indexOf('uxExporter') != -1) {
        args.set_enableAjax(false);
    }
}

function reloadRainbowReport() {
    var grid = $find(detectionQueueRainbowReport_uxReportGrid).get_masterTableView();
    grid.rebind();
}

function setTabActive(isGridView) {
    var isGridViewMode = isGridView == '1';
    $(".view-data-options > .btn-option").removeClass("active");
    if (isGridViewMode) {
        $(".view-data-options > .btn-option-gridview").addClass("active");
    } else {
        $(".view-data-options > .btn-option-cardview").addClass("active");
    }
}
function setExportOption(isAllFields) {
    $("#" + detectionQueue_hddExportOption).val(isAllFields);
}

function doOpenNewPopup(encodeURL) {
    return parent.ShowPopupModal(encodeURL, 'auto');
}

function getCurrentState(e, state) {
    var targetId = state == 0 ? $(e).parents("[data-selector*='wk-btn']").attr("id") :
        $(e).parents("[data-selector*='disposition-dialog']").attr("targetid");
    var itemExists = null;
    if (listCurrentWork.length > 0) {
        // itemExists = listCurrentWork.find(function (x) { return x.targetId == targetId });

        for (var index = 0; index < listCurrentWork.length; ++index) {
            if (listCurrentWork[index].targetId === targetId) {
                itemExists = listCurrentWork[index];
                break;
            }
        }
    }

    if (itemExists == null) {
        var itemCurrentWork = {
            state: -1,
            listDis: [],
            isReturn: true,
            targetId: ""
        }

        itemCurrentWork.targetId = targetId;
        itemCurrentWork.listDis = [];
        var element = $("#" + targetId);
        itemCurrentWork.state = element.find(".state-work").eq(0).attr("data-state");
        $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li")
            .each(function (index) {
                if ($(this).find("input").filter(":checked").length > 0) {
                    itemCurrentWork.listDis.push($(this).find("input").filter(":checked").val());
                }
            });

        if (state == 3) {
            var curentValue = $(e).parents(".disposition-content").find("input").val();
            var currentChecked = $(e).parents(".disposition-content").find("input").is(":checked");
            if (!currentChecked) {
                itemCurrentWork.listDis.push(curentValue);
            } else {
                if (itemCurrentWork.listDis.indexOf(curentValue) != -1) {
                    itemCurrentWork.listDis.splice(itemCurrentWork.listDis.indexOf(curentValue), 1);
                }
            }
        }

        listCurrentWork.push(itemCurrentWork);
    }
}

function getNewSate(e, state) {
    var targetId = state == 0 ? $(e).parents("[data-selector*='wk-btn']").attr("id") :
        $(e).parents("[data-selector*='disposition-dialog']").attr("targetid");
    if (targetId) {
        var itemExists = null;
        if (listNewWork.length > 0) {
            //itemExists = listNewWork.find(function (x) { return x.targetId == targetId });
            for (var index = 0; index < listNewWork.length; ++index) {
                if (listNewWork[index].targetId === targetId) {
                    itemExists = listNewWork[index];
                    break;
                }
            }
        }
        if (itemExists == null) {
            var itemNewWork = {
                state: -1,
                listDis: [],
                targetId: ""
            }

            var element = $("#" + targetId);
            itemNewWork.state = state;
            itemNewWork.targetId = targetId;
            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li")
                .each(function (index) {
                    if ($(this).find("input").filter(":checked").length > 0) {
                        itemNewWork.listDis.push($(this).find("input").filter(":checked").val());
                    }
                });
            listNewWork.push(itemNewWork);
        } else {
            itemExists.state = state;
            itemExists.listDis = [];
            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li")
                .each(function (index) {
                    if ($(this).find("input").filter(":checked").length > 0) {
                        itemExists.listDis.push($(this).find("input").filter(":checked").val());
                    }
                });
        }
    }
}

function removeDisableBtn(targetId) {
    var itemCurrentWork = null;
    for (var index = 0; index < listCurrentWork.length; ++index) {
        if (listCurrentWork[index].targetId === targetId) {
            itemCurrentWork = listCurrentWork[index];
            break;
        }
    }

    var itemNewWork = null;
    for (var index = 0; index < listNewWork.length; ++index) {
        if (listNewWork[index].targetId === targetId) {
            itemNewWork = listNewWork[index];
            break;
        }
    }

    if (itemCurrentWork && itemNewWork) {
        if (itemCurrentWork.targetId.length > 0) {
            var element = $("#" + itemCurrentWork.targetId);
            // Check not change any disposition
            var isChangeState = ((itemCurrentWork.state != itemNewWork.state.toString()) && itemNewWork.state == 1);
            if (isChangeState) {
                $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "']").find(".btn-submit-disposition").removeClass("btn-disabled");
            }
            else {
                var isHaveDis = itemNewWork.listDis.length > 0;
                var isChangeDis = JSON.stringify(itemCurrentWork.listDis.sort()) != JSON.stringify(itemNewWork.listDis.sort());
                if (isHaveDis && isChangeDis) {
                    $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "']").find(".btn-submit-disposition").removeClass("btn-disabled");
                } else {
                    $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "']").find(".btn-submit-disposition").addClass("btn-disabled");
                }
            }
        }
    }
}

function changeWorked(e) {
    if ($(e).parents(".disposition-content").find("input").prop("checked")) {
        $(e).parents(".disposition-content").find("input").prop("checked", false);
        $(e).parents(".disposition-content").find("input").change();
    } else {
        $(e).parents(".disposition-content").find("input").prop("checked", true);
        $(e).parents(".disposition-content").find("input").change();
    }
}

function changeWorkedState(e, state) {
    var targetId = state == 0 ? $(e).parents("[data-selector*='wk-btn']").attr("id") :
        $(e).parents("[data-selector*='disposition-dialog']").attr("targetid");
    if ($(e).attr("data-wip") != 3) {
        getCurrentState(e, state);
        switch (state) {
            case 0:
                var dataState = $("#" + targetId).find(".state-work").eq(0).attr("data-state");
                if (dataState == 0) {
                    buildData(e, 2, true);
                } else {
                    buildData(e, 0);
                    removeItemDisable(targetId);
                }
                break;
            case 1:
                $(e).find("input").prop("checked", true);
                workProgressClick(e, targetId);
                removeItemDisable(targetId);
                getNewSate(e, state);
                removeDisableBtn(targetId);
                break;
            case 2:
                dispositionClick(e, targetId);
                break;
            case 3:
                getNewSate(e, state);
                selectDisposition(e, targetId);
                removeDisableBtn(targetId);
                break;
            default:
                break;
        }
    }
}

function changeStyleAfterCheck(targetId, state) {
    var curentTR = $("#" + targetId).parents('tr');
    var curentTD = $("#" + targetId).parents('td');
    var merchantNameCard = curentTD.find("div[id*='merchantName']").find("a.link");
    var isCardView = $("#" + targetId).attr("iscardview");
    curentTR.children(".merchantName").find("span:first").css("border-bottom", "2px solid Transparent");
    merchantNameCard.css("border-bottom", "2px solid Transparent");

    switch (state) {
        case 0:
            var dataState = $("#" + targetId).find(".state-work").eq(0).attr("data-state");
            if (dataState == 0) {
                curentTR.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
                merchantNameCard.css("border-bottom", "2px solid #ffb6c1");
            } else if (dataState == 2) {
                if (isCardView) {
                    curentTD.removeClass("row-checked");
                }
            }
            break;
        case 1:
            if (isCardView) {
                curentTD.removeClass("row-checked");
            }
            break;
        case 2:
            curentTR.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
            merchantNameCard.css("border-bottom", "2px solid #ffb6c1");
            if (isCardView) {
                curentTD.addClass("row-checked");
            }
            break;
        case 3:
            curentTR.children(".merchantName").find("span:first").css("border-bottom", "2px solid #ffb6c1");
            merchantNameCard.css("border-bottom", "2px solid #ffb6c1");
            if (isCardView) {
                curentTD.addClass("row-checked");
            }
            break;
        default:
            break;
    }
}

function removeItemDisable(targetId) {
    $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
        var itemDisable = ($(this).attr("class"));
        if (itemDisable && itemDisable == "item-disable") {
            $(this).remove();
        }
    });
}

function returnCurrentState(targetId) {
    var itemCurrentWork = null;
    for (var index = 0; index < listCurrentWork.length; ++index) {
        if (listCurrentWork[index].targetId === targetId) {
            itemCurrentWork = listCurrentWork[index];
            break;
        }
    }

    if (itemCurrentWork && itemCurrentWork.isReturn) {
        $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "']").find(".btn-submit-disposition").addClass("btn-disabled");
        var state = parseInt(itemCurrentWork.state);
        switchWorkStatus(itemCurrentWork.targetId, state);

        $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "']").find("ul").find("li").each(function (index) {
            var val = $(this).find("input").val();
            if (itemCurrentWork.listDis.indexOf(val) != -1) {
                $(this).find("input").prop("checked", true);
            } else {
                $(this).find("input").prop("checked", false);
            }
        });

        if (state == 1) {
            $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "'] .rd-work").prop("checked", true);
            $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "'] .rd-dis").prop("checked", false);
        } else if (state == 2) {
            $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "'] .rd-work").prop("checked", false);
            $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "'] .rd-dis").prop("checked", true);
        } else {
            $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "'] .rd-work").prop("checked", false);
            $(".disposition-dialog[targetid='" + itemCurrentWork.targetId + "'] .rd-dis").prop("checked", false);
        }
    }
}

function returnCurrentWork(targetId, message) {
    //var itemCurrentWork = listCurrentWork.find(function (x) { return x.targetId == targetId });
    var itemCurrentWork = null;
    for (var index = 0; index < listCurrentWork.length; ++index) {
        if (listCurrentWork[index].targetId === targetId) {
            itemCurrentWork = listCurrentWork[index];
            break;
        }
    }

    if (itemCurrentWork) {
        var state = parseInt(itemCurrentWork.state);
        switchWorkStatus(targetId, state);

        $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
            var val = $(this).find("input").val();
            if (itemCurrentWork.listDis.indexOf(val) != -1) {
                $(this).find("input").prop("checked", true);
            }
        });
        if (state == 1) {
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", true);
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
        } else if (state == 2) {
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", false);
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", true);
        } else {
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", false);
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
        }
        if (message) {
            $("#" + hddMessage_ClientID).val(message.toString());
        }

        $get(uxOpenWarning_ClientID).click();
    }
}

function switchWorkStatus(targetId, WorkStateID) {
    var element = $("#" + targetId);
    switch (WorkStateID) {
        case 0:
            element.find(".work-status")[0].innerText = lblWorkStatus;
            element.find(".work-status").eq(0).attr("class", "work-status");
            element.find(".work-ico").eq(0).attr("class", "work-ico");
            element.find(".state-work").eq(0).attr("data-state", 0);
            if ($(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").length == 0) {
                $("#" + targetId).addClass("wk-selected");
            }
            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function () {
                $(this).find("input").prop("checked", false);
            });
            break;
        case 1:
            element.find(".work-status")[0].innerText = lblWorkInProgressStatus;
            element.find(".work-status").eq(0).attr("class", "work-status txt-wip");
            element.find(".work-ico").eq(0).attr("class", "work-ico icon-warning");
            element.find(".state-work").eq(0).attr("data-state", 1);
            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function () {
                $(this).find("input").prop("checked", false);
            });
            break;
        case 2:
            element.find(".work-status")[0].innerText = lblWorkedStatus;
            element.find(".work-status").eq(0).attr("class", "work-status txt-wkd");
            element.find(".work-ico").eq(0).attr("class", "work-ico icon-check");
            element.find(".state-work").eq(0).attr("data-state", 2);
            break;
        default:
            break;
    }
}

function changeMerchantWorked(e, targetId) {

    if ($(e).attr("data-state") != "0") {
        switchWorkStatus(targetId, 0);
        removeDefault(targetId);
    } else {
        switchWorkStatus(targetId, 2);
    }
    removeDefault(targetId);
}

function workPopBeforeShow(sender, args) {
    sender.updateLocation();
    sender._adjustCallout();
    $("#" + sender._targetControlID).addClass("tooltipactive");
    returnCurrentState(sender._targetControlID);
    setDispositionByTargetID(sender._targetControlID);
}

function workPopBeforeHide(sender, args) {
    $("#" + sender._targetControlID).removeClass("tooltipactive");
    $(".disposition-dialog[targetid='" + sender._targetControlID + "']").find(".btn-submit-disposition").addClass("btn-disabled");
}

function workPopShow(sender, args) {
    //setDispositionByTargetID(sender._targetControlID);
}

function wipOtherShow(sender, args) {
    var targetId = $(sender._element)[0].id;
    $("#RadToolTipWrapper_" + targetId).addClass("active");
}

function wipOtherHideShow(sender, args) {
    var targetId = $(sender._element)[0].id;

    $("#RadToolTipWrapper_" + targetId).removeClass("active");
}

function openWarning(message) {
    parent.ShowPopupModal(rootURL + 'Risk_MCF/rm_MCF_WorkWarning.aspx?' + message, 'auto');
}

function setDispositionByTargetID(targetId) {

    if ($("#" + targetId).hasClass("wk-selected")) {
        var workingMerchantID = $("#" + targetId).attr("workingMerchantID");
        var reportDate = $("#" + targetId).attr("reportdate");
        var dataState = $("#" + targetId).find(".state-work").eq(0).attr("data-state");
        if (dataState == 2) {
            //   $(".disposition-dialog[targetid='" + targetId + "']").addClass("loadding");
            //   $.when(checkDisposition(workingMerchantID, reportDate)).done(function (data) {
            //  rebindDispositionValue(JSON.parse(data.d).Table, targetId);

            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
                if ($(this).find("input").attr("isdefault")) {
                    $(this).find("input").prop("checked", true);
                }
            });

            //   $(".disposition-dialog[targetid='" + targetId + "']").removeClass("loadding");
            //   });
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", true);
            $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
                if ($(this).find("input").attr("isdefault")) {
                    $(this).find("input").prop("checked", true);
                }
            });
        } else if (dataState == 0) {
            switchWorkStatus(targetId, 0);
            $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
        }
        $("#" + targetId).removeClass("wk-selected")
    }
}

function removeDefault(targetId) {
    $(".disposition-dialog[targetid='" + targetId + "'] .rd-work").prop("checked", false);
    $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", false);
    $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
        if ($(this).find("input").attr("isdefault")) {
            $(this).find("input").prop("checked", false);
        }
    });
}

function dispositionClick(e, targetId, isSave) {
    var workingMerchantID = $("#" + targetId).attr("WorkingMerchantID");
    var reportDate = $("#" + targetId).attr("reportdate");
    // $(".disposition-dialog[targetid='" + targetId + "']").addClass("loadding");
    if (isSave) {
        switchWorkStatus(targetId, 2);
    }

    if ($(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").length > 0) {
        //  $.when(checkDisposition(workingMerchantID, reportDate)).done(function (data) {
        //   $(".disposition-dialog[targetid='" + targetId + "']").removeClass("loadding");
        //  rebindDispositionValue(JSON.parse(data.d).Table, targetId);
        $(".disposition-dialog[targetid='" + targetId + "'] .rd-dis").prop("checked", true);
        $(".disposition-dialog[targetid='" + targetId + "']").find("ul").find("li").each(function (index) {
            $(this).find("input").prop("checked", false);
            if ($(this).find("input").attr("isdefault")) {
                $(this).find("input").prop("checked", true);
            }
        });
        getNewSate(e, 2);
        removeDisableBtn(targetId);
        //  });
    } else {
        $("#" + targetId).addClass("wk-selected");
    }
}

function workProgressClick(e, targetId, isSave) {
    if (isSave) {
        switchWorkStatus(targetId, 1);
    }
    $(e).parents(".disposition-dialog").find("ul").find("li").each(function (index) {
        $(this).find("input").prop("checked", false);
    });
}

function rebindDispositionValue(dataSource, targetId) {
    var element;
    dataSource.forEach(function (item) {
        if (item) {
            element = $(".disposition-dialog[targetid='" + targetId + "']").find("ul").
                find("li").find("span").filter(function () {
                    return $(this).text() == item[1];
                })
            if (element.length > 0) {
                if (item[2]) {
                    element.parent().find("input").attr("isdefault", "true");

                } else {
                    element.parent().find("input").removeAttr("isdefault");
                }
            }
        }
    })
}

function selectDisposition(e, targetId, isSave) {
    //var itemNewWork = listNewWork.find(function (x) { return x.targetId == targetId });
    var itemNewWork = null;
    for (var index = 0; index < listNewWork.length; ++index) {
        if (listNewWork[index].targetId === targetId) {
            itemNewWork = listNewWork[index];
            break;
        }
    }

    if (itemNewWork) {
        if (itemNewWork.listDis.length > 0) {
            if (isSave) {
                switchWorkStatus($(e).parents(".disposition-dialog").attr("targetid"), 2);
            }
            $(e).parents(".disposition-dialog").find(".rd-dis").last().prop("checked", true);
        } else {
            if (isSave) {
                switchWorkStatus($(e).parents(".disposition-dialog").attr("targetid"), 0);
            }
            $(e).parents(".disposition-dialog").find(".rd-dis").last().prop("checked", false);
        }
    }
}

function updateSummary(assignmentID, reportDate, applyFilterId_value) {
    $.when(refreshAssignmentSummary(assignmentID, reportDate, applyFilterId_value)).done(function (data) {
        var dataRow = (JSON.parse(data.d));
        var eligible = $("#" + pnlAssignmentList_ClientID).find(".eligible");
        var alertCount = $("#" + pnlAssignmentList_ClientID).find(".alertCount");
        var merchantVolume = $("#" + pnlAssignmentList_ClientID).find(".merchantVolume");
        var wipCount = $("#" + pnlAssignmentList_ClientID).find(".wipCount");
        var wipVolume = $("#" + pnlAssignmentList_ClientID).find(".wipVolume");
        var workedCount = $("#" + pnlAssignmentList_ClientID).find(".workedCount");
        var workedVolume = $("#" + pnlAssignmentList_ClientID).find(".workedVolume");
        var requeuedCount = $("#" + pnlAssignmentList_ClientID).find(".requeuedCount");
        var requeuedVolume = $("#" + pnlAssignmentList_ClientID).find(".requeuedVolume");

        if (eligible.length > 0) {
            eligible[0].innerText = dataRow[0].TotalMerchantCount ? dataRow[0].TotalMerchantCount : 'N/A';
        }
        if (alertCount.length > 0) {
            alertCount[0].innerText = dataRow[0].AlertMerchantCount;
        }
        if (wipCount.length > 0) {
            wipCount[0].innerText = dataRow[0].WIPCount;
        }
        if (workedCount.length > 0) {
            workedCount[0].innerText = dataRow[0].WorkedCount;
        }
        if (requeuedCount.length > 0) {
            requeuedCount[0].innerText = dataRow[0].RequeueCount ? dataRow[0].RequeueCount : 'N/A';
        }
        var worked_count = parseInt(dataRow[0].WorkedCount);
        var alertCount = parseInt(dataRow[0].AlertMerchantCount);
        resetProgressBar(worked_count, alertCount);
    })
}

function saveDisposition(e) {
    var targetId = $(e).parents("[data-selector*='disposition-dialog']").attr("targetid");
    var newState = -1;
    var element = $("#" + targetId);
    var cycleid = element.attr("cycleid");
    var parentCycleID = element.attr("parentcycleid");
    var merchantNumber = element.attr("merchantnumber");
    var assignmentID = element.attr("assignmentid");
    var reportDate = element.attr("reportdate");
    var dispositionList = "";
    // var itemNewWork = listNewWork.find(function (x) { return x.targetId == targetId });

    var itemNewWork = null;
    for (var index = 0; index < listNewWork.length; ++index) {
        if (listNewWork[index].targetId === targetId) {
            itemNewWork = listNewWork[index];
            break;
        }
    }
    if (itemNewWork) {

        var newState = itemNewWork.state;
        if (newState == 3 || newState == 2) {
            newState = 2;
            dispositionList = itemNewWork.listDis.join();
        }

        $(".disposition-dialog[targetid='" + targetId + "']").addClass("loadding");
        $.when(updateDisposition(targetId, cycleid, parentCycleID, merchantNumber, assignmentID, reportDate, dispositionList, newState)).done(function (data) {
            var dataRow = (JSON.parse(data.d).Table)[0];
            if (dataRow[0] == 0) {
                $("#" + targetId).parents("tr").find(".item-worked")[0].innerText = dataRow[3];
                $("#" + targetId).parents("tr").find(".item-para-worked")[0].innerText = dataRow[2];
                if ($("#" + targetId).parents("tr").find("[data-selector*='disposition']").length > 0) {
                    $("#" + targetId).parents("tr").find("[data-selector*='disposition']")[0].innerText = dataRow[4];
                    $("#" + targetId).parents("tr").find("[data-selector*='disposition']").attr("title", dataRow[4]);
                }
                changeStyleAfterCheck(targetId, itemNewWork.state);
                switch (itemNewWork.state) {
                    case 0:
                        changeMerchantWorked(e, targetId);
                        break;
                    case 1:
                        workProgressClick(e, targetId, true);
                        break;
                    case 2:
                        dispositionClick(e, targetId, true);
                        break;
                    case 3:
                        selectDisposition(e, targetId, true);
                        break;
                }

                //updateSummary(assignmentID, reportDate, applyFilterId_value);
                deleteWorkInList(targetId);
                closeTooltip(targetId);
            } else {
                returnCurrentWork(targetId, dataRow[1]);
                deleteWorkInList(targetId);
            }

            $(".disposition-dialog[targetid='" + targetId + "']").removeClass("loadding");
        });
    }
}

function buildData(e, state, isWk) {
    var targetId = (state == 0 || isWk) ? $(e).parents("[data-selector*='wk-btn']").attr("id") :
        $(e).parents("[data-selector*='disposition-dialog']").attr("targetid");

    var newState = -1;
    var element = $("#" + targetId);
    var cycleid = element.attr("cycleid");
    var parentCycleID = element.attr("parentcycleid");
    var merchantNumber = element.attr("merchantnumber");
    var assignmentID = element.attr("assignmentid");
    var reportDate = element.attr("reportdate");
    var dispositionList = "";
    var newState = state;
    if (!$(".disposition-dialog[targetid='" + targetId + "']").hasClass("loadding")) {
        $(".disposition-dialog[targetid='" + targetId + "']").addClass("loadding");
        $.when(updateDisposition(targetId, cycleid, parentCycleID, merchantNumber, assignmentID, reportDate, dispositionList, newState)).done(function (data) {
            var dataRow = (JSON.parse(data.d).Table)[0];
            if (dataRow[0] == 0) {
                $("#" + targetId).parents("tr").find(".item-worked")[0].innerText = dataRow[3];
                $("#" + targetId).parents("tr").find(".item-para-worked")[0].innerText = dataRow[2];
                if ($("#" + targetId).parents("tr").find("[data-selector*='disposition']").length > 0) {
                    $("#" + targetId).parents("tr").find("[data-selector*='disposition']")[0].innerText = dataRow[4];
                    $("#" + targetId).parents("tr").find("[data-selector*='disposition']").attr("title", dataRow[4]);
                }
                changeStyleAfterCheck(targetId, state);
                switch (state) {
                    case 0:
                        changeMerchantWorked(e, targetId);
                        break;
                    //case 1:
                    //    workProgressClick(e, targetId, true);
                    //    break;
                    case 2:
                        dispositionClick(e, targetId, true);
                        break;
                    //case 3:
                    //    selectDisposition(e, targetId);
                    //    break;
                }

                //updateSummary(assignmentID, reportDate, applyFilterId_value);
                deleteWorkInList(targetId);
                closeTooltip(targetId);
            } else {
                returnCurrentWork(targetId, dataRow[1]);
                deleteWorkInList(targetId);
            }

            $(".disposition-dialog[targetid='" + targetId + "']").removeClass("loadding");
        });
    }
}

function checkDisposition(WorkingMerchantID, reportDate) {
    var url = "rm_MCF_DQNextQWebMethod.aspx/CheckDisposition";
    return $.ajax({
        type: "POST",
        url: url,
        data: '{"Id":"' + WorkingMerchantID + '",reportDate:"' + reportDate + '"}',
        contentType: "application/json; charset=utf-8",
    });
}

function updateDisposition(targetId, cycleid, parentCycleID, merchantNumber, assignmentID, reportDate, dispositionList, newState) {
    // var itemCurrentWork = listCurrentWork.find(function (x) { return x.targetId == targetId });
    var itemCurrentWork = null;
    for (var index = 0; index < listCurrentWork.length; ++index) {
        if (listCurrentWork[index].targetId === targetId) {
            itemCurrentWork = listCurrentWork[index];
            break;
        }
    }
    itemCurrentWork.isReturn = false;

    if (dispositionList.length == 0 && newState == 2 && itemCurrentWork.state != 0) {
        newState = 0;
    }
    var url = "rm_MCF_DQNextQWebMethod.aspx/UpdateDisposition";
    return $.ajax({
        type: "POST",
        url: url,
        data: '{"dispositionList":"' + dispositionList +
            '",workStateID:"' + newState +
            '",feWorkStateID:"' + itemCurrentWork.state +
            '",cycleID:"' + cycleid +
            '",ParentCycleID:"' + parentCycleID +
            '",reportDate:"' + reportDate +
            '",assignmentID:"' + assignmentID +
            '",merchantNumber:"' + merchantNumber +
            '",viewCode:"' + "DQ" + '"}',
        contentType: "application/json; charset=utf-8",
    });
}

function refreshAssignmentSummary(assignmentID, reportDate, applyFilterId) {
    var url = "rm_MCF_DQNextQWebMethod.aspx/GetAssignmentsForDetectionQueue";
    return $.ajax({
        type: "POST",
        url: url,
        data: '{"assignmentID":"' + assignmentID +
            '",reportDate:"' + reportDate +
            '",applyFilterId:"' + applyFilterId + '"}',
        contentType: "application/json; charset=utf-8",
    });
}

function ChangeWorkedStatusOption(obj, option) {
    var isRebinGrid = $(obj).hasClass('btn-work-selected');
    $('#uxWorkedNotWorked .btn-work-js').removeClass('btn-work-selected');
    $(obj).addClass('btn-work-selected');
    if (!isRebinGrid) {
        $get(rm_MCF_DetectionQueueRainbowReport_filterWorkingStatus).value = option;
        $get(rm_MCF_DetectionQueueRainbowReport_btnFilterWorkingStatus).click();
    }
}

function disableRQAll() {
    $("#" + detectionQueueRainbowReport_uxReportGrid).find("input[id*=chkHeader]").prop("checked", false);
    $("#" + detectionQueueRainbowReport_uxReportGrid).find("input[id*=chkHeader]").attr("disabled", true);
}

function refreshDataEvent() {
    parent.HidePopupModal();
    $get(rm_MCF_DetectionQueueRainbowReport_btnRefreshRainbow).click();
}

function closeCurrentDisposition() {
    var currentToolTip = Telerik.Web.UI.RadToolTip.getCurrent();
    if (currentToolTip) {
        var element = currentToolTip.get_element();
        var id = ($(element)[0].id);
        $("#RadToolTipWrapper_" + id).hide();
    }
}

function closeTooltip(targetId) {
    var activeTooltip = Telerik.Web.UI.RadToolTip.getCurrent();
    if (activeTooltip) {
        if (activeTooltip._targetControlID == targetId) {
            activeTooltip.hide();
        }
    }
}

function RequeueAllMerchants(chk, isWQButtonOnly, filterWorkingStatus) {
    var actionUrl = rootURL + "Risk_MCF/rm_MCF_DQBarometerReportPopup.aspx/UpdateAllRequeuedMerchant";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"status":"' + chk.checked + '","filterWorkingStatus":"' + filterWorkingStatus + '","applyFilteredId":"' + applyFilteredId + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var requeuedCount = result.d[0];
            var requeuedVolume = result.d[1];
            var isWorkQueued = result.d[2];

            //hide requeue button
            if (parseInt(requeuedCount) == 0) {
                $('#btnRequeueBoundary').addClass('hide');
                $('#divRemoveWorkQueue').addClass('hide');
            }
            else if (parseInt(requeuedCount) > 0 && isWorkQueued.toLowerCase() == 'true') {
                $('#divRemoveWorkQueue').removeClass('hide');
            }
        }
    });
    doHeaderCheck(chk, isWQButtonOnly);
}

function doHeaderCheck(sender, isWQButtonOnly) {
    if (sender.checked) {
        $("input[id*=chkItem]:not([rqwip*='other'])").prop("checked", true);
        $("input[id*=chkItem]:not([rqwip*='other'])").prop("disabled", true);
        $('#btnRequeueBoundary').removeClass('hide');
        if (isWQButtonOnly) {
            $('#divRemoveWorkQueue').addClass('hide');
        }
    } else {
        $("input[id*=chkItem]:not([rqwip*='other'])").prop("checked", false);
        $("input[id*=chkItem]:not([rqwip*='other'])").prop("disabled", false);

        $('#btnRequeueBoundary').addClass('hide');
        $('#divRemoveWorkQueue').addClass('hide');
    }
}

function RequeueSingleMerchant(chk, isWQButtonOnly, cycleId, parentCycleID) {
    var actionUrl = rootURL + "Risk_MCF/rm_MCF_DQBarometerReportPopup.aspx/UpdateRequeuedMerchant";
    $.ajax({
        type: "POST",
        async: false,
        url: actionUrl,
        data: '{"status":"' + chk.checked + '","merchantNumber":"' + chk.value + '","cycleId":"' + cycleId + '","parentCycleID":"' + parentCycleID + '","applyFilteredId":"' + applyFilteredId + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var requeuedCount = result.d[0];
            var requeuedVolume = result.d[1];
            var isAllSelected = result.d[2];
            var merchantIsWorkQueued = result.d[3];

            //hide requeue button
            if (parseInt(requeuedCount) == 0) {
                $('#btnRequeueBoundary').addClass('hide');
                $('#divRemoveWorkQueue').addClass('hide');
            }
            else if (parseInt(requeuedCount) > 0 && merchantIsWorkQueued.toLowerCase() == 'true' && !isWQButtonOnly) {
                $('#divRemoveWorkQueue').removeClass('hide');
            }
            else if (merchantIsWorkQueued.toLowerCase() == 'false')
                $('#divRemoveWorkQueue').addClass('hide');

            //if (isAllSelected.toLowerCase() == 'true') {
            //    $("input[id*=chkHeader]").prop("checked", true);
            //    $("input[id*=chkHeader]").prop("disabled", true);
            //}
            //else {
            //    $("input[id*=chkHeader]").prop("disabled", false);
            //}
        }
    });
    doItemCheck(chk, isWQButtonOnly);
}

function doItemCheck(sender, isWQButtonOnly) {
    if (!sender.checked) {
        $("input[id*=chkHeader]").prop("checked", false);
    }
    else if (isWQButtonOnly || sender.checked) {
        $('#btnRequeueBoundary').removeClass('hide');
    }
}

function RebindAndShowStausWhenCloseModal() {
    HidePopupModal();
    $('#btnRequeueBoundary').addClass('hide');
    $('#divRemoveWorkQueue').addClass('hide');
    $("input[id*=chkHeader]").prop("checked", false);
    //$("input[id*=chkHeader]").prop("disabled", true);
    $get(rm_MCF_DetectionQueueRainbowReport_btnRebindReportGrid).click();
}

function RebindGrid_CustomViewChange() {
    $get(rm_MCF_DetectionQueueRainbowReport_btnFilterWorkingStatus).click();
}

function ReloadHover() {
    $('[data-hover="dropdown"]').dropdownHover();
}

function gridDatabound() {
    var isCheckall = true;
    var checkbox = null
    if ($("#" + rm_MCF_DQRainbow_optCard).is(":checked")) {
        checkbox = $("#" + rm_MCF_DQRainbow_chkHeaderCV);
        $("#" + detectionQueueRainbowReport_uxReportGrid + " .rgMasterTable").find("input[id*='chkItemCV']:not([rqwip*='other'])").each(function () {
            if (!$(this).is(":checked"))
                isCheckall = false;
        });
    }
    else {
        checkbox = $("#" + detectionQueueRainbowReport_uxReportGrid + " thead th").find("input[id*='chkHeader']");

        $("#" + detectionQueueRainbowReport_uxReportGrid + " tbody tr").each(function () {
            //var height = $(this).height();
            var td = $(this).find('.item-worked');
            if (td && td.length > 0) {
                //$(td[0]).height(height);
                var chkitem = $(this).find("input[id*=chkItem]:not([rqwip*='other'])");
                if (chkitem && chkitem.length > 0 && !chkitem[0].checked)
                    isCheckall = false;
            }
        });
    }
    if (!isCheckall)
        checkbox.prop("checked", false);
}

$(document).ready(function () {
    LoadAssignmentInfo();
});

function LoadAssignmentInfo() {
    $get(rm_MCF_DetectionQueueRainbowReport_uxReloadAssignment).click();
}
