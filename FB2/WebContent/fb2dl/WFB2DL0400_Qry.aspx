<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0400_Qry.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0400_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".year").mask('9999');
            $(".number").mask('999');            
            $.unblockUI();

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

            //Grid部門代號取得部門名稱的ajax
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
        }

     
        function CheckYM(source, arguments) {
            if ($("#txt_DATA_YM_S").val() != "" && $("#txt_DATA_YM_E").val() != "") {
                if ($("#txt_DATA_YM_E").val() < $("#txt_DATA_YM_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }

        //清空畫面
        function ClearAll() {
            $("#txt_DATA_YM_S").val("");
            $("#txt_DATA_YM_E").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_tree_DEPT_NO").val("");
            $("#txt_tree_DEPT_NAME").val("");
            $("#ddl_WS_CD").val("");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                 <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="29%" />
                                <col width="10%" />
                                <col width="19%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--年度--%>
                                    <tr>
                                    <th align="left" class="Body_TableHeader">
                                         <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2ib_lb_START_YM%>"></asp:Label>:</th>
                                    </th>
                                    <td colspan="3">
                                         <asp:TextBox ID="txt_YEAR_S" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year" ></asp:TextBox>
                                        ~ 
                                       <asp:TextBox ID="txt_YEAR_E" runat="server" Width="60px" ClientIDMode="Static"  CssClass="MandatoryField year" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sj_required_assess_year_s%>"
                                            ControlToValidate="txt_YEAR_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="qry2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sj_required_assess_year_e%>"
                                            ControlToValidate="txt_YEAR_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                         <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_YEAR_S"
                                                        ControlToValidate="txt_YEAR_E" ErrorMessage="<%$Resources:Resource,wfb2sj_error_assess_year_s_e%>" Type="Integer" Operator="GreaterThanEqual"
                                                        Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <!--驗證長度需為4 -->	
								            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                                ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_YEAR_S" ForeColor="Red" ValidationGroup="GroupA"
                                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_e%>" ControlToValidate="txt_YEAR_E" ForeColor="Red" ValidationGroup="GroupA"
                                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- 工號  --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                     <%-- 部門  --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="getDept();" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="140px" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>             
                                      <%--職種--%>
                                   <th align="left" class="Body_TableHeader">                                      
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_ws_cd%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>               
                                </tr>
                                
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DL0400EXCEL" runat="server" Text="匯出EXCEL" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DL0400EXCEL_Click" />
                                            <%--<asp:Button ID="WFB2DL0400Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DL0400Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                    </td>
                </tr>

            </table>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
         <Triggers>
                <%--匯出EXCEL按鈕必寫--%>
            <asp:PostBackTrigger ControlID="WFB2DL0400EXCEL" />  
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

