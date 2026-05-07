var whichFilter = "";
function doGetMenuId(obj){
    return obj.getAttribute('mnId');
}
function doGetHiddenId(obj){
    return obj.getAttribute('hdId');
}
function doGetSubmitFilterId(obj){
    return obj.getAttribute('smId');
}
function doFilter(e, filter)
{
    var isEnter = false;
    var isIE = /MSIE/.test(navigator.userAgent);  
    var keyCode =  isIE ? e.keyCode : e.which;   
    isEnter = (keyCode == 13); 
    if(isEnter) 
    {
        var hddID = doGetHiddenId(filter);
        var hddFilterOption = $get(hddID);
        hddFilterOption.value = 'Contains';
        var btnID = doGetSubmitFilterId(filter);
        document.getElementById(btnID).click();
    }   
}

function doFilterOnMenu(sender, args)
{
    var menuItemValue = args.get_item().get_value();
    var hddFilterOption = $get(doGetHiddenId(whichFilter));
    hddFilterOption.value = menuItemValue;
    var btnID = doGetSubmitFilterId(whichFilter);
    document.getElementById(btnID).click();
}

function showFilter(e, filter)
{
    whichFilter = filter = filter.parentNode.getElementsByTagName('input')[0];
    
    var mnuID = doGetMenuId(filter);
    
    var contextMenu = $find(mnuID);
    if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget)))
    {
       contextMenu.show(e);
    }
    $telerik.cancelRawEvent(e);
}