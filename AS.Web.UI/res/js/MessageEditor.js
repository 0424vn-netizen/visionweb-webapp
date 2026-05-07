function CountRemainingCharacter() {
    var uxRemainingCharacter = MessageEditor_uxRemainingCharacter;
    var uxMessages = MessageEditor_uxMessage;
    var maxCharacter = MessageEditor_MaxCharacter;

    var messagesText = uxMessages.val();
    var currentLegth = messagesText.length;

    if (currentLegth > maxCharacter) {
        uxMessages.val(messagesText.substring(0, maxCharacter));
        currentLegth = maxCharacter;
    }
    uxRemainingCharacter.html(Text_YouHave + ' ' + parseInt(maxCharacter - currentLegth) + ' ' + Text_CharsRemaining);

}

function GetMessageText() {
    var uxMessages = MessageEditor_uxMessage;
    return uxMessages.val();
}

function ClearMessage() {
    var uxMessages = MessageEditor_uxMessage;
    uxMessages.val("");
    var uxRemainingCharacter = MessageEditor_uxRemainingCharacter;
    uxRemainingCharacter.html(Text_YouHave + ' ' + MessageEditor_MaxCharacter + ' ' +Text_CharsRemaining);
}
function FocusTextBox() {
    var uxMessages = document.getElementById(MessageEditor_idMessage);
    uxMessages.focus();
}