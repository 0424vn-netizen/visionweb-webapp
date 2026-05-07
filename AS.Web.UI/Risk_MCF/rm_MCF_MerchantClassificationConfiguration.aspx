<%@ Page Title="Merchant Classification Configuration" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MerchantClassificationConfiguration.aspx.cs" Inherits="rm_MCF_MerchantClassificationConfiguration" ValidateRequest="false" meta:resourcekey="PageResource1" %>

<asp:content id="Content2" contentplaceholderid="ContentPage" runat="Server">
    <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="litMultiplierSetting">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="litMultiplierSetting" LoadingPanelID="uxLoadingPanelCustom"/>
                    <tek:AjaxUpdatedControl ControlID="uxGroupList" LoadingPanelID="uxLoadingPanelCustom"/>
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxCreateMode">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxCreateMode" />
                </UpdatedControls>
            </tek:AjaxSetting>
                    
            <tek:AjaxSetting AjaxControlID="uxSearchButton">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGroupList" LoadingPanelID="uxLoadingPanelCustom"/>
                    <tek:AjaxUpdatedControl ControlID="uxExportTop" LoadingPanelID="uxLoadingPanelCustom"/>
                </UpdatedControls>
            </tek:AjaxSetting>       
            <tek:AjaxSetting AjaxControlID="btnRebind">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGroupList" LoadingPanelID="uxLoadingPanelCustom"/>
                </UpdatedControls>
            </tek:AjaxSetting>            
            <tek:AjaxSetting AjaxControlID="uxChangeFilterStatus">
                <UpdatedControls>                    
                    <tek:AjaxUpdatedControl ControlID="uxGroupList"  LoadingPanelID="uxLoadingPanelCustom"/>
                </UpdatedControls>
            </tek:AjaxSetting>           
            <tek:AjaxSetting AjaxControlID="uxGroupList">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxGroupList"  LoadingPanelID="uxLoadingPanelCustom"/>
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
    <as:Panel runat="server" ID="uxFilteringOptionsContainer"
        Width="100%" TemplateName="ascontainer_greyborder.tpl" meta:resourcekey="uxFilteringOptionsContainerResource1">
        <div class="row collapse report-filter-panel">
            <div class="col-md-12 report-filter">
                <div class="filter-block text-left">
                    <div class="filter-item">
                        <label class="text-default-gray inline-block  valign-top">
                            <as:Literal ID="Literal1" runat="server" Text="Classification" meta:resourcekey="uxLiteralClassification"></as:Literal>                        </label>
                        <div class="inline-block">
                            <as:RadTextBox ID="uxClassificationName" runat="server" Width="250px" MaxLength="50"
                                CssClass="rf_TextBox normal-italic" LabelCssClass="" LabelWidth="64px" Resize="None" onkeypress="return SearchEnterOnTextbox(event);" EmptyMessage='<%# GetLocalResourceObject("uxFilterClassification.Text")%>'>
                         </as:RadTextBox>
                             <div class="bottom-error text-left" style="width:260px;">                        
                                <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxClassificationName" Message="" ShowOnLoad="False">
                                            </as:ValidatorMessage>
                            </div>
                            
                        </div>
                    </div>                    
                </div>
                <div class="filter-block text-left">                    
                     <div class="filter-item">
                         <label class="font-weight-bold text-dark-gray">&nbsp;&nbsp;or&nbsp;&nbsp;</label>
                         
                     </div>
                </div>
                <div class="filter-block text-left">                
                    <div class="filter-item">
                        <label class="text-default-gray inline-block valign-top">
                            <as:Literal ID="ltAttributeName" runat="server" Text="Attribute" meta:resourcekey="uxLiteralAttribute"></as:Literal></label>
                        <div class="inline-block">
                            <as:RadComboBox ID="uxAttributeList" runat="server" EnableEmbeddedBaseStylesheet="False" Filter="Contains" MarkFirstMatch="true"
                                DataValueField="AttributeID" DataTextField="AttributeName" Width="250px" 
                                MaxLength="100" meta:resourcekey="uxFilterAttribute" CheckBoxes="true" EmptyMessage='<%# GetLocalResourceObject("uxFilterAttribute.Text")%>' OnClientItemChecked="OnClientItemChecked">
                                
                            </as:RadComboBox>
                        </div>
                    </div>
                    <div class="filter-item valign-bottom">
                        <as:Button ID="uxSearchButton" runat="server" Text="Submit" OnClick="uxSearchButton_Click" OnClientClick="if(!ValidateInputFilter()) {return false;}"
                            CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="uxSearchButtonResource1" />
                    </div>
                </div>                
            </div>
        </div>
    </as:Panel>
    <div class="row">
        <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
            <span class="btn btn-link btn-report-filter">
                <as:Literal ID="Literal2" runat="server" Text="FILTER" meta:resourcekey="Literal2Resource1"></as:Literal></span>
        </div>
    </div>
    <as:PlaceHolder ID="dfd" runat="server">
        <div class="row no-margin-bottom">
             <div class="col-md-8 mb-3x">
                     <uc:PageTitle ID="uxReportTitle" ReportTitle="Merchant Classification Configuration" HasFilteringOption="false" meta:resourcekey="uxReportTitleResource1"
                    runat="server" /> 
                </div>
                              
               <div class="col-md-4 ipmt dropdown on-top mb-3x">
                   <h1 class="dark-blue pull-right">
                       <as:LinkButton ID="litMultiplierSetting" CssClass="link-back font-size-default" runat="server" Text="Multiplier Settings" meta:resourcekey="uxLiteralMultiplierSetting"  OnClientClick="ShowPopupModal('rm_MCF_MultiplierSettingsModal.aspx', 'auto'); return false;"></as:LinkButton>
                   </h1>
               </div>
                     
            <div class="col-md-12">
                <i class="text-muted"><as:Literal ID="Literal4" meta:resourcekey="uxReportSubTitleResource1" runat="server"></as:Literal></i>
            </div>
            
        </div>
        <div class="height-6"></div>
        <div class="row">
            <div class="col-md-12 no-margin-action-container">
                <as:LinkButton runat="server" ID="uxCreateMode" CssClass="btn btn-default" OnClientClick="ShowPopupModal('rm_MCF_MerchantClassification_CreateNewModal.aspx', 'auto'); return false;" meta:resourcekey="uxCreateModeResource1">Create New Classification</as:LinkButton>
            </div>
        </div>
         <div class="height-18"></div>
        <div id="uxFilterStatusContainer" runat="server">
            <div class="row">
                <div class="col-md-6 dark-blue">                    
                    <div class="control-inline">
                        <as:RadioButton ID="uxFilterStatusAll" runat="server" Text="All" GroupName="uxFilterStatus"
                            xValue="-1" Checked="true" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusAllResource1" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxFilterStatusActive" runat="server" Text="Active" GroupName="uxFilterStatus"
                            xValue="1"  onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusActiveResource1" />
                    </div>
                    <div class="control-inline">
                        <as:RadioButton ID="uxFilterStatusInactive" runat="server" Text="Inactive" GroupName="uxFilterStatus"
                            xValue="0" onclick="uxFilterStatus_Checked()" meta:resourcekey="uxFilterStatusInactiveResource1" />
                    </div>
                    <asp:Button ID="uxChangeFilterStatus" runat="server" OnClick="uxChangeFilterStatus_Click"
                        CssClass="display-none" meta:resourcekey="uxChangeFilterStatusResource1"></asp:Button>
                </div>
                <div class="col-md-6 pull-right mt-4x">                    
                     <uc:UxExport ID="uxExportTop" runat="server" GridID="uxGroupList" IsOnTop="true" ShowPDF="false"
                GridHeader="RISK MANAGEMENT - ASSIGNMENT - CONFIGURATION"  meta:resourcekey="uxExportTopResource1"/>
                </div>
            </div>
        </div>     
        
          
        <div id="uxDivPannel" runat="server" class="mt-1x">
           
            <as:ASGrid ID="uxGroupList" runat="server" GridLines="None" AllowPaging="True" AllowAutomaticUpdates="True" IsAutoExportTemplate="true"
                AllowSorting="True" AutoGenerateColumns="False" AllowFilteringByColumn="false"  InsertTempColumnAtTheEnd="false"
                OnItemCommand="uxGroupList_ItemCommand" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxGroupListResource1">
                <MasterTableView DataKeyNames="MerchantClassificationID" CommandItemDisplay="None">
                    <Columns>
                        <%--Classification name column --%>
                        <as:ASGridBoundColumn HeaderText="Classification Name" HeaderTooltip="Classification Name" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Left" UniqueName="MerchantClassificationName" SortExpression="MerchantClassificationName"
                            DataField="MerchantClassificationName" meta:resourcekey="ASGridTemplateColumnResource1" >
                            
                        </as:ASGridBoundColumn>
                        <%--Attributes column --%>
                        <as:ASGridBoundColumn HeaderText="Attributes" HeaderTooltip="Attributes" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Right" UniqueName="Attributes" SortExpression="Attributes" ASFormat="Integer"
                            DataField="Attributes" meta:resourcekey="ASGridTemplateColumnResource2" HeaderStyle-Width="130px">
                            
                        </as:ASGridBoundColumn>
                        <%-- Multiplier column --%>
                        <as:ASGridBoundColumn HeaderText="Multiplier" HeaderTooltip="Multiplier" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Right" UniqueName="Multiplier" SortExpression="Multiplier" ASFormat="Number1Digit"
                            DataField="Multiplier" meta:resourcekey="ASGridTemplateColumnResource3"  HeaderStyle-Width="130px">
                        </as:ASGridBoundColumn>
                        <%--Active column--%>
                        <as:ASGridBoundColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center" DataField="IsActiveText" UniqueName="ActivateDeactivateHidden"
                            Display="false" meta:resourcekey="ASGridBoundColumnResource1"  HeaderStyle-Width="130px">
                                                                                    
                        </as:ASGridBoundColumn>                        
                        <tek:GridHyperLinkColumn 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="ActivateDeactivate" HeaderStyle-Width="130px" meta:resourcekey="ASGridTemplateColumnResource5">
                           
                        </tek:GridHyperLinkColumn>
                        <as:ASGridTemplateColumn 
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" UniqueName="Edit"  HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <as:Literal ID="btnEdit" runat="server" Text="Edit" meta:resourcekey="LinkButton1Resource1" />
                            </ItemTemplate>                            
                          
                        </as:ASGridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </as:ASGrid>
        </div>
        <div style="display:none" > <asp:Button runat="server" ID="btnRebind" OnClick="btnRebind_Click"/> </div>
        <asp:Button ID="uxActivateDeactivate" runat="server" OnClick="uxActivateDeactivate_Click"
            CssClass="display-none" meta:resourcekey="uxActivateDeactivateResource1" />
        <asp:HiddenField ID="uxActivateDeactivateData" runat="server" />       
    </as:PlaceHolder>

    <as:Validator ID="Validator1" runat="server" ValidationFunction="ValidateInputFilter" MessageType="Inline" MessageContainerClientID="" meta:resourcekey="Validator1Resource1">
        <Items>            
            <as:RegExValidationItem ControlToValidateID="uxClassificationName" RegularExpression="^((?!(<[^ \t]))(?!(&#)).)*$" meta:resourcekey="ValidationMessages_V1" />
        </Items>
    </as:Validator>
    <tek:RadCodeBlock runat="server" ID="RadCodeBlock">
        <script type="text/javascript">
            var rm_Txt_Classification_Name = '<%= uxClassificationName.ClientID %>';
            var rm_Btn_Search = '<%= uxSearchButton.ClientID %>';
            var rm_Btn_Rebind_ClientId = '<%= btnRebind.ClientID %>';
            var rm_Empty_Message_Attributes = '<%= GetLocalResourceObject("uxFilterAttribute.Text").ToString() %>';
            var rm_MerchantClassification_uxChangeFilterStatus = "<%=uxChangeFilterStatus.ClientID%>";
            var rm_MerchantClassification_uxActivateDeactivateData = "<%=uxActivateDeactivateData.ClientID%>";
            var rm_MerchantClassification_uxActivateDeactivate = "<%=uxActivateDeactivate.ClientID%>";
            var rm_Atributes_ClientID = "<%=uxAttributeList.ClientID %>";
            var rm_MerchantClassification_js_String1 = '<%=GetLocalResourceObject("rm_MerchantClassification_js_String1").ToString()%>';
            var rm_MerchantClassification_js_String2 = '<%=GetLocalResourceObject("rm_MerchantClassification_js_String2").ToString()%>';
            var rm_MerchantClassification_js_String3 = '<%=GetLocalResourceObject("rm_MerchantClassification_js_String3").ToString()%>';
            var rm_Message_Classification_InUse = '<%= GetLocalResourceObject("ClassificationInUser.Message").ToString() %>';
            </script>
        <script src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_MerchantClassification.js"></script>
    </tek:RadCodeBlock>
</asp:content>


