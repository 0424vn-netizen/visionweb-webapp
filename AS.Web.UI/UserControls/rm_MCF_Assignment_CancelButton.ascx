<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rm_MCF_Assignment_CancelButton.ascx.cs" Inherits="UserControls_rm_MCF_Assignment_CancelButton" %>

<as:Button ID="btnCancelAssignment" runat="server" OnClientClick="onCancel()" OnClick="btnCancelAssignment_Click" Text="Cancel" CssClass="btn btn-default" IsStandardButton="False" meta:resourcekey="btnCancelAssignmentResource1" />
<script type="text/javascript">
    function onCancel() {
        if (typeof (Risk_Assignment_Info_uxAssignmentName) != 'undefined') {
            var assignmentName = $('#' + Risk_Assignment_Info_uxAssignmentName).val();
            assignmentName = assignmentName.replace('&#', '');
            $('#' + Risk_Assignment_Info_uxAssignmentName).val(assignmentName);
          
            if (ValidateSpecialCharacters() == false)
                $('#' + Risk_Assignment_Info_uxAssignmentName).val("");
        }
    }
  
</script>