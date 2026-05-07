using System;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Reflection;
using System.ComponentModel;
using AS.Controls.Grid;
using AS.Controls.Selector;

namespace AS.Controls.Global
{

    /// <summary>
    /// Summary description for VWMultiSelector
    /// </summary>
    public class VWMultiSelector : BaseSelector
    {
        TextBox txtFilterLeft, txtFilterRight;
        ListBox uxLeft, uxRight;
        Button uxAddItem, uxRemoveItem, uxAddAllItem, uxRemoveAllItem, btnFilterLeft, btnFilterRight;
        RadContextMenu mnuFilter;
        HiddenField hddFilterOption;

        public VWMultiSelector()
            : base(typeof(VWMultiSelector))
        {         

            txtFilterLeft = new TextBox();
            txtFilterLeft.CssClass = _CssClass;

            txtFilterRight = new TextBox();
            txtFilterRight.CssClass = _CssClass;

            uxAddItem = new Button();
            uxAddItem.Text = ">";
            uxAddItem.Click += new System.EventHandler(uxAddItem_Click);

            uxRemoveItem = new Button();
            uxRemoveItem.Text = "<";
            uxRemoveItem.Click += new System.EventHandler(uxRemoveItem_Click);

            uxAddAllItem = new Button();
            uxAddAllItem.Text = ">>";
            uxAddAllItem.Click += new System.EventHandler(uxAddAllItem_Click);

            uxRemoveAllItem = new Button();
            uxRemoveAllItem.Text = "<<";
            uxRemoveAllItem.Click += new System.EventHandler(uxRemoveAllItem_Click);

            uxLeft = new ListBox();
            uxLeft.SelectionMode = ListSelectionMode.Multiple;
            uxLeft.Height = new Unit(_Height);
            uxLeft.Width = new Unit(_Width);
            uxLeft.CssClass = _CssClass;

            uxRight = new ListBox();
            uxRight.SelectionMode = ListSelectionMode.Multiple;
            uxRight.Height = new Unit(_Height);
            uxRight.Width = new Unit(_Width);
            uxRight.CssClass = _CssClass;

            btnFilterLeft = new Button();
            btnFilterLeft.Attributes.Add("style", "display: none");
            btnFilterLeft.Click += new System.EventHandler(btnFilterLeft_Click);

            btnFilterRight = new Button();
            btnFilterRight.Attributes.Add("style", "display: none");
            btnFilterRight.Click += new System.EventHandler(btnFilterRight_Click);

            hddFilterOption = new HiddenField();

            mnuFilter = new RadContextMenu();
            mnuFilter.EnableEmbeddedSkins = EnableEmbeddedSkins;
            mnuFilter.Skin = Skin;
            mnuFilter.Width = new Unit(_WidthContextMenu);
            mnuFilter.OnClientItemClicked = "doFilterOnMenu";
            RadMenuItem mnItem;

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_NoFilter;
            mnItem.Value = "NoFilter";
            mnuFilter.Items.Add(mnItem);

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_Contains;
            mnItem.Value = "Contains";
            mnuFilter.Items.Add(mnItem);

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_DoesNotContain;
            mnItem.Value = "DoesNotContain";
            mnuFilter.Items.Add(mnItem);

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_StartsWith;
            mnItem.Value = "StartsWith";
            mnuFilter.Items.Add(mnItem);

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_EndsWith;
            mnItem.Value = "EndsWith";
            mnuFilter.Items.Add(mnItem);

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_EqualTo;
            mnItem.Value = "EqualTo";
            mnuFilter.Items.Add(mnItem);

            mnItem = new RadMenuItem();
            mnItem.Text = Resources.LanguageResource.AS_ASGrid_FilterMenu_NotEqualTo;
            mnItem.Value = "NotEqualTo";
            mnuFilter.Items.Add(mnItem);
        }       

        #region Properties

        public ListBox ListBoxLeft { get { return uxLeft; } set { uxLeft = value; } }
        public ListBox ListBoxRight { get { return uxRight; } set { uxRight = value; } }

