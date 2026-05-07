using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AS.Controls.Grid;
using AS.Controls.Pages;

namespace AS.Controls.Global
{
    public class CustomPager : ASCustomPager
    {
        public CustomPager()
        {
        }
        protected override void CreateChildControls()
        {

            base.CreateChildControls();

            Repeater pagerList = (Repeater)FindControl("pagerList");
            if (pagerList != null)
            {
                const int GROUP_SIZE = 5;
                int totalGroup = ParentGrid.PageCount / GROUP_SIZE;
                if (ParentGrid.PageCount % GROUP_SIZE > 0) totalGroup++;
                int currentGroup = ParentGrid.CurrentPageIndex / GROUP_SIZE;


                List<int> pages = new List<int>();
                for (int i = 1; i <= GROUP_SIZE; i++)
                {
                    int page = currentGroup * GROUP_SIZE + i;
                    if (page > ParentGrid.PageCount) { break; }
                    pages.Add(page);
                }
                pagerList.DataSource = pages;
                pagerList.DataBind();
                System.Web.UI.WebControls.LinkButton pagerPrevGroup = (System.Web.UI.WebControls.LinkButton)FindControl("pagerPrevGroup");
                pagerPrevGroup.Visible = currentGroup > 0;
                pagerPrevGroup.CommandArgument = (currentGroup * GROUP_SIZE).ToString();
                System.Web.UI.WebControls.LinkButton pagerNextGroup = (System.Web.UI.WebControls.LinkButton)FindControl("pagerNextGroup");
                pagerNextGroup.Visible = currentGroup < totalGroup - 1;
                pagerNextGroup.CommandArgument = ((currentGroup + 1) * GROUP_SIZE + 1).ToString();
            }
            this.PagerInformationString = Resources.LanguageResource.AS_CustomPager_PagerInformationString;// GetGlobalResourceObject("LanguageResource", "AS_CustomPager_PagerInformationString").ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bool isModal = false;
            if (this.Page is ReportPage)
            {
                isModal = ((ReportPage)Page).IsModal;
            }
            else if (this.Page is NonReportPage)
            {
                isModal = ((NonReportPage)Page).IsModal;
            }

            if (isModal)
            {
                AS.Controls.Telerik.ASRadComboBox cboPageSize = this.FindControl("pagerPageSizeComboBox") as AS.Controls.Telerik.ASRadComboBox;
                if (cboPageSize != null)
                {
                    cboPageSize.SelectedIndexChanged += (s, ev) =>
                    {
                        AS.Controls.Telerik.ASRadAjaxManager ajax = (AS.Controls.Telerik.ASRadAjaxManager)AS.Controls.Telerik.ASRadAjaxManager.GetCurrent(this.Page);
                        if (ajax != null) ajax.ResponseScripts.Add("setTimeout('AdjustModalSize();', 500);");

                    };
                }
            }
        }
        protected void pagerGo_Command(object sender, CommandEventArgs e)
        {
            System.Web.UI.WebControls.LinkButton button = (System.Web.UI.WebControls.LinkButton)sender;
            this.GotoPage(int.Parse(button.Text));
        }
        protected void pagerGoToGroup_Command(object sender, CommandEventArgs e)
        {
            this.GotoPage(int.Parse(e.CommandArgument.ToString()));
        }


    }
}