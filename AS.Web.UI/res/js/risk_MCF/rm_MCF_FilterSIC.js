function doHeaderCheck(sender) {
    var uxIsAdded = $(Risk_FilterSIC_uxIsAdded);
    var uxIsSelectAll = $(Risk_FilterSIC_uxIsSelectAll);
    var uxSICCode = $(Risk_FilterSIC_uxSICCode);
    var uxAllFlag = $(Risk_FilterSIC_uxAllFlag);

    uxIsSelectAll.val(Risk_FilterSIC_IS_SELECT_ALL_FLAG);
    uxIsAdded.val(sender.checked);
    uxSICCode.val("");

    if (sender.checked) {
        uxAllFlag.val(Risk_FilterSIC_IS_SELECT_ALL_FLAG);
        $("input[id*=chkItem]").attr("checked", "checked");
    } else {
        uxAllFlag.val(Risk_FilterSIC_IS_DESELECT_ALL_FLAG);
        $("input[id*=chkItem]").removeAttr("checked");
    }
    $(Risk_FilterSIC_btnHidden).click();
}

function doItemCheck(sender, sicCode) {

    var uxIsAdded = $(Risk_FilterSIC_uxIsAdded);
    var uxIsSelectAll = $(Risk_FilterSIC_uxIsSelectAll);
    var uxSICCode = $(Risk_FilterSIC_uxSICCode);

    uxIsSelectAll.val(Risk_FilterSIC_IS_DESELECT_ALL_FLAG);
    uxIsAdded.val(sender.checked);
    uxSICCode.val(sicCode);


    if (!sender.checked) {
        $("input[id*=chkHeader]").removeAttr("checked");
        $(Risk_FilterSIC_uxAllFlag).val(Risk_FilterSIC_IS_DESELECT_ALL_FLAG);
    }

    $(Risk_FilterSIC_btnHidden).click();
}