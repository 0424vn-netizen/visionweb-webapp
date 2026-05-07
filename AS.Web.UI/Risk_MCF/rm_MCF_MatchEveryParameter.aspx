<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="rm_MCF_MatchEveryParameter.aspx.cs" Inherits="As.VisionWeb.Web.RiskManagementMatchEveryParameter" meta:resourcekey="PageResource1" %>

<asp:Content ContentPlaceHolderID="ContentPage" runat="server" ID="uxMainModalContent">
    <as:ASModalContainer ID="uxModalContainer" runat="server" ContainerCssClass="container" WidthCssClass="modal-xl">
        <div class="row">
            <div class="col-md-12">
                <h3 class="modal-title title-auto-queue font-35">
                    <as:Literal ID="lblAssignmentName" runat="server" Text="Text"></as:Literal>
                </h3>
            </div>
        </div>
        <div class="height-10"></div>
        <as:ASGrid ID="uxReportGrid" runat="server" AutoGenerateColumns="false" ASPagingMethod="None"
            AllowExportAtWebServices="false" AllowPaging="false" AllowSorting="true" CssClass="in" IsCacheTemplateFile="false"
            meta:resourcekey="uxReportGridResource1" Width="100%" IsAutoExportTemplate="true" OnItemDataBound="uxReportGrid_ItemDataBound">
            <MasterTableView>
                <Columns>
                    <as:ASGridBoundColumn HeaderText="P#" DataField="ParameterKey" UniqueName="ParameterKey"  HeaderStyle-Width="50px"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="false"
                        SortExpression="ParameterName" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource01">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Parameter Name" DataField="Parameter" UniqueName="Parameter" HeaderStyle-Width="200px"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="false"
                        SortExpression="ParameterName" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource02">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Re-Alert" DataField="ReAlert"
                        SortExpression="ReAlert" UniqueName="ReAlert" HeaderStyle-HorizontalAlign="Center" ASFormat="DynamicString"
                        ItemStyle-HorizontalAlign="Right" ASDefaultNullValue="N/A" AllowSorting="false"
                        EmptyDataText="N/A" meta:resourcekey="ASGridBoundColumnResource09">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </as:ASGridBoundColumn>

                    <as:ASGridBoundColumn HeaderText="Parameter Indicator" DataField="ParameterValue"
                        SortExpression="ParameterValue" UniqueName="ParameterValue" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" ASDefaultNullValue="N/A" AllowSorting="false"
                        EmptyDataText="N/A" meta:resourcekey="ASGridBoundColumnResource03">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Actual Indicator" DataField="ActualValue" SortExpression="ActualValue"
                        AllowSorting="false" HeaderStyle-HorizontalAlign="Center" UniqueName="ActualValue"
                        ItemStyle-HorizontalAlign="Right" EmptyDataText="N/A" meta:resourcekey="ASGridBoundColumnResource04">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Parameter Threshold" DataField="ParameterThreshold"
                        AllowSorting="false" SortExpression="ParameterThreshold" UniqueName="ParameterThreshold"
                        HeaderStyle-HorizontalAlign="Center" EmptyDataText="N/A"
                        ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource05">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Actual Threshold" DataField="ActualThreshold"
                        AllowSorting="false" SortExpression="ActualThreshold" UniqueName="ActualThreshold"
                        HeaderStyle-HorizontalAlign="Center" EmptyDataText="N/A"
                        ItemStyle-HorizontalAlign="Right" meta:resourcekey="ASGridBoundColumnResource06">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="Violated On" DataField="ViolatedOn" UniqueName="ViolatedOn"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" AllowSorting="true"
                        SortExpression="ViolatedOn" ASFormat="DateAndTime12Hours" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource07">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Center" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                    <as:ASGridBoundColumn HeaderText="File Type" DataField="FileTypeCodes" UniqueName="FileTypeCodes"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" AllowSorting="false"
                        SortExpression="FileTypeCodes" ASFormat="StaticString" ItemStyle-Wrap="true" meta:resourcekey="ASGridBoundColumnResource08">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                        <ItemStyle HorizontalAlign="Left" Wrap="True"></ItemStyle>
                    </as:ASGridBoundColumn>
                </Columns>
            </MasterTableView>
        </as:ASGrid>
        <div class="row">
            <div class="col-xs-12 text-right form-action-container">
                <as:Button runat="server" meta:resourcekey="uxCloseResource" class="btn btn-default" ID="uxClose" Text="CLOSE" OnClientClick="parent.ClosePopupModal(1);" IsStandardButton="False" />
            </div>
        </div>
    </as:ASModalContainer>
    
    <script>
        function showParameterViolationDetailsModal(encodeURL) {
            if (encodeURL) {
                parent.ShowPopupModalChild(2, encodeURL, 'auto');
            }
        }
    </script>
</asp:Content>
