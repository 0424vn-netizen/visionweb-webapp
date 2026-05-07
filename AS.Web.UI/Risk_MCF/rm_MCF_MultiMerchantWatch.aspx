<%@ Page Title="Multi-Merchant Watch" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="rm_MCF_MultiMerchantWatch.aspx.cs" Inherits="rm_MCF_MultiMerchantWatch"
    EnableEventValidation="false" meta:resourcekey="PageResource1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:PlaceHolder ID="fgf" runat="server">
        <%--Filtering option--%>
        <as:Container runat="server" ID="AsContainer1" Width="100%">
            <div class="row collapse report-filter-panel">
                <div class="col-md-12 report-filter">
                    <div class="filter-item text-left">
                        <label>
                            <asp:Literal ID="LteralType" runat="server" meta:resourcekey="LteralTypeResource1"> Type:</asp:Literal></label><br>
                        <as:RadComboBox ID="uxFilterType" runat="server" EnableEmbeddedSkins="false" EnableEmbeddedBaseStylesheet="false"
                            Width="250" DataValueField="DataKey" DataTextField="DataText" OnClientSelectedIndexChanged="uxFilterType_OnClientSelectedIndexChanged"
                            OnSelectedIndexChanged="uxFilterType_OnSelectedIndexChanged" MaxHeight="350" Filter="Contains" MarkFirstMatch="true"
                            AutoPostBack="true" meta:resourcekey="uxFilterTypeResource1">
                        </as:RadComboBox>
                    </div>

                    <div id="cidSearchValue" class="filter-item text-left">
                        <label><span id="cidSearchValuePrompt"></span></label>
                        <br>
                        <as:RadComboBox ID="uxSearchValueCommonKey" runat="server" EnableEmbeddedSkins="false"
                            MaxHeight="350" Filter="Contains" MarkFirstMatch="true"
                            EnableEmbeddedBaseStylesheet="false" Width="300" DataValueField="DataKey" DataTextField="DataText"
                            AllowCustomText="true" EmptyMessage="enter first few characters"
                            OnItemsRequested="uxSearchValueCommonKey_ItemsRequested" meta:resourcekey="uxSearchValueCommonKeyResource1">
                        </as:RadComboBox>

                    </div>
                    <div class="filter-item valign-bottom" id="cidSubmitPanel">
                        <as:Button ID="uxSubmit" runat="server" Text="Add to Watch" OnClick="uxSubmit_OnClick"
                            OnClientClick="return ValidateData();" meta:resourcekey="uxSubmitResource1" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </as:Container>
        <div class="row">
            <div class="col-md-12 text-center" data-target=".report-filter-panel" data-toggle="collapse">
                <span class="btn btn-link btn-report-filter">
                    <asp:Literal ID="LiteralFilter" runat="server" meta:resourcekey="LiteralFilterResource1">FILTER</asp:Literal>
                </span>
            </div>
        </div>
        <%--End filtering--%>
        <div class="row">
            <div class="col-md-12 no-margin-bottom">
                <uc:PageTitle ID="uxPageTitle" runat="server" ReportTitle="Multi-Merchant Watch" meta:resourcekey="uxPageTitleResource1" />
            </div>
        </div>
        <div class="height-18"></div>
        <div class="row">
            <div class="col-md-9 no-margin-bottom" id="divGrid" runat="server">
                <!--Multiwatch Type-->
                <as:ASGrid ID="uxGrid" runat="server" AutoGenerateColumns="False" AllowSorting="false" InsertTempColumnAtTheEnd="false"
                    AllowPaging="False" GridLines="None" OnItemCommand="uxGrid_OnItemCommand" HeaderStyle-Width="80px" meta:resourcekey="uxGridResource1">
                    <MasterTableView AllowSorting="false" TableLayout="Auto">
                        <Columns>
                            <tek:GridBoundColumn DataField="FilterType" UniqueName="FilterType" Visible="false" meta:resourcekey="GridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </tek:GridBoundColumn>
                            <tek:GridBoundColumn DataField="FilterValue" UniqueName="FilterValue" Visible="false" meta:resourcekey="GridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>
                            </tek:GridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Watch Type" DataField="FilterTypeText" UniqueName="FilterTypeText"
                                HeaderTooltip="Watch Type" ASFormat="StaticString" SortExpression="FilterTypeText" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Watch" DataField="FilterValueText" UniqueName="FilterValueText"
                                HeaderTooltip="Watch" ASFormat="StaticString" SortExpression="FilterValueText" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="# of Merchants" DataField="MerchantCount" UniqueName="MerchantCount"
                                HeaderTooltip="Merchant Count" ASFormat="Integer" meta:resourcekey="ASGridBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <tek:GridTemplateColumn HeaderText="Action" UniqueName="RemoveCommand" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Center" HeaderTooltip="Action" meta:resourcekey="GridTemplateColumnResource1">
                                <ItemTemplate>
                                    <as:LinkButton ID="uxRemove" runat="server" CommandName="Remove" Text="Remove from Watch" meta:resourcekey="uxRemoveResource1" />
                                </ItemTemplate>

                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Bottom"></HeaderStyle>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </tek:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>

                    <HeaderStyle Width="80px"></HeaderStyle>

                </as:ASGrid>
                <!--Multi-merchant On Watch-->
            </div>
        </div>

        <uc:UxExport ID="uxExport" runat="server" GridID="uxReportGrid" GridHeader="Merchant(s) On Watch"
            GridTitle="Merchant(s) On Watch"
            FileName="Risk Management – Merchant Information - Multi-Merchant Watch"
            ShowWord="false" ShowPDF="false" meta:resourcekey="uxExportResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" IsAutoExportTemplate="true" IsCacheTemplateFile="false"
            AllowSorting="true" InsertTempColumnAtTheEnd="false" AllowPaging="true" GridLines="None" GridName="Risk Management - Merchant Information - Multi-Merchant Watch" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName"
                        ASFormat="DynamicString" HeaderTooltip="Merchant Name" SortExpression="MerchantName" meta:resourcekey="ASGridBoundColumnResource4" HeaderStyle-Width ="200px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant ID" DataField="MerchantNumber" UniqueName="MerchantNumber"
                        SortExpression="MerchantNumber" Display="false" meta:resourcekey="ASGridBoundColumnResource5">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Merchant Only" DataField="MerchantOnly" UniqueName="MerchantOnly"
                        ASFormat="StaticString" HeaderTooltip="Merchant Only" SortExpression="MerchantOnly" meta:resourcekey="ASGridBoundColumnResource6" HeaderStyle-Width ="70px">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>

        <div class="display-none">
            <as:Button ID="btnProcess" runat="server" OnClick="btnProcess_Click" IsStandardButton="true" meta:resourcekey="btnProcessResource1" />
            <as:HiddenField ID="hddProcessData" runat="server" />
            <as:Button ID="btnLoadList" runat="server" OnClick="btnProcess_Click" IsStandardButton="true" meta:resourcekey="btnLoadListResource1" />
            <as:Button ID="uxReloadGrids" runat="server" OnClick="uxReloadGrids_Click" IsStandardButton="true" meta:resourcekey="uxReloadGridsResource1" />
        </div>
        <as:RadCodeBlock ID="JavaScript" runat="server">
            <script type="text/javascript">
                var uxFilterTypeID = '<%=uxFilterType.ClientID%>';
                var uxReloadGridsID = '<%=uxReloadGrids.ClientID%>';
                var uxSearchValueCommonKeyID = '<%=uxSearchValueCommonKey.ClientID%>';
                var hddProcessDataID = '<%=hddProcessData.ClientID%>';
                var btnLoadListID = '<%=btnLoadList.ClientID%>';
                var btnProcessID = '<%=btnProcess.ClientID%>';
                var rm_MultiMerchantWatch_js_String1 = '<%=GetLocalResourceObject("rm_MultiMerchantWatch_js_String1").ToString()%>';
                var rm_MultiMerchantWatch_js_String2 = '<%=GetLocalResourceObject("rm_MultiMerchantWatch_js_String2").ToString()%>';
            </script>
            <script type="text/javascript" src="<%=ResolveUrl("~")%>res/js/risk_MCF/rm_MCF_MultiMerchantWatch.js"></script>
        </as:RadCodeBlock>
        <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxFilterType">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxSearchValueCommonKey" LoadingPanelID="uxInvisiblePanel" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxReportGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                        <tek:AjaxUpdatedControl ControlID="divGrid" LoadingPanelID="None" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="btnLoadList">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="btnLoadList">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxExport" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </as:RadAjaxManagerProxy>
    </as:PlaceHolder>
</asp:Content>
