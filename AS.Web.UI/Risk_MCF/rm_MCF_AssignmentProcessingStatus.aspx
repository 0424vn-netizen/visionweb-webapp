<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="rm_MCF_AssignmentProcessingStatus.aspx.cs" Inherits="rm_MCF_AssignmentProcessingStatus"
    Title="ASSIGNMENT PROCESSING STATUS" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
        <as:RadAjaxManagerProxy ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <tek:AjaxSetting AjaxControlID="uxGrid">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxSubmit">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
                <tek:AjaxSetting AjaxControlID="uxSubmitAjax">
                    <UpdatedControls>
                        <tek:AjaxUpdatedControl ControlID="uxGrid" />
                    </UpdatedControls>
                </tek:AjaxSetting>
            </AjaxSettings>
        </as:RadAjaxManagerProxy>

        <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xl">
            <div class="row">
                <div class="col-xs-6">
                    <h3 class="modal-title">
                        <asp:Literal ID="rm_AssignmentProcessingStatus_aspxText1" runat="server" meta:resourcekey="rm_AssignmentProcessingStatus_aspxText1Resource1"> Assignment Processing Status</asp:Literal></h3>
                </div>
                <div class="col-xs-6 text-muted text-right">
                    <asp:Literal ID="rm_AssignmentProcessingStatus_aspxText2" runat="server" meta:resourcekey="rm_AssignmentProcessingStatus_aspxText2Resource1"> Last Queue Processing Date:</asp:Literal>
                    <div class="control-inline last">
                        <asp:Label ID="uxLastJobQueueDate" runat="server" Text="N/A" meta:resourcekey="uxLastJobQueueDateResource1" />
                    </div>
                </div>
            </div>
            <div class="row radio-button-list dark-blue">
                <div class="height-8"></div>
                <div class="col-md-12">
                    <div class="control-inline">
                        <label class="first">
                            <asp:Literal ID="rm_AssignmentProcessingStatus_aspxText3" runat="server" meta:resourcekey="rm_AssignmentProcessingStatus_aspxText3Resource1">Status:</asp:Literal></label>

                    </div>
                    <div class="control-inline">
                        <asp:CheckBox ID="chbxQueued" runat="server" Text="Queued" AutoPostBack="false" Checked="true" meta:resourcekey="chbxQueuedResource1" />
                    </div>
                    <div class="control-inline">
                        <asp:CheckBox ID="chbxInProgress" runat="server" Text="In Progress" AutoPostBack="false"
                            Checked="true" meta:resourcekey="chbxInProgressResource1" />
                    </div>
                    <div class="control-inline">
                        <asp:CheckBox ID="chbxCompleted" runat="server" Text="Completed" AutoPostBack="false" meta:resourcekey="chbxCompletedResource1" />
                    </div>
                    <div class="control-inline">
                        <asp:CheckBox ID="chbxCancelled" runat="server" Text="Cancelled" AutoPostBack="false" meta:resourcekey="chbxCancelledResource1" />
                    </div>
                    <div class="control-inline">
                        <asp:CheckBox ID="chbxError" runat="server" Text="Error" AutoPostBack="false" Checked="true" meta:resourcekey="chbxErrorResource1" />
                    </div>
                    <div class="control-inline valign-top last">
                        <asp:Button ID="uxSubmit" runat="server" Text="Submit" CssClass="btn btn-default"
                            OnClick="uxSubmit_Click" meta:resourcekey="uxSubmitResource1" />
                    </div>
                </div>
                <div class="height-14"></div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <as:ASGrid ID="uxGrid" runat="server" AutoGenerateColumns="False" PageSize="10"
                        AllowSorting="True" AllowPaging="true" ASPagingMethod="None" GridLines="None" CssClass="in" meta:resourcekey="uxGridResource1">
                        <MasterTableView>
                            <Columns>
                                <as:ASGridBoundColumn HeaderText="ID" DataField="AssignmentID" UniqueName="AssignmentID"  HeaderStyle-Width="60px"
                                    HeaderTooltip="Assignment ID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource1">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn HeaderText="Assignment Name" DataField="AssignmentName" UniqueName="AssignmentName" HeaderStyle-Width="250px"
                                    ASFormat="DynamicString" HeaderTooltip="Assignment Name" SortExpression="AssignmentName" meta:resourcekey="ASGridBoundColumnResource2">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn HeaderText="Type" DataField="AssignmentType" UniqueName="AssignmentType"
                                    ASFormat="StaticString" HeaderTooltip="Assignment Type" meta:resourcekey="ASGridBoundColumnResource3">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn HeaderText="Proc. Status" DataField="ProcessingStatus" UniqueName="ProcessingStatus"
                                    ASFormat="StaticString" HeaderTooltip="Processing Status" SortExpression="ProcessingStatus" meta:resourcekey="ASGridBoundColumnResource4">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                </as:ASGridBoundColumn>
                                <as:ASGridBoundColumn HeaderText="Last Proc. Status Date" DataField="ProcessingStatusDate"
                                    UniqueName="ProcessingStatusDate" HeaderTooltip="Last processing status date"
                                    SortExpression="ProcessingStatusDate" ASFormat="DateAndTime12Hours" meta:resourcekey="ASGridBoundColumnResource5">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text=""></ModelErrorMessage>
                                    </ColumnValidationSettings>

                                    <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                                </as:ASGridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </as:ASGrid>
                </div>
            </div>

            <div class="display-none">
                <as:Button ID="uxSubmitAjax" runat="server" Text="Submit" OnClick="uxSubmitAjax_Click" meta:resourcekey="uxSubmitAjaxResource1" />
            </div>

        </as:ASModalContainer>

        <as:RadCodeBlock ID="JavaScript" runat="server">
            <script type="text/javascript">
                var rm_AssignmentProcessingStatus_uxSubmitAjax = '<%= uxSubmitAjax.ClientID %>';
            </script>
            <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_AssignmentProcessingStatus.js"></script>
        </as:RadCodeBlock>
</asp:Content>