        private HttpSessionState Session
        {
            get
            {
                return HttpContext.Current.Session;
            }
        }
        private DataTable _DataSourceOrigination = new DataTable();
        [Bindable(false)]
        [Browsable(false)]
        public DataTable DataSourceOrigination
        {
            get
            {
                return (DataTable)Session[UserControlID + "DataSourceOrigination"];
            }
            set
            {
                Session[UserControlID + "DataSourceOrigination"] = _DataSourceOrigination = value as DataTable;
                BindLeft();
            }
        }

        private DataTable _DataSourceDestination = new DataTable();
        [Bindable(false)]
        [Browsable(false)]
        public DataTable DataSourceDestination
        {
            get
            {
                return (DataTable)Session[UserControlID + "DataSourceDestination"];
            }
            set
            {
                Session[UserControlID + "DataSourceDestination"] = _DataSourceDestination = value as DataTable;
                BindRight();
            }
        }

        public string DataValueField
        {
            get;
            set;
        }

        public string DataTextField
        {
            get;
            set;
        }

        public bool EnableEmbeddedSkins { get; set; }

        public string Skin { get; set; }

        public string AddText { set { uxAddItem.Text = value; } }
        public string AddAllText { set { uxAddAllItem.Text = value; } }
        public string RemoveText { set { uxRemoveItem.Text = value; } }
        public string RemoveAllText { set { uxRemoveAllItem.Text = value; } }
        public bool HideAddAll { set { uxAddAllItem.Visible = false; } }
        public bool HideRemoveAll { set { uxRemoveAllItem.Visible = false; } }

        public bool _ShowFilter = true;
        public bool ShowFilter { set { _ShowFilter = value; } }

        public bool _ShowTooltip = true;
        public bool ShowTooltip { set { _ShowTooltip = value; } }

        public bool _CheckExisted = true;
        public bool CheckExisted { set { _CheckExisted = value; } }

        private int _Width = 300;
        public int WidthSelector
        {
            set
            {
                _Width = value;
                uxLeft.Width = new Unit(_Width);
                uxRight.Width = new Unit(_Width);
                txtFilterLeft.Width = new Unit(_Width - 22);
                txtFilterRight.Width = new Unit(_Width - 22);
            }
        }

        private int _Height = 180;
        public int HeightSelector
        {
            set
            {
                _Height = value;
                uxLeft.Height = new Unit(_Height);
                uxRight.Height = new Unit(_Height);
            }
        }

        private int _WidthButton = 60;
        public int WidthButton
        {
            set
            {
                _WidthButton = value;
                uxAddItem.Width = uxRemoveItem.Width = uxAddAllItem.Width = uxRemoveAllItem.Width = new Unit(_WidthButton);
            }
        }

        private int _WidthContextMenu = 130;
        public int WidthContextMenu
        {
            set
            {
                _WidthContextMenu = value;
                mnuFilter.Width = new Unit(_WidthContextMenu);
            }
        }

        public bool EnableFilterWithEnterPress
        {
            set
            {
                if (value)
                {
                    txtFilterLeft.Attributes.Add("onkeypress", "doFilter(event, this);");
                    txtFilterRight.Attributes.Add("onkeypress", "doFilter(event, this);");
                }
            }
        }

        [Bindable(false)]
        [Browsable(false)]
        public ListItemCollection SelectedItems
        {
            get
            {
                return uxRight.Items;
            }
        }

        public string CSSAddButton
        {
            set { uxAddItem.CssClass = value; }
        }

        public string CSSAddAllButton
        {
            set { uxAddAllItem.CssClass = value; }
        }

        public string CSSRemoveButton
        {
            set { uxRemoveItem.CssClass = value; }
        }

        public string CSSRemoveAllButton
        {
            set { uxRemoveAllItem.CssClass = value; }
        }

        private string _CssClass = "TwoListBox";
        public override string CssClass
        {
            set
            {
                uxLeft.CssClass = uxRight.CssClass = value;
            }
        }

