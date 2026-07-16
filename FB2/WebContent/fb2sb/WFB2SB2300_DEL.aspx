<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2300_Del.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2300_Del" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_CHG_AMT_B {
            text-align: right;
            background-color: white;
            border-width: 0;
            color: black;
        }
    </style>
    <title></title>



    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();

        });


        function iniForm() {
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $(".time").mask('9999/99/99');
            $(".number").mask('9999/99/99');
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ClearAll() {
            $('#ddl_SYS_ID').val(-1);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2990400_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="14%" />
                                <col width="30%" />
                                <col width="12%" />
                                <col width="44%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_DATA_YM" runat="server" MaxLength="10" Width="123px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_SALARY_ID" runat="server" MaxLength="10" Width="120px" ClientIDMode="Static"></asp:Label>
                                        <asp:HiddenField ID="hid_SALARY_ID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbll_EMP_ID" runat="server" MaxLength="5" Width="120px" ClientIDMode="Static"></asp:Label>
                                        <asp:HiddenField ID="hid_EMP_ID" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_EMP_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbll_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbll_CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_chg_status%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_CHG_STATUS" runat="server" Text="D-刪除"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbll_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_PROCESS_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text="N-未核定"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbll_SALARY_STATUS" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_AMT_B" runat="server" Text="<%$Resources:Resource,wfb2sb_CHG_AMT%>"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_CHG_AMT_B" runat="server" ClientIDMode="Static" Enabled="false" MaxLength="5" Width="64px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sb_REMARK%>"></asp:Label>
                                        : 
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="150" Width="421px" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="CREATED_BY" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CREATED_BY%>"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbl_CREATED_BY" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="CREATED_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CREATED_DT%>"></asp:Label>
                                        : 
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbl_CREATED_DT" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="hid_SEQ_NO_B" runat="server" />
                                    </td>

                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2300Ok3" runat="server" OnClick="WFB2SB2300Ok3_Click" OnClientClick="return confirm($('#hidwfb299_Del_ConfirmMessage').val());" Text="<%$Resources:Resource,btn_confirm%>" />
                                            <%--<asp:Button ID="WFB2SB2300Ok3" runat="server" OnClick="WFB2SB2300Ok3_Click" OnClientClick="return confirm($('#hidwfb299_Del_ConfirmMessage').val());" Text="<%$Resources:Resource,btn_confirm%>" />--%>
                                            <asp:Button ID="WFB2SB2300Cancel" runat="server" OnClick="WFB2SB2300Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" Text="<%$Resources:Resource,btn_Cancel%>" type="button" />
                                        </div>
                                    </td>
                                </tr>
                </tr>
            </table>

            <!--2990400_QRY結束-->


            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />


            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


