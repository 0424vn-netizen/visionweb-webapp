using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.NetCore.Api.Client;
using AS.NetCore.Api.Client.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Data;
using Telerik.Web.UI;

[PagePermission("AddEditChain")]
public partial class ModifyMerchantChainModal : NonReportPage
{
    enum PostBackAction
    {
        UpdateChain
    }
    string _MerchantNumber = string.Empty;
    string _ChainNumber = string.Empty;
    private void ProcessQueryString()
    {
        if (IsSecureQueryString)
        {
            _MerchantNumber = SecureQueryString["MerchNum"];
            _ChainNumber = SecureQueryString["Chain"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            if (IsSecureQueryString)
            {
                ProcessQueryString();
                if (_ChainNumber.Trim().IsNullOrEmpty())
                {
                    uxLtrDetails.Text = VeraCodeSolution.DoVeraCode(
                        string.Format(GetLocalResourceObject("ModifyMerchantChainModal_aspx_cs_ToAddMerchantToExistingChain").ToString(), _MerchantNumber)
                        );
                }
                else
                {
                    uxLtrDetails.Text = VeraCodeSolution.DoVeraCode(
                        string.Format(GetLocalResourceObject("ModifyMerchantChainModal_aspx_cs_MerchantIsPartOfChain").ToString(), _MerchantNumber, _ChainNumber)
                        );
                    uxPlcAdd.Visible = true;
                }
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        base.OnPostBackActions(type, sender);
        switch ((PostBackAction)type)
        {
            case PostBackAction.UpdateChain:
                {
                    UpdateChain();
                }
                break;
        }
    }

    protected void UpdateChain()
    {
        if (IsSecureQueryString)
        {
            string chainSrc = SecureQueryString["Chain"].ToString();
            string merchantNumber = SecureQueryString["MerchNum"];
            string spaName = "spa_UpdateChainNumber";

            bool isUpdateMicro = GeneralFuncsLib.GetDataOfExtendedSetting("UpdateChainToMicro") == "true";
            if (isUpdateMicro)
            {
                UpdateChainNumberToMicroDB(merchantNumber, uxChain.SelectedValue.Trim());
            }

            FilterParameterCollection parameters = new FilterParameterCollection();
            FilterParameterCollection paramOuts = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
            parameters.Add(new FilterParameter("@ChainNumber", uxChain.SelectedValue, DbType.AnsiString));
            WebServices.CsReportServices.ExecuteNonQueryCommand(spaName, parameters, out paramOuts);

            //insert a comment       
            if (chainSrc.Trim() != uxChain.SelectedValue.Trim())
            {
                parameters = new FilterParameterCollection();
                FilterParameterCollection tempParameters = null;
                parameters.AddLoggedInUserReportingParams();
                parameters.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.AnsiString));
                if (chainSrc.IsNullOrEmpty())
                    parameters.Add(new FilterParameter("@Comment", GetLocalResourceObject("ModifyMerchantChainModal_aspx_cs_AddedToChain").ToString() + " " + uxChain.SelectedValue.Trim(), DbType.String));
                else
                {
                    if (!uxChain.SelectedValue.IsNullOrEmpty())
                        parameters.Add(new FilterParameter("@Comment", string.Format(GetLocalResourceObject("ModifyMerchantChainModal_aspx_cs_MoveFromChainToChain").ToString(), chainSrc) + " " + uxChain.SelectedValue.Trim(), DbType.String));
                    else
                        parameters.Add(new FilterParameter("@Comment", GetLocalResourceObject("ModifyMerchantChainModal_aspx_cs_DeleteFromChain").ToString() + " " + chainSrc, DbType.String));
                }
                WebServices.MsReportServices.ExecuteNonQueryCommand("spa_cs_InsertCommentsOfMerchant", parameters, out tempParameters);
            }
        }
    }

    protected BaseRequestData<T> InitRequestModel<T>(T model) where T : RequestBody, new()
    {
        var request = new BaseRequestData<T>()
        {
            Data = model,
        };

        request.Header.AsClientId = SessionManager.CurrentUser.ASClient;

        return request;
    }

    private void UpdateChainNumberToMicroDB(string merchantNumber, string chainNumber )
    {
        UpdateChainNumberRequest requestItem = new UpdateChainNumberRequest()
        {
            MerchantNumber = merchantNumber,
            ChainNumber = chainNumber
        };

        var request = InitRequestModel(requestItem);

        var client = new FapsApiClient();
        client.UpdateChainNumber(request);
    }

    protected void Save_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.UpdateChain);
        ClientScript.RegisterStartupScript(GetType(), "CloseModal", "UpdateChainSuccess();", true);
    }

    protected void uxChain_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 2)
        {
            string spaName = "spa_GetChainNumber";
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserReportingParams(true);
            parameters.Add(new FilterParameter("@ChainName", e.Text, DbType.String));
            uxChain.DataSource = WebServices.CsReportServices.GetReports(spaName, parameters);
            uxChain.DataBind();
        }
        else
        {
            uxChain.Items.Clear();
        }
    }
}
