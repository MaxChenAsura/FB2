<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP0100_Qry.aspx.cs" Inherits="WebContent_WFB2SP0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.unblockUI();
            iniForm();
        });
        var li_id;
        function iniForm() {
            $.unblockUI();
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $('#txt_EMP_NAME').attr("readonly", true);
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
                                $('#txt_RETIRE_DT').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                                
                                if ($("#ddl_COMPUTER_TYPE").val() == "A") {
                                    //系統日
                                    $('#txt_RETIRE_DT').val($('#hid_sys_date').val());
                                    $('#hid_sys_RETIRE_DT').val($.trim(JData.LEAVE_DT))
                                } else {
                                    //退休日
                                    $('#txt_RETIRE_DT').val($.trim(JData.LEAVE_DT));
                                    $('#hid_sys_RETIRE_DT').val($.trim(JData.LEAVE_DT))
                                }
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

        //執行檢核
        function executeCheck() {
            var processed = false;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                processed = true;
            } else {
                return processed;
            }
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

        function doClear() {
            $("#ddl_COMPUTER_TYPE").val("A");
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
            $("#txt_RETIRE_DT").val($("#hid_defalut_RETIRE_DT").val());
            $("#ddl_DELEGATE_YN").val("N");
            $("#txt_DELEGATE_DT").val("");
            return false;
        }




    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <%--計算類別--%>
                                        <asp:Label ID="lb_COMPUTER_TYPE" runat="server" Text="計算類別"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_COMPUTER_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="ddl_COMPUTER_TYPE_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--工號--%>
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5" CssClass="MandatoryField"> </asp:TextBox>
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="工號不可空白"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--退休日--%>
                                        <asp:Label ID="lb_RETIRE_DT" runat="server" Text="退休日"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_RETIRE_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="此人尚未退休"
                                            ControlToValidate="txt_RETIRE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <%--委任經理人--%>
                                        <asp:Label ID="Label2" runat="server" Text="委任經理人"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DELEGATE_YN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--委任日--%>
                                        <asp:Label ID="Label1" runat="server" Text="委任日"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_DELEGATE_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:CustomValidator ID="qry4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="委任日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_DELEGATE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn  ID="WFB2SP0100Execute" runat="server" Text="計算" OnClick="WFB2SP0100Execute_Click" OnClientClick="return executeCheck();"  />
                                            <%-- 
                                            <asp:Button ID="WFB2SP0100Execute" runat="server" Text="計算" OnClick="WFB2SP0100Execute_Click" OnClientClick="return executeCheck();"  />
                                            --%>
                                            <asp:Button ID="btn_clear" runat="server" Text="清除" OnClientClick="return doClear();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>

            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_RETIRE_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_sys_RETIRE_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_sys_date" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
