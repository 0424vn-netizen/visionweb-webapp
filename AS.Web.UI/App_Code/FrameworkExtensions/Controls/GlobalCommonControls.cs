using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Controls.ASP.Net;
using System.IO;
using System.Xml;
using System.Web.Caching;
using System.Text;
using AS.Controls.Grid;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;

/// <summary>
/// Summary description for GlobalCommonControls
/// </summary>
namespace AS.Controls.Global
{
    public class ASRepeater : Repeater
    {
        public string NumberOfColumns { get; set; }
        public ASRepeater() { }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.BeginRender();
            base.Render(writer);
            DataTable data = this.DataSource as DataTable;
            if (data == null || data.Rows.Count == 0)
            {
                writer.Write(
                                        "<tr class=\"rgNoRecords\"><td style=\"text-align:left;\" colspan=" + NumberOfColumns + ">"
                                        + "<div>" + Resources.LanguageResource.AS_ASRepeater_NoDataFound + "</div>"
                                        + "</td>"
                                   + "</tr>");
            }
            writer.EndRender();
        }
    }
    public class LinkButton : ASLinkButton
    {
    }
    public class Button : ASButton
    {
        public bool IsStandardButton
        {
            set
            {
                ViewState["IsStandardButton"] = value;
            }
            get
            {
                return (bool)ViewState["IsStandardButton"];
            }
        }

        public Button()
        {
            IsStandardButton = false;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.BeginRender();
            if (IsStandardButton)
            {

                base.Render(writer);
            }
            else
            {
                // writer.Write("<input type=\"submit\" class=\"LongSubmitButtonLeft\" disabled=\"true\" value=\" \" />");
                base.Render(writer);
                // writer.Write("<input  type=\"submit\" class=\"LongSubmitButtonRight\" disabled=\"true\"  value=\" \"></input>");
            }
            writer.EndRender();
        }
    }
    public class CheckBox : ASCheckBox
    {
        public string Value
        {
            set { ViewState["Value"] = value; }
            get
            {
                if (ViewState["Value"] == null) return string.Empty;
                else return ViewState["Value"].ToString();
            }
        }
    }
    public class CheckBoxList : ASCheckBoxList
    {
    }

    public class Literal : ASLiteral
    {

    }
    public class Panel : ASPanel
    {
    }
    public class PlaceHolder : ASPlaceHolder
    {
    }
    public class RadioButton : ASRadioButton
    {
        public string Value
        {
            set { ViewState["Value"] = value; }
            get
            {
                if (ViewState["Value"] == null) return string.Empty;
                else return ViewState["Value"].ToString();
            }
        }
    }
    public class RadioButtonList : ASRadioButtonList
    {
    }
    public class TextBox : ASTextBox
    {
        public TextBox()
        {
        }
    }
    public class HiddenField : System.Web.UI.WebControls.HiddenField
    {
    }

    public class ClientResourceSetting : PlaceHolder
    {
        public ClientResourceSetting()
        {
            ASClientID = "default";
            ResourceKey = "";
        }
        public string Text
        {
            set;
            get;
        }

        protected string ASClientID
        {
            set
            {
                ViewState["ASClientID"] = value;
            }
            get
            {
                return ViewState["ASClientID"].ToString();
            }
        }
        public string ResourceKey
        {
            set
            {
                ViewState["ResourceKey"] = value;
            }
            get
            {
                return ViewState["ResourceKey"].ToSafeString();
            }
        }

        public bool RootURLProcess { get; set; }

        public override void RenderControl(HtmlTextWriter writer)
        {
            this.Text = this.GetValue(ResourceKey);
            writer.Write(this.Text);
        }

        public string GetValue(string key)
        {
            XmlNode node = GetXmlNode(key);
            string resultText = string.Empty;
            if (node != null)
            {
                resultText = node.InnerXml.Replace("&amp;", "&");
            }
            if (RootURLProcess)
            {
                resultText = resultText.Replace("[RootURL]", ResolveUrl("~/"));
            }
            return resultText.Trim();
        }
        public string GetTitle(string key)
        {
            XmlNode node = GetXmlNode(key);
            string resultText = string.Empty;
            if (node != null && node.Attributes["title"] != null)
            {
                resultText = node.Attributes["title"].Value.Replace("&amp;", "&");
            }
            if (RootURLProcess)
            {
                resultText = resultText.Replace("[RootURL]", ResolveUrl("~/"));
            }
            return resultText;
        }
        protected XmlNode GetXmlNode(string key)
        {
            if (ASClientID == "default")
            {
                ASClientID = SessionManager.CurrentClient.ToString();
            }
            //load xml & cache
            string xmlFile = this.GetFileName();
            string cacheKey = xmlFile.Trim();

            XmlDocument doc = null;
            if (HttpRuntime.Cache[cacheKey] == null)
            {
                doc = new XmlDocument();
                doc.Load(xmlFile);
                HttpRuntime.Cache.Add(cacheKey, doc, new CacheDependency(xmlFile), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            else
            {
                doc = (XmlDocument)HttpRuntime.Cache[cacheKey];
            }

            XmlNode node = doc.SelectSingleNode("//Client[@ClientID=\"" + ASClientID + "\"]/Item[@mode=\"" + key + "\"]");
            if (node == null)
                node = doc.SelectSingleNode("//Client[@ClientID=\"default\"]/Item[@mode=\"" + key + "\"]");
            if (node != null)
            {
                XmlNodeList _resNodeList = node.SelectNodes("//reskey");
                foreach (XmlNode _resNode in _resNodeList)
                {
                    _resNode.ParentNode.InnerXml = HttpContext.GetGlobalResourceObject("Template", _resNode.InnerText).ToString();
                }
            }
            return node;
        }
        protected string GetFileName()
        {
            string xmlFile = "~/App_Data/ClientResourceSettings.xml";
            return MapPathSecure(xmlFile);
        }

    }
    public class ASModalContainer : PlaceHolder
    {
        private string _Width = string.Empty;
        public string Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private string _WidthCssClass = "modal-xxl";
        public string WidthCssClass
        {
            get { return _WidthCssClass; }
            set { _WidthCssClass = value; }
        }

        private string _ContainerCssClass = "container";
        public string ContainerCssClass
        {
            get { return _ContainerCssClass; }
            set { _ContainerCssClass = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string html = string.Empty;
            var customClass = "block-center " + (_WidthCssClass ?? string.Empty);
            html = "<div id=\"cidModalContainer\" class=\"" + customClass + "\"";
            if (_Width != string.Empty)
            {
                html += " style=\"width:" + _Width + ";\">";
            }
            else
            {
                html += ">";
            }
            html += " <div class=\"" + _ContainerCssClass + "\">";
            writer.Write(html);

            base.Render(writer);

            html = "</div></div>";
            writer.Write(html);
        }
    }

    public class TableColumnHeader : GlobalUserControl
    {
        public string CssClass
        {
            get;
            set;
        }
        public Alignment Alignment
        {
            get;
            set;
        }
        public string HeaderText
        {
            get;
            set;
        }
        public string HeaderTooltip
        {
            get;
            set;
        }
        public string HeaderTooltipID
        {
            get;
            set;
        }
        public string Width
        {
            get;
            set;
        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                StringBuilder html = new StringBuilder();
                string style = string.Empty;
                if (!string.IsNullOrEmpty(HeaderTooltipID))
                {
                    html.AppendFormat(" <th class=\"{0}\" style=\"{1}\" > <span id=\"{2}\"> {3} </span></th>", CssClass, style, HeaderTooltipID, HeaderText.Replace("$", SessionManager.CurrencySymbol));
                }
                else
                {
                    html.AppendFormat(" <th class=\"{0}\" title=\"{1}\" style=\"{2}\" >{3}</th>", CssClass, HeaderTooltip.Replace("$", SessionManager.CurrencySymbol), style, HeaderText.Replace("$", SessionManager.CurrencySymbol));
                }
                writer.Write(html);
            }
            base.Render(writer);
        }
    }

    public enum Alignment
    {
        Center = 0,
        Left = 1,
        Right = 2
    }

    public class TableColumnContent : GlobalUserControl
    {
        public TableColumnContent()
        {
        }

        public TableColumnContent(Alignment alignment, bool visible = true, FormatType asFormat = FormatType.Auto)
        {
            this.Visible = visible;
            this.Alignment = alignment;
            this.ASFormat = asFormat;
        }

        public string CssClass
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
        public string DataField
        {
            get;
            set;
        }
        public string ToolTip
        {
            get;
            set;
        }
        public Alignment Alignment
        {
            get;
            set;
        }
        public FormatType ASFormat
        {
            get;
            set;
        }
        public string Color
        {
            get;
            set;
        }
        public string BackgroundColor
        {
            get;
            set;
        }
        string defaultNullValue = "N/A";
        public string CustomFormatDataValue(object dataValue, FormatType asFormat)
        {
            if (dataValue is DBNull)
            {
                return defaultNullValue;
            }
            try
            {
                decimal TempValue = 0M;
                switch (asFormat)
                {
                    case FormatType.Auto:
                        {
                            if (dataValue is Int16
                                || dataValue is Int32
                                || dataValue is Int64
                                || dataValue is uint
                                || dataValue is sbyte
                                || dataValue is byte
                                || dataValue is ushort
                                || dataValue is ulong

                                )
                            {
                                this.Alignment = Global.Alignment.Right;
                                return FormatConvertResolver.FormatInteger(dataValue);
                            }

                            else if (dataValue is float
                                || dataValue is Single
                                || dataValue is Double

                                )
                            {
                                this.Alignment = Global.Alignment.Right;
                                return FormatConvertResolver.FormatNumber(dataValue);
                            }
                            else if (dataValue is Decimal)
                            {
                                TempValue = decimal.Parse(dataValue.ToString());
                                this.Alignment = Global.Alignment.Right;

                                if (TempValue < 0) this.Color = ColorTranslator.ToHtml(System.Drawing.Color.Red);
                                return FormatConvertResolver.FormatCurrency(TempValue, SessionManager.CurrencyFortmat);
                            }
                            else if (dataValue is DateTime)
                            {
                                this.Alignment = Global.Alignment.Center;
                                return FormatConvertResolver.FormatDateTime((DateTime)dataValue);
                            }
                            else if (dataValue is String)
                            {
                                this.Alignment = Global.Alignment.Center;
                                return FormatConvertResolver.FormatString(dataValue.ToString());
                            }
                            break;
                        }
                    case FormatType.DateAndTime:
                        {
                            this.Alignment = Global.Alignment.Center;
                            return FormatConvertResolver.FormatDateTime((DateTime)dataValue);
                        }
                    case FormatType.DateAndTime12Hours:
                        {
                            this.Alignment = Global.Alignment.Center;
                            return FormatConvertResolver.FormatDateTime12Hours((DateTime)dataValue);
                        }
                    case FormatType.Date:
                        {
                            this.Alignment = Global.Alignment.Center;
                            return FormatConvertResolver.FormatDateOnly((DateTime)dataValue);
                        }
                    case FormatType.Currency:
                        {
                            TempValue = decimal.Parse(dataValue.ToString());
                            this.Alignment = Global.Alignment.Right;
                            if (TempValue < 0)
                            {
                                this.Color = ColorTranslator.ToHtml(System.Drawing.Color.Red);
                            }
                            return FormatConvertResolver.FormatCurrency(TempValue, SessionManager.CurrencyFortmat);
                        }
                    case FormatType.Currency4Digits:
                        {
                            TempValue = decimal.Parse(dataValue.ToString());
                            this.Alignment = Global.Alignment.Right;
                            if (TempValue < 0)
                            {
                                this.Color = ColorTranslator.ToHtml(System.Drawing.Color.Red);
                            }
                            return FormatConvertResolver.FormatCurrency4Digits(TempValue, SessionManager.CurrencyFortmat);
                        }
                    case FormatType.Percentage:
                        {
                            this.Alignment = Global.Alignment.Right;
                            return FormatConvertResolver.FormatPercent(decimal.Parse(dataValue.ToString()));
                        }
                    case FormatType.Number:
                        {
                            this.Alignment = Global.Alignment.Right;
                            return FormatConvertResolver.FormatNumber(dataValue);
                        }
                    case FormatType.Integer:
                        {
                            this.Alignment = Global.Alignment.Right;
                            return FormatConvertResolver.FormatInteger(dataValue);
                        }
                    case FormatType.DynamicString:
                        {
                            this.Alignment = Global.Alignment.Right;
                            return FormatConvertResolver.FormatString(dataValue.ToString());
                        }

                    case FormatType.StaticString:
                        {
                            this.Alignment = Global.Alignment.Center;
                            return FormatConvertResolver.FormatString(dataValue.ToString());
                        }
                    case FormatType.None:
                        {
                            return dataValue == DBNull.Value ? string.Empty : dataValue.ToString();
                        }
                }
                return string.Empty;
            }
            catch
            {
                return "AS System: Invalid Data Type";
            }

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.Visible)
            {
                StringBuilder html = new StringBuilder();
                Alignment alignment = Alignment == null ? Alignment.Center : Alignment;
                StringBuilder style = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Color))
                {
                    style.AppendFormat("color:{0};", this.Color);
                }
                if (!string.IsNullOrEmpty(this.BackgroundColor))
                {
                    style.AppendFormat("background-color:{0};", this.BackgroundColor);
                }
                html.AppendFormat(" <td align=\"{0}\" class=\"{1}\"  title=\"{2}\" style=\"{3}\" >{4}</td>", alignment.ToString().ToLower(), CssClass, ToolTip, style, Text);
                writer.Write(html);
            }
            base.Render(writer);
        }

        public string RenderHtml()
        {
            if (this.Visible)
            {
                StringBuilder html = new StringBuilder();
                Alignment alignment = Alignment == null ? Alignment.Center : Alignment;
                StringBuilder style = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Color))
                {
                    style.AppendFormat("color:{0};", this.Color);
                }
                if (!string.IsNullOrEmpty(this.BackgroundColor))
                {
                    style.AppendFormat("background-color:{0};", this.BackgroundColor);
                }
                html.AppendFormat(" <td align=\"{0}\" class=\"{1}\"  title=\"{2}\" style=\"{3}\" >{4}</td>", alignment.ToString().ToLower(), CssClass, ToolTip, style, Text);
                return html.ToString();
            }

            else return string.Empty;

        }

    }

    public class ASMCFWorkContent : GlobalUserControl
    {
        public ASMCFWorkContent() { }
        public Dictionary<string, string> DicAttrToolTip { get; set; }
        public string CssClass { get; set; }
        public bool IsDisabled { get; set; }
        public DataTable DispositionTable { get; set; }
        public string DispositionChecked { get; set; }
        public WebSiteEnums.PageSectionEnums PageSection { get; set; }

        protected override void CreateChildControls()
        {
            if (DicAttrToolTip != null)
            {
                var ctrl = SetWorkStateForRainbowItem();
                if (!IsDisabled)
                {
                    if (DispositionTable != null && !DispositionTable.Columns.Contains("Checked"))
                    {
                        DispositionTable.Columns.Add("Checked", typeof(bool));
                    }
                    DataTable dataTableTemp = new DataTable();
                    List<string> dipositionCheckedList = DispositionChecked.Split(',').ToList();
                    foreach (DataRow item in DispositionTable.Rows)
                    {
                        item["Checked"] = dipositionCheckedList.Any(x => x.Trim().Equals(item["DispositionID"].ToString().Trim()));
                    }

                    DispositionTable = DispositionTable.AsEnumerable().Where(x => x.Field<bool>("Checked") == true || x.Field<bool>("IsActive") == true).CopyToDataTable();

                    RM_MCF_GeneralFuncsLib.GenerateWorkPopover(DicAttrToolTip, DispositionTable, ctrl, PageSection);
                }
                this.Controls.Add(ctrl);
                base.CreateChildControls();
            }
        }

        private HtmlGenericControl SetWorkStateForRainbowItem()
        {
            WebSiteEnums.WorkStatus workStateID = (WebSiteEnums.WorkStatus)Enum.Parse(typeof(WebSiteEnums.WorkStatus), DicAttrToolTip["WorkStateID"]);
            WebSiteEnums.WorkStatus currentStatus = (WebSiteEnums.WorkStatus)Enum.Parse(typeof(WebSiteEnums.WorkStatus), DicAttrToolTip["CurrentStatus"]);
            HtmlGenericControl divWork = new HtmlGenericControl("div");
            divWork.Attributes.Add("class", string.Format("checkbox-middle wk-btn {0}", CssClass));
            divWork.Attributes.Add("data-selector", "wk-btn");
            HtmlGenericControl uxlblWork = new HtmlGenericControl("div");
            uxlblWork.ID = "uxlblWork";
            uxlblWork.Attributes.Add("class", "work-checkbox");

            HtmlGenericControl workItem = new HtmlGenericControl("div");
            workItem.Attributes.Add("class", "work-item");
            workItem.Attributes.Add("data-selector", "work-item");

            HtmlGenericControl workStatus = new HtmlGenericControl("span");
            workStatus.ID = "workStatus";
            workStatus.Attributes.Add("class", string.Format("{0}", IsDisabled ? "disable" : string.Empty));
            HtmlGenericControl workIcon = new HtmlGenericControl("span");
            workIcon.Attributes.Add("class", string.Format("work-ico {0}", IsDisabled ? "disable" : string.Empty));
            HtmlInputCheckBox cbox = new HtmlInputCheckBox();
            cbox.ID = "cBox";
            cbox.Attributes.Add("onclick", "changeWorkedState(this, 0)");
            cbox.Attributes.Add("data-wip", ((int)currentStatus).ToString());
            cbox.Attributes.Add("class", "state-work");
            cbox.Attributes.Add("data-selector", "state-work");
            if (IsDisabled)
            {
                cbox.Attributes.Add("disabled", "disabled");
            }
            HtmlGenericControl space = new HtmlGenericControl("span");
            space.InnerHtml = "&nbsp;";

            switch (workStateID)
            {
                case WebSiteEnums.WorkStatus.Work:
                    workStatus.InnerText = RM_MCF_GeneralFuncsLib.GetResourceValue("WorkStatus_Text").ToString();
                    workStatus.Attributes.Add("class", string.Format("work-status {0}", IsDisabled ? "disable" : string.Empty));
                    cbox.Attributes.Add("data-state", DicAttrToolTip["WorkStateID"]);
                    break;
                case WebSiteEnums.WorkStatus.WorkInProgress:
                    var userID = SessionManager.CurrentUser.UserID;
                    workStatus.InnerText = RM_MCF_GeneralFuncsLib.GetResourceValue("WorkInProgressStatus_Text").ToString();

                    workStatus.Attributes.Add("class", string.Format("work-status txt-wip{0} {1}"
                        , (currentStatus != WebSiteEnums.WorkStatus.WorkInProgressByOther ? string.Empty : "-disable"), IsDisabled ? "disable" : string.Empty));
                    cbox.Attributes.Add("data-state", DicAttrToolTip["WorkStateID"]);
                    workIcon.Attributes.Add("class", string.Format("work-ico icon-warning{0} {1}"
                        , (currentStatus != WebSiteEnums.WorkStatus.WorkInProgressByOther ? string.Empty : "-disable"), IsDisabled ? "disable" : string.Empty));
                    break;
                case WebSiteEnums.WorkStatus.Worked:
                    workStatus.InnerText = RM_MCF_GeneralFuncsLib.GetResourceValue("WorkedStatus_Text").ToString();
                    workStatus.Attributes.Add("class", string.Format("work-status txt-wkd {0}", IsDisabled ? "disable" : string.Empty));
                    cbox.Attributes.Add("data-state", DicAttrToolTip["WorkStateID"]);
                    workIcon.Attributes.Add("class", string.Format("work-ico icon-check {0}", IsDisabled ? "disable" : string.Empty));
                    break;
            }

            workItem.Controls.AddAt(0, workIcon);
            workItem.Controls.AddAt(1, workStatus);

            uxlblWork.Controls.AddAt(0, workItem);
            uxlblWork.Controls.AddAt(1, cbox);

            divWork.Controls.AddAt(0, uxlblWork);
            divWork.Controls.AddAt(1, space);

            return divWork;
        }
    }
}