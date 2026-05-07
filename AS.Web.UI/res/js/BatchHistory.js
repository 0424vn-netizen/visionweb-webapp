function ajaxRequestStart(sender, args) {
    if (typeof (UxExporter_OnRequestStart) == 'function')
        UxExporter_OnRequestStart(sender, args);
}
function ajaxResponseEnd(sender, args) {
    SetHeaderDrilldownGrid();
    SetHeaderBatchMerchantGrid();
    if (typeof (UxExporter_OnResponseEnd) == 'function')
        UxExporter_OnResponseEnd(sender, args);
}

$(document).ready(function () {
    SetHeaderDrilldownGrid();
    SetHeaderBatchMerchantGrid();
    addCheckSpecialCharactersForDate();
});

function SetHeaderDrilldownGrid() {
    if (isEnableEntityName == 'True') {
        addGroupHeadersForStaticRadGrid(uxDrilldownGridID,
       [
           ['', 1, 'rgHeader mh'],
           ['', 1, 'rgHeader mh'],
           ['', 1, 'rgHeader mh'],
           ['', 1, 'rgHeader mh'],
           ['', 1, 'rgHeader mh'],
           [BH_js_BankCard, 3, 'rgHeader mh'],
           [BH_js_NonBankCard, 3, 'rgHeader mh'],
           [BH_js_Total, 3, 'rgHeader mh']

       ]);
    }
    else {
        addGroupHeadersForStaticRadGrid(uxDrilldownGridID,
      [
          ['', 1, 'rgHeader mh'],
          ['', 1, 'rgHeader mh'],
          ['', 1, 'rgHeader mh'],
          ['', 1, 'rgHeader mh'],
          [BH_js_BankCard, 3, 'rgHeader mh'],
          [BH_js_NonBankCard, 3, 'rgHeader mh'],
          [BH_js_Total, 3, 'rgHeader mh']

      ]);
    }
    appendHeaderforPrinter(uxDrilldownGridID);
}


function SetHeaderBatchMerchantGrid() {
    var arrayCol= [];
    var colc = $('#' + uxBatchMerchantGridID + ' table tr').first().find('th').length;
    for (var i = 0; i < colc - 10 ; i++) {
        arrayCol.push(['', 1, 'rgHeader mh']);
    }
    arrayCol.push([BH_js_BankCard, 3, 'rgHeader mh']);
    arrayCol.push([BH_js_NonBankCard, 3, 'rgHeader mh']);
    arrayCol.push([BH_js_Total, 3, 'rgHeader mh']);

    addGroupHeadersForStaticRadGrid(uxBatchMerchantGridID,
       arrayCol);

    //addGroupHeadersForStaticRadGrid(uxBatchMerchantGridID,
    //    [
    //        ['', 1, 'rgHeader mh'],
    //        ['', 1, 'rgHeader mh'],
    //        ['', 1, 'rgHeader mh'],
    //        ['', 1, 'rgHeader mh'],
    //        ['', 1, 'rgHeader mh'],
    //        ['', 1, 'rgHeader mh'],
    //        ['', 1, 'rgHeader mh'],
    //        [BH_js_BankCard, 3, 'rgHeader mh'],
    //        [BH_js_NonBankCard, 3, 'rgHeader mh'],
    //        [BH_js_Total, 3, 'rgHeader mh']
    //    ]);

    appendHeaderforPrinter(uxBatchMerchantGridID);
}