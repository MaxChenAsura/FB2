<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0400_Upload.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0400_Upload" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function checkConfirm() {

            history.back(-2);
        }
        //上傳檢查
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
            processed = confirm($("#hidconfirm_Execute").val() + msg);
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="55%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_UploadExcel" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UploadExcel%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:FileUpload ID="FileUpload1" runat="server" class="MandatoryField" Width="600px" />
                            <%-- 檢查附檔名只能全大寫或全小寫的xls,xlsx --%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                                ErrorMessage="<%$Resources:Resource,wfb2_format_excel%>" ValidationExpression="^([a-zA-Z].*|[1-9].*)\.(xls|XLS|xlsx|XLSX)$" Display="None"></asp:RegularExpressionValidator>
                            <%-- 路徑是否為空白 --%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupB"
                                ErrorMessage="<%$Resources:Resource,wfb2_required_excel%>" Display="None"></asp:RequiredFieldValidator>
                        </td>

                        <td align="right" class="Body_label">
                            <aces:Btn ID="WFB2HB0401ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0401ExcelImport%>" OnClick="WFB2HB0401ExcelImport_Click" OnClientClick="return checkUpload(this.value);" />
                            <%--<asp:Button ID="WFB2HB0401ExcelImport" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0401ExcelImport%>" OnClick="WFB2HB0401ExcelImport_Click" OnClientClick="return checkUpload(this.value);" />--%>
                            <asp:Button ID="btn_back" Text="<%$Resources:Resource,wfb2hb_btn_back%>" runat="server" OnClick="btn_back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br>
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
                                            <aces:Btn ID="WFB2HB0401ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0401ExcelDown%>" OnClick="WFB2HB0401ExcelDown_Click"  />
                                            <%--<asp:Button ID="WFB2HB0401ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0401ExcelDown%>" OnClick="WFB2HB0401ExcelDown_Click" OnClientClick="BlockUI();" />--%>
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidconfirm_Execute" Value="<%$Resources:Resource,confirm_Execute%>" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2HB0401ExcelDown" />
            <asp:PostBackTrigger ControlID="WFB2HB0401ExcelImport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
