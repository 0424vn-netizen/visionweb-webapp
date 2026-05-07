<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_MerchantClassification_Attribute.ascx.cs" Inherits="UserControls_rm_MCF_MerchantClassification_Attribute" %>

<as:ASGrid ID="uxAttributeDataGrid" runat="server" ShowPageTotal="False" FooterStyle-CssClass="freeze-table-footer" InsertTempColumnAtTheEnd="False" OnNeedDataSource="uxAttributeDataGrid_NeedDataSource"
    AutoGenerateColumns="False" ASPagingMethod="None" AllowExportAtWebServices="False" GridName="Attribute List" OnItemDataBound="uxAttributeDataGrid_ItemDataBound" OnItemCommand="uxAttributeDataGrid_ItemCommand"
    ClientSettings-ClientEvents-OnRowDeleted="AdjustModalSize()">

    <MasterTableView DataKeyNames="RecordID">
        <Columns>
            <as:ASGridTemplateColumn UniqueName="RecordID" DataField="RecordID" HeaderStyle-Width="0px" Visible="false"></as:ASGridTemplateColumn>
            <as:ASGridTemplateColumn UniqueName="AttributeName" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" DataField="AttributeName" HeaderText="Attribute Name" meta:resourcekey="ASGridTemplateColumnResource1" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <as:RadComboBox ID="uxAttributeList" runat="server" EnableEmbeddedBaseStylesheet="False" Filter="Contains" MarkFirstMatch="true"
                        DataValueField="AttributeID" DataTextField="AttributeName" Width="100%" EmptyMessage='<%# GetLocalResourceObject("Attribute.SelectText") %>' OnSelectedIndexChanged="uxAttributeList_SelectedIndexChanged" OnItemDataBound="uxAttributeList_ItemDataBound" AutoPostBack="true" HeaderStyle-HorizontalAlign="Center">
                    </as:RadComboBox>
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage runat="server" ID="uxAttributeListMsg" ApplyFor="uxAttributeList" Message="" ShowOnLoad="False" />
                    </div>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridTemplateColumn UniqueName="FromValue" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" DataField="FromValue" HeaderText="From" meta:resourcekey="ASGridTemplateColumnResource2" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <as:TextBox ID="uxAttributeFrom" runat="server" MaxLength="9" CssClass="form-control classToValidate" Width="100%" onkeypress="return InputOnTextBox(event)" onblur="checkInputCharacter(this);" />
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="ValidatorMessage1" runat="server" ApplyFor="uxAttributeFrom" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="text-number"></ItemStyle>
            </as:ASGridTemplateColumn>
            <as:ASGridTemplateColumn HeaderText="To" UniqueName="ToValue" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" DataField="ToValue" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource3">
                <ItemTemplate>
                    <as:TextBox ID="uxAttributeTo" runat="server" MaxLength="9" CssClass="form-control" Width="100%" onkeypress="return InputOnTextBox(event)" onblur="checkInputCharacter(this);" />
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="ValidatorMessage2" runat="server" ApplyFor="uxAttributeTo" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                    <as:HiddenField ID="hddMinValueFromTo" runat="server" />
                    <as:HiddenField ID="hddMaxValueFromTo" runat="server" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" CssClass="text-number"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridTemplateColumn HeaderText="Operand" UniqueName="Operand" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" DataField="Operand" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource4">
                <ItemTemplate>
                    <as:RadComboBox ID="uxOperand" runat="server" EnableEmbeddedBaseStylesheet="False"
                        DataValueField="Value" DataTextField="Text" Width="100%" Filter="Contains" MarkFirstMatch="true"
                        MaxLength="100" meta:resourcekey="uxOperand" EmptyMessage='<%# GetLocalResourceObject("uxOperand.Text") %>' AutoPostBack="true" OnSelectedIndexChanged="operand_SelectedIndexChanged">
                    </as:RadComboBox>
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="ValidatorMessage3" runat="server" ApplyFor="uxOperand" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridTemplateColumn HeaderText="Metric" UniqueName="Metric" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="220px" DataField="MetricValue" HeaderStyle-HorizontalAlign="Center" meta:resourcekey="ASGridTemplateColumnResource5">
                <ItemTemplate>
                    <as:RadComboBox ID="uxMetric" runat="server" EnableEmbeddedBaseStylesheet="False" Filter="Contains" MarkFirstMatch="true"
                        DataValueField="Value" DataTextField="Text" Width="100%"  OnClientBlur="MetricClassificationOnBlur" OnClientItemChecked="MetricClassificationItemCheck"
                        MaxLength="100" meta:resourcekey="uxMetric" EmptyMessage='<%# GetLocalResourceObject("uxMetric.Text") %>'>
                    </as:RadComboBox>
                    <asp:PlaceHolder runat="server" ID="uxMetricTextList_Panel" Visible="false">
                        <asp:TextBox ID="uxMetricTextList" runat="server" Width="75%" onkeypress="return false;" onblur="checkInputCharacter(this);" />
                        <asp:HiddenField runat="server" ID="uxMetricListTextHide" />
                        <a href="#" runat="server" id="btnlinkFindOwner" class="btn-link" onclick="return ShowPopupModal('FindOwners.aspx', 'auto'); return false;">
                            <asp:Literal ID="Literal2" runat="server" Text="Find" meta:resourcekey="LiteralResourceFind" /></a>
                    </asp:PlaceHolder>
                    <div class="flex-box">
                        <span></span>
                        <asp:Label ID="uxLabelMetricFrom" Visible="false" runat="server" Text="From" CssClass="ml-2x pr-4" meta:resourcekey="LabelResourceFrom" />
                        <as:TextBox ID="uxMetricFrom" Visible="false" runat="server" MaxLength="9" CssClass="form-control classToValidate" onkeypress="return InputOnTextBox(event)" onblur="checkInputCharacter(this);" />
                        <asp:Label ID="uxLabelMetricTo" Visible="false" runat="server" Text="To" meta:resourcekey="LabelResourceTo" CssClass="ml-2x pr-4" />
                        <as:TextBox ID="uxMetricTo" Visible="false" runat="server" MaxLength="9" CssClass="form-control classToValidate" onkeypress="return InputOnTextBox(event)" onblur="checkInputCharacter(this);" />
                        <as:HiddenField ID="hddMinValueMetric" runat="server" />
                        <as:HiddenField ID="hddMaxValueMetric" runat="server" />
                    </div>


                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="ValidatorMessage4" runat="server" ApplyFor="uxMetric" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                    <div class="bottom-error text-left">
                        <as:ValidatorMessage ID="ValidatorMessage5" runat="server" ApplyFor="uxMetricTextList" Message="" ShowOnLoad="False"></as:ValidatorMessage>
                    </div>
                    <div class="flex-box">
                        <div style="width: 40px"></div>
                        <div class="bottom-error text-left" style="width: 85px">
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage8" ApplyFor="uxMetricFrom" Message="" ShowOnLoad="False" />
                        </div>
                        <div style="width: 22px"></div>
                        <div class="bottom-error text-left" style="width: 85px">
                            <as:ValidatorMessage runat="server" ID="ValidatorMessage9" ApplyFor="uxMetricTo" Message="" ShowOnLoad="False" />
                        </div>
                    </div>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top"></ItemStyle>
            </as:ASGridTemplateColumn>

            <as:ASGridTemplateColumn UniqueName="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                <ItemTemplate>
                    <div class="mt-1x">
                        <as:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" OnClientClick="return ConfirmDelete();" meta:resourcekey="DeleteResource" />
                    </div>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top"></ItemStyle>
            </as:ASGridTemplateColumn>
        </Columns>
    </MasterTableView>

