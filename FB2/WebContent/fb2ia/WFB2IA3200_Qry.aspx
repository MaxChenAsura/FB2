<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA3200_Qry.aspx.cs" Inherits="WebContent_WFB2IA3200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99')
            $('.empid').mask('99999');
            gridviewScroll(); //呼叫凍結視窗 
            $.unblockUI();
            $("#txt_EMP_NAME").attr("readonly", true);

            //寫在這，按查詢才不會消失
            $('#txt_COMPANY_SNAME').attr("readonly", true);
            //公司代號取得公司名稱的ajax
            $("#txt_COMPANY_CD").change(function () {
                if ($("#txt_COMPANY_CD").val().length == 1) {
                    $.ajax({
                        url: "../comm/WFB2CompanyData.ashx",
                        data: {
                            COMPANY_CD: $('#txt_COMPANY_CD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_COMPANY_SNAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_COMPANY_SNAME').val(JData.COMPANY_SNAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_COMPANY_SNAME').val("");
                }
            });

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
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //凍結視窗 
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                headerrowcount: 2,
                freezesize: 6
            });
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 6
            });
            $('#<%=gv_result3.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 6
            });
            $('#<%=gv_result4.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F",
                freezesize: 6
            });
        }
        function ddlCheck(source, arguments) {
            if ($("#ddl_BILLS_KIND").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        //function companyCheck(value) {
        //    __doPostBack('question', 'true');
        //}

        //清空畫面
        function ClearAll() {
            $("#txt_COMPANY_CD").val("");
            $("#txt_COMPANY_SNAME").val("");
            $("#txt_FEES_YM").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_LICENSE_ID").val("");
            $("#ddl_BILLS_KIND").val("-1");

            return false;
        }

        function openIA1200(url) {
            location.href = url;
        }
        function openIA1200Dtl(url) {
            location.href = url;
        }

        function openQry() {
            window.location.href("WFB2IA3200_Qry.aspx");
            return false;
        }
        function checkDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
                //return false;
            }


            if (processed) {
                processed = confirm($("#hidconfirm_Execute").val() + msg + "?");
                BlockUI();
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
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
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="1" Width="50px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_Company_CD%>"
                                            ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <input id="bt_COMPANY_CD_SEARCH" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_SNAME', '', '');" />
                                        <asp:TextBox ID="txt_COMPANY_SNAME" MaxLength="60" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <!-- >匯入帳單種類-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BILLS_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_BILLS_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_BILLS_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_required_BILLS_KIND%>" ClientValidationFunction="ddlCheck" ForeColor="Red"
                                            ControlToValidate="ddl_BILLS_KIND" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_FEES_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FEES_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_FEES_YM" runat="server" MaxLength="7" Width="70px" CssClass="MandatoryField date2" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_FEES_YM%>"
                                            ControlToValidate="txt_FEES_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_FEES_YM%>" ControlToValidate="txt_FEES_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_LICENSE_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" MaxLength="20" runat="server" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID_isError%>" ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                     <!-- >比對異常種類-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ia_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList> 
                                        <asp:DropDownList ID="ddl_YNB" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                      
                                    </td>                                    
                                </tr>
                                <tr>
                                    <th></th>
                                    <td></td>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA3200Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Process%>" OnClick="WFB2IA3200Process_Click" />
                                            <aces:Btn ID="WFB2IA3200Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" ValidationGroup="GroupA" OnClick="WFB2IA3200Search_Click" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2IA3200ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200ExcelDown%>" ValidationGroup="GroupA" OnClick="WFB2IA3200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                            <%--<asp:Button ID="WFB2IA3200Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Process%>" OnClick="WFB2IA3200Process_Click" />
                                            <asp:Button ID="WFB2IA3200Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" ValidationGroup="GroupA" OnClick="WFB2IA3200Search_Click" OnClientClick="CheckValid();" />
                                            <asp:Button ID="WFB2IA3200ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200ExcelDown%>" ValidationGroup="GroupA" OnClick="WFB2IA3200ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />--%>

                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2df_btn_clear%>" OnClientClick="return ClearAll();" CausesValidation="false" />
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
                 <tr>
                    <td>          
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
                                     <!-- >註記別-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EXCUTE_RESULT" runat="server" Text="<%$Resources:Resource,wfb2ia_FLAG_RESULT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_BN" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                        
                                    </td>
                                    <th></th>
                                    <td></td>   
                                     <td></td>                               
                                </tr>
                                </tbody>
                            </table>    
                           </td>
                     </tr>     
                    <tr>
                        <td align="right" colspan="10">
                            <%-- 
                                  <aces:Btn ID="WFB2IA3200Excute" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Excute%>" OnClick="WFB2IA3200Excute_Click" Visible="false" style="height: 21px"/>
                                <asp:Button ID="WFB2IA3200Excute" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Excute%>" OnClick="WFB2IA3200Excute_Click" Visible="false" style="height: 21px"/>
                           --%>
                            <aces:Btn ID="WFB2IA3200Excute" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Excute%>" OnClick="WFB2IA3200Excute_Click" Visible="false" style="height: 21px"/>
                        </td>
                    </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2IA3200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_COMPANY_CD"
                        Name="company_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_FEES_YM" DefaultValue=""
                        Name="fees_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_LICENSE_ID"
                        Name="license_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_BILLS_KIND"
                        Name="bills_kind" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_TYPE"
                        Name="type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_YNB"
                        Name="ynb" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="2220px" OnRowCreated="gv_result_RowCreated" OnPageIndexChanging="gv_result_PageIndexChanging"
                OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--聘用單位--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_SNAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--身份證/居留證--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_LICENCE_ID2%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_emp_id%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--員工區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_CD%>" SortExpression="EMP_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CD_NAME" runat="server" Text='<%#Bind("EMP_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CHG_CD_NAME" runat="server" Text='<%#Bind("EMP_CHG_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--身份別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_IDENTITY_KIND%>" SortExpression="IDENTITY_KIND_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_IDENTITY_KIND_NAME" runat="server" Text='<%#Bind("IDENTITY_KIND_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--眷屬姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_FAMILY_NAME%>" SortExpression="FAMILY_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_FAMILY_NAME" runat="server" Text='<%#Bind("FAMILY_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單投保金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_INS_AMT%>" SortExpression="BILLS_INS_AMT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_BILLS_INS_AMT" runat="server" Text='<%#Bind("BILLS_INS_AMT","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_HR_CHG_CD%>" SortExpression="CHANG_TYPE" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_CHANG_TYPE" runat="server" Text='<%#Bind("CHANG_TYPE")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計費註記--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_REMARK%>" SortExpression="FEES_REMARK" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_FEES_REMARK" runat="server" Text='<%#Bind("FEES_REMARK")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--自付金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="FEES_SELF" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_FEES_SELF" runat="server" Text='<%#Bind("FEES_SELF","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--單位負擔--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_CMP%>" SortExpression="FEES_CMP" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_FEES_CMP" runat="server" Text='<%#Bind("FEES_CMP","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--合計金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES%>" SortExpression="FEES" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_FEES" runat="server" Text='<%#Bind("FEES","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--自付金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_SELF%>" SortExpression="TRACED_FEES_SELF" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_TRACED_FEES_SELF" runat="server" Text='<%#Bind("TRACED_FEES_SELF","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--單位負擔--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES_CMP%>" SortExpression="TRACED_FEES_CMP" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_TRACED_FEES_CMP" runat="server" Text='<%#Bind("TRACED_FEES_CMP","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--合計金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_FEES%>" SortExpression="TRACED_FEES" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_TRACED_FEES" runat="server" Text='<%#Bind("TRACED_FEES","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單合計保費--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_TOT%>" SortExpression="BILLS_TOT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_BILLS_TOT" runat="server" Text='<%#Bind("BILLS_TOT","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--本月代扣保費--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_FEES2%>" SortExpression="INS_FEES" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_INS_FEES" runat="server" Text='<%#Bind("INS_FEES","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--差異金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_DIFF_AMT%>" SortExpression="DIFF_AMT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_DIFF_AMT" runat="server" Text='<%#Bind("DIFF_AMT","{0:N0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比對結果說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PROCESS_MEMO%>" SortExpression="PROCESS_MEMO" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PROCESS_MEMO" runat="server" Text='<%#Bind("PROCESS_MEMO")%>' Width="250px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_function%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            
                             <aces:Btn ID="WFB2IA3201Level_Chg" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                             <aces:Btn ID="WFB2IA3200Trace" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Trace%>" />
                             
                           <%-- <asp:Button ID="WFB2IA3201Level_Chg" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                            <asp:Button ID="WFB2IA3200Trace" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Trace%>" />--%>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>

            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1470px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1"  ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LICENSE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_emp_id%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_prid_cd%>" SortExpression="EMP_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CD_NAME" runat="server" Text='<%#Bind("EMP_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CHG_CD_NAME" runat="server" Text='<%#Bind("EMP_CHG_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>" SortExpression="LAST_UPDATE_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_LAST_UPDATE_DT" runat="server" Text='<%#Bind("LAST_UPDATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單投保金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_INS_AMT%>" SortExpression="BILLS_INS_AMT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_BILLS_INS_AMT" runat="server" Text='<%#Bind("BILLS_INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單自付金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_FEES%>" SortExpression="BILLS_FEES" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_BILLS_FEES" runat="server" Text='<%#Bind("BILLS_FEES","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_ins_fees%>" SortExpression="INS_FEES" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_INS_FEES" runat="server" Text='<%#Bind("INS_FEES","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_DIFF_AMT%>" SortExpression="DIFF_AMT1" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_DIFF_AMT1" runat="server" Text='<%#Bind("DIFF_AMT1","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比對結果說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PROCESS_MEMO%>" SortExpression="PROCESS_MEMO" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PROCESS_MEMO" runat="server" Text='<%#Bind("PROCESS_MEMO")%>' Width="250px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_function%>" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2IA3202Level_Chg" runat="server" CommandName="ChangLeve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                           <aces:Btn ID="WFB2IA3201Trace" runat="server" CommandName="TraceFeesB" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Trace%>" />
                           <%-- <asp:Button ID="WFB2IA3202Level_Chg" runat="server" CommandName="ChangLeve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                            <asp:Button ID="WFB2IA3201Trace" runat="server" CommandName="TraceFeesB" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Trace%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>

            <asp:GridView ID="gv_result3" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1620px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1"  ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LICENSE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_emp_id%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_CD%>" SortExpression="EMP_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CD_NAME" runat="server" Text='<%#Bind("EMP_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CHG_CD_NAME" runat="server" Text='<%#Bind("EMP_CHG_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>" SortExpression="LAST_UPDATE_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="70px" Style="text-align: left;" ID="lb_LAST_UPDATE_DT" runat="server" Text='<%#Bind("LAST_UPDATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單投保金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_INS_AMT%>" SortExpression="BILLS_INS_AMT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_BILLS_INS_AMT" runat="server" Text='<%#Bind("BILLS_INS_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單自提率%--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_RATE%>" SortExpression="RATE" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_RATE" runat="server" Text='<%#Bind("RATE","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單自付額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_FEES%>" SortExpression="BILLS_FEES" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_BILLS_FEES" runat="server" Text='<%#Bind("BILLS_FEES","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--代扣自付額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_ins_fees%>" SortExpression="INS_FEES" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_INS_FEES" runat="server" Text='<%#Bind("INS_FEES","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_DIFF_AMT%>" SortExpression="DIFF_AMT1" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: right;" ID="lb_DIFF_AMT1" runat="server" Text='<%#Bind("DIFF_AMT1","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比對結果說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PROCESS_MEMO%>" SortExpression="PROCESS_MEMO" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PROCESS_MEMO" runat="server" Text='<%#Bind("PROCESS_MEMO")%>' Width="250px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_function%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2IA3203Level_Chg" runat="server" CommandName="ChangLeve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                            <aces:Btn ID="WFB2IA3202Trace" runat="server" CommandName="TraceFeesC" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3202Trace%>" />
                           <%-- <asp:Button ID="WFB2IA3203Level_Chg" runat="server" CommandName="ChangLeve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                            <asp:Button ID="WFB2IA3202Trace" runat="server" CommandName="TraceFeesC" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3202Trace%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>

            <asp:GridView ID="gv_result4" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1460px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1"   ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_COMPANY_CD%>" SortExpression="COMPANY_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_COMPANY_SNAME" runat="server" Text='<%#Bind("COMPANY_SNAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LICENSE_ID%>" SortExpression="LICENSE_ID" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_LICENSE_ID" runat="server" Text='<%#Bind("LICENSE_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_emp_id%>" SortExpression="EMP_ID" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ib_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sa_EMP_CD%>" SortExpression="EMP_CD_NAME" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CD_NAME" runat="server" Text='<%#Bind("EMP_CD_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dd_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD_NAME" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMP_CHG_CD_NAME" runat="server" Text='<%#Bind("EMP_CHG_CD_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_LAST_UPDATE_DT%>" SortExpression="LAST_UPDATE_DT" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_LAST_UPDATE_DT" runat="server" Text='<%#Bind("LAST_UPDATE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單投保金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_INS_AMT%>" SortExpression="BILLS_INS_AMT" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: right;" ID="lb_BILLS_INS_AMT" runat="server" Text='<%#Bind("BILLS_INS_AMT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--帳單雇主提撥額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_BILLS_FEES2%>" SortExpression="BILLS_FEES" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Width="140px" Style="text-align: right;" ID="lb_BILLS_FEES" runat="server" Text='<%#Bind("BILLS_FEES","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--代扣雇主提撥額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_INS_FEES3%>" SortExpression="INS_FEES" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label Width="140px" Style="text-align: right;" ID="lb_INS_FEES" runat="server" Text='<%#Bind("INS_FEES","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_DIFF_AMT%>" SortExpression="DIFF_AMT1" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: right;" ID="lb_DIFF_AMT1" runat="server" Text='<%#Bind("DIFF_AMT1","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比對結果說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_lb_PROCESS_MEMO%>" SortExpression="PROCESS_MEMO" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PROCESS_MEMO" runat="server" Text='<%#Bind("PROCESS_MEMO")%>' Width="250px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_function%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2IA3204Level_Chg" runat="server" CommandName="ChangLeve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />
                            <%--<asp:Button ID="WFB2IA3204Level_Chg" runat="server" CommandName="ChangLeve" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" Text="<%$Resources:Resource,wfb2ia_WFB2IA3201Level_Chg%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA3200ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidconfirm_Execute" Value="<%$Resources:Resource,confirm_Execute%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>

