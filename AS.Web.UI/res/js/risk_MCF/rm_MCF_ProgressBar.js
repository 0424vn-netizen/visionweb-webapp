function initAllProgressBar() {
    $(".progressbar-wrapper").each(function (i, wrapper) {
        var tdHeight = $(wrapper).parent().height();
        var tdWidth = $(wrapper).parent().width();
        $(wrapper).css("height", tdHeight);
        $(wrapper).find(".ProgressBarDiv").height(tdHeight);

        var percentText = $(wrapper).find(".percent-text");
        var textHeight = $(percentText).height();
        var textWidith = $(percentText).width();
        $(percentText).css({ "left": tdWidth / 2 - textWidith / 2, "top": tdHeight / 2 - textHeight / 2 });
    });
}