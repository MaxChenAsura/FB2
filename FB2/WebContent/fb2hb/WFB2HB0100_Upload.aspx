<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_Upload.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0100_Upload" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        //檔案匯入檢驗
        function MyCheckValid(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
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
                <td>
                    <br/>
                </td>  
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UploadExcel%>"></asp:Label>:
                </th>
                <td align="left" class="Body_label">
                    <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="600px" />
                </td>

                <td align="right" class="Body_label">
                    <aces:Btn ID="WFB2HB0101ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0101ExcelImport%>" OnClick="WFB2HB0101ExcelImport_Click" OnClientClick="return MyCheckValid(this.value);" />
                    <%--<asp:Button ID="WFB2HB0101ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0101ExcelImport%>" OnClick="WFB2HB0101ExcelImport_Click" />--%>
                    <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2dl_btn_back%>" OnClick="btn_back_Click" />

                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA"
                        ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA"
                        ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <br/>
                </td>
            </tr>
            <tr>
                <td>
                    <br/>
                </td>
            </tr>
            <tr>
                <td>
                    <br/>
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
                                    <aces:Btn ID="WFB2HB0101ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0101ExcelDown%>" OnClick="WFB2HB0101ExcelDown_Click" />
                                    <%--<asp:Button ID="WFB2HB0101ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0101ExcelDown%>" OnClick="WFB2HB0101ExcelDown_Click" />--%>
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
