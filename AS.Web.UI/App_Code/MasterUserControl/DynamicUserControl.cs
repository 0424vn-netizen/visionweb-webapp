using AS.Controls.Global;
using AS.Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Xml;
public abstract class ModuleUserControl : GlobalUserControl
{
    public abstract void LoadData(params object[] paramters);

    public abstract void ReloadData(params object[] paramters);

    public abstract bool SubmitData(params object[] paramters);
    public abstract bool ValidateData(object extraInfo);
    public void RegisterClientValidationFunction(ControlCollection controls)
    {
        AS.Controls.Global.HiddenField hdfClientValidationFunction = (AS.Controls.Global.HiddenField)this.Parent.FindControl("hdfClientValidationFunction");
        if (hdfClientValidationFunction != null)
        {
            foreach (Control control in controls)
            {
                if (control is AS.Controls.Global.Validator && control.Visible)
                {


                    if (hdfClientValidationFunction.Value.IsNullOrEmpty())
                    {
                        hdfClientValidationFunction.Value = ((AS.Controls.Global.Validator)control).ValidationFunction;
                    }
                    else
                    {
                        hdfClientValidationFunction.Value = hdfClientValidationFunction.Value + "," + ((AS.Controls.Global.Validator)control).ValidationFunction;
                    }

                }
            }
        }
    }
}
public class DynamicUserControl<T> : GlobalUserControl where T: ModuleUserControl
{
    private List<T> renderedControls = new List<T>();

    [Browsable(true),
    Category("Configutation"),
    Description("Specifies an absolute or relative path to the content to display")]
    protected string GroupPage { get; set; }


    public string ClientIDValidationFunction
    {
        get 
        {
            return _validationPanel.ClientID;
        }
    }
    PlaceHolder _usercontrolPanel;
    HiddenField _validationPanel;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _usercontrolPanel = (PlaceHolder)FindControl("ctrlListPanel");
        _validationPanel = (HiddenField)FindControl("hdfClientValidationFunction");
        LoadUserControl();
    }

    protected bool SubmitData(params object[] paramters)
    {
        foreach (T uc in renderedControls)
        {
            uc.SubmitData(paramters);
        }
        return true;
    }

    protected void ReloadData(params object[] paramters)
    {
        foreach (T uc in renderedControls)
        {
            uc.ReloadData(paramters);
        }
    }

    protected void LoadData(params object[] paramters)
    {
        foreach (T uc in renderedControls)
        {
            uc.LoadData(paramters);
        }
    }

    public bool Validate(object extraInfo)
    {
        bool isValid = true;
        foreach (T uc in renderedControls)
        {
            if (isValid)
                isValid = uc.ValidateData(extraInfo);
        }
        return isValid;
    }

    private void LoadUserControl()
    {
        int clientId = SessionManager.CurrentClient;
        XmlDocument xmlDoc = General.GetXMLDocument("~/App_Data/UserControlConfig.xml");

        //Get UserControl file name from config file
        XmlNodeList nodes = xmlDoc.SelectNodes("Clients/Client[@Id='" + clientId + "']/Control[@GroupPage='" + this.GroupPage + "']");
        foreach (XmlNode node in nodes)
        {
            string controlPath = node.Attributes["ControlPath"].Value;
            if (!controlPath.IsNullOrEmpty())
            {
                Control control = new Control();
                control = LoadControl(controlPath);
                control.ID = string.Format("ux_{0}_{1}", controlPath.Split('.')[0].Replace("~", "").Replace("/", ""), clientId);
                _usercontrolPanel.Controls.Add(control);

                renderedControls.Add((T)control);
            }
        }
    }
}