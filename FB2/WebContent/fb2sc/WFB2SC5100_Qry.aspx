<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC5100_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC5100_Qry" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();

        });

        function iniForm() {
            getCurrent_DATA_YM();
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_EMP_DESC').attr("readonly", true);
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ymd').mask('9999/99/99');
            $('.ym').mask('9999/99');
            $.unblockUI();

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
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

            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_DESC').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_DESC').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_DESC').val("");
                }
            });
        }
        function getCurrent_DATA_YM() {
            var datatime = new Date();
            var year = datatime.getFullYear().toString();
            var month = (datatime.getMonth() + 1).toString();
            if (month.length == 1)
                month = "0" + month;
            $("#txt_DATA_YM").val(year + month);
        }
        //清空
        function ClearAll() {
            $("#txt_PAY_ID").val("");
            $("#ddl_SALARY_TYPE").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_DESC").val("");
            return false;
        }
        function delayedShow() {
            if (Page_IsValid != null && Page_IsValid == true) {
                BlockUI();
            }
        }
        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = $("[type=radio]");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
                else {
                    $('#hid_selectedIndex').val(i);
                }
            }
            $('#WFB2SC5100Check').click();
        }
        function MycheckDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        function doUnBlock() {
            $.unblockUI();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="14%" />
                                <col width="86%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--關帳代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_ID" runat="server" MaxLength="11" Width="100px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入關帳代號"
                                            ControlToValidate="txt_PAY_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="關帳代號輸入格式錯誤(英數字和'-' 組成)"
                                            ControlToValidate="txt_PAY_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z-]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請選擇發薪類別"
                                            ControlToValidate="ddl_SALARY_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <%--部門--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N', '');" runat="server" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" MaxLength="60" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label runat="server" ID="lb_EMP_ID" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>" />
                                    </th>
                                    <td>
                                        <asp:TextBox runat="server" ID="txt_EMP_ID" Width="64" MaxLength="5" ClientIDMode="Static" />
                                        <input type="button" runat="server" id="btn_EMP_ID" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_DESC', 'N', ''); return false;" />
                                        <asp:TextBox runat="server" ID="txt_EMP_DESC" BorderWidth="0" ClientIDMode="Static" Style="background-color: white; color: black;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC5100Print" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC510Print%>" OnClick="WFB2SC5100Print_Click" OnClientClick="return MycheckDowning(this.value);" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SC5100Print" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC510Print%>" OnClick="WFB2SC5100Print_Click" OnClientClick="return MycheckDowning(this.value);" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="return ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC5100Print" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
