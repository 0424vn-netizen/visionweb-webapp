function uxFilterStatus_Checked() {
    document.getElementById(uxChangeFilterStatus_ClientID).click();
}

$(document).ready(function () {
    $('#btnCreateUserGroup').click(function (e) {
        e.preventDefault();
        ShowPopupModal('AddEditUserGroup.aspx', 'auto');
    });
});

function doPostBack() {
    document.getElementById(uxChangeFilterStatus_ClientID).click();
}