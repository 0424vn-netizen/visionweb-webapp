<%--Create by: Hao Dang
Ticket: 43784 Queue Enhancements --%>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_DQAddToQueueModal.aspx.cs" Inherits="rm_MCF_DQAddToQueueModal" MasterPageFile="~/MasterPagePopup.master" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" Width="900px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-xs-12">
                <asp:Literal ID="lblMerchantSelected" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="height-8"></div>
        <div class="row" runat="server" id="workQueueList">
            <div class="col-xs-2">
                <asp:Literal ID="WorkQueue" Text="Work Queue:" runat="server" meta:resourcekey="WorkQueueResource" />
            </div>
            <div class="col-xs-6">
                <as:RadComboBox ID="uxComboWorkQueueList" runat="server" MaxHeight="260px" EnableEmbeddedSkins="false" Filter="Contains" Width="100%" meta:resourcekey="uxComboWorkQueueListResource1" />
            </div>
        <div class="height-8"></div>
        </div>
        <div class="height-8"></div>
        <div class="row">
            <div class="col-xs-12">
                <as:ASGrid ID="uxMerchantSelected" runat="server" AllowPaging="true" GridLines="None" AllowSorting="True" AllowSortFilterWhenExport="true"
                    OnNeedDataSource="uxMerchantSelected_NeedDataSource" OnItemDataBound="uxMerchantSelected_ItemDataBound"
                     OnPreRender="uxMerchantSelected_PreRender"
                    AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod1" CssClass="in" meta:resourcekey="uxMerchantSelectedReportGridResource1" XOverFlowable="true" HeaderStyle-Width="120px">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Merchant Number" DataField="MerchantNumber" UniqueName="MerchantNumber" ItemStyle-HorizontalAlign="Center"
                                SortExpression="MerchantNumber" HeaderTooltip="Merchant Number" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Merchant Name" DataField="MerchantName" UniqueName="MerchantName" ItemStyle-HorizontalAlign="Left"
                                SortExpression="MerchantName" HeaderTooltip="Entity Name" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Current Work Queue" DataField="CurrentWQ" UniqueName="CurrentWQ" ItemStyle-HorizontalAlign="Center"
                                HeaderTooltip="Current Work Queue" SortExpression="CurrentWQ" ASDefaultNullValue="&mdash;" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>

        </div>
        <div class="row">
            <div class="col-xs-12 form-action-container text-right">
                <as:Button ID="btnRequeue" runat="server" Text="Submit" CssClass="btn btn-default control-inline" OnClick="btnRequeue_Click" meta:resourcekey="btnRequeueResource1" />
                <as:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default control-inline" OnClientClick="ClosePopupModal();" meta:resourcekey="btnCancelResource1" />
            </div>
        </div>
    </as:ASModalContainer>

</asp:Content>
