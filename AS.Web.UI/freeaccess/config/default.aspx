<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="freeaccess_config_default" ViewStateEncryptionMode="Always" EnableViewStateMac="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hierarchy Configurations</title>
    <style type="text/css">
        html,body
        {
        	background-color: #FFF;
        	color: #000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <tek:RadScriptManager ID="uxMasterScriptManager" runat="server" AsyncPostBackTimeout="120" EnablePageMethods="true" />
    
  
    <tek:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" Skin="Forest"/>
    <tek:RadFormDecorator runat="server" DecoratedControls="All"/>
    
    <tek:RadAjaxManager runat="server" DefaultLoadingPanelID="uxLoadingPanel" EnableAJAX="false">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxClients">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxHierarchyList" />
                <tek:AjaxUpdatedControl ControlID="uxDrillDownHierarchyList" />
                <tek:AjaxUpdatedControl ControlID="uxUserModes" />
                <tek:AjaxUpdatedControl ControlID="uxPositionHierarchyList" />
                <tek:AjaxUpdatedControl ControlID="uxAssHierarchyFilterList" />
                <tek:AjaxUpdatedControl ControlID="uxMMWHierarchyList" />
            </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxHierarchyList">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxHierarchyList" />
                <tek:AjaxUpdatedControl ControlID="uxDrillDownHierarchyList" />
                <tek:AjaxUpdatedControl ControlID="uxUserModes" />
                <tek:AjaxUpdatedControl ControlID="uxPositionHierarchyList" />
                <tek:AjaxUpdatedControl ControlID="uxAssHierarchyFilterList" />
                <tek:AjaxUpdatedControl ControlID="uxMMWHierarchyList" />
            </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManager>
    <tek:RadAjaxLoadingPanel runat="server" ID="uxLoadingPanel"/>
    
    <h1>Client
     <tek:RadComboBox runat="server" ID="uxClients" AutoPostBack="true" OnSelectedIndexChanged="RadComboBox_SelectedIndexChanged" DataTextField="ClientName" DataValueField="ASClient"/>
    </h1>
    <div>
    <h2>Filtering options</h2>
    <h3> Grid 1. Define hierarchy </h3>
            <b>Note: </b><br />
            Column with <b style="color: red">*</b> can not be blank.<br />
            <b>EntityType:</b><br />
            - To be displayed as a reporting hierarchy filtering option, Entity Type must be NOT NULL<br />
            - To hide it from reporting hierarchy filtering option, Entity Type must be NULL<br />
            <b>Hierarchy Level:</b><br />
            - HierarchyLevel = 1: reserved for PORTFOLIO (SITEID)<br />
            - HierarchyLevel = 0: any user can use this hierarchy report filtering options<br />
            - For any hierachy options, please start by 2.<br />
            <b>Validate Input:</b><br />
                <div style="position:relative;padding-left: 15px;">
                True: Validate Expression => Valid charaters<br />
                False: Validate Expression => Invalid charaters<br />
                NULL: Validate Expression => validate regular expression<br />
                </div>
            <b>Is Merchant ID:</b> True for MerchantNumber mode
    <tek:RadGrid runat="server" ID="uxHierarchyList" OnNeedDataSource="RadGrid_NeedDataSource" AutoGenerateColumns="false" AllowMultiRowEdit="false" HeaderStyle-Wrap="false" AlternatingItemStyle-Wrap="false" ItemStyle-Wrap="false" OnItemCommand="RadGrid_ItemCommand">
        <MasterTableView EditMode="InPlace" CommandItemDisplay="TopAndBottom" DataKeyNames="HierarchyID"> 
            <Columns>
                <tek:GridEditCommandColumn ButtonType="ImageButton" />
                <tek:GridBoundColumn DataField="HierarchyName" HeaderText="Hierarchy Name *" UniqueName="HierarchyName" />
                <tek:GridBoundColumn DataField="HierarchyMode" HeaderText="Hierarchy Mode *" UniqueName="HierarchyMode" />
                <tek:GridBoundColumn DataField="HierarchyLevel" HeaderText="Hierarchy Level (int) *" UniqueName="HierarchyLevel" />
                <tek:GridBoundColumn DataField="BEProcessor" HeaderText="Processor" UniqueName="BEProcessor" />
                <tek:GridBoundColumn DataField="EntityType" HeaderText="Entity Type (int)" UniqueName="EntityType" />
                
                <tek:GridBoundColumn DataField="UserMode" HeaderText="User Mode" UniqueName="UserMode" />
                
                <tek:GridBoundColumn DataField="MinLength" HeaderText="Min Length (int)" UniqueName="MinLength" />
                <tek:GridBoundColumn DataField="MaxLength" HeaderText="Max Length (int)" UniqueName="MaxLength" />
                <tek:GridBoundColumn DataField="ValidateExpression" HeaderText="Validate Expression" UniqueName="ValidateExpression" />
                <tek:GridBoundColumn DataField="ValidInput" HeaderText="Valid Input (bool)" UniqueName="ValidInput" />
                <tek:GridBoundColumn DataField="MaxLenMsg" HeaderText="MaxLength Msg" UniqueName="MaxLenMsg" />
                <tek:GridBoundColumn DataField="MinLenMsg" HeaderText="MinLength Msg" UniqueName="MinLenMsg" />
                <tek:GridBoundColumn DataField="InvalidMsg" HeaderText="Invalid Msg" UniqueName="InvalidMsg" />
                <tek:GridBoundColumn DataField="IsMerchant" HeaderText="Is Merchant (bool)" UniqueName="IsMerchant" />
                <tek:GridBoundColumn DataField="HierarchyGridName" HeaderText="Grid Name" UniqueName="HierarchyGridName" />
                <tek:GridBoundColumn DataField="HierarchyPrefix" HeaderText="Hierarchy Prefix" UniqueName="HierarchyPrefix" />
                <tek:GridBoundColumn DataField="ShowPrefix" HeaderText="Show Prefix (bool)" UniqueName="ShowPrefix" />
                
                
                 <tek:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" />
            </Columns>
            <CommandItemSettings ShowRefreshButton="true"  />
        </MasterTableView>
        <ClientSettings>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
        </ClientSettings>
    </tek:RadGrid>
    <table width="100%">
    <tr>
        <td valign="top">
        <h3> Grid 1.1. Define drilldown </h3>
    <tek:RadGrid runat="server" ID="uxDrillDownHierarchyList" OnNeedDataSource="RadGrid_NeedDataSource" AutoGenerateColumns="false"  Width="400px" OnItemDataBound="RadGrid_ItemDataBound"  DataKeyNames="HierarchyID">
        <MasterTableView> 
            <Columns>
                
                <tek:GridBoundColumn DataField="HierarchyName" HeaderText="Hierarchy Name" UniqueName="HierarchyName" />
                <tek:GridTemplateColumn HeaderText="Drilldown Hierarchy">
                    <ItemTemplate>
                    <tek:RadComboBox ID="uxDrilldownItems" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBox_SelectedIndexChanged" DataTextField="HierarchyName" DataValueField="HierarchyID"/>
                    <asp:HiddenField runat="server" ID="uxDrilldownHierarchyID" Value='<%#Eval("HierarchyID") %>' />
                    </ItemTemplate>
                </tek:GridTemplateColumn>
            </Columns>
        </MasterTableView>
        
    </tek:RadGrid>
    </td>
        <td valign="top">
        <h3> Grid 1.2. Define position for <tek:RadComboBox ID="uxUserModes" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="RadComboBox_SelectedIndexChanged" DataTextField="UserMode" DataValueField="UserMode" /></h3>
     <tek:RadGrid runat="server" ID="uxPositionHierarchyList" OnNeedDataSource="RadGrid_NeedDataSource" AutoGenerateColumns="false"  Width="600px"  OnItemDataBound="RadGrid_ItemDataBound" OnItemCreated="RadGrid_ItemCreated" >
        <MasterTableView EditMode="InPlace" CommandItemDisplay="None"  DataKeyNames="HierarchyID"> 
            <Columns>
                <tek:GridTemplateColumn HeaderText="Previous Hierarchy">
                    <ItemTemplate>
                    <tek:RadComboBox ID="uxPreHierarchies" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBox_SelectedIndexChanged" DataTextField="HierarchyName" DataValueField="HierarchyID" />
                    <asp:HiddenField runat="server" ID="uxSelectedHierarchyID" Value='<%#Eval("HierarchyID") %>' />
                    </ItemTemplate>
                </tek:GridTemplateColumn>
                <tek:GridBoundColumn DataField="HierarchyName" HeaderText="Hierarchy Name" UniqueName="HierarchyName" />
                <tek:GridTemplateColumn HeaderText="Next Hierarchy">
                    <ItemTemplate>
                    <tek:RadComboBox ID="uxNextHierarchies" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBox_SelectedIndexChanged" DataTextField="HierarchyName" DataValueField="HierarchyID"/>
                    </ItemTemplate>
                </tek:GridTemplateColumn>
                
                <tek:GridTemplateColumn HeaderText="Position Group (int)">
                    <ItemTemplate>
                    <tek:RadTextBox runat="server" ID="uxPositionGroup" Text='<%#Eval("PositionGroup") %>' Width="30px" OnTextChanged="RadTextBox_TextChanged" AutoPostBack="true"  />
                    </ItemTemplate>
                </tek:GridTemplateColumn>
                <tek:GridTemplateColumn HeaderText="Change group" Visible="false">
                    <ItemTemplate>
                    <tek:RadComboBox ID="uxGroupItems" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadComboBox_SelectedIndexChanged"/>
                    
                    </ItemTemplate>
                </tek:GridTemplateColumn>
            </Columns>
 
        </MasterTableView>
        
    </tek:RadGrid>
        </td>
    </tr>
    </table>
    
                    

    
     
    </div>
    <div>
    <h2> Grid 2. Assignment Hierarchy Filter</h2>
    <tek:RadGrid runat="server" ID="uxAssHierarchyFilterList" OnNeedDataSource="RadGrid_NeedDataSource" AutoGenerateColumns="false"  Width="600px" OnItemCreated="RadGrid_ItemCreated" OnItemCommand="RadGrid_ItemCommand" HeaderStyle-Wrap="false" AlternatingItemStyle-Wrap="false" ItemStyle-Wrap="false">
        <MasterTableView EditMode="InPlace" CommandItemDisplay="Top" DataKeyNames="HierarchyID"> 
            <Columns>
                <tek:GridEditCommandColumn ButtonType="ImageButton" />
                <tek:GridBoundColumn DataField="HierarchyName" HeaderText="Hierarchy Name" UniqueName="HierarchyName" ReadOnly="true" />
                <tek:GridBoundColumn DataField="DataText" HeaderText="Data Text" UniqueName="DataText" />
                <tek:GridBoundColumn DataField="DataKey" HeaderText="Data Key" UniqueName="DataKey" />
                <tek:GridBoundColumn DataField="SourceTable" HeaderText="Source Table" UniqueName="SourceTable" />
                <tek:GridBoundColumn DataField="MIFEntity" HeaderText="MIF Entity" UniqueName="MIFEntity" />
                <tek:GridBoundColumn DataField="SortSeq" HeaderText="Sort (int)" UniqueName="SortSeq" />
                <tek:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" />
            </Columns>
            <CommandItemTemplate>
                <div style="padding: 3px;">
                <tek:RadComboBox ID="uxHierarchies" runat="server" DataTextField="HierarchyName" DataValueField="HierarchyID" AutoPostBack="false"/>&nbsp;
                <tek:RadButton ID="uxAddHierarchy" CommandName="AddHierarchy" runat="server" Text="Add to list" />
                </div>
            </CommandItemTemplate>
        </MasterTableView>
        
    </tek:RadGrid>
    </div>
    <div>
    <h2> Grid 3. Multi Merchant Watch</h2>
    <tek:RadGrid runat="server" ID="uxMMWHierarchyList" OnNeedDataSource="RadGrid_NeedDataSource" AutoGenerateColumns="false"   OnItemCreated="RadGrid_ItemCreated" OnItemCommand="RadGrid_ItemCommand" Width="100%" HeaderStyle-Wrap="false" AlternatingItemStyle-Wrap="false" ItemStyle-Wrap="false">
        <MasterTableView EditMode="InPlace" CommandItemDisplay="Top" DataKeyNames="HierarchyID"> 
            <Columns>
                <tek:GridEditCommandColumn ButtonType="ImageButton" />
                <tek:GridBoundColumn DataField="HierarchyName" HeaderText="Hierarchy Name" UniqueName="HierarchyName" ReadOnly="true" />
                <tek:GridBoundColumn DataField="HierarchyGridHeaderText" HeaderText="Grid Header Text" UniqueName="HierarchyGridHeaderText"  HeaderStyle-Wrap="false" />
                <tek:GridBoundColumn DataField="HierarchyHeaderTooltip" HeaderText="Header Tooltip" UniqueName="HierarchyHeaderTooltip" HeaderStyle-Wrap="false"/>
                <tek:GridBoundColumn DataField="FilterTypeText" HeaderText="Filter Text" UniqueName="FilterTypeText" />
                <tek:GridBoundColumn DataField="FilterValueTextColumn" HeaderText="Filter Value Column" UniqueName="FilterValueTextColumn" HeaderStyle-Wrap="false"/>
                <tek:GridBoundColumn DataField="MWEntity" HeaderText="MW Entity" UniqueName="MWEntity" HeaderStyle-Wrap="false"/>
                <tek:GridBoundColumn DataField="MIFEntity" HeaderText="MIF Entity" UniqueName="MIFEntity" HeaderStyle-Wrap="false"/>
                <tek:GridBoundColumn DataField="MinCharactersSearch" HeaderText="Chars (int)" HeaderTooltip="Min Character to search " UniqueName="MinCharactersSearch" HeaderStyle-Wrap="false"/>
                <tek:GridBoundColumn DataField="DataText" HeaderText="Data Text" UniqueName="DataText" />
                <tek:GridBoundColumn DataField="DataKey" HeaderText="Data Key" UniqueName="DataKey" />
                <tek:GridBoundColumn DataField="SourceTable" HeaderText="Source Table" UniqueName="SourceTable" />
                <tek:GridBoundColumn DataField="SortSeq" HeaderText="Sort (int)" UniqueName="SortSeq" HeaderStyle-Wrap="false"/>
                <tek:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" />
            </Columns>
            <CommandItemTemplate>
                <div style="padding: 3px;">
                <tek:RadComboBox ID="uxHierarchies" runat="server" DataTextField="HierarchyName" DataValueField="HierarchyID"/>&nbsp;
                <tek:RadButton  ID="uxAddHierarchy" CommandName="AddHierarchy" runat="server" Text="Add to list" />
                </div>
            </CommandItemTemplate>
        </MasterTableView>
         <ClientSettings>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
        </ClientSettings>
    </tek:RadGrid>
    </div>
    
    </form>
</body>
</html>
