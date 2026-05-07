

//==================================== API =====================================================
var countGlobal = 0;
function doSave() {
    if (!onCheckBeforSave())
        return false;

    var pm = checkParameterValid();
    if (!pm)
        return false;

    //do postback
    var bt = $get(Risk_Parameter_btSave);
    bt.click();
}

function doSaveParameterList(isNeedSelectParam) {
    if (isNeedSelectParam) {
        var pm = checkParameterValid();
        if (!pm)
            return false;
    }
    //do postback
    var bt = $get(Risk_Parameter_btSave);
    bt.click();
}

function checkParameterValid() {
    if (countGlobal == 0) {
        alert(Risk_Parameter_js_ParameterMustBeSelected);
        return false;
    }
    return true;
}
function SetHeaderUxParameterList() {
    var arrayCol = [];
    var groupAlert = true;
    var tableHeader = $('#' + uxParameterListID).find("thead tr:last-child th");

    for (var i = 0; i < tableHeader.length; i++) {

        var headerStyle = tableHeader.eq(i).attr("style");
        var headerTitle = tableHeader.eq(i).attr("title");

        if (headerStyle.replace(/\s/g, '').indexOf("display:none") == -1) {
            if (headerTitle) {
                if (headerTitle.indexOf(Param_js_Indicator) == -1) {
                    if (headerTitle.indexOf(Param_js_Threshold) == -1) {
                        arrayCol.push(['', 1, 'rgHeader mh border-right end-border']);
                    }
                } else {
                    if (groupAlert === true) {
                        arrayCol.push([Param_js_Alert, 2, 'rgHeader mh']);
                        groupAlert = false;
                    } else {
                        arrayCol.push([Param_js_ReAlert, 2, 'rgHeader mh']);
                    }
                }
            } else {
                arrayCol.push(['', 1, 'rgHeader mh border-right end-border']);
            }
        }
    }
    addGroupHeadersForStaticRadGridParameter(uxParameterListID,
       arrayCol, true);

}

function addGroupHeadersForStaticRadGridParameter(gridId, headers, isIngoreColNone, isInsertMoreRows) {
    var table;
    var asGrid = $('#' + gridId);
    if (!isInsertMoreRows)
        isInsertMoreRows = false;

    if (asGrid != null && asGrid.length > 0) {
        var rgHeaderWrapper = asGrid.children('.rgHeaderWrapper');
        if (rgHeaderWrapper != null && rgHeaderWrapper.length > 0) {
            table = document.getElementById(gridId + '_ctl00_Header');
        }
        else {
            table = document.getElementById(gridId + '_ctl00');
        }
    }
    else {
        table = document.getElementById(gridId + '_ctl00');
    }
    if (table == null) {
        return;
    }

    if (table.hasModified) {
        return;
    }

    if (!isInsertMoreRows) {
        table.hasModified = true;
    }

    addHeaderCell = function (row) {
        var th = document.createElement('th');
        th.setAttribute('scope', 'col');
        return row.appendChild(th);
    };
    var row = table.insertRow(0),
        cell = null,
        cellIndex = 1;

    var tableHeader = $(table).find("thead tr:last-child th");
    var childHeader = $(table).find("thead tr:last-child");
    $(childHeader).find("th:first-child").addClass('start-border');
    $(childHeader).find("th:last-child").addClass('end-border');
    $(childHeader).find("th:nth-last-child(2)").addClass('end-border');
    if (!isInsertMoreRows) {
        $(childHeader).find("th:first-child").addClass('border-right');
    }

    if (isIngoreColNone) {
        if (isIngoreColNone === true) {
            var arrDisplay = [];
            var headerIndex = 0;
            for (var j = 0; j < tableHeader.length; j++) {
                if (tableHeader.eq(j).attr("style").replace(/\s/g, '').indexOf("display:none") == -1) {
                    j += headers[headerIndex][1] - 1;
                    arrDisplay.push(j);
                    headerIndex++;
                }
            }
        }
    }


    for (var i = 0; i < headers.length; i++) {

        cell = addHeaderCell(row);
        var colSpan = headers[i][1],
            num = cellIndex + (colSpan - 1),
            startHeader = $(table).find("thead tr:last-child th:nth-child(" + (cellIndex - 1) + ")"),
            endHeader = $(table).find("thead tr:last-child th:nth-child(" + num + ")");

        cell.innerHTML = headers[i][0];
        cell.setAttribute('colSpan', colSpan);
        if (isIngoreColNone) {
            if (isIngoreColNone === true) {
                if ((i < headers.length - 1 ? headers[i + 1][1] != 1 : true) ||
                      i != 0) {
                    tableHeader.eq(arrDisplay[i]).addClass('end-border');
                }
            } else {
                $(startHeader).addClass('end-border');
                $(endHeader).addClass('end-border');
            }
        } else if (colSpan > 1) {
            $(startHeader).addClass('end-border');
            $(endHeader).addClass('end-border');
        } else if (!isInsertMoreRows) {
            $(startHeader).addClass('end-border');
            $(endHeader).addClass('end-border');
        }

        cell.className = headers[i][2] + (colSpan > 1 ? " group-header" : "");
        cell.style.textAlign = 'center';
        cell.setAttribute('align', 'center');
        cellIndex += colSpan;

    }

    $('.group-header').first().prev().addClass("end-border");

}

