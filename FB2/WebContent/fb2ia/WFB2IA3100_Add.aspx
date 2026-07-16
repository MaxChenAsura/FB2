<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA3100_Add.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA3100_Add" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".ym").mask('9999/99');
            $.unblockUI();
            $('#txt_COMPANY_NAME').attr("readonly", true);
            $('#txt_HEALTH_ORG_ID').attr("readonly", true);
            $('#txt_LABOR_ORG_ID').attr("readonly", true);
            $('#uploadpath').attr("readonly", true);
            $("#FileUpload1").val($("#hid_FILE_PATH").val());
            $('#FileUpload1').change(function () {
                $('#uploadpath').val($('#FileUpload1').val());
            });
            //公司代號取得公司名稱的ajax
            $("#txt_COMPANY_CD").change(function () {
                if ($("#txt_COMPANY_CD").val().length == 1) {
                    $.ajax({
                        url: "../comm/WFB2CompanyData.ashx",
                        data: {
                            COMPANY_CD: $('#txt_COMPANY_CD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_COMPANY_NAME').val("");
                                $('#txt_HEALTH_ORG_ID').val("");
                                $('#txt_LABOR_ORG_ID').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_COMPANY_NAME').val(JData.COMPANY_SNAME);
                                $('#txt_HEALTH_ORG_ID').val(JData.HEALTH_ORG_ID);
                                $('#txt_LABOR_ORG_ID').val(JData.LABOR_ORG_ID);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_COMPANY_NAME').val("");
                    $('#txt_HEALTH_ORG_ID').val("");
                    $('#txt_LABOR_ORG_ID').val("");
                }
            });
        }


        function ddlCheck(source, arguments) {
            if ($("#ddl_BILLS_KIND").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Company_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + id).val(obj.CD);
                $("#" + name).val(obj.DESC);
                __doPostBack('question', 'true');
            }
        }

        function Checkformat_xlsx(source, arguments) {
            if ($("#ddl_BILLS_KIND").val() == "A") {
                var re = /\.xlsx+$/;
                if (!re.test($("#uploadpath").val()))
                    arguments.IsValid = false;
                else {
                    arguments.IsValid = true;
                }

            }
            else
                arguments.IsValid = true;

        }
        function Checkformat_txt(source, arguments) {
            if ($("#ddl_BILLS_KIND").val() != "A") {
                var re = /\.(txt)+$/;
                if (!re.test($("#uploadpath").val()))
                    arguments.IsValid = false;
                else {
                    arguments.IsValid = true;
                }

            }
            else
                arguments.IsValid = true;
        }
        function upload(){
            $('#FileUpload1').click();
            $('#uploadpath').val($('#FileUpload1').val());
            return false;
        }
        //清空
        function doClear() {
            $("#ddl_BILLS_KIND").val("-1");
            $("#txt_COMPANY_CD").val("");
            $("#txt_COMPANY_NAME").val("");
            $("#txt_FEES_YM").val("");
            $("#txt_HEALTH_ORG_ID").val("");
            $("#uploadpath").val("");
            $("#txt_LABOR_ORG_ID").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="5%" />


                            </colgroup>
                            <tbody>

                                <tr>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="1" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" ></asp:TextBox>
                                        <input id="bt_COMPANY_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_NAME', '', 'Y');" />
                                        <asp:TextBox ID="txt_COMPANY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_COMPANY_CD%>"
                                            ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BILLS_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_BILLS_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_BILLS_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_required_BILLS_KIND%>" ClientValidationFunction="ddlCheck" ForeColor="Red"
                                            ControlToValidate="ddl_BILLS_KIND" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                   
                                </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FEES_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FEES_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_FEES_YM" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_FEES_YM%>"
                                            ControlToValidate="txt_FEES_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_FEES_YM%>" ControlToValidate="txt_FEES_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HEALTH_ORG_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HEALTH_ORG_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HEALTH_ORG_ID" runat="server" MaxLength="15" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_HEALTH_ORG_ID%>"
                                            ControlToValidate="txt_HEALTH_ORG_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FILE_PATH"  runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FILE_PATH%>"></asp:Label>:
                                    </th>
                                    <td style="border: hidden">
                                        <asp:TextBox ID="uploadpath" ClientIDMode="Static" runat="server" CssClass="MandatoryField" Style="position:fixed;margin-top:0px; z-index:1;" Width="290px"></asp:TextBox>
                                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="MandatoryField" Style=" z-index:1;" ClientIDMode="Static" Width="350px" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_FileUpload1%>"
                                            ControlToValidate="uploadpath" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="需使用xlsx檔" ClientValidationFunction="Checkformat_xlsx" ForeColor="Red"
                                            ControlToValidate="uploadpath" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="需使用txt檔" ClientValidationFunction="Checkformat_txt" ForeColor="Red"
                                            ControlToValidate="uploadpath" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LABOR_ORG_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_LABOR_ORG_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LABOR_ORG_ID" runat="server" MaxLength="15" Width="80px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_LABOR_ORG_ID%>"
                                            ControlToValidate="txt_LABOR_ORG_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <th></th>
                                    <th></th>
                                    <th></th>

                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA3101Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3101Process%>" ClientIDMode="Static"  OnClick="WFB2IA3101Process_Click" OnClientClick="CheckValid();"  ValidationGroup="GroupA" />

                                            <%--<asp:Button ID="WFB2IA3101Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3101Process%>" ClientIDMode="Static"  OnClick="WFB2IA3101Process_Click" OnClientClick="CheckValid();"  ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />
                                            <asp:Button ID="btn_backpage" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_backpage%>" OnClick="btn_backpage_Click" OnClientClick="BlockUI();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>

            </table>

            <asp:HiddenField ID="HID_COMPANY_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_FILE_PATH" runat="server" ClientIDMode="Static" />
            <!--紀錄頁數(跳頁用)-->
            <%--<asp:HiddenField ID="HID_BILLS_KIND" runat="server" ClientIDMode="Static" />--%>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->

            <%--<asp:HiddenField ID="hid_wfb2si_Announce_NotExecuteMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Announce_NotReleaseMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Announce_Message" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Announce_ReleaseMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Gen_dt_NotReleaseMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Release_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2si_Release_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA3101Process" />
            <%--<asp:PostBackTrigger ControlID="txt_COMPANY_CD" />--%>
            <%--<asp:PostBackTrigger ControlID="uploadbutton" />--%>
            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
