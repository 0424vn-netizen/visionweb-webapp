function ShowHideTR_CC(tr, visible)
{
    if (visible == 'false')
        $('#' + tr).addClass('hide');
    else
        $('#'+ tr).removeClass('hide');
}

function ValidationBankAssgined()
{
    var hdfSelectedBank = document.getElementById(SelectedBank_ClientID);
    if (hdfSelectedBank.value == 'true')
        return true;
    return false;
}

function DisableClientContol()
{
    $('a.aspNetDisabled').each(function ()
    {
        if(!this.hasAttribute('js-ignore-disable-rule'))
            $(this).attr('href', '#').attr('onclick', 'return false;');
    })
}
