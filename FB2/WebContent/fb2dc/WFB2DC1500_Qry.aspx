<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC1500_Qry.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC1500_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask("9999/99");

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_DEPT_NO").blur(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });

            $.unblockUI();
        }
        function ClearAll() {
            $("#txt_DUTY_YM").val("");
            $("#txt_DEPT_NO").val("");
            $("#ddl_PLANT_CD").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_WORK_CD").val("-1");
            $("#txt_DEPT_NAME").val("");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
        <colgroup>
            <col width="15%" />
            <col width="20%" />
            <col width="15%" />
            <col width="55%" />
        </colgroup>
        <tbody>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DUTY_YM" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DUTY_YM%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="4">
                    <asp:TextBox ID="txt_DUTY_YM" runat="server" MaxLength="5" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_DUTY_YM%>" ControlToValidate="txt_DUTY_YM" ForeColor="Red"
                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                    </asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Required_DUTY_YM%>"
                        ControlToValidate="txt_DUTY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PLANT_CD%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>

                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DEPT_NO%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                    <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_WS_CD%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_WORK_CD%>"></asp:Label>:</th>
                <td align="left" class="Body_label">

                    <asp:DropDownList ID="ddl_WORK_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td align="left" class="Body_label"></td>
                <td align="right" class="Body_label" colspan="8">
                    <%-- 欠勤率 --%>
                    <aces:Btn ID="WFB2DC1500ExportXLS1" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1500_ExportXLS1%>" OnClick="WFB2DC1500_ExportXLS1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                    <%-- 請假統計 --%>
                    <aces:Btn ID="WFB2DC1500ExportXLS2" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1500_ExportXLS2%>" OnClick="WFB2DC1500_ExportXLS2_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                    <%-- 加班統計 --%>
                    <aces:Btn ID="WFB2DC1500ExportXLS3" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1500_ExportXLS3%>" OnClick="WFB2DC1500_ExportXLS3_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                    <%--                    <asp:Button ID="WFB2DC1500ExportXLS1" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1500_ExportXLS1%>" OnClick="WFB2DC1500_ExportXLS1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                    <asp:Button ID="WFB2DC1500ExportXLS2" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1500_ExportXLS2%>" OnClick="WFB2DC1500_ExportXLS2_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                    <asp:Button ID="WFB2DC1500ExportXLS3" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1500_ExportXLS3%>" OnClick="WFB2DC1500_ExportXLS3_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>

                    <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
                </td>
            </tr>

            <tr>
                <td align="center" height="1" colspan="20">
                    <hr>
                </td>
            </tr>

            <!-- end: Create MODULE ID -->
        </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
