<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0500_Add.aspx.cs" Inherits="WebContent_WFB2DC0500_Add" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
            ShowTime();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

            $("#txt_PERSON_NAME").attr("readonly", true);
            $("#txt_PERSON_DC").attr("readonly", true);
            $("#txt_CARD_NAME").attr("readonly", true);

            $.unblockUI();
        }

        function OpenPERSON_ID() {
            var rbl = $("#rbl_BORROW_TYPE input:radio:checked").val();
            if (rbl == '1') {
                var is_super = "Y"; //不須控管
                OpenEmpSearch('txt_PERSON_ID', 'txt_PERSON_NAME', is_super,'Y');
                

            } else {
                OpenSearch('Vendor_d_Search.aspx', 'txt_PERSON_ID', 'txt_PERSON_NAME', '', 'Y');
               
            }
        }

        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_PERSON_ID").val(obj.EMP_ID);
                $("#txt_PERSON_NAME").val(obj.EMP_NAME);
                //$("#hid_set").val("Y");
                stop();
                document.getElementById('hid_getEmpName').click();
            } else {
                $("#hid_set").val("");
            }
        }

        function Vendor_d_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_PERSON_ID").val(obj.CD);
                $("#txt_PERSON_NAME").val(obj.DESC);
                //$("#hid_set").val("Y");

                stop();
            } else {
                $("#hid_set").val("");
            }
        }

        function fnOpenSearch() {
            var temp_card_cd = $("#hid_TEMP_CARD_CD").val();
            if ($("#ddl_TEMP_CARD_CD").val() != "-1") {
                temp_card_cd = $("#ddl_TEMP_CARD_CD").val();
            }
            OpenSearch('Card_Search.aspx', 'txt_CARD_NO', 'txt_CARD_NAME', 'CARD_TYPE=Borrow&TEMP_CARD_CD=' + temp_card_cd,'Y');
            
        }
        
        function Card_Search(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_CARD_NO").val(obj.CD);                
                //$("#hid_set").val("Y");
                if (obj.DESC != "&nbsp;") {
                    $("#txt_CARD_NAME").val(obj.DESC);
                }
                else {
                    $("#txt_CARD_NAME").val("");
                }
                stop();
            } else {
                $("#hid_set").val("");
            }
        }

        function ShowTime() {
            //開始計時
            timerID = setTimeout("ShowTime()", 180000); //3分鐘重設
            //借用期間
            var sdt = new Date();
            var smonth = sdt.getMonth() + 1; //月份從零開始需加1
            var sday = sdt.getDate();
            if (smonth < 10) {
                smonth = "0" + smonth;
            }
            if (sday < 10) {
                sday = "0" + sday;
            }
            var sdate = sdt.getFullYear() + "/" + smonth + "/" + sday;
            $("#txt_START_DT_S").val(sdate);

            var hour = sdt.getHours();
            var min = sdt.getMinutes();
            if (min < 15) {
                hour = hour - 1;
                min = min + 60 - 15;
            } else {
                var min = min - 15;
            }
            if (hour < 10) {
                hour = "0" + hour;
            }
            if (min < 10) {
                min = "0" + min;
            }
            $("#ddl_START_DT_S_H").val(hour);
            $("#ddl_START_DT_S_M").val(min);
            $("#ddl_START_DT_E_H").val(hour);
            $("#ddl_START_DT_E_M").val(min);
        }

        function stop() {
            clearTimeout(timerID); //停止計時
        }

        function openQry() {
            window.location.href("WFB2DC0500_Qry.aspx");
            return false;
        }

        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            } else {
                $("#txt_PERSON_NAME").val("");
            }

        }

        function ChangeTIME() {
            $("#ddl_START_DT_E_H").val($("#ddl_START_DT_S_H").val());
            $("#ddl_START_DT_E_M").val($("#ddl_START_DT_S_M").val());
        }

        function ChangeBORROW_END_DT() {
            var re = /(19|20)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])/;
            if (re.test($("#txt_START_DT_S").val())) {
                //alert("yes");
                $("#hid_START_DT_S").val($("#txt_START_DT_S").val());
                $("#ddl_START_DT_E_H").val($("#ddl_START_DT_S_H").val());
                $("#ddl_START_DT_E_M").val($("#ddl_START_DT_S_M").val());

                document.getElementById('hid_getBORROW_END_DT').click();

                return false;
            }
            else {
                alert("借用期間起日期格式錯誤(正確日期格式,如:2013/01/05)!");
            }

        }

        function Check_ID_LENGTH(source, arguments) {
            if ($("#txt_PERSON_ID").val().length != 5)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_label2">
                <colgroup>
                    <col width="80%" />
                    <col width="20%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <td>
                            <table cellspacing="1" cellpadding="1" width="93%" border="0" class="Body_label2">
                                <colgroup>
                                    <col width="22%" />
                                    <col width="32%" />
                                    <col width="20%" />
                                    <col width="16%" />
                                </colgroup>
                                <tbody>
                                    <!-- START: 1st Line in Search Criteria area -->
                                    <tr>
                                        <th align="left" colspan="4" class="Body_TableHeader3">
                                        <aces:Btn ID="WFB2DC0500Borrow" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Borrow%>" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <aces:Btn ID="WFB2DC0500Return" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Return%>" OnClick="WFB2DC0500Return_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />

<%--                                            <asp:Button ID="WFB2DC0500Borrow" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Borrow%>" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <asp:Button ID="WFB2DC0500Return" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Return%>" OnClick="WFB2DC0500Return_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />--%>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_BORROW_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_TYPE%>"></asp:Label>:                                    
                                        </th>
                                        <%--借卡人員 --%>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:RadioButtonList ID="rbl_BORROW_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman">
                                                <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_PERSON%>" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_COMPANY%>" Value="2"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_PERSON_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>"></asp:Label>:
                                        </th>
                                        <%--工號/廠商人員編號 --%>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_PERSON_ID" runat="server" MaxLength="5" Width="150px" ClientIDMode="Static" CssClass="MandatoryField"
                                                onblur="javascript:getEmpName(this.id)" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                            <asp:Button ID="bt_PERSON_ID" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClientClick="OpenPERSON_ID();" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <asp:TextBox ID="txt_PERSON_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Width="180px" Font-Names="Times New Roman"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_PERSON_ID%>"
                                                ControlToValidate="txt_PERSON_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_PERSON_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                                ControlToValidate="txt_PERSON_ID" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>

                                        </td>
                                    </tr>

                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_PERSON_DC" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_DC%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <%--部門/廠商別 --%>
                                            <asp:TextBox ID="txt_PERSON_DC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="420px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_TEMP_CARD_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_TEMP_CARD_CD%>"></asp:Label>:
                                        </th>
                                         <%--臨時卡區分 --%>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:DropDownList ID="ddl_TEMP_CARD_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" OnSelectedIndexChanged="ddl_TEMP_CARD_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_TEMP_CARD_CD%>"
                                                ControlToValidate="ddl_TEMP_CARD_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- 借用卡號--%>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_CARD_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NO2%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_CARD_NO" runat="server" MaxLength="8" Width="150px" ClientIDMode="Static" CssClass="MandatoryField" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" OnTextChanged="txt_CARD_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Button ID="btn_CARD_NO" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClientClick="fnOpenSearch();" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <asp:TextBox ID="txt_CARD_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Width="200px" Font-Names="Times New Roman"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_NO2%>"
                                                ControlToValidate="txt_CARD_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_START_DT_RANGE%>"></asp:Label>:
                                        </th>
                                        <td class="Body_label2" colspan="3">
                                            <%--<asp:TextBox ID="txt_START_DT_S" runat="server" ClientIDMode="Static" CssClass="MandatoryField"  onblur="javascript:ChangeBORROW_END_DT()" Font-Bold="True" Font-Size="30px" Width="236px" Font-Names="Times New Roman"></asp:TextBox>--%>
                                            <asp:TextBox ID="txt_START_DT_S" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" OnTextChanged="txt_START_DT_S_TextChanged" AutoPostBack="true" Font-Bold="True" Font-Size="30px" Width="236px" Font-Names="Times New Roman"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_S_H" runat="server" ClientIDMode="Static" onblur="javascript:ChangeTIME()" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>&nbsp;
                                            <asp:Label ID="Label2" runat="server" Text=":" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_S_M" runat="server" ClientIDMode="Static" onblur="javascript:ChangeTIME()" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>
                                            <asp:Label ID="Label1" runat="server" Text="~" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:Label><br />
                                            <asp:TextBox ID="txt_START_DT_E" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Width="236px" Font-Names="Times New Roman"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_E_H" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>&nbsp;
                                            <asp:Label ID="Label3" runat="server" Text=":" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_E_M" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT_S%>"
                                                ControlToValidate="txt_START_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorSTART_DT_S" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DATE_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_BORROW_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_REASON_CD%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:DropDownList ID="ddl_BORROW_REASON_CD" runat="server" CssClass="MandatoryField" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_BORROW_REASON_CD%>"
                                                ControlToValidate="ddl_BORROW_REASON_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- 是否重新製卡--%>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_IS_RE_MARK" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_RE_MARK%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:RadioButtonList ID="rbl_IS_RE_MARK" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman">
                                                <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_Y%>" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_N%>" Value="N" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="4" bgcolor="red" style="font-family: 'Times New Roman', Times, serif; font-size: 30px; font-weight: bold">
                                            <marquee onmouseover="this.stop()" onmouseout="this.start()" behavior="alternate">
                                                <font color="yellow"><asp:Label ID="lb_MSG" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_MSG%>"></asp:Label></font></marquee>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td valign="top" align="left">
                            <table cellspacing="1" cellpadding="1" width="10%" border="0" class="Body_label2">
                                <colgroup>
                                    <col width="100%" />
                                </colgroup>
                                <tbody>
                                    <%-- 照片 --%>
                                    <!-- START: 1st Line in Search Criteria area -->
                                    <tr rowspan="8">
                                        <td style="border-right: #cccc99 1px solid; border-top: #cccc99 1px solid; border-left: #cccc99 1px solid; border-bottom: #cccc99 1px solid;">
                                            <asp:Image ID="EmpPhoto" runat="server" Width="200px" Height="200px" ImageUrl="" />
                                        </td>

                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <aces:Btn ID="WFB2DC0500Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" OnClick="WFB2DC0500Save_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                            <asp:Button ID="WFB2DC0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Cancel%>" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DC0500Cancel_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:HiddenField ID="hid_set" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_TEMP_CARD_CD" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <asp:Button ID="hid_getBORROW_END_DT" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getBORROW_END_DT_Click" />
            <asp:HiddenField ID="hid_START_DT_S" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

