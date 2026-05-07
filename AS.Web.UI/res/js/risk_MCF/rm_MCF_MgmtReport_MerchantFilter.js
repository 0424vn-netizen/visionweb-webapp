function doSubmit() {
    uxSelectedMerchantsClient = document.getElementById(MgmtReport_MerchantFilter_uxSelectedMerchants);
    parent.RebindMerchantFilter(uxSelectedMerchantsClient.value);
    parent.HidePopupModal();
}

var uxSelectedMerchantsClient = null;
function doSetSelectedMerchants(merchantNumber, checked) {
    if (merchantNumber == '') return;
    uxSelectedMerchantsClient = document.getElementById(MgmtReport_MerchantFilter_uxSelectedMerchants);

    if (checked) {
        if (uxSelectedMerchantsClient.value.indexOf(',' + merchantNumber + ',') < 0) {
            uxSelectedMerchantsClient.value += merchantNumber + ',';
        }
    }
    else {
        uxSelectedMerchantsClient.value = uxSelectedMerchantsClient.value.replace(new RegExp(',' + merchantNumber + ',', 'gi'), ',');
    }
    if (uxSelectedMerchantsClient.value.substr(0, 1) != ',') {
        uxSelectedMerchantsClient.value = ',' + uxSelectedMerchantsClient.value;
    }
    if (uxSelectedMerchantsClient.value == ',') uxSelectedMerchantsClient.value = '';
}