function masterAjax_requestStart(sender, args) {
    var isParent = false,
        myParent = getRealParent();
    try {
        isParent = myParent.doResetTimeOut;
    }
    catch (e) { };
    if (isParent) {
        try {
            myParent.doResetTimeOut();
        }
        catch (err) { };
    }
    else {
        try {
            doResetTimeOut();
        }
        catch (err) { };
    }
    if (window.ajaxRequestStart) {
        return window.ajaxRequestStart(sender, args);
    }
}
function masterAjax_responseEnd(sender, args) {
    if (window.ajaxResponseEnd) {
        return window.ajaxResponseEnd(sender, args);
    }
    $('[data-hover="dropdown"]').dropdownHover();  //Re-register dropdown hover
}