<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL3110_Qry.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL3110_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $("#txt_MAIL_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            //$("#txt_EFFECT_YEAR").datepicker({ dateFormat: 'yyyy' });
            $(".date").datepicker({ dateFormat: 'yy' });
            $(".date").mask('9999');
            $(".date2").mask('9999/99/99');
            $(".empid").mask('99999');
            $.unblockUI();
            $('#txt_EMP_NAME').attr("readonly", true);
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
        //function doEmpAjax() {
        //    //ajax 取得員工基本資料
        //    $.ajax({
        //        url: "../commgeo/WFB2GetEmpData.ashx",
        //        data: {
        //            EMP_ID: $('#txt_EMP_ID').val()
        //        },
        //        type: "GET",
        //        dataType: 'json',
        //        success: function (JData) {
        //            if (JData.errMsg != "") {
        //                $('#txt_EMP_NAME').val("");
        //                $('#txt_EMP_CD').val("");
        //                $('#txt_DEPT_DESC').val("");
        //                $('#txt_LEAVE_DT').val("");
        //                alert(JData.errMsg);
        //            } else {
        //                $('#txt_EMP_NAME').val(JData.EMP_NAME);
        //                $('#txt_EMP_CD').val(JData.EMP_CD_DESC);
        //                $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
        //                $('#txt_LEAVE_DT').val(JData.LEAVE_DT);
        //            }
        //        },

        //        error: function (xhr, ajaxOptions, thrownError) {
        //            alert(xhr.status);
        //            alert(thrownError);
        //        }
        //    });
        //}
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ClearAll() {
            $('#txt_EFFECT_YEAR').val("");
            $('#txt_MAIL_DT').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
            $('#txt_TITLE').val("");
            $('#txt_MAIL_DESC').val("");

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

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidwfb2sc_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function checkconfirm(checkconfirmmessage) {
            var ret = confirm(checkconfirmmessage);
            if (ret) {
                __doPostBack('execute', '');

            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>            
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="40%" />
                                <col width="10%" />
                                <col width="40%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EFFECT_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EFFECT_YEAR" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField date" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_required_DATA_YEAR%>"
                                            ControlToValidate="txt_EFFECT_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sl_error_DATA_YEAR%>" ControlToValidate="txt_EFFECT_YEAR" ForeColor="Red"
                                            ValidationExpression="\d\d\d\d" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_MAIL_DT" runat="server" Text="<%$Resources:Resource,wfb2se_lb_MAIL_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIL_DT" runat="server" Columns="10" ClientIDMode="Static" CssClass="MandatoryField date2" OnTextChanged="txt_MAIL_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_lbl_MAIL_DT_Required%>"
                                            ControlToValidate="txt_MAIL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="寄送日期格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_MAIL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>

                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2se_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th></th>
                                    <th></th>

                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_TITLE" runat="server" Text="<%$Resources:Resource,wfb2se_lb_TITLE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TITLE" runat="server" MaxLength="150" Width="200px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_lbl_TITLE_Required%>"
                                            ControlToValidate="txt_TITLE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th></th>
                                    <th></th>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_MAIL_DESC" runat="server" Text="<%$Resources:Resource,wfb2se_MAIL_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIL_DESC" runat="server" MaxLength="250" Width="100%" Rows="4" TextMode="MultiLine" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_lbl_MAIL_DESC_Required%>"
                                            ControlToValidate="txt_MAIL_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th></th>
                                    <th></th>
                                </tr>
                                
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SL3110Execute" runat="server" Text="<%$Resources:Resource,btn_Execute%>" ValidationGroup="GroupA" OnClick="WFB2SL3110Execute_Click" />

                                            <%--<asp:Button ID="WFB2SL3110Execute" runat="server" Text="<%$Resources:Resource,btn_Execute%>" ValidationGroup="GroupA" OnClick="WFB2SL3110Execute_Click" />--%>
                                            <input id="WFB2SB2200Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <!--2SE220_QRY結束-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="Hidwfb2sc_Mail_addSuccessMessage" Value="<%$Resources:Resource,wfb2ib_CheckEdit_Required%>" />

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

</asp:Content>


