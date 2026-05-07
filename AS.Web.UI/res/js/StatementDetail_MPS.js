$(document).ready(function () {
    if (!$('table#tblSettlementDiscount tr:odd').hasClass('rgNoRecords')) {
        $('table#tblSettlementDiscount tr:odd').removeClass('rgAltRow').addClass('rgRow');
        $('table#tblSettlementDiscount tr:even').removeClass('rgAltRow').addClass('rgAltRow');
    }
    var row_class = 'Row';
    $('.settlement_discount_tb_tr').each(function (i) {
        $(this).addClass(row_class);
        if (row_class == 'Row') {
            row_class = 'AltRow';
        }
        else {
            row_class = 'Row';
        }
    });
});