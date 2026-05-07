<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_ReportMerchantInformation.ascx.cs" Inherits="UserControls_rm_MCF_ReportMerchantInformation" %>

<as:RadAjaxManagerProxy ID="RadAjaxProxy" runat="server">
    <AjaxSettings>
        <tek:AjaxSetting AjaxControlID="uxSave">
            <UpdatedControls>
                <tek:AjaxUpdatedControl ControlID="uxWatch" />
                <tek:AjaxUpdatedControl ControlID="uxMultiWatchList" />
                <tek:AjaxUpdatedControl ControlID="uxProfile" />
            </UpdatedControls>
        </tek:AjaxSetting>
    </AjaxSettings>
</as:RadAjaxManagerProxy>
<div id="uxMerchantInformationGrid" class="in">
    <div class="row">
        <div class="col-md-12">
            <table class="ASTable">
                <colgroup>
                    <col style="width: 110px" />
                    <col style="width: 180px" />
                    <col/>
                    <col style="width: 125px" />
                    <col style="width: 120px" />
                    <col style="width: 170px" />
                    <col style="width: 100px" />
					<col style="width: 110px" />
                </colgroup>
                <tr class="Row">
                    <td class="heading valign-middle"><as:Literal ID="ltMerchantName" runat="server" Text="Merchant Name:" meta:resourcekey="ltMerchantNameResource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxMerchantName" runat="server" meta:resourcekey="uxMerchantNameResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal3" runat="server" Text="Appr.Date:" meta:resourcekey="Literal3Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxApprovalDate" runat="server" meta:resourcekey="uxApprovalDateResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal4" runat="server" Text="Last Batch Activity:" meta:resourcekey="Literal4Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxLastActiveDate" runat="server" meta:resourcekey="uxLastActiveDateResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal runat="server" ID="uxHierarchy1" meta:resourcekey="uxHierarchy1Resource1" />
                    </td>
                    <td>
                        <as:Literal ID="uxHierarchyValue1" runat="server" meta:resourcekey="uxHierarchyValue1Resource1" />&nbsp;
                    </td>
                </tr>
                <tr class="AltRow">
                    <td class="heading valign-middle" rowspan="2"><as:Literal ID="Literal5" runat="server" Text="Address:" meta:resourcekey="Literal5Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxAddress1" runat="server" meta:resourcekey="uxAddress1Resource1" />
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal6" runat="server" Text="Owner:" meta:resourcekey="Literal6Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxOwner" runat="server" meta:resourcekey="uxOwnerResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal7" runat="server" Text="Last Statement:" meta:resourcekey="Literal7Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxLastStatementDate" runat="server" meta:resourcekey="uxLastStatementDateResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal runat="server" ID="uxHierarchy2" meta:resourcekey="uxHierarchy2Resource1" />
                    </td>
                    <td>
                        <as:Literal ID="uxHierarchyValue2" runat="server" meta:resourcekey="uxHierarchyValue2Resource1" />&nbsp;                
                    </td>
                </tr>
                <tr class="Row">
                    <td>
                        <as:Literal ID="uxAddress2" runat="server" meta:resourcekey="uxAddress2Resource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal8" runat="server" Text="Phone:" meta:resourcekey="Literal8Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxPhone" runat="server" meta:resourcekey="uxPhoneResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal9" runat="server" Text="Profile:" meta:resourcekey="Literal9Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:RadComboBox ID="uxProfile" runat="server" EnableEmbeddedBaseStylesheet="false"
                            DataValueField="DataKey" DataTextField="DataText" Width="130px">
                        </as:RadComboBox>
                        <as:Literal ID="uxProfileReadOnly" runat="server" meta:resourcekey="uxProfileReadOnlyResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal runat="server" ID="uxHierarchy3" meta:resourcekey="uxHierarchy3Resource1" />
                        <as:Literal runat="server" ID="uxServicedByLabel" Visible="False" meta:resourcekey="uxServicedByLabelResource1" />
                    </td>
                    <td>
                        <as:Literal ID="uxHierarchyValue3" runat="server" meta:resourcekey="uxHierarchyValue3Resource1" />
                        <as:Literal ID="uxServicedByValue" runat="server" Visible="False" meta:resourcekey="uxServicedByValueResource1" />
                    </td>
                </tr>
                <tr class="AltRow">
                    <td></td>
                    <td></td>
                    <td class="heading valign-middle"><as:Literal ID="Literal10" runat="server" Text="Fax:" meta:resourcekey="Literal10Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxFax" runat="server" meta:resourcekey="uxFaxResource1" />&nbsp;
                
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal11" runat="server" Text="Status:" meta:resourcekey="Literal11Resource1"></as:Literal>
                    </td>
                    <td>
                        <as:Literal ID="uxStatus" runat="server" meta:resourcekey="uxStatusResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle">
                        <as:Literal runat="server" ID="uxHierarchy4" meta:resourcekey="uxHierarchy4Resource1" />
                    </td>
                    <td>
                        <as:Literal ID="uxHierarchyValue4" runat="server" meta:resourcekey="uxHierarchyValue4Resource1" />&nbsp;
                    </td>
                </tr>
                <tr class="Row">
                    <td class="heading valign-middle"><as:Literal ID="Literal12" runat="server" Text="SIC/MCC:" meta:resourcekey="Literal12Resource1"></as:Literal>
                    </td>
                    <td colspan="3">
                        <as:Literal ID="uxSIC" runat="server" meta:resourcekey="uxSICResource1" />&nbsp;
                    </td>
                    <td class="heading valign-middle"><as:Literal ID="Literal13" runat="server" Text="Watch / Multi-Watch:" meta:resourcekey="Literal13Resource1"></as:Literal>
                    </td>
                    <td class="valign-middle">
                        <div class="inline-block">
                            <as:CheckBox ID="uxWatch" runat="server" meta:resourcekey="uxWatchResource1" />
                        </div>
                        <div class="inline-block">
                            <label>
                                <as:Literal ID="uxMultiWatchList" runat="server" Text="N/A" meta:resourcekey="uxMultiWatchListResource1" />
                            </label>
                        </div>
                    </td>
                    <td class="heading">
                        <as:Literal runat="server" ID="uxHierarchy5" meta:resourcekey="uxHierarchy5Resource1" />
                    </td>
                    <td>
                        <as:Literal ID="uxHierarchyValue5" runat="server" meta:resourcekey="uxHierarchyValue5Resource1" />&nbsp;
                    </td>
                </tr>
                <tr runat="server" id="pnlMarketData" class="AltRow">
                    <td class="heading">
                        <as:Literal runat="server" ID="Literal2" Text="Market Data" meta:resourcekey="Literal2Resource1" />
                    </td>
                    <td colspan="3">
                        <as:Literal ID="uxMarketData" runat="server" meta:resourcekey="uxMarketDataResource1" />&nbsp;
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr runat="server" id="pnlRelationshipManager" class="Row">
                    <td class="heading">
                        <as:Literal runat="server" ID="Literal1" Text="Relationship Manager" meta:resourcekey="Literal1Resource1" />
                    </td>
                    <td colspan="7">
                        <as:Literal ID="uxRelationshipManager" runat="server" meta:resourcekey="uxRelationshipManagerResource1" />&nbsp;
                    </td>
                </tr>
                <%--TK25937 – FIS - Merchant Profile/ Risk Report Enhancement--%>
                <tr runat="server" id="pnlEmail">
                    <td class="heading"><as:Literal ID="Literal14" runat="server" Text="Email" meta:resourcekey="Literal14Resource1"></as:Literal>
                    </td>
                    <td colspan="7">
                        <as:Literal ID="uxEmail" runat="server" meta:resourcekey="uxEmailResource1" />
                    </td>
                </tr>
                <%--End TK25937 – FIS - Merchant Profile/ Risk Report Enhancement--%>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 action-container text-right">
            <as:Button ID="uxSave" runat="server" Text="Save Merchant Info" CssClass="btn btn-default"
                OnClick="uxSave_OnClick" IsStandardButton="False" meta:resourcekey="uxSaveResource1" />
        </div>
    </div>
</div>
