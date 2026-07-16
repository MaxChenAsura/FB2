<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0200_Dtl.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0200_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        function iniForm() {
        }
        function checkConfirm() {
            $("#hid_parentFuncID").val(parentFuncID);
            //window.location.href("WFB2HA0200_Qry.aspx?parentFuncId=" + parentFuncID);
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
                                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="6" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>

                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_START_DT%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_START_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>

                                                </td>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_END_DT%>"></asp:Label>:

                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_END_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" CssClass="date"></asp:TextBox>

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
                                            <col width="35%" />
                                            <col width="12%" />
                                            <col width="21%" />
                                            <col width="20%" />
                                        </colgroup>
                                        <tbody>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_NAME%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ReadOnly="true" CssClass="" Width="300px"></asp:TextBox>

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
                                                    <asp:TextBox ID="txt_DEPT_SNAME" runat="server" BorderWidth="0" ReadOnly="true" Width="300px"></asp:TextBox>
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
                                                    <asp:TextBox ID="txt_DEPT_ENAME" runat="server" BorderWidth="0" ReadOnly="true" Width="300px"></asp:TextBox>
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
                                                    <asp:TextBox ID="txt_HEAD_EMP_ID" runat="server" BorderWidth="0" ReadOnly="true" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                                    <%--<input id="bt_HEAD_EMP_ID" type="button" value="..." />--%>
                                                    <asp:TextBox ID="txt_HEAD_EMP_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>

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
                                                    <asp:DropDownList ID="ddl_DEPT_LEVEL" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ORG_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ORG_TYPE%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_ORG_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
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
                                            <%--<tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_DEPT_WS_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_WS_TYPE%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_DEPT_WS_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                </td>
                                            </tr>--%>
                                            <%-- <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ACC_SALARY_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_SALARY_CD%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_ACC_SALARY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_CD%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:DropDownList ID="ddl_ACC_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_ACC_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_ACC_DEPT_NO%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label" colspan="7">
                                                    <asp:TextBox ID="txt_ACC_DEPT_NO" runat="server" Width="64px" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                                    <%--<input id="bt_ACC_DEPT_NO" type="button" value="..." />--%>
                                                    <asp:TextBox ID="txt_ACC_DEPT_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_REMARK%>"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_REMARK" runat="server" BorderWidth="0" ReadOnly="true" Width="600px"></asp:TextBox>
                                                </td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                                <td align="left" class="Body_label"></td>
                                            </tr>
                                            <tr>
                                                <th></th>
                                                <td class="Body_label" style="text-align: right" colspan="4">

                                                    <asp:Button ID="btn_back" Text="<%$Resources:Resource,wfb2ha_btn_back2%>" OnClick="btn_back_Click" OnClientClick="checkConfirm();" runat="server" />
                                                     <%--<input id="btn_back" type="button" value="<%$Resources:Resource,wfb2ha_btn_back2%>" onclick="checkConfirm();" runat="server" />--%>
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
            <asp:HiddenField ID="hid_parentFuncID" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
