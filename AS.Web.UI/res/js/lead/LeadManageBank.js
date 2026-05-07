function CloseMe() {
    //Close modal
    return parent.HidePopupModal();
}

function SubmitForm(totalBankSelected) {
    var parentWindows = getRealParent();
    var parentCountSelectedBank = $(parentWindows.document).find("#LiteralCountSelectedBank");
    parentCountSelectedBank.text(totalBankSelected);
    var parentSelectedBank = $(parentWindows.document).find("#hdfSelectedBank");
    parentSelectedBank.val(true);
    //Close modal
    return parent.HidePopupModal();
}

//Disable right filter
function HideRightFilter() {
    $('[class="js-textbox-filter"][name$="txtFilterRight"]').addClass("hide");
    $(".imgFilterRight").addClass("hide");
}

$(document).ready(function () {
    HideRightFilter();
});