        public string CssClassTextBoxFilter
        {
            set
            {
                txtFilterLeft.CssClass = txtFilterRight.CssClass = value;
            }
        }


        /// <summary>
        /// Get the client ID of the left list box
        /// </summary>
        public string SourceClientID
        {
            get
            {
                return uxLeft.ClientID;
            }
        }

        /// <summary>
        /// Get the client ID of the right list box
        /// </summary>
        public string DestClientID
        {
            get
            {
                return uxRight.ClientID;
            }
        }

        private string UserControlID
        {
            get { return this.ID; }
        }

        public void ClearFilter()
        {
            txtFilterLeft.Text = txtFilterRight.Text = string.Empty;
        }

        private string _SortExpression = string.Empty;
        public string SortExpression
        {
            get
            {
                return this._SortExpression;
            }
            set
            {
                _SortExpression = value;
            }
        }

        private bool _allowSortExpression = true;
        public bool AllowSortExpression
        {
            get
            {
                return this._allowSortExpression;
            }
            set
            {
                _allowSortExpression = value;
            }
        }

        public string FilterImageUrl { get; set; }

        /// <summary>
        /// Filter menu
        /// </summary>
        protected RadContextMenu FilterMenu
        {
            get
            {
                return mnuFilter;
            }
        }

        #endregion

        #region override methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            txtFilterLeft.Attributes.Add("mnId", mnuFilter.ClientID);
            txtFilterLeft.Attributes.Add("hdId", hddFilterOption.ClientID);
            txtFilterLeft.Attributes.Add("smId", btnFilterLeft.ClientID);


            txtFilterRight.Attributes.Add("mnId", mnuFilter.ClientID);
            txtFilterRight.Attributes.Add("hdId", hddFilterOption.ClientID);
            txtFilterRight.Attributes.Add("smId", btnFilterRight.ClientID);

            uxRight.DataBound += new System.EventHandler(uxRight_DataBound);
        }

        void uxRight_DataBound(object sender, EventArgs e)
        {
            if (_allowToFireMoveDataEvent) OnMovedData(e);
        }

