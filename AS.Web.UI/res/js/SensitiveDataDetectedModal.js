function checkBoxClick(object, cardKey) {    
    if (object.checked) {
        cardKeyList += cardKey + ","
        document.getElementById(hdCardSelected).value = cardKeyList.substring(0, cardKeyList.length - 1);
        $('#' + btnSubmit).prop('disabled', false);

    }
    else {
        cardKeyList = cardKeyList.replace(cardKey + ",", "");
        document.getElementById(hdCardSelected).value = cardKeyList.substring(0, cardKeyList.length - 1);
        if (cardKeyList == "") {
            $('#' + btnSubmit).prop('disabled', true);
        }
    }
}

function submitNote(isDisregard, action) { 
    var lstCard = isDisregard ? "" : cardKeyList;
    if (action === 'Edit') {
        parent.SubmitEditNote(lstCard);
        return;
    }
    parent.SubmitAddNote(lstCard, true);
}

function buildContent() {
    var cards = parent.window.cardNumbers;
    var htmlData = "";
    $.each(cards, function (index, value) {
        var cls = index % 2 == 1 ? "AltRow" : "Row";
        htmlData += '<tr class=' + cls + '> <td class="text-left view">'
        htmlData += '<span class="control-inline">';
        htmlData += '<input type="checkbox" onclick="checkBoxClick(this,' + "'" + value + "'" + ')" id="checkbox' + index + '" />';
        htmlData += '<label for="checkbox"' + index + '>' + value + '</label>';
        htmlData += "</span> </td> </tr>";
    })

    $('#header').after(htmlData);
}

$(document).ready(function () {
    buildContent();
})