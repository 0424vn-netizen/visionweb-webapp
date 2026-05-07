<%@ Page Title="Manage Auto Queue"  Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_AutoQueue.aspx.cs" MasterPageFile="~/MasterPage.master" 
    Inherits="rm_MCF_AutoQueue" meta:resourcekey="pageTitle" %>

<%@ Register TagName="AutoQueueDetail" Src="~/UserControls/rm_MCF_AutoQueueDetail.ascx" TagPrefix="uc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <h2>
        <as:Literal ID="Literal3" runat="server" meta:resourcekey="lblManageAutoQueue"></as:Literal>
    </h2>
    <div class="grid-action-container">
        <asp:Button runat="server" ID="btnAddnew"   meta:resourcekey="btnCreateNewAutoQueue" UseSubmitBehavior="False" 
            CssClass="btn btn-default" OnClientClick="return  aqModule.OpenAutoQueueModal('rm_MCF_CreateEditAutoQueueModal.aspx',true)"/>
        <asp:Button runat="server" ID="btnRedistribute" meta:resourcekey="btnRedistribute" UseSubmitBehavior="False"
            CssClass="btn btn-default" OnClientClick="return aqModule.OpenAutoQueueModal('rm_MCF_RedistributeWorkQueueModal.aspx')"/>
        <asp:Button runat="server" ID="btnRedistributeStatus" meta:resourcekey="btnRedistributeStatus" UseSubmitBehavior="False"
            CssClass="btn btn-default" OnClientClick="return aqModule.OpenAutoQueueModal('rm_MCF_WorkQueueRedistributionStatus.aspx')"/>
    </div>
    <as:PlaceHolder runat="server">
        <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="ActiveAQGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="ActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                        <tek:AjaxUpdatedControl ControlID="InActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                        <tek:AjaxUpdatedControl ControlID="aqDetail" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="InActiveAQGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="InActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                        <tek:AjaxUpdatedControl ControlID="ActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                        <tek:AjaxUpdatedControl ControlID="aqDetail" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="btnDoReloadGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="ActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                 <tek:AjaxSetting AjaxControlID="btnDoReloadData">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="ActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                        <tek:AjaxUpdatedControl ControlID="InActiveAQGrid" LoadingPanelID="uxLoadingPanelCustom" />
                        <tek:AjaxUpdatedControl ControlID="aqDetail" LoadingPanelID="uxLoadingPanelCustom" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </tek:RadAjaxManagerProxy>
        <div class="row col-padding-15">
            <div class="col-xs-12">
                <h4 class="mt-9x title-auto-queue">
                    <as:Literal ID="lblActive1" runat="server" meta:resourcekey="lblActive"></as:Literal>
                </h4>
            </div>
            <div class="col-md-6">
                <as:PlaceHolder ID="aqList" runat="server">
                    <as:ASGrid runat="server" ID="ActiveAQGrid" Width="100%" OnRowDrop="OnRowDrop" OnNeedDataSource="ActiveAQGrid_OnNeedDataSource"
                        OnItemCommand="ActiveAQGrid_OnItemCommand" OnItemDataBound="ActiveAQGrid_OnItemDataBound" AllowSorting="False" meta:resourcekey="asAQGrid"
                        AutoGenerateColumns="False" CssClass="grid-style-row">
                        <ClientSettings AllowRowsDragDrop="True" AllowColumnsReorder="False" ReorderColumnsOnClient="False" EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" EnableDragToSelectRows="True"></Selecting>
                            <ClientEvents OnRowClick="aqModule.onActiveRowClick"></ClientEvents>
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <as:ASGridBoundColumn DataField="ProcessingOrder" UniqueName="ProcessingOrder" Display="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="0px"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn DataField="AutoQueueID" UniqueName="AutoQueueID" Display="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="0px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="IsActived" UniqueName="IsActived" Display="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="0px"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridTemplateColumn UniqueName="ProcessingOrder" ItemStyle-CssClass="border-left" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                    <HeaderStyle HorizontalAlign="Center"  Width="50px"></HeaderStyle>
                                    <ItemTemplate>
                                        <p><%#Eval("ProcessingOrder")%>.</p>
                                    </ItemTemplate>
                                </as:ASGridTemplateColumn>

                                <as:ASGridTemplateColumn UniqueName="AutoQueueName" 
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                                    <ItemTemplate>
                                        <div><%#Eval("AutoQueueName")%></div>
                                        <div>
                                            <span class="text-muted">
                                                <as:Literal ID="Literal1" runat="server" meta:resourcekey="lblLastRun"></as:Literal></span>
                                            <%#Eval("lastRunText")%>
                                        </div>
                                        <span>
                                            <span class="text-muted">
                                                <as:Literal ID="Literal2" runat="server" meta:resourcekey="lblCreateOn"></as:Literal></span>
                                            <%# Rm_AutoQueueBusiness.ConvertDateTimeAQ(Eval("CreatedDTS").ToString())%>
                                        </span>
                                    </ItemTemplate>
                                </as:ASGridTemplateColumn>

                                <as:ASGridBoundColumn DataField="TotalOfAssignment" UniqueName="TotalOfAssignment" HeaderTooltip="Assignment"
                                    HeaderText="Assignments" ItemStyle-HorizontalAlign="Center" meta:resourcekey="asGridboundColumnAssignment" AllowSorting="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridBoundColumn DataField="TotalOfWorkQueue" UniqueName="TotalOfWorkQueue" HeaderTooltip="Work Queue"
                                    HeaderText="Work Queues" ItemStyle-HorizontalAlign="Center" meta:resourcekey="asGridboundColumnWorkQueue" AllowSorting="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <%--44078 - VW - Paysafe - Assignment Processing Status = 'Completed' includes AQ allocation to WQ--%>
                               <%-- <as:ASGridTemplateColumn DataField="IsIncludedAggregateQueue" UniqueName="IsIncludedAggregateQueue" HeaderTooltip="Included Aggregate Queue"
                                    HeaderText="Aggregate Queue" ItemStyle-HorizontalAlign="Center" meta:resourcekey="asGridboundColumnAggregateQueue" ItemStyle-CssClass="border-right">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("IsIncludedAggregateQueue")%>'></asp:Label>
                                        <div class="text-center">
                                            <span class="<%#Eval("ClassUI")%>"></span>
                                        </div>
                                    </ItemTemplate>
                                </as:ASGridTemplateColumn>--%>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>

                    <h4 class="mt-9x title-auto-queue">
                        <as:Literal ID="Literal1" runat="server" meta:resourcekey="lblInactive"></as:Literal>
                    </h4>

                    <as:ASGrid runat="server" ID="InActiveAQGrid" Width="100%" OnNeedDataSource="InActiveAQGrid_OnNeedDataSource"
                        OnItemCommand="InActiveAQGrid_OnItemCommand" CssClass="grid-style-row text-muted"
                        AutoGenerateColumns="False" OnItemDataBound="InActiveAQGrid_OnItemDataBound" OnRowDrop="InActiveAQGrid_OnRowDrop" meta:resourcekey="asAQGrid">
                        <ClientSettings AllowRowsDragDrop="True" AllowColumnsReorder="False" ReorderColumnsOnClient="False" EnablePostBackOnRowClick="True">
                            <Selecting AllowRowSelect="True" EnableDragToSelectRows="True"></Selecting>
                            <ClientEvents OnRowClick="aqModule.onInActiveRowClick"></ClientEvents>
                        </ClientSettings>
                        <MasterTableView>
                            <Columns>
                                <as:ASGridBoundColumn DataField="ProcessingOrder" UniqueName="ProcessingOrder" Display="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="0px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="AutoQueueID" UniqueName="AutoQueueID" Display="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="0px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="IsActived" UniqueName="IsActived" Display="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="0px"></HeaderStyle>
                                </as:ASGridBoundColumn>

                                <as:ASGridTemplateColumn UniqueName="ProcessingOrder" ItemStyle-CssClass="border-left" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Top">
                                    <HeaderStyle HorizontalAlign="Left" Width="50px"></HeaderStyle>
                                    <ItemTemplate>
                                        <p><%#Eval("ProcessingOrder")%>.</p>
                                    </ItemTemplate>
                                </as:ASGridTemplateColumn>

                                <as:ASGridTemplateColumn UniqueName="AutoQueueName" 
                                    ItemStyle-HorizontalAlign="Left">
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <ItemTemplate>
                                        <div>
                                            <div><%#Eval("AutoQueueName")%></div>
                                            <div>
                                                <span>
                                                    <as:Literal ID="Literal1" runat="server" meta:resourcekey="lblLastRun"></as:Literal>
                                                </span>
                                                <span><%#Eval("lastRunText")%></span>
                                            </div>
                                            <div>
                                                <span>
                                                    <as:Literal ID="Literal2" runat="server" meta:resourcekey="lblCreateOn"></as:Literal>
                                                </span>
                                                <span><%# Rm_AutoQueueBusiness.ConvertDateTimeAQ(Eval("CreatedDTS").ToString())%></span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </as:ASGridTemplateColumn>

                                <as:ASGridBoundColumn DataField="TotalOfAssignment" UniqueName="TotalOfAssignment" HeaderTooltip="Assignment"
                                    HeaderText="Assignments" ItemStyle-HorizontalAlign="Center" meta:resourcekey="asGridboundColumnAssignment"  AllowSorting="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn DataField="TotalOfWorkQueue" UniqueName="TotalOfWorkQueue" HeaderTooltip="Work Queue"
                                    HeaderText="Work Queues" ItemStyle-HorizontalAlign="Center" meta:resourcekey="asGridboundColumnWorkQueue"  AllowSorting="False">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <%--44078 - VW - Paysafe - Assignment Processing Status = 'Completed' includes AQ allocation to WQ--%>
                                <%--<as:ASGridTemplateColumn DataField="IsIncludedAggregateQueue" ItemStyle-CssClass="border-right" UniqueName="IsIncludedAggregateQueue" HeaderTooltip="Included Aggregate Queue"
                                    HeaderText="Aggregate Queue" ItemStyle-HorizontalAlign="Center" meta:resourcekey="asGridboundColumnAggregateQueue">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("IsIncludedAggregateQueue")%>'></asp:Label>
                                        <div class="text-center">
                                            <span class="<%#Eval("ClassUI")%>"></span>
                                        </div>
                                    </ItemTemplate>
                                </as:ASGridTemplateColumn>--%>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>

                </as:PlaceHolder>
            </div>
            <div class="col-md-6 pt-43">
                <asp:Panel ID="aqDetail" runat="server" CssClass="box-gray">
                    <uc:AutoQueueDetail runat="server" ID="ucDetail" />
                </asp:Panel>
            </div>
        </div>
    </as:PlaceHolder>
    <label id="lblAqId" class="display-none"><%=AutoQueueItem.ID.ToString().DoVeraCode()%></label>
    <as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
        <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AutoQueue.js"></script>
        <script>
            var aqModule = new AutoQueueModule();
            var clientActionGrid = '<%= ActiveAQGrid.ClientID%>';
            var clientInActionGrid = '<%= InActiveAQGrid.ClientID%>';
            var aqActive = '<%= IsActiveSelectAq%>';
            var btnDoReload = '<%= btnDoReloadGrid.ClientID%>'; 
            var btnDoReloadData = '<%= btnDoReloadData.ClientID%>';
            var ucDetail = '<%= aqDetail.ClientID%>';     
            var ucActivedGrid = '<%= ActiveAQGrid.ClientID%>';
            var ucInActivedGrid = '<%= InActiveAQGrid.ClientID%>';
            var MesWarning = '<%= GetLocalResourceObject("textWarning") %>';
        </script>
    </as:ASRadCodeBlock>
    <asp:Button ID="btnDoReloadGrid" runat="server" OnClick="btnDoReloadGrid_OnClick" CssClass="display-none" />
    <asp:Button ID="btnDoReloadData" runat="server" OnClick="btnDoReloadData_OnClick" CssClass="display-none" />
</asp:Content>
