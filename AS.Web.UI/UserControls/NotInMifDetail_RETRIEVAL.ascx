<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotInMifDetail_RETRIEVAL.ascx.cs" Inherits="UserControls_NotInMifDetail_RETRIEVAL" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxNotInMifDetailGridRetrieval">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxNotInMifDetailGridRetrieval" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row">
    <div class="col-xs-10">
        <h2 class="grid-title on-top" data-toggle="collapse" data-target="#notInMifDetailGridRetrieval" aria-expanded="true">
            <as:Literal ID="litGridTitle" runat="server" meta:resourcekey="litGridTitleResource1" />
            <span class="text-muted">
                <as:Literal ID="litGridSubTitle" runat="server" meta:resourcekey="litGridSubTitleResource1" />
            </span>
        </h2>

    </div>
</div>

<div class="row">
    <div class="col-xs-10 as-inline">
        <span class="mr-5x">
            <asp:Label CssClass="text-default-gray" ID="Literal2" runat="server" Text="File Type:" meta:resourcekey="uxFileTypeResource1" />
            <label class="extend-title">
                <asp:Literal ID="uxltFileType" runat="server" meta:resourcekey="MerchantSelectedResource" />
            </label>
        </span>
        <span class="mr-5x">
            <asp:Label CssClass="text-default-gray" ID="Literal1" runat="server" Text="File Source:" meta:resourcekey="uxFileSourceResource1" />
            <label class="extend-title">
                <asp:Literal ID="uxltFileSource" runat="server" meta:resourcekey="MerchantSelectedResource" />
            </label>
        </span>
        <span>
            <asp:Label CssClass="text-default-gray" ID="Literal3" runat="server" Text="Report Date:" meta:resourcekey="uxReportDateResource1" />
            <label class="extend-title">
                <asp:Literal ID="uxltReportDate" runat="server" meta:resourcekey="MerchantSelectedResource" />
            </label>
        </span>
    </div>
    <div class="col-xs-2 mt-1x noinmif-export">
        <uc:UxExport ID="uxExporterRetrieval" runat="server" GridID="uxNotInMifDetailGridRetrieval" ShowWord="false" IsOnTop="true" OnNeedExportConfig="uxExporterRetrieval_NeedExportConfig" />
    </div>
</div>
<div class="height-8"></div>

<div class="in" id="notInMifDetailGridRetrieval">
    <as:ASGrid ID="uxNotInMifDetailGridRetrieval" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" OnItemDataBound="uxNotInMifDetailGrid_ItemDataBound" OnNeedDataSource="uxNotInMifDetailGrid_NeedDataSource"
        CssClass="in" meta:resourcekey="uxNotInMifDetailGridResource1" XOverFlowable="true" HeaderStyle-Width="120px"
        ShowReportTotal="true" IsAutoExportTemplate="true" IsCacheTemplateFile="false">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransDate" UniqueName="TransDate"
                    SortExpression="TransDate" HeaderTooltip="Trans Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Date Received" DataField="DateReceived" UniqueName="DateReceived"
                    SortExpression="DateReceived" HeaderTooltip="Date Received" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Due Date" DataField="DueDate" UniqueName="DueDate" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Due Date" SortExpression="DueDate" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Card Type" DataField="CardType" UniqueName="CardType" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Card Type" SortExpression="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Card #" DataField="CardNumber" UniqueName="CardNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Card #" SortExpression="CardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Card #" DataField="PartialCardNumber" UniqueName="PartialCardNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Card #" SortExpression="PartialCardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource7">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="RoutingAccountNumber" UniqueName="RoutingAccountNumber" HeaderStyle-Width="140px"
                        ASFormat="StaticString" SortExpression="RoutingAccountNumber" Visible="false" meta:resourcekey="RoutingAccountNumber">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text=""></ModelErrorMessage>
                        </ColumnValidationSettings>

                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </as:ASGridBoundColumn>
                <as:ASGridBoundColumn DataField="PartialRoutingACC" UniqueName="PartialRoutingACC" HeaderStyle-Width="140px"
                    ASFormat="StaticString" Visible="false"
                    SortExpression="PartialRoutingACC" meta:resourcekey="RoutingAccountNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Exp Date" DataField="ExpDate" UniqueName="ExpDate" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Exp Date" SortExpression="ExpDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Acquirer Reference #" DataField="ReferenceNumber" UniqueName="ReferenceNumber" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px"
                    HeaderTooltip="Acquirer Reference #" SortExpression="ReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="180px"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="RC #" DataField="RC" UniqueName="RC" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="RC #" SortExpression="RC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>


                <as:ASGridBoundColumn HeaderText="MCOMM Claim ID" HeaderStyle-Width="120px" HeaderTooltip="MCOMMClaimID" DataField="MCOMMClaimID"
                    UniqueName="MCOMMClaimID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceMCOMMClaimID">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="VROLCase #" HeaderStyle-Width="120px" HeaderTooltip="VROL Case Number" DataField="VROLCaseNumber"
                    UniqueName="VROLCaseNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceVROLCaseNumber">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                 
                <as:ASGridBoundColumn HeaderText="Request Type" DataField="RequestType" UniqueName="RequestType" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Request Type" SortExpression="RequestType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Amount" DataField="TransAmount" UniqueName="TransAmount" ASIsTotalColumn="true"
                    HeaderTooltip="Trans Amount" SortExpression="TransAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</div>
