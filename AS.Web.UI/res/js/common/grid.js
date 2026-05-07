// js for radgrid
function adjustHeightStaticHeaderGrid(gridId) {
    getScrollBarSize = function () {
        var inner = document.createElement('p');
        inner.style.width = '100%';
        inner.style.height = '200px';

        var outer = document.createElement('div');
        outer.style.position = 'absolute';
        outer.style.top = '0px';
        outer.style.left = '0px';
        outer.style.visibility = 'hidden';
        outer.style.width = '200px';
        outer.style.height = '150px';
        outer.style.overflow = 'hidden';
        outer.appendChild(inner);

        document.body.appendChild(outer);
        var w1 = inner.offsetWidth;
        outer.style.overflow = 'scroll';
        var w2 = inner.offsetWidth;
        if (w1 == w2) w2 = outer.clientWidth;

        document.body.removeChild(outer);

        return (w1 - w2);
    };
    var barSize = getScrollBarSize();
    var divData = document.getElementById(gridId + '_GridData');
    var tables = divData.getElementsByTagName('table');
    for (var i = 0; i < tables.length; i++) {
        if (tables[i].className.indexOf('rgMasterTable') >= 0) {
            divData.style.height = (tables[i].clientHeight + barSize) + 'px';
            break;
        }
    }
}

function addGroupHeadersForStaticRadGrid(gridId, headers, isIngoreColNone, isInsertMoreRows) {
    var table;
    var asGrid = $('#' + gridId);
    if (!isInsertMoreRows)
        isInsertMoreRows = false;
   
    if (asGrid != null && asGrid.length > 0) {
        var rgHeaderWrapper = asGrid.children('.rgHeaderWrapper');
        if (rgHeaderWrapper != null && rgHeaderWrapper.length > 0) {
            table = document.getElementById(gridId + '_ctl00_Header');
        }
        else {
            table = document.getElementById(gridId + '_ctl00');
        }
    }
    else {
        table = document.getElementById(gridId + '_ctl00');
    }
    if (table == null) {
        return;
    }

    if (table.hasModified) {
        return;
    }

    if (!isInsertMoreRows) {
        table.hasModified = true;
    }

    addHeaderCell = function (row) {
        var th = document.createElement('th');
        th.setAttribute('scope', 'col');
        return row.appendChild(th);
    };
    var row = table.insertRow(0),
        cell = null,
        cellIndex = 1;

    var tableHeader = $(table).find("thead tr:last-child th");
    var childHeader = $(table).find("thead tr:last-child");
    $(childHeader).find("th:first-child").addClass('start-border');
    $(childHeader).find("th:last-child").addClass('end-border');
    $(childHeader).find("th:nth-last-child(2)").addClass('end-border');
    if (!isInsertMoreRows) {
        $(childHeader).find("th:first-child").addClass('border-right');
    }

    if (isIngoreColNone) {
        if (isIngoreColNone === true) {
            var arrDisplay = [];
            var headerIndex = 0;    
            for (var j = 0; j < tableHeader.length; j++) {
                if (tableHeader.eq(j).attr("style").replace(/\s/g, '').indexOf("display:none") == -1) {
                    j += headers[headerIndex][1] - 1;
                    arrDisplay.push(j);
                    headerIndex++;
                }
            }
        }
    }


    for (var i = 0; i < headers.length; i++) {

        cell = addHeaderCell(row);
        var colSpan = headers[i][1],
            num = cellIndex + (colSpan - 1),
            startHeader = $(table).find("thead tr:last-child th:nth-child(" + (cellIndex - 1) + ")"),
            endHeader = $(table).find("thead tr:last-child th:nth-child(" + num + ")");

        cell.innerHTML = headers[i][0];
        cell.setAttribute('colSpan', colSpan);
        if (isIngoreColNone) {
            if (isIngoreColNone === true) {
                if ((i < headers.length - 1 ? headers[i + 1][1] != 1 : true) ||
                  (i > 0 ? headers[i - 1][1] != 1 : true) && i != 0) {
                    tableHeader.eq(arrDisplay[i]).addClass('end-border');
                }
            } else {
                $(startHeader).addClass('end-border');
                $(endHeader).addClass('end-border');
            }
        } else if (colSpan > 1) {
            $(startHeader).addClass('end-border');
            $(endHeader).addClass('end-border');
        } else if (!isInsertMoreRows) {
            $(startHeader).addClass('end-border');
            $(endHeader).addClass('end-border');
        }

        cell.className = headers[i][2] + (colSpan > 1 ? " group-header" : "");
        cell.style.textAlign = 'center';
        cell.setAttribute('align', 'center');
        cellIndex += colSpan;

    }

    $('.group-header').first().prev().addClass("end-border");

}
function addGroupHeadersForStaticTable(tableId, headers, isInsertMoreRows) {
    var table = document.getElementById(tableId);
    if (table == null) return;

    addHeaderCell = function (row) {
        var th = document.createElement('th');
        th.setAttribute('scope', 'col');
        return row.appendChild(th);
    };
    var row = table.insertRow(0);
    var cell = null,
        cellIndex = 2;

    for (var i = 0; i < headers.length; i++) {
        cell = addHeaderCell(row);

        var colSpan = headers[i][1],
           num = cellIndex + (colSpan - 1),
           startHeader = $(table).find("thead tr:last-child th:nth-child(" + (cellIndex - 1) + ")"),
           endHeader = $(table).find("thead tr:last-child th:nth-child(" + num + ")");

        cell.innerHTML = headers[i][0];
        cell.setAttribute('colSpan', colSpan);

        if (colSpan > 1 || isInsertMoreRows) {
            $(startHeader).addClass('end-border');
            $(endHeader).addClass('end-border');
        }

        cell.className = headers[i][2] + (colSpan > 1 ? " group-header" : "");
        cell.style.textAlign = 'center';
        cell.setAttribute('align', 'center');
        cellIndex += colSpan;
    }

    $('.group-header').first().prev().addClass("end-border");

}
function addGroupHeadersForRadGrid(gridId, headers) {
    var table = document.getElementById(gridId + '_GridHeader');
    table = table.getElementsByTagName('table');
    for (var i = 0; i < table.length; i++) {
        if (table[i].className.indexOf('rgMasterTable') >= 0) {
            table = table[i];
            break;
        }
    }
    addHeaderCell = function (row) {
        var th = document.createElement('th');
        th.setAttribute('scope', 'col');
        return row.appendChild(th);
    };
    var row = table.insertRow(0);
    var cell = null;
    for (var i = 0; i < headers.length; i++) {
        cell = addHeaderCell(row);

        if (headers[i][1] == 1) {
            cell.setAttribute('rowSpan', 2);
            cell.innerHTML = table.rows[1].cells[currentColoumn].innerHTML;
            table.rows[1].removeChild(table.rows[1].cells[currentColoumn]);
            currentColoumn--;
        }
        else {
            cell.innerHTML = headers[i][0];
            cell.setAttribute('colSpan', headers[i][1]);

        }
        currentColoumn += headers[i][1];
        cell.className = headers[i][2];
        cell.style.textAlign = 'center';
        cell.setAttribute('align', 'center');
    }
    if ($.browser.msie && parseInt($.browser.version) == 7) {
        var defaultCells = table.rows[1].cells;
        var cellCount = defaultCells.length;

        row = table.insertRow(0);
        for (var i = 0; i < cellCount; i++) {
            cell = addHeaderCell(row);

            cell.className = defaultCells[i].className;

        }
    }
    $(document).ready(function () {
        adjustHeightStaticHeaderGrid(gridId);
    });
}

