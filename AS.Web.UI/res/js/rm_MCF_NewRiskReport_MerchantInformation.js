ShowMsg = function(msg) {
    alert(msg);
}
OpenDetailModal = function (e) {  
    var loadingPanel = $find(uxLoadingPanel_ClientID);
    loadingPanel.addCssClass('position-fixed');
    showLoading();
    document.getElementById(mfc_merchant_info_hdActionModalID).value = $(e).attr("actionModal");
    document.getElementById(mfc_merchant_info_uxShowModalDetailID).click();
   
}

