<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_AutoQueueDetail.ascx.cs" Inherits="UserControls_rm_AutoQueueDetail" %>

<as:PlaceHolder ID="aqView" runat="server">
    <div class="row col-padding-15">
        <div class="col-md-10">
            <h3 class="title-auto-queue font-weight-bold mt-m-1x mb-5x ellipsis"><%=AutoQueueItem.Name.DoVeraCode()%></h3>
        </div>
        <div class="col-md-2 text-right">
            <button class="btn btn-default" onclick="<%=string.Format("return aqModule.OpenAutoQueueModal('{0}',true)",AqEditLink.DoVeraCode()) %>">
                <as:Literal ID="Literal8" runat="server" meta:resourcekey="btnEditAutoQueue"></as:Literal>
            </button>
        </div>
    </div>
    <div class="row col-padding-15">
        <div class="col-md-12">
            <label class="font-weight-bold" runat="server">
                <as:Literal ID="Literal5" runat="server" meta:resourcekey="lblLastRun"></as:Literal>
            </label>
            <span>
                <span id="queueStatus"><%=LastRun.DoVeraCode()%></span>
                <span id="queueicon"></span>
                <span id="aqStatusCode" class="display-none"><%=AqStatusCode.DoVeraCode()%></span>
            </span>
        </div>
        <div class="col-md-12 mt-2x">
            <label runat="server" class="font-weight-bold">
                <as:Literal ID="Literal6" runat="server" meta:resourcekey="lblCreateOn"></as:Literal>
            </label>
            <span><%=AutoQueueItem.CreateOn.DoVeraCode()%></span>
        </div>
    </div>
    <div class="mt-4x mb-3x">
        <%=AutoQueueItem.Description.DoVeraCode()%>
    </div>
    <div class="row col-padding-15">
        <div class="col-md-6">
            <h4 class="title-auto-queue">
                <as:Literal ID="Literal2" runat="server" meta:resourcekey="lblAssignments"></as:Literal>
            </h4>

            <div class="list-group">
                <as:ASRepeater runat="server" ID="AssignmentsList">
                    <ItemTemplate>
                        <div class="item">
                            <label>                               
                                <%# Eval("AssignmentName")%>
                            </label>
                        </div>
                    </ItemTemplate>
                </as:ASRepeater>
            </div>
        </div>

        <div class="col-md-6">
            <h4 class="title-auto-queue">
                <as:Literal ID="Literal1" runat="server" meta:resourcekey="lblWorkQueue"></as:Literal>
            </h4>
            <div class="list-group">
                <as:ASRepeater runat="server" ID="WorkQueueList">
                    <ItemTemplate>
                        <div class="item">
                            <label>                               
                                <span class="select-italic-item"><%# Eval("AssignmentName")%></span>
                            </label>
                        </div>
                    </ItemTemplate>
                </as:ASRepeater>
            </div>
        </div>
    </div>
</as:PlaceHolder>

<as:PlaceHolder ID="apLog" runat="server">
    <h4 class="title-auto-queue mt-8x">
        <as:Literal ID="Literal3" runat="server" meta:resourcekey="lblChangelog"></as:Literal>
    </h4>
    <div class="list-group-white">
        <as:ASRepeater runat="server" ID="ChangeLogGrid">
            <ItemTemplate>
                <div class="item">
                    <div class="row">
                        <div class="col-xs-7">
                            <span class="text-gray-light"><%# Eval("ActionTypeDesc")%></span>
                        </div>
                        <div class="col-xs-5 text-right">
                            <span><%# Rm_AutoQueueBusiness.ConvertDateTimeAQ(Eval("CreatedDTS").ToString())%></span>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </as:ASRepeater>
    </div>
    <div runat="server" id="uxChangeLog">
        <p>
            <a class="text-decoration-dotted" onclick="<%=string.Format("return aqModule.OpenAutoQueueModal('{0}')",AqAllChangeLog.DoVeraCode()) %>">
                <as:Literal ID="Literal7" runat="server" meta:resourcekey="lblViewAllChanges"></as:Literal>
            </a>
        </p>
    </div>

</as:PlaceHolder>

<as:ASRadCodeBlock ID="ASRadCodeBlock1" runat="server">
    <script src="<%= ResolveUrl("~/")%>res/js/risk/rm_AutoQueueDetail.js"></script>
    <script>
        var aqDetailModule = new AutoQueueDetailModule();
    </script>
</as:ASRadCodeBlock>

