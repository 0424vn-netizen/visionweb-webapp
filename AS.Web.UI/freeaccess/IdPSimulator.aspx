<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IdPSimulator.aspx.cs" Inherits="freeaccess_IdPSimulator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Identity Provider Simulator</title>
    <style type="text/css">
    
        div#formFields {  width:400px; float: left; }
        h1  { font-size: 48px; font-weight: bold; text-align:center; }
        div#formFields { border: #ccc 1px solid; padding: 10px;  }
        .fieldLabels 
        { 
        	float: left;
            width: 108px;
            height: 19px;
            padding: 3px;
             
            
        }
        
        div#allControls {   border: #ccc 1px solid; padding: 30px; margin-left: 100px; width: 1030px; height: 270px;              }
        div.options { width: 250px; height: 250px; float: left; border:#ccc 1px solid; padding: 10px;  margin-left: 20px;   }
        
        #sendResponse {  font-size: 20px; font-weight: bold;  clear:both; margin-top: 0px; margin-left: 127px; }
         
        div#typeSelection { border: #ccc 1px solid; margin: 10px; margin-left: 10px; padding: 10px; padding-left: 0; }
        
        div.innerOptions {  border: #ccc 1px solid; margin: 10px; margin-left: 10px; padding: 10px; padding-left: 0;                                          }
           
        .radiobuttonlabel {
            margin: 3px 20px 0 10px;
            float: left;
        }
        
        div.optionsLabel { font-size: 18px; font-weight: bold; margin-left: 10px; }
        
        .responseSigned {  margin: 10px; font-size: 18px; }
    </style>
</head>
<body>
    <form id="ResponseForm" method="post" runat="server">
    <div style="width: 1300px">
        <h1>Identity Provider Simulator</h1>
        <div id="allControls">
            <div id="formFields">
                <div id="typeSelection">
                    <div class="radiobuttonlabel">Type:</div>
                    <asp:RadioButtonList ID="RadioButtonSSO_SLO"  runat="server" RepeatDirection="Horizontal" > 
                        <asp:ListItem Value="SSO" Selected="True">SSO&nbsp;</asp:ListItem><asp:ListItem Value="SLO"/> 
                    </asp:RadioButtonList>
                </div>
                <br /> 
                <span class="fieldLabels">Partner Name&nbsp;&nbsp;&nbsp;&nbsp; </span>
                <asp:TextBox ID="partnerName" runat="server" >TNBCI</asp:TextBox>   
                <br />
                <br />
                <span class="fieldLabels">Client ID</span><asp:TextBox ID="clientID" 
                    runat="server" >29</asp:TextBox><br />
                <br />
                <span class="fieldLabels">User Name&nbsp;&nbsp;&nbsp;&nbsp; </span><asp:TextBox ID="userName" runat="server">8788200018835</asp:TextBox>  
                <br />
                <br />
                </div>
            <div class="options">
                <p><asp:CheckBox ID="ResponseSigned" CssClass="responseSigned" text=" Response Signed" runat="server" /></p>
                <div class="innerOptions">
                 <div class="optionsLabel">Assertion Processing</div>
                 <asp:RadioButtonList ID="RadioButtonListAssertionSignedEncrypt"  runat="server" > 
                        <asp:ListItem Value="None" />
                        <asp:ListItem Value="Signed" Selected="True" /> 
                        <asp:ListItem Value="Encrypted"/> 
                    </asp:RadioButtonList>
                </div>           
                
                <br /><br />
                
            </div>
           
            <div class="options">
                <p><asp:CheckBox ID="CheckBoxSendSecurityLevel" CssClass="responseSigned" text="Send Security Level" runat="server" Checked="true" /></p>
                <div class="innerOptions">
                     <div class="optionsLabel">Security Level</div>
                            <asp:RadioButtonList ID="RadioButtonListSecurityLevel"  runat="server" > 
                            <asp:ListItem Value="1" Selected="True" />
                            <asp:ListItem Value="2"/> 
                            <asp:ListItem Value="3"/> 
                        </asp:RadioButtonList>
                     </div> 
				
				 <div class="innerOptions">
				  <div class="optionsLabel">Destination Application</div>
				  &nbsp;&nbsp;
						<asp:DropDownList ID="drpApplication" runat="server">
							<asp:ListItem Text="MS Reporting" Value="1">
							</asp:ListItem>
							<asp:ListItem Text="PCI" Value="2">
							</asp:ListItem>
							<asp:ListItem Text="None" Value="3">
							</asp:ListItem>
                            <asp:ListItem Text="Total" Value="4">
							</asp:ListItem>
						</asp:DropDownList>
                
                </div>
                				
            </div>
        </div>   
        <br /><br />
        <asp:Button ID="sendResponse" runat="server" Text="Send Response" Height="39px" 
                Width="186px" onclick="sendResponse_Click"  />
        <asp:Button ID="uxSendResponseFromSample" runat="server" Text="Send Response From File" Height="39px" 
                Width="186px" onclick="uxSendResponseFromSample_Click"  />
        
    </div>
    </form>
    
</body>
</html>
