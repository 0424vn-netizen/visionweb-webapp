<%@ Page Title="STATEMENT DETAIL" Language="C#" MasterPageFile="~/MasterPagePopup.master"
    AutoEventWireup="true" CodeFile="StatementDetail_FIS.aspx.cs" Inherits="StatementsDetailReport"
    EnableEventValidation="false" meta:resourcekey="PageResource1" %>

<asp:Content ContentPlaceHolderID="head" runat="server" ID="Content1">
    <![if IE 7]>         
     <style type="text/css">
         .divTable {
             padding: 1.2% 2.5% 0% 2.5% !important;
         }
     </style>
    <![endif]>
      <![if !IE 7]>         
     <style type="text/css">
         .divTable {
             padding: 0% 2.5% 0% 2.5% !important;
         }
     </style>
    <![endif]>
 <![if !IE]>  
 <style type="text/css" media="print">
     .PrintPaddingRight {
         padding-right: 15px !important;
     }

     .PrintDiv {
         width: 650px !important;
         margin-right: 10px !important;
     }

     @media print {
         .PrintMode {
             font-size: xx-small;
         }
     }
 </style>
    <![endif]>
     <![if IE]>
      <style type="text/css" media="print">
          .PrintMode {
              font-size: x-small;
          }
      </style>
    <![endif]>
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
    </style>

    <style type="text/css">
        @media print {
            .noprint {
                display: none;
            }
        }

        .reporttitle {
            color: Black;
            font-family: Arial;
            font-size: 19px;
            font-weight: bold;
            text-align: left;
        }

        .gridtitle {
            color: Black;
            font-family: Arial;
            font-size: 15px;
            font-weight: bold;
        }

        .statementtbl {
            border: solid 1px #969696;
        }


        table.statementtbl td {
            padding: 1.5% 2.5% 1.5% 2.5%;
        }

        #wrapper {
            width: 98%;
            margin: 0 auto;
            text-align: center;
        }

        /* --Style for report-- */
        .reporttitle {
            font-family: Arial;
            font-size: 19px;
            color: Black;
            font-weight: bold;
            text-align: left;
        }

        .reportsubtitle {
            font-family: Arial;
            font-size: 16px;
            color: Black;
            font-weight: bold;
            text-align: left;
        }

        .gridtitle {
            font-family: Arial;
            font-size: 15px;
            color: Black;
            font-weight: bold;
        }

        #wrapper {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPage" runat="Server">
    <as:ASModalContainer ID="uxModalContainer" runat="server" WidthCssClass="modal-xxl" ContainerCssClass="container" Width="">
        <div class="row">
            <div class="col-md-9">
                <h3 class="grid-title"><as:Literal ID="ltSTATEMENT" runat="server" Text="STATEMENT" meta:resourcekey="ltSTATEMENTResource1"></as:Literal></h3>
            </div>
            <!--Export-->
            <div class="col-md-3 text-right">
                <div class="hide-for-print" runat="server" id="divExport">
                    <asp:ImageButton ID="ImageButton1" ImageUrl="~/res/Images/printer.jpg" Width="24px"
                        Height="24px" runat="server" ToolTip="Printer Friendly Version" OnClientClick="window.print();" meta:resourcekey="ImageButton1Resource1">
                    </asp:ImageButton>
                    <asp:ImageButton ID="uxExportExcel" ImageUrl="res/images/excel.gif" Width="24px"
                        Visible="false" Height="24px" runat="server" ToolTip="Export to Excel" OnClick="uxExportExcel_Click" meta:resourcekey="uxExportExcelResource1">
                    </asp:ImageButton>
                    <asp:ImageButton ID="uxExportPDF" ImageUrl="res/images/pdf.gif" Width="24px" Height="24px"
                        runat="server" ToolTip="Export to PDF" OnClick="uxExportPDF_Click" meta:resourcekey="uxExportPDFResource1"></asp:ImageButton>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 in">
                <table class="ASTable no-border">
                    <tr>
                        <td>
                            <span class="gridtitle"><as:Literal ID="Literal1" runat="server" Text="Merchant:" meta:resourcekey="Literal1Resource1"></as:Literal>&nbsp;</span>
                            <asp:Literal ID="uxTitle" runat="server" meta:resourcekey="uxTitleResource1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="gridtitle"><as:Literal ID="Literal2" runat="server" Text="Report Period:" meta:resourcekey="Literal2Resource1"></as:Literal>&nbsp;</span>
                            <asp:Literal ID="uxTitle1" runat="server" meta:resourcekey="uxTitle1Resource1"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="height-24"></div>
        <div class="row">
            <div class="col-md-12 in">
                <%--this div use for view  #969696--%>
                <div id="divViewContent" style="border: 1px solid #969696;" class="showviewonly"
                    runat="server" visible="true">
                    <asp:Literal ID="uxViewContent" runat="server" meta:resourcekey="uxViewContentResource1"></asp:Literal>
                </div>
                <%--this div use for print--%>
                <div id="divContent" style="border: 1px solid #969696; width: 95%;" class="divTable PrintDiv showprintonly"
                    runat="server" visible="true">
                    <asp:Literal ID="uxContentsSafari" runat="server" meta:resourcekey="uxContentsSafariResource1"></asp:Literal>
                </div>
                <%--this tbl use for export--%>
                <table class="ASTable" id="tblContents" runat="server"
                    visible="false">
                    <tr>
                        <td>
                            <asp:Literal ID="uxContents" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <tek:RadCodeBlock ID="uxRadCodeBlock" runat="server">
            <script type="text/javascript">
                function PrintPage() {
                    if (window.print)
                        try { setTimeout('window.print()', 500); } catch (ex) { }
                    else
                        alert('<%= GetLocalResourceObject("StatementDetail_FIS_aspx_AlertJavascript").ToString() %>');
                }
            </script>
        </tek:RadCodeBlock>
    </as:ASModalContainer>
</asp:Content>
