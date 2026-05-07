function disableRiskScoreTextBoxes() { };
function doOpenSubPopup(url, width, height) {
    var scrW = getScreenWidth();
    var scrH = getScreenHeight();
    var sizeRate = 0.70;
    if (width == null)
        width = scrW * sizeRate;
    if (height == null)
        height = scrH * sizeRate;

    parent.master_closeModalEvent = function () {

        filter_closeModalEvent(parent._modalID, parent._clientIDbtn);
        parameter_closeModalEvent(parent._modalID);
        parent.master_closeModalEvent = null;
    }
    return parent.ShowPopupModalChild(1, url, 'auto');
}
parent._saveConfirm = false;
parent._modalID = '';
parent.setModalID = function (id) {
    parent._modalID = id;
}

parent._clientIDbtn = ''
parent.setClientIDbtn = function (id) {
    parent._clientIDbtn = id;
}

parent.setSaveConfirm = function (value) {
    parent._saveConfirm = value;
}
parent.getSaveConfirm = function () {
    return parent._saveConfirm;
}

parent.master_showModalEvent = function () {
}


parent.saveAssignment = function () {
    var btn = document.getElementById(rm_AdhocCreate_uxSaveAssignment);
    btn.click();
}



function ValidateCriteriaFilter() {
    var result = false;
    if (ValidateAssignmentFilters() == false){
        moveTo('#markupFilter');
        return false;
    }
    if(ValidateAssignmentParameters() == false) {
        moveTo('#markupParam');
        return false;
    }
    //count the number of selected merchant and get confirm from user before processing saving process
    saveParameters();
    parent.setSaveConfirm(true);
    btnCountClick();
    return false;
}
//-->

function moveTo(id) {
    $('html, body').animate({
        scrollTop: $(id).offset().top + 'px'
    }, 'fast');
}

/****SHOW/HIDE FINISH BUTTON*****/
function showFinishButton() {
    var isShow = true;
    $("#uxAssignmentParamsGrid").find("div[id*='param']").each(function () {
        if ($("#uxAssignmentParamsGrid").find("div[id='" + $(this).prop("id") + "']").length > 1) {
            $("#uxAssignmentParamsGrid").find("div[id='" + $(this).prop("id") + "']").each(function () {
                if ($(this).parents("div[class*='group']").children("div[id*='param']").length == 1) {
                    isShow = false;
                    $(this).find("span[id*='uxStandAloneIcon']").removeClass("hide");
                } else {
                    $(this).find("span[id*='uxStandAloneIcon']").addClass("hide");
                }
            });
        } else {
            if (!$(this).find("span[id*='uxStandAloneIcon']").hasClass("hide")) {
                $(this).find("span[id*='uxStandAloneIcon']").addClass("hide");
            }
        }
    });
    if (isShow)
        $("#" + rm_AdhocCreate_uxSave).removeAttr("disabled");
    else
        $("#" + rm_AdhocCreate_uxSave).attr("disabled", "disabled");
    //Check show/hide title
    if ($("#" + Risk_Assignment_Parameters_rdMatchAll).prop("checked") == false) {
        //$("#divGroupTitle").text(Risk_Assignment_Parameters_js_group);
        $("#divGroupTitle").removeClass("hide");
    } else {
        //$("#divGroupTitle").text("");
        $("#divGroupTitle").addClass("hide");
    }
    //Reset timeout
    resetTimeout();
}

jQuery(document).ready(function () {
    showFinishButton();
    var width = $(".modal-xxl").width();
    var height = $("#fixedPanel").height();
    var bgColor = $(".body-modal").css("background-color");
    $("#fixedHeight").css("height", height + "px");
    $("#fixedPanel").css({"bottom": "0px", "position": "fixed", "width": width - 40 + "px", "z-index": 1000, "background": "#fff", "left": "0px","right": "0px","margin": "auto" });
    $("#fixedFooter").css({"height": "20px", "background-color" : bgColor});
    $("#fixedContain").css({"border-left" : "1px solid #ccc", "border-right" : "1px solid #ccc","border-bottom" : "1px solid #ccc", "padding-right":"17px", "padding-bottom" : "24px" });
    $("#fixedShadowBox").css({"box-shadow": "#DAD3D3 0px -2px 1px", "width":"1030px", "left":"25px","height":"1px"});
});