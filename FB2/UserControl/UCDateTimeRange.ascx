<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/UserControl/UCDateTimeRange.ascx.cs" Inherits="UserControl_UCDateTimeRange" %>
<script type="text/javascript">
    //jQuery(document).ready(function () {
    //    $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
    //    $('#txt_LEAVE_DT_S').mask('9999/99/99');
    //    $('#txt_LEAVE_DT_E').mask('9999/99/99');
    //});
    //function MaskFor
</script>

<asp:TextBox ID="txt_LEAVE_DT_S" runat="server" Width="81px" CssClass="date"></asp:TextBox>
~  
<asp:TextBox ID="txt_LEAVE_DT_E" runat="server" Width="81px" CssClass="date"></asp:TextBox>
<asp:RegularExpressionValidator ControlToValidate="txt_LEAVE_DT_S" ID="Validator_DT_S" Enabled="false" ForeColor="Red" runat="server" Display="None" />
<asp:RegularExpressionValidator ControlToValidate="txt_LEAVE_DT_E" ID="Validator_DT_E" Enabled="false" ForeColor="Red" runat="server" Display="None" />
<asp:CustomValidator ID="CusV_txt_LEAVE_DT_S" Enabled="false" ForeColor="Red" runat="server" Display="None" ValidateEmptyText="true"></asp:CustomValidator>
<asp:CustomValidator ID="CusV_txt_LEAVE_DT_E" Enabled="false" ForeColor="Red" runat="server" Display="None" ValidateEmptyText="true"></asp:CustomValidator>


<asp:CompareValidator ID="CompareValidator1" ControlToCompare="txt_LEAVE_DT_S" ControlToValidate="txt_LEAVE_DT_E" Enabled="false" runat="server" Display="None" />