$(document).ready(function () {
    if (isModal) {
        setTimeout(function () { AdjustModalSize(); }, 500);
        if (window.chrome) {
            repaintDOM();
            
        }

        window.onresize = function (event) {
            if (window.chrome) {
                repaintDOM();
            }
        };
    }
});

function repaintDOM() {
    var bodyElm = $('.body-modal');

    bodyElm.css("padding-bottom", "1px");
    setTimeout(function () {
        bodyElm.css("padding-bottom", "");
    },400);
}

function customLoadingPanel_OnClientShowing(sender, args) {
    var updatedControlWrapper = args.get_updatedElement();//get reference to the updated control's wrapper element
    var loadingElement = args.get_loadingElement();//get reference to the loading panel's element

    //size and position the loading panel
    var divParent = $("div[id*=" + (args._updatedElement.id + 'Panel') + "]");
    loadingElement.style.width = divParent.width() + 15 + "px";
    if (parseInt(loadingElement.style.left, 10) < 0) {
        loadingElement.style.left = updatedControlWrapper.offsetLeft + 15 + "px";
    }
}