using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;

namespace As.VisionWeb.Web
{
    public partial class SubsiteDistributionsUserControl : GlobalUserControl, IRiskParamFilter
    {
        #region properties
        public int HeightUC { get; set; }
        public int WidthUC { get; set; }
        public string HierarchyFilterText { get; set; }
        public string HierarchyFilterMode { get; set; }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            int outputPrimID = 0;

            if (string.IsNullOrEmpty(PrimaryID))
                return;

            if (!int.TryParse(this.PrimaryID, out outputPrimID))
                return;

            DataTable listOri = null;
            DataTable listDes = null;
            var isInclude = true;

            if (int.Parse(PrimaryID) == 0)
            {
                listOri = GetFilterList(WebSiteEnums.AssignmentFilterModes.All, 0, (int)Mode, ref isInclude);
                listDes = listOri.Clone();
            }
            else
            {
                listOri = GetFilterList(WebSiteEnums.AssignmentFilterModes.NotAssigned, outputPrimID, (int)Mode, ref isInclude);
                listDes = GetFilterList(WebSiteEnums.AssignmentFilterModes.Assigned, outputPrimID, (int)Mode, ref isInclude);
            }

            uxHierarchyFilter.DataSourceOrigination = listOri;
            uxHierarchyFilter.DataSourceDestination = listDes;
        }        
        private DataTable GetFilterList(WebSiteEnums.AssignmentFilterModes whichMode, int primaryID, int mode, ref bool isExclude)
        {
            var currentUser = SessionManager.CurrentUser;
            FilterParameterCollection parames = new FilterParameterCollection();

            parames.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
            parames.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
            parames.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));
            
            if (currentUser.SiteID >= 0)
                parames.Add(new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32));

            parames.Add(new FilterParameter("@AssignmentID", primaryID, DbType.Int32));
            parames.Add(new FilterParameter("@OptionID", whichMode, DbType.Int32));
            parames.Add(new FilterParameter("@Mode", mode, DbType.Int32));
            parames.Add(new FilterParameter("@ItemCode", "SubSiteDistribution", DbType.String));
            parames.Add(new FilterParameter("@LanguageID", SessionManager.CurrentLanguage, DbType.Int32));
            var data = WebServices.RiskServices.GetReportsAsDataSet("spa_MCF_Get_AssignmentItem", parames);
            
            if (data.Tables.Count > 0)
            {
                return data.Tables[0];
            }

            return new DataTable();
        }        
        protected void MultiSelector_OnMovedData(object sender, EventArgs e)
        {
            if (IsPostBack && SelectedChanged != null)
            {
                this.SelectedChanged(this, e);
            }
        }      

        #region IRiskParamFilter Members

        public WebSiteEnums.ParamFilterMode Mode { get; set; }

        public string PrimaryID { get; set; }
        public string ParamID { get; set; }
        public string FilterID { get; set; }
        public int Save()
        {
            int primaryId = 0;
            if (string.IsNullOrEmpty(PrimaryID) || !int.TryParse(PrimaryID, out primaryId))
                return 1;

            string itemValues = uxHierarchyFilter.GetSelectedItemsAsString(",");
            var currentUser = SessionManager.CurrentUser;
            FilterParameterCollection parames = new FilterParameterCollection();

            parames.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
            parames.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
            parames.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));

            if (currentUser.SiteID >= 0)
                parames.Add(new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32));

            parames.Add(new FilterParameter("@AssignmentID", primaryId, DbType.Int32));
            parames.Add(new FilterParameter("@Mode", this.Mode, DbType.Int32));
            parames.Add(new FilterParameter("@ItemCode", "SubSiteDistribution", DbType.String));
            parames.Add(new FilterParameter("@ItemValue", itemValues, DbType.String));
            WebServices.RiskServices.GetReports("spa_MCF_Save_AssignmentItem", parames);
            return 0;
        }
        public string SaveIncludeExcludeISONumber()
        {
            string _CodeList = uxHierarchyFilter.GetSelectedItemsAsString(",");
            SessionManager.IncludeExcludeItem = _CodeList;
            return _CodeList;
        }

        public event EventHandler SelectedChanged;

        #endregion
    }
}

