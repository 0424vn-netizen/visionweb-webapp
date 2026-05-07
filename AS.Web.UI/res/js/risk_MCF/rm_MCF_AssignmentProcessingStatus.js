function Refresh() {
    document.getElementById(rm_AssignmentProcessingStatus_uxSubmitAjax).click();
}

parent.intervalID = setInterval("Refresh();", 30000); //30 secs
parent.master_closeModalEvent = function () {//clear automation-event call refresh
    if (parent.intervalID) clearInterval(parent.intervalID);
    parent.master_closeModalEvent = null;
}


function masterAjax_responseEnd(sender, args) {

    setTimeout("AdjustModalSize();", 500);
}
//jQuery(document).ready(function () {
//    setTimeout("AdjustModalSize();", 500);
//});