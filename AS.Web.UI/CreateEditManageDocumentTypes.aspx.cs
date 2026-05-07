using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateEditManageDocumentTypes : ReportPage
{
    #region enum
    enum PostBackAction
    {

    }

    enum DataBindAction
    {
        BindDocumentType,
        BindDocumentTypeDetail
    }
    enum Status
    {
        Active,
        Deactive
    }
    int Source
    {
        get
        {
            if (IsSecureQueryString)
            {
                return int.Parse(SecureQueryString["Source"].ToString());
            }
            else
            {
                return 0;
            }
        }
    }
    int DocumentTypeID
    {
        get
        {
            if (IsSecureQueryString)
            {
                return int.Parse(SecureQueryString["DocumentTypeID"].ToString());
            }
            else
            {
                return 0;
            }
        }
    }

    string SourceName
    {
        get
        {
            if (IsSecureQueryString)
            {
                return SecureQueryString["SourceName"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
    bool IsNotDeactive
    {
        get
        {
            if (ViewState["IsNotDeactive"] != null)
                return bool.Parse(ViewState["IsNotDeactive"].ToString());
            else
                return false;
        }
        set
        {
            ViewState["IsNotDeactive"] = value;
        }
    }

    #endregion
    #region events


    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
        if (DocumentTypeID == 0)
        {
            Page.Title = GetLocalResourceObject("pageTitleresourceAddNew").ToString();
        }
        else
        {
            Page.Title = GetLocalResourceObject("pageTitleresourceEdit").ToString();

        }
        if (IsUserWithPermission("ManageDocumentTypesAlice") && IsUserWithPermission("ManageDocumentTypesCMS"))
        {
            uxRowSource.Visible = true;
        }
        else
        {
            foreach (AS.Controls.Validators.ValidationItem item in uxValidator.Items)
            {
                if (item.ControlToValidateID.Equals("uxSource"))
                {
                    uxValidator.Items.Remove(item);
                    break;
                }
            }
            uxRowSource.Visible = false;
        }
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindDocumentType, uxSource);
            uxSource.SetSelectedValue(Source.ToString().Split(' '));

            if (DocumentTypeID != 0)
            {
                OnDataBindControls(DataBindAction.BindDocumentTypeDetail, uxSource);
            }
            if (IsUserWithPermission("ManageDocumentTypesAlice") && !IsUserWithPermission("ManageDocumentTypesCMS") && !IsUserWithPermission("MSManageDocumentTypesCMS"))
            {
                uxSource.SetSelectedValue(new string[] { "1" });
            }
            if (!IsUserWithPermission("ManageDocumentTypesAlice") && (IsUserWithPermission("ManageDocumentTypesCMS") || IsUserWithPermission("MSManageDocumentTypesCMS") || IsUserWithPermission("SponsorManageDocumentTypes")))
            {
                uxSource.SetSelectedValue(new string[] { "2" });
            }

        }
    }
    protected override void OnDataBindControls(Enum type, object sender)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindDocumentType:
                parameters.AddLanguageID();
                parameters.Add("@ASClient", SessionManager.CurrentClient, DbType.Int32);
                parameters.Add("@RefType", "ManageDocumentTypesSource", DbType.String);
                uxSource.DataTextField = "KeyName";
                uxSource.DataValueField = "KeyValue";
                uxSource.DataSource = WebServices.CsReportServices.GetReports("spa_GetRefTableValues", parameters);
                uxSource.DataBind();
                break;

            case DataBindAction.BindDocumentTypeDetail:
                parameters.AddLanguageID();
                parameters.AddLoggedInUserParamsWithRecId();
                parameters.Add("@SourceID", Source, DbType.Int32);
                parameters.Add("@DocumentTypeID", DocumentTypeID, DbType.Int32);
                DataTable dt = WebServices.SecurityServices.GetReports("spa_MDT_Get_DocumentType", parameters);
                if (dt.HasData())
                {
                    uxDocumentTypeID.Value = dt.Rows[0]["DocumentTypeID"].ToString();
                    uxDocumentTypeName.Text = dt.Rows[0]["DocumentType"].ToString();
                    uxDescription.Text = dt.Rows[0]["Description"].ToString();
                    IsNotDeactive = dt.Rows[0]["IsNotDeActive"].ToBoolean();
                    uxSourceName.Text = dt.Rows[0]["SourceName"].ToString();
                    uxSourceName.Visible = true;
                    uxSource.Visible = false;

                    if (dt.Rows[0]["IsActive"].ToBoolean())
                    {
                        uxActive.Checked = true;
                        uxDeActive.Checked = false;
                    }
                    else
                    {
                        uxActive.Checked = false;
                        uxDeActive.Checked = true;
                    }
                }
                break;
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        if (uxValidator.IsValid())
        {
            if (IsNotDeactive && uxDeActive.Checked)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ManageDocumentTypes_Message", "parent.ShowPopupModalChild(1, 'ManageDocumentTypes_Message.aspx', 'auto');", true);
            }
            else
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParamsWithRecId();

                string sourceIds = string.Empty;

                if (DocumentTypeID == 0)
                {
                    var sources = new List<string>();
                    foreach (ListItem item in uxSource.SelectedItems)
                    {
                        sources.Add(item.Value);
                    }

                    sourceIds = string.Join(",", sources);
                }

                var model = new AS.Web.Business.ReportService.DocumentTypeModel() 
                { 
                    Id = DocumentTypeID , 
                    Name = uxDocumentTypeName.Text,
                    Description = uxDescription.Text,
                    IsActive = uxActive.Checked,
                    SourceId = Source,
                    SourceIdList = sourceIds,
                    IsSyncData = GeneralFuncsLib.HasShadowUnderwriting()                    
                };

                var result = WebServices.CsReportServices.CreateUpdateDocumentType(SessionManager.CurrentUser.UserID, parameters, model);

                if (result == 0)
                {
                    uxDocumentTypeMsg.Message = GetLocalResourceObject("DocumentTypeExists_0").ToString();
                    uxDocumentTypeMsg.ShowOnLoad = true;
                    uxlbuxDocumentType.CssClass = "control-label label-error";
                }
                else if (result == -1)
                {
                    uxDocumentTypeMsg.Message = GetLocalResourceObject("DocumentTypeExists_1").ToString();
                    uxDocumentTypeMsg.ShowOnLoad = true;
                    uxlbuxDocumentType.CssClass = "control-label label-error";
                }
                else if (result == -2)
                {
                    uxDocumentTypeMsg.Message = GetLocalResourceObject("DocumentTypeExists_2").ToString();
                    uxDocumentTypeMsg.ShowOnLoad = true;
                    uxlbuxDocumentType.CssClass = "control-label label-error";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DoClose", "parent.DoClose();", true);
                }
            }
        }
    }
    #endregion

}