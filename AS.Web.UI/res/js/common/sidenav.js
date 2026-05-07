$(document).ready(function () {

    function checkNavHeight() {
        if ($('.affix').hasClass("narrow-screen")) {
            $('.affix').removeClass("narrow-screen");
        }
        if ($('.affix').height() > ($(window).height() - $('#' + uxWellcomeBar).height())) {
            $('.affix').addClass("narrow-screen");
        }
    }
    
    //Track scrolling to move left navigation menu
    var isDefaultTheme = ($('body').hasClass("mif-sidebar-container")) ? true : false;
    if (isDefaultTheme) {
        $('.mif-sidebar-container').css("top", $('.container').offset().top + "px");
    }
    $(window).scroll(function () {
        var baseElement = isDefaultTheme ? ".mif-container" : ".container";

        var position = $(baseElement).offset().top - $(window).scrollTop() - 0.5;
        var fixedHeight = 0;
        if ($('#' + uxWellcomeBar).height() != null) {
            fixedHeight = $('#' + uxWellcomeBar).height();
        }

        if (position > 0) {
            $('.affix').css({ "top": position + "px", "position": "fixed" });
        } else {
            $('.affix').css({ "top": fixedHeight + "px", "position": "fixed" });
        }
    });
    if ($('.btn-report-filter').length > 0 && $('.affix').length > 0) {
        $('#containFilter').on('click', () => {
            if (!isDefaultTheme) {
                var fixedHeight = $('#' + uxWellcomeBar).height();
                if ($('.affix').offset().top > fixedHeight) {
                    $('.affix').css({ "top": "", "position": "absolute" });
                }
            }
        });
    }
    checkNavHeight();
    $(window).resize(function () {
        checkNavHeight();
    });

    // fix position whenever click on left navigator
    $("ul.merchant-sidenav a").click(function () {
        var theLink = $(this);
        var id = theLink.attr('href');
        if (id.toLowerCase() != '#') {
            var moveTop = 0;
            if (id.toLowerCase().indexOf("merchinfo") > 0) {
                moveTop = -30;
            }
            else {
                moveTop = -5;
            }

            if ($(id).offset() != undefined) {
                $("html,body").animate({
                    scrollTop: $(id).offset().top + moveTop
                }, "slow");
            }
        }
    });
    // fix position whenever click on left navigator
    $(".mif-content-container .nav-report a").click(function () {
        var theLink = $(this);
        var id = theLink.attr('href');
        if (id.toLowerCase() != '#') {            
            $("html,body").animate({
                scrollTop: $(id).offset().top - 10
            }, "slow");
        }
    });
});