        public event System.EventHandler MovedData;
        private bool _allowToFireMoveDataEvent = true;
        public virtual void OnMovedData(EventArgs e)
        {
            if (MovedData != null)
            {
                _allowToFireMoveDataEvent = false;
                MovedData(this, e);
                _allowToFireMoveDataEvent = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            _DataSourceOrigination = DataSourceOrigination;
            _DataSourceDestination = DataSourceDestination;
            RisgisterDoubleClick();
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterClientScriptBlock(typeof(VWMultiSelector), "MultiSelector.js", "<script src=\"" + ResolveUrl("~/res/js/common/MultiSelector.js") + "\" type=\"text/javascript\"></script>");
            
            var url = this.Page.Request.Url.AbsolutePath;
            var segments = this.Page.Request.Url.Segments;

            if (segments != null && segments.Length > 0)
                url = segments[segments.Length - 1];

            var jsValidateFunction = string.Format("addValidationMultiSelectorFilter(this, event,'{0}');", url);
            txtFilterLeft.Attributes.Add("onblur", jsValidateFunction);
            txtFilterRight.Attributes.Add("onblur", jsValidateFunction);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<table class=\"MultiSelector\">\r\n");
            if (_ShowFilter)
            {
                //<tr><td>Left textbox filter</td><td></td><td>Right textbox filter</td></tr>
                writer.Write("    <tr>\r\n");
                //Left text box filter
                writer.Write("        <td>\r\n");
                txtFilterLeft.RenderControl(writer);
                if (string.IsNullOrEmpty(FilterImageUrl))
                {
                    FilterImageUrl = ResolveUrl("~/res/img/filter_ico.png");
                }
                writer.Write("            <img class=\"imgFilterLeft\" src=\"" + FilterImageUrl + "\" alt=\"filter\" onclick=\"showFilter(event, this)\" />\r\n");
                writer.Write("        </td>\r\n");
                //Space <td>
                writer.Write("        <td>\r\n");
                writer.Write("        </td>\r\n");
                //Right text box filter
                writer.Write("        <td>\r\n");
                txtFilterRight.RenderControl(writer);
                writer.Write("            <img class=\"imgFilterRight\" src=\"" + FilterImageUrl + "\" alt=\"filter\" onclick=\"showFilter(event, this)\" />\r\n");
                writer.Write("        </td>\r\n");
                writer.Write("    </tr>\r\n");
            }

            writer.Write("    <tr valign=\"center\">\r\n");
            //List Box Left
            writer.Write("        <td>\r\n");
            writer.Write("            <div class='TwoListBoxLeft'>\r\n");
            uxLeft.RenderControl(writer);
            writer.Write("            </div>\r\n");
            writer.Write("        </td>\r\n");
            //Buttons: Add, Add All, Remove, Remove All
            writer.Write("        <td class=\"TwoListButtons\">\r\n");
            //Button Add
            writer.Write("            <div>\r\n");
            uxAddItem.RenderControl(writer);
            writer.Write("            </div>\r\n");
            //Button Remove
            writer.Write("            <div>\r\n");
            uxRemoveItem.RenderControl(writer);
            writer.Write("            </div>\r\n");
            //Button Add All
            writer.Write("            <div>\r\n");
            uxAddAllItem.RenderControl(writer);
            writer.Write("            </div>\r\n");
            //Button Revove All
            writer.Write("            <div>\r\n");
            uxRemoveAllItem.RenderControl(writer);
            writer.Write("            </div>\r\n");
            writer.Write("        </td>\r\n");
            //List Box Right
            writer.Write("        <td>\r\n");
            writer.Write("            <div class='TwoListBoxRight'>\r\n");
            uxRight.RenderControl(writer);
            writer.Write("            </div>\r\n");
            writer.Write("        </td>\r\n");

            writer.Write("    </tr>\r\n");
            writer.Write("</table>\r\n");

            btnFilterLeft.RenderControl(writer);
            btnFilterRight.RenderControl(writer);
            mnuFilter.RenderControl(writer);
            hddFilterOption.RenderControl(writer);
        }
        #endregion

        #region event handlers
        void btnFilterRight_Click(object sender, EventArgs e)
        {
            string filterExpression = txtFilterRight.Text.Trim().Replace("'", "''");
            filterExpression = !string.IsNullOrEmpty(filterExpression) ? EscapeLikeValue(filterExpression) : string.Empty;
            switch (hddFilterOption.Value)
            {
                case "NoFilter":
                    txtFilterRight.Text = filterExpression = string.Empty;
                    filterExpression = string.Format(DataTextField + " like '%{0}%'", filterExpression);
                    break;
                case "Contains":
                    filterExpression = string.Format(DataTextField + " like '%{0}%'", filterExpression);
                    break;
                case "DoesNotContain":
                    filterExpression = string.Format(DataTextField + " not like '%{0}%'", filterExpression);
                    break;
                case "StartsWith":
                    filterExpression = string.Format(DataTextField + " like '{0}%'", filterExpression);
                    break;
                case "EndsWith":
                    filterExpression = string.Format(DataTextField + " like '%{0}'", filterExpression);
                    break;
                case "EqualTo":
                    filterExpression = string.Format(DataTextField + " = '{0}'", filterExpression);
                    break;
                case "NotEqualTo":
                    filterExpression = string.Format(DataTextField + " <> '{0}'", filterExpression);
                    break;
            }
            _DataSourceDestination.DefaultView.RowFilter = filterExpression;
            BindRight();
        }

