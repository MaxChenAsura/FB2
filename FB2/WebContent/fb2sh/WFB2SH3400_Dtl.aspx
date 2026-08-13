<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sh/WFB2SH3400_Dtl.aspx.cs" Inherits="WebContent_WFB2SH3400_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        $(function () {

            iniForm();
        });
        var li_id;
        function iniForm() {
            $('#ddlPerPageRow').css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $('#ddlPerPageRow2').css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $(".txtDisabled").css("background-color", "white").css("color", "black").css("border-width", "0");
            //GridView必須
            $.unblockUI();
            gridviewScroll();
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
            $("#tabs").tabs();
            $('#ul li').click(function () {
                li_id = $(this).attr('id');
            });
            if ($("#hid_tab_id").val() != "")
                li_id = $("#hid_tab_id").val();
            ChangeTab(li_id);

        };

        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
            $("#hid_tab_id").val("");
        }


        function ShowRecord(obj) {
            $("#hid_tab_id").val(li_id);
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
        };

        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 7

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

        };


        var choietr = null;
        //回傳目前Checkbox被勾選的數量
        function checkboxsSelected() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            $(":checkbox:checked").each(function () {
                HaveCheck++;
            });
            /*
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                    if (choietr == null) {
                        choietr = i;
                    }
                }
            }
            */
            return HaveCheck;
        };


        //核可,檢查是否有勾選
        function checkApprove() {
            var processed = true;
            BlockUI();
            var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());
            //if (checkboxsSelected() > 0) {
            //    alert("請取消異常註記!");
            //    processed = false;
            //} else {
            processed = confirm("確定要核可?");
            //}

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        };

        //駁回,檢查是否有勾選
        function checkReject() {
            var processed = true;
            BlockUI();
            var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());
            if (remark == "" /* && checkboxsSelected() == "0"*/) {
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
        //年獎相關資料下載
        function checkDowning(msg) {
            var processed = true;
            BlockUI();
            processed = confirm("確定要進行" + msg);
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
        //清空
        function doClear() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            return false;
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
                    <col width="15%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="15%" />
                    <col width="5%" />
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
                            <!--年獎總金額-->
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_award_total_amount%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AWARD_TOTAL_AMOUNT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--年獎對象總人數-->
                            <asp:Label ID="Label10" runat="server" Text=" <%$Resources:Resource,wfb2sh_lb_award_total_decimal%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_AWARD_TOTAL_PEOPLE" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <!--備註說明-->
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_remark%>"></asp:Label>:
                        </th>
                        <td colspan="5">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--Excel下載功能-->
                            <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2sh_lb_exceldown%>"></asp:Label>:
                        </th>
                        <td colspan="3">

                            <aces:Btn ID="WFB2SH0401ExcelDown1" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_approvedata%>" OnClick="WFB2SH0401ExcelDown1_Click" OnClientClick="return checkDowning(this.value);" />
                            <%-- 
                            <asp:Button ID="WFB2SH0401ExcelDown1" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_approvedata%>" OnClick="WFB2SH0401ExcelDown1_Click" OnClientClick="return checkDowning(this.value);" />
                            <asp:Button ID="WFB2SH0401ExcelDown2" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_approve_pre%>" OnClick="WFB2SH0401ExcelDown2_Click" OnClientClick="return checkDowning(this.value);" />
                            <asp:Button ID="WFB2SH0401ExcelDown3" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_approve_original%>" OnClick="WFB2SH0401ExcelDown3_Click" OnClientClick="return checkDowning(this.value);" />
                            --%>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <asp:Button ID="WFB2SH3400Back" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SH3400Back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_EMP_ID%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass=" empid"></asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <%--<th></th>--%>
                        <td align="right" class="Body_label" colspan="3">
                            <div id="init_grid">

                                <aces:Btn ID="WFB2SH0401Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2SH0401Search_Click" OnClientClick="BlockUI();" />
                                <%--
                                <asp:Button ID="WFB2SH0401Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2SH0401Search_Click" OnClientClick="BlockUI();" />
                                --%>

                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />

                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3">
                            <aces:Btn ID="WFB2SH3400Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SH3400Mark_Click" OnClientClick="BlockUI();" />

                            <%--
                            <asp:Button ID="WFB2SH3400Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClick="WFB2SH3400Mark_Click" OnClientClick="BlockUI();" />
                            --%>
                        </td>
                        <td align="right" colspan="3">

                            <aces:Btn ID="WFB2SH3400Approve" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_approve%>" OnClick="WFB2SH3400Approve_Click" OnClientClick="return checkApprove();" />
                            <aces:Btn ID="WFB2SH3400Reject" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_reject%>" OnClick="WFB2SH3400Reject_Click" OnClientClick="return checkReject();" />
                            <%--
                            <asp:Button ID="WFB2SH3400Approve" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_approve%>" OnClick="WFB2SH3400Approve_Click" OnClientClick="return checkApprove();" />
                            <asp:Button ID="WFB2SH3400Reject" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_reject%>" OnClick="WFB2SH3400Reject_Click" OnClientClick="return checkReject();" />
                            --%>
                        </td>
                    </tr>

                </tbody>
            </table>

            <!-- 頁籤 開始-------------------  -->
            <div id="tabs" style="width: 1020px">                
                <div >
                    <br />
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                        SelectCountMethod="getCountDtl" TypeName="CFB2SH3400DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="txt_AWARD_YEAR"
                                Name="award_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                           
                            <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue=""
                                Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                            <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                                Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                        OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_approve_mark%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" Value='<%#Bind("APPROVE_MARK")%> ' ClientIDMode="Static" />
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="40px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--序號--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--異動狀態--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_chg_status%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="CHG_STATUS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>' Width="70px"></asp:Label>
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
                           
                            <%--職能俸--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_ability_pay%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right" SortExpression="ABILITY_PAY">
                                <ItemTemplate>
                                    <asp:Label ID="lb_ABILITY_PAY" runat="server" Text='<%#Bind("ABILITY_PAY","{0:n0}")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <%--今回考核--%>
                            <asp:TemplateField HeaderText="今回考核" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="left" SortExpression="SCORE_FINAL">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SCORE_FINAL" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--年奬係數--%>
                            <asp:TemplateField HeaderText="年奬係數" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="right" SortExpression="AWARD_DIFFER">
                                <ItemTemplate>
                                    <asp:Label ID="lb_AWARD_DIFFER" runat="server" Text='<%#Bind("AWARD_DIFFER","{0:n0}")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--年資獎金--%>
                            <asp:TemplateField HeaderText="年資獎金" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="right" SortExpression="AWARD_DAYS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_AWARD_DAYS" runat="server" Text='<%#Bind("AWARD_DAYS")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--計算起日--%>
                            <asp:TemplateField HeaderText="計算起日" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="left" SortExpression="AWARD_SDT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_AWARD_SDT" runat="server" Text='<%#Bind("AWARD_SDT")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--計算訖日--%>
                            <asp:TemplateField HeaderText="計算訖日" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="left" SortExpression="AWARD_EDT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_AWARD_EDT" runat="server" Text='<%#Bind("AWARD_EDT")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--在職天數--%>
                            <asp:TemplateField HeaderText="在職天數" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="right" SortExpression="JOB_DT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_JOB_DT" runat="server" Text='<%#Bind("JOB_DT")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--事假扣除天數--%>
                            <asp:TemplateField HeaderText="事假扣除天數" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="right" SortExpression="PERSONAL_LEAVE_DAYS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_PERSONAL_LEAVE_DAYS" runat="server" Text='<%#Bind("PERSONAL_LEAVE_DAYS")%>' Width="70px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--有薪病假扣除天數--%>
                            <asp:TemplateField HeaderText="有薪病假扣除天數" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="left" SortExpression="PERSONAL_LEAVE_DAYS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_PERSONAL_LEAVE_DAYS" runat="server" Text='<%#Bind("PERSONAL_LEAVE_DAYS")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--實際在職天數--%>
                            <asp:TemplateField HeaderText="實際在職天數" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="left" SortExpression="ACTUAL_JOB_DAYS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_ACTUAL_JOB_DAYS" runat="server" Text='<%#Bind("ACTUAL_JOB_DAYS")%>' Width="120px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--紀律扣除金額--%>
                            <asp:TemplateField HeaderText="紀律扣除金額" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RULE_DECAMT" runat="server" Text='<%#Bind("RULE_DECAMT","{0:n0}")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--實際年終獎金--%>
                            <asp:TemplateField HeaderText="實際年終獎金" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lb_ACTUAL_AMT" runat="server" Text='<%#Bind("ACTUAL_AMT","{0:n0}")%>' Width="80px"></asp:Label>
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
                </div>
                

                
               
            <!-- 頁籤 結束-------------------  -->

            <!-- SH040_Qry的查詢條件 -->
            <asp:HiddenField ID="HID_FREEZE_FLAG" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />

            <!-- -->


            <!-- 是否為supervisor  -->
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_tab_id" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SH0401ExcelDown1" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