function addScrollForGidData(gridId, maxPageSizeToStartScroll) {

    maxPageSizeToStartScroll = maxPageSizeToStartScroll || 20;
    var gridContainer = $('#' + gridId);
    var gridDataContainer = gridContainer.find(".rgDataDiv");
    //var pageSize = parseInt(gridContainer.find("[id$='pagerPageSizeComboBox_Input']").val() || 10);
    var tableRows = gridDataContainer.find("table.rgMasterTable > tbody > tr");
    var rows = parseInt(tableRows.length || 0);

    if (rows > maxPageSizeToStartScroll) {
        var totalHeightOfFirst20Row = 0;
        tableRows.each(function (index, item) {
            if (index < 20) {
                totalHeightOfFirst20Row += $(item).outerHeight();
            }
        });
        gridDataContainer.css("max-height", totalHeightOfFirst20Row + "px");

    } else {
        gridDataContainer.css("max-height", "inherit");
    }
}

function addValidateForFilterBox() {
    if ($(".RadGrid input[type=text].rgFilterBox").length > 0) {
        $(".RadGrid input[type=text].rgFilterBox").each(function () {
            $(this).change(function (e) {
                if (isIncludeSpecialCharacters(this)) {
                    alert(content.AlertMsgSpecialCharacters);
                    removeSpecialCharacters(this);
                    this.focus();
                }
            })
        })
    }
    
}

function addScrollForGidDataAndValidFilterBox(gridId, maxPageSizeToStartScroll, isOverFlowable) {
    if (isOverFlowable == "True") {
        addScrollForGidData(gridId, maxPageSizeToStartScroll);
    }
    addValidateForFilterBox();
}