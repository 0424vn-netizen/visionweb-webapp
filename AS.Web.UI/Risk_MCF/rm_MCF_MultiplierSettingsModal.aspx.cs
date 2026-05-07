using System;
using AS.Common.DBManager;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using AS.Controls.Pages;

[PagePermission("RskMerClass,MSRskMerClass")]
public partial class rm_MCF_MultiplierSettingsModal : NonReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindStatus
    }

    enum MultiplierSetting
    { 
        Highest = 1,
        Lowest =2
    }

    enum PostBackAction
    {
        Close,
        Update
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;

        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindStatus);

        }       
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindStatus:
                BindStatus();
                break;
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Update);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Update:
                UpdateSetting();

                break;

        }
    }

    private int GetSetting()
    {
        ListItem item =  uxRadioMode.Items.Cast<ListItem>().ToList().FirstOrDefault(t=>t.Selected);
        if (item != null)
            return item.Value.ToInt();
        else return 1;
    }

    private void UpdateSetting()
    {

        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParamsWithRecId();

        parames.Add(new FilterParameter("@StatusMS ", GetSetting(), DbType.Int32));
        parames.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_MRS_Save_MultiplierSetting", parames, out paramesOut);

        FilterParameter returnValue = paramesOut.FindFilterParameterByName("@ReturnValue", true);

        if (returnValue != null)
        {
            //Reload parent list
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "CloseModal", "ClosePopupModal();", true);
        }
    }

    private void BindStatus()
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParamsWithRecId();

        parames.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));
        
        FilterParameterCollection paramesOut = new FilterParameterCollection();

        // Get OutParams value
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_MRS_Get_MultiplierSetting", parames, out paramesOut);

        FilterParameter isIncluded = paramesOut.FindFilterParameterByName("@ReturnValue", true);

        if (isIncluded != null)
        {
            uxRadioMode.Items[0].Selected = isIncluded.ParameterValue.ToInt() == 1; // 1 is Highest
            uxRadioMode.Items[0].Value = string.Format("{0:D}", MultiplierSetting.Highest);
            uxRadioMode.Items[1].Selected = !uxRadioMode.Items[0].Selected;
            uxRadioMode.Items[1].Value = string.Format("{0:D}", MultiplierSetting.Lowest); 
        }
    }

}