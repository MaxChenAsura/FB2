<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sh/WFB2SH3200_Dtl.aspx.cs" Inherits="WebContent_WFB2SH3200_Dtl" Culture="auto" UICulture="auto" %>

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
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");
            $(".txtDisabled").css("background-color", "white").css("color", "black").css("border-width", "0");
            gridviewScroll();
            $.unblockUI();
            //$(".envUnit").mask('9.9');
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
                    freezesize: 8

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
            //其它需要檢核的

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
                return;
            }

            return processed;
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

        }

        //檢核 支付狀態一括更新
        function CheckUpdateAction() {
            var processed = true;
            BlockUI();
            var processed = true;
            if (checkboxsSelected() > 0) {
                processed = confirm("確定更新支付狀態？");
            } else {
                alert("請選取資料!");
                choietr = null;
                processed = false;
            }
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function doBlock() {
            BlockUI();
        }
        function doUnBlock() {
            $.unblockUI();
        }
        //年獎相關資料下載
        function checkSampleDown(msg) {
            var processed = true;
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //檔案匯入檢驗
        function checkUpload(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupB")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_EMP_CHG_CD").val("-1");
            $("#txt_LEVEL_CD").val("");
            $("#ddl_qry_PAY_TYPE").val("-1");
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
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--年度-->
                            <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_year%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AWARD_YEAR" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                      <th align="left" class="Body_TableHeader">
                            <!--年獎發放日期-->
                            <asp:Label ID="Label31" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AWARD_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
              
                    <tr>
                        
                        <th align="left" class="Body_TableHeader">
                            <!--年獎對象總人數-->
                            <asp:Label ID="Label10" runat="server" Text=" <%$Resources:Resource,wfb2sh_lb_award_total_decimal%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AWARD_TOTAL_PEOPLE" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--年獎總金額-->
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_total_amount%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AWARD_TOTAL_AMOUNT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--核可狀態-->
                            <asp:Label ID="Label33" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_approve_status%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--薪資轉出日期-->
                            <asp:Label ID="Label32" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_salary_trans_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_SALARY_TRANS_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <!--備註說明-->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_remark%>"></asp:Label>:
                        </th>
                        <td colspan="5">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" Enabled="false" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--Excel下載功能-->
                            <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_exceldown%>"></asp:Label>:
                        </th>
                        <td colspan="3">

                            <aces:Btn ID="WFB2SH3201ExcelDown1" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_exceldownmaintain%>" OnClick="WFB2SH3201ExcelDown1_Click" OnClientClick="return checkDowning(this.value);" />

                            <%--
                            <asp:Button ID="WFB2SH3201ExcelDown1" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_exceldownmaintain%>" OnClick="WFB2SH3201ExcelDown1_Click" OnClientClick="return checkDowning(this.value);" />
                            <asp:Button ID="WFB2SH3201ExcelDown2" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_exceldownoriginal%>" OnClick="WFB2SH3201ExcelDown2_Click" OnClientClick="return checkDowning(this.value);" />
                            <asp:Button ID="WFB2SH3201ExcelDown3" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_exceldownlevel%>" OnClick="WFB2SH3201ExcelDown3_Click" OnClientClick="return checkDowning(this.value);" />
                            --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                                <colgroup>
                                    <col width="10%" />
                                    <col width="20%" />
                                    <col width="10%" />
                                    <col width="20%" />
                                    <col width="10%" />
                                    <col width="20%" />
                                    <col width="10%" />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <%--工號--%>
                                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_emp_id%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"></asp:TextBox>
                                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <%--姓名--%>
                                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_emp_name%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <th></th>
                                        <td align="right" colspan="6">

                                            <aces:Btn ID="WFB2SH3201Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SH3201Search_Click" OnClientClick="BlockUI();" />
                                            <%--
                                            <asp:Button ID="WFB2SH3201Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClick="WFB2SH3201Search_Click" OnClientClick="BlockUI();" />
                                            --%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sg_btn_clear%>" onclick="ClearAll();" />
                                            <asp:Button ID="WFB2SH3200Back" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SH3200Back_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>  
                                         <th align="left" class="Body_TableHeader">
                                            <%--刪除原因--%>
                                            <asp:Label ID="Label1" runat="server" Text="刪除原因"></asp:Label>:</th>
                                        <td align="left" class="Body_label" colspan="2">
                                            <asp:TextBox ID="txt_DELETE_MEMO" TextMode="MultiLine" Columns="50" Rows="2" runat="server"  BorderWidth="1"/>
                                        </td>                                     
                                        <td align="right" class="Body_label" colspan="4">
                                            <div id="init_grid">

                                                <aces:Btn ID="WFB2SH3201Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" OnClick="WFB2SH3201Delete_Click" OnClientClick="return doDelete();" />
                                                <%--
                                                <asp:Button ID="WFB2SH3201Delete" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_delete%>" OnClick="WFB2SH3201Delete_Click" OnClientClick="return doDelete();" />
                                                --%>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                SelectCountMethod="getCountDtl" TypeName="CFB2SH3200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />                 
                    <asp:ControlParameter ControlID="txt_AWARD_YEAR"
                        Name="award_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                    

                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異常註記--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_approve_mark%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_MARK">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>' Width="40px"></asp:Label> 
                            <!-- 凍結註記 -->
                            <asp:HiddenField ID="hid_DELETE_FLAG" runat="server" Value='<%#Bind("DELETE_FLAG")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--異動狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_chg_status%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="CHG_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_approve_flag%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_FLAG">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_ws_cd%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_level_cd%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_emp_id%>" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_emp_name%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務代號--%>
                    <asp:TemplateField HeaderText="職務代號" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務名稱--%>
                    <asp:TemplateField HeaderText="職務名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考績--%>
                    <asp:TemplateField HeaderText="考績" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="center" SortExpression="SCORE_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_FINAL" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年奬係數--%>
                    <asp:TemplateField HeaderText="年奬係數" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="AWARD_DIFFER">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_DIFFER" runat="server" Text='<%#Bind("AWARD_DIFFER","{0:n0}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年資獎金--%>
                    <asp:TemplateField HeaderText="年資獎金" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="AWARD_DAYS">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_DAYS" runat="server" Text='<%#Bind("AWARD_DAYS")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計算起日--%>
                    <asp:TemplateField HeaderText="計算起日" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="AWARD_SDT">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_SDT" runat="server" Text='<%#Bind("AWARD_SDT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計算訖日--%>
                    <asp:TemplateField HeaderText="計算訖日" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="AWARD_EDT">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_EDT" runat="server" Text='<%#Bind("AWARD_EDT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--在職天數--%>
                    <asp:TemplateField HeaderText="在職天數" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="JOB_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_JOB_DT" runat="server" Text='<%#Bind("JOB_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--事假扣除天數--%>
                    <asp:TemplateField HeaderText="事假扣除天數" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="PERSONAL_LEAVE_DAYS">
                        <ItemTemplate>
                            <asp:Label ID="lb_PERSONAL_LEAVE_DAYS" runat="server" Text='<%#Bind("PERSONAL_LEAVE_DAYS")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--有薪病假扣除天數--%>
                    <asp:TemplateField HeaderText="有薪病假<BR>扣除天數" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="SICK_LEAVE_DAYS">
                        <ItemTemplate>
                            <asp:Label ID="lb_SICK_LEAVE_DAYS" runat="server" Text='<%#Bind("SICK_LEAVE_DAYS","{0:n0}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--實際在職天數--%>
                    <asp:TemplateField HeaderText="實際在職天數" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="ACTUAL_JOB_DAYS">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACTUAL_JOB_DAYS" runat="server" Text='<%#Bind("ACTUAL_JOB_DAYS","{0:n0}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--紀律扣除金額--%>
                    <asp:TemplateField HeaderText="紀律<BR>扣除金額" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="RULE_DECAMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_RULE_DECAMT" runat="server" Text='<%#Bind("RULE_DECAMT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--應發年終獎金--%>
                    <asp:TemplateField HeaderText="應發<BR>年終獎金" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="SHOULD_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_SHOULD_AMT" runat="server" Text='<%#Bind("SHOULD_AMT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--稅額--%>
                    <asp:TemplateField HeaderText="稅額" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="TAX_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_TAX_AMT" runat="server" Text='<%#Bind("TAX_AMT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--實發年獎金額--%>
                    <asp:TemplateField HeaderText="實發年獎金額" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right" SortExpression="ACTUAL_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACTUAL_AMT" runat="server" Text='<%#Bind("ACTUAL_AMT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--刪除原因--%>
                    <asp:TemplateField HeaderText="刪除原因" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="left" SortExpression="DELETE_MEMO">
                        <ItemTemplate>
                            <asp:Label ID="lb_DELETE_MEMO" runat="server" Text='<%#Bind("DELETE_MEMO")%>' Width="100px"></asp:Label>
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

            <!-- SH320_Qry的查詢條件 -->
            <asp:HiddenField ID="HID_FREEZE_FLAG" runat="server" ClientIDMode="Static" />

            <!-- 是否為supervisor  -->
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
        
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
