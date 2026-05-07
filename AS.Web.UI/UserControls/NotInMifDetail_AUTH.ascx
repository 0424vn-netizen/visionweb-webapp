<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NotInMifDetail_AUTH.ascx.cs" Inherits="UserControls_NotInMifDetail_AUTH" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>

<tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxNotInMifDetailGridAuth">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxNotInMifDetailGridAuth" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</tek:RadAjaxManagerProxy>

<div class="row">
    <div class="col-xs-10">
        <h2 class="grid-title on-top" data-toggle="collapse" data-target="#notInMifDetailGridAuth" aria-expanded="true">
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
        <uc:UxExport ID="uxExporterAuth" runat="server" GridID="uxNotInMifDetailGridAuth" ShowWord="false" IsOnTop="true" OnNeedExportConfig="uxExporterAuth_NeedExportConfig" />
    </div>
</div>
<div class="height-8"></div>

<div class="in" id="notInMifDetailGridAuth">
    <as:ASGrid ID="uxNotInMifDetailGridAuth" runat="server" AllowPaging="true" GridLines="None" AutoGenerateColumns="false" ASPagingMethod="SPASingleMethod" OnItemDataBound="uxNotInMifDetailGrid_ItemDataBound"
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
                    HeaderTooltip="Expired Date" SortExpression="ExpDate" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource8">
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
                <as:ASGridBoundColumn HeaderText="Auth Amount" DataField="AuthAmount" UniqueName="AuthAmount" ASIsTotalColumn="true"
                    HeaderTooltip="Auth Amount" SortExpression="AuthAmount" ASFormat="Currency" meta:resourcekey="ASGridBoundColumnResource10">
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
                <as:ASGridBoundColumn HeaderText="A/D" DataField="AD" UniqueName="AD" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="A/D" SortExpression="AD" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource12">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="RC" DataField="RC" UniqueName="RC" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="RC" SortExpression="RC" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource13">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="AVS" DataField="AVS" UniqueName="AVS" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="AVS" SortExpression="AVS" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource14">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="CVV" DataField="CVV" UniqueName="CVV" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="CVV" SortExpression="CVV" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource15">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Auth Source" DataField="AuthSource" UniqueName="AuthSource" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Auth Source" SortExpression="AuthSource" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource16">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="Cust ID" DataField="CustID" UniqueName="CustID" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="Cust ID" SortExpression="CustID" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource17">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
                <as:ASGridBoundColumn HeaderText="MOTO" DataField="MOTO" UniqueName="MOTO" ItemStyle-HorizontalAlign="Center"
                    HeaderTooltip="MOTO" SortExpression="MOTO" ASFormat="StaticString" meta:resourcekey="ASGridBoundColumnResource18">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text=""></ModelErrorMessage>
                    </ColumnValidationSettings>

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </as:ASGridBoundColumn>
            </Columns>
        </MasterTableView>
    </as:ASGrid>
</div>