        void btnFilterLeft_Click(object sender, EventArgs e)
        {
            string filterExpression = txtFilterLeft.Text.Trim().Replace("'", "''");
            filterExpression = !string.IsNullOrEmpty(filterExpression) ? EscapeLikeValue(filterExpression) : string.Empty;
            switch (hddFilterOption.Value)
            {
                case "NoFilter":
                    txtFilterLeft.Text = filterExpression = string.Empty;
                    filterExpression = string.Format(DataTextField + " like '%{0}%'", filterExpression);
                    break;
                case "Contains":
                    filterExpression = string.Format(DataTextField + " like '%{0}%'", filterExpression);
                    break;
                case "DoesNotContain":
                    filterExpression = string.Format(DataTextField + " not like '%{0}%'", filterExpression);
                    break;
                case "StartsWith":
                    filterExpression = string.Format(DataTextField + " like '{0}%'", filterExpression);
                    break;
                case "EndsWith":
                    filterExpression = string.Format(DataTextField + " like '%{0}'", filterExpression);
                    break;
                case "EqualTo":
                    filterExpression = string.Format(DataTextField + " = '{0}'", filterExpression);
                    break;
                case "NotEqualTo":
                    filterExpression = string.Format(DataTextField + " <> '{0}'", filterExpression);
                    break;
            }
            _DataSourceOrigination.DefaultView.RowFilter = filterExpression;
            BindLeft();
        }

        void uxRemoveItem_Click(object sender, EventArgs e)
        {
            MoveRightToLeft();
        }

        void uxAddItem_Click(object sender, EventArgs e)
        {
            MoveLeftToRight();
        }

        public void MoveLeftToRight()
        {
            foreach (ListItem item in this.uxLeft.Items)
            {
                if (item.Selected)
                {
                    LeftToRight(item.Value, item.Text);
                }
            }
            BindLeft();
            BindRight();
        }

        public void MoveRightToLeft()
        {
            foreach (ListItem item in this.uxRight.Items)
            {
                if (item.Selected)
                {
                    RightToLeft(item.Value, item.Text);
                }
            }
            BindLeft();
            BindRight();
        }

