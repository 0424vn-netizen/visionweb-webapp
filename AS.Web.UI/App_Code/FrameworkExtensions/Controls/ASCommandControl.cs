using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CommandButton
/// </summary>
namespace AS.Controls.Global
{
    public class ASCommandControl : Control,ICallbackEventHandler
    {
        public string GetCallbackResult()
        {
            return ResponseString;
        }

        public string ClientSuccessCallbackFunc { get; set; }

        public string ClientErrorCallbackFunc { get; set; }

        public string CallServerFunc { get; set; }

        public string ResponseString { get; set; }

        public bool IsRefreshPostData { get; set; }

        public bool IsUseAsync { get; set; }

        public event CreateResponseDataEvent CreateResponseData;
        public delegate void CreateResponseDataEvent(ASCommandControl sender, string eventArgument);

        protected virtual void OnCreateResponseData(ASCommandControl sender, string eventArgument)
        {
            if (CreateResponseData != null)
                CreateResponseData(sender,eventArgument);
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            OnCreateResponseData(this, eventArgument);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (CallServerFunc.IsNullOrEmpty())
                CallServerFunc = "CallServerOf" + ClientID;
            ClientScriptManager cm = Page.ClientScript;
            string reference = (IsRefreshPostData ? "__theFormPostData='';  WebForm_InitCallback();" : string.Empty)
                + cm.GetCallbackEventReference(this, "arg", ClientSuccessCallbackFunc, "",ClientErrorCallbackFunc, IsUseAsync);
            string script = "function " + CallServerFunc + "(arg, context){" + reference + ";}";
            cm.RegisterClientScriptBlock(GetType(), CallServerFunc, script, true);
        }

    }
}