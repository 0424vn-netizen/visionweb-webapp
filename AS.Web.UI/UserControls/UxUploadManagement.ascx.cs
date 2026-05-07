using AS.Common;
using AS.Common.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using RadComboBox = AS.Controls.Global.RadComboBox;
using RadTextBox = AS.Controls.Global.RadTextBox;

public partial class UserControls_UxUploadManagement : GlobalUserControl
{
    #region Properties
    public delegate void UploadEventHandler(object sender, UploadEventArgs removeEventArgs);
    public event UploadEventHandler UploadEvent;
    public bool IsTargetBrowser { get; set; }
    public UploadedFile UploadedFile { get; set; }
    public Func<List<UploadItemModel>, string, string> GetDataXml { get; set; }
    public List<UploadItemModel> DataSourceRpt
    {
        get
        {
            if (Session["DataSourceRpt"] == null)
                Session["DataSourceRpt"] = new List<UploadItemModel>();
            return (List<UploadItemModel>)Session["DataSourceRpt"];
        }
    }
    public string ToJsValidate { get; set; }
    public string JsError = string.Empty;
   
    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSourceRpt.Clear();
            uploadFile.MaxFileSize = GeneralFuncsLib.DataShare_MaximumFileSize * 1024 * 1024;
            uploadFile.AllowedFileExtensions = GeneralFuncsLib.GetAllowedFileTypeArray(GeneralFuncsLib.DataShare_AllowFileType);
        }
    }

    protected void uxRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        var model = e.Item.DataItem as UploadItemModel;
        var itemRpt = e.Item.FindControl("uxItemRepeater") as UserControls_UxUploadItemRepeater;
        Label lblText = itemRpt.FindControl("lblText") as Label;
        RadTextBox txtNote = itemRpt.FindControl("txtNote") as RadTextBox;
        Label lblIssueFile = itemRpt.FindControl("lblIssueFile") as Label;
        var uploading = itemRpt.FindControl("uploading");

        HtmlGenericControl uploadingBar = itemRpt.FindControl("progressBar") as HtmlGenericControl;
        HtmlGenericControl fileSize = itemRpt.FindControl("fileSize") as HtmlGenericControl;
        HtmlGenericControl fileIssue = itemRpt.FindControl("fileIssue") as HtmlGenericControl;
        var hdf = itemRpt.FindControl("hdfId") as HiddenField;
        lblText.Text = model.FileName.DoVeraCode();

        lblText.ToolTip = model.FileName.DoVeraCode();
        itemRpt.FileType = "../res/images/IconsFile/" + model.FileName.Substring(model.FileName.LastIndexOf(".") + 1) + ".svg";
        itemRpt.FileSize = (model.ContentLength / 1024).ToString();
        hdf.Value = model.Id.ToString();
        if (!CheckExtension(model))
        {
            lblIssueFile.Style.Add("color", "#FF0000");
            lblIssueFile.Attributes.Add("data-fileName", model.FileName);
            lblIssueFile.Attributes.Add("data-issue", "Invalid");
            lblIssueFile.Text = GetGlobalResourceObject("ValMsg", "Invalid_File_Type").ToString();
            itemRpt.FileType = "../res/images/IconsFile/invalid.svg";
            uploading.Visible = false;
            fileSize.Visible = false;
            uploadingBar.Visible = false;
            txtNote.Enabled = false;

        }
        else if (!CheckSize(model))
        {
            lblIssueFile.Style.Add("color", "#FF0000");
            lblIssueFile.Attributes.Add("data-fileName", model.FileName);
            lblIssueFile.Attributes.Add("data-issue", "Over");
            itemRpt.FileType = "../res/images/IconsFile/invalid.svg";
            lblIssueFile.Text = GetGlobalResourceObject("ValMsg", "File_Size_Over").ToString();
            uploading.Visible = false;
            uploadingBar.Visible = false;
            fileSize.Visible = false;
            txtNote.Enabled = false;
        }
        else if (model.DataBytes != null && model.DataBytes.Length > 0)
        {
            fileSize.Visible = true;
            fileIssue.Visible = true;
            uploading.Visible = false;
            uploadingBar.Visible = false;
            txtNote.Text = model.Note.DoVeraCode();
            hdf.Value = model.Id.ToString().DoVeraCode();
            fileIssue.Attributes["class"] += " hidden";
        }
        else
        {
            fileSize.Attributes["class"] += " hidden";
            fileIssue.Attributes["class"] += " hidden";
            uploading.Visible = true;
            uploadingBar.Visible = true;
            itemRpt.IdFileClient = model.IdFileClient;
        }
    }

    protected void btnClick_OnClick(object sender, EventArgs e)
    {
        AddUploadedControl();
    }

    protected void btnSubmit_OnClick(object sender, EventArgs e)
    {
        AddNewUploadControl();
        CreateJsValidate();
        if (UploadEvent != null)
        {
            var eventArgs = new UploadEventArgs();
            eventArgs.IsUpload = true;
            UploadEvent(sender, eventArgs);
        }
    }

    protected void uxItemRepeater_OnRemoveItem(object sender, UploadRemoveEventArgs removeeventargs)
    {
        GetDataFromUI();
        var itemRemove = DataSourceRpt.SingleOrDefault(x => x.Id == removeeventargs.IdControl);
        if (itemRemove != null)
        {
            DataSourceRpt.Remove(itemRemove);
            uxRepeater.DataSource = DataSourceRpt;
            uxRepeater.DataBind();
        }
        if (UploadEvent != null)
        {
            var eventArgs = new UploadEventArgs();
            eventArgs.IsDelete = DataSourceRpt.Count == 0;
            UploadEvent(sender, eventArgs);
        }
        CreateJsValidate();
    }

    protected void uxRepeater_PreRender(object sender, EventArgs e)
    {
        ((NonReportPage)this.Page).AjaxAddResponseScript("AdjustModalSize();");
    }

    #endregion

    #region Overrides
    #endregion

    #region Privates

    private void CreateJsValidate()
    {
        foreach (RepeaterItem item in uxRepeater.Items)
        {
            var itemRpt = item.FindControl("uxItemRepeater") as UserControls_UxUploadItemRepeater;
            var script = itemRpt.ToValidateJs;
            var pattern = "</?script.*>";
            var reg = new Regex(pattern);
            script = reg.Replace(script, "");
            ToJsValidate += script;
        }

        ToJsValidate = string.Format("function valRepeaterItem(){{var isItemRepeaterValid=true;{0} return isItemRepeaterValid;}} ;", ToJsValidate);
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "uxRepeaterScript", ToJsValidate, true);
    }

    private void AddNewUploadControl()
    {
        GetDataFromUI();
        List<UploadItemModel> list = DataSourceRpt;
        var idctrl = list.Count > 0 ? list.Max(x => x.Id) : 0;
        string dataFile = uxhiddenFields.Value;
        if (string.IsNullOrEmpty(dataFile))
            return;
        var listFileUpload = JsonConvert.DeserializeObject<List<UploadItemModel>>(dataFile);
        if (listFileUpload == null || listFileUpload.Count == 0)
            return;
        for (int i = 0; i < listFileUpload.Count; i++)
        {
            var item = new UploadItemModel();
            item.FileName = listFileUpload[i].FileName;
            item.Id = idctrl + 1;
            item.ContentLength = listFileUpload[i].ContentLength;
            item.Extension = "." + listFileUpload[i].FileName.Substring(listFileUpload[i].FileName.LastIndexOf(".") + 1);
            item.IdFileClient = item.FileName.ToLower();
            list.Add(item);
            idctrl++;
        }
        uxRepeater.DataSource = list;
        uxRepeater.DataBind();
    }

    private void AddUploadedControl()
    {
        var idctrl = DataSourceRpt.Count > 0 ? DataSourceRpt.Max(x => x.Id) : 0;
        var list = DataSourceRpt.Where(x => !string.IsNullOrEmpty(x.IdFileClient)).ToList();
        foreach (var item in list)
        {
            var file = GetFile(item.FileName);
            if (file != null)
            {
                byte[] dataBytes = new byte[item.ContentLength];
                file.InputStream.Read(dataBytes, 0, dataBytes.Length);
                item.DataBytes = dataBytes;
            }
            item.IdFileClient = string.Empty;
        }
        uploadFile.UploadedFiles.Clear();
    }

    private UploadedFile GetFile(string fileName)
    {
        for (int i = 0; i < uploadFile.UploadedFiles.Count; i++)
        {
            if (uploadFile.UploadedFiles[i].FileName.Equals(fileName))
            {
                var data = uploadFile.UploadedFiles[i];
                uploadFile.UploadedFiles.RemoveAt(i);
                return data;
            }
        }
        return null;
    }
    #endregion

    #region Publics

    public void ReLoadItems()
    {
        GetDataFromUI();
        uxRepeater.DataSource = DataSourceRpt;
        uxRepeater.DataBind();
        CreateJsValidate();
    }

    public void GetDataFromUI()
    {
        var items = uxRepeater.Items;

        foreach (RepeaterItem item in items)
        {
            var itemRpt = item.FindControl("uxItemRepeater") as UserControls_UxUploadItemRepeater;

            Label lblText = itemRpt.FindControl("lblText") as Label;
            RadTextBox txtNote = itemRpt.FindControl("txtNote") as RadTextBox;
            var hdf = itemRpt.FindControl("hdfId") as HiddenField;
            var idControl = string.IsNullOrEmpty(hdf.Value) ? 0 : Convert.ToInt32(hdf.Value);

            var modelEdit = DataSourceRpt.SingleOrDefault(x => x.Id == idControl);
            if (modelEdit != null)
            {
                modelEdit.FileName = lblText.Text;
                modelEdit.Note = txtNote.Text;
                modelEdit.IdFileClient = string.Empty;
            }
        }
    }

    public bool IsValid()
    {
        foreach (RepeaterItem item in uxRepeater.Items)
        {
            var itemRpt = item.FindControl("uxItemRepeater") as UserControls_UxUploadItemRepeater;
            var hdf = itemRpt.FindControl("hdfId") as HiddenField;
            var idControl = string.IsNullOrEmpty(hdf.Value) ? 0 : Convert.ToInt32(hdf.Value);
            var modelRpt = DataSourceRpt.SingleOrDefault(x => x.Id == idControl);

            if (modelRpt != null)
            {
                if (!CheckExtension(modelRpt))
                    return false;
                if (!CheckSize(modelRpt))
                    return false;
            }

            if (!itemRpt.IsValid())
                return false;
        }

        return true;
    }

    private bool CheckSize(UploadItemModel modelRpt)
    {
        return modelRpt.ContentLength <= uploadFile.MaxFileSize && modelRpt.ContentLength >= 0;
    }

    private bool CheckExtension(UploadItemModel modelRpt)
    {
        string allowedType = GeneralFuncsLib.GetAllowedFileTypeStandard(GeneralFuncsLib.DataShare_AllowFileType);
        var dataExtension = allowedType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        return dataExtension.Any(x => x.Equals(modelRpt.Extension, StringComparison.InvariantCultureIgnoreCase));
    }

    #endregion

    protected void btnClearAllUpload_Click(object sender, EventArgs e)
    {
        DataSourceRpt.Clear();
        uxRepeater.DataSource = null;
        uxRepeater.DataBind();
        CreateJsValidate();
    }
}