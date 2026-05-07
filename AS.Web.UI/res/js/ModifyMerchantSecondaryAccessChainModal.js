function UpdateChainSuccess() {
    ClosePopupModal();
  parent.UpdateHierachy();
}
function validator() {
    if (!dovalidation()) {
        AdjustModalSize();
        return false;
    }
    return true;
}
function ValidateData() {
    var chain = $find(ModifyMerchantChainModal_uxChain);
    var emptyMess = ModifyMerchantChainModal_EmptyMessage;
    if (chain.get_selectedItem() === null && chain.get_text() !== '' && chain.get_text() !==emptyMess) {
        return false;
    }
    return true;
}