function SelectDateChange() {
    var uxradDateRange = $get(uxradDateRangeID);
    var uxradIEYearToDate = $get(uxradIEYearToDateID);
    var uxradTwelveMonth = $get(uxradTwelveMonthID);
    var uxIEDateFrom = $find(uxIEDateFromID);
    var uxIEDateTo = $find(uxIEDateToID);

    if (uxradDateRange.checked) {
        uxIEDateFrom.set_selectedDate(defaultFromDate);
        uxIEDateTo.set_selectedDate(new Date());
        $('#' + pnlDateRangecontrolsfromID).css('display', 'inline-block');
        $('#' + pnlDateRangecontrolstoID).css('display', 'inline-block');
        $('#' + pnlDateRangecontrolsfromID).css('visibility', 'visible');
        $('#' + pnlDateRangecontrolstoID).css('visibility', 'visible');

    }
    else if (uxradIEYearToDate.checked) {
        $('#' + pnlDateRangecontrolsfromID).css('display', 'none');
        $('#' + pnlDateRangecontrolstoID).css('display', 'none');
    }
    else {
        $('#' + pnlDateRangecontrolsfromID).css('display', 'none');
        $('#' + pnlDateRangecontrolstoID).css('display', 'none');
    }
}
function uxNetProfitOnClientSelectedIndexChanged(sender, arg) {
    $('#' + uxNetProfitFromID).val('');
    $('#' + uxNetProfitToID).val('');
    if (sender.get_selectedItem().get_value() == 'GREATERTHAN' || sender.get_selectedItem().get_value() == 'LESSTHAN') {
        $('#' + pnlProfitFromID).css('display', 'inline-block');
        $('#' + pnlProfitToID).css('display', 'none');
        if ($.trim($('#' + uxNetProfitFromID).val()) == '0' && $.trim($('#' + uxNetProfitTo).val()) > 0) {
            $('#' + uxNetProfitFromID).val('');
        }
    }
    else if (sender.get_selectedItem().get_value() == 'BETWEEN') {
        $('#' + pnlProfitFromID).css('display', 'inline-block');
        $('#' + pnlProfitToID).css('display', 'inline-block');
    }
    else {
        $('#' + pnlProfitFromID).css('display', 'none');
        $('#' + pnlProfitToID).css('display', 'none');
    }
}
function SubmitFilter()
{
    document.getElementById(uxReportFilterID + "_btSubmit").click();
    return false;
}
$(document).ready(function () {
    $('#' + uxReportFilterID + "_btSubmit").removeAttr('onclick');
    $('#' + uxReportFilterID + "_btSubmit").click(function () {
        var uxradDateRange = $get(uxradDateRangeID);
        var uxIEDateFrom = $find(uxIEDateFromID);
        var uxIEDateTo = $find(uxIEDateToID);
        var uxNetProfit = $find(uxNetProfitID);

        if (uxradDateRange.checked) {
            if (uxIEDateFrom.get_selectedDate() == null) {
                alert(Text_DateFormatInvalid);
                return false;
            }
            else if (uxIEDateTo.get_selectedDate() == null) {
                alert(Text_DateFormatInvalid);
                return false;
            }
            else if (uxIEDateFrom.get_selectedDate() > uxIEDateTo.get_selectedDate()) {
                alert(Text_EndGreaterBeginDate);
                return false;
            }

            else if (new Date(uxIEDateFrom.get_selectedDate()) > new Date()) {
                alert(Text_EndGreaterToday);
                return false;
            }

        }
       
        var uxRadNetProfitTo = $find(uxNetProfitToID);
        var uxRadNetProfitFrom = $find(uxNetProfitFromID);

        if (uxNetProfit.get_selectedItem().get_value() == 'BETWEEN') {
         
            if (uxRadNetProfitFrom.get_value().toString() == '' && uxRadNetProfitTo.get_value().toString() != '') {
                alert(Text_NetProfitFromRequired);
                $('#' + uxNetProfitFromID).focus();
                return false;
            }
            if (uxRadNetProfitTo.get_value().toString() == '' && uxRadNetProfitFrom.get_value().toString() != '') {
                alert(Text_NetProfitToRequired);
                $('#' + uxNetProfitToID).focus();
                return false;
            }            
           
            if (uxRadNetProfitTo.get_value() < uxRadNetProfitFrom.get_value()) {
                alert(Text_ToGreaterFrom);
                return false;
            }

        }        
        return rf_Submit();
    });

    $('#' + uxReportFilterID + ' input[type=text]').keypress(function (e) { 
        var code = e.keyCode || e.which;
        if (code == 13) {
            if (isCancelEnterPressMPSReportFilter) {
                return false;
            }
            setTimeout(function () {
                $('#' + uxReportFilterID + "_btSubmit").click();
            }, 200);
            return false;
        }
    });

    addCheckSpecialCharactersForDate();
});