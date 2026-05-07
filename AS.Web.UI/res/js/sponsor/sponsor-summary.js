$(document).ready(function () {
    SetHeaderGrid();
    let dateMode = $('#' + uxDaily).prop('checked') ? 'uxDaily' : 'uxDateRange';
    ShowDailySearch(dateMode);
    $('.chzn-select').change(function (event, item) {
        let currentSelected = item.selected;
        let selecteds = $('#' + event.target.id).val();

        if (selecteds.length == 0) {
            let currentValue = item.selected || item.deselected;
            if (currentValue != '-1')
                currentValue = '-1';

            $('#' + event.target.id).val(currentValue);
            $('#' + event.target.id).trigger('chosen:updated');
        } else {
            if (selecteds.indexOf('-1') >= 0) {
                let value = selecteds.find((ele, indx) => ele != -1);
                $('#' + event.target.id).val(value);
                $('#' + event.target.id).trigger('chosen:updated');
            }
        }

        if (currentSelected == '-1') {
            $('#' + event.target.id + ' option:selected').prop('selected', false);
            $('#' + event.target.id).val('-1');
            $('#' + event.target.id).trigger('chosen:updated');
        }

        if (item.deselected == '-1') {
            return;
        }
    });
});
function SetHeaderGrid() {
    let secondRowHeaders =
        [
            ['', 1, 'GridHeader_FirstColumn rgHeader border-lr'],
            ['', 1, 'rgHeader border-right'],
            ['', 1, 'rgHeader mh  border-right'],
            [AlertedMsg, 3, 'rgHeader mh  border-right'],
            [WorkedMsg, 3, 'rgHeader mh  border-right'],
            [CasesMsg, 2, 'rgHeader border-right'],
            ['', 1, 'rgHeader border-right'],
            ['', 1, 'GridHeader_LastColumn rgHeader border-right']
        ];
    let firstRowHeaders =
        [
            ['', 1, 'GridHeader_FirstColumn rgHeader mh border-right'],
            ['', 1, 'rgHeader mh border-right'],            
            [DisinctMerchantDesc, 7, 'rgHeader mh', 'rgHeader mh'],
            ['', 2, 'rgHeader mh border-right'],
            ['', 1, 'rgHeader mh border-right'],
            ['', 1, 'rgHeader mh border-right']

        ]
    addGroupHeadersForStaticRadGrid(uxSponsoredEntityGrid, secondRowHeaders, false, true);
    addGroupHeadersForStaticRadGrid(uxSponsoredEntityGrid, firstRowHeaders, false);
}

function ChangeDateOption(sender, args) {
    let pickerFrom = $find(uxBeginDate);
    let pickerTo = $find(uxEndDate);
    let dateValue = $(sender).val();
    let now = new Date();
    ShowDailySearch(dateValue);   
    
    if (dateValue == "uxDaily") {
        pickerFrom.set_selectedDate(now);
    }
    else {
        pickerTo.set_selectedDate(now);
        now.setDate(1);
        pickerFrom.set_selectedDate(now);
    }
}

function ShowDailySearch(dateValue) {    
    if (dateValue == "uxDaily") {
        $('#' + uxEntityList).addClass('hide');
        $('#uxMultiEntities').removeClass('hide');
        $('#uxEndDate-wrapper').addClass('hide');
    }
    else {
        $('#' + uxEntityList).removeClass('hide');
        $('#uxMultiEntities').addClass('hide');
        $('#uxEndDate-wrapper').removeClass('hide');
    }
}

function ValidateData() {
    let dateMode = $('#' + uxDaily).prop('checked') ? 'uxDaily' : 'uxDateRange';
    let pickerFrom = $find(uxBeginDate);
    let pickerTo = $find(uxEndDate);
    let startDate = pickerFrom.get_textBox().value;
    let endDate = pickerTo.get_textBox().value;
    let currentDate = new Date();

    if (dateMode == "uxDaily") {
        if (startDate == "" || !isDate(startDate)) {
            alert(Msg_V2);
            return false;
        }
        if (new Date(startDate) > currentDate) {
            alert(String.format(ReportFilter_NotGreaterThanToDay, 'Daily'));
            return false;
        }
    }
    else {
        if (startDate == "" || endDate == "" || !isDate(startDate) || !isDate(endDate)) {
            alert(Msg_V2);
            return false;
        }
        if (new Date(startDate) > new Date(endDate)) {
            alert(dateRangeMsg + ": " + Msg_V3);
            return false;
        }
        if (new Date(startDate) > currentDate || new Date(endDate) > currentDate)
        {
            alert(String.format(ReportFilter_NotGreaterThanToDay, 'Date Range'));
            return false;
        }
    }
    return true;
}

function masterAjax_responseEnd(sender, args) {
    if (sender.__EVENTTARGET.indexOf('uxSponsoredEntityGrid') >= 0)
        SetHeaderGrid();
}

function siteJumpClick(sender, userid) {
    var $sjCtrl = $(sender);

    //Exisiting request in queue, just ignore to prevent multiple click
    if ($sjCtrl.data("isLoading")) {
        return;
    }

    $sjCtrl.data("isLoading", true);

    $.ajax({
        type: "POST",
        url: currentPageURL + "/GetSiteJumpUrl",
        data: JSON.stringify({ encUser: userid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            let returnData = result.d;

            if (!returnData) {
                alert(invalidUserMsg);
                return;
            }

            window.open(returnData);
        },
        complete: function (response) {
            $sjCtrl.data("isLoading", false);

            //process not authorize
            let statusCode = response.statusCode().status;
            if (statusCode === 401) {
                window.location.href = _LOGIN_URL;
            }
        }
    });
}