using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using Telerik.Web.UI;
using AS.Common;

public partial class NotInMifDetailModal : ReportPage
{
    #region Properties

    public string FileType
    {
        get
        {
            if (IsSecureQueryString)
            {
                return SecureQueryString["fileType"].ToString();
            }
            return string.Empty;
        }
    }


    public string MerchantNumber
    {
        get
        {
            if (IsSecureQueryString)
            {
                return SecureQueryString["merchantNumber"].ToString();
            }
            return string.Empty;
        }
    }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (!IsPostBack)
        {
            //Visible report
            uxNotInMifAuth.Visible = FileType.Equals("Authorization", StringComparison.OrdinalIgnoreCase);
            uxNotInMifBatch.Visible = FileType.Equals("Batch", StringComparison.OrdinalIgnoreCase);
            uxNotInMifChargeback.Visible = FileType.Equals("Chargeback", StringComparison.OrdinalIgnoreCase);
            uxNotInMifRetrieval.Visible = FileType.Equals("Retrieval", StringComparison.OrdinalIgnoreCase);
        }
    }
}