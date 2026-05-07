using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public partial class rm_MCF_MessageCustomModal : ReportPage
{
    #region ---- Member variable  ----
    enum MessageModalType
    {
        OnlyShowMessage = 0,
        ManageCustomChange = 1,
        ManageCustomViewChange = 2,
        ManageCustomDelete = 3
    }

    const string COL_TEXT = "Text";
    //1 is Public to Private Assignments
    //2 is Public to Private TAV
    //3 is 
    private MessageModalType ModeMessage
    {
        get
        {
            if (SecureQueryString["Mode"] != null)
            {
                return (MessageModalType)Convert.ToInt32(SecureQueryString["Mode"]);
            }
            else
            {
                return MessageModalType.OnlyShowMessage;
            }
        }
    }

    protected string Data
    {
        get
        {
            if (SecureQueryString["Data"] != null)
            {
                return SecureQueryString["Data"];
            }
            else
            {
                return string.Empty;
            }
        }
    }
    protected string Message
    {
        get
        {
            if (SecureQueryString["Message"] != null)
            {
                return SecureQueryString["Message"];
            }
            else
            {
                return string.Empty;
            }
        }
    }
    #endregion ---- Member variable  ----
    #region ---- Propeties  ----
    #endregion ---- Propeties  ----
    #region ---- Events Methods ----
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            GetData();
        }
    }
    protected void uxSubmit_Click(object sender, EventArgs e)
    {

        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.EditColumnModal.CloseEditCustomViewModal();"));
    }
    #endregion ---- Events Methods ----
    #region ---- Private Methods ----
    public void GetData()
    {
        if (!string.IsNullOrEmpty(Data))
        {
            List<string> listData = Data.Split(',').ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(COL_TEXT, typeof(string)));
            (from c in listData where !c.Trim().Equals(string.Empty) select new { Text = c.Trim() }).ToList().ForEach(item =>
               {
                   DataRow dataRow = dt.NewRow();
                   dataRow[COL_TEXT] = item.Text;
                   dt.Rows.Add(dataRow);
               });
            uxRptMessage.DataSource = dt;
            uxRptMessage.DataBind();
        }
        if (ModeMessage == MessageModalType.OnlyShowMessage)
        {
            lblMessage.Text = Message;
            uxRptMessage.Visible = false;
            lblMesssageFooter.Visible = false;
        }
        else if (ModeMessage == MessageModalType.ManageCustomChange)
        {
            lblMessage.Text = GetLocalResourceObject("Message_" + GetCustomViewMessageResourceTypeMode.ToString() + "_Header").ToString();
            lblMesssageFooter.Text = GetLocalResourceObject("Message_"+ GetCustomViewMessageResourceTypeMode.ToString() + "_Footer").ToString();
        }
        else if (ModeMessage == MessageModalType.ManageCustomViewChange)
        {
            lblMessage.Text = GetLocalResourceObject("Message_TVA_Header").ToString();
            lblMesssageFooter.Text = GetLocalResourceObject("Message_TVA_Footer").ToString();
        }
        else if (ModeMessage == MessageModalType.ManageCustomDelete)
        {
            lblMessage.Text = GetLocalResourceObject("Message_"+ GetCustomViewMessageResourceTypeMode.ToString() + "_Delete_Header").ToString();
            lblMesssageFooter.Text = GetLocalResourceObject("Message_"+ GetCustomViewMessageResourceTypeMode.ToString() + "_Delete_Footer").ToString();
        }
    }
    #endregion ---- Private Methods ----
}