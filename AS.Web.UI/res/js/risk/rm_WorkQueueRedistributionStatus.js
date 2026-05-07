var intervalRefesh = 10000;
function Refresh() {
    document.getElementById(rm_WorkQueueRedistributionStatus_uxSubmitAjax).click();
}

parent.intervalID = setInterval("Refresh();", intervalRefesh); //10 secs
parent.master_closeModalEvent = function () {//clear automation-event call refresh
    if (parent.intervalID) clearInterval(parent.intervalID);
    parent.master_closeModalEvent = null;
}

function ResetRefesh() {
    if (parent.intervalID) clearInterval(parent.intervalID);
    parent.intervalID = setInterval("Refresh();", intervalRefesh);
}

function masterAjax_responseEnd(sender, args) {
    setTimeout("AdjustModalSize();", 500);
}