        void uxAddAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in this.uxLeft.Items)
            {
                LeftToRight(item.Value, item.Text);
            }
            BindLeft();
            BindRight();
        }

        void uxRemoveAllItem_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in this.uxRight.Items)
            {
                RightToLeft(item.Value, item.Text);
            }
            BindLeft();
            BindRight();
        }

        public event System.EventHandler DoubleClick;


        public virtual void OnDoubleClick(EventArgs e)
        {
            if (this.DoubleClick != null)
                this.DoubleClick(this, e);
        }

        void RisgisterDoubleClick()
        {
            if (Page.Request["__EVENTARGUMENT"] != null && Page.Request["__EVENTARGUMENT"] == TwoListBoxEvent.DoubleClick.ToString())
                OnDoubleClick(new EventArgs());
            uxLeft.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(uxLeft, TwoListBoxEvent.DoubleClick.ToString()));
            uxRight.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(uxRight, TwoListBoxEvent.DoubleClick.ToString()));
        }

        #endregion

        #region Move Data: Ori -> Des; Des ->Ori

        private bool IsExistedRowInDataTable(DataTable list, string keyFind, string valueFind)
        {
            foreach (DataRow row in list.Rows)
            {
                if (row[keyFind].ToString() == valueFind)
                {
                    return true;
                }
            }
            return false;
        }

        private void LeftToRight(string dataKey, string dataText)
        {
            if (_CheckExisted && IsExistedRowInDataTable(_DataSourceDestination, DataValueField, dataKey))
                return;

            string condition = DataValueField + " = '" + dataKey + "'";
            DataRow[] rows = _DataSourceOrigination.Select(condition);

            if (rows != null && rows.Length > 0)
            {
                DataRow rowSrc = _DataSourceDestination.NewRow();

                CopyRowToRow(rows[0], rowSrc);
                // Add to the right
                _DataSourceDestination.Rows.Add(rowSrc);
                // Remove at the left
                _DataSourceOrigination.Rows.Remove(rows[0]);
            }

        }

        /// <summary>
        /// Just Copy columns which existing in row2
        /// </summary>
        /// <param name="row1">Source</param>
        /// <param name="row2">Destination</param>
        private void CopyRowToRow(DataRow row1, DataRow row2)
        {

            for (int i = 0; i < row1.Table.Columns.Count; i++)
            {
                DataColumn col = row1.Table.Columns[i];
                if (row2.Table.Columns.Contains(col.ColumnName))
                {
                    row2[col.ColumnName] = row1[col.ColumnName];
                }
            }

        }

        private void RightToLeft(string dataKey, string dataText)
        {
            if (_CheckExisted && IsExistedRowInDataTable(_DataSourceOrigination, DataValueField, dataKey))
                return;

            string condition = DataValueField + " = '" + dataKey + "'";
            DataRow[] rows = _DataSourceDestination.Select(condition);
            if (rows != null && rows.Length > 0)
            {
                DataRow rowSrc = _DataSourceOrigination.NewRow();

                CopyRowToRow(rows[0], rowSrc);
                // Add to the left
                _DataSourceOrigination.Rows.Add(rowSrc);
                // Remove at the Right
                _DataSourceDestination.Rows.Remove(rows[0]);

            }

        }

        private DataTable RemoveRow(DataRow rowRemove, DataTable data)
        {
            foreach (DataRow row in data.Rows)
            {
                if (row[DataValueField].ToString() == rowRemove[DataValueField].ToString())
                {
                    data.Rows.Remove(row);
                    return data;
                }
            }
            return data;
        }


        #endregion

        #region Bind Data

        private void BindLeft()
        {
            if (_DataSourceOrigination == null) return;

            if (AllowSortExpression)
            {
                if (SortExpression.Equals(string.Empty))
                    _DataSourceOrigination.DefaultView.Sort = DataTextField + " ASC";
                else
                    _DataSourceOrigination.DefaultView.Sort = SortExpression;
            }               

            this.uxLeft.DataSource = _DataSourceOrigination;
            this.uxLeft.DataTextField = DataTextField;
            this.uxLeft.DataValueField = DataValueField;
            this.uxLeft.DataBind();
            if (_ShowTooltip)
            {
                //Set Tooltip for each item
                for (int i = 0; i < uxLeft.Items.Count; i++)
                {
                    uxLeft.Items[i].Attributes.Add("title", uxLeft.Items[i].Text);
                }
            }

        }

        private void BindRight()
        {
            if (_DataSourceDestination == null) return;

            if (AllowSortExpression)
            {
                if (SortExpression.Equals(string.Empty))
                    _DataSourceDestination.DefaultView.Sort = DataTextField + " ASC";
                else
                    _DataSourceDestination.DefaultView.Sort = SortExpression;
            }

            this.uxRight.DataSource = _DataSourceDestination;
            this.uxRight.DataTextField = DataTextField;
            this.uxRight.DataValueField = DataValueField;
            this.uxRight.DataBind();
            if (_ShowTooltip)
            {
                //Set Tooltip for each item
                for (int i = 0; i < uxRight.Items.Count; i++)
                {
                    uxRight.Items[i].Attributes.Add("title", uxRight.Items[i].Text);
                }
            }
        }

        public void Rebind()
        {
            BindLeft();
            BindRight();
        }

        public void Reset()
        {
            txtFilterLeft.Text = txtFilterRight.Text = string.Empty;
        }

        #endregion

        public string GetSelectedItemsAsString(string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < uxRight.Items.Count; i++)
            {
                sb.Append(uxRight.Items[i].Value + (i == uxRight.Items.Count - 1 ? "" : delimiter));
            }
            return sb.ToString();
        }

        public DataTable GetSelectedItemsAsDataTable()
        {
            return _DataSourceDestination;
        }
        public string EscapeLikeValue(string valueWithoutWildcards)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueWithoutWildcards.Length; i++)
            {
                char c = valueWithoutWildcards[i];
                if (c == '*' || c == '%' || c == '[' || c == ']')
                    sb.Append("[").Append(c).Append("]");
                else if (c == '\'')
                    sb.Append("''");
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
