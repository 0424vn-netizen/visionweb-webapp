$(document).ready(function () {
    SetHeaderGrid();
});
function ajaxResponseEnd(sender, args) {
    SetHeaderGrid();
}
function SetHeaderGrid() {
    addGroupHeadersForStaticRadGrid(rm_Group_ActiveModal_uxReportGrid,
            [['', 1, 'rgHeader mh'],
            ['Merchant Count', 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh'],
            ['Worked', 2, 'rgHeader mh'],
            ['', 1, 'rgHeader mh']]);
}