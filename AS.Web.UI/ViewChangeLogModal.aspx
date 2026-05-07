<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewChangeLogModal.aspx.cs" MasterPageFile="~/MasterPagePopup.master" Inherits="ViewChangeLog" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl">
        <h2 class="modal-title">
            <as:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitleGridResource1"></as:Literal>
        </h2>
        <div class="row">
            <div class="col-xs-12">
                <as:ASGrid ID="uxReportGrid" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" AllowSorting="true"
                    CssClass="in" meta:resourcekey="uxMerchantSelectedReportGridResource1" XOverFlowable="true" HeaderStyle-Width="120px">
                    <MasterTableView>
                        <Columns>
                            <as:ASGridBoundColumn HeaderText="Date/Time" DataField="ReportDate" SortExpression="ReportDate" ItemStyle-HorizontalAlign="Center"
                                UniqueName="ReportDate" ASFormat="DateAndTime12Hours" HeaderTooltip="Date/Time" meta:resourcekey="ASGridBoundColumnResource1">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Changed By" DataField="ChangedByUser" SortExpression="ChangedByUser" ItemStyle-HorizontalAlign="Left"
                                UniqueName="ChangedByUser" ASFormat="StaticString" HeaderTooltip="Changed By" meta:resourcekey="ASGridBoundColumnResource2">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Changed Type" DataField="ChangeType" SortExpression="ChangeType" ItemStyle-HorizontalAlign="Left"
                                UniqueName="ChangeType" HeaderTooltip="Changed Type" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text=""></ModelErrorMessage>
                                </ColumnValidationSettings>

                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn HeaderText="Field" DataField="FieldName" SortExpression="FieldName" ItemStyle-HorizontalAlign="Left"
                                UniqueName="FieldName" HeaderTooltip="Field" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
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
                <as:HiddenField runat="server" ID="uxHdUrlViewDetail" />
                <as:Button class="btn btn-default" ID="btnViewDetails" OnClientClick="viewDetailLog();" Text="View Details" runat="server" meta:resourcekey="bntViewDetailsResource" />
                <as:Button class="btn btn-default" ID="btnCancel" Text="Close" OnClientClick="ClosePopupModal();" runat="server" meta:resourcekey="bntCloseResource" />
            </div>
        </div>
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var ViewChangeLogModal_uxHdUrlViewDetail_ClientID = "<%= uxHdUrlViewDetail.ClientID%>";
            function viewDetailLog() {
                parent.window.location = $("#" + ViewChangeLogModal_uxHdUrlViewDetail_ClientID).val();
            }
        </script>
    </tek:RadCodeBlock>
</asp:Content>
