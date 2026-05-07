<%@ Page Language="C#" AutoEventWireup="true"  %>

 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

 

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title></title>

    <style type="text/css">

   

    tbody.cellStyle > tr > td

    {

      background-color:White;

      border:Solid 1px Green;

      padding:5px 8px 5px 8px;

      font-family:Verdana;

      font-size:10px;

      text-align:left;

      vertical-align:middle;

      height:20px;

      overflow:hidden;

      cursor:default;

    }

   

    </style>

</head>

<body>

    <form id="form1" runat="server">

 

      <div style="width:370px">

            <table style="table-layout:fixed;background-color:Gray;width:100%;" border="0" cellpadding="0" cellspacing="0">

                  <tbody class="cellStyle">

                        <tr><td>One</td><td>One</td></tr>

                        <tr><td>Two</td><td>Two</td></tr>

                        <tr><td>Three</td><td>Three</td></tr>

                  </tbody>

            </table>

      </div>

    </form>

</body>

</html>