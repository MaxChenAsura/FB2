<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE0200_Upload.aspx.cs" Inherits="WebContent_fb2se_WFB2SE0200_Upload" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $.unblockUI();
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
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_EFFECT_YM%>"></asp:Label>:
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="6" Width="64px" CssClass="ym MandatoryField date" ClientIDMode="Static" Text=""></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_EFFECT_YM%>"
                        ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                        ErrorMessage="<%$Resources:Resource,wfb2se_error_EFFECT_YM%>" ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA"
                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
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
                    <aces:Btn ID="WFB2SE0200LoadAdd" runat="server" Text="<%$Resources:Resource,wfb2se_WFB2SE0200LoadAdd%>" OnClick="WFB2SE0200LoadAdd_Click" OnClientClick="return checkDowning(this.value);" />
                    <aces:Btn ID="WFB2SE0200Upload" runat="server" Text="<%$Resources:Resource,wfb2da_lb_Upload%>" OnClick="WFB2SE0200Upload_Click" OnClientClick="return MyCheckValid(this.value);" />
                    <asp:Button ID="WFB2SE0200Previous" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
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
                                    <asp:Button ID="WFB2DA0400DownLoadExcel" runat="server" Text="<%$Resources:Resource,lb_DownLoad%>" OnClick="btn_Excel_Down_Click" />
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

