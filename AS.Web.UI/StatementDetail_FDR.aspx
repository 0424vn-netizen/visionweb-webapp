<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatementDetail_FDR.aspx.cs" Inherits="StatementDetail_FDR" Title="" meta:resourcekey="PageResource1" %>

<%@ Register Src="~/UserControls/PageTitle.ascx" TagName="ReportTitle" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><as:Literal ID="ltSTATEMENTDETAIL" runat="server" Text="STATEMENT DETAIL" meta:resourcekey="ltSTATEMENTDETAILResource1"></as:Literal></title>
    <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
        <link href="<%= ResolveUrl("~/")%>res/css/bootstrap/bootstrap.min.css" rel="stylesheet" />
        <script src="<%= ResolveUrl("~/")%>res/js/jquery/jquery-3.6.0.min.js"></script>
        <script src="<%= ResolveUrl("~/")%>res/js/jquery/jquery-migrate-3.3.2.min.js"></script>
        <script src="<%= ResolveUrl("~/")%>res/js/common/modal.js"></script>
    </tek:RadCodeBlock>
    <style type="text/css">
        .showprintonly {
            display: none;
        }

        @media print {
            .showprintonly {
                display: block;
            }
        }

        .showviewonly {
            display: block;
        }

        @media print {
            .showviewonly {
                display: none;
            }
        }

        .gridtitle {
            font-family: Arial;
            font-size: 15px;
            color: Black;
            font-weight: bold;
        }
        .grid-title {
            cursor: text !important;
        }
        
        #divExport {
            margin-top: 24px;
        }
        .border {
            border: 1px solid #969696;
        }
    </style>
</head>
<body class="body-modal">
    <form id="form1" runat="server">
        <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
            <div class="row">
                <div class="col-md-9">
                    <h3 class="grid-title"><as:Literal ID="Literal1" runat="server" Text="STATEMENT" meta:resourcekey="Literal1Resource1"></as:Literal></h3>
                </div>
                <!--Export-->
                <div class="col-md-3 text-right">
                    <div class="hidden-print" runat="server" id="divExport">
                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/res/Images/printer.jpg" Width="24px" Height="24px"
                            runat="server" ToolTip="Printer Friendly Version" OnClientClick="window.print();" meta:resourcekey="ImageButton1Resource1"></asp:ImageButton>
                        <asp:ImageButton ID="ImageButton2" ImageUrl="~/res/Images/excel.gif" Width="24px" Height="24px"
                            runat="server" ToolTip="Export to Excel" OnClick="uxExportExcel_Click" meta:resourcekey="ImageButton2Resource1"></asp:ImageButton>
                        <asp:ImageButton ID="ImageButton3" ImageUrl="~/res/Images/pdf.gif" Width="24px" Height="24px"
                            runat="server" ToolTip="Export to PDF" OnClick="uxExportPDF_Click" meta:resourcekey="ImageButton3Resource1"></asp:ImageButton>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <table class="ASTable no-border">
                        <tr>
                            <td>
                                <span class="gridtitle"><as:Literal ID="Literal2" runat="server" Text="Merchant:" meta:resourcekey="Literal2Resource1"></as:Literal>&nbsp;</span>
                                <asp:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitleResource1"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span class="gridtitle"><as:Literal ID="Literal3" runat="server" Text="Report Period:" meta:resourcekey="Literal3Resource1"></as:Literal>&nbsp;</span>
                                <asp:Literal ID="uxTitle1" runat="server" meta:resourcekey="uxTitle1Resource1"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 in">
                    <%--this div use for view  #969696--%>

                    <div id="divViewContent" class="showviewonly border" runat="server" visible="true">
                        <asp:Literal ID="uxViewContent" runat="server" meta:resourcekey="uxViewContentResource1"></asp:Literal>
                    </div>

                    <%--this div use for print--%>
                    <div id="divContent" class="showprintonly border" runat="server" visible="true">
                        <div style="text-align: left;">
                            <asp:Literal ID="uxContentsSafari" runat="server" meta:resourcekey="uxContentsSafariResource1"></asp:Literal>
                        </div>
                    </div>
                    <%--this tbl use for export--%>
                    <table class="statementtbl" width="100%" align="center" id="tblContents" runat="server" visible="false">
                        <tr>
                            <td align="left" valign="top" colspan="11" class="PrintPaddingRight">
                                <span class="statement">
                                    <asp:Literal ID="uxContents" runat="server"></asp:Literal>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </as:ASModalContainer>
    </form>
    <tek:RadCodeBlock runat="server" ID="JVCodeBlock">
        <script type="text/javascript">
            setTimeout("AdjustModalSize();", 100);
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement != null && window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz az well)
                return oWindow;
            }
        </script>
    </tek:RadCodeBlock>
</body>
</html>
