<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0300_Dtl.aspx.cs" Inherits="WebContent_WFB2SJ0300_Dtl" Culture="auto" UICulture="auto" %>
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
        //核可,檢查是否有勾選
        function checkApprove() {
            var processed = true;
            if (confirm("確定要核可?")) {
                processed = true;
            } else {
                processed = false;
            }

            //if (checkboxsSelected() > 0) {
            //    alert("請取消異常註記!");
            //    processed = false;
            //} else {
            //    processed = confirm("確定要核可?");
            //}
            if (!processed) {
                $.unblockUI();
            } else {
                BlockUI();
            }
            return processed;

        };

        function checkMark() {
            var processed = true;
            BlockUI();
            if (checkboxsSelected() > 0) {
                //processed = confirm("確定要進行一括註記?");
                processed = true;
            } else {
                //alert("請選取異常註記 ");
                //processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        }

        //駁回,檢查是否有勾選
        function checkReject() {
            var processed = true;
            BlockUI();
            var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());
            if (remark == "" /*  && checkboxsSelected() == "0"*/) {
                alert("請輸入備註說明!");
                processed = false;
            } else {
                processed = confirm("確定要駁回?");
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        };

        //考核結果下載
        function checkDowning(msg) {
            var processed = true;
            BlockUI();
            processed = confirm("確定要進行" + msg);
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_LEVEL_CD").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_WS_CD").val("-1");
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
                            <!--考核年度-->
                            <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_year%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--考核類別-->
                            <asp:Label ID="Label29" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_type%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--考核發佈日期-->
                            <asp:Label ID="Label32" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_assess_release_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_ASSESS_RELEASE_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--核可狀態-->
                            <asp:Label ID="Label33" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_approve_status%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" CssClass="txtDisabled" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <!--備註說明-->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_remark%>"></asp:Label>:
                        </th>
                        <td colspan="3">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--Excel下載功能-->
                            <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_function%>"></asp:Label>:
                        </th>
                        <td colspan="3">
                            <aces:Btn ID="WFB2SJ0300ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_assess_result_down%>" OnClick="WFB2SJ0300ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />
                            <%--<asp:Button ID="WFB2SJ0300ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_assess_result_down%>" OnClick="WFB2SJ0300ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4">
                            <asp:Button ID="WFB2SJ0300Back" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SJ0300Back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="80px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <aces:Btn ID="WFB2SJ0301Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ0301Search_Click" OnClientClick="BlockUI();" />
                            <%--<asp:Button ID="WFB2SJ0301Search" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_search%>" OnClick="WFB2SJ0301Search_Click" OnClientClick="BlockUI();" />--%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sj_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <aces:Btn ID="WFB2SJ0300Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SJ0300Mark_Click" OnClientClick="return checkMark();" />
                            <%--<asp:Button ID="WFB2SJ0300Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SJ0300Mark_Click" OnClientClick="return checkMark();" />--%>
                        </td>
                        <td align="right" colspan="2">
                            <aces:Btn ID="WFB2SJ0300Approve" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_approve%>" OnClick="WFB2SJ0300Approve_Click" OnClientClick="return checkApprove();" />
                            <aces:Btn ID="WFB2SJ0300Reject" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_reject%>" OnClick="WFB2SJ0300Reject_Click" OnClientClick="return checkReject();" />
                           <%-- <asp:Button ID="WFB2SJ0300Approve" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_approve%>" OnClick="WFB2SJ0300Approve_Click" OnClientClick="return checkApprove();" />
                            <asp:Button ID="WFB2SJ0300Reject" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_reject%>" OnClick="WFB2SJ0300Reject_Click" OnClientClick="return checkReject();" />--%>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                SelectCountMethod="getCountDtl" TypeName="CFB2SJ0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_ASSESS_YEAR"
                        Name="assess_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="HID_ASSESS_TYPE"
                        Name="assess_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
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
                    <%--異常註記--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_approve_mark%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" Value='<%#Bind("APPROVE_MARK")%> ' ClientIDMode="Static" />
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="40px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_rownumber%>" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_level_cd%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--當年昇格--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_levelup_flag%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVELUP_FLAG">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVELUP_FLAG" runat="server" Text='<%#Bind("LEVELUP_FLAG")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--跨部異動--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_dept_flag%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_FLAG">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_FLAG" runat="server" Text='<%#Bind("DEPT_FLAG")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--修改註記--%>
                    <asp:TemplateField HeaderText="修改註記" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="CHG_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_emp_id%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_emp_name%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_dept_no_default%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" SortExpression="DEPT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_NO")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部課--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_dept_name_default%>" HeaderStyle-Width="260px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME_DESC")%>' Width="260px"></asp:Label>
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_recent_level_work_years%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="right" SortExpression="RECENT_LEVEL_WORK_YEARS">
                        <ItemTemplate>
                            <asp:Label ID="lb_RECENT_LEVEL_WORK_YEARS" runat="server" Text='<%#Bind("RECENT_LEVEL_WORK_YEARS")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--年齡--%>
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

                    <%--前年度考績--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_score_ly%>" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="center" SortExpression="SCORE_LY">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_LY" runat="server" Text='<%#Bind("SCORE_LY")%>' Width="90px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--部門提出--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_score_dept%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="SCORE_DEPT">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_DEPT" runat="server" Text='<%#Bind("SCORE_DEPT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--最終考績--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sj_lb_score_final%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="SCORE_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_FINALS" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="80px"></asp:Label>
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
                            <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!-- SJ020_Qry的查詢條件 -->
            <asp:HiddenField ID="HID_FREEZE_FLAG" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />


            <asp:HiddenField ID="HID_ASSESS_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_resultMarkCount" runat="server" ClientIDMode="Static" />
            <!-- 是否為supervisor  -->
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>

            <asp:PostBackTrigger ControlID="WFB2SJ0300ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
