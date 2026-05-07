using AS.Common.DataProtection;
using AS.Common.Mail;
using AS.VW.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Task.Common
{
    public static class EmailHelper
    {
        public static SmtpMailSettings InitialMailSettings()
        {
            var mailSetting = new SmtpMailSettings();
            SmtpAuthentication authen;
            switch (AppConfigurations.GetStringAppSettings("SmtpAuthentication", "anonymous").ToLower())
            {
                case "anonymous":
                    authen = SmtpAuthentication.Anonymous;
                    break;
                case "basic":
                    authen = SmtpAuthentication.Basic;
                    break;
                case "ntlm":
                    authen = SmtpAuthentication.NTLM;
                    break;
                default:
                    authen = SmtpAuthentication.Anonymous;
                    break;
            }

            mailSetting.AuthenticationType = authen;
            mailSetting.EnableSSL = AppConfigurations.GetStringAppSettings("SmtpSSL") == "true";
            mailSetting.Password = Cryptophy.DecryptText("SmtpPassword");
            mailSetting.Username = Cryptophy.DecryptText("SmtpUsername");
            mailSetting.Server = AppConfigurations.GetStringAppSettings("SmtpServer");
            mailSetting.Port = int.Parse(AppConfigurations.GetStringAppSettings("SmtpPort"));

            return mailSetting;
        }

        public static void SendEmailAlert(string taskName, string content)
        {
            var isSendAlert = AppConfigurations.GetBoolAppSettings("IsSendAlert", false);

            if (!isSendAlert || string.IsNullOrWhiteSpace(content)) return;

            var configEmail = InitialMailSettings();
            var fromEmail = AppConfigurations.GetStringAppSettings("SendAlertFromEmail", string.Empty);
            var toEmail = AppConfigurations.GetStringAppSettings("SendAlertToEmail", string.Empty);
            var subject = AppConfigurations.GetStringAppSettings("SendAlertSubject", "[VW] Scheduler Task Alert");

            if (!string.IsNullOrEmpty(fromEmail) && !string.IsNullOrEmpty(toEmail))
            {
                try
                {
                    SmtpMail.SendEmail(fromEmail, toEmail, $"{subject} - {taskName}", content, configEmail);
                }
                catch (Exception ex)
                {
                    AS.Common.Logger.LoggerManager.Error("SendEmailAlert: " + ex.Message, ex);
                }
            }
        }
    }
}
