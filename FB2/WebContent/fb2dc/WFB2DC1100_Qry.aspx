<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC1100_Qry.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC1100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $('.number4').mask('9999');
            
            //工號取得姓名的ajax
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_EMP_ID").blur(function () {
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
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
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
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_CALENDAR_DT_S").val("");
            $("#txt_CALENDAR_DT_E").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $('#txt_COUNT').val("");
            var checkedItems = $('#cb_DUTY_CHECK_RESULT').find('input:checked');
            for (i = 0; i < checkedItems.length; i++) {
                checkedItems.attr("checked", false);
            }
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
        <colgroup>
            <col width="7%" />
            <col width="12%" />
            <col width="20%" />
            <col width="7%" />
            <col width="16%" />
            <col width="15%" />
            <col width="20%" />
        </colgroup>
        <tbody>
            <!-- START: 1st Line in Search Criteria area -->
            <tr>

                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="2">
                    <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                    <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                    <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>

                </td>

                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CALENDAR_DT2%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="3">
                    <asp:TextBox ID="txt_CALENDAR_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                    ~
                     <asp:TextBox ID="txt_CALENDAR_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>

                    
                    <asp:CustomValidator ID="cv_CALENDAR_DT_S" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CALENDAR_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_CALENDAR_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                    <asp:CustomValidator ID="cv_CALENDAR_DT_E" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CALENDAR_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_CALENDAR_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_CALENDAR_DT_S"
                        ControlToValidate="txt_CALENDAR_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dc_GreaterThanEqual_CALENDAR_DT%>" Type="Date" Operator="GreaterThanEqual"
                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Required_CALENDAR_DT_S%>"
                        ControlToValidate="txt_CALENDAR_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Required_CALENDAR_DT_E%>"
                        ControlToValidate="txt_CALENDAR_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                </td>



            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DEPT_NO%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="2">
                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                    <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_COUNT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_COUNT%>"></asp:Label>
                </th>
                <td align="left" class="Body_label" colspan="3">
                    <asp:TextBox ID="txt_COUNT" runat="server" MaxLength="4" size="4"  CssClass="number4" ClientIDMode="Static"></asp:TextBox>
                    次 </td>

            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DUTY_CHECK_RESULT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DUTY_CHECK_RESULT2%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="6">
                    <asp:CheckBoxList ID="cb_DUTY_CHECK_RESULT" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal"></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td align="right" class="Body_label" colspan="7">
                    <div id="init">
                        <aces:Btn ID="WFB2DC1100Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1100Export%>" OnClick="WFB2DC1100Export_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                        <%--<asp:Button ID="WFB2DC1100Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC1100Export%>" OnClick="WFB2DC1100Export_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                        
                        <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" height="1" colspan="7">
                    <hr />
                </td>
            </tr>
            <!-- end: Create MODULE ID -->
        </tbody>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
