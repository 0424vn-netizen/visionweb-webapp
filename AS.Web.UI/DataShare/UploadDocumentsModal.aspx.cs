using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UploadDocumentsModal : NonReportPage
{
    public int DocID
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["docId"]))
                return int.Parse(SecureQueryString["docId"]);
            else
                return -1;
        }
    }

    public bool IsUpdateMode
    {
        get
        {
            return DocID > 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;

        if (!IsPostBack)
        {
            msgLt.Text = string.Format(GetLocalResourceObject("ltInfoFile.Text").ToString(), GetAllowedFileTypeText(GeneralFuncsLib.DataShare_AllowFileType), GeneralFuncsLib.DataShare_MaximumFiles, GeneralFuncsLib.DataShare_MaximumFileSize);
            LoadData();

            if (IsUpdateMode)
            {
                BindDataUpload();
            }
        }
    }

    private string GetAllowedFileTypeText(string unStandardString)
    {
        if (unStandardString.IsNullOrEmpty()) return string.Empty;
        var fileTypes = unStandardString.ToUpper().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (fileTypes.Length == 1)
        {
            return fileTypes.Aggregate((a, b) => a + ", " + b);
        }
        return string.Format("{0} {1} {2}", fileTypes.Take(fileTypes.Length - 1).Aggregate((a, b) => a + ", " + b), GetLocalResourceObject("lblAnd").ToString(), fileTypes[fileTypes.Length - 1]);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            if (!IsUpdateMode)
            {
                SubmitCase();
            }
            else
            {
                UpdateCase();
            }
        }

        Page.ClientScript.RegisterStartupScript(this.GetType(), "reload", "parent.reloadDocuments(); ", true);
    }

    protected void SubmitCase()
    {
        string xmlData = GetXMLData();
        var shareWithUser = GetListOfValues(uxShareUsers.SelectedItems);
        var shareWithGroup = GetListOfValues(uxShareGroups.SelectedItems);
        var expirationDate = uxExpirationDate.SelectedDate;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@UserList", shareWithUser, DbType.String));
        parameters.Add(new FilterParameter("@GroupList", shareWithGroup, DbType.String));
        parameters.Add(new FilterParameter("@ExpirationDate", expirationDate, DbType.DateTime));
        parameters.Add(new FilterParameter("@Attachment", xmlData, DbType.String));

        WebServices.CsReportServices.GetReports("spa_cs_DataShare_CreateDocuments", parameters);
    }

    protected void UpdateCase()
    {
        var shareWithUser = GetListOfValues(uxShareUsers.SelectedItems);
        var shareWithGroup = GetListOfValues(uxShareGroups.SelectedItems);
        var expirationDate = uxExpirationDate.SelectedDate;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new AS.Common.DBManager.FilterParameter("@UserList", shareWithUser, DbType.String));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@GroupList", shareWithGroup, DbType.String));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@Description", uxDescription.Text, DbType.String));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@ExpirationDate", expirationDate, DbType.DateTime));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@DocumentName", uxDocumentName.Text, DbType.String));
        parameters.Add(new AS.Common.DBManager.FilterParameter("@DocID", DocID, DbType.Int32));

        WebServices.CsReportServices.GetReports("spa_cs_DataShare_EditDocuments", parameters);
    }

    private string GetListOfValues(ListItemCollection items)
    {
        string result = string.Empty;
        foreach (ListItem i in items)
        {
            result += i.Value + ',';
        }
        return result.Trim(',');
    }

    private string GetXMLData()
    {
        string dataXml = string.Empty;
        List<FileUploadModel> model = new List<FileUploadModel>();

        uxUpload.GetDataFromUI();
        List<UploadItemModel> ds = uxUpload.DataSourceRpt;

        foreach (UploadItemModel item in ds)
        {
            long docId = UploadToDocServer(item);
            if (docId != -1)
            {
                FileUploadModel itemTemplate = new FileUploadModel()
                {
                    FileName = item.FileName,
                    Note = item.Note,
                    ServerDocumentID = docId,
                    FileSize = item.ContentLength / 1000,
                    Extension = item.Extension

                };
                model.Add(itemTemplate);
            }
        }

        if (model != null && model.Count > 0)
        {
            dataXml = AS.Common.Utilities.SerializationUtil.SerializeObjectToXMLString(model, typeof(List<FileUploadModel>));
            dataXml = dataXml.Replace("FileUploadModel", "Item").Replace("ArrayOfItem", "Attachment");
        }

        return dataXml;
    }

    private long UploadToDocServer(UploadItemModel uploadedFile)
    {
        long docId = -1;
        byte[] buffer = uploadedFile.DataBytes;

        docId = WebServices.DocServices.UploadDoc(SessionManager.CurrentClient,
            SessionManager.CurrentUser.UserID,
             buffer, uploadedFile.FileName, (string)Application["WebSiteIPAddr"]);

        return docId;
    }

    private bool ValidateInput()
    {
        if (!ValidateUserAndGroup() || !uxUpload.IsValid())
            return false;
        return true;
    }

    private bool ValidateUserAndGroup()
    {
        //if (string.IsNullOrEmpty(uxShareGroups.SelectedValue) && string.IsNullOrEmpty(uxUserListTextHide.Value))
        //    return false;

        return true;
    }

    private void ShowMessageBox(string message)
    {
        message = String.Format("setTimeout(\"alert('{0}');\",100);", message).ToString();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlerMessage", message, true);
    }

    private bool CheckExpression(System.Text.RegularExpressions.Regex reg, string str_value)
    {
        return (reg.IsMatch(str_value) && reg.Matches(str_value)[0].Length == str_value.Length);
    }

    private void LoadData()
    {
        uxShareUsers.Placeholder = GetLocalResourceObject("phSelectUser").ToString();
        uxShareGroups.Placeholder = GetLocalResourceObject("phSelectGroup").ToString();
        SetExpirationDate(null);
        LoadUserList();
        LoadGroupList();
    }

    private void BindDataUpload()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@DocID", DocID, DbType.Int32));
        var data = WebServices.CsReportServices.GetReports("spa_cs_DataShare_GetDocumentUploadDetail", parameters);

        if (data != null)
        {
            SetGeneralInfo(data);
            phUploadFile.Visible = false;
            phFileInfo.Visible = true;
            phDocumentName.Visible = true;
            uxDocumentName.Enabled = false;
        }
    }

    private void SetGeneralInfo(DataTable data)
    {
        var itemUsers = data.Rows[0]["ShareWithUserIDs"].ToString().Split(',');
        uxShareUsers.SetSelectedValue(itemUsers);

        var itemGroups = data.Rows[0]["ShareWithGroupsID"].ToString().Split(',');
        uxShareGroups.SetSelectedValue(itemGroups);

        int fileSize = 0;
        int.TryParse(data.Rows[0]["FileSize"].ToString(), out fileSize);
        Literal3.Text = GetLocalResourceObject("ltEditDocumentResources.Text").ToString();
        uxDescription.Text = data.Rows[0]["Description"].ToString();
        uxFileName.Text = data.Rows[0]["DocumentName"].ToString();
        uxTitleFileName.Attributes.Add("title", data.Rows[0]["DocumentName"].ToString());
        uxDocumentName.Text = data.Rows[0]["DocumentName"].ToString();
        uxFileType.Attributes.Add("src", string.Format("../res/images/IconsFile/{0}.svg",
            data.Rows[0]["FileType"].ToString().Replace(".", string.Empty)));
        uxFileSize.InnerText = fileSize < 1000 ? fileSize.ToString() + "KB" :
            ((float)fileSize / 1000).ToString("0.00") + "MB";

        SetExpirationDate(data);
    }

    private void LoadUserList()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@PageName", "UploadDocuments", DbType.String));
        DataTable data = WebServices.CsReportServices.GetReports("spa_cs_DataShare_GetUserList", parameters);

        uxShareUsers.DataValueField = "UserID";
        uxShareUsers.DataTextField = "UserName";
        uxShareUsers.DataSource = data;
        uxShareUsers.DataBind();
    }

    private void LoadGroupList()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        DataTable data = WebServices.CsReportServices.GetReports("spa_cs_DataShare_GetGroups", parameters);

        uxShareGroups.DataValueField = "GroupUserID";
        uxShareGroups.DataTextField = "GroupName";
        uxShareGroups.DataSource = data;
        uxShareGroups.DataBind();
    }

    private void SetExpirationDate(DataTable data)
    {
        //resfeh max min
        uxExpirationDate.MinDate = DateTime.MinValue;
        uxExpirationDate.MaxDate = DateTime.MaxValue;

        if (data != null)
        {
            DateTime itemSelected = (DateTime)data.Rows[0]["ExpirationDate"];
            DateTime maxDate = (DateTime)data.Rows[0]["ExpirationDateMax"];
            uxExpirationDate.SelectedDate = itemSelected;
            if (itemSelected.Date < DateTime.Now.Date)
            {
                uxExpirationDate.Enabled = false;
            }
            else
            {
                uxExpirationDate.MinDate = DateTime.Now;
                uxExpirationDate.MaxDate = maxDate;
            }
        }
        else
        {
            DateTime nowDate = DateTime.Now;
            uxExpirationDate.SelectedDate = nowDate.AddDays(3);
            uxExpirationDate.MinDate = nowDate;
            uxExpirationDate.MaxDate = nowDate.AddDays(29);
        }
    }

    protected void uxDescription_PreRender(object sender, EventArgs e)
    {
        uxDescription.EmptyMessage = GetLocalResourceObject("phDescription").ToString();
    }
}