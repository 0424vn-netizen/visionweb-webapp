<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="Message_ViewUnreadMessagesModal.aspx.cs" Inherits="Message_ViewUnreadMessagesModal"
    Title="Messages" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <h2 class="modal-title">
            <as:Literal ID="ltMessages" runat="server" Text="Messages" meta:resourcekey="ltMessagesResource1"></as:Literal>
        </h2>
        <div class="height-10"></div>
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <i class="text-left">
                        <as:Literal ID="ltSomethingNew" runat="server" Text='You have the following unread messages. Please check the box in
                        the "Read" column
                    once you have reviewed these messages.'
                            meta:resourcekey="ltSomethingNewResource1"></as:Literal>
                        <br />
                        <as:Literal ID="ltSomethingNewer" runat="server" Text="All past messages can be reviewed at any time on the Messages tab of the site. If
                        you would like to view all past messages now, click the View Past Messages button
                        below."
                            meta:resourcekey="ltSomethingNewerResource1"></as:Literal>
                    </i>
                </div>
            </div>
        </div>
        <div class="height-22"></div>
        <uc:UxExport ID="uxExporterTop" runat="server" GridID="uxReportGrid" IsOnTop="true"
            ShowWord="false" OnNeedExportConfig="uxExport_OnNeedExportConfig" GridTitle="New Messages" meta:resourcekey="uxExporterTopResource1" />
        <as:ASGrid ID="uxReportGrid" runat="server" ASPagingMethod="SPASingleMethod" GridName="New Messages"
            AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" AllowSortFilterWhenExport="true" CssClass="in" meta:resourcekey="uxReportGridResource1">
            <MasterTableView>
                <Columns>
                    <as:ASGridTemplateColumn HeaderText="Read" HeaderTooltip="Read" DataField="MessageID"
                        UniqueName="MessageID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource1">
                        <ItemTemplate>
                            <input type="checkbox" onclick='doReadMessage(<%# Eval("MessageID") %>)' />
                        </ItemTemplate>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridTemplateColumn>
                    <as:ASGridBoundColumn UniqueName="DateTime" HeaderText="Date/Time" DataField="Date/Time"
                        ASFormat="DateAndTime12Hours" HeaderTooltip="Date/Time" SortExpression="Date/Time"
                        HeaderStyle-Width="150px" meta:resourcekey="ASGridBoundColumnResource1">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center" Width="150px"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="PostedBy" HeaderText="Posted By" DataField="PostedBy"
                        ASFormat="StaticString" HeaderTooltip="Posted By" SortExpression="PostedBy" meta:resourcekey="ASGridBoundColumnResource2">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn UniqueName="Comment" HeaderText="Message Text" DataField="Comment"
                        HeaderTooltip="Message Text" SortExpression="Comment" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource3">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <div class="row">
            <div class="col-xs-6 form-action-container">
                <div class="text-left">
                    <as:Button runat="server" Text="View Past Messages" ID="uxReviewPastMessages" CssClass="btn btn-default"
                        OnClick="uxReviewPastMessages_Click" meta:resourcekey="uxReviewPastMessagesResource1" />
                </div>
            </div>
            <div class="col-xs-6 form-action-container text-right">
                <as:Button runat="server" Text="Close" ID="uxClose" CssClass="btn btn-default" OnClientClick="parent.HidePopupModal();" meta:resourcekey="uxCloseResource1" />
            </div>
        </div>
        <as:Button ID="uxReadMesage" runat="server" Text="Button" CssClass="display-none"
            IsStandardButton="true" CausesValidation="false" OnClick="uxReadMesage_Click" meta:resourcekey="uxReadMesageResource1" />
        <as:HiddenField ID="uxMessageID" runat="server" />
    </as:ASModalContainer>
    <tek:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript">
            var uxReadMesage_ClientID = '<%=uxReadMesage.ClientID %>';
            var uxMessageID_ClientID = '<%=uxMessageID.ClientID %>';            
        </script>
        <script src="<%=ResolveUrl("~")%>res/js/Message_ViewUnreadMessagesModal.js"></script>
    </tek:RadCodeBlock>
    <as:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxReportGrid">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxReadMesage">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxReportGrid" />
                    <tek:AjaxUpdatedControl ControlID="uxExporterTop" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </as:RadAjaxManagerProxy>
</asp:Content>
