<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0100_Gen.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0100_Gen" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

        function MyCheckValid(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {

                $.unblockUI();
            } else {
                processed = false;
                return processed;
            }

            BlockUI();
            processed = confirm("確定要進行 預設行事曆生成 ?(原已存在的資料會覆蓋)");
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <colgroup>
            <col width="15%" />
            <col width="55%" />
            <col width="30%" />
        </colgroup>
        <tbody>
            <tr>
                <%--日期區間--%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DATE" runat="server" Text="<%$Resources:Resource,wfb2da_lb_DATE%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label" colspan="3">
                    <asp:TextBox ID="txt_START_DATE" runat="server" Width="80px" CssClass="date MandatoryField" ClientIDMode="Static"></asp:TextBox>
                    ~
                    <asp:TextBox ID="txt_END_DATE" runat="server" Width="80px" CssClass="date MandatoryField" ClientIDMode="Static"></asp:TextBox>

                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_START_DATE%>"
                        ControlToValidate="txt_START_DATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_END_DATE%>"
                        ControlToValidate="txt_END_DATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidatorSTART_DATE" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2da_ERR_START_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_START_DATE" ValidationGroup="GroupA" Display="None">
                    </asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidatorEND_DATE" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2da_ERR_END_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_END_DATE" ValidationGroup="GroupA" Display="None">
                    </asp:CustomValidator>

                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DATE"
                        ControlToValidate="txt_END_DATE" ErrorMessage="<%$Resources:Resource,wfb2da_ERR_DATE_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <%--行事曆代碼--%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:DropDownList ID="ddl_CALENDAR_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_CALENDAR_CD%>"
                        ControlToValidate="ddl_CALENDAR_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                </td>
            </tr>

            <tr>
                <th></th>
                <td align="right" class="Body_label">
                    <aces:Btn ID="WFB2DA0100GEN" runat="server" Text="<%$Resources:Resource,wfb2da_GEN%>" OnClick="WFB2DA0100GEN_Click" OnClientClick="return MyCheckValid(this.value);" />

                    <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                </td>
            </tr>
        </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

</asp:Content>

