<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0200_Mod.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0200_Mod" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();

        });
        function iniForm() {
            $.unblockUI();
            $("#txt_END_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_add_START_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            $('#txt_HEAD_EMP_NAME').attr("readonly", true);
            $('#txt_ACC_DEPT_NAME').attr("readonly", true);
            $(".ymd").mask("9999/99/99");
            $(".EnNum7").mask("AAAAAAA");
            $(".EnNum7").css("ime-mode", "disabled");
            $(".EnNum5").mask("AAAAA");
            $(".EnNum5").css("ime-mode", "disabled");
        }

        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;

            if (!processed)
                $.unblockUI();

            return processed;
        }
        function checkConfirm() {
            $("#hid_parentFuncID").val(parentFuncID);
            return confirm("是否確定取消?");
            //if (result)
            //    window.location.href("WFB2HA0200_Qry.aspx?parentFuncId=" + parentFuncID);
        }

        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            } else {
                $("#txt_HEAD_EMP_NAME").val("");
            }
        }

        function getACC_DEPT_Name(ACC_DEPT_id) {
            var txt = document.getElementById(ACC_DEPT_id);
            if (txt.value != "") {
                document.getElementById('hid_getACC_DEPT_Name').click();
                return false;
            } else {
                $("#txt_ACC_DEPT_NAME").val("");
            }
        }

        function openQry() {
            window.location.href("WFB2HA0200_Qry.aspx?parentFuncId=" + parentFuncID);
            return false;
        }

        function Check_DEPT_NO(source, arguments) {
            var dept_no = $("#txt_add_DEPT_NO").val();
            if (dept_no.length < 7)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="3" cellpadding="0" width="100%">
                <tr>
                    <td colspan="3">
                        <table cellspacing="1" cellpadding="1" width="100%">
                            <tr>
                                <td align="right">
                                    <table cellspacing="1" cellpadding="1" width="1020" border="0"
                                        class="Body_Label">
                                        <colgroup>
                                            <col width="100%" />
                                        </colgroup>
                                        <tbody>
                                            <tr>
                                                <td align="right" class="Body_label"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                        <colgroup>
                                            <col width="12%" />
                                            <col width="40%" />
                                            <col width="12%" />
                                            <col width="16%" />
                                            <col width="20%" />
                                        </colgroup>
                                        <tbody>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_NO%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                                    <asp:TextBox ID="txt_add_DEPT_NO" runat="server" MaxLength="7" ClientIDMode="Static" Width="64px" CssClass="EnNum7 MandatoryField"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="ValidDeptNo" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_DEPT_NO%>"
                                                        ControlToValidate="txt_add_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_DEPT_NO_Chinese%>" ControlToValidate="txt_add_DEPT_NO" ForeColor="Red"
                                                        ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_DEPT_NO_length7%>" ClientValidationFunction="Check_DEPT_NO" ForeColor="Red"
                                                        ControlToValidate="txt_add_DEPT_NO" ValidationGroup="GroupA" Display="None">
                                                    </asp:CustomValidator>
                                                </td>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEFAULT_PLANT%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_DEFAULT_PLANT" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                                    
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_DEFAULT_PLANT%>"
                                                        ControlToValidate="ddl_DEFAULT_PLANT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>                                                   
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_Edit_START_DT%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_START_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                                    <asp:TextBox ID="txt_add_START_DT" runat="server" Width="81px" MaxLength="10" ClientIDMode="Static" CssClass="ymd MandatoryField date"></asp:TextBox>                                                    
                                                    <asp:RequiredFieldValidator ID="ValidStartDt" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_START_DT%>"
                                                        ControlToValidate="txt_add_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cv_add_START_DT" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_add_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                </td>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_END_DT%>"></asp:Label>:

                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_END_DT" runat="server" Width="81px" MaxLength="10" ClientIDMode="Static" CssClass="ymd date"></asp:TextBox>
                                                    <asp:CustomValidator ID="cv_END_DT" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_END_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                                                    
                                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_END_DT%>"
                                                        ControlToValidate="txt_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>--%>
                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT"
                                                        ControlToValidate="txt_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ha_Start_End_Date%>" Type="Date" Operator="GreaterThanEqual"
                                                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_add_START_DT"
                                                        ControlToValidate="txt_END_DT" ErrorMessage="<%$Resources:Resource,wfb2ha_Start_End_Date%>" Type="Date" Operator="GreaterThanEqual"
                                                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>

                            <tr>
                                <td align="left">
                                    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                        <colgroup>
                                            <col width="12%" />
                                            <col width="40%" />
                                            <col width="12%" />
                                            <col width="16%" />
                                            <col width="20%" />
                                        </colgroup>
                                        <tbody>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_NAME%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="60" ClientIDMode="Static" CssClass="MandatoryField" Width="300px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_DEPT_NAME%>"
                                                        ControlToValidate="txt_DEPT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>

                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_SNAME" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_SNAME%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_DEPT_SNAME" runat="server" MaxLength="60" ClientIDMode="Static" CssClass="MandatoryField" Width="300px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_SNAME%>"
                                                        ControlToValidate="txt_DEPT_SNAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>

                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_ENAME" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_ENAME%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_DEPT_ENAME" runat="server" MaxLength="60" ClientIDMode="Static" Width="300px"></asp:TextBox>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>

                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_HEAD_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_HEAD_EMP_ID%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_HEAD_EMP_ID" runat="server" MaxLength="5" Width="64px" CssClass="EnNum5 MandatoryField" ClientIDMode="Static" onblur="javascript:getEmpName(this.id)"></asp:TextBox>
                                                    <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_HEAD_EMP_ID', 'txt_HEAD_EMP_NAME', 'N');" />
                                                    <asp:TextBox ID="txt_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_HEAD_EMP_ID%>"
                                                        ControlToValidate="txt_HEAD_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>

                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_LEVEL" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_DEPT_LEVEL" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_LEVEL%>"
                                                        ControlToValidate="ddl_DEPT_LEVEL" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ORG_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ORG_TYPE%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_ORG_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_ORG_TYPE%>"
                                                        ControlToValidate="ddl_ORG_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>


                            <tr>
                                <td align="left">

                                    <table cellspacing="1" cellpadding="1" width="1020" border="0"
                                        class="Body_Label">
                                        <colgroup>
                                            <col width="12%" />
                                            <col width="88%" />
                                        </colgroup>
                                        <tbody>
                                            <%-- <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_DEPT_WS_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_WS_TYPE%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_DEPT_WS_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_DEPT_WS_TYPE%>"
                                                ControlToValidate="ddl_DEPT_WS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>--%>
                                            <%--<tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_ACC_SALARY_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_SALARY_CD%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_ACC_SALARY_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_ACC_SALARY_CD%>"
                                                ControlToValidate="ddl_ACC_SALARY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>--%>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_CD%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_ACC_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_ACC_CD%>"
                                                        ControlToValidate="ddl_ACC_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ACC_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_DEPT_NO%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label" colspan="7">
                                                    <asp:TextBox ID="txt_ACC_DEPT_NO" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="EnNum5 MandatoryField" onblur="javascript:getACC_DEPT_Name(this.id)"></asp:TextBox>
                                                    <input id="bt_ACC_DEPT_NO" type="button" value="..." onclick="OpenSearch('DeptAcc_Search.aspx', 'txt_ACC_DEPT_NO', 'txt_ACC_DEPT_NAME', '');" />
                                                    <asp:TextBox ID="txt_ACC_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_ACC_DEPT_NO%>"
                                                        ControlToValidate="txt_ACC_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_REMARK%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="60" ClientIDMode="Static" Width="600px"></asp:TextBox>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>
                                            <tr>
                                                <th></th>
                                                <td class="Body_label" style="text-align: right" colspan="4">
                                                    <aces:Btn ID="WFB2HA0200Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Save%>" OnClick="WFB2HA0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA"  />
                                                     <aces:Btn ID="WFB2HA0101Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Save%>" OnClick="WFB2HA0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA"  />

                                                    <%--<asp:Button ID="WFB2HA0200Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0200Save%>" OnClick="WFB2HA0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                                                    <asp:Button ID="btn_back" Text="<%$Resources:Resource,wfb2ha_btn_back%>" OnClick="btn_back_Click" OnClientClick="return checkConfirm();" runat="server" />
                                                     <%--<input id="btn_back" type="button" value="<%$Resources:Resource,wfb2ha_btn_back%>" onclick="checkConfirm();" runat="server" />--%>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <asp:Button ID="hid_getACC_DEPT_Name" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getACC_DEPT_Name_Click" />
            <asp:HiddenField ID="hid_parentFuncID" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
