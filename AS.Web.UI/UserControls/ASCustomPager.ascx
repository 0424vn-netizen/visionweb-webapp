<%@ Control Language="C#" AutoEventWireup="true" Inherits="AS.Controls.Global.CustomPager" %>
<%--
This is the default template for current framework pager:
    . All controls(if present) must have exactly ID and Type (as declared below) to work as expected
    . You can remove any of below controls
          * However, the text box is required if we have the RangeValidator (as it will be the target for the validator)
    . See the link below to add more custom by your own (the html code below will be replaced for the code between PagerTemplate tag of parent grid):
    http://demos.telerik.com/aspnet-ajax/grid/examples/programming/pagertemplate/defaultcs.aspx
For more information about implement please take a look at AS.Framework 3.0 : AS.Controls\Grid
--%>

<div class="row">
<div class="col-xs-3">
    <span class="rgPageText"><asp:Literal ID="Literal1" runat="server" Text="Page:" meta:resourcekey="LiteralResource1"/></span>
    <asp:Button runat="server" ID="pagerPrevButton" Text="Previous" Enabled="<%# ParentGrid.CurrentPageIndex > 0  ? true : false %>" CssClass="rgPagePrev" CausesValidation="False" meta:resourcekey="pagerPrevButtonResource1" />
    <asp:LinkButton runat="server" ID="pagerPrevGroup" Text="..." CssClass="rgPageNum" OnCommand="pagerGoToGroup_Command" meta:resourcekey="pagerPrevGroupResource1"/>
    <asp:Repeater runat="server" ID="pagerList">
        <ItemTemplate>
            <asp:LinkButton runat="server" ID="pagerPrevButton" Text='<%# Container.DataItem %>' CssClass='<%# ParentGrid.CurrentPageIndex+1==(int)Container.DataItem?"rgPageNum rgCurrentPage":"rgPageNum" %>' OnCommand="pagerGo_Command" />                    
        </ItemTemplate>
    </asp:Repeater>
    <asp:LinkButton runat="server" ID="pagerNextGroup" Text="..." CssClass="rgPageNum" OnCommand="pagerGoToGroup_Command"/>
    <asp:Button runat="server" ID="pagerNextButton" Text="Next" CssClass="rgPageNext" meta:resourcekey="pagerNextButtonResource1" />
    <asp:LinkButton runat="server" ID="pagerGoToButton" Text="Go" CssClass="rgPageGo" Visible="False" meta:resourcekey="pagerGoToButtonResource1" />
    <asp:TextBox runat="server" ID="pagerUserProvideTextBox" CssClass="rgPageBox" Visible="False" />
    <asp:RangeValidator runat="server" ID="pagerRangeValidator" ControlToValidate="pagerUserProvideTextBox" meta:resourcekey="pagerRangeValidatorResource1" />
</div>
<div class="col-xs-6 text-center">
    <span class="rgPageText"><asp:Literal runat="server" ID="pagerInformationLabel" meta:resourcekey="pagerInformationLabelResource1" /></span>
</div>

<div class="col-xs-3 text-right">
    <span class="rgPageText"><asp:Literal ID="Literal2" runat="server" Text="Results per page:" meta:resourcekey="LiteralResource2"/></span>
    <as:ASRadComboBox runat="server" ID="pagerPageSizeComboBox"  CssClass="rgPageComboBox"/>
</div>
</div>
