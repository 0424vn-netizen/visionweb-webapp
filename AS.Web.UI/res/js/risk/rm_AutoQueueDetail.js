
Sys.Application.add_load(function (sender, args) {
    aqDetailModule.DefaultStatus();    
    // Stop progress Update Queue Status
    //aqDetailModule.DoUpdateQueue();
});

var AutoQueueDetailModule = function (options) {
    var self = this;
   
    self.DoUpdateQueue = function () {        
        if (!aqActive || IsStopProcess) return;
        var queueTimer = setInterval(function () {
            if (IsStopProcess) return;
            $.ajax({
                type: 'POST',
                url: 'rm_AutoQueue.aspx/UpdatelastRunStatus',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: false,
                success: function (data) {                    
                    if (data.d != "") {
                        var text = data.d.match(/.*;/g).toString().replace(";", "");
                        $("#queueStatus").html(text);
                        var classStatus = data.d.match(/;.*/g).toString().replace(";", "");
                        if (classStatus == "stop") {
                            clearInterval(queueTimer);
                            IsStopProcess = true;
                        }
                        SetCss(classStatus);
                        doReload();
                    }
                },
            });
        }, 10000);
                 
    }

    function SetCss(classStatus) {
        switch (classStatus) {
            case "now":
                {
                    $("#queueicon").addClass("loading");
                    break;
                }
            case "last":
                {
                    $("#queueStatus").addClass("text-green");
                    $("#queueicon").addClass("finishloading");
                    break;
                }           
            default:
                {
                    resetCss();                    
                    break;
                }
        }
    }

    function resetCss() {
        $("#queueStatus").removeClass("text-green");
        $("#queueicon").removeClass("loading");
        $("#queueicon").removeClass("finishloading");
    }

    var doReload = function () {        
        document.getElementById(btnDoReload).click();
    }

    self.DefaultStatus=function() {
        var text = $("#aqStatusCode").html();        
        if (text == "stop") {
            IsStopProcess = true;
        }
        if (!IsStopProcess) {
            SetCss(text);
        }        
    }   
}