using AS.Common;
using System;


public partial class UserControls_MessageEditor : GlobalUserControl
{
    protected enum DataBindAction
    {
        CountRemainCharacters
    }

    #region Properties
    public int MaxCharacter
    {
        set
        {
            this.uxMessage.MaxLength = value;
        }
        get
        {
            return this.uxMessage.MaxLength;
        }
    }
    public string HeaderText
    {
        set
        {
            this.uxHeader.InnerHtml = VeraCodeSolution.DoVeraCode(value);
        }
        get
        {
            return this.uxHeader.InnerHtml;
        }
    }
    public string Message
    {
        set
        {
            this.uxMessage.Text = VeraCodeSolution.DoVeraCode(value);
            this.OnDataBindControls(DataBindAction.CountRemainCharacters, this);
        }
        get
        {
            return this.uxMessage.Text.Trim();
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        this.OnDataBindControls(DataBindAction.CountRemainCharacters, sender);
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        switch ((DataBindAction)type)
        {
            case DataBindAction.CountRemainCharacters:
                this.uxRemainingCharacter.InnerHtml = VeraCodeSolution.DoVeraCode(
                    string.Format(GetLocalResourceObject("MessageEditorJS_Text_YouHave").ToString() + " {0} " +
                        GetLocalResourceObject("MessageEditorJS_Text_CharsRemaining").ToString(), this.MaxCharacter - this.uxMessage.Text.Trim().Length));                    
                break;
        }
    }

}
