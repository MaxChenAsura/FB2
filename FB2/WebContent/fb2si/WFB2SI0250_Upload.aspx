<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2si/WFB2SI0250_Upload.aspx.cs" Inherits="WebContent_fb2si_WFB2SI0250_Upload" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $("#txt_YEAR").mask("9999");
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
            processed = confirm("確定要進行" + msg +'?');
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
                <%--年度--%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_year%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:TextBox ID="txt_YEAR" Width="80px" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_YEAR%>"
                        ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                    </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2da_YEAR_isError%>" ClientValidationFunction="CheckYEAR" ForeColor="Red"
                        ControlToValidate="txt_YEAR" ValidationGroup="GroupA" Display="None">
                    </asp:CustomValidator>

                </td>
            </tr>
            <tr>
                <%--EXCEL上傳檔案 --%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,UploadExcel%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="600px" />
                </td>

                <td align="right" class="Body_label">
                 
                    <aces:Btn ID="WFB2SI0250Upload" runat="server" Text="<%$Resources:Resource,wfb2da_lb_Upload%>" OnClick="WFB2SI0250Upload_Click" OnClientClick="return MyCheckValid(this.value);" />
                  
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_lb_UploadExcel_isNull%>"
                        ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA"
                        ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left" class="Body_label" colspan="5">

                    <fieldset style="padding: 5px">
                        <legend class="Body_label">
                            <asp:Label ID="lb_TemplateDownload" runat="server" Text="<%$Resources:Resource,TemplateDownload%>"></asp:Label>
                        </legend>
                        <table align="right">
                            <tr>
                                <td>
                                    <asp:Button ID="WFB2SI0250DownLoadExcel" runat="server" Text="<%$Resources:Resource,lb_DownLoad%>" OnClick="btn_Excel_Down_Click" />
                                </td>
                            </tr>
                        </table>

                    </fieldset>
                </td>
            </tr>
            <tr>
                <td align="left" class="Body_label"></td>
                <td align="left" class="Body_label"></td>
            </tr>
        </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>

