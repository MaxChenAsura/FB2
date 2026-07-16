<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL1100_ImportExcel.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL1100_ImportExcel" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $('#txt_DATA_YM_search').mask('9999');
            $('#txt_COMPANY_NAME_search').attr("readonly", true);
            $.unblockUI();

            $("#txt_COMPANY_CD_search").change(function () {
                $("#txt_COMPANY_CD_search").val($("#txt_COMPANY_CD_search").val().toUpperCase());
            });
        }
        function confirmImportAfter() {
            if (confirm($('#hidwfb2sl_confirmImport').val())) {
                $('#btn_confirmImport').click();
            }
        }
        function MyCheckValid() {
            setTimeout("MydelayedShow()", 1);
        }

        function MydelayedShow() {
            if (Page_IsValid != null && Page_IsValid == true) {
                $.unblockUI();
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="50%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--公司別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_COMPANY_CD_search%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD_search" runat="server" MaxLength="1" Width="50px" class="MandatoryField"
                                            OnTextChanged="txt_COMPANY_CD_search_TextChanged" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_COMPANY_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD_search', 'txt_COMPANY_NAME_search', '', '');" />
                                        <asp:TextBox ID="txt_COMPANY_NAME_search" runat="server" BorderWidth="0" MaxLength="60" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_COMPANY_CD_search_isNull%>"
                                            ControlToValidate="txt_COMPANY_CD_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM_search" runat="server" MaxLength="4" Width="50px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isNull%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isError%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <%--資料格式--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_DATA_FORMAT_search" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddl_DATA_FORMAT" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="A" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_A%>"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_D%>"></asp:ListItem>
                                            <asp:ListItem Value="V" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_V%>"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_isNull%>"
                                            ControlToValidate="ddl_DATA_FORMAT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_isNull%>"
                                            ControlToValidate="ddl_DATA_FORMAT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--上傳檔案--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_Excel_Upload_all" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_Excel_Upload%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="MandatoryField" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_Excel_Upload_isNull%>"
                                            ControlToValidate="FileUpload1" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr style="padding-bottom: 30px;">
                                    <td align="right" class="Body_label" colspan="4">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SL1100Download" runat="server" Text="<%$Resources:Resource,wfb2sl_download_example%>" OnClick="WFB2SL1100Download_Click" ValidationGroup="GroupB" OnClientClick="MyCheckValid();" />
                                            <aces:Btn ID="WFB2SL110Process" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL110Process%>" OnClick="WFB2SL110Process_Click" ValidationGroup="GroupA" OnClientClick="MyCheckValid();" />
                                           <%-- <asp:Button ID="WFB2SL1100Download" runat="server" Text="<%$Resources:Resource,wfb2sl_download_example%>" OnClick="WFB2SL1100Download_Click" ValidationGroup="GroupB" OnClientClick="MyCheckValid();" />
                                            <asp:Button ID="WFB2SL110Process" runat="server" Text="<%$Resources:Resource,wfb2sl_WFB2SL110Process%>" OnClick="WFB2SL110Process_Click" ValidationGroup="GroupA" OnClientClick="MyCheckValid();" />--%>
                                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_back%>" OnClick="btn_back_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>


                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lb_import_error" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader" colspan="4">
                                    <asp:Label ID="lb_description" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT2%>"></asp:Label>:
                                </th>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_descriptionA" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_A%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_descriptionD" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_D%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Body_label">
                                    <asp:Label ID="lb_descriptionV" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_FORMAT_V%>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_file_path" runat="server" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sl_confirmImport" Value="<%$Resources:Resource,wfb2sl_confirmImport%>" />
            <asp:Button ID="btn_confirmImport" runat="server" OnClick="btn_confirmImport_Click" ClientIDMode="Static" Style="display: none" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SL1100Download" />
            <asp:PostBackTrigger ControlID="WFB2SL110Process" />
            <asp:PostBackTrigger ControlID="btn_confirmImport" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


