using AS.WEB.UI.SamlSign;

namespace As.VisionWeb.Web
{
    public partial class SingleLogOff : SamlS
    {
        protected override bool ProcessUser()
        {
            LogoffStatus.Text = "You have been logged off.";
            Session.Abandon();

            return true;
        }

        protected override void ProcessSamlResponseFail()
        {
            LogoffStatus.Text = "You have been logged off.";
            Session.Abandon();
        }
    }
}
