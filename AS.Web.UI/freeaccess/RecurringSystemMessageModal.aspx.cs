using System;

public partial class freeaccess_RecurringSystemMessageModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.IsOpenRecurringSystemMessage = false;
        GeneralFuncsLib.SetCookie("is_open_recurring_system_message", "False");
    }
}