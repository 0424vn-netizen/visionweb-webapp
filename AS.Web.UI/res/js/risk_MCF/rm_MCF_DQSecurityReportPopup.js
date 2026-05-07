function OpenFlagColorTable() {
    ShowPopupModal("rm_MCF_ColorLegend.aspx", 'auto')
    return false;
}
function OpenInstanceWindow(url, name) {
    var width = screen.availWidth * 0.9;
    var height = screen.availHeight * 0.8;
    var left = (screen.availWidth / 2) - (width / 2);
    var top = (screen.availHeight / 2) - (height / 2);
    var pwin = window.open(url, "_blank", "scrollbars=1,menubar=0,toolbar=0,resizable=1,location=0,width=" + width.toString() + ",height=" + height.toString() + ",left=" + left.toString() + ",top=" + top.toString(), true);
    if (pwin != null) {
        pwin.focus();
    }
}

function MerchantNumberClick(merchantNumber, totalvalue, reportDate, assignmentID) {

    var data = {
        "status": "true",
        "merchantNumber": merchantNumber,
        "reportDate": reportDate,
        "assignmentID": assignmentID
    };

    var actionUrl = rootURL + "risk_MCF/rm_MCF_DQSecurityReportPopup.aspx/MerchantNumberClick";
    $.ajax({
        type: "post",
        url: actionUrl,
        async: false,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var url = result.d[0];
            var reloadRainbow = result.d[1].toLowerCase() === 'true';
            if (reloadRainbow)
                reloadRainbowReport();

            openPopupWindow(url, 'RiskReport');
        },
        error: function (result) {
            console.log(result.responseText);
        }

    });
}

//Check ajax ManageAssignment UC
function masterAjax_responseEnd(sender, args) {
    //SetHeaderGrid();
    if (typeof initAllProgressBar == 'function') {
        initAllProgressBar();
    }
}

function PageChanged(index) {
    var url = "rm_MCF_DQSecurityReportPopup.aspx/GetTransaction";    
    $("#uxProgress").show();
    var data = {};
    data["pageIndex"] = index;
    data["reportDate"] = rm_MCF_UxFlatReport_ReportDate;
    data["isCSViewFullCard"] = rm_MCF_UxFlatReport_IsCSViewFullCard;

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {            
            var data = result.d[0];
            var pager = result.d[1];

            $("#uxProgress").hide();
            $("#transactionTbl tr:not(:first)").remove();
            $("#transactionTbl").append(data);

            $('#transPagerWrapper').html(pager);      

            //show tooltip for dupcount in rm_mcf_transaction.ascx
            if ($('#TableColumnHeader35').length > 0 && uxTooltipDupeCount) {
                uxTooltipDupeCount.set_targetControlID('TableColumnHeader35');
            }
        },
        error: function (result) {
            $("#uxProgress").hide();
        }
    });
}