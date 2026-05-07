using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Represents control custom pager - TK42763
/// </summary>
public partial class UserControls_ASPager : System.Web.UI.UserControl
{
    #region CONST

    private readonly int GROUP_SIZE = 5;

    #endregion

    #region Properties

    protected int CurrentPageIndex { get; set; }
    protected int CurrentGroup { get; set; }
    protected int TotalGroup { get; set; }
    protected int PageCount { get; set; }
    private string _FuncPageChange = "PageChanged";
    public string FuncPageChange
    {
        get { return _FuncPageChange; }
        set { _FuncPageChange = value; }
    }

    #endregion

    //#region Events

    //public event Page_Changed PageChanged;
    //public delegate void Page_Changed(object sender, int pageIndex);

    //#endregion

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void uxPager_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        switch (e.Item.ItemType)
        {
            case ListItemType.Header:
                Literal uxPreviousGroup = e.Item.FindControl("uxPreviousGroup") as Literal;
                if (CurrentGroup > 0)
                {
                    uxPreviousGroup.Visible = true;
                    uxPreviousGroup.Text = VeraCodeExtensions.DoVeraCode(string.Format("<a class='rgPageNum' href='javascript:{0}({1})'>...</a>", FuncPageChange, CurrentGroup * GROUP_SIZE));
                }
                else
                    uxPreviousGroup.Visible = false;
                break;
            case ListItemType.Footer:
                Literal uxNextGroup = e.Item.FindControl("uxNextGroup") as Literal;
                if (CurrentGroup < TotalGroup - 1)
                {
                    uxNextGroup.Visible = true;
                    uxNextGroup.Text = VeraCodeExtensions.DoVeraCode(string.Format("<a class='rgPageNum' href='javascript:{0}({1})'>...</a>", FuncPageChange, (CurrentGroup + 1) * GROUP_SIZE + 1));
                }
                else
                    uxNextGroup.Visible = false;
                break;
        }
    }

    #endregion

    #region Utilities

    // Need move to common
    private CultureInfo GetCulture()
    {
        string selectedLanguage = WebSiteConstants.USCulture;

        if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.English)
        {
            selectedLanguage = WebSiteConstants.USCulture;
        }
        else if (SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.Spanish)
        {
            selectedLanguage = WebSiteConstants.SpanishCulture;
        }

        return new CultureInfo(selectedLanguage);

    }

    public void InitPager(int totalRows, int pageSize, int currentPageIndex = 0)
    {
        CurrentPageIndex = currentPageIndex;
        double dblPageCount = (double)((decimal)totalRows / Convert.ToDecimal(pageSize));
        PageCount = (int)Math.Ceiling(dblPageCount);

        TotalGroup = PageCount / GROUP_SIZE;
        if (PageCount % GROUP_SIZE > 0)
            TotalGroup++;
        CurrentGroup = currentPageIndex / GROUP_SIZE;

        List<int> pages = new List<int>();
        for (int i = 1; i <= GROUP_SIZE; i++)
        {
            int page = CurrentGroup * GROUP_SIZE + i;
            if (page > PageCount)
                break;
            pages.Add(page);
        }
        pagerList.DataSource = pages;
        pagerList.DataBind();

        string pagerText = HttpContext.GetGlobalResourceObject("MessageManager", "Generic_PagerText", GetCulture()).ToString();
        pagerInformationLabel.Text = VeraCodeExtensions.DoVeraCode(string.Format(pagerText, (currentPageIndex + 1), PageCount, (currentPageIndex * pageSize) + 1, (currentPageIndex + 1) * pageSize < totalRows ? (currentPageIndex + 1) * pageSize : totalRows, totalRows));
    }

    #endregion

}