function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    SetHeaderUxParameterList();
    UxExporter_OnResponseEnd(sender, args);
}

function onCheckBeforSave() {
    var count = 0;
    var isErrrorMasster = true;
    //variable save first Alert 
    var IsSaveTemp = false;
    for (var i = 0; i < ParameterInfo.length; i++) {
        var isError = true;
        var isMCF = i % 2 != 0;
        var paramInfo = ParameterInfo[i];
        var cb = $get(paramInfo.CbClientId);

        if (cb.checked) {
            count++;
            if (!isMCF) {
                var indClientId = $get(paramInfo.IndClientId);
                if (indClientId) {
                    var isErrortemp = checkInputValue(indClientId, isMCF, true, true);
                    if (isErrortemp) {
                        removemessage(indClientId);
                    }
                    if (isError && !isErrortemp)
                        isError = isErrortemp;
                }
                else {
                    //from -> to
                    var indFromID = $get(paramInfo.IndFromID);
                    var indToID = $get(paramInfo.IndToID);
                    if (indFromID || indToID) {
                        var isErrortemp = checkInputValue(indFromID, isMCF, true);
                        if (isErrortemp)
                            isErrortemp = checkInputValue(indToID, isMCF, true);
                        if (isErrortemp) {
                            removemessage(indFromID);
                        }
                        if (isError && !isErrortemp)
                            isError = isErrortemp;
                    }
                }

                var thsClientId = $get(paramInfo.ThsClientId);
                if (thsClientId) {
                    var isErrortemp = checkInputValue(thsClientId, isMCF, true, true);
                    if (isErrortemp) {
                        removemessage(thsClientId);
                    }
                    if (isError && !isErrortemp)
                        isError = isErrortemp;
                }
                else {
                    //low -> high
                    var thsHighID = $get(paramInfo.ThsHighID);
                    var thsLowID = $get(paramInfo.ThsLowID);
                    if (thsHighID || thsLowID) {
                        var isErrortemp = checkInputValue(thsLowID, isMCF, true);
                        if (isErrortemp)
                            isErrortemp = checkInputValue(thsHighID, isMCF, true);
                        if (isErrortemp) {
                            removemessage(thsHighID);
                        }
                        if (isError && !isErrortemp)
                            isError = isErrortemp;
                    }
                }
            }
            else//check NRT
                isError = checkValidationReAlert(paramInfo, ParameterInfo[i - 1]);

            var parameterkey = $(cb).parents("tr").find("td.parameterkey")[0];
            var parameter = $(cb).parents("tr").find("td.parameter")[0];
            //if have error set bold and red Parameter and ParameterKey
            if (!isError) {
                $(parameterkey).addClass("red font-weight-bold")
                $(parameter).addClass("red font-weight-bold")
            }
            else if (i % 2 == 0) {
                IsSaveTemp = true
            }
            else if (IsSaveTemp) {
                $(parameterkey).removeClass("red font-weight-bold")
                $(parameter).removeClass("red font-weight-bold")
                IsSaveTemp = false;
            }
            if (isErrrorMasster && !isError) {
                isErrrorMasster = isError;
                moveTo("#" + $(cb).parents("tr").attr("id"));
            }
        }
    }
    countGlobal = count;
    return isErrrorMasster;
}
function removemessage(id) {
    $($(id).parents("td").find("div[id='msgError']")[0]).html("");
}

