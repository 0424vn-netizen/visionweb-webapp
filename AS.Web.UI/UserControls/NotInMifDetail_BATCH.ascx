<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotInMifDetail_BATCH.ascx.cs" Inherits="UserControls_NotInMifDetail_BATCH" %>

<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxNotInMifDetailGridBatch">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxNotInMifDetailGridBatch" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>
<div class="row">
    <div class="col-xs-10">
        <h2 class="grid-title on-top" data-toggle="collapse" data-target="#notInMifDetailGridBatch" aria-expanded="true">
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
    <div class="col-xs-2 mt-1x f-right noinmif-export">
        <uc:UxExport ID="uxExporterBath" runat="server" GridID="uxNotInMifDetailGridBatch" ShowWord="false" IsOnTop="true" OnNeedExportConfig="uxExporterBath_NeedExportConfig" />
    </div>
</div>
<div class="height-8"></div>

<div id="notInMifDetailGridBatch" class="in">
    <as:ASGrid ID="uxNotInMifDetailGridBatch" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" OnItemDataBound="uxNotInMifDetailGrid_ItemDataBound"
        OnNeedDataSource="uxNotInMifDetailGrid_NeedDataSource" CssClass="in" meta:resourcekey="uxNotInMifDetailGridResource1" 
        XOverFlowable="true" HeaderStyle-Width="120px" ShowReportTotal="true" IsAutoExportTemplate="true" IsCacheTemplateFile="false">
        <MasterTableView>
            <Columns>
                <as:ASGridBoundColumn HeaderText="Trans Date" DataField="TransDate" UniqueName="TransDate"
                    SortExpression="TransDate" HeaderTooltip="Trans Date" ASFormat="Date" meta:resourcekey="ASGridBoundColumnResource1">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Time" DataField="TransTime" UniqueName="TransTime" ItemStyle-HorizontalAlign="Center"
                    SortExpression="TransTime" HeaderTooltip="Trans Time" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource2">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Trans Code" DataField="TransCodeDescription" UniqueName="TransCodeDescription" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Trans Code" SortExpression="TransCodeDescription" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource3">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Terminal #" DataField="TerminalNumber" UniqueName="TerminalNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Terminal #" SortExpression="TerminalNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource19">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Batch #" DataField="BatchNumber" UniqueName="BatchNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Batch #" SortExpression="BatchNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource20">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>


                <as:ASGridBoundColumn HeaderText="Keyed" DataField="Keyed" UniqueName="Keyed" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Keyed" SortExpression="Keyed" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource4">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="EMV" DataField="EMV" UniqueName="EMV" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="EMV" SortExpression="EMV" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource5">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>

                <as:ASGridBoundColumn HeaderText="Type" DataField="CardType" UniqueName="CardType" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Type" SortExpression="CardType" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource6">
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
                <as:ASGridBoundColumn HeaderText="Auth #" DataField="AuthNumber" UniqueName="AuthNumber" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Auth #" SortExpression="AuthNumber" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource9">
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
