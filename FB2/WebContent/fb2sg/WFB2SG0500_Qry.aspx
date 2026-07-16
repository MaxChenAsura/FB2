<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0500_Qry.aspx.cs" Inherits="WebContent_WFB2SG0500_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask("9999/99/99");

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
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });


        }


        //查詢前檢核
        function CheckDownload() {
            //其它需要檢核的
            
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                processed = true;
            } else {
                processed = false;
            }
            if (processed) {
                var startDT = new Date($("#txt_START_DT").val());
                var endDT = new Date($("#txt_END_DT").val());
                var diffDay = (endDT - startDT) / 86400000;
                if (diffDay > 365) {
                    alert("起迄日不可大於1年");
                    processed = false;
                    return false;
                }
            }
            BlockUI();
            processed = confirm("確定要進行一時金對象下載?");
            if (!processed) {
                $.unblockUI();
                return false;
            }

            if (!processed)
                $.unblockUI();


            return processed;
        }

        //清空畫面
        function ClearAll() {
            var day = new Date();
            var lastyear = day.getFullYear() - 1;

            $("#txt_START_DT").val(lastyear+"/01/01");
            $("#txt_END_DT").val(lastyear + "/12/31");


            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_EMP_STATUS").val("01");
            $("#ddl_EMP_CD").val("-1");
        }



    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="40%" />
                    <col width="10%" />
                    <col width="40%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--日期區間--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_date_s_e%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_START_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            ~ 
                           <asp:TextBox ID="txt_END_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_date_s%>"
                                ControlToValidate="txt_Start_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="qry2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sg_required_date_e%>"
                                ControlToValidate="txt_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="qry3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_date_s%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_Start_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                            <asp:CustomValidator ID="qry4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sg_format_date_e%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                            <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_Start_DT"
                                ControlToValidate="txt_END_DT" ErrorMessage="<%$Resources:Resource,wfb2sg_format_date_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"></asp:TextBox>
                              <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--在職狀態--%>
                            <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_status%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_EMP_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <%--員工區分--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_prid_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="4">
                            
                            <aces:Btn ID="WFB2SG0500ExcelDown" runat="server" Text="一時金對象下載" OnClick="WFB2SG0500ExcelDown_Click" OnClientClick="return CheckDownload();" />
                            <%--
                            <asp:Button ID="WFB2SG0500ExcelDown" runat="server" Text="一時金對象下載" OnClick="WFB2SG0500ExcelDown_Click" OnClientClick="return CheckDownload();" />
                                --%>
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>

            <!--比較用的日期-->
            <asp:HiddenField ID="HID_STARTDT_365" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SG0500ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
