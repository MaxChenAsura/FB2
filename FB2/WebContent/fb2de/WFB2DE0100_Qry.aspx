<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2de/WFB2DE0100_Qry.aspx.cs" Inherits="WebContent_fb2de_WFB2DE0100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }
        function addzero(input) {
            if (input.value.length == 1) input.value = '0' + input.value;
            return input;
        }
       
        function saveCheck() {
            if (Page_ClientValidate("GroupA")) {
                if (confirm('是否確定修改')) {
                    CheckValid();

                }
                else {
                    return false;
                }

            } else {
                return false;
            }
             
            
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table class="Body_Label" width="1048px">
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_BF_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2de_lb_BF_AMOUNT%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label" style="TEXT-ALIGN: left">
                <asp:TextBox ID="txt_BF_AMOUNT" runat="server" MaxLength="6" Width="70" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField" ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
            </td>
            <td class="Body_Label" style="TEXT-ALIGN: left">元
            </td>
            <asp:RequiredFieldValidator ID="Validator_BF_AMOUNT_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_BF_AMOUNT_NotNull%>" ControlToValidate="txt_BF_AMOUNT"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_BF_AMOUNT_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_BF_AMOUNT_Range%>" ControlToValidate="txt_BF_AMOUNT"
                MinimumValue="1" MaximumValue="999999" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_DN_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2de_lb_DN_AMOUNT%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_DN_AMOUNT" runat="server" MaxLength="6" Width="70" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"  ClientIDMode="Static" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>
            </td>
            <td class="Body_Label" style="TEXT-ALIGN: left">元
            </td>
             <asp:RequiredFieldValidator ID="Validator_DN_AMOUNT_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_DN_AMOUNT_NotNull%>" ControlToValidate="txt_DN_AMOUNT"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_DN_AMOUNT_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_DN_AMOUNT_Range%>" ControlToValidate="txt_DN_AMOUNT"
                MinimumValue="1" MaximumValue="999999" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_BR_START" runat="server" Text="<%$Resources:Resource,wfb2de_lb_BR_START%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_BR_START" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_BR_START" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"   ClientIDMode="Static" onblur="return addzero(this)"  onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
             <asp:RequiredFieldValidator ID="Validator_BR_START_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_BR_START_NotNull%>" ControlToValidate="txt_BR_START"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_BR_START_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_BR_START_Range%>" ControlToValidate="txt_BR_START"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>

        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_BR_END" runat="server" Text="<%$Resources:Resource,wfb2de_lb_BR_END%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_BR_END" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_BR_END" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"  ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
           
              <asp:RequiredFieldValidator ID="Validator_BR_END_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_BR_END_NotNull%>" ControlToValidate="txt_BR_END"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_BR_END_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_BR_END_Range%>" ControlToValidate="txt_BR_END"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>

        </tr>

         <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_MD_START" runat="server" Text="<%$Resources:Resource,wfb2de_lb_MD_START%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_MD_START" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_MD_START" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"   ClientIDMode="Static" onblur="return addzero(this)"  onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
             <asp:RequiredFieldValidator ID="Validator_MD_START_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_MD_START_NotNull%>" ControlToValidate="txt_MD_START"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_MD_START_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_MD_START_Range%>" ControlToValidate="txt_MD_START"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
         </tr>

        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_MD_END" runat="server" Text="<%$Resources:Resource,wfb2de_lb_MD_END%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_MD_END" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_MD_END" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"  ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
           
              <asp:RequiredFieldValidator ID="Validator_MD_END_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_MD_END_NotNull%>" ControlToValidate="txt_MD_END"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_MD_END_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_MD_END_Range%>" ControlToValidate="txt_MD_END"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>

        </tr>

        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_DN_START" runat="server" Text="<%$Resources:Resource,wfb2de_lb_DN_START%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_DN_START" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_DN_START" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"  ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
              <asp:RequiredFieldValidator ID="Validator_DN_START_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_DN_START_NotNull%>" ControlToValidate="txt_DN_START"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_DN_START_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_DN_START_Range%>" ControlToValidate="txt_DN_START"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_DN_END" runat="server" Text="<%$Resources:Resource,wfb2de_lb_DN_END%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_DN_END" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_DN_END" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField" ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
              <asp:RequiredFieldValidator ID="Validator_DN_END_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_DN_END_NotNull%>" ControlToValidate="txt_DN_END"
                  ForeColor="Red" Display="None" ValidationGroup="GroupA" >
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_DN_END_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_DN_END_Range%>" ControlToValidate="txt_DN_END"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_LAST_BR_TIME" runat="server" Text="<%$Resources:Resource,wfb2de_lb_LAST_BR_TIME%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_LAST_BR_TIME" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_LAST_BR_TIME" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"  ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText(); " onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
              <asp:RequiredFieldValidator ID="Validator_LAST_BR_TIME_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_LAST_BR_TIME_NotNull%>" ControlToValidate="txt_LAST_BR_TIME"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_LAST_BR_TIME_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_LAST_BR_TIME_Range%>" ControlToValidate="txt_LAST_BR_TIME"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_LAST_DN_TIME" runat="server" Text="<%$Resources:Resource,wfb2de_lb_LAST_DN_TIME%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_LAST_DN_TIME" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_LAST_DN_TIME" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled" CssClass="MandatoryField"  ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
              <asp:RequiredFieldValidator ID="Validator_LAST_DN_TIME_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_LAST_DN_TIME_NotNull%>" ControlToValidate="txt_LAST_DN_TIME"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_LAST_DN_TIME_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_LAST_DN_TIME_Range%>" ControlToValidate="txt_LAST_DN_TIME"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <td class="Body_Label" style="TEXT-ALIGN: left; background-color: #808080; width: 250px; height: 30px">
                <asp:Label ID="lb_COURSE_DN_TIME" runat="server" Text="<%$Resources:Resource,wfb2de_lb_COURSE_DN_TIME%>" ForeColor="White"></asp:Label>
            </td>
            <td class="Body_Label">
                <asp:DropDownList ID="ddl_COURSE_DN_TIME" runat="server" Width="70px" CssClass="MandatoryField"></asp:DropDownList>
            </td>
            <td class="Body_Label">
                <asp:TextBox ID="txt_COURSE_DN_TIME" runat="server" MaxLength="2" Width="30" Style="text-align: right;ime-mode:disabled; margin-top: 0px;" CssClass="MandatoryField" ClientIDMode="Static" onblur="return addzero(this)" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>(HH:MM)
            </td>
              <asp:RequiredFieldValidator ID="Validator_COURSE_DN_TIME_NotNull" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_COURSE_DN_TIME_NotNull%>" ControlToValidate="txt_COURSE_DN_TIME"
                ForeColor="Red" Display="None" ValidationGroup="GroupA">
            </asp:RequiredFieldValidator>
            <asp:RangeValidator ID="Validator_COURSE_DN_TIME_Range" runat="server" ErrorMessage="<%$Resources:Resource,wfb2de_COURSE_DN_TIME_Range%>" ControlToValidate="txt_COURSE_DN_TIME"
                MinimumValue="00" MaximumValue="59" ValidationGroup="GroupA" Display="None" Type="Integer">
            </asp:RangeValidator>
        </tr>
        <tr>
            <th></th>
            <th style="width: 70px"></th>
            <th style="width: 150px"></th>

            <td class="Body_Label" style="TEXT-ALIGN: right">
                <aces:Btn ID="WFB2DE0100Save" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0100Save%>" OnClientClick="return saveCheck();" ValidationGroup="GroupA" OnClick="WFB2DE0100Save_Click"  />

                <%--
                    <aces:Btn ID="WFB2DE0100Save" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0100Save%>" OnClientClick="return saveCheck();" ValidationGroup="GroupA" OnClick="WFB2DE0100Save_Click"  />
                    <asp:Button ID="WFB2DE0100Save" runat="server" Text="<%$Resources:Resource,wfb2de_WFB2DE0100Save%>" OnClientClick="return saveCheck();" ValidationGroup="GroupA" OnClick="WFB2DE0100Save_Click"  />
                    --%>
            </td>

        </tr>
    </table>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

</asp:Content>

