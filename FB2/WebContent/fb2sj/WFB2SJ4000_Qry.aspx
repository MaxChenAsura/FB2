<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ4000_Qry.aspx.cs" Inherits="WebContent_WFB2SJ4000_Qry" Culture="auto" UICulture="auto" %>
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
            $('.date').mask('9999/99/99');
            $(".year").mask('9999');

            //GridView必須
            //gridviewScroll();
            $.unblockUI();
        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 6

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }

         var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            return HaveCheck;
        }

        //查詢前檢核
        function CheckSearch() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                processed = checkYear();
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }

        function checkYear() {
            var startYear = $("#txt_ASSESS_YEAR_S").val();
            var endYear = $("#txt_ASSESS_YEAR_E").val();
            var day = new Date();
            var year = day.getFullYear();
            var errMsg = "";

            if (startYear < (year - 5)) {
                errMsg += "僅能查前5年的資料\r\n";
                //alert("僅能查前5年的資料");

            }
            if (endYear > year) {
                errMsg += "年度(迄)不可大於系統年\r\n";
            }
            if (errMsg == "") {
                return true;
            } else {
                alert(errMsg);
                return false;
            }
        }
    

        //清空畫面
        function ClearAll() {
            var day = new Date();
            var year = day.getFullYear();
            $("#txt_ASSESS_YEAR_S").val(year);
            $("#txt_ASSESS_YEAR_E").val(year);
            $("#ddl_ASSESS_TYPE").val("-1");
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="30%" />
                    <col width="20%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--考核年度--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_year%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_ASSESS_YEAR_S" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                            ~ 
                           <asp:TextBox ID="txt_ASSESS_YEAR_E" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year" ></asp:TextBox>
                             <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sj_required_assess_year_s%>"
                                ControlToValidate="txt_ASSESS_YEAR_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="qry2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sj_required_assess_year_e%>"
                                ControlToValidate="txt_ASSESS_YEAR_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                             <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_ASSESS_YEAR_S"
                                            ControlToValidate="txt_ASSESS_YEAR_E" ErrorMessage="<%$Resources:Resource,wfb2sj_error_assess_year_s_e%>" Type="Integer" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <!--驗證長度需為4 -->	
								<asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_ASSESS_YEAR_S" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_e%>" ControlToValidate="txt_ASSESS_YEAR_E" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </td>
						<th align="left" class="Body_TableHeader">
                            <%--考核類別--%>
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SJ4000Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ4000Search_Click"   OnClientClick="return CheckSearch();"/>
                            <%--<asp:Button ID="WFB2SJ4000Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ4000Search_Click"   OnClientClick="return CheckSearch();"/>--%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sj_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData" 
                SelectCountMethod="getCount" TypeName="CFB2SJ4000DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
					<asp:ControlParameter ControlID="txt_ASSESS_YEAR_S"
                        Name="assess_year_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_ASSESS_YEAR_E"
                        Name="assess_year_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ASSESS_TYPE"
                        Name="assess_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                   
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                            <!-- 資料凍結註記-->
                            <asp:HiddenField ID="hid_FREEZE_FLAG" runat="server" Value='<%#Bind("FREEZE_FLAG")%> ' ClientIDMode="Static" />
                            <!-- 對象生成日-->
                            <asp:HiddenField ID="hid_TARGET_GEN_DT" runat="server" Value='<%#Bind("TARGET_GEN_DT")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
					<%--考核年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_year%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="ASSESS_YEAR">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text='<%#Bind("ASSESS_YEAR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_type%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="ASSESS_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>' Width="100px"></asp:Label>
                              <!-- 考核類別 -->
                            <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" Value='<%#Bind("ASSESS_TYPE_DESC")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
					<%--核可狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_approve_status%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text='<%#Bind("APPROVE_STATUS_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
					<%--提出核可人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_release_by%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="RELEASE_BY">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_BY" runat="server" Text='<%#Bind("RELEASE_BY_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_release_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可人員--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_approve_by%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_BY">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_BY" runat="server" Text='<%#Bind("APPROVE_BY_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_approve_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核發佈日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_release_dt%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="ASSESS_RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_RELEASE_DT" runat="server" Text='<%#Bind("ASSESS_RELEASE_DT", "{0:yyyy/MM/dd}")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_function%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btn_detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sj_btn_detail%>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

            </asp:GridView>

            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

			<!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
