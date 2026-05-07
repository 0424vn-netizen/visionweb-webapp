<%@ Page Title="STATEMENT DETAILS" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="StatementDetail_IHP.aspx.cs" Inherits="StatementDetail_IHP" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:Literal ID="uxHeader" Visible="False" Text="No data to display" runat="server" meta:resourcekey="uxHeaderResource1"></as:Literal>
    <div style="width: 99%; margin: auto;">
        <div class="PageTitleArea">
            <div style="float: left;" class="PageTitle">
                <img alt="logo" src="App_Themes/MPS/images/logo.jpg" /><br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="uxClientAddress1" runat="server" Style='float: left; text-align: left; font-weight: normal'
                                meta:resourcekey="uxClientAddress1Resource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="uxClientAddress2" runat="server" Style='float: left; text-align: left; font-weight: normal'
                                meta:resourcekey="uxClientAddress2Resource1"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: left;" class="PageTitle" id="Logo" runat="server" visible="false">
                <img alt="logo" src="App_Themes/MPS/images/logo.jpg" runat="server" id="uxImageLogo" /><br />
                <table style='font-size: 10px'>
                    <tr>
                        <td>
                            <asp:Label ID="uxClientAddress1_PDF" runat="server" Style='float: left; text-align: left; font-weight: normal'
                                meta:resourcekey="uxClientAddress1_PDFResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="uxClientAddress2_PDF" runat="server" Style='float: left; text-align: left; font-weight: normal'
                                meta:resourcekey="uxClientAddress2_PDFResource1"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="float: right;" runat="server" id="uxHeading">
                <table>
                    <tr>
                        <td valign="middle">
                            <asp:ImageButton ID="uxExportPDF" runat="server" ImageUrl="res/images/pdf.gif" OnClick="uxExportPDF_Click"
                                Width="24px" Height="24px" ToolTip="Export to PDF" meta:resourcekey="uxExportPDFResource1" /></td>
                        <td valign="middle">
                            <input onclick="PrintPage();" class="FormButtonPrinter" type="button" value="" /></td>
                    </tr>
                </table>

            </div>
            <br style="clear: both" />
        </div>
        <br style="clear: both;" />
        <as:PlaceHolder runat="server" ID="uxExportPanel">
            <div style="text-align: left;">
                <asp:Repeater ID="uxMerchantInfo" runat="server">
                    <ItemTemplate>
                        <div style="float: left; text-align: left; margin-left: 2px;">
                            <%# Eval("DeliveryAddressLine1")%><br />
                            <%# Eval("DeliveryAddressLine2")%><br />
                            <%# Eval("DeliveryAddressLine3")%><br />
                            <%# Eval("DeliveryAddressLine4") == string.Empty ? "" : Eval("DeliveryAddressLine4") + "<br />"%>
                            <%# Eval("DeliveryAddressLine5")%><br />
                        </div>
                        <div style="float: right; text-align: left; margin-right: 20px; margin-top: -60px">
                            <table>
                                <tr>
                                    <td>
                                        <as:Literal ID="ltMERCHANTID" runat="server" Text="MERCHANT ID:" meta:resourcekey="ltMERCHANTIDResource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("MerchantID")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal1" runat="server" Text="MERCHANT STATEMENT PERIOD ENDING:" meta:resourcekey="Literal1Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Convert.ToDateTime(Eval("MonthEndDate")).ToShortDateString()%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal2" runat="server" Text="DBA:" meta:resourcekey="Literal2Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("DBA")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <u>
                                            <as:Literal ID="Literal3" runat="server" Text="HOW TO REACH US:" meta:resourcekey="Literal3Resource1"></as:Literal></u>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal4" runat="server" Text="SALES SUPPORT EMAIL:" meta:resourcekey="Literal4Resource1"></as:Literal>
                                    </td>
                                    <td align="right" style="text-transform: uppercase;"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal5" runat="server" Text="CUSTOMER SUPPORT EMAIL:" meta:resourcekey="Literal5Resource1"></as:Literal>
                                    </td>
                                    <td align="right" style="text-transform: uppercase;">
                                        <as:Literal ID="Literal6" runat="server" Text="ics@mercurypay.com" meta:resourcekey="Literal6Resource1"></as:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal7" runat="server" Text="CUSTOMER SUPPORT PHONE:" meta:resourcekey="Literal7Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("CustomerSupportPhone")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="uxMerchantInfor_PDF" runat="server" Visible="false">
                    <ItemTemplate>
                        <div style="float: left; text-align: left; margin-left: 2px;">
                            <%# Eval("DeliveryAddressLine1")%><br />
                            <%# Eval("DeliveryAddressLine2")%><br />
                            <%# Eval("DeliveryAddressLine3")%><br />
                            <%# Eval("DeliveryAddressLine4") == string.Empty ? "" : Eval("DeliveryAddressLine4") + "<br />"%>
                            <%# Eval("DeliveryAddressLine5")%><br />
                        </div>
                        <div style="float: right; text-align: left; margin-top: -60px">
                            <table style='font-size: 10px'>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal7" runat="server" Text="MERCHANT ID:" meta:resourcekey="Literal7Resource2"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("MerchantID")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal8" runat="server" Text="MERCHANT STATEMENT PERIOD ENDING:" meta:resourcekey="Literal8Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Convert.ToDateTime(Eval("MonthEndDate")).ToShortDateString()%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal9" runat="server" Text="DBA:" meta:resourcekey="Literal9Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("DBA")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <u>
                                            <as:Literal ID="Literal10" runat="server" Text="HOW TO REACH US:" meta:resourcekey="Literal10Resource1"></as:Literal></u>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal11" runat="server" Text="SALES SUPPORT EMAIL:" meta:resourcekey="Literal11Resource1"></as:Literal>
                                    </td>
                                    <td align="right" style="text-transform: uppercase;">
                                        <as:Literal ID="Literal12" runat="server" Text="Salessupport@mercurypay.com" meta:resourcekey="Literal12Resource1"></as:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal13" runat="server" Text="CUSTOMER SUPPORT EMAIL:" meta:resourcekey="Literal13Resource1"></as:Literal>
                                    </td>
                                    <td align="right" style="text-transform: uppercase;">
                                        <as:Literal ID="Literal14" runat="server" Text="ics@mercurypay.com" meta:resourcekey="Literal14Resource1"></as:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal15" runat="server" Text="CUSTOMER SUPPORT PHONE:" meta:resourcekey="Literal15Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("CustomerSupportPhone")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Repeater ID="uxMerchantInfo_Header" runat="server" Visible="false">
                    <ItemTemplate>
                        <div style="float: left; text-align: left; margin-left: 2px;">
                            <table style='font-size: 10px'>
                                <tr>
                                    <td>
                                        <u>
                                            <as:Literal ID="Literal15" runat="server" Text="HOW TO REACH US:" meta:resourcekey="Literal15Resource2"></as:Literal></u>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal16" runat="server" Text="SALES SUPPORT EMAIL:" meta:resourcekey="Literal16Resource1"></as:Literal>
                                    </td>
                                    <td align="right" style="text-transform: uppercase;">
                                        <as:Literal ID="Literal17" runat="server" Text="Salessupport@mercurypay.com" meta:resourcekey="Literal17Resource1"></as:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal18" runat="server" Text="CUSTOMER SUPPORT EMAIL:" meta:resourcekey="Literal18Resource1"></as:Literal>
                                    </td>
                                    <td align="right" style="text-transform: uppercase;">
                                        <as:Literal ID="Literal19" runat="server" Text="ics@mercurypay.com" meta:resourcekey="Literal19Resource1"></as:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal20" runat="server" Text="CUSTOMER SUPPORT PHONE:" meta:resourcekey="Literal20Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("CustomerSupportPhone")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="float: right; text-align: left; margin-top: -60px">
                            <table style='font-size: 10px'>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal21" runat="server" Text="MERCHANT ID:" meta:resourcekey="Literal21Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("MerchantID")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal22" runat="server" Text="MERCHANT STATEMENT PERIOD ENDING:" meta:resourcekey="Literal22Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Convert.ToDateTime(Eval("MonthEndDate")).ToShortDateString()%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <as:Literal ID="Literal23" runat="server" Text="DBA:" meta:resourcekey="Literal23Resource1"></as:Literal>
                                    </td>
                                    <td align="right">
                                        <%# Eval("DBA")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div style="clear: both;">
                </div>
            </div>
            <br />
            <!-- MERCHANT ACCOUNT SUMMARY -->
            <as:Panel ID="pnlMerchantAccountSummary" runat="server" meta:resourcekey="pnlMerchantAccountSummaryResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal23" runat="server" Text="MERCHANT ACCOUNT SUMMARY" meta:resourcekey="Literal23Resource2"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table class="ForPrint" width="100%">
                    <tr>
                        <td align="center">
                            <DCWC:Chart ID="uxVolumeCardTypeChartForPrint" runat="server" Width="260px" Height="220px" meta:resourcekey="uxVolumeCardTypeChartForPrintResource1">

                                <Legend Name="Default" EquallySpacedItems="True" AutoFitText="False" BackColor="Transparent" Font="Arial, 10px" Alignment="Center">
                                    <Position Y="77" Width="100" Height="10" Auto="False"></Position>
                                </Legend>
                                <Legends>
                                    <DCWC:Legend AutoFitText="False" BackColor="Transparent" TableStyle="Auto" Enabled="True"
                                        EquallySpacedItems="True" Font="Arial, 10px" Name="Default" Alignment="Center">
                                        <Position X="10" Width="100" Y="77" Height="10" />
                                    </DCWC:Legend>
                                </Legends>
                                <Series>
                                    <DCWC:Series ChartArea="Area1" XValueType="Double" Name="Default" ChartType="Pie"
                                        Font="Arial, 10px, style=Bold" CustomAttributes="DoughnutRadius=20, PieLabelStyle=Disabled, ArrowsType=SharpTriangle, PieDrawingStyle=Concave, ArrowSize=1, CollectedLabel=Other, MinimumRelativePieSize=70"
                                        MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240" YValueType="Double"
                                        ShowLabelAsValue="false" ToolTip="#VALY{P}">
                                        <EmptyPointStyle BorderWidth="0" />
                                    </DCWC:Series>
                                </Series>
                                <ChartAreas>
                                    <DCWC:ChartArea Name="Area1" BorderColor="64, 64, 64, 64" BackGradientEndColor="Transparent"
                                        BackColor="Transparent" ShadowColor="Transparent" BackGradientType="TopBottom"
                                        AlignOrientation="All" AlignType="AxesView">
                                        <AxisY2>
                                            <MajorGrid Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisY2>
                                        <AxisX2>
                                            <MajorGrid Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisX2>
                                        <AxisY>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Font="Arial, 10px"></LabelStyle>
                                            <MajorGrid Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisY>
                                        <AxisX>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Font="Arial, 10px, style=Bold"></LabelStyle>
                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisX>
                                        <Position X="0" Width="100" Y="5" Height="70" />
                                        <Area3DStyle PointGapDepth="900" YAngle="100" RightAngleAxes="False" WallWidth="5"
                                            Clustered="True" XAngle="60"></Area3DStyle>
                                    </DCWC:ChartArea>
                                </ChartAreas>
                                <Titles>
                                    <DCWC:Title Font="Arial, 12px" Name="Title1" Text="MONTHLY VOLUME" Docking="Bottom"
                                        Color="#000000">
                                    </DCWC:Title>
                                </Titles>
                            </DCWC:Chart>

                        </td>
                        <td width="10%">&nbsp;</td>
                        <td>
                            <DCWC:Chart ID="uxVolumeYTDChartForPrint" runat="server" Width="280px" Height="220px" meta:resourcekey="uxVolumeYTDChartForPrintResource1">
                                <Series>
                                    <DCWC:Series Name="Default" ToolTip="#VALY{N}" Color="#95363C" BorderColor="120, 64, 64, 64"
                                        ChartType="Column" ShadowOffset="1">
                                        <EmptyPointStyle BorderWidth="0" />
                                    </DCWC:Series>
                                </Series>
                                <ChartAreas>
                                    <DCWC:ChartArea Name="Area1" BackColor="#C7D9F1" ShadowColor="transparent">
                                        <AxisY LineColor="DimGray" LabelsAutoFit="true" LabelsAutoFitStyle="DecreaseFont">
                                            <MajorGrid LineColor="DimGray" />

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Font="Arial, 10px" Format="N0" />
                                        </AxisY>
                                        <AxisX Margin="False" Interval="1">
                                            <MajorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="1" IntervalType="Auto" Disabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="1" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Format="MMM" Font="Arial, 10px" />
                                        </AxisX>
                                        <Area3DStyle WallWidth="8" PointDepth="200" RightAngleAxes="true" YAngle="30" PointGapDepth="300"
                                            Enable3D="false" />

                                        <AxisX2>
                                            <MajorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisX2>

                                        <AxisY2>
                                            <MajorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisY2>

                                        <Position X="2" Width="100" Y="10" Height="70" />
                                        <InnerPlotPosition X="20" Width="70" Height="80" />
                                    </DCWC:ChartArea>
                                </ChartAreas>
                                <Titles>
                                    <DCWC:Title Font="Arial, 12px" Name="Title1" Text="PRIOR 12 MONTH VOLUME HISTORY"
                                        Docking="Bottom" Color="#000000">
                                    </DCWC:Title>
                                </Titles>

                                <Legend Name="Default" Enabled="False"></Legend>
                                <Legends>
                                    <DCWC:Legend Enabled="False" Name="Default">
                                    </DCWC:Legend>
                                </Legends>
                            </DCWC:Chart>
                        </td>
                        <td width="10%">&nbsp;</td>
                        <td width="50%">
                            <asp:Repeater ID="uxMerchantAccountInformationSummaryForPrint" runat="server">
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%" style="border: 1px solid #00A4E4;" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="height: 20px!important; valign: middle">
                                                            <as:Literal ID="Literal23" runat="server" Text="SUMMARY AS OF" meta:resourcekey="Literal23Resource3"></as:Literal>&nbsp;
                                                            <%# Convert.ToDateTime(Eval("SummaryAs")).ToShortDateString()%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal24" runat="server" Text="SALES" meta:resourcekey="Literal24Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("SalesAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal25" runat="server" Text="CREDITS" meta:resourcekey="Literal25Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ReturnAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal26" runat="server" Text="ADJUSTMENTS" meta:resourcekey="Literal26Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("AdjustmentAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal27" runat="server" Text="SETTLEMENT/DISCOUNT" meta:resourcekey="Literal27Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("SettleDiscountAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal28" runat="server" Text="PRODUCTS & SERVICES" meta:resourcekey="Literal28Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ProductsServicesAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal29" runat="server" Text="DUES, ASSESSMENTS AND OTHER" meta:resourcekey="Literal29Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("DuesAssessmentOtherAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal30" runat="server" Text="INTERCHANGE" meta:resourcekey="Literal30Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <u>
                                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("InterchangeAmount"), 2)%></u>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal31" runat="server" Text="TOTAL" meta:resourcekey="Literal31Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("TotalAmount"), 2)%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
                <table class="noprint">
                    <tr>
                        <td>
                            <DCWC:Chart ID="uxVolumeCardTypeChart" runat="server" Width="380px" Height="220px" meta:resourcekey="uxVolumeCardTypeChartResource1">

                                <Legend Name="Default" EquallySpacedItems="True" AutoFitText="False" BackColor="Transparent" Font="Arial, 12px" Alignment="Center">
                                    <Position Y="77" Width="100" Height="10" Auto="False"></Position>
                                </Legend>
                                <Legends>
                                    <DCWC:Legend AutoFitText="False" BackColor="Transparent" TableStyle="Auto" Enabled="True"
                                        EquallySpacedItems="True" Font="Arial, 12px" Name="Default" Alignment="Center">
                                        <Position X="10" Width="100" Y="77" Height="10" />
                                    </DCWC:Legend>
                                </Legends>
                                <Series>
                                    <DCWC:Series ChartArea="Area1" XValueType="Double" Name="Default" ChartType="Pie"
                                        Font="Arial, 12px, style=Bold" CustomAttributes="DoughnutRadius=20, PieLabelStyle=Disabled, ArrowsType=SharpTriangle, PieDrawingStyle=Concave, ArrowSize=1, CollectedLabel=Other, MinimumRelativePieSize=70"
                                        MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240" YValueType="Double"
                                        ShowLabelAsValue="false" ToolTip="#PERCENT{P0}">
                                        <EmptyPointStyle BorderWidth="0" />
                                    </DCWC:Series>
                                </Series>
                                <ChartAreas>
                                    <DCWC:ChartArea Name="Area1" BorderColor="64, 64, 64, 64" BackGradientEndColor="Transparent"
                                        BackColor="Transparent" ShadowColor="Transparent" BackGradientType="TopBottom"
                                        AlignOrientation="All" AlignType="AxesView">
                                        <AxisY2>
                                            <MajorGrid Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisY2>
                                        <AxisX2>
                                            <MajorGrid Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisX2>
                                        <AxisY>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Font="Arial, 12px"></LabelStyle>
                                            <MajorGrid Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisY>
                                        <AxisX>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Font="Arial, 12px, style=Bold"></LabelStyle>
                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisX>
                                        <Position X="0" Width="100" Y="5" Height="70" />
                                        <Area3DStyle PointGapDepth="900" YAngle="100" RightAngleAxes="False" WallWidth="5"
                                            Clustered="True" XAngle="60"></Area3DStyle>
                                    </DCWC:ChartArea>
                                </ChartAreas>
                                <Titles>
                                    <DCWC:Title Font="Arial, 14px" Name="Title1" Text="MONTHLY VOLUME" Docking="Bottom"
                                        Color="#000000">
                                    </DCWC:Title>
                                </Titles>
                            </DCWC:Chart>
                        </td>
                        <td>
                            <DCWC:Chart ID="uxVolumeYTDChart" runat="server" Width="380px" Height="220px" meta:resourcekey="uxVolumeYTDChartResource1">
                                <Series>
                                    <DCWC:Series Name="Default" ToolTip="#VALY{N}" Color="#95363C" BorderColor="120, 64, 64, 64"
                                        ChartType="Column" ShadowOffset="1">
                                        <EmptyPointStyle BorderWidth="0" />
                                    </DCWC:Series>
                                </Series>
                                <ChartAreas>
                                    <DCWC:ChartArea Name="Area1" BackColor="#C7D9F1" ShadowColor="transparent">
                                        <AxisY LineColor="DimGray" LabelsAutoFit="true" LabelsAutoFitStyle="DecreaseFont">
                                            <MajorGrid LineColor="DimGray" />

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Font="Arial, 12px" Format="N0" />
                                        </AxisY>
                                        <AxisX Margin="False" Interval="1">
                                            <MajorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="1" IntervalType="Auto" Disabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="1" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle Format="MMM" Font="Arial, 12px" />
                                        </AxisX>
                                        <Area3DStyle WallWidth="8" PointDepth="200" RightAngleAxes="true" YAngle="30" PointGapDepth="300"
                                            Enable3D="false" />

                                        <AxisX2>
                                            <MajorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisX2>

                                        <AxisY2>
                                            <MajorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorGrid>

                                            <MinorGrid IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorGrid>

                                            <MajorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Disabled="False"></MajorTickMark>

                                            <MinorTickMark IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto" Enabled="False"></MinorTickMark>

                                            <LabelStyle IntervalOffset="Auto" IntervalOffsetType="Auto" Interval="Auto" IntervalType="Auto"></LabelStyle>
                                        </AxisY2>

                                        <Position X="2" Width="100" Y="10" Height="70" />
                                        <InnerPlotPosition X="20" Width="70" Height="80" />
                                    </DCWC:ChartArea>
                                </ChartAreas>
                                <Titles>
                                    <DCWC:Title Font="Arial, 14px" Name="Title1" Text="PRIOR 12 MONTH VOLUME HISTORY"
                                        Docking="Bottom" Color="#000000">
                                    </DCWC:Title>
                                </Titles>

                                <Legend Name="Default" Enabled="False"></Legend>
                                <Legends>
                                    <DCWC:Legend Enabled="False" Name="Default">
                                    </DCWC:Legend>
                                </Legends>
                            </DCWC:Chart>
                        </td>
                        <td>
                            <asp:Repeater ID="uxMerchantAccountInformationSummary" runat="server">
                                <ItemTemplate>
                                    <table class="mps_stmt_AccInfoSum">
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%" style="border: 1px solid #00A4E4;" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="height: 20px!important; valign: middle">
                                                            <as:Literal ID="Literal31" runat="server" Text="SUMMARY AS OF" meta:resourcekey="Literal31Resource2"></as:Literal>&nbsp;
                                                            <%# Convert.ToDateTime(Eval("SummaryAs")).ToShortDateString()%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal32" runat="server" Text="SALES" meta:resourcekey="Literal32Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("SalesAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal33" runat="server" Text="CREDITS" meta:resourcekey="Literal33Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ReturnAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal34" runat="server" Text="ADJUSTMENTS" meta:resourcekey="Literal34Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("AdjustmentAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal35" runat="server" Text="SETTLEMENT/DISCOUNT" meta:resourcekey="Literal35Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("SettleDiscountAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal36" runat="server" Text="PRODUCTS & SERVICES" meta:resourcekey="Literal36Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ProductsServicesAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal37" runat="server" Text="DUES, ASSESSMENTS AND OTHER" meta:resourcekey="Literal37Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("DuesAssessmentOtherAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal38" runat="server" Text="INTERCHANGE" meta:resourcekey="Literal38Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <u>
                                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("InterchangeAmount"), 2)%></u>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal39" runat="server" Text="TOTAL" meta:resourcekey="Literal39Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("TotalAmount"), 2)%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="uxMerchantAccountInformationSummary_PDF" runat="server" Visible="false">
                                <ItemTemplate>
                                    <table width="360px">
                                        <tr>
                                            <td colspan="2">
                                                <table width="100%" style="border: 1px solid Black;" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="height: 20px!important; valign: middle">
                                                            <as:Literal ID="Literal39" runat="server" Text="SUMMARY AS OF" meta:resourcekey="Literal39Resource2"></as:Literal>&nbsp;
                                                            <%# Convert.ToDateTime(Eval("SummaryAs")).ToShortDateString()%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal40" runat="server" Text="SALES" meta:resourcekey="Literal40Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("SalesAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal41" runat="server" Text="CREDITS" meta:resourcekey="Literal41Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ReturnAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal42" runat="server" Text="ADJUSTMENTS" meta:resourcekey="Literal42Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("AdjustmentAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal43" runat="server" Text="SETTLEMENT/DISCOUNT" meta:resourcekey="Literal43Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("SettleDiscountAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal44" runat="server" Text="PRODUCTS & SERVICES" meta:resourcekey="Literal44Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ProductsServicesAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal45" runat="server" Text="DUES, ASSESSMENTS AND OTHER" meta:resourcekey="Literal45Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("DuesAssessmentOtherAmount"), 2)%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal46" runat="server" Text="INTERCHANGE" meta:resourcekey="Literal46Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <u>
                                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("InterchangeAmount"), 2)%></u>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <as:Literal ID="Literal47" runat="server" Text="TOTAL" meta:resourcekey="Literal47Resource1"></as:Literal>
                                            </td>
                                            <td align="right">
                                                <%# AS.Common.Formater.FormatData.FormatNumber(Eval("TotalAmount"), 2)%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </table>
            </as:Panel>
            <br />
            <!-- DEPOSIT -->
            <as:Panel ID="pnlDeposit" runat="server" meta:resourcekey="pnlDepositResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal47" runat="server" Text="DEPOSIT DETAIL" meta:resourcekey="Literal47Resource2"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder" border="1">
                    <asp:Repeater ID="uxDeposits" runat="server">
                        <HeaderTemplate>
                            <tr class="header">
                                <td>
                                    <as:Literal ID="Literal47" runat="server" Text="DEPOSIT DATE" meta:resourcekey="Literal47Resource3"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal48" runat="server" Text="REFERENCE NUMBER" meta:resourcekey="Literal48Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal49" runat="server" Text="ITEMS" meta:resourcekey="Literal49Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal50" runat="server" Text="SALES" meta:resourcekey="Literal50Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal51" runat="server" Text="CREDITS" meta:resourcekey="Literal51Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal52" runat="server" Text="DISCOUNTS" meta:resourcekey="Literal52Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal53" runat="server" Text="BATCH TOTAL" meta:resourcekey="Literal53Resource1"></as:Literal>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Convert.ToDateTime(Eval("DepositDate")).ToShortDateString()%>
                                </td>
                                <td>
                                    <%# Eval("ReferenceNumber")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Sales"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Credits"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Discounts"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("NetDeposit"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxDeposits_BatchTotal" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal53" runat="server" Text="BATCH TOTAL" meta:resourcekey="Literal53Resource2"></as:Literal>
                                </td>
                                <td align="right">
                                    <%# Eval("Deposits")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Sales"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Credits"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Discounts"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("NetDeposit"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxDeposits_Adjustment" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td colspan="2">
                                    <as:Literal ID="Literal53" runat="server" Text="SUMMARY OF ADJUSTMENTS" meta:resourcekey="Literal53Resource3"></as:Literal>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Sales"),2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Credits"),2)%>
                                </td>
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <br />
                <div style="clear: both;">
                </div>
                <%--      <table width="100%" border="1" class="MPSBorder">
                    <tr>
                        <td colspan="7">
                            NEWS AND OTHER INFORMATION<br />
                            INTEGRATED PAYMENT PROCESSOR, MERCURY PAYMENT SYSTEMS HAS BEEN NAMED 2012 BEST CHANNEL
                            VENDOR FOR PAYMENT PROCESSING BY BUSINESS SOLUTIONS MAGAZINE (BSM) FOR THE FOURTH
                            YEAR IN A ROW. POINT-OF-SALE (POS) RESELLERS SURVEYED BY BSM GAVE MERCURY AN OVERALL
                            SCORE OF 4.54 ON A SCALE OF 0 - 5 IN SEVEN CATEGORIES: SERVICE/SUPPORT, CHANNEL
                            FRIENDLY, CHANNEL PROGRAM, PRODUCT FEATURES, PRODUCT RELIABILITY, PRODUCT INNOVATION,
                            AND ADEQUATE VAR MARGINS.
                        </td>
                    </tr>
                </table>
                <br />--%>
            </as:Panel>
            <!-- AJUSTMENT -->
            <as:Panel ID="pnlAdjustment" runat="server" meta:resourcekey="pnlAdjustmentResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal53" runat="server" Text="AJUSTMENT" meta:resourcekey="Literal53Resource4"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder">
                    <asp:Repeater ID="uxAdjustment" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal53" runat="server" Text="DATE POSTED" meta:resourcekey="Literal53Resource5"></as:Literal>
                                </td>
                                <td>
                                    <as:Literal ID="Literal54" runat="server" Text="DESCRIPTION" meta:resourcekey="Literal54Resource1"></as:Literal>
                                </td>
                                <td>
                                    <as:Literal ID="Literal55" runat="server" Text="AMOUNT" meta:resourcekey="Literal55Resource1"></as:Literal>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </as:Panel>
            <!-- SETTELEMENT/DISCOUNT -->
            <as:Panel ID="pnlSettlement" runat="server" meta:resourcekey="pnlSettlementResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal55" runat="server" Text="SETTLEMENT/DISCOUNT" meta:resourcekey="Literal55Resource2"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder" border="1">
                    <asp:Repeater ID="uxSettelement" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal55" runat="server" Text="DESCRIPTION" meta:resourcekey="Literal55Resource3"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal56" runat="server" Text="ITEMS" meta:resourcekey="Literal56Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal57" runat="server" Text="VOLUME" meta:resourcekey="Literal57Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal58" runat="server" Text="AVERAGE TICKET" meta:resourcekey="Literal58Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal59" runat="server" Text="DISCOUNT RATE" meta:resourcekey="Literal59Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal60" runat="server" Text="ITEM RATE" meta:resourcekey="Literal60Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal61" runat="server" Text="AMOUNT" meta:resourcekey="Literal61Resource1"></as:Literal>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Volume"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("AverageTicket"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("DiscountRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ItemRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxSettelement_Total" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal61" runat="server" Text="TOTAL" meta:resourcekey="Literal61Resource2"></as:Literal>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Volume"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("AverageTicket"), 2)%>
                                </td>
                                <td colspan="3" align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </as:Panel>
            <!-- PRODUCTS & SERVICES -->
            <as:Panel ID="pnlProducts" runat="server" meta:resourcekey="pnlProductsResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal61" runat="server" Text="PRODUCTS & SERVICES" meta:resourcekey="Literal61Resource3"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder" border="1">
                    <asp:Repeater ID="uxProducts" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal61" runat="server" Text="DESCRIPTION" meta:resourcekey="Literal61Resource4"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal62" runat="server" Text="ITEMS" meta:resourcekey="Literal62Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal63" runat="server" Text="VOLUME" meta:resourcekey="Literal63Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal64" runat="server" Text="VOLUME RATE" meta:resourcekey="Literal64Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal65" runat="server" Text="ITEM RATE" meta:resourcekey="Literal65Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal66" runat="server" Text="AMOUNT" meta:resourcekey="Literal66Resource1"></as:Literal>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td align="right">
                                    <%#Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Volume"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("VolumeRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ItemRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxProducts_Total" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal66" runat="server" Text="TOTAL" meta:resourcekey="Literal66Resource2"></as:Literal>
                                </td>
                                <td colspan="5" align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </as:Panel>
            <!-- DUE, ASSESSMENTS AND OTHER -->
            <as:Panel ID="pnlDue" runat="server" meta:resourcekey="pnlDueResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal66" runat="server" Text="DUES, ASSESSMENTS AND OTHER" meta:resourcekey="Literal66Resource3"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder" border="1">
                    <asp:Repeater ID="uxDue" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal66" runat="server" Text="DESCRIPTION" meta:resourcekey="Literal66Resource4"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal67" runat="server" Text="ITEMS" meta:resourcekey="Literal67Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal68" runat="server" Text="VOLUME" meta:resourcekey="Literal68Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal69" runat="server" Text="VOLUME RATE" meta:resourcekey="Literal69Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal70" runat="server" Text="ITEM RATE" meta:resourcekey="Literal70Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal71" runat="server" Text="AMOUNT" meta:resourcekey="Literal71Resource1"></as:Literal>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Volume"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("VolumeRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ItemRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxDue_Total" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal71" runat="server" Text="TOTAL" meta:resourcekey="Literal71Resource2"></as:Literal>
                                </td>
                                <td colspan="5" align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <br />
                <div style="clear: both;">
                </div>
                <%--   <table width="100%" class="MPSBorder" border="1">
                    <tr>
                        <td>
                            NEWS AND OTHER INFORMATION<br />
                            “INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL AND SAVINGS - WHETHER THAT
                            IS IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE A MORE ENHANCED HOLISTIC
                            SHOPPING EXPERIENCE,” SAID MARIO SHILIASHKI, GROUP HEAD, U.S. MARKETS EMERGING PAYMENTS
                            LEAD, MASTERCARD. “OUR COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL
                            MAKE MASTERCARD THE ‘GO-TO’ OFFERS SOLUTION FOR MERCHANTS AND ISSUERS LOOKING FOR
                            A STRONGER CONNECTION WITH OUR CARDHOLDERS.”
                        </td>
                    </tr>
                </table>--%>
            </as:Panel>
            <as:Panel ID="pnlInterchange" runat="server" meta:resourcekey="pnlInterchangeResource1">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;<as:Literal ID="Literal71" runat="server" Text="INTERCHANGE" meta:resourcekey="Literal71Resource3"></as:Literal></b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder" border="1">
                    <asp:Repeater ID="uxInterchange" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal71" runat="server" Text="CARD TYPE" meta:resourcekey="Literal71Resource4"></as:Literal>
                                </td>
                                <td>
                                    <as:Literal ID="Literal72" runat="server" Text="DESCRIPTION" meta:resourcekey="Literal72Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal73" runat="server" Text="ITEMS" meta:resourcekey="Literal73Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal74" runat="server" Text="VOLUME" meta:resourcekey="Literal74Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal75" runat="server" Text="VOLUME RATE" meta:resourcekey="Literal75Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal76" runat="server" Text="ITEM RATE" meta:resourcekey="Literal76Resource1"></as:Literal>
                                </td>
                                <td align="center">
                                    <as:Literal ID="Literal77" runat="server" Text="AMOUNT" meta:resourcekey="Literal77Resource1"></as:Literal>
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("CardType")%>
                                </td>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Volume"), 2)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("VolumeRate"), 4).ToString() == "0.0000" ? "&nbsp;" : AS.Common.Formater.FormatData.FormatNumber(Eval("VolumeRate"), 4) %>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("ItemRate"), 4)%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="uxInterchange_Total">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <as:Literal ID="Literal77" runat="server" Text="TOTAL" meta:resourcekey="Literal77Resource2"></as:Literal>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Volume"), 2)%>
                                </td>
                                <td colspan="3" align="right">
                                    <%# AS.Common.Formater.FormatData.FormatNumber(Eval("Amount"), 2)%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <br />
                <div style="clear: both;">
                </div>
                <%-- <table width="100%">
                    <tr>
                        <td>
                            “INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL AND SAVINGS - WHETHER THAT
                            IS IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE A MORE ENHANCED HOLISTIC
                            SHOPPING EXPERIENCE,” SAID MARIO SHILIASHKI, GROUP HEAD, U.S. MARKETS EMERGING PAYMENTS
                            LEAD, MASTERCARD. “OUR COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL
                            MAKE MASTERCARD THE ‘GO-TO’ OFFERS SOLUTION FOR MERCHANTS AND ISSUERS LOOKING FOR
                            A STRONGER CONNECTION WITH OUR CARDHOLDERS.”
                        </td>
                    </tr>
                </table>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" class="MPSBorder" border="1" runat="server" id="uxFooter">
                    <tr>
                        <td>
                            NEWS AND OTHER INFORMATION<br />
                            “INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL AND SAVINGS - WHETHER THAT
                            IS IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE A MORE ENHANCED HOLISTIC
                            SHOPPING EXPERIENCE,” SAID MARIO SHILIASHKI, GROUP HEAD, U.S. MARKETS EMERGING PAYMENTS
                            LEAD, MASTERCARD. “OUR COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL
                            MAKE MASTERCARD THE ‘GO-TO’ OFFERS SOLUTION FOR MERCHANTS AND ISSUERS LOOKING FOR
                            A STRONGER CONNECTION WITH OUR CARDHOLDERS.”
                        </td>
                    </tr>
                </table>
                <br />--%>
                <div style="clear: both;">
                </div>
            </as:Panel>
        </as:PlaceHolder>
    </div>

    <script type="text/javascript">
        function PrintPage() {
            if (window.print)
                try { setTimeout('window.print()', 500); } catch (ex) { }
            else
                alert('<%= GetLocalResourceObject("StatementDetail_IHP_aspx_AlertJavascript").ToString() %>');
        }
    </script>

</asp:Content>
