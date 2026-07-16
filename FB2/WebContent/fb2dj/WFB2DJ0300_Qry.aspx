<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dj/WFB2DJ0300_Qry.aspx.cs" Inherits="WebContent_WFB2DJ0300_Qry" Culture="auto" UICulture="auto" %>

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
            //GridView凍結視窗必須
            gridviewScroll();
            $.unblockUI();

            //$('#txt_qry_EMP_NAME').attr("readonly", true);
            $('#txt_qry_DEPT_NAME').attr("readonly", true);
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
                                $('#txt_qry_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_DEPT_NAME').val("");
                }
            });
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
                // CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }

        //刪除,檢查是否有勾選
        function doDelete() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                processed = confirm("確定要刪除?");
            } else {
                alert("請選取資料!");
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
            //var processed = false;
            //if (checkboxsSelected() == 0) {
            //    processed = confirm("確定要刪除?");
            //}else{

            //}

            //BlockUI();
            //processed = confirm("確定要刪除?");
            //if (!processed) {
            //    $.unblockUI();
            //}
            //return processed;
        }

        //修改時檢核
        function doPlus() {
            var processed = false;
            if (LookUpCheckboxs() != 1) {
                alert("請選取一筆資料!");
                return processed;
            }
            BlockUI();
            processed = confirm("確定更改計薪狀態更為加扣項？注意:更改後則無法異動！");
            if (!processed) {
                $.unblockUI();
            }
            return processed;

        }

        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function LookUpCheckboxs() {
            var result = 0;
            $(":checkbox[id$=cb_check_freezeitem]").each(function (index, ele) {
                if (ele.checked) {
                    result += 1;
                }
            });
            return result;

        }
        //查詢前檢核
        function CheckSearch() {
            //其它需要檢核的
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = false;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                processed = true;
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
        }

        //清空畫面
        function ClearAll() {
            var today = getTodayDate();
            $("#txt_APPLY_DT_S").val(today);
            $("#txt_APPLY_DT_E").val(today);
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_qry_DEPT_NAME").val("");
            //$('#txt_qry_EMP_NAME').val("");
            $("#txt_IFLOW_NO").val("");
            $("#ddl_ENV_CHECK_STATUS").val("-1");
            $("#ddl_ENV_SALARY_STATUS").val("-1");
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
                    <col width="20%" />
                    <col width="10%" />
                    <col width="30%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--津貼期間--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_apply_dt_s_e%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_APPLY_DT_S" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            ~ 
                           <asp:TextBox ID="txt_APPLY_DT_E" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_apply_dt_s%>"
                                ControlToValidate="txt_APPLY_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="qry2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dj_required_apply_dt_e%>"
                                ControlToValidate="txt_APPLY_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qry3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dj_format_apply_dt_s%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                            <!--驗證日期格式-->
                            <asp:CustomValidator ID="qry4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dj_format_apply_dt_e%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_APPLY_DT_S"
                                ControlToValidate="txt_APPLY_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dj_error_apply_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                            <%--  
                             <asp:TextBox ID="txt_qry_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                            --%>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--部門代號--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_dept_no%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" Width="70px" ClientIDMode="Static" MaxLength="7"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_qry_DEPT_NAME', 'N', '');" />
                            <asp:TextBox ID="txt_qry_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="140px" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--比對狀態--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_check_status%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ENV_CHECK_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--計薪狀態--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_env_salary_status%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ENV_SALARY_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--表單編號--%>
                            <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2dj_lb_iflow_no%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2DJ0300Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DJ0300Search_Click" OnClientClick="return CheckSearch();" />

                            <%--<asp:Button ID="WFB2DJ0300Search" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_search%>" OnClick="WFB2DJ0300Search_Click" OnClientClick="return CheckSearch();" />--%>

                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dj_btn_clear%>" onclick="ClearAll();" />
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
                                <aces:Btn ID="WFB2DJ0300Plus" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_Plus%>" Visible="false" OnClientClick="return doPlus(); " OnClick="WFB2DJ0300Plus_Click" />
                                <aces:Btn ID="WFB2DJ0300Delete" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_delete%>" Visible="false" OnClick="WFB2DJ0300Delete_Click" OnClientClick="return doDelete(); " />

                                 <%--<asp:Button ID="WFB2DJ0300Plus" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_Plus%>" Visible="false" OnClientClick="return doPlus(); " OnClick="WFB2DJ0300Plus_Click" />--%>
                                <%--<asp:Button ID="WFB2DJ0300Delete" runat="server" Text="<%$Resources:Resource,wfb2dj_btn_delete%>" Visible="false" OnClick="WFB2DJ0300Delete_Click" OnClientClick="return doDelete(); " />--%>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DJ0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />

                    <asp:ControlParameter ControlID="hid_qry_APPLY_DT_S"
                        Name="appleDT_S" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_APPLY_DT_E" DefaultValue=""
                        Name="appleDT_E" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_EMP_ID"
                        Name="empID" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_EMP_NAME" DefaultValue=""
                        Name="empName" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_DEPT_NO"
                        Name="deptNO" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_ENV_CHECK_STATUS" DefaultValue=""
                        Name="checkStatus" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_ENV_SALARY_STATUS"
                        Name="salaryStatus" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_qry_IFLOW_NO" DefaultValue=""
                        Name="iflowNO" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />

                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_dept_name%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" SortExpression="DEPT_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Layout NO.--%>
                    <asp:TemplateField HeaderText="Layout NO." HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="LAYOUT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_LAYOUT_NOC" runat="server" Text='<%#Bind("LAYOUT_NO")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_emp_id%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_emp_name%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_dept_no%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="DEPT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--輪值表--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_work_shift_cd%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="WORK_SHIFT_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text='<%#Bind("WORK_SHIFT_CD")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--津貼等級--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_allowance_type%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="ENV_ALLOWANCE_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_ALLOWANCE_TYPE" runat="server" Text='<%#Bind("ENV_ALLOWANCE_TYPE")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--津貼日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_apply_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="APPLY_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPLY_DT" runat="server" Text='<%#Bind("APPLY_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--表單編號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_iflow_no%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" SortExpression="IFLOW_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text='<%#Bind("IFLOW_NO")%>' Width="200px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  申請時段--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_apply_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="ENV_APPLY_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_APPLY_CD" runat="server" Text='<%#Bind("ENV_APPLY_CD_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比對狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_check_status%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="ENV_CHECK_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_CHECK_STATUS" runat="server" Text='<%#Bind("ENV_CHECK_STATUS_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比對異常訊息--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_check_log%>" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="left" SortExpression="ENV_CHECK_LOG">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_CHECK_LOG" runat="server" Text='<%#Bind("ENV_CHECK_LOG_DESC")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計薪狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_env_salary_status%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left" SortExpression="ENV_SALARY_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_SALARY_STATUS" runat="server" Text='<%#Bind("ENV_SALARY_STATUS_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dj_lb_salary_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="SALARY_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--當DB無資料時，就會使用此table --%>
                <%-- <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>  
                                  <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>  
                                 <asp:Label ID="Label1" runat="server" Text="序號" Width="40px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>  </td>
                            <td>  </td>

                        </table>
                  </EmptyDataTemplate>--%>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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
            <!-- 查詢條件 -->
            <asp:HiddenField ID="hid_qry_APPLY_DT_S" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_APPLY_DT_E" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_DEPT_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_ENV_CHECK_STATUS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_ENV_SALARY_STATUS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_qry_IFLOW_NO" runat="server" ClientIDMode="Static" />


            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
