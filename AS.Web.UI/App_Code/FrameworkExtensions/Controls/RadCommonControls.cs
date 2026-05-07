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
using AS.Controls.Telerik;

namespace AS.Controls.Global
{
    public class RadAjaxLoadingPanel : ASRadAjaxLoadingPanel
    {
    }
    public class RadAjaxManager : ASRadAjaxManager
    {

    }
    public class RadAjaxManagerProxy : ASRadAjaxManagerProxy
    {
    }
    public class RadAjaxPanel : ASRadAjaxPanel
    {
    }
    public class RadCodeBlock : ASRadCodeBlock
    {
    }
    public class RadComboBox : ASRadComboBox
    {
    }
    public class RadComboBoxCheckBox : ASRadComboBox
    {
    }
    public class RadComboBoxItem : ASRadComboBoxItem
    {
    }
    public class RadDatePicker : ASRadDatePicker
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (GeneralFuncsLib.GetDataOfExtendedSetting("MultiLanguage").ToLower().Equals("true"))
            {
                string curCulture = GeneralFuncsLib.GetCurrentCulture();
                this.Culture = new System.Globalization.CultureInfo(curCulture);
                this.DateInput.DateFormat = WebSiteConstants.DATE_FORMAT;
                this.Calendar.FastNavigationSettings.TodayButtonCaption = Resources.LanguageResource.RadDatePicker_TodayButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_TodayButtonCaption").ToString();
                this.Calendar.FastNavigationSettings.CancelButtonCaption = Resources.LanguageResource.RadDatePicker_CancelButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_CancelButtonCaption").ToString();
                this.Calendar.FastNavigationSettings.OkButtonCaption = Resources.LanguageResource.RadDatePicker_OkButtonCaption;//HttpContext.GetGlobalResourceObject("LanguageResource", "RadDatePicker_OkButtonCaption").ToString();
            }
        }
    }

    public class RadDateTimePicker : ASRadDateTimePicker
    {
    }
    public class RadListBox : ASRadListBox
    {
    }
    public class RadTextBox : ASRadTextBox
    {
    }
    public class RadNumericTextBox : ASRadNumericTextBox
    {
    }
    public class RadToolTip : ASRadToolTip
    {
    }
    public class RadWindow : ASRadWindow
    {

    }
    public class RadWindowManager : ASRadWindowManager
    {
    }
    public class RadTreeView : ASRadTreeView
    {
    }
    

}