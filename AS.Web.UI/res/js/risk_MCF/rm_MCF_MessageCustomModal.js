$(document).ready(function () {
    $('ul.message-list').each(function () {
        if ($(this).find('li').length > 4) {
            $('li', this).eq(3).nextAll().hide().addClass('toggleable');
            $(this).append('<li class="more"><a herf="#"  style="text-decoration: underline;cursor: pointer;">' + msg_custom_Modal_more + '</a></li>')
        }
        $(this).on('click', '.more', toggleShow)
    })
})
function toggleShow() {
    $(this).siblings('li.toggleable').slideToggle(0);
    $(".more").remove();
    AdjustModalSize();
}
function onSubmitData() {
    parent.OnSubMitDataMesssage();
    parent.ClosePopupModal(3);
}