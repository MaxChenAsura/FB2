<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA1200_Open.aspx.cs" Inherits="WebContent_WFB2IA1200_Open" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask('9999/99');
            $(".number2").mask('999');
            $(".number3").mask('99.9');
            $(".amt").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $(".number").mask('9999999'); //輸入時，不需要有千分位符號
            $(".number").css("text-align", "right");
        }

        function CheckIsZero(source, arguments) {
            var value = $.trim(arguments.Value);
            if (parseInt(value) == 0) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }
        function OpenRemarkSearch(obj_id, obj_name) {
            //OpenSearch('LICENSE_ID_Search.aspx', obj_id, obj_name, "IDENTITY_KIND=" + $("#ddl_SUB_DESC_IDENTITY").val() + "&EMP_ID=" + $("#txt_NEW_EMP_ID").val() + "&LICENSE_ID=" + $("#txt_NEW_LICENSE_ID").val(), 'Y');
            OpenSearch('CommCode_Search.aspx', obj_id, obj_name, 'MAIN_CD=TRACED_MEMO&SYS_CD=IA','Y');
        }
        function CommCode_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                if ($("#HID_ins_type").val() == 'A') {                    
                    $("#" + obj_desc).val('勞保' + obj.DESC);
                }
                if ($("#HID_ins_type").val() == 'B') {                   
                    $("#" + obj_desc).val('健保' + obj.DESC);
                }
                if ($("#HID_ins_type").val() == 'C') {
                    $("#" + obj_desc).val('勞退' + obj.DESC);
                }
            
               
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="550" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="20%" />
                                <col width="20%" />
                                <col width="20%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FAMILY_LICENSE_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FAMILY_NAME" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_FAMILY_NAME" runat="server" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SALARY_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_YM2%>"
                                            ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_SALARY_YM%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TRACE_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_TRACE_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_TRACE_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_TRACE_TYPE%>"
                                            ControlToValidate="ddl_TRACE_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TRACE_AMT" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_TRACE_AMT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TRACE_AMT" runat="server" MaxLength="7" CssClass="MandatoryField number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_TRACE_AMT%>"
                                            ControlToValidate="txt_TRACE_AMT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorTRACE_AMT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_TRACE_AMT_ZERO%>" ClientValidationFunction="CheckIsZero" ForeColor="Red"
                                            ControlToValidate="txt_TRACE_AMT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorTRACE_AMT2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_ERR_TRACE_AMT_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TRACE_AMT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_REMARK_DESC%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="300" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_NEW_REMARK" type="button" value="..." onclick="OpenRemarkSearch('txt_REMARK', 'txt_REMARK');" />
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<aces:Btn ID="WFB2IA1206Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1206Save%>" OnClick="WFB2IA1206Save_Click" ValidationGroup="GroupA" />--%>
                                            
                                            <asp:Button ID="WFB2IA1206Save" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1206Save%>" OnClick="WFB2IA1206Save_Click" ValidationGroup="GroupA" />
                                            
                                            <asp:Button ID="WFB2IA1206Cancel" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_back%>" OnClick="WFB2IA1206Cancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_ins_type" runat="server" ClientIDMode="Static" />   
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

