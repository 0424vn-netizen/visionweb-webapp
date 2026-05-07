<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true"
    CodeFile="CbSequenceDetail.aspx.cs" Inherits="CbSequenceDetail" Title="CB SEQUENCE DETAIL" meta:resourcekey="PageResource1" %>

<%@ Register TagName="PageTitle" Src="~/UserControls/PageTitle.ascx" TagPrefix="uc" %>
<%@ Register TagName="UxExport" Src="~/UserControls/UxExport.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="">
        <h3 class="modal-title"><as:Literal ID="ltCBDetail" runat="server" Text="CB Detail" meta:resourcekey="ltCBDetailResource1"></as:Literal></h3>
        <asp:Repeater ID="rptCBDetail" runat="server" EnableViewState="true">
            <ItemTemplate>
                <table class="ASTable">
                    <colgroup>
                        <col style="width: 35%" />
                        <col style="width: 65%" />
                    </colgroup>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltMID" runat="server" Text="MID:" meta:resourcekey="ltMIDResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("MID")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltTransactionID" runat="server" Text="Transaction ID:" meta:resourcekey="ltTransactionIDResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("TransactionID")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltTransactionSource" runat="server" Text="Transaction Source:" meta:resourcekey="ltTransactionSourceResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("TransactionSource")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltSequenceNumber" runat="server" Text="Sequence Number:" meta:resourcekey="ltSequenceNumberResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("SequenceNumber")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltCardNumber" runat="server" Text="Card Number:" meta:resourcekey="ltCardNumberResource1"></as:Literal>
                        </td>
                        <td>
                            <%# DisplayCardNumber(Eval("CardNumber"),Eval("PartialCardNumber"), Eval("RecordID"), Eval("ReportDate") )%>
                                
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" Visible="<%# GeneralFuncsLib.Show_RoutingAccountNumber %>" ID="phRouting">
                        <tr class="Row" >
                            <td class="heading"><as:Literal ID="Literal1" runat="server" meta:resourcekey="ltRoutingAccountNumberResource1"></as:Literal>
                            </td>
                            <td>
                                <%# DisplayCardNumber(Eval("RoutingAccountNumber"),Eval("PartialRoutingACC"), Eval("RecordID"), Eval("ReportDate") )%>
                                
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltReferenceNumber" runat="server" Text="Acquirer Reference Number:" meta:resourcekey="ltReferenceNumberResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("ReferenceNumber")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltCBAmount" runat="server" Text="CB Amount:" meta:resourcekey="ltCBAmountResource1"></as:Literal>
                        </td>
                        <td>
                            <%# FormatCurrency(Eval("CBAmount"))%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="CBReason" runat="server" Text="CB Reason:" meta:resourcekey="CBReasonResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("ReasonCode")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltCBReasonDesc" runat="server" Text="CB Reason Desc:" meta:resourcekey="ltCBReasonDescResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("CBReasonDesc")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltCentralProcessingDate" runat="server" Text="Central Processing Date:" meta:resourcekey="ltCentralProcessingDateResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("CentralProcessingDate")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltMCC" runat="server" Text="MCC:" meta:resourcekey="ltMCCResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("MCC")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltFeeAttribute" runat="server" Text="Fee Attribute:" meta:resourcekey="ltFeeAttributeResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("FeeAttribute")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltTranDate" runat="server" Text="Tran Date:" meta:resourcekey="ltTranDateResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("TranDate")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltTranType" runat="server" Text="Tran Type:" meta:resourcekey="ltTranTypeResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("TranType")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltCBReferenceNumber" runat="server" Text="CB Reference Number:" meta:resourcekey="ltCBReferenceNumberResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("CBReferenceNumber")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltInterchangeFee" runat="server" Text="Interchange Fee:" meta:resourcekey="ltInterchangeFeeResource1"></as:Literal>
                        </td>
                        <td>
                            <%# FormatCurrency(Eval("InterchangeFee"))%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltClearanceCode" runat="server" Text="Clearance Code:" meta:resourcekey="ltClearanceCodeResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("ClearanceCode")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltClearanceDate" runat="server" Text="Clearance Date:" meta:resourcekey="ltClearanceDateResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("ClearanceDate")%>
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltCBType" runat="server" Text="CB Type:" meta:resourcekey="ltCBTypeResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("CBType")%>
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltCBTypeDescr" runat="server" Text="CB Type Descr:" meta:resourcekey="ltCBTypeDescrResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("CBTypeDesc")%>
                                
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltDisposition" runat="server" Text="Disposition:" meta:resourcekey="ltDispositionResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("Disposition")%>
                                
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltAuthCode" runat="server" Text="Auth Code:" meta:resourcekey="ltAuthCodeResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("AuthCode")%>
                                
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltMessage1" runat="server" Text="Message 1:" meta:resourcekey="ltMessage1Resource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("Message1")%>
                                
                        </td>
                    </tr>
                    <tr class="AltRow">
                        <td class="heading"><as:Literal ID="ltMessage2" runat="server" Text="Message 2:" meta:resourcekey="ltMessage2Resource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("Message2")%>
                                
                        </td>
                    </tr>
                    <tr class="Row">
                        <td class="heading"><as:Literal ID="ltFeeProgramIndicator" runat="server" Text="Fee Program Indicator:" meta:resourcekey="ltFeeProgramIndicatorResource1"></as:Literal>
                        </td>
                        <td>
                            <%# Eval("FeeProgramIndicator")%>
                                
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </as:ASModalContainer>
</asp:Content>
