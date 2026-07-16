<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sm/WFB2SM3100_Qry.aspx.cs" Inherits="WebContent_WFB2SM3100_Qry" Culture="auto" UICulture="auto" %>

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
            gridviewScroll();
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
            //寫在這，按查詢才不會消失
            $('#txt_DEPT_NAME').attr("readonly", true);
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
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME_lb').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME_lb').val(JData.DEPT_NAME);
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

        function doUnblock() {
            $.unblockUI();
        }
        function doBlock() {
            BlockUI();
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
        //資料生成前檢核
        function CheckExecute(msg) {
            var processed = true;
            var year =$("#txt_ASSESS_YEAR").val();
            var maxYear = $("#HID_MAX_ASSESS_YEAR").val();
            /*
            if (isNaN(year)==false) {
                if (year > maxYear) {
                    alert("尚未有" + year + "年度的考核資料");
                    return false;
                }
            } else {
                return false;
            }
            */
            if (Page_ClientValidate("GroupA")) {
                if (confirm("確定要進行" + msg + "?")) {
                    processed = true;
                    BlockUI();
                } else {
                    processed = false;
                }
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
        }
       
        //資料下載
        function checkDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
               BlockUI();
            }
            else {
                processed = false;
                return false;
            }
            if (processed) {
                processed = confirm("確定要進行" + msg + "?");
                BlockUI();
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //清空畫面
        function ClearAll() {
            var day = new Date();
            var year = day.getFullYear();
            $("#txt_ASSESS_YEAR").val(year);
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_LEVEL_CD").val("");
            //$("#txt_DEPT_NAME").val("");
            $("#txt_DEPT_NAME_lb").val("");
            $("#ddl_WS_CD").val("-1");
        }

        function checkYear() {
            var re = /^\d\d\d\d$/;
            if (re.test($("#txt_ASSESS_YEAR").val())) {
                return true;
            }
            else {
               //$("#txt_NEW_DATA_YEAR").val("");
                alert('年度輸入錯誤');
                return false;
            }
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
                    <col width="35%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--年度--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sm_YEAR%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_required_YEAR%>"
                                ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證長度需為4 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--資格--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_level_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_LEVEL_CD" runat="server" Width="40px" ClientIDMode="Static" MaxLength="3"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--職種--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_ws_cd%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left">
                            <%-- 
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_dept_name%>"></asp:Label>:</th>
                            --%>
                            <td align="left"></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--部門代號--%> 
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_dept_no%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME_lb', 'N', '');" />
                            <asp:TextBox ID="txt_DEPT_NAME_lb" runat="server" Width="200px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>
                          <%--部門名稱--%>
                        <%-- 
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_dept_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="200px" ClientIDMode="Static" ></asp:TextBox>
                        </td>
                        --%>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SM3100Execute" runat="server"  Text="資料生成" OnClick="WFB2SM3100Execute_Click" OnClientClick="return CheckExecute(this.value);"/>
                            <aces:Btn ID="WFB2SM3100Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SM3100Search_Click" OnClientClick="return CheckSearch();" />
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sj_btn_clear%>" onclick="ClearAll();" />
                            <%-- 
                            <asp:Button ID="WFB2SM3100Execute" runat="server"  Text="資料生成" OnClick="WFB2SM3100Execute_Click" OnClientClick="return CheckExecute(this.value);"/>
                            <asp:Button ID="WFB2SM3100Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SM3100Search_Click" OnClientClick="return CheckSearch();" />
                            --%>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2SM3100ExcelDown" runat="server" Text="資料下載" Visible="false" OnClick="WFB2SM3100ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                <%-- 
                                <asp:Button ID="WFB2SM3100ExcelDown" runat="server" Text="資料下載" Visible="false" OnClick="WFB2SM3100ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                                --%>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SM3100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_ASSESS_YEAR"
                        Name="assess_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_LEVEL_CD"
                        Name="level_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_WS_CD"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <%-- 
                    <asp:ControlParameter ControlID="txt_DEPT_NAME"
                        Name="dept_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                        --%>
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
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_assess_year%>" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text='<%#Bind("ASSESS_YEAR")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_pjob_cd%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_emp_id%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_emp_name%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_dept_no%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部課--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_dept_20_name%>" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME_DESC" runat="server" Text='<%#Bind("DEPT_NAME_DESC")%>' Width="300px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_ws_cd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_pjob_desc%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" SortExpression="PJOB_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格年資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_recent_level_work_years%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right" SortExpression="RECENT_LEVEL_WORK_YEARS">
                        <ItemTemplate>
                            <asp:Label ID="lb_RECENT_LEVEL_WORK_YEARS" runat="server" Text='<%#Bind("RECENT_LEVEL_WORK_YEARS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  年齡--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_age%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="right" SortExpression="AGE">
                        <ItemTemplate>
                            <asp:Label ID="lb_AGE" runat="server" Text='<%#Bind("AGE")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--入社年資--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_work_years%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="right" SortExpression="WORK_YEARS">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_YEARS" runat="server" Text='<%#Bind("WORK_YEARS")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--健檢三高加班管控對象--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_OVERTIME_CTL_CD%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="left" SortExpression="OVERTIME_CTL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_CTL_CD" runat="server" Text='<%#Bind("OVERTIME_CTL_CD_DESC")%>' Width="200px"></asp:Label>
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
            <asp:HiddenField ID="HID_MAX_ASSESS_YEAR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SM3100ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
