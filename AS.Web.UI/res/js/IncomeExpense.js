function ajaxRequestStart(sender, args) {
    UxExporter_OnRequestStart(sender, args);
}

function ajaxResponseEnd(sender, args) {
    UxExporter_OnResponseEnd(sender, args);
}

$(window).load(function () {
    $(".as-animated-tabstrip").asAnimatedTab();

});
$(window).resize(function () {
    $(".as-animated-tabstrip").asAnimatedTab();
});