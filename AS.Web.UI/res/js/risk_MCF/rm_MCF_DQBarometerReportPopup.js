function OpenFlagColorTable() {
    ShowPopupModal("rm_MCF_ColorLegend.aspx", 'auto')
    return false;
}

var riskReportInprogress = false;
function MerchantNumberClick(merchantNumber, totalvalue, reportDate, assignmentID) {
    // Abort previous request if user clicks another merchant
    if (riskReportInprogress) {
        return;
    }

    riskReportInprogress = true;
    var data = {
        "status": "true",
        "merchantNumber": merchantNumber,
        "reportDate": reportDate,
        "assignmentID": assignmentID
    };

    var actionUrl = rootURL + "risk_MCF/rm_MCF_DQBarometerReportPopup.aspx/MerchantNumberClick";
    $.ajax({
        type: "post",
        url: actionUrl,
        async: true,
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
        },
        complete: function () {
            riskReportInprogress = false;
        }
    });
}

//Check ajax ManageAssignment UC
function masterAjax_responseEnd(sender, args) {
    SetHeaderGrid();
    SetHeaderAssignmentSummaryScreenWidth();
    if (typeof initAllProgressBar == 'function') {
        initAllProgressBar();
    }
    $('[data-hover="dropdown"]').dropdownHover();
}