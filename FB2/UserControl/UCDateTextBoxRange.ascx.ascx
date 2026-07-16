<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/UserControl/UCDateTextBoxRange.ascx.cs" Inherits="UserControl_UCDateTextBoxRange" %>
<script type="text/javascript">
    function DateTimeMask(mask) {
        $('#txt_Year_DT_S').mask(mask);
        $('#txt_Year_DT_E').mask(mask);
    }
    function YearStartChange() {
        $('#hid_Year_DT_S').val($('#txt_Year_DT_S').val()) ;
        if($('#hid_Year_DT_S').val().length == 4)
            $('#hid_Year_DT_S').val($('#hid_Year_DT_S').val() + "/01/01");
        if($('#hid_Year_DT_S').val().length == 7)
            $('#hid_Year_DT_S').val($('#hid_Year_DT_S').val() + "/01");
    }
    function YearEndChange() {
        $('#hid_Year_DT_E').val($('#txt_Year_DT_E').val());
        if ($('#hid_Year_DT_E').val().length == 4)
            $('#hid_Year_DT_E').val($('#hid_Year_DT_E').val() + "/01/01");
        if ($('#hid_Year_DT_E').val().length == 7)
            $('#hid_Year_DT_E').val($('#hid_Year_DT_E').val() + "/01");
    }
</script>

<asp:TextBox ID="txt_Year_DT_S" runat="server" Width="81px" ></asp:TextBox>
~  
<asp:TextBox ID="txt_Year_DT_E" runat="server" Width="81px" ></asp:TextBox>
<asp:TextBox ID="hid_Year_DT_S" runat="server" style="display:none;" />
<asp:TextBox ID="hid_Year_DT_E" runat="server" style="display:none;" />
<asp:RegularExpressionValidator ID="Validator_Year_DT_S" runat="server"
    ControlToValidate="txt_Year_DT_S" ForeColor="Red"
    Display="None" >
</asp:RegularExpressionValidator>
<asp:RegularExpressionValidator ID="Validator_Year_DT_E" runat="server"
    ControlToValidate="txt_Year_DT_E" ForeColor="Red"
    Display="None"  >
</asp:RegularExpressionValidator>
<asp:CompareValidator ID="CompareValidator_Year" runat="server" ControlToCompare="hid_Year_DT_S"
    ControlToValidate="hid_Year_DT_E" Type="Date" Operator="GreaterThan"  
    Display="None" >
</asp:CompareValidator>
