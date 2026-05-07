using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Services;

[PagePermission("RskMerClass,MSRskMerClass")]
public partial class rm_MCF_MerchantClassification_CreateNewModal : NonReportPage
{
    enum PostBackAction
    {
        Close,
        Update
    }

    protected string ClassificationId
    {
        get { return ViewState["ClassificationId"].ToString(); }
        set { ViewState["ClassificationId"] = value; }
    }

    protected bool IsEditMode
    {
        get {return !string.IsNullOrEmpty(ClassificationId);}
    }


    private void ShowData()
    {
        
        uxAttribute1.AttributeTable = GetClassificationInformation();
        if (uxAttribute1.AttributeTable != null && uxAttribute1.AttributeTable.Rows.Count > 0)
        {
            DataRow row = uxAttribute1.AttributeTable.Rows[0];
            uxClassificationText.Text = row["MerchantClassificationName"].ToString();
            uxMultiplier.Text = row["Multiplier"].ToString();            
        }
    }

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected)
            return;
        PageType = SecurePageType.Modal;

        if (!IsPostBack)
        {
            ClassificationId = (SecureQueryString !=null ? SecureQueryString["ClassId"]:string.Empty);

            uxAttribute1.ClassificationId = ClassificationId;
            hidClassificationID.Value = CryptorServices.Current.EncryptText(ClassificationId);

            if (IsEditMode)
            {
                Page.Title = GetLocalResourceObject("PageResource2.Title").ToString();
                litTitle.Text = GetLocalResourceObject("uxTitle2.Text").ToString();
                ShowData();
            }
            else {
                uxClassificationText.EmptyMessage = GetLocalResourceObject("ClassificationName.Text").ToString();
                uxMultiplier.Text = "1.0";
            }
            
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
                if (ValidateInput())
                {
                    SaveData(ClassificationId);
                    AjaxAddResponseScript("parent.RebindGrid();");
                }
                break;

        }
    }

    protected void lnkAddAttribute_Click(object sender, EventArgs e)
    {
        uxAttribute1.InsertNew();
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "asjust", "AdjustModalSize()", true);
    }
    
    #endregion

    #region Methods

    private bool ValidateInput()
    {
        bool isValid = true;

        isValid = CheckClassificationName() 
            && CheckMultiplier() 
            && uxAttribute1.CheckAtleastOneAttribute()
            && uxAttribute1.CheckDuplicateAttribute()
            && uxAttribute1.ValidateDataAttribute();

        return isValid;
    }

    private bool CheckClassificationName()
    {
        bool isValid = true;
        // Check Classification name
        string className = uxClassificationText.Text.Trim();
        // Check special characters
        string strRegex = WebSiteConstants.REG_SPECIAL_CHARACTERS;
        Regex re = new Regex(strRegex);

        if (string.IsNullOrEmpty(className))
        {
            ShowErrorMessageForClassificationName(string.Format(GetGlobalResourceObject("ValMsg", "CustomRequired").ToString(), "Classification Name"));
            isValid = false;
        }
        else if (!re.IsMatch(className))
        {
            ShowErrorMessageForClassificationName(GetLocalResourceObject("uxClassificationText_InvalidText.Message").ToString());
            isValid = false;
        }
        else
        {
            // Check duplicate
            // true -> ok         
            string json = rm_MCF_MerchantClassification_CreateNewModal.CheckDuplicateClassificationName(CryptorServices.Current.EncryptText(ClassificationId), className);
            if ("false".Equals(json))
            {
                ShowErrorMessageForClassificationName(GetLocalResourceObject("uxClassificationText_Duplicate.Message").ToString());
                isValid = false;
            }
        }

        return isValid;
    }

    private bool CheckMultiplier()
    {
       
        string multiplier = uxMultiplier.Text.Trim();
        if (string.IsNullOrEmpty(multiplier))
        {
            ShowErrorMessageForMultiplier(string.Format(GetGlobalResourceObject("ValMsg", "CustomRequired").ToString(), "Multiplier"));
            return false;
        } 

        if (decimal.Parse(multiplier) <=0)
        {
            ShowErrorMessageForMultiplier(GetLocalResourceObject("uxClassificationText_GreateThan0.Message").ToString());
            return false;
        }
        
        return true;
    }

    private void ShowErrorMessageForClassificationName(string message)

    {
        uxClassificationLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
        ShowServerErrorMessage(ValidatorMessage3, message);
        uxClassificationText.Focus();
    }


    private void ShowErrorMessageForMultiplier(string message)
    {
        uxMultiplierLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
        ShowServerErrorMessage(ValidatorMessage4, message);
        uxMultiplier.Focus();
    }

    private DataTable GetClassificationInformation()
    {
        DataTable tb = new DataTable();
        FilterParameterCollection prams = new FilterParameterCollection();
        prams.AddLoggedInUserParamsWithRecId();
        prams.AddLanguageID();
        prams.Add(new FilterParameter("@ClassificationID", ClassificationId, DbType.Int32));
        tb = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_MerchantClassificationDetails", prams);
        
        return tb;

    }

    private string GetData()
    {
        ClassificationAttribute data = uxAttribute1.GetData();
        string xml = AS.Common.Utilities.SerializationUtil.SerializeObjectToXMLString(data, typeof(ClassificationAttribute));

        return xml;
    }

    private DataTable SaveData(string attributeId)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();        
        parameters.AddLoggedInUserParamsWithRecId();
        // edit mode
        if (!string.IsNullOrEmpty(ClassificationId))
        {
            parameters.Add(new FilterParameter("@ClassificationID", ClassificationId, DbType.Int32));
        }
        parameters.Add(new FilterParameter("@ClassifictionName", uxClassificationText.Text, DbType.String));
        parameters.Add(new FilterParameter("@Multiflier", uxMultiplier.Text, DbType.Decimal));
        parameters.Add(new FilterParameter("@AttributeList", GetData(), DbType.Xml));

        DataTable tb = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Save_MerchantClassification", parameters);

        return tb;
    }
    

    [WebMethod(EnableSession = true)]
    public static string CheckDuplicateClassificationName(string classID, string name)
    {
        string value = VeraCodeExtensions.DoVeraCode(name);
        string ID = CryptorServices.Current.DecryptText(classID);
        FilterParameterCollection parameters = new FilterParameterCollection();

        parameters.AddLoggedInUserParamsWithRecId();
        if (!string.IsNullOrEmpty(ID))
        {
            parameters.Add(new FilterParameter("@MIFClassificationID", ID, DbType.Int32));
        }
        
        parameters.Add(new FilterParameter("@ClassificationName", value, DbType.String));
        parameters.Add(new FilterParameter("@ReturnValue", 1, DbType.Int32, true));

        FilterParameterCollection parameterOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_MRS_Check_DuplicateClassificationName", parameters, out parameterOut);

        int result = Convert.ToInt32(parameterOut[0].ParameterValue);

        bool valid = (result == 0 ? true : false);
        return valid.ToString().ToLower();
    }
        
    #endregion
}