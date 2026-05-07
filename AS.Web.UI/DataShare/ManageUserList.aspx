<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageUserList.aspx.cs"  MasterPageFile="~/MasterPagePopup.master" Inherits="ManageUserList" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <tek:RadAjaxManagerProxy runat="server" ID="uxRadAjaxManager1">
        <AjaxSettings>
            <tek:AjaxSetting AjaxControlID="uxUserSelector">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxUserSelector" />
                </UpdatedControls>
            </tek:AjaxSetting>
            <tek:AjaxSetting AjaxControlID="uxSave">
                <UpdatedControls>
                    <tek:AjaxUpdatedControl ControlID="uxSave" />
                </UpdatedControls>
            </tek:AjaxSetting>
        </AjaxSettings>
    </tek:RadAjaxManagerProxy>

<as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-md" ContainerCssClass="container" Width="" >
    <div id="PageContent">
        <div class="row">
            <div class="col-xs-12">
                <h3 class="modal-title"><asp:Literal runat="server" ID="uxTitle" meta:resourcekey="uxTitleResource1"></asp:Literal></h3>

                <as:MultiSelector ID="uxUserSelector" runat="server" DataValueField="UserID" DataTextField="UserName"
                    AddText=">" RemoveText="<" WidthSelector="251" HeightSelector="190" WidthButton="25" AllowUsingLINQ="true"
                    EmptyMessageLeft="Enter here to filter" CssClassTextBoxFilter=""
                    CSSAddAllButton="btn-to-right-all" CSSAddButton="btn-to-right" CSSRemoveAllButton="btn-to-left-all"
                    CSSRemoveButton="btn-to-left" ShowTooltip="false" CheckExisted="true" OnDoubleClick="uxSelector_DoubleClick" meta:resourcekey="uxUserSelectorResource1" />
            </div>
        </div>
            
        <div class="row">
            <div class="col-xs-12 form-action-container">
                <div class="inline-block">
                    <asp:Button ID="uxSave" runat="server"  OnClick="uxSave_Click" Text="Save" CssClass="btn btn-default" meta:resourcekey="uxSaveResource1"/>
                </div>
                <asp:Button ID="uxCancel" runat="server"  OnClientClick="parent.HidePopupModalChild(1);" Text="Cancel" CssClass="btn btn-default" meta:resourcekey="uxCancelResource1"/>
                </div>
            </div>
        </div>

    </as:ASModalContainer>
    <tek:RadCodeBlock runat="server" ID="DocumentCodeBlockGeneral">    
        <script type="text/javascript">
            function SaveAndExit() {
                parent.HidePopupModalChild(1);
                parent.refreshGrid();
            }
            
        </script>
    </tek:RadCodeBlock>
</asp:Content>

