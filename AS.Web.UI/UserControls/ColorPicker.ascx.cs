using System;
using System.Drawing;


public partial class UserControls_ColorPicker : System.Web.UI.UserControl
{
    public Color SelectedColor
    {
        get 
        {
            return this.uxColorPicker.SelectedColor;
        }
        set 
        {            
            this.uxColorPicker.SelectedColor = value;
            this.uxSelectedColor.InnerHtml = ColorTranslator.ToHtml(value);
        }
    }    
    public string DefaultColor
    {       
        set
        {
            this.uxColorPicker.SelectedColor = ColorTranslator.FromHtml(value);
            this.uxSelectedColor.InnerHtml = ColorTranslator.ToHtml(this.SelectedColor);           
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.uxSelectedColor.InnerHtml = ColorTranslator.ToHtml(this.SelectedColor);      
        // If exists --> do not regist        
    }   
   
}
