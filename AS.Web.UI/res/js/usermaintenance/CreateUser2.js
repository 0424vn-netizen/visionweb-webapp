function closeMeUser2() {
    if (isFromCreateChainModal == 'true') {
        ClosePopupModal();
        parent.UpdateResult();
    } else {
        parent.doRebindUserList();
        return parent.HidePopupModal();
    }
}