function checkValidationReAlert(paramInfoNRT, paramInfo) {
    //check realert
    var indicatorNRT = $get(paramInfoNRT.IndClientId);
    var thsClientIdeNRT = $get(paramInfoNRT.ThsClientId);
    var indicator = $get(paramInfo.IndClientId);
    var thsClientIde = $get(paramInfo.ThsClientId);

    //check if indicator or threshold orther null
    if (!checkValueNullOrEmpty($(indicatorNRT).val()) || !checkValueNullOrEmpty($(thsClientIdeNRT).val())) {
        var checkInputIndicatorMCF = indicatorNRT ? checkInputValue(indicatorNRT, true, true) : false;
        var errorIn = checkvalidation(indicatorNRT, indicator, thsClientIde, true);
        if (errorIn && checkInputIndicatorMCF)
            removemessage(indicatorNRT);

        var checkInputThresholdMCF = thsClientIdeNRT ? checkInputValue(thsClientIdeNRT, true, true) : true;
        var errorTh = checkvalidation(thsClientIdeNRT, indicator, thsClientIde, false);
        if (errorTh && checkInputThresholdMCF)
            removemessage(thsClientIdeNRT);
        if (!errorIn || !checkInputIndicatorMCF || !errorTh || !checkInputThresholdMCF)
            return false;
    }
    removemessage(indicatorNRT);
    removemessage(thsClientIdeNRT);
    return true;
}
function checkvalidation(parameterNRT, parameterIn, parameterTh, isIndicator) {
    if (parameterNRT == null || $(parameterNRT) == null || $(parameterNRT) == undefined)
        return true
    var parameterValueNRT = getValueNumber(parameterNRT);
    var parameterValueIn = getValueNumber(parameterIn);
    var parameterValueTh = getValueNumber(parameterTh);
    if (parameterValueIn === 0 && parameterValueTh === 0 && getValueNumber(parameterNRT) !== 0) {
        showMessageError(parameterNRT, true, "", RiskParameter_js_Threshold_ReAlertCM);
        return false
    }
    var parameter = isIndicator ? parameterValueIn : parameterValueTh;
    if (parameter === 0 && parameterValueNRT !== 0) {
        if (isIndicator)
            showMessageError(parameterNRT, true, "", RiskParameter_js_Indicator_ReAlert01);
        else
            showMessageError(parameterNRT, true, "", RiskParameter_js_Threshold_ReAlert01);
        return false
    }
    if (parameter > 0 && parameterValueNRT === "") {
        if (isIndicator)
            showMessageError(parameterNRT, true, "", RiskParameter_js_Indicator_ReAlert02);
        else
            showMessageError(parameterNRT, true, "", RiskParameter_js_Threshold_ReAlert02);
        return false
    }
    return checkMinMax(parameterNRT, true, true);;
}

function checkValueNullOrEmpty(para) {
    if (para == null || para == undefined || para == "")
        return true
    return false

}
function getValueNumber(parameter) {
    var val = "";
    if (parameter == null || parameter == undefined)
        return val;
    val = $(parameter).val();
    if (checkValueNullOrEmpty(val))
        return "";
    var currentValueStr = removeCommas(checkStartWithCommas(val));
    var currentValue = (currentValueStr.trim().length == 0) ? "" : parseFloat(currentValueStr, 10);
    return currentValue;
}
function moveTo(id) {
    $('html, body').animate({
        scrollTop: $(id).offset().top + 'px'
    }, 'fast');
}
$(document).ready(function () {
    SetHeaderUxParameterList();
});
