$(function () {
    $.receiveMessage(
      function (e) {
          $('#uxIframeCaseHitory').css('height', e.data + 'px');
          $('#uxIframeCaseHitory').attr("scrolling", "no");
          $(document.getElementById('uxIframeCaseHitory').contentWindow.document.getElementById('uxCaseHistory_uxExporter_divExport')).parent().attr('style', 'display:block')
      },
        MerchantProfile_CaseMgmtDomain
    );
});
function reloadParentCasePage() {
    try {
        var buttonApply = document.getElementById('uxIframeCaseHitory').contentWindow.document.getElementById('uxCaseHistory_uxApply');
        buttonApply.click();
    } catch (ex) {

    }
}
