<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotInMifDetail_CHARGEBACK.ascx.cs" Inherits="UserControls_NotInMifDetail_CHARGEBACK" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxNotInMifDetailGridChargeback">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxNotInMifDetailGridChargeback" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>

<div class="row">
    <div class="col-xs-10">
        <h2 class="grid-title on-top" data-toggle="collapse" data-target="#notInMifDetailGridChargeback" aria-expanded="true">
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
        <uc:UxExport ID="uxExporterChargeback" runat="server" GridID="uxNotInMifDetailGridChargeback" ShowWord="false" IsOnTop="true" OnNeedExportConfig="uxExporterChargeback_NeedExportConfig" />
    </div>
</div>
<div class="height-8"></div>

<div class="in" id="notInMifDetailGridChargeback">
    <as:ASGrid ID="uxNotInMifDetailGridChargeback" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" OnItemDataBound="uxNotInMifDetailGrid_ItemDataBound" OnNeedDataSource="uxNotInMifDetailGrid_NeedDataSource"
        CssClass="in" meta:resourcekey="uxNotInMifDetailGridResource1" XOverFlowable="true" HeaderStyle-Width="120px"
        ShowReportTotal="true" IsAutoExportTemplate="true" IsCacheTemplateFile="false">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransDate" UniqueName="TransDate" ItemStyle-HorizontalAlign="Center"
                    SortExpression="TransDate" HeaderTooltip="Trans Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" />
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Type" DataField="CardType" UniqueName="CardType" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Type" SortExpression="CardType" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource6">
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
                    HeaderTooltip="Card #" SortExpression="PartialCardNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource710">
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
                    HeaderTooltip="Exp Date" SortExpression="ExpDate" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource8">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="CB Type" DataField="CBType" UniqueName="CBType" ItemStyle-HorizontalAlign="Center"
                    SortExpression="CBType" HeaderTooltip="CB Type" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="CB Type Description" DataField="CBTypeDescription" UniqueName="CBTypeDescription" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="CB Type Description" SortExpression="CBTypeDescription" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="RC" DataField="RC" UniqueName="RC" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Reason Code" SortExpression="RC" ASFormat="DynamicString" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Reson Text" DataField="ReasonText" UniqueName="ReasonText" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Reson Text" SortExpression="ReasonText" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Disposition" DataField="Disposition" UniqueName="Disposition" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Disposition" SortExpression="Disposition" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Acquirer Reference #" DataField="ReferenceNumber" UniqueName="ReferenceNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Acquirer Reference number" SortExpression="ReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center" Width="180px"></HeaderStyle>
                </as:ASGridBoundColumn>
                 
                <as:ASGridBoundColumn HeaderText="CB Reference #" HeaderTooltip="CB Reference Number" HeaderStyle-Width="185px" DataField="ChargebackReferenceNumber"
                    UniqueName="ChargebackReferenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResourceCBReferenceNumber">
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

                <as:ASGridBoundColumn HeaderText="CB Sequence Number" DataField="CBSequenceNumber" UniqueName="CBSequenceNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="CB Sequence Number" SortExpression="CBSequenceNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="CB Amount" DataField="CBAmount" UniqueName="CBAmount" ASIsTotalColumn="true" ItemStyle-HorizontalAlign="Right"
                    HeaderTooltip="CB Amount" SortExpression="CBAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource11">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="1st CB Amount" DataField="FirstCBAmount" UniqueName="FirstCBAmount" ASIsTotalColumn="true"
                    HeaderTooltip="1st CB Amount" SortExpression="FirstCBAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</div>
