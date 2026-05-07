<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPagePopup.master" ValidateRequest="false" meta:resourcekey="PageResource1" AutoEventWireup="true" CodeFile="rm_MCF_MerchantClassificationConfiguration_ViewModal.aspx.cs" Inherits="rm_MCF_MerchantClassificationConfiguration_ViewModal" %>

<asp:content id="Content1" contentplaceholderid="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-12">
                <h2 class="modal-title mb-1x">
                    <asp:Literal ID="litTitle" runat="server" Text="Classification" meta:resourcekey="uxTitle"></asp:Literal>
                </h2>
            </div>
        </div>
        <div class="row">
            <!-- Classification  !-->
            <div class="col-md-12 RadGrid RadGrid_Default mt-m-1x">
                <table class="rgMasterTable rgClipCells  w-100">
                    <colgroup>
                        <col />
                        <col style="width: 55%" />     
                        <col />                 
                         <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <th class="rgHeader"></th>
                        <th class="rgHeader"></th>
                        <th class="rgHeader"></th>
                        <th class="rgHeader"></th>
                    </tr>
                    <tr class="rgRow">
                       <td class="text-default-gray font-12 font-weight-bold">
                             <asp:Literal runat="server" Text="Classification Name:"  meta:resourcekey="uxresourceClassificationName" />
                        </td>
                        <td>
                            <%= BindValue("MerchantClassificationName") %>
                        </td>
                        <td class="text-default-gray font-12 font-weight-bold">
                              <asp:Literal runat="server" Text="Multiplier:" meta:resourcekey="uxresourceMultiplier" />
                        </td>
                        <td class="text-left">
                            <%= BindValue("Multiplier") %>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- End of Classification  !-->
        </div>

        <!-- Attributes!-->
        <div class="row">
            <div class="height-24"></div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <h2 class="modal-title">
                    <asp:Literal ID="litAttribute" runat="server" meta:resourcekey="uxAttribute" Text="Assign Attribute(s)"></asp:Literal>
                </h2>
            </div>
        </div>
        <!-- Attribute list !-->
        <div class="row">
            <div class="col-md-12">
                <as:ASGrid ID="uxAttributeGrid" runat="server" AllowPaging="false" GridLines="None" ShowReportTotal="true" OnNeedDataSource="uxAttributeGrid_NeedDataSource"
                    AllowSorting="True" AutoGenerateColumns="False"  CssClass="in" meta:resourcekey="uxParameterListResource1">
                    <MasterTableView DataKeyNames="RecordID">
                        <Columns>
                            <as:ASGridBoundColumn UniqueName="AttributeName" HeaderText="Attribute Name" DataField="AttributeName"
                                ASFormat="DynamicString" HeaderTooltip="Attribute Name" meta:resourcekey="ASGridBoundColumnResourceAttributeName">
                                <ColumnValidationSettings>
                                </ColumnValidationSettings>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            </as:ASGridBoundColumn> 
                            <as:ASGridBoundColumn UniqueName="FromValue" HeaderText="From" ASFormat="Integer"
                                DataField="FromValue" ItemStyle-HorizontalAlign="Right"
                                HeaderTooltip="From" meta:resourcekey="ASGridBoundColumnResourceFrom">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="ToValue" HeaderText="To" ASFormat="Integer"
                                DataField="ToValue" ItemStyle-HorizontalAlign="Right"
                                HeaderTooltip="To" meta:resourcekey="ASGridBoundColumnResourceTo">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </as:ASGridBoundColumn>

                            <as:ASGridBoundColumn UniqueName="Operand" HeaderText="Operand"
                                DataField="OperandDisplay" ItemStyle-HorizontalAlign="Right"
                                HeaderTooltip="Operand" meta:resourcekey="ASGridBoundColumnResourceOperand">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>

                            </as:ASGridBoundColumn>
                            <as:ASGridBoundColumn UniqueName="MetricValue" HeaderText="Metric"
                                DataField="MetricDescription" meta:resourcekey="ASGridBoundColumnResourceMetric">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                 <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </as:ASGridBoundColumn> 
                        </Columns>
                    </MasterTableView>
                </as:ASGrid>
            </div>
        </div>
        <div class="height-6"></div>
        <div class="row">
            <div class="col-md-12 text-right form-action-container">
                <asp:Button ID="uxCancel" OnClientClick="return parent.HidePopupModal();" Text="Close" class="btn btn-default" runat="server" meta:resourcekey="uxCloseResource"></asp:Button>
            </div>
        </div>
    </as:ASModalContainer>
</asp:content>
