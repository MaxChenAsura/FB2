<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0100_EXPORT.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0100_EXPORT" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

            $("#txt_START_DATE").mask("9999/99/99");
            $("#txt_END_DATE").mask("9999/99/99");
            $.unblockUI();
        }

        function CheckYEAR(source, arguments) {
            var re = /^[0-9]+$/;
            if ($("#txt_YEAR").val().trim() != "") {
                if (!re.test($("#txt_YEAR").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }

        function MyCheckValid(msg) {
            var month = "";
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                $.unblockUI();
            } else {
                processed = false;
                return processed;
            }

            BlockUI();
            processed = confirm("確定要進行" + msg+"?");
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <colgroup>
            <col width="15%" />
            <col width="55%" />
            <col width="30%" />
        </colgroup>
        <tbody>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <%--輪值表--%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_WORK_SHIFT_CD%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:DropDownList ID="ddl_WORK_SHIFT_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_WORK_SHIFT_CD2%>"
                        ControlToValidate="ddl_WORK_SHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <%--日期區間--%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DATE" runat="server" Text="<%$Resources:Resource,wfb2db_lb_DATE%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label" colspan="3">
                    <asp:TextBox ID="txt_START_DATE" runat="server" Width="80px" CssClass="date MandatoryField" ClientIDMode="Static"></asp:TextBox>
                    ~
                    <asp:TextBox ID="txt_END_DATE" runat="server" Width="80px" CssClass="date MandatoryField" ClientIDMode="Static"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_START_DATE%>"
                        ControlToValidate="txt_START_DATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_END_DATE%>"
                        ControlToValidate="txt_END_DATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidatorSTART_DATE" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2db_ERR_START_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_START_DATE" ValidationGroup="GroupA" Display="None">
                    </asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidatorEND_DATE" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2db_ERR_END_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_END_DATE" ValidationGroup="GroupA" Display="None">
                    </asp:CustomValidator>

                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DATE"
                        ControlToValidate="txt_END_DATE" ErrorMessage="<%$Resources:Resource,wfb2db_ERR_DATE_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td align="right" class="Body_label">
                    <aces:Btn ID="WFB2DB0100EXCEL" runat="server" Text="<%$Resources:Resource,btn_excelexport%>" OnClick="WFB2DB0100EXCEL_Click" OnClientClick="return MyCheckValid(this.value);" />
                    <asp:Button ID="WFB2DB0100Previous" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                </td>
            </tr>
      </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

</asp:Content>