</as:ASGrid>

<tek:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var client_Grid_Attribute_Id = "<%= uxAttributeDataGrid.ClientID %>";
        var messageConfirm = "<%= GetLocalResourceObject("confirmDelete.Text") %>";
        var messageRequired = "<%= GetGlobalResourceObject("MessageManager","Field_Require") %>";
        var messageOnlyNumber = "<%= GetGlobalResourceObject("MessageManager","ValidationMessages_RestrictionPasswordGreaterOrEqualThanField") %>";
        var messageDuplidate = "<%= GetLocalResourceObject("AttributeDuplicate.Message") %>";
        var messageFromNumber = "<%= GetLocalResourceObject("FromValueNumber.Message") %>"
        var messageToNumber = "<%= GetLocalResourceObject("ToValueNumber.Message") %>"
        var messageToMustGreaterThanFrom = '<%= GetGlobalResourceObject("MessageManager", "RiskParameter_js_ToMustBeGreaterThanFrom1") %>';
        var msgitemsSelected = '<%= GetLocalResourceObject("LiteralResourceFindMoreitem").ToString() %>';

        var messageFrom = "<%= GetLocalResourceObject("ASGridTemplateColumnResource2.HeaderText") %>";
        var messageTo = "<%= GetLocalResourceObject("ASGridTemplateColumnResource3.HeaderText") %>";
        var messageOperand = "<%= GetLocalResourceObject("ASGridTemplateColumnResource4.HeaderText") %>";
        var messageMetric = "<%= GetLocalResourceObject("ASGridTemplateColumnResource5.HeaderText") %>";
        var messageMetric_From = "<%= GetLocalResourceObject("Msg_MetricFromBetween.Message") %>";
        var messageMetric_3 = "<%= GetLocalResourceObject("Msg_MetricRequired_Between.Message") %>";
        var messageMetric_To = "<%= GetLocalResourceObject("Msg_MetricToBetween.Message") %>";
        var messageToMustGreaterThanorEqualFrom = '<%= GetGlobalResourceObject("MessageManager", "RiskParameter_js_ToMustBeGreaterThanFrom") %>';

    </script>
    <script src="<%= ResolveUrl("~/")%>res/js/risk_MCF/rm_MCF_MerchantClassification_Attribute_Modal.js"></script>
</tek:RadCodeBlock>
