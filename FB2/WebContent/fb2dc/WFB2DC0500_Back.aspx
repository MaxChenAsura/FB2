<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0500_Back.aspx.cs" Inherits="WebContent_WFB2DC0500_Back" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();

        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

            $("#txt_BORROW_TYPE").attr("readonly", true);
            $("#txt_PERSON_ID").attr("readonly", true);
            $("#txt_PERSON_NAME").attr("readonly", true);
            $("#txt_PERSON_DC").attr("readonly", true);
            $("#txt_TEMP_CARD_CD").attr("readonly", true);
            $("#txt_CARD_NAME").attr("readonly", true);
            $("#txt_RETURN_DT").attr("readonly", true);

            $.unblockUI();
        }

        function fnOpenSearch() {
            var temp_card_cd = $("#hid_TEMP_CARD_CD").val();
            OpenSearch('Card_Search.aspx', 'txt_CARD_NO', 'txt_CARD_NAME', 'CARD_TYPE=Return&TEMP_CARD_CD=' + temp_card_cd,'Y');
            //var json = OpenSearch('Card_Search.aspx', 'txt_CARD_NO', 'txt_CARD_NAME', 'CARD_TYPE=Return&TEMP_CARD_CD=' + temp_card_cd);
            //if (json != undefined) {
            //    $("#txt_CARD_NO").val(json.CD);
            //    $("#txt_CARD_NAME").val(json.DESC);
            //    $("#hid_set").val("Y");

            //} else {
            //    $("#hid_set").val("");
            //}
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
                $("#hid_set").val("Y");
                if (obj.DESC != "&nbsp;") {
                    $("#txt_CARD_NAME").val(obj.DESC);
                }
                else {
                    $("#txt_CARD_NAME").val("");
                }
                __doPostBack('returnCard', 'true');
            } else {
                $("#hid_set").val("");
            }
        }

        function openQry() {
            window.location.href("WFB2DC0500_Qry.aspx");
            return false;
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
                                        <aces:Btn ID="WFB2DC0500Borrow" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Borrow%>" OnClick="WFB2DC0500Borrow_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <aces:Btn ID="WFB2DC0500Return" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Return%>" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />

<%--                                            <asp:Button ID="WFB2DC0500Borrow" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Borrow%>" OnClick="WFB2DC0500Borrow_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <asp:Button ID="WFB2DC0500Return" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Return%>" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />--%>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_BORROW_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_TYPE%>"></asp:Label>:                                    
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_BORROW_TYPE" runat="server" BorderWidth="0" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_PERSON_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_PERSON_ID" runat="server" BorderWidth="0" ClientIDMode="Static" Width="150px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                            <asp:TextBox ID="txt_PERSON_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="250px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_PERSON_DC" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_DC%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_PERSON_DC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="420px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_TEMP_CARD_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_TEMP_CARD_CD%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_TEMP_CARD_CD" runat="server" BorderWidth="0" ClientIDMode="Static" Width="420px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_CARD_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NO2%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_CARD_NO" runat="server" MaxLength="8" Width="150px" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" OnTextChanged="txt_CARD_NO_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:Button ID="btn_CARD_NO" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClientClick="fnOpenSearch();" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                                            <asp:TextBox ID="txt_CARD_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="200px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_NO2%>"
                                                ControlToValidate="txt_CARD_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_START_DT_RANGE%>"></asp:Label>:
                                        </th>
                                        <td class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_START_DT_S" runat="server" ClientIDMode="Static" Enabled="false" Width="236px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_S_H" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>&nbsp;
                                            <asp:Label ID="Label2" runat="server" Text=":" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_S_M" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>
                                            <asp:Label ID="Label3" runat="server" Text="~" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:Label><br />
                                            <asp:TextBox ID="txt_START_DT_E" runat="server" ClientIDMode="Static" Enabled="false" Width="236px" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_E_H" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>&nbsp;
                                            <asp:Label ID="Label1" runat="server" Text=":" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_E_M" runat="server" ClientIDMode="Static" Enabled="false" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_RETURN_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_RETURN_DT%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:TextBox ID="txt_RETURN_DT" runat="server" BorderWidth="0" ClientIDMode="Static" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- 卡片狀態--%>
                                        <th align="left" class="Body_TableHeader3">
                                            <asp:Label ID="lb_BORROW_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_STATUS%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label2" colspan="3">
                                            <asp:DropDownList ID="ddl_BORROW_STATUS" runat="server" CssClass="MandatoryField" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" OnSelectedIndexChanged="ddl_BORROW_STATUS_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_BORROW_STATUS%>"
                                                ControlToValidate="ddl_BORROW_STATUS" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
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
                            <aces:Btn ID="WFB2DC0500Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DC0500Save_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />

                            <%--<asp:Button ID="WFB2DC0500Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DC0500Save_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />--%>
                            
                            <asp:Button ID="WFB2DC0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Cancel%>" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DC0500Cancel_Click" Font-Bold="True" Font-Size="30px" Font-Names="Times New Roman" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:HiddenField ID="hid_set" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_TEMP_CARD_CD" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

