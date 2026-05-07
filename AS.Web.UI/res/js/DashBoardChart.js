/*
    currentState = 1: 

*/
$(document).ready(function () {    
    var currentState = 1;
    //First Load
    $('#current-value').text(netVolumeSumCurrent);
    $('#current-prevalue').text(netVolumeSumPrevious);
    $('#alt1-value').text(yoyVolumeGrowthPercent);
  
    if (dashboard_IsMerchantLoggedIn) {
        $('#alt2-value').css("display", 'none');
        $('#alt2-title').css("display", 'none');
    }
    else {
        $('#alt2-value').text(yoyMerchantGrowthPercent);
    }

    //End 
    var netChart = document.getElementById('netChart');
    var volumeChart = document.getElementById('volumeChart');
    var merchantChart = document.getElementById('merchantChart');
    $('#alt1-value').click(function () {
        
        if (currentState == 2) {
            $('#current-title').text("12 Month Net Volume");
            $('#current-subtitle').text("Previous Year");
            $('#current-value').text(netVolumeSumCurrent);
            $('#current-prevalue').text(netVolumeSumPrevious);
            $('#alt1-value').text(yoyVolumeGrowthPercent);
            $('#alt1-title').text("YOY Volume Growth");
            currentState = 1;

            netChart.style.display = '';
            volumeChart.style.display = 'none';
            merchantChart.style.display = 'none';
            $(DashBoard_uxNetVolumeChart).data("kendoChart").refresh();

        } else {
            $('#current-title').text("12 Month Merchant Account Growth");
            $('#current-subtitle').text("Count vs. Previous Year");
            $('#current-value').text(yoyVolumeGrowthPercent);
            $('#current-prevalue').text(yoyVolumeGrowthPrevious);
            $('#alt1-value').text(netVolumeSumCurrent_Format);
            $('#alt1-title').text("Net Volume");

            if (currentState == 3 || dashboard_IsMerchantLoggedIn == false) {
                $('#alt2-value').text(yoyMerchantGrowthPercent);
                $('#alt2-title').text("YOY Merchant Growth");
            }
            currentState = 2;
            volumeChart.style.display = '';
            netChart.style.display = 'none';
            merchantChart.style.display = 'none';
            $(DashBoard_uxVolumeGrowthChart).data("kendoChart").refresh();
        }
    });

    $('#alt2-value').click(function () {
       if (currentState == 3) {
            $('#current-title').text("12 Month Net Volume");
            $('#current-subtitle').text("Previous Year");
            $('#current-value').text(netVolumeSumCurrent);
            $('#current-prevalue').text(netVolumeSumPrevious);
            $('#alt2-value').text(yoyMerchantGrowthPercent);
            $('#alt2-title').text("YOY Merchant Growth");
            currentState = 1;
            netChart.style.display = '';
            volumeChart.style.display = 'none';
            merchantChart.style.display = 'none';
            $(DashBoard_uxNetVolumeChart).data("kendoChart").refresh();
       }
       else {
            $('#current-title').text("Year Over Year Merchant Growth");
            $('#current-subtitle').text("Count vs. Previous Year");
            $('#current-value').text(yoyMerchantGrowthPercent);
            $('#current-prevalue').text(yoyMerchantGrowthPrevious);
            $('#alt2-value').text(netVolumeSumCurrent_Format);
            $('#alt2-title').text("Net Volume");

            if (currentState == 2) {
                $('#alt1-value').text(yoyVolumeGrowthPercent);
                $('#alt1-title').text("YOY Volume Growth");
            }

            currentState = 3;
            netChart.style.display = 'none';
            volumeChart.style.display = 'none';
            merchantChart.style.display = '';
            $(DashBoard_uxMerchantGrownChart).data("kendoChart").refresh();
        }
    });
});
