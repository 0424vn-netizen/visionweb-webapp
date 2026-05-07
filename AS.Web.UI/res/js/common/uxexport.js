//===================== UxExport control API ==========================//

function UxExporter_OnResponseEnd(sender, args) {
    var eventTarget = args.get_eventTarget();
    var gridID = GetGridID(eventTarget);
    if (gridID == "") return;

    ShowHideExportControl(gridID);

    return true;
}

function UxExporter_OnRequestStart(sender, args) {
    var target = sender.__EVENTTARGET;
    if (target.indexOf("imgExcel") >= 0 ||
                target.indexOf("imgWord") >= 0 ||
                target.indexOf("imgCSV") >= 0 ||
                target.indexOf("imgPDF") >= 0)
        args.set_enableAjax(false);
}

//show or hide the export control from the grid ID
function ShowHideExportControl(gridID) {
    var grid = $find(gridID);
    if (grid) {
        try {
            var countItems = grid.get_masterTableView().get_dataItems().length;

            var exportTop = UxExporters.GetByGridID(gridID, false);
            var exportBottom = UxExporters.GetByGridID(gridID, true);

            if (exportTop) exportTop.VisibleIcons(countItems > 0);
            if (exportBottom) exportBottom.VisibleIcons(countItems > 0);

        } catch (e) { }
    }
}

//=======================================================================//

//get the grid ID from the target element when do ajax
function GetGridID(eventTarget) {
    try {
        var idx = eventTarget.indexOf('$');
        if (idx >= 0) {
            var gridID = eventTarget.substr(0, idx);
            return gridID;
        }
        else
            return eventTarget;
    } catch (e) {

        return "";
    }
}



//The static class for get the UxExporter object.
var UxExporters =
         {
             "GetByID": function(Id) {

                 for (var i = 0; i < UxExporterInfos.length; i++) {
                     var element = UxExporterInfos[i];
                     if (element.ID == Id) {
                         return new UxExporter(Id, element.ExcelID, element.CSVID, element.WordID, element.PDFID, element.GridHeaderID, element.GridID, element.IsBottom);
                     }
                 }

                 return null;
             },

             "GetByGridID": function(gridID, isBottom) {
                 if (isBottom) {
                     for (var i = 0; i < UxExporterInfos.length; i++) {
                         var element = UxExporterInfos[i];
                         if (element.GridID == gridID && element.IsBottom) {
                             return new UxExporter(element.GridID, element.ExcelID, element.CSVID, element.WordID,
                            element.PDFID, element.GridHeaderID, element.GridID, element.IsBottom);
                         }
                     }
                 }
                 else {
                     for (var i = 0; i < UxExporterInfos.length; i++) {
                         var element = UxExporterInfos[i];
                         if (element.GridID == gridID && element.IsBottom == false) {
                             return new UxExporter(element.GridID, element.ExcelID, element.CSVID, element.WordID,
                            element.PDFID, element.GridHeaderID, element.GridID);
                         }
                     }
                 }

                 return null;   //cannot find the exporter control.
             },

             "CountExporters": function() {
                 return UxExporterInfos.length;
             }
         };

//class UxExporter for manipulate on UxExporter control on client side.
function UxExporter(Id, ExcelId, CSVId, WordId, PDFId, GridHeaderId, GridId) {
    this._Id = Id;
    this._ExcelId = ExcelId;
    this._CSVId = CSVId;
    this._WordId = WordId;
    this._PDFId = PDFId;
    this._GridHeaderId = GridHeaderId;
    this._GridId = GridId;
}

UxExporter.prototype.VisibleIcons = function(isShow) {
    var excel = null;
    var word = null;
    var pdf = null;
    var csv = null;
    var exportLabel = null;
    if (this._ExcelId != "")
        excel = document.getElementById(this._ExcelId);
    if (this._WordId != "")
        word = document.getElementById(this._WordId);
    if (this._CSVId != "")
        csv = document.getElementById(this._CSVId);
    if (this._PDFId != "")
        pdf = document.getElementById(this._PDFId);
    if (this._GridHeaderId != "")
        exportLabel = document.getElementById(this._GridHeaderId);

    if (!isShow) {
        if (excel != null) excel.style.visibility = "hidden";
        if (word != null) word.style.visibility = "hidden"
        if (pdf != null) pdf.style.visibility = "hidden";
        if (csv != null) csv.style.visibility = "hidden";
        if (exportLabel != null) exportLabel.style.visibility = "hidden";
    }
    else {
        if (excel != null) excel.style.visibility = "";
        if (word != null) word.style.visibility = ""
        if (pdf != null) pdf.style.visibility = "";
        if (csv != null) csv.style.visibility = "";
        if (exportLabel != null) exportLabel.style.visibility = "";
    }

}

UxExporter.prototype.GetExcelElement = function() {
    return $get(this._ExcelId);
}
UxExporter.prototype.GetWordElement = function() {
    return $get(this._WordId);
}
UxExporter.prototype.GetPDFElement = function() {
    return $get(this._PDFId);
}
UxExporter.prototype.GetCSVElement = function() {
    return $get(this._CSVId);
}
         