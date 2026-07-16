<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC4100_Detail1.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC4100_Detail1" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.mask.definitions['~'] =' [1 - 12]';
            $('#txtPayroll_YM').mask('9999/99');
            $('#lb_Payroll_YM_Value').mask('9999/99');
            $('#hid_parentFuncId').val(parentFuncID);
            //iniForm();
        });
        function iniForm() {
            $.unblockUI();
        }
        function Open(action) {
            var SALARY_TYPE = $('#hid_SALARY_TYPE').val();
            var SALARY_DT = $('#hid_SALARY_DT').val();
            var SALARY_YM = $('#hid_SALARY_YM').val();
            var EMP_ID = $('#hid_EMP_ID').val();
            var DATA_SDT = $('#hid_DATA_SDT').val();
            var DATA_EDT = $('#hid_DATA_EDT').val();
            if (action == "OOverDtl")
                window.open("../fb2di/WFB2DI0700_Qry.aspx?EMP_ID=" + EMP_ID + "&DATA_SDT=" + DATA_SDT + "&DATA_EDT=" + DATA_EDT + "&parentFuncId=" + parentFuncID, 'WFB2DI0700');
            if (action == "LeaveDtl")
                window.open("../fb2dh/WFB2DH0700_Qry.aspx?fn=FB2SC410&emp_id=" + EMP_ID + "&apply_leave_sdt=" + DATA_SDT + "&apply_leave_edt=" + DATA_EDT + "&parentFuncId=" + parentFuncID, 'WFB2DH0700')
            if (action == "ShiftDtl")
                window.open("../fb2dc/WFB2DC0800_Qry.aspx?EMP_ID=" + EMP_ID + "&DATA_SDT=" + DATA_SDT + "&DATA_EDT=" + DATA_EDT + "&parentFuncId=" + parentFuncID, 'WFB2DC0800')
            if (action == "InsDtl")
                window.open("../fb2ia/WFB2IA5200_Qry.aspx?EMP_ID=" + EMP_ID + "&SALARY_YM=" + SALARY_YM + "&parentFuncId=" + parentFuncID, 'WFB2IA5200')
            //if (action == "EnvDtl")
            //    window.Open("WebContent/fb2dj/WFB2DJ0400_Qry.aspx?EMP_ID=" + EMP_ID + "&SALARY_DT=" + SALARY_DT + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=1020px;dialogHeight=700px;scroll=auto')
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader" width="15%">
                                    <asp:Label runat="server" ID="lb_Payroll_YM" Text="<%$Resources:Resource,wfb2sc_Payroll_YM%>" ClientIDMode="Static" />
                                </th>
                                <td colspan="5">
                                    <asp:Label ID="lb_Payroll_YM_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader" width="15%">
                                    <asp:Label runat="server" ID="lb_EMP_NAME" Text="<%$Resources:Resource,wfb2sc_EMP_NAME%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_EMP_NAME_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader" width="15%">
                                    <asp:Label runat="server" ID="lb_EMP_NO" Text="<%$Resources:Resource,wfb2sc_EMP_NO%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_EMP_NO_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader" width="15%">
                                    <asp:Label runat="server" ID="lb_ON_DEPT" Text="<%$Resources:Resource,wfb2sc_ON_DEPT%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_ON_DEPT_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SALARY_PAY_METHOD" Text="<%$Resources:Resource,wfb2sc_SALARY_PAY_METHOD%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_SALARY_PAY_METHOD_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <th></th>
                                <td></td>
                                <th></th>
                                <td></td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_To_Bank_Name" Text="<%$Resources:Resource,wfb2sc_To_Bank_Name%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_To_Bank_Name_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SALARY_ACCOUNT_NO" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_ACCOUNT_NO1%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_SALARY_ACCOUNT_NO_Value" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_REMIT_DT1" Text="<%$Resources:Resource,wfb2sc_REMIT_DT1%>" />
                                </th>
                                <td>
                                    <asp:Label ID="lb_REMIT_DT1_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                         <aces:Btn runat="server" ID="WFB2SC4100Email" OnClick="WFB2SC4100Email_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Email%>" ClientIDMode="Static" />
                        <aces:Btn runat="server" ID="WFB2SC4100InsDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100InsDtl%>" ClientIDMode="Static" OnClientClick="Open('InsDtl');" />
                        <aces:Btn runat="server" ID="WFB2SC4100OOverDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100OOverDtl%>" ClientIDMode="Static" OnClientClick="Open('OOverDtl');" />
                        <aces:Btn runat="server" ID="WFB2SC4100LeaveDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100LeaveDtl%>" ClientIDMode="Static" OnClientClick="Open('LeaveDtl');" />
                        <aces:Btn runat="server" ID="WFB2SC4100ShiftDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100ShiftDtl%>" ClientIDMode="Static" OnClientClick="Open('ShiftDtl');" />

                        <%--<asp:Button runat="server" ID="WFB2SC4100Email" OnClick="WFB2SC4100Email_Click" OnClientClick="BlockUI();" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100Email%>" ClientIDMode="Static" />
                        <asp:Button runat="server" ID="WFB2SC4100InsDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100InsDtl%>" ClientIDMode="Static" OnClientClick="Open('InsDtl');" />
                        <asp:Button runat="server" ID="WFB2SC4100OOverDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100OOverDtl%>" ClientIDMode="Static" OnClientClick="Open('OOverDtl');" />
                        <asp:Button runat="server" ID="WFB2SC4100LeaveDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100LeaveDtl%>" ClientIDMode="Static" OnClientClick="Open('LeaveDtl');" />
                        <asp:Button runat="server" ID="WFB2SC4100ShiftDtl" Text="<%$Resources:Resource,wfb2sc_WFB2SC4100ShiftDtl%>" ClientIDMode="Static" OnClientClick="Open('ShiftDtl');" />--%>
                        <asp:Button runat="server" ID="btn_back" Text="<%$Resources:Resource,wfb2sc_btn_back%>" ClientIDMode="Static" OnClick="btn_back_Click"  /> <%--OnClientClick="location.href='WFB2SC4100_Qry.aspx';return false;"--%>
                    </td>
                </tr>
            </table>
            <hr style="width: 1020px; text-align: left;" />
            <div runat="server" id="DivSalary_Detail">
                <table runat="server" id="tbSalary_Detail" width="1020px" style="border-collapse: collapse; border: 2px solid #000;" />
            </div>
            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_SALARY_YM" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_SALARY_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DATA_SDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DATA_EDT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_parentFuncId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
