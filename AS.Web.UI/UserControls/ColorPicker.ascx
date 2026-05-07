<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ColorPicker.ascx.cs" Inherits="UserControls_ColorPicker" %>
<div style="float: left">
    <tek:RadColorPicker ID="uxColorPicker" runat="server" ShowIcon="true" PaletteModes="HSB"
        ShowEmptyColor="false" OnClientColorChange="uxColorPicker_UpdateSelectedColor">
    </tek:RadColorPicker>
</div>
<div id="uxSelectedColor" style="float: left; padding-left: 5px; padding-top: 3px"
    runat="server">
    #003172
</div>
<div style="clear: both">
</div>



