function masterAjax_requestStart(sender, args) {
    var isParent = false,
        myParent = getRealParent();
    try {
        isParent = myParent.doResetTimeOut;
    }
    catch (e) { };
    try {
    if (window.opener != null && window.opener.doResetTimeOut != null) {
        window.opener.doResetTimeOut();
    }
    else if (window.opener != null && window.opener.parent != null && window.opener.parent.doResetTimeOut != null) {
        window.opener.parent.doResetTimeOut();
    }
    else {
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
    }
    }
    catch (err) {
        doResetTimeOut();
     };
    if (window.ajaxRequestStart) {
        return window.ajaxRequestStart(sender, args);
    }
}
function masterAjax_responseEnd(sender, args) {
    if (window.ajaxResponseEnd) {
        return window.ajaxResponseEnd(sender, args);
    }
}