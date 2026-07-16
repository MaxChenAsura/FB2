<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sg/WFB2SG0400_Dtl.aspx.cs" Inherits="WebContent_WFB2SG0400_Dtl" Culture="auto" UICulture="auto" %>
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
            //畫簽
            $("#tabs").tabs();

            reCommaOnly("FESTIVAL_TOTAL_AMT", 0);
            reCommaOnly("FESTIVAL_AMT", 0);
            reComma("FESTIVAL_AMT", 0);
            //GridView必須
            gridviewScroll();
            $.unblockUI();

            $('#tab2').click(function () {
                $('#<%=gv_result2.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"
                });
            });

            $('#tab1').one('click', function () {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 6

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            });

            $('#tab1').trigger('click');

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

        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
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


        //核可,檢查是否有勾選
        function checkApprove() {
            var processed = true;
            BlockUI();
            var remark = $.trim($("textarea[id $= 'txt_REMARK']").val());
            //if (remark != "") {
            //    alert("請取消異常註記 或  備註說明!");
            //    processed = false;
            //}
            //else
            if (checkboxsSelected() > 0) {
                alert("請取消異常註記");
                processed = false;
            } else {
                processed = confirm("確定要核可?");
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
            if (remark == "" /*&& checkboxsSelected() == 0*/) {
                alert("請輸入備註說明!");
                processed = false;
            } else {
                processed = confirm("確定要駁回?");
            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;

        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
        }
        ////下載
        //function checkDowning(msg) {
        //    var processed = true;
        //    BlockUI();
        //    processed = confirm("確定要進行" + msg);
        //    if (!processed) {
        //        $.unblockUI();
        //    }
        //    return processed;
        //}

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
                            <!--節金類別-->
                            <asp:Label ID="Label" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_type%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TYPE_DESC" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <!--節日日期-->
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_dt%>">:</asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <!--節金發放日期-->
                            <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_pay_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_PAY_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--節金總金額 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_total_amt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TOTAL_AMT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <%--節金對象總人數 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_festival_total_num%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_FESTIVAL_TOTAL_NUM" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%--薪資轉出日期 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_salary_trans_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_SALARY_TRANS_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                        <%--核可狀態 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_approve_status%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:TextBox ID="txt_APPROVE_STATUS" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>
                    </tr>
                    <tr>
                        <%--備註說明 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_remark%>"></asp:Label>:
                        </th>
                        <td colspan="5">
                            <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_Excel_Download" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_exceldown%>"></asp:Label>:
                        </th>
                        <td colspan="3">
                            <aces:Btn ID="WFB2SG0401ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_approvedata%>" OnClick="WFB2SG0401ExcelDown_Click"  OnClientClick="return checkDowning(this.value);"/>

                            <%--<asp:Button ID="WFB2SG0401ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_approvedata%>" OnClick="WFB2SG0401ExcelDown_Click" OnClientClick="return checkDowning(this.value);" />--%>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                            <asp:Button ID="WFB2SG0400Back" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_back%>" OnClientClick="BlockUI();" OnClick="WFB2SG0400Back_Click" />
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
                            <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5"> </asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', '');" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--姓名--%>
                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_name%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="80px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="5">
                             <aces:Btn ID="WFB2SG0401Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClientClick="BlockUI();" OnClick="WFB2SG0401Search_Click" />
                           <%-- <asp:Button ID="WFB2SG0401Search" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_search%>" OnClientClick="BlockUI();" OnClick="WFB2SG0401Search_Click" />--%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sg_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <aces:Btn ID="WFB2SG0400Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClientClick="return checkMark();" OnClick="WFB2SG0400Mark_Click" />
                            <%--
                            <asp:Button ID="WFB2SG0400Mark" runat="server" Text="<%$Resources:Resource,wfb2sj_btn_mark%>" OnClientClick="return checkMark();" OnClick="WFB2SG0400Mark_Click" />--%>
                        </td>
                        <td align="right" colspan="2">
                            <aces:Btn ID="WFB2SG0400Approve" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_approve%>" OnClick="WFB2SG0400Approve_Click" OnClientClick="return checkApprove();" />
                            <aces:Btn ID="WFB2SG0400Reject" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_reject%>" OnClick="WFB2SG0400Reject_Click" OnClientClick="return checkReject();" />

                            <%--<asp:Button ID="WFB2SG0400Approve" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_approve%>" OnClick="WFB2SG0400Approve_Click" OnClientClick="return checkApprove();" />
                            <asp:Button ID="WFB2SG0400Reject" runat="server" Text="<%$Resources:Resource,wfb2sg_btn_reject%>" OnClick="WFB2SG0400Reject_Click" OnClientClick="return checkReject();" />--%>
                        </td>
                    </tr>

                </tbody>
            </table>

            <!-- 頁籤 開始-------------------  -->
            <div id="tabs" style="width: 1020px">
                <ul>
                    <li id="tab1"><a href="#tabs-1"><span id="span_tabs_1">【節金發放對象】</span></a></li>
                    <li id="tab2"><a href="#tabs-2"><span id="span_tabs_2">【節金對象條件】</span></a></li>
                </ul>
                <div id="tabs-1">
                    <br />
                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                        SelectCountMethod="getCountDtl" TypeName="CFB2SG0400DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                        OnSelected="ods1_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="HID_FESTIVAL_TYPE"
                                Name="festival_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="txt_FESTIVAL_DT"
                                Name="festival_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="txt_FESTIVAL_PAY_DT"
                                Name="festival_pay_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                            <asp:ControlParameter ControlID="HID_RELEASE_DT"
                                Name="release_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_approve_mark%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="80px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--序號--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_rownumber%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hid_APPROVE_MARK" runat="server" Value='<%#Bind("APPROVE_MARK")%> ' ClientIDMode="Static" />
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="60px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--異常註記
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_approve_mark%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" >
                                <ItemTemplate>
                                    <asp:Label ID="lb_APPROVE_MARK" runat="server" Text='<%#Bind("APPROVE_MARK")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:TemplateField>
                            --%>
                            <%--異動狀態--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_chg_status%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="CHG_STATUS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS_DESC")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--核可--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_approve_flag%>" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="APPROVE_FLAG">
                                <ItemTemplate>
                                    <asp:Label ID="lb_APPROVE_FLAG" runat="server" Text='<%#Bind("APPROVE_FLAG")%>' Width="40px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--工號--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_id%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--姓名--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_name%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_NAME">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--員工區分--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_CD">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EMP_CD" runat="server" Text='<%#Bind("EMP_CD_DESC")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--資格代號--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_level_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="LEVEL_CD">
                                <ItemTemplate>
                                    <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--入社日期--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_join_dt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="center" SortExpression="JOIN_DT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--在職年資--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_work_days%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="WORK_DAYS">
                                <ItemTemplate>
                                    <asp:Label ID="lb_WORK_DAYS" runat="server" Text='<%#Bind("WORK_DAYS")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--節金金額--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_amt%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="FESTIVAL_AMT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT","{0:n0}")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--節金金額--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_amt_old%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FESTIVAL_AMT_OLD" runat="server" Text='<%#Bind("FESTIVAL_AMT_OLD","{0:n0}")%>' Width="120px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--支付狀態--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_pay_type%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="PAY_TYPE">
                                <ItemTemplate>
                                    <asp:Label ID="PAY_TYPE" runat="server" Text='<%#Bind("PAY_TYPE_DESC")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--支付狀態--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_pay_type_old%>" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="PAY_TYPE_OLD" runat="server" Text='<%#Bind("PAY_TYPE_OLD_DESC")%>' Width="120px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--在職區分--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_emp_chg_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="EMP_CHG_CD">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text='<%#Bind("EMP_CHG_CD_DESC")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--職務代號--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_pjob_cd%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="center" SortExpression="PJOB_CD">
                                <ItemTemplate>
                                    <asp:Label ID="PJOB_CD" runat="server" Text='<%#Bind("PJOB_CD")%>' Width="80px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <%--當DB無資料時，就會使用此table --%>
                        <HeaderStyle CssClass="GridviewScrollHeader" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                        <tr height="100%" valign="top">
                            <td class="GridviewScrollPager TD">
                                <asp:DropDownList ID="ddlPerPageRow" runat="server" ClientIDMode="Static" AutoPostBack="true" Style="font-size: 14px;">
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
                </div>
                <div id="tabs-2">
                    <br />
                    <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="getDataDtl2"
                        SelectCountMethod="getCountDtl2" TypeName="CFB2SG0400DAO" EnablePaging="True"
                        SortParameterName="sortExpression" OnSelecting="obs2_Selecting"
                        OnSelected="ods2_Selected">
                        <SelectParameters>
                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                            <asp:Parameter Name="maximumRows" Type="Int32" />
                            <asp:ControlParameter ControlID="HID_FESTIVAL_TYPE"
                                Name="festival_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="true" OnSorting="gv_result2_Sorting"
                        OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1020px"
                        OnPageIndexChanging="gv_result2_PageIndexChanging" OnDataBound="gv_result2_DataBound">
                        <Columns>
                            <%--序號--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_rownumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--節金類別--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_type%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FESTIVAL_TYPE" runat="server" Text='<%#Bind("FESTIVAL_TYPE_DESC")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--節金給付條件--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_pay_cond%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FESTIVAL_PAY_COND" runat="server" Text='<%#Bind("FESTIVAL_PAY_COND")%>' Width="200px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--節金給付金額--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_festival_amt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FESTIVAL_AMT" runat="server" Text='<%#Bind("FESTIVAL_AMT","{0:n0}")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--在職年資起--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_work_years_sdt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lb_WORK_YEARS_SDT" runat="server" Text='<%#Bind("WORK_YEARS_SDT")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--在職年資迄--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_work_years_edt%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lb_WORK_YEARS_EDT" runat="server" Text='<%#Bind("WORK_YEARS_EDT")%>' Width="100px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--員工區分--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sg_lb_prid_cd%>" HeaderStyle-Width="260px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_PRID_CD" runat="server" Text='<%#Bind("PRID_CD")%>' Width="260px"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="GridviewScrollHeader" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                        <tr height="100%" valign="top">
                            <td class="GridviewScrollPager TD">
                                <asp:DropDownList ID="ddlPerPageRow2" runat="server" ClientIDMode="Static" AutoPostBack="true" Style="font-size: 14px;">
                                    <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                    <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="font-size: 14px;">
                                <asp:Label ID="lb_TotalCount2" runat="server" Text="" Width="200px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <!-- 頁籤 結束-------------------  -->

            <!-- SG040_Qry的查詢條件 -->
            <asp:HiddenField ID="HID_Qry_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Qry_MP_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Qry_FESTIVAL_DT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Qry_FESTIVAL_PAY_DT" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="HID_FREEZE_FLAG" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_APPROVE_STATUS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FESTIVAL_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_RELEASE_DT" runat="server" ClientIDMode="Static" />

            <!-- -->


            <!-- 是否為supervisor  -->
            <asp:HiddenField ID="HID_IS_SUPERVISOR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SG0401ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
