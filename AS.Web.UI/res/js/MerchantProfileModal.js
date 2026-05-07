
function validator() {
    if (!dovalitation()) {
        AdjustModalSize();
        return false;
    }
    return true;
}
function checkComment() {
    var uxRemainingCharacter = $("#uxTextLimit");
    var uxMessages = MerchantProfileModal_uxCommentText;
    var maxCharacter = 1000;

    var messagesText = uxMessages.val();
    var currentLegth = messagesText.length;

    if (currentLegth > maxCharacter) {
        uxMessages.val(messagesText.substring(0, maxCharacter));
        currentLegth = maxCharacter;
        return false;
    }
    uxRemainingCharacter.html(parseInt(maxCharacter - currentLegth));
    return true;
}