parent.setModalID('ParameterListModal');
function callFuncFormParent() {
    parent.ClosePopupModal(1);
    parent.ReloadParamList();
}
jQuery(document).ready(function () {
    var width = $(".modal-md").width();
    var height = $("#fixedPanel").height();
    var bgColor = $(".body-modal").css("background-color");
    $("#fixedHeight").css("height", height + "px");
    $("#fixedPanel").css({ "bottom": "0px", "position": "fixed", "width": width - 40 + "px", "z-index": 1000, "background": "#fff", "padding": "0 8px 24px 0", "left": "0px", "right": "0px", "margin": "auto", "border-bottom": "20px solid " + bgColor });
});