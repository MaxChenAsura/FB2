<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0200_Upload.aspx.cs" Inherits="WebContent_fb2hb_WFB2SG0200_Upload" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function checkConfirm() {
            history.back(-2);
        }

        $(function () {

            $.unblockUI();
        });
        function iniForm() {
            $.unblockUI();
        }

        //檔案匯入檢驗
        function checkUpload(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupB")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }

            BlockUI();
            processed = confirm("確定要進行" + msg);
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
            <col width="10%" />
            <col width="30%" />
            <col width="10%" />
            <col width="30%" />
            <col width="20%" />
        </colgroup>
        <tbody>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txt_FESTIVAL_TYPE_DESC" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txt_EMP_CD_DESC" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt%>"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txt_FESTIVAL_DT" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>"></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txt_FESTIVAL_PAY_DT" runat="server" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th></th>
                <td align="right" colspan="5">
                    <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_back%>" OnClick="btn_back_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
        </tbody>
    </table>


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
                    <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UploadExcel%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="MandatoryField" Width="600px" />
                </td>

                <td align="right" class="Body_label">
                    <aces:Btn ID="WFB2SG0202ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_import%>" OnClick="WFB2SG0202ExcelImport_Click"  OnClientClick="return checkUpload(this.value);" />
                    <%--<asp:Button ID="WFB2SG0202ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_import%>" OnClick="WFB2SG0202ExcelImport_Click"  OnClientClick="return checkUpload(this.value);"/>--%>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                        ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                        ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>
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
                            <asp:Label ID="lb_TemplateDownload" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TemplateDownload%>"></asp:Label>

                        </legend>
                        <table align="right">
                            <tr>
                                <td>
                                    <p style="color: red;">
                                        <asp:Label ID="lb_type" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_type%>"></asp:Label>:

                                    </p>
                                </td>
                                <td>
                                    <aces:Btn ID="WFB2SG0202ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_exceldown%>" OnClick="WFB2SG0201ExcelDown_Click" />
                                    <%--<asp:Button ID="WFB2SG0202ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_exceldown%>" OnClick="WFB2SG0201ExcelDown_Click" />--%>
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
    <!-- SG020_Qry的查詢條件 -->
    <asp:HiddenField ID="HID_Qry_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_Qry_MP_CD" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_Qry_FESTIVAL_DT" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_Qry_FESTIVAL_PAY_DT" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="HID_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_EMP_CD" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_TARGET_GEN_DT" runat="server" ClientIDMode="Static" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
</asp:Content>
