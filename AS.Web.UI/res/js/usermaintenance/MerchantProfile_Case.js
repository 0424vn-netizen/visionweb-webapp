$(function () {
    $.receiveMessage(
      function (e) {
          $('#uxIframeCaseHitory').css('height', e.data + 'px');
          $('#uxIframeCaseHitory').attr("scrolling", "no");
      },
        MerchantProfile_CaseMgmtDomain
    );
});
function reloadParentCasePage() {
    try {
        $('#uxIframeCaseHitory').attr("src", $('#uxIframeCaseHitory').attr("src"));
    } catch (ex) {

    }
}