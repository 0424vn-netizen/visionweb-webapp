<%@ Page Title="STATEMENT DETAILS" Language="C#" AutoEventWireup="true" CodeFile="StatementDetail_MPS.aspx.cs"
    Inherits="StatementDetail_MPS" %>

<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>
<html>
<head id="Head1" runat="server">
<style>
@media print{
    h1.print{
        display:block;
    }
}
@media screen{
    h1.print{
        display:none;
    }
}
</style>
</head>
<body runat="server">
    <h1 class="print">Hello World</h1>
    <as:Literal ID="uxHeader" Visible="false" Text="No data to display" runat="server"></as:Literal>
    <div style="width: 99%; margin: auto;">
        
        <img src="../App_Themes/MPS/images/logo.jpg" alt='logo'/>
        <br style="clear: both;" />
        <as:PlaceHolder runat="server" ID="uxExportPanel">
            <div style="text-align: left;">
                <asp:Repeater ID="uxMerchantInfo" runat="server">
                    <ItemTemplate>
                        <div style="float: left; text-align: left; margin-left: 20px;">
                            <%# Eval("ReturnAddressLine1")%><br />
                            <%# Eval("ReturnAddressLine2")%><br />
                            <%# Eval("DeliveryAddressLine1")%><br />
                            <%# Eval("DeliveryAddressLine2")%><br />
                            <%# Eval("DeliveryAddressLine3")%><br />
                            <%# Eval("DeliveryAddressLine4")%><br />
                        </div>
                        <div style="float: right; text-align: left; margin-right: 20px">
                            <table>
                                <tr>
                                    <td>
                                        MERCHANT ID:
                                    </td>
                                    <td>
                                        <%# Eval("MerchantID")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        MERCHANT STATEMENT PERIOD ENDING:
                                    </td>
                                    <td>
                                        <%# Convert.ToDateTime(Eval("MonthEndDate")).ToShortDateString()%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        DBA:
                                    </td>
                                    <td>
                                        <%# Eval("DBA")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <u>HOW TO REACH US:</u>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SALES SUPPORT EMAIL:
                                    </td>
                                    <td>
                                        Salessupport@mercurypay.com
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        CUSTOMER SUPPORT EMAIL:
                                    </td>
                                    <td>
                                        ics@mercurypay.com
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        CUSTOMER SUPPORT PHONE:
                                    </td>
                                    <td>
                                        <%# Eval("CustomerSupportPhone")%>
                                    </td>
                                </tr>
                            </table>
                            <%--CREDIT CARD MERCHANT STATEMENT<br />
                            DATE:
                            <%# Eval("ReportDate", "{0:MM/dd/yyyy}")%><br />
                            CODES:
                            <%# Eval("HoldSw")%>&nbsp; &nbsp;&nbsp;FORM: 9&nbsp;&nbsp;&nbsp;&nbsp;<%#Eval("TransitNumber")%><br />
                            MERCHANT:
                            <%# Eval("MerchantNumber")%><br />
                            <%# Eval("DDANumber")%><br />
                            <br />
                            <br />
                            <%# Eval("Name")%><br />
                            <%# Eval("Attention")%><br />
                            <%# Eval("Address1")%>&nbsp;<%# Eval("Address2")%><br />
                            <%# Eval("City")%>&nbsp;<%# Eval("State")%>&nbsp;<%# Eval("Zip")%>--%>
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
            <as:Panel ID="pnlMerchantAccountSummary" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b style="color:White">&nbsp;MERCHANT ACCOUNT SUMMARY</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table>
                    <tr>
                        <td>
                            <DCWC:Chart ID="uxVolumeCardTypeChart" runat="server" Width="320px" Height="220px"
                                AntiAliasing="All" TextAntiAliasingQuality="High" BackGradientEndColor="Gray"
                                BorderLineColor="SlateGray" BorderLineStyle="Solid" Palette="AcidWash" BackGradientType="DiagonalLeft">
                                <Legends>
                                    <DCWC:Legend AutoFitText="False" BackColor="Transparent" Alignment="Center" TableStyle="Wide"
                                        Docking="Bottom" Enabled="True" EquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold"
                                        Name="Default">
                                        <Position X="5" Width="90" Y="80" Height="20" />
                                    </DCWC:Legend>
                                </Legends>
                                <BorderSkin SkinStyle="Emboss" FrameBackGradientEndColor="LightGray" PageColor="Transparent">
                                </BorderSkin>
                                <Series>
                                    <DCWC:Series ChartArea="Area1" XValueType="Double" Name="Default" ChartType="Pie"
                                        Font="Trebuchet MS, 8.25pt, style=Bold" CustomAttributes="DoughnutRadius=50, PieLabelStyle=Disabled, ArrowsType=SharpTriangle, PieDrawingStyle=Concave, ArrowSize=1, CollectedLabel=Other, MinimumRelativePieSize=70"
                                        MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240" YValueType="Double"
                                        ShowLabelAsValue="False">
                                        <EmptyPointStyle BorderWidth="0" />
                                    </DCWC:Series>
                                </Series>
                                <ChartAreas>
                                    <DCWC:ChartArea Name="Area1" BorderColor="64, 64, 64, 64" BackGradientEndColor="Transparent"
                                        BackColor="Transparent" ShadowColor="Transparent" BackGradientType="TopBottom"
                                        AlignOrientation="All" AlignType="AxesView">
                                        <AxisY2>
                                            <MajorGrid Enabled="False"></MajorGrid>
                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisY2>
                                        <AxisX2>
                                            <MajorGrid Enabled="False"></MajorGrid>
                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisX2>
                                        <AxisY>
                                            <LabelStyle Font="Trebuchet MS, 8.25pt"></LabelStyle>
                                            <MajorGrid Enabled="False"></MajorGrid>
                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisY>
                                        <AxisX>
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold"></LabelStyle>
                                            <MajorGrid LineColor="64, 64, 64, 64" Enabled="False"></MajorGrid>
                                            <MajorTickMark Enabled="False"></MajorTickMark>
                                        </AxisX>
                                        <Position X="10" Width="80" Y="20" Height="60" />
                                        <Area3DStyle PointGapDepth="900" YAngle="100" RightAngleAxes="False" WallWidth="5"
                                            Clustered="True" XAngle="60"></Area3DStyle>
                                    </DCWC:ChartArea>
                                </ChartAreas>
                                <Titles>
                                    <DCWC:Title Font="Microsoft Sans Serif, 8.25pt, style=Bold" Name="Title1" Text="Volume by Card Type">
                                    </DCWC:Title>
                                </Titles>
                            </DCWC:Chart>
                        </td>
                        <td>
                            <DCWC:Chart ID="uxVolumeYTDChart" runat="server" Width="320px" Height="220px" AntiAliasing="All"
                                TextAntiAliasingQuality="High" BackGradientEndColor="Gray" BorderLineColor="SlateGray"
                                BorderLineStyle="Solid" Palette="AcidWash" BackGradientType="DiagonalLeft">
                                <Series>
                                    <DCWC:Series Name="Default" BackGradientEndColor="White" ToolTip="#VALY{C}" Color="79, 129, 189"
                                        BorderColor="120, 64, 64, 64" ChartType="Column" ShadowOffset="1">
                                        <EmptyPointStyle BorderWidth="0" />
                                    </DCWC:Series>
                                </Series>
                                <BorderSkin SkinStyle="Emboss" FrameBackGradientEndColor="LightGray" PageColor="Transparent">
                                </BorderSkin>
                                <ChartAreas>
                                    <DCWC:ChartArea Name="Area1" BackColor="transparent" ShadowColor="transparent">
                                        <AxisY LineColor="DimGray" LabelsAutoFit="true" LabelsAutoFitStyle="DecreaseFont">
                                            <MajorGrid LineColor="DimGray" />
                                            <LabelStyle Font="Trebuchet MS, 7.5pt, style=Bold" Format="C" />
                                        </AxisY>
                                        <AxisX Margin="False">
                                            <LabelStyle Format="MMM" />
                                        </AxisX>
                                        <Area3DStyle WallWidth="8" PointDepth="200" RightAngleAxes="true" YAngle="30" PointGapDepth="300"
                                            Enable3D="false" />
                                        <Position X="2" Width="90" Y="17" Height="80" />
                                        <InnerPlotPosition X="30" Width="70" Height="80" />
                                    </DCWC:ChartArea>
                                </ChartAreas>
                                <Titles>
                                    <DCWC:Title Font="Arial, 9pt, style=Bold" Name="Title1" Text="YTD Volume">
                                    </DCWC:Title>
                                </Titles>
                                <Legends>
                                    <DCWC:Legend Enabled="False" Name="Default">
                                    </DCWC:Legend>
                                </Legends>
                            </DCWC:Chart>
                        </td>
                        <td>
                            <asp:Repeater ID="uxMerchantAccountInformationSummary" runat="server">
                                <ItemTemplate>
                                    <table width="320px" border="1">
                                        <tr>
                                            <td colspan="2">
                                                SUMMARY AS OF &nbsp;
                                                <%# Convert.ToDateTime(Eval("SummaryAs")).ToShortDateString()%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                SALES
                                            </td>
                                            <td>
                                                <%# Eval("SalesAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CREDITS
                                            </td>
                                            <td>
                                                <%# Eval("ReturnAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                ADJUSTMENT
                                            </td>
                                            <td>
                                                <%# Eval("AdjustmentAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                SETTLEMENT/DISCOUNT
                                            </td>
                                            <td>
                                                <%# Eval("SettleDiscountAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PRODUCTS & SERVICES
                                            </td>
                                            <td>
                                                <%# Eval("ProductsServicesAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                DUES, ASSESSMENT AND OTHER
                                            </td>
                                            <td>
                                                <%# Eval("DuesAssessmentOtherAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                INTERCHANGE
                                            </td>
                                            <td>
                                                <%# Eval("InterchangeAmount")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                TOTAL
                                            </td>
                                            <td>
                                                <%# Eval("TotalAmount")%>
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
            <as:Panel ID="pnlDeposit" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;DEPOSIT DETAIL</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" border="1">
                    <asp:Repeater ID="uxDeposits" runat="server">
                        <HeaderTemplate>
                            <tr class="header">
                                <td>
                                    DEPOSIT DATE
                                </td>
                                <td align="center">
                                    REFERENCE NUMBER
                                </td>
                                <td align="center">
                                    ITEMS
                                </td>
                                <td align="center">
                                    SALES
                                </td>
                                <td align="center">
                                    CREDITS
                                </td>
                                <td align="center">
                                    DISCOUNTS
                                </td>
                                <td align="center">
                                    NET DEPOSIT
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
                                    <%# Eval("Sales")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Credits")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Discounts")%>
                                </td>
                                <td align="right">
                                    <%# Eval("NetDeposit")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxDeposits_BatchTotal" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    BATCH TOTAL
                                </td>
                                <td align="right">
                                    <%# Eval("Deposits")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Sales")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Credits")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Discounts")%>
                                </td>
                                <td align="right">
                                    <%# Eval("NetDeposit")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxDeposits_Adjustment" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td colspan="2">
                                    SUMMARY OF ADJUSTMENT
                                </td>
                                <td align="right">
                                    <%# Eval("Items")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Sales")%>
                                </td>
                                <td align="right">
                                    <%# Eval("Credits")%>
                                </td>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%" border="1">
                    <tr>
                        <td colspan="7">
                            NEWS AND OTHER INFORMATION<br />
                            INTEGRATED PAYMENT PROCESSOR, MERCURY PAYMENT SYSTEMS HAS BEEN NAMED 2012 BEST CHANNEL
                            VENDOR FOR PAYMENT PROCESSING BY BUSINESS<br />
                            SOLUTIONS MAGAZINE (BSM) FOR THE FOURTH YEAR IN A ROW. POINT-OF-SALE (POS) RESELLERS
                            SURVEYED BY BSM GAVE MERCURY AN OVERRALL SCORE OF 4.54 ON A<br />
                            SCALE OF 0-5 IN SEVEN CATEGORIES: SERVICE/SUPPORT, CHANNEL FRIENDLY, CHANNEL PROGRAM,
                            PRODUCT FEATURES, PRODUCT RELIABILITY, PRODUCT<br />
                            INNOVATION, AND ADEQUATE VAR MARGINS.
                        </td>
                    </tr>
                </table>
                <br />
            </as:Panel>
            <!-- AJUSTMENT -->
            <as:Panel ID="pnlAdjustment" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;AJUSTMENT</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%">
                    <asp:Repeater ID="uxAdjustment" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    DATE POSTED
                                </td>
                                <td>
                                    DESCRIPTION
                                </td>
                                <td>
                                    AMOUNT
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </as:Panel>
            <!-- SETTELEMENT/DISCOUNT -->
            <as:Panel ID="pnlSettlement" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;SETTELEMENT/DISCOUNT</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%">
                    <asp:Repeater ID="uxSettelement" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    DESCRIPTION
                                </td>
                                <td>
                                    ITEMS
                                </td>
                                <td>
                                    VOLUME
                                </td>
                                <td>
                                    AVERAGE TICKET
                                </td>
                                <td>
                                    DISCOUNT RATE
                                </td>
                                <td>
                                    ITEM RATE
                                </td>
                                <td>
                                    AMOUNT
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td>
                                    <%# Eval("Items")%>
                                </td>
                                <td>
                                    <%# Eval("Volume")%>
                                </td>
                                <td>
                                    <%# Eval("AverageTicket")%>
                                </td>
                                <td>
                                    <%# Eval("DiscountRate")%>
                                </td>
                                <td>
                                    <%# Eval("ItemRate")%>
                                </td>
                                <td>
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxSettelement_Total" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    TOTAL
                                </td>
                                <td>
                                    <%# Eval("Items")%>
                                </td>
                                <td>
                                    <%# Eval("Volume")%>
                                </td>
                                <td>
                                    <%# Eval("AverageTicket")%>
                                </td>
                                <td colspan="3">
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </as:Panel>
            <!-- PRODUCTS & SERVICES -->
            <as:Panel ID="pnlProducts" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;PRODUCTS & SERVICES</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%">
                    <asp:Repeater ID="uxProducts" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    DESCRIPTION
                                </td>
                                <td>
                                    ITEMS
                                </td>
                                <td>
                                    VOLUME
                                </td>
                                <td>
                                    VOLUME RATE
                                </td>
                                <td>
                                    ITEM RATE
                                </td>
                                <td>
                                    AMOUNT
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td>
                                    <%# Eval("Items")%>
                                </td>
                                <td>
                                    <%# Eval("Volume")%>
                                </td>
                                <td>
                                    <%# Eval("VolumeRate")%>
                                </td>
                                <td>
                                    <%# Eval("ItemRate")%>
                                </td>
                                <td>
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxProducts_Total" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    TOTAL
                                </td>
                                <td colspan="5">
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </as:Panel>
            <!-- DUE, ASSESSMENTS AND OTHER -->
            <as:Panel ID="pnlDue" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;DUE, ASSESSMENTS AND OTHER</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%">
                    <asp:Repeater ID="uxDue" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    DESCRIPTION
                                </td>
                                <td>
                                    ITEMS
                                </td>
                                <td>
                                    VOLUME
                                </td>
                                <td>
                                    VOLUME RATE
                                </td>
                                <td>
                                    ITEM RATE
                                </td>
                                <td>
                                    AMOUNT
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("Description")%>
                                </td>
                                <td>
                                    <%# Eval("Items")%>
                                </td>
                                <td>
                                    <%# Eval("Volume")%>
                                </td>
                                <td>
                                    <%# Eval("VolumeRate")%>
                                </td>
                                <td>
                                    <%# Eval("ItemRate")%>
                                </td>
                                <td>
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="uxDue_Total" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    TOTAL
                                </td>
                                <td colspan="5">
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            NEWS AND OTHER INFORMATION "INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL
                            AND SAVINGS-WHETHER THAT IS IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE
                            A
                            <br />
                            MORE ENHANCED HOLISTIC SHOPPING EXPERIENCE," SAID MARIO SHILIASHKI, GROUP HEAD,
                            U.S MARKETS EMERGING PAYMENTS LEAD, MASTERCARD."OUR
                            <br />
                            COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL MAKE MASTERCARD THE 'GO-TO'OFFERS
                            SOLLUTION FOR MERCHANTS AND ISSUES LOOKING FOR<br />
                            A STRONGER CONNECTION WITH OUR CARDHOLDER. "
                        </td>
                    </tr>
                </table>
            </as:Panel>
            <as:Panel ID="pnlInterchange" runat="server">
                <br />
                <div class="ContainerPanelHeader">
                    <b>&nbsp;INTERCHANGE</b>
                </div>
                <br />
                <div style="clear: both;">
                </div>
                <table width="100%">
                    <asp:Repeater ID="uxInterchange" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <td>
                                    CARD TYPE
                                </td>
                                <td>
                                    DESCRIPTION
                                </td>
                                <td>
                                    ITEMS
                                </td>
                                <td>
                                    VOLUME
                                </td>
                                <td>
                                    VOLUME RATE
                                </td>
                                <td>
                                    ITEM RATE
                                </td>
                                <td>
                                    AMOUNT
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
                                <td>
                                    <%# Eval("Items")%>
                                </td>
                                <td>
                                    <%# Eval("Volume")%>
                                </td>
                                <td>
                                    <%# Eval("VolumeRate")%>
                                </td>
                                <td>
                                    <%# Eval("ItemRate")%>
                                </td>
                                <td>
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="uxInterchange_Total">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    TOTAL
                                </td>
                                <td colspan="6">
                                    <%# Eval("Amount")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            "INCREASINGLY, CONSUMERS ARE LOOKING FOR A GREAT DEAL AND SAVINGS-WHETHER THAT IS
                            IN PAPER COUPONS OR THROUGH DIGITAL CHANNELS THAT CREATE A<br />
                            MORE ENHANCED HOLISTIC SHOPPING EXPERIENCE," SAID MARIO SHILIASHKI, GROUP HEAD,
                            U.S MARKETS EMERGING PAYMENTS LEAD, MASTERCARD."OUR
                            <br />
                            COLLABORATION WITH PARTNERS LIKE LOCAL OFFER NETWORK WILL MAKE MASTERCARD THE 'GO-TO'
                            OFFERS SOLUTION FOR MERCHANTS AND ISSUERS LOOKING FOR
                            <br />
                            A STRONGER CONNECTION WITH OUR CARDHOLDERS."
                        </td>
                    </tr>
                </table>
            </as:Panel>
        </as:PlaceHolder>
    </div>
</body>
</html>
