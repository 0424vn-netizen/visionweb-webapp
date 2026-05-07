<%@ Page Title="Add/Modify Merchant Chain" Language="C#" MasterPageFile="~/MasterPagePopup.master" AutoEventWireup="true" 
    CodeFile="ModifyMerchantSecondaryAccessChainModal.aspx.cs" Inherits="ModifyMerchantSecondaryAccessChainModal" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" Runat="Server">
    <as:ASModalContainer ID="ASModalContainer1" runat="server" Width="530px" ContainerCssClass="container" WidthCssClass="modal-xxl">
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <p>
                        <as:Literal ID="uxLtrDetails" runat="server" meta:resourcekey="uxLtrDetailsResource1"></as:Literal><br />
                        <as:PlaceHolder ID="uxPlcAdd" runat="server" Visible="false">
                            <as:Literal ID="ltToAssignMerchant" runat="server" Text="To assign this Merchant to a new Secondary Access Chain, select from the list below." meta:resourcekey="ltToAssignMerchantResource1"></as:Literal><br />
                            <as:Literal ID="ltToDeleteMerchant" runat="server" Text="To delete this Merchant from the Secondary Access Chain, leave the Secondary Access Chain ID blank." meta:resourcekey="ltToDeleteMerchantResource1"></as:Literal>
                        </as:PlaceHolder>
                    </p>
                    <div class="form-inline">
                        <div class="control-inline last valign-top">
                            <as:ValidatorLabel ID="ValidatorLabel1" runat="server" ApplyFor="uxChain" Text="Secondary Access Chain ID:" meta:resourcekey="ValidatorLabel1Resource1"></as:ValidatorLabel>
                        </div>
                        <div class="inline-block">
                            <as:RadComboBox ID="uxChain" runat="server" DataTextField="ChainAccessNumber" DataValueField="ChainAccessNumber" EnableLoadOnDemand="true"
                                MarkFirstMatch="false" AppendDataBoundItems="true"
                                AllowCustomText="false" EmptyMessage="Please enter at least 3 digits" OnItemsRequested="uxChain_OnItemsRequested"
                                MaxLength="100" Width="285px" meta:resourcekey="uxChainResource1">
                                <Items>
                                    <as:RadComboBoxItem meta:resourcekey="RadComboBoxItemResource1" />
                                </Items>
                            </as:RadComboBox>
                            <as:Button ID="uxSubmit" runat="server" Text="Submit" OnClick="Save_Click" OnClientClick="return validator();"
                                CssClass="btn btn-default valign-top" meta:resourcekey="uxSubmitResource1" />
                            <div class="bottom-error">
                                <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxChain"></as:ValidatorMessage>
                            </div>
                        </div>
                    </div>
                </div>
            </div> 
        </div>
        <as:Validator ID="Validator1" runat="server" ValidationFunction="dovalidation" MessageType="Inline" meta:resourcekey="ValidatorResource1">
            <Items>
                <as:CustomValidationItem ClientValidationFunction="ValidateData" ControlToValidateID="uxChain" Message="You have entered an invalid Secondary Access Chain ID. Please try again." />
            </Items>
        </as:Validator>
    </as:ASModalContainer>
    <as:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <script type="text/javascript">
            var ModifyMerchantChainModal_uxChain = '<%= uxChain.ClientID %>';
            var ModifyMerchantChainModal_EmptyMessage = '<%= uxChain.EmptyMessage  %>';
        </script>
        <script type="text/javascript" src="<%=ResolveUrl("~") %>res/js/ModifyMerchantSecondaryAccessChainModal.js"></script>
    </as:RadCodeBlock>
</asp:Content>

