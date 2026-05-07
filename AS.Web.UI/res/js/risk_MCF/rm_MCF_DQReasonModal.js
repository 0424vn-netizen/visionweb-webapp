$(document).ready(function () {
        AdjustWindowSize(1540)
});
function showMatchEveryParameter(encodeURL)
{
    if (encodeURL) {
        parent.ShowPopupModalChild(1,encodeURL, 'auto');
    }
}

function showParameterViolationDetailsModal(encodeURL) {
    if (encodeURL) {
        parent.ShowPopupModalChild(1, encodeURL, 'auto');